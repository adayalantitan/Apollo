#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class admin_ExceptionViewer : System.Web.UI.Page
    {

        #region flushLog_Click method
        /// <summary>Purges information from the Exception Log</summary>
        /// <param name="sender">Object firing the event</param>
        /// <param name="e">EventArgs</param>
        protected void flushLog_Click(object sender, EventArgs e)
        {
            try
            {
                DataIO.ExecuteActionQuery(DataIO.GetCommandFromStoredProc("App_DeleteExceptionLog"));
                labelStatus.Text = "The Exception Log has been purged.<br/><br/>";
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                labelStatus.Text = "An error occurred while trying to purge the Exception Log.<br/><br/>";
            }
            finally
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ReloadGrid", "setTimeout('RefreshGrid()',200);", true);                
            }
        }
        #endregion

        #region Page_Load method
        /// <summary>Method fired when the Page is loaded</summary>
        /// <param name="sender">Object firing the event</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion

    }

}
