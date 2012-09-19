using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class admin_SupportDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region flushLog_Click method
        /// <summary>Purges information from the Exception Log</summary>
        /// <param name="sender">Object firing the event</param>
        /// <param name="e">EventArgs</param>
        protected void flushLog_Click(object sender, EventArgs e)
        {
            try
            {
                DataIO.ExecuteActionQuery(DataIO.GetCommandFromStoredProc("App_DeleteStatusLog"));
                labelStatus.Text = "The Status Log has been purged.<br/><br/>";
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                labelStatus.Text = "An error occurred while trying to purge the Status Log.<br/><br/>";
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ReloadGrid", "setTimeout('RefreshGrid()',200);", true);
            }
        }
        #endregion
    }
}