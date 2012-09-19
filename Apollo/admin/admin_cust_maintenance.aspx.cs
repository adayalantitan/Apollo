﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

namespace Apollo
{
    public partial class admin_admin_cust_maintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region excelExport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void excelExport_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WebCommon.WriteDebugMessage(string.Format("Customer Rollup List Exported by: {0}", Security.GetCurrentUserId));
                Hashtable spParams = new Hashtable();
                DataSet customerData = DataIO.ExecuteDataSetQuery(DataIO.GetCommandFromStoredProc("MASTERDATA_GETCUSTOMERROLLUP", spParams));
                WebCommon.ExportHtmlToExcel("CustomerRollupList", WebCommon.DataTableToHtmlTable(customerData.Tables[0], "Customer Rollup"));
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