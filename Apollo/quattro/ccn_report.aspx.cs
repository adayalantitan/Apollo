using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using Titan.DataIO;
namespace Apollo
{
    public partial class quattro_ccn_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                canEdit.Value = (Security.IsContractAdminTeam() ? "-1" : "0");
            }
        }

        #region excelExport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void excelExport_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WebCommon.WriteDebugMessage(string.Format("Quattro-CCNs Report Exported by: {0}", Security.GetCurrentUserId));
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("QUATTRO_GETCONTRACTCCNS",
                        Param.CreateParam("SORTBY", SqlDbType.VarChar, "ATTACHMENT_DATE"),
                        Param.CreateParam("SORTDIR", SqlDbType.VarChar, "DESC"),
                        Param.CreateParam("ATTACHMENTDATEFROM", SqlDbType.Date, new DateTime(2002, 1, 1)),
                        Param.CreateParam("ATTACHMENTDATETO", SqlDbType.Date, DateTime.Now)));
                }
                WebCommon.ExportHtmlToExcel("QuattroCCNReport", WebCommon.DataTableToHtmlTable(ds.Tables[0], "Quattro CCN Report"));
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
        }
        #endregion

    }
}