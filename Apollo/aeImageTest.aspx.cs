#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class aeImageTest : System.Web.UI.Page
    {

        #region GetAEImageList method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <returns>TBD</returns>
        private string GetAEImageList(string aeId)
        {
            //Hashtable values = new Hashtable();
            //values["AEID"] = aeId;            
            return DigitalLibraryService.ExecuteAEDashboardSearch(aeId);
        }
        #endregion

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool isSalesCoordinator = Security.IsSalesCoordinator();
                try
                {
                    string aeId = (string)Request.QueryString["aeId"] ?? "";
                    if (String.IsNullOrEmpty(aeId))
                    {
                        return;
                    }
                    string aeImageList = GetAEImageList(aeId);
                    if (String.IsNullOrEmpty(aeImageList))
                    {
                        aePhotoGallery.InnerHtml = "";
                        aePhotoGallery.Style["display"] = "none";
                        return;
                    }
                    aePhotoGallery.InnerHtml = string.Format("Below are the Photos that have been uploaded for your Contracts in the past week:<br/><br/>{0}", aeImageList);
                    aePhotoGallery.Style["display"] = "block";
                }
                catch (Exception ex)
                {
                    WebCommon.LogExceptionInfo(ex);
                    aePhotoGallery.InnerHtml = "";
                    aePhotoGallery.Style["display"] = "none";
                }
            }
        }
        #endregion

    }

}
