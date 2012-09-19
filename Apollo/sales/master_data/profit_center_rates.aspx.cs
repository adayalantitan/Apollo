#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class sales_master_data_profit_center_rates : System.Web.UI.Page
    {

        #region AddProfitCenterRateXrefRecord method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool AddProfitCenterRateXrefRecord(Hashtable values)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_ADDRATEPROFITCENTERXREFRECORD",
                        Param.CreateParam("PROFITCENTERID", SqlDbType.Int, values["PROFITCENTERID"]),
                        Param.CreateParam("RATEID", SqlDbType.Int, values["RATEID"]),
                        Param.CreateParam("EFFECTIVEDATE", SqlDbType.Date, values["EFFECTIVEDATE"])));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
            return true;
        }
        #endregion

        #region CheckExistingEffectiveDate method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool CheckExistingEffectiveDate(Hashtable values)
        {
            int count;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                count = (int)io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("MASTERDATA_CHECKEXISTINGPROFITCENTERRATEEFFECTIVEDATE",
                    Param.CreateParam("PROFITCENTERID", SqlDbType.Int, values["PROFITCENTERID"]),
                    Param.CreateParam("EFFECTIVEDATE", SqlDbType.Date, values["EFFECTIVEDATE"])));
            }
            return (count > 0);
        }
        #endregion

        #region DeleteProfitCenterRateXrefRecord method
        /// <summary>TBD</summary>
        /// <param name="rateProfitCenterXrefId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool DeleteProfitCenterRateXrefRecord(int rateProfitCenterXrefId)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_DELETERATEPROFITCENTERXREFRECORD", Param.CreateParam("RATEPROFITCENTERXREFID", SqlDbType.Int, rateProfitCenterXrefId)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
            return true;
        }
        #endregion

        #region GetRateHistory method
        /// <summary>TBD</summary>
        /// <param name="profitCenterId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string GetRateHistory(int profitCenterId)
        {
            StringBuilder tableBuilder = new StringBuilder();
            DataSet historyData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                historyData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GETRATEDETAILHISTORYBYPROFITCENTERID", Param.CreateParam("PROFITCENTERID", SqlDbType.Int, profitCenterId)));
            }
            if (historyData.Tables[0].Rows.Count == 0)
            {
                return string.Empty;
            }
            //Record fields            
            int rateProfitCenterXrefId, rateId;
            string effectiveDate, rateName;            
            tableBuilder.AppendLine(@"<table width=""100%"" cellpadding=""1"" cellspacing=""1"" border=""0"" style=""color:#2f4070;"">");
            foreach (DataRow historyRow in historyData.Tables[0].Rows)
            {
                //Populate values
                rateProfitCenterXrefId = (int)IO.GetDataRowValue(historyRow, "RATE_PROFIT_CENTER_XREF_ID", 0);
                rateName = (string)IO.GetDataRowValue(historyRow, "RATE_NAME", "&nbsp;");
                rateId = (int)IO.GetDataRowValue(historyRow, "RATE_ID", 0);
                effectiveDate = (string)IO.GetDataRowValue(historyRow, "EFFECTIVE_DATE", "&nbsp;");         
                tableBuilder.AppendLine(@"<tr bgcolor=""#ffffcc"" style=""cursor:pointer;"">");                
                tableBuilder.AppendLine(@"<td width=""10%"" valign=""middle"" align=""center""><img alt=""Delete"" src=""/Images/icon_delete.gif"" onclick=""DeleteRateDetailRecord(" + rateProfitCenterXrefId + @");""></td>");
                //Type
                tableBuilder.AppendLine(@"<td width=""20%"" valign=""top"" align=""center"" style=""padding-left:2px;"">" + effectiveDate + "</td>");
                //Rate Name
                tableBuilder.AppendLine(@"<td width=""50%"" valign=""top"" align=""center"" style=""padding-left:2px;"">" + rateName + "</td>");
                tableBuilder.AppendLine(@"<td width=""20%"" valign=""top"" align=""center"" style=""padding-left:2px;""><a href=""#"" onclick=""ShowRateDetail(" + rateId + ",'" + rateName + "','" + effectiveDate + @"');"">See Rates</a></td>");
                tableBuilder.AppendLine(@"</tr>");
            }
            tableBuilder.AppendLine(@"</table>");
            return tableBuilder.ToString();
        }
        #endregion

        #region Page_Init method
        /// <summary>TBD</summary>
        protected void Page_Init()
        {
            if (!Security.IsAdminUser() && !Security.IsCorporateUser())
            {
                WebCommon.ShowAlert("You are not authorized to use this portion of the application.");
                Server.Transfer("/Default.aspx");
            }                        
            rateNameDataSource.ConnectionString = WebCommon.ConnectionString;
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

        #region LoadRateDetails method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string LoadRateDetails(Hashtable values)
        {            
            StringBuilder tableBuilder = new StringBuilder();
            int counter = 0;
            DataSet rateInfo;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                rateInfo = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GETRATESBYRATEID", Param.CreateParam("RATEID", SqlDbType.Int, values["RATEID"])));
            }
            tableBuilder.AppendLine(@"<table cellspacing=""1"" cellpadding=""1"" width=""100%"" align=""center"" id=""detailsTable"" runat=""server"">");
            tableBuilder.AppendLine(@"<tr><td nowrap class=""buttonHead_Right"" width=""20%"">Local Renew %</td>");
            tableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""20%"">Local New %</td>");
            tableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""20%"">National Renew %</td>");
            tableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""20%"">National New %</td>");
            tableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Center"" width=""20%"">Effective From</td></tr>");
            foreach (DataRow row in rateInfo.Tables[0].Rows)
            {
                tableBuilder.AppendLine(@"<tr class=""infoBoxSearch""" + ((counter++ % 2 == 0) ? "0" : "1") + @">");
                tableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(row, "RATE_LOCAL_RENEW", "&nbsp;") + "</td>");
                tableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(row, "RATE_LOCAL_NEW", "&nbsp;") + "</td>");
                tableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(row, "RATE_NATIONAL_RENEW", "&nbsp;") + "</td>");
                tableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(row, "RATE_NATIONAL_NEW", "&nbsp;") + "</td>");
                tableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(row, "RATE_EFFECTIVE_DATE", "&nbsp;") + "</td>");
                tableBuilder.AppendLine(@"</tr>");
            }
            tableBuilder.AppendLine(@"</table>");
            return tableBuilder.ToString();
        }
        #endregion
    }
}
