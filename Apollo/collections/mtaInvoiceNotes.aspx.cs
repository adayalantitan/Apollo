#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class collections_mtaInvoiceNotes : System.Web.UI.Page
    {

        #region excelExport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void excelExport_Click(object sender, ImageClickEventArgs e)
        {
            Hashtable spParams = new Hashtable();
            string reportName = "MTA Invoices w/Payments and Notes";
            try
            {
                DataSet reportData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    reportData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Collections_GetMTAInvoiceNotesGrid"));
                }
                WebCommon.WriteDebugMessage(string.Format("MTA Invoice Report Exported by: {0}", Security.GetCurrentUserId));
                WebCommon.ExportHtmlToExcel("MTAInvoiceReport", WebCommon.DataTableToHtmlTable(reportData.Tables[0], reportName));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
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
