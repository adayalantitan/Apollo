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
    public partial class _Default : System.Web.UI.Page
    {

        #region GetAEImageList method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string GetAEImageList()
        {
            return DigitalLibraryService.ExecuteAEDashboardSearch(Security.UserAEId);
        }
        #endregion

        #region Page_Load method
        /// <summary>Event fired when the page is first loaded</summary>
        /// <param name="sender">Object firing the event</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ntId.Value = Security.GetCurrentUserId;
                /*
                try
                {
                    aeLookup.Style["display"] = (Security.IsAdminUser() || Security.IsSalesCoordinator() || Security.IsDigitalUser() || Security.IsMarketingUser()) ? "block" : "none";
                    if (String.IsNullOrEmpty(Security.UserAEId))
                    {
                        return;
                    }                    
                    string aeImageList = GetAEImageList();
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
                 */
            }
        }
        #endregion

    }

}
