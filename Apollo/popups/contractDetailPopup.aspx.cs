using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Titan.DataIO;

namespace Apollo
{
    public partial class popups_contractDetailPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int contractNumber = -1;
            int companyId = -1;
            if (!int.TryParse(Request.QueryString["contractNumber"], out contractNumber))
            {
                Response.Write("Could not load contract details.");
                Response.Flush();
                return;
            }
            if (!int.TryParse(Request.QueryString["companyId"], out companyId))
            {
                companyId = 1;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowContractDetail", string.Format("ShowContractDetail({0},{1});", contractNumber, companyId), true);
        }

        #region GetContractDetail method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod()]
        public static Hashtable GetContractDetail(Hashtable values)
        {
            Hashtable contractDetailValues = new Hashtable();
            DataSet contractDetailData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                contractDetailData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("",
                    Param.CreateParam("contractNumber", SqlDbType.Int, Convert.ToInt32(values["CONTRACTNUMBER"])),
                    Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(values["COMPANYID"]))));
            }
            contractDetailValues["CONTRACT_NUMBER"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTRACT_NUMBER", "");
            contractDetailValues["CONTRACT_ENTRY_DATE"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTRACT_ENTRY_DATE", "");
            contractDetailValues["CONTRACT_START_DATE"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTRACT_START_DATE", "");
            contractDetailValues["AGENCY"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AGENCY", "");
            contractDetailValues["CONTACT_NAME"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTACT_NAME", "");
            contractDetailValues["AGENCY_FEE"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AGENCY_FEE", "");
            contractDetailValues["ADVERTISER"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "ADVERTISER", "");
            contractDetailValues["PROGRAM"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "PROGRAM", "");
            contractDetailValues["AGENCY_PO_NUMBER"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AGENCY_PO_NUMBER", "");
            contractDetailValues["AE1_NAME"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AE1_NAME", "");
            contractDetailValues["AE2_NAME"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AE2_NAME", "");
            contractDetailValues["AE3_NAME"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AE3_NAME", "");
            contractDetailValues["PRODUCT_CLASS_DESCRIPTION"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "PRODUCT_CLASS_DESCRIPTION", "");
            contractDetailValues["LOCAL_OR_NATIONAL"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "LOCAL_OR_NATIONAL", "");
            contractDetailValues["HAS_ATTACHMENTS"] = IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "HAS_ATTACHMENTS", "0");
            contractDetailValues["LINE_ITEM_TABLE"] = OnlineFlashReport.BuildDetailLineItemTable(contractDetailData);
            contractDetailValues["TRANSACTIONS_TABLE"] = OnlineFlashReport.BuildDetailTransactionsTable(contractDetailData);
            return contractDetailValues;
        }
        #endregion
    }
}