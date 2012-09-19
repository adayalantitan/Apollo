#region Using Statements
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class quattro_uninvoiced_contract_breakdown : System.Web.UI.Page
    {

        #region excelExport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void excelExport_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WebCommon.WriteDebugMessage(string.Format("Quattro-Uninvoiced Contract Breakdown Exported by: {0}", Security.GetCurrentUserId));
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                if (String.Compare("Approved For Invoicing", sortIndex.Value, true) == 0)
                {
                    spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, "Approval Id"));
                }
                else
                {
                    spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sortIndex.Value));
                }                
                spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sortOrder.Value));
                if (!String.IsNullOrEmpty(enteredBy.Value))
                {
                    spParams.Add(Param.CreateParam("enteredBy", SqlDbType.VarChar, enteredBy.Value));
                }
                if (!String.IsNullOrEmpty(contractNumber.Value))
                {
                    spParams.Add(Param.CreateParam("contractNumber", SqlDbType.VarChar, contractNumber.Value));
                }
                if (!String.IsNullOrEmpty(campaignType.Value))
                {
                    spParams.Add(Param.CreateParam("campaignType", SqlDbType.VarChar, campaignType.Value));
                }
                if (!String.IsNullOrEmpty(customer.Value))
                {
                    spParams.Add(Param.CreateParam("customer", SqlDbType.VarChar, customer.Value));
                }
                if (!String.IsNullOrEmpty(numCmpgnSegments.Value))
                {
                    spParams.Add(Param.CreateParam("numCmpgnSegments", SqlDbType.VarChar, numCmpgnSegments.Value));
                }
                if (!String.IsNullOrEmpty(numCmpgnSegments.Value))
                {
                    spParams.Add(Param.CreateParam("numBarterSegments", SqlDbType.VarChar, numBarterSegments.Value));
                }
                if (!String.IsNullOrEmpty(market.Value))
                {
                    spParams.Add(Param.CreateParam("market", SqlDbType.VarChar, market.Value));
                }
                if (!String.IsNullOrEmpty(isMissingSplit.Value))
                {
                    spParams.Add(Param.CreateParam("isMissingSplit", SqlDbType.VarChar, isMissingSplit.Value));
                }
                if (!String.IsNullOrEmpty(isBanner.Value))
                {
                    spParams.Add(Param.CreateParam("isBanner", SqlDbType.VarChar, isBanner.Value));
                }
                if (!String.IsNullOrEmpty(companyId.Value))
                {
                    spParams.Add(Param.CreateParam("companyid", SqlDbType.Int, Convert.ToInt32(companyId.Value)));
                }
                spParams.Add(Param.CreateParam("forExcel", SqlDbType.Int, -1));
                DataSet quattroData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    quattroData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetUninvoicedContractBreakdown", spParams));
                }
                WebCommon.ExportHtmlToExcel("QuattroUninvoicedContractBreakdown", WebCommon.DataTableToHtmlTable(quattroData.Tables[0], "Contracts not setup for Invoicing"));
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

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebCommon.WriteDebugMessage(string.Format("Quattro-Uninvoiced Contract Breakdown Requested by: {0}", Security.GetCurrentUserId));
                isApprover.Value = ((Security.GetCurrentUserId == "TSMITH" || Security.GetCurrentUserId == "SSHAFIROVICH") ? "-1" : "0");
            }
        }
        #endregion

    }

}
