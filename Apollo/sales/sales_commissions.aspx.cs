#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

    /// <summary>sales_commissions.aspx code-behind</summary>
    public partial class sales_sales_commissions : System.Web.UI.Page
    {
        public const string SCREEN_NAME = "sales_commissions";

        #region AddNotes method
        /// <summary>Static method to add a note to the contract</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool AddNotes(string contractNumber, string companyId, string noteText)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("ADDNOTE",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(contractNumber)),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)),
                        Param.CreateParam("NOTETEXT", SqlDbType.NText, noteText),
                        Param.CreateParam("USERID", SqlDbType.VarChar, Security.GetCurrentUserId))); ;
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
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    return ((int)io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_CHECKEXISTINGCONTRACTCOMMISSIONEFFECTIVEDATE",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, values["CONTRACTNUMBER"]),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, values["COMPANYID"]),
                        Param.CreateParam("EFFECTIVEDATE", SqlDbType.Date, values["EFFECTIVEDATE"]))) > 0);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while checking for an existing effective date.");
            }
        }
        #endregion

        #region ContractNoteRow property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        private static string ContractNoteRow
        {
            get
            {
                StringBuilder noteRow = new StringBuilder();
                noteRow.AppendLine(@"<tr>");
                noteRow.AppendLine(@"   <td valign=""top"" align=""center"" nowrap width=""15%"" style=""padding:3px;"">{0}</td>");
                noteRow.AppendLine(@"   <td valign=""top"" align=""left"" width=""72%"" style=""padding:3px;"">{1}</td>");
                noteRow.AppendLine(@"   <td valign=""top"" align=""center"" nowrap width=""13%"" style=""padding:3px;"">{2}</td>");
                noteRow.AppendLine(@"</tr>");
                return noteRow.ToString();
            }
        }
        #endregion

        #region DeleteHistoryItem method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool DeleteHistoryItem(Hashtable values)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_DeleteContractDetailHistoryItem",
                        Param.CreateParam("COMMISSIONID", SqlDbType.Int, values["COMMISSIONID"]),
                        Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, values["ENTEREDBY"])));
                }
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to delete the history item.");
            }            
        }
        #endregion

        #region GetNullableField method
        /// <summary>TBD</summary>
        /// <param name="fieldValue">TBD</param>
        /// <returns>TBD</returns>
        private static object GetNullableField(object fieldValue)
        {
            return (!String.IsNullOrEmpty(Convert.ToString(fieldValue))) ? fieldValue : DBNull.Value;
        }
        #endregion

        #region GetNullableStringParamValue method
        /// <summary>TBD</summary>
        /// <param name="value">TBD</param>
        /// <returns>TBD</returns>
        private object GetNullableStringParamValue(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }
            return value;
        }
        #endregion

        #region LoadContractDetail method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static Hashtable LoadContractDetail(Hashtable values)
        {
            try
            {
                //TODO: Put into a Service Method
                bool isInitialView = ((int)values["COMMISSIONID"] == -1);
                Hashtable contractDetail = new Hashtable();
                DataRow contractDetailData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    contractDetailData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_GetContractDetail",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(values["CONTRACTNUMBER"])),
                        Param.CreateParam("COMMISSIONID", SqlDbType.Int, Convert.ToInt32(values["COMMISSIONID"])),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(values["COMPANYID"])))).Tables[0].Rows[0];
                }
                contractDetail["CONTRACT_NUMBER"] = IO.GetDataRowValue(contractDetailData, "CONTRACT_NUMBER", -1);
                contractDetail["TOTALPERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "TOTALPERCENTAGE", "");
                contractDetail["COMMISSION_ID"] = (isInitialView) ? -1 : IO.GetDataRowValue(contractDetailData, "COMMISSION_ID", -1);
                contractDetail["ENTRYDATE"] = IO.GetDataRowValue(contractDetailData, "ENTRYDATE", "");
                contractDetail["ADVERTISER"] = IO.GetDataRowValue(contractDetailData, "ADVERTISER", "");
                contractDetail["AGENCY"] = IO.GetDataRowValue(contractDetailData, "AGENCY", "");
                contractDetail["PROGRAM"] = IO.GetDataRowValue(contractDetailData, "PROGRAM", "");
                contractDetail["TOTALCONTRACT"] = IO.GetDataRowValue(contractDetailData, "TOTALCONTRACT", "");
                contractDetail["PACKAGE_ID"] = IO.GetDataRowValue(contractDetailData, "PACKAGE_ID", "");
                contractDetail["AE_SPLIT_EFFECTIVE_FROM_DATE"] = (isInitialView) ? "" : IO.GetDataRowValue(contractDetailData, "AE_SPLIT_EFFECTIVE_FROM_DATE", "");
                contractDetail["AE_1_NAME"] = IO.GetDataRowValue(contractDetailData, "AE_1_NAME", "");
                contractDetail["AE_1_ID"] = IO.GetDataRowValue(contractDetailData, "AE_1_ID", "");
                contractDetail["AE_1_REVENUE_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_1_REV_PERCENTAGE", "");
                contractDetail["AE_1_COMMISSION_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_1_COMM_PERCENTAGE", "");
                contractDetail["AE_1_FIXED_POINTS_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_1_POINTS_NEW", "");
                contractDetail["AE_2_NAME"] = IO.GetDataRowValue(contractDetailData, "AE_2_NAME", "");
                contractDetail["AE_2_ID"] = IO.GetDataRowValue(contractDetailData, "AE_2_ID", "");
                contractDetail["AE_2_REVENUE_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_2_REV_PERCENTAGE", "");
                contractDetail["AE_2_COMMISSION_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_2_COMM_PERCENTAGE", "");
                contractDetail["AE_2_FIXED_POINTS_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_2_POINTS_NEW", "");
                contractDetail["AE_3_NAME"] = IO.GetDataRowValue(contractDetailData, "AE_3_NAME", "");
                contractDetail["AE_3_ID"] = IO.GetDataRowValue(contractDetailData, "AE_3_ID", "");
                contractDetail["AE_3_REVENUE_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_3_REV_PERCENTAGE", "");
                contractDetail["AE_3_COMMISSION_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_3_COMM_PERCENTAGE", "");
                contractDetail["AE_3_FIXED_POINTS_PERCENTAGE"] = IO.GetDataRowValue(contractDetailData, "AE_3_POINTS_NEW", "");
                contractDetail["APPROVED_BY_ID"] = (isInitialView) ? "" : IO.GetDataRowValue(contractDetailData, "APPROVED_BY_ID", "");
                contractDetail["APPROVED_BY_DATE"] = (isInitialView) ? "" : IO.GetDataRowValue(contractDetailData, "APPROVED_BY_DATE", "");
                contractDetail["USE_POINTS"] = IO.GetDataRowValue(contractDetailData, "USE_POINTS", "");
                contractDetail["COMPANYID"] = values["COMPANYID"];
                return contractDetail;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("An error occurred while trying to details for Contract: {0}", values["CONTRACT_NUMBER"]), ex));
                throw new Exception("An error occurred while trying to retrieve the Contract details.");
            }
        }
        #endregion

        #region LoadContractDetailHistory method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string LoadContractDetailHistory(Hashtable values)
        {
            try
            {
                StringBuilder historyTable = new StringBuilder();
                DataSet historyData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    historyData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_GETCONTRACTDETAILHISTORY",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(values["CONTRACTNUMBER"])),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(values["COMPANYID"]))));
                }
                bool allowDelete = (historyData.Tables[0].Rows.Count > 0);
                int count = 0;
                historyTable.AppendLine(@"<table width=""100%"">");
                int commissionId, companyId;
                string ae1Name, ae2Name, ae3Name, package, effectiveFrom, source;
                foreach (DataRow row in historyData.Tables[0].Rows)
                {
                    //Can't delete the last record
                    allowDelete = (count != historyData.Tables[0].Rows.Count - 1);
                    commissionId = (int)IO.GetDataRowValue(row, "COMMISSION_ID", -1);
                    companyId = Convert.ToInt32(values["COMPANYID"]);
                    source = (string)IO.GetDataRowValue(row, "SOURCE", "");
                    effectiveFrom = (string)IO.GetDataRowValue(row, "AE_SPLIT_EFFECTIVE_FROM_DATE", "&nbsp;");
                    ae1Name = (string)IO.GetDataRowValue(row, "AE_1_NAME", "&nbsp;");
                    ae2Name = (string)IO.GetDataRowValue(row, "AE_2_NAME", "&nbsp;");
                    ae3Name = (string)IO.GetDataRowValue(row, "AE_3_NAME", "&nbsp;");
                    package = (string)IO.GetDataRowValue(row, "PACKAGE", "&nbsp;");
                    //need commissionId, source, companyId
                    historyTable.AppendFormat(@"<tr id=""historyRow{0}"">", count);
                    historyTable.AppendLine(@"<td align=""center"" width=""5%"">" + ((!allowDelete) ? "&nbsp;" : @"<img src=""/Images/icon_delete.gif"" alt=""Delete"" style=""cursor:pointer"" onclick=""DeleteContractDetailHistoryItem(" + values["CONTRACTNUMBER"] + "," + companyId + "," + commissionId + ",'" + source + @"');"" />") + "</td>");
                    historyTable.AppendLine(@"<td align=""center"" width=""5%""><img src=""/Images/icon_edit.gif"" style=""cursor:pointer"" alt=""Edit"" onclick=""EditContractDetailHistoryItem(" + values["CONTRACTNUMBER"] + "," + companyId + "," + commissionId + "," + count + ",'" + ((allowDelete) ? "0" : "1") + @"');"" /></td>");
                    historyTable.AppendLine(@"<td align=""center"" width=""15%"">" + effectiveFrom + "</td>");
                    historyTable.AppendLine(@"<td align=""center"" width=""15%"">" + ae1Name + "</td>");
                    historyTable.AppendLine(@"<td align=""center"" width=""15%"">" + ae2Name + "</td>");
                    historyTable.AppendLine(@"<td align=""center"" width=""15%"">" + ae3Name + "</td>");
                    historyTable.AppendLine(@"<td align=""center"" width=""15%"">" + package + "</td>");
                    if (commissionId == -1)
                    {
                        historyTable.AppendLine(@"<td align=""center"" width=""15%"">&nbsp;</td>");
                    }
                    else
                    {
                        historyTable.AppendLine(@"<td align=""center"" width=""15%""><a href=""#"" onclick=""ShowContractDetailHistoryItem(" + values["CONTRACTNUMBER"] + "," + companyId + "," + commissionId + ",'" + source + "','" + effectiveFrom + @"');"">Details</a></td>");
                    }
                    historyTable.AppendLine("</tr>");
                    count++;
                }
                historyTable.AppendLine("</table>");
                return historyTable.ToString();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to load the Contract history.");
            }
        }
        #endregion

        #region LoadContractDetailHistoryItem method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string LoadContractDetailHistoryItem(Hashtable values)
        {
            try
            {
                StringBuilder detailsTableBuilder = new StringBuilder();
                DataRow historyItem;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    historyItem = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_GetContractDetailHistoryItem",
                        Param.CreateParam("COMMISSIONID", SqlDbType.Int, Convert.ToInt32(values["COMMISSIONID"])),
                        Param.CreateParam("SOURCE", SqlDbType.VarChar, Convert.ToString(values["SOURCE"])),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(values["COMPANYID"])))).Tables[0].Rows[0];
                }
                detailsTableBuilder.AppendLine(@"<table cellspacing=""1"" cellpadding=""1"" width=""100%"" align=""center"" id=""detailsTable"" runat=""server"">");
                detailsTableBuilder.AppendLine(@"<tr><td nowrap class=""buttonHead_Right"" width=""15%""></td>");
                detailsTableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""15%"">AE</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""10%"">Revenue Split %</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""10%"">Commission Split %</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""15%"">Package</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Right"" width=""15%"">Fixed Points</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap class=""buttonHead_Center"" width=""20%"">Effective From</td></tr>");
                detailsTableBuilder.AppendLine(@"<tr class=""infoBoxSearch0"">");
                detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">AE #1</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_1_NAME", "&nbsp;") + "</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_1_REVENUE_PERCENTAGE", "&nbsp;") + "</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_1_COMMISSION_PERCENTAGE", "&nbsp;") + "</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "PACKAGE", "&nbsp;") + "</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_1_FIXED_POINTS_PERCENTAGE", "&nbsp;") + "</td>");
                detailsTableBuilder.AppendLine(@"<td nowrap align=""center"">" + IO.GetDataRowValue(historyItem, "AE_SPLIT_EFFECTIVE_FROM_DATE", "&nbsp;") + "</td>");
                detailsTableBuilder.AppendLine(@"</tr>");
                if ((string)IO.GetDataRowValue(historyItem, "AE_2_NAME", "") != "")
                {
                    detailsTableBuilder.AppendLine(@"<tr class=""infoBoxSearch1"">");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">AE #2</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_2_NAME", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_2_REVENUE_PERCENTAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_2_COMMISSION_PERCENTAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "PACKAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_2_FIXED_POINTS_PERCENTAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""center"">" + IO.GetDataRowValue(historyItem, "AE_SPLIT_EFFECTIVE_FROM_DATE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"</tr>");
                }
                if ((string)IO.GetDataRowValue(historyItem, "AE_3_NAME", "") != "")
                {
                    detailsTableBuilder.AppendLine(@"<tr class=""infoBoxSearch0"">");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">AE #3</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_3_NAME", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_3_REVENUE_PERCENTAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_3_COMMISSION_PERCENTAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "PACKAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""right"">" + IO.GetDataRowValue(historyItem, "AE_3_FIXED_POINTS_PERCENTAGE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"<td nowrap align=""center"">" + IO.GetDataRowValue(historyItem, "AE_SPLIT_EFFECTIVE_FROM_DATE", "&nbsp;") + "</td>");
                    detailsTableBuilder.AppendLine(@"</tr>");
                }
                detailsTableBuilder.AppendLine(@"</table>");
                return detailsTableBuilder.ToString();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to load the Contract History Detail.");
            }
        }
        #endregion

        #region LoadNoteData method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string LoadNoteData(string contractNumber, string companyId)
        {
            try
            {
                StringBuilder noteTable = new StringBuilder();
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("GETNOTES",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(contractNumber)),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId))));
                }
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return "";
                }
                noteTable.AppendLine(@"<table width=""100%"">");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    noteTable.AppendLine(
                        string.Format(ContractNoteRow,
                            IO.GetDataRowValue(row, "NOTE_DATE", "&nbsp;"),
                            HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "NOTE_TEXT", "&nbsp;")),
                            Security.GetFullUserNameFromId((string)IO.GetDataRowValue(row, "USER_ID", "")))
                    );
                }
                noteTable.AppendLine("</table>");
                return noteTable.ToString();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return string.Empty;
            }
        }
        #endregion

        #region Page_Error method
        /// <summary>TBD</summary>
        protected void Page_Error()
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            Server.ClearError();
            WebCommon.LogExceptionInfo(ex);
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

        #region Page_Init method
        /// <summary>TBD</summary>
        protected void Page_Init()
        {
            if (!Security.IsAdminUser() && !Security.IsCorporateUser())
            {
                WebCommon.ShowAlert("You are not authorized to use this portion of the application.");                
                Server.Transfer("/Default.aspx");
            }            
        }
        #endregion

        #region SaveContractData method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool SaveContractData(Hashtable values)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<SqlParameter>();
            spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(values["CONTRACTNUMBER"])));
            spParams.Add(Param.CreateParam("EFFECTIVEDATE", SqlDbType.DateTime, GetNullableField(values["EFFECTIVEDATE"])));
            spParams.Add(Param.CreateParam("PACKAGEID", SqlDbType.Int, GetNullableField(values["PACKAGE"])));
            spParams.Add(Param.CreateParam("AE1ID", SqlDbType.VarChar, GetNullableField(values["AE1ID"])));
            spParams.Add(Param.CreateParam("AE1REVPERCENTAGE", SqlDbType.Decimal, GetNullableField(values["AE1REV"])));
            spParams.Add(Param.CreateParam("AE1COMPERCENTAGE", SqlDbType.Decimal, GetNullableField(values["AE1COM"])));
            spParams.Add(Param.CreateParam("AE1POINTS", SqlDbType.Decimal, GetNullableField(values["AE1FIXED"])));
            spParams.Add(Param.CreateParam("AE2ID", SqlDbType.VarChar, GetNullableField(values["AE2ID"])));
            spParams.Add(Param.CreateParam("AE2REVPERCENTAGE", SqlDbType.Decimal, GetNullableField(values["AE2REV"])));
            spParams.Add(Param.CreateParam("AE2COMPERCENTAGE", SqlDbType.Decimal, GetNullableField(values["AE2COM"])));
            spParams.Add(Param.CreateParam("AE2POINTS", SqlDbType.Decimal, GetNullableField(values["AE2FIXED"])));
            spParams.Add(Param.CreateParam("AE3ID", SqlDbType.VarChar, GetNullableField(values["AE3ID"])));
            spParams.Add(Param.CreateParam("AE3REVPERCENTAGE", SqlDbType.Decimal, GetNullableField(values["AE3REV"])));
            spParams.Add(Param.CreateParam("AE3COMPERCENTAGE", SqlDbType.Decimal, GetNullableField(values["AE3COM"])));
            spParams.Add(Param.CreateParam("AE3POINTS", SqlDbType.Decimal, GetNullableField(values["AE3FIXED"])));
            spParams.Add(Param.CreateParam("USEPOINTS", SqlDbType.VarChar, GetNullableField(values["USEPOINTS"])));
            spParams.Add(Param.CreateParam("APPROVEDBYID", SqlDbType.VarChar, GetNullableField(values["APPROVEDBY"])));
            spParams.Add(Param.CreateParam("APPROVEDBYDATE", SqlDbType.DateTime, GetNullableField(values["APPROVEDBYDATE"])));
            spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(values["COMPANYID"])));
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    if (Convert.ToInt32(values["COMMISSIONID"]) != -1 && (Convert.ToInt32(values["ISUPDATING"]) != 0))
                    {
                        spParams.Add(Param.CreateParam("COMMISSIONID", SqlDbType.Int, Convert.ToInt32(values["COMMISSIONID"])));
                        io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_UpdateCommissionAllocation", spParams));
                    }
                    else
                    {
                        io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_AddCommissionAllocation", spParams));
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to save the contract data.");
            }            
        }
        #endregion

    }

}
