#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class quattro_attachmentDownload : System.Web.UI.Page
    {

        #region downloadPDFs_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void downloadPDFs_Click(object sender, EventArgs e)
        {
            try
            {
                if (attachmentIDs.Value == "")
                {
                    throw new Exception("No attachment IDs were specified.");
                }
                PdfMerge5 pdfMerger = new PdfMerge5();
                string extension = "pdf";
                string fileName = string.Format("{0}_{1}{2}{3}{4}{5}{6}.{7}", "QuattroAttachments", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                if (attachmentIDs.Value.Contains(","))
                {
                    string[] attachmentIdList = attachmentIDs.Value.Split(',');
                    foreach (string attachmentId in attachmentIdList)
                    {
                        pdfMerger.AddDocument(GetAttachment(attachmentId));
                    }
                }
                else
                {
                    pdfMerger.AddDocument(GetAttachment(attachmentIDs.Value));
                }
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                pdfMerger.Merge(Response.OutputStream);
                Response.Flush();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                WebCommon.ShowAlert("An error occurred while trying to download the attachments.");
            }
        }
        #endregion

        #region GetAttachment method
        /// <summary>TBD</summary>
        /// <param name="attachmentId">TBD</param>
        /// <returns>TBD</returns>
        public byte[] GetAttachment(string attachmentId)
        {
            byte[] attachmentBytes = null;
            Uri uri = new Uri(string.Format("http://apollo.titan360.com/quattro/quattro_attachments{0}/{1}.{2}", (Convert.ToInt32(companyId.Value) == 1 ? "" : "_toc"), attachmentId, "PDF"));
            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateDefault(uri);
            request.UseDefaultCredentials = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            int responseLength = Convert.ToInt32(response.ContentLength);
            using (BinaryReader br = new BinaryReader(response.GetResponseStream()))
            {
                attachmentBytes = br.ReadBytes(responseLength);
            }
            return attachmentBytes;
        }
        #endregion

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion

    }

}
