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
    public partial class quattro_uninvoiced_segment_breakdown : System.Web.UI.Page
    {

        #region excelExport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void excelExport_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WebCommon.WriteDebugMessage(string.Format("Quattro-Uninvoiced Segment Breakdown Exported by: {0}", Security.GetCurrentUserId));
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sortIndex.Value));
                spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sortOrder.Value));
                if (!String.IsNullOrEmpty(enteredBy.Value))
                {
                    spParams.Add(Param.CreateParam("enteredBy", SqlDbType.VarChar, enteredBy.Value));
                }
                if (!String.IsNullOrEmpty(contractNumber.Value))
                {
                    spParams.Add(Param.CreateParam("contractNumber", SqlDbType.VarChar, contractNumber.Value));
                }
                if (!String.IsNullOrEmpty(contractStatus.Value))
                {
                    spParams.Add(Param.CreateParam("contractStatus", SqlDbType.VarChar, contractStatus.Value));
                }
                if (!String.IsNullOrEmpty(campaignType.Value))
                {
                    spParams.Add(Param.CreateParam("campaignType", SqlDbType.VarChar, campaignType.Value));
                }
                if (!String.IsNullOrEmpty(customer.Value))
                {
                    spParams.Add(Param.CreateParam("customer", SqlDbType.VarChar, customer.Value));
                }
                if (!String.IsNullOrEmpty(segmentStatus.Value))
                {
                    spParams.Add(Param.CreateParam("segmentStatus", SqlDbType.VarChar, segmentStatus.Value));
                }
                if (!String.IsNullOrEmpty(segmentSalesMarket.Value))
                {
                    spParams.Add(Param.CreateParam("segmentSalesMarket", SqlDbType.VarChar, segmentSalesMarket.Value));
                }
                if (!String.IsNullOrEmpty(segmentProfCtrSplit.Value))
                {
                    spParams.Add(Param.CreateParam("isMissingSplit", SqlDbType.VarChar, segmentProfCtrSplit.Value));
                }
                if (!String.IsNullOrEmpty(mediaType.Value))
                {
                    spParams.Add(Param.CreateParam("mediaType", SqlDbType.VarChar, mediaType.Value));
                }
                if (!String.IsNullOrEmpty(mediaProduct.Value))
                {
                    spParams.Add(Param.CreateParam("mediaProduct", SqlDbType.VarChar, mediaProduct.Value));
                }
                if (!String.IsNullOrEmpty(segmentSpaceRes.Value))
                {
                    spParams.Add(Param.CreateParam("segmentSpaceReserved", SqlDbType.VarChar, segmentSpaceRes.Value));
                }
                if (!String.IsNullOrEmpty(companyId.Value))
                {
                    spParams.Add(Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(companyId.Value)));
                }
                DataSet quattroData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    quattroData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetUninvoicedSegmentInfo", spParams));
                }
                WebCommon.ExportHtmlToExcel("QuattroUninvoicedSegmentBreakdown", WebCommon.DataTableToHtmlTable(quattroData.Tables[0], "Segments not setup for Invoicing"));
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
                WebCommon.WriteDebugMessage(string.Format("Quattro-Uninvoiced Segment Breakdown Requested by: {0}", Security.GetCurrentUserId));
            }
        }
        #endregion

    }

}
