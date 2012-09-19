#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
    public partial class sales_sales_flash : System.Web.UI.Page
    {

        #region Member variables
        /// <summary>TBD</summary>
        private const string SCREEN_NAME = "sales_flash";
        #endregion

        #region clear_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void clear_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExecuteSearch();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
        }
        #endregion

        #region ExecuteSearch method
        /// <summary>TBD</summary>
        public void ExecuteSearch()
        {
            try
            {
                DateTime startDate = Convert.ToDateTime(dropDownStartDateMonth.SelectedValue + "/1/" + dropDownStartDateYear.SelectedValue);
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, startDate));
                spParams.Add(Param.CreateParam("ENTRYDATESTART", SqlDbType.Date, Convert.ToDateTime(textEnteredFrom.Text)));
                spParams.Add(Param.CreateParam("ENTRYDATEEND", SqlDbType.Date, Convert.ToDateTime(textEnteredThru.Text)));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32((String.IsNullOrEmpty(Request.Form[dropDownCompany.UniqueID]) ? "1" : Request.Form[dropDownCompany.UniqueID]))));
                spParams.Add(Param.CreateParam("TRANSITY", SqlDbType.VarChar, (checkMediaTransit.Checked ? "Y" : "")));
                spParams.Add(Param.CreateParam("TRANSITN", SqlDbType.VarChar, (checkMediaNonTransit.Checked ? "N" : "")));
                spParams.Add(Param.CreateParam("SUMFIELDS", SqlDbType.VarChar, GetSPSumFields()));
                spParams.Add(Param.CreateParam("ORDERBY", SqlDbType.VarChar, GetSPOrderBy()));
                if (!String.IsNullOrEmpty(textProgram.Text))
                {
                    spParams.Add(Param.CreateParam("PROGRAM", SqlDbType.VarChar, textProgram.Text));
                }
                if (!String.IsNullOrEmpty(Request.Form[dropDownProductClass.UniqueID]))
                {
                    spParams.Add(Param.CreateParam("PARENTPRODUCTCLASSID", SqlDbType.Int, Convert.ToString(Request.Form[dropDownProductClass.UniqueID])));
                }
                if (!String.IsNullOrEmpty(productClassSearch.Id.Text))
                {
                    spParams.Add(Param.CreateParam("PRODUCTCLASSID", SqlDbType.VarChar, productClassSearch.Id.Text));
                }
                if (!String.IsNullOrEmpty(dropDownCustomerType.SelectedValue))
                {
                    spParams.Add(Param.CreateParam("CUSTOMERCODE", SqlDbType.VarChar, dropDownCustomerType.SelectedValue));
                }
                if (!String.IsNullOrEmpty(textStartingFrom.Text))
                {
                    spParams.Add(Param.CreateParam("CONTRACTFROM", SqlDbType.Date, Convert.ToDateTime(textStartingFrom.Text)));
                }
                if (!String.IsNullOrEmpty(textStartingTo.Text))
                {
                    spParams.Add(Param.CreateParam("CONTRACTTO", SqlDbType.Date, Convert.ToDateTime(textStartingTo.Text)));
                }
                if (!String.IsNullOrEmpty(textEndingFrom.Text))
                {
                    spParams.Add(Param.CreateParam("CONTRACTENDDATEFROM", SqlDbType.Date, Convert.ToDateTime(textEndingFrom.Text)));
                }
                if (!String.IsNullOrEmpty(textEndingTo.Text))
                {
                    spParams.Add(Param.CreateParam("CONTRACTENDDATETO", SqlDbType.Date, Convert.ToDateTime(textEndingFrom.Text)));
                }
                if (!String.IsNullOrEmpty(textBillingFrom.Text))
                {
                    spParams.Add(Param.CreateParam("BILLINGFROM", SqlDbType.Date, Convert.ToDateTime(textBillingFrom.Text)));
                }
                if (!String.IsNullOrEmpty(textBillingTo.Text))
                {
                    spParams.Add(Param.CreateParam("BILLINGTO", SqlDbType.Date, Convert.ToDateTime(textBillingTo.Text)));
                }
                if (!String.IsNullOrEmpty(textInvoiceCreationDateFrom.Text))
                {
                    spParams.Add(Param.CreateParam("INVOICECREATIONDATEFROM", SqlDbType.Date, Convert.ToDateTime(textInvoiceCreationDateFrom.Text)));
                }
                if (!String.IsNullOrEmpty(textInvoiceCreationDateTo.Text))
                {
                    spParams.Add(Param.CreateParam("INVOICECREATIONDATETO", SqlDbType.Date, Convert.ToDateTime(textInvoiceCreationDateTo.Text)));
                }
                if (!String.IsNullOrEmpty(mediaFormSearch.Id.Text))
                {
                    spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, Convert.ToInt32(mediaFormSearch.Id.Text)));
                }
                if (!String.IsNullOrEmpty(Request.Form[dropDownMediaType.UniqueID]))
                {
                    spParams.Add(Param.CreateParam("MEDIATYPEID", SqlDbType.VarChar, Convert.ToString(Request.Form[dropDownMediaType.UniqueID])));
                }
                if (!String.IsNullOrEmpty(Request.Form[dropDownMarket.UniqueID]))
                {
                    spParams.Add(Param.CreateParam("MARKETID", SqlDbType.VarChar, Convert.ToString(Request.Form[dropDownMarket.UniqueID])));
                    if (checkLocal.Checked || checkShipIn.Checked || checkShipOut.Checked)
                    {
                        spParams.Add(Param.CreateParam("LOCALSHIPIN", SqlDbType.Int, 1));
                        if (checkLocal.Checked)
                        {
                            spParams.Add(Param.CreateParam("LOCAL", SqlDbType.VarChar, "LOCAL"));
                        }
                        if (checkShipIn.Checked)
                        {
                            spParams.Add(Param.CreateParam("SHIPIN", SqlDbType.VarChar, "SHIPIN"));
                        }
                        if (checkShipOut.Checked)
                        {
                            spParams.Add(Param.CreateParam("SHIPOUT", SqlDbType.VarChar, "SHIPOUT"));
                        }
                    }
                }
                else
                {
                    if (radioLocal.Checked || radioShipIn.Checked || radioShipOut.Checked)
                    {
                        spParams.Add(Param.CreateParam("LOCALSHIPIN", SqlDbType.Int, 1));
                        if (radioLocal.Checked)
                        {
                            spParams.Add(Param.CreateParam("LOCAL", SqlDbType.VarChar, "LOCAL"));
                        }
                        if (radioShipIn.Checked)
                        {
                            spParams.Add(Param.CreateParam("SHIPIN", SqlDbType.VarChar, "SHIPIN"));
                        }
                        if (radioShipOut.Checked)
                        {
                            spParams.Add(Param.CreateParam("SHIPOUT", SqlDbType.VarChar, "SHIPOUT"));
                        }
                    }
                }
                if (checkExcludeMTA.Checked)
                {
                    spParams.Add(Param.CreateParam("EXCLUDEMTA", SqlDbType.Int, -1));
                }
                if (!String.IsNullOrEmpty(Request.Form[dropDownProfitCenter.UniqueID]))
                {
                    spParams.Add(Param.CreateParam("PROFITCENTERID", SqlDbType.Int, Convert.ToInt32(Request.Form[dropDownProfitCenter.UniqueID])));
                }
                if (!String.IsNullOrEmpty(ae.Id.Text))
                {
                    spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, ae.Id.Text));
                }
                spParams.Add(Param.CreateParam("SHOWCONSOLIDATED", SqlDbType.Int, (radioConsolidated.Checked) ? 1 : 0));
                if (radioConsolidated.Checked)
                {
                    if (!String.IsNullOrEmpty(conAgency.Id.Text))
                    {
                        spParams.Add(Param.CreateParam("AGENCYID", SqlDbType.VarChar, Convert.ToString(conAgency.Id.Text)));
                    }
                    if (!String.IsNullOrEmpty(conAdvertiser.Id.Text))
                    {
                        spParams.Add(Param.CreateParam("ADVERTISERID", SqlDbType.VarChar, Convert.ToString(conAdvertiser.Id.Text)));
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(agency.Id.Text))
                    {
                        spParams.Add(Param.CreateParam("AGENCYID", SqlDbType.VarChar, Convert.ToString(agency.Id.Text)));
                    }
                    if (!String.IsNullOrEmpty(advertiser.Id.Text))
                    {
                        spParams.Add(Param.CreateParam("ADVERTISERID", SqlDbType.VarChar, Convert.ToString(advertiser.Id.Text)));
                    }
                }
                if (!String.IsNullOrEmpty(textContractNumber.Text))
                {
                    spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, Convert.ToInt32(textContractNumber.Text)));
                }
                if (!String.IsNullOrEmpty(textPanelType.Text))
                {
                    DataTable panelTypeTable = WebCommon.GetPanelTypeTable();
                    string[] panelTypes = textPanelType.Text.Split(',');
                    foreach (string panelType in panelTypes)
                    {
                        panelTypeTable.Rows.Add(panelType);
                    }
                    spParams.Add(Param.CreateParam("PANELTYPES", SqlDbType.Structured, panelTypeTable));
                }
                if (!String.IsNullOrEmpty(dropDownRevenue.SelectedValue))
                {
                    spParams.Add(Param.CreateParam("REVENUETYPE", SqlDbType.VarChar, dropDownRevenue.SelectedValue));
                }
                if (!String.IsNullOrEmpty(dropDownNatOrLocal.SelectedValue))
                {
                    spParams.Add(Param.CreateParam("LOCALITY", SqlDbType.VarChar, dropDownNatOrLocal.SelectedValue));
                }
                DataSet flashData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    flashData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("ONLINEFLASH_GETFLASHREPORT", spParams));
                }
                if (flashData == null || flashData.Tables[0].Rows.Count == 0)
                {
                    labelFlashReport.Text = "";
                    labelFlashReport.Text = OnlineFlashReport.GetNoRecordFoundDisplay();
                }
                else
                {
                    labelFlashReport.Text = "";
                    //Store the sorted flashReportResults in the Session object for quick Excel exporting.
                    FlashReportResults = flashData;
                    int minYear = Convert.ToInt32(flashData.Tables[0].Rows[0]["MIN_YEAR"]);
                    int maxYear = Convert.ToInt32(flashData.Tables[0].Rows[0]["MAX_YEAR"]);
                    int[] allYears = GetBillingYears(Convert.ToString(flashData.Tables[0].Rows[0]["ALL_YEARS"]));
                    bool outsideBounds = false;
                    if (startDate.Year < allYears[0])
                    {
                        outsideBounds = true;
                        labelFlashReportMessage.Text = string.Format("The Start Date you selected was before the earliest billing. Start Year has been changed to: {0}", minYear);
                        startDate = startDate.AddYears(minYear - startDate.Year);
                        dropDownStartDateYear.SelectedValue = Convert.ToString(minYear);
                    }
                    else if (startDate.Year > allYears[allYears.Length - 1])
                    {
                        outsideBounds = true;
                        labelFlashReportMessage.Text = string.Format("The Start Date you selected was after the latest billing. Start Year has been changed to: {0}", maxYear);
                        startDate = startDate.AddYears(maxYear - startDate.Year);
                        dropDownStartDateYear.SelectedValue = Convert.ToString(maxYear);
                    }
                    else if (!IsBillingYear(allYears, startDate.Year))
                    {
                        //Get The Closest year
                        for (var i = allYears.Length - 1; i >= 0; i--)
                        {
                            if (startDate.Year > allYears[i])
                            {
                                outsideBounds = true;
                                labelFlashReportMessage.Text = string.Format("The Start Year you selected did not have billing. Start Year has been changed to: {0}", allYears[i]);
                                startDate = startDate.AddYears(allYears[i] - startDate.Year);
                                dropDownStartDateYear.SelectedValue = Convert.ToString(allYears[i]);
                                break;
                            }
                        }
                    }
                    labelFlashReportMessage.Visible = outsideBounds;
                    labelDropDownFlashYear.Visible = true;
                    PopulateFlashReportYearDropDown(allYears, startDate.Year);
                    dropDownFlashYear.Visible = true;
                    string market = (String.IsNullOrEmpty(Request.Form[dropDownMarket.UniqueID]) ? "" : dropDownMarketSelectedText.Value);
                    string companyId = (String.IsNullOrEmpty(Request.Form[dropDownCompany.UniqueID]) ? "1" : Request.Form[dropDownCompany.UniqueID]);
                    OnlineFlashReport flashReport = new OnlineFlashReport(FlashReportResults, GetSumFields(), startDate, market, companyId);
                    labelFlashReport.Text = flashReport.GenerateReport();
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                FlashReportResults = null;
                labelFlashReport.Text = "";
                labelFlashReport.Text = OnlineFlashReport.GetErrorOccurredDisplay();
            }
            finally
            {
                flashGridUpdPnl.Update();
                flashGridUpdPnlProgress.Visible = false;
                searchResults.Style["display"] = "block";
            }
        }
        #endregion

        #region exportToExcel_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void exportToExcel_Click(object sender, EventArgs e)
        {
            bool isExpanded = (flashReportExpanded.Value == "1");
            int tableNumber = Convert.ToInt32(excelTable.Value);
            DateTime startDate = Convert.ToDateTime(dropDownStartDateMonth.SelectedValue + "/1/" + dropDownStartDateYear.SelectedValue);
            int yearDiff = Convert.ToInt32(dropDownFlashYearSelectedValue.Value) - startDate.Year;
            startDate = startDate.AddYears(yearDiff);
            string market = (String.IsNullOrEmpty(dropDownMarket.SelectedValue) ? "" : dropDownMarket.SelectedItem.Text);
            string companyId = (String.IsNullOrEmpty(dropDownCompany.SelectedValue) ? "1" : dropDownCompany.SelectedValue);
            OnlineFlashReport flashReport = new OnlineFlashReport(FlashReportResults, GetSumFields(), startDate, market, companyId);
            WebCommon.ExportHtmlToExcel("OnlineFlashReport", flashReport.GenerateReportExcelExport(isExpanded, tableNumber));
        }
        #endregion

        #region exportToExcelAlternate_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void exportToExcelAlternate_Click(object sender, EventArgs e)
        {
            int tableNumber = Convert.ToInt32(excelTable.Value);
            DateTime startDate = Convert.ToDateTime(dropDownStartDateMonth.SelectedValue + "/1/" + dropDownStartDateYear.SelectedValue);
            int yearDiff = Convert.ToInt32(dropDownFlashYearSelectedValue.Value) - startDate.Year;
            startDate = startDate.AddYears(yearDiff);
            string market = (String.IsNullOrEmpty(dropDownMarket.SelectedValue) ? "" : dropDownMarket.SelectedItem.Text);
            string companyId = (String.IsNullOrEmpty(dropDownCompany.SelectedValue) ? "1" : dropDownCompany.SelectedValue);
            WebCommon.WriteDebugMessage(string.Format("Billings Flash (Alternate report) Exported by: {0}", Security.GetCurrentUserId));
            OnlineFlashWithoutSubTotalReport flashReport = new OnlineFlashWithoutSubTotalReport(FlashReportResults, GetSumFields(), startDate, market, companyId);
            WebCommon.ExportHtmlToExcel("OnlineFlashReport", flashReport.GenerateReportExcelExport(true, 1));
        }
        #endregion

        #region FlashReportResults property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        private DataSet FlashReportResults
        {
            get { return Session["flashReportResultsDataTable"] as DataSet ?? null; }
            set { Session["flashReportResultsDataTable"] = value; }
        }
        #endregion

        #region GetBillingYears method
        /// <summary>TBD</summary>
        /// <param name="allYears">TBD</param>
        /// <returns>TBD</returns>
        public int[] GetBillingYears(string allYears)
        {
            if (String.IsNullOrEmpty(allYears))
            {
                return new int[0];
            }
            ArrayList billingYears = new ArrayList();
            if (allYears.Contains(','))
            {
                string[] allYearsSplit = allYears.Split(',');
                foreach (string year in allYearsSplit)
                {
                    billingYears.Add(Convert.ToInt32(year.Trim()));
                }
            }
            else
            {
                billingYears.Add(Convert.ToInt32(allYears.Trim()));
            }
            return (int[])billingYears.ToArray(typeof(int));
        }
        #endregion

        #region GetSearchParamValue method
        /// <summary>TBD</summary>
        /// <param name="searchParams">TBD</param>
        /// <param name="key">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        public object GetSearchParamValue(Hashtable searchParams, string key, object defaultValue)
        {
            if (searchParams.ContainsKey(key))
            {
                return searchParams[key];
            }
            return defaultValue;
        }
        #endregion

        #region GetSPOrderBy method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string GetSPOrderBy()
        {
            if (radioSortByAdvertiser.Checked)
            {
                return "ADVERTISER, CONTRACT_NUMBER DESC";
            }
            return "CONTRACT_NUMBER DESC, ADVERTISER";
        }
        #endregion

        #region GetSPSumFields method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string GetSPSumFields()
        {
            string sumFields = string.Empty;
            if (checkProfitCenter.Checked)
            {
                sumFields += ((!String.IsNullOrEmpty(sumFields)) ? "," : "") + "PROFIT_CENTER";
            }
            if (checkRevenueType.Checked)
            {
                sumFields += ((!String.IsNullOrEmpty(sumFields)) ? "," : "") + "REVENUE_TYPE";
            }
            if (checkMediaType.Checked)
            {
                sumFields += ((!String.IsNullOrEmpty(sumFields)) ? "," : "") + "MEDIA_TYPE";
            }
            if (checkMediaForm.Checked)
            {
                sumFields += ((!String.IsNullOrEmpty(sumFields)) ? "," : "") + "MEDIA_FORM";
            }
            if (checkParentProductClass.Checked)
            {
                sumFields += ((!String.IsNullOrEmpty(sumFields)) ? "," : "") + "PARENT_PROD_CLASS";
            }
            if (checkProductClass.Checked)
            {
                sumFields += ((!String.IsNullOrEmpty(sumFields)) ? "," : "") + "PRODUCT_CLASS";
            }
            if (checkAe.Checked)
            {
                sumFields += ((!String.IsNullOrEmpty(sumFields)) ? "," : "") + "AE";
            }
            if (!String.IsNullOrEmpty(sumFields))
            {
                sumFields += ",";
            }
            return sumFields;
        }
        #endregion

        #region GetSumFields method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string[] GetSumFields()
        {
            ArrayList sumFields = new ArrayList();
            //If a Sales Type was selected and a market was specified,
            //  include the Market as a Sum Field
            //If a market was not selected and the 'All' sales type was not selected
            //  include Market as a Sum Field
            if (String.IsNullOrEmpty(Request.Form[dropDownMarket.UniqueID]) && !radioAllSalesType.Checked)
            {
                sumFields.Add("MARKET");
            }
            if (checkProfitCenter.Checked)
            {
                sumFields.Add("PROFIT_CENTER");
            }
            if (checkRevenueType.Checked)
            {
                sumFields.Add("REVENUE_TYPE");
            }
            if (checkMediaType.Checked)
            {
                sumFields.Add("MEDIA_TYPE");
            }
            if (checkMediaForm.Checked)
            {
                sumFields.Add("MEDIA_FORM");
            }
            if (checkParentProductClass.Checked)
            {
                sumFields.Add("PARENT_PROD_CLASS");
            }
            if (checkProductClass.Checked)
            {
                sumFields.Add("PRODUCT_CLASS");
            }
            if (checkAe.Checked)
            {
                sumFields.Add("AE");
            }
            return (string[])sumFields.ToArray(typeof(string));
        }
        #endregion

        #region IsBillingYear method
        /// <summary>TBD</summary>
        /// <param name="billingYears">TBD</param>
        /// <param name="year">TBD</param>
        /// <returns>TBD</returns>
        public bool IsBillingYear(int[] billingYears, int year)
        {
            for (int i = 0; i < billingYears.Length; i++)
            {
                if (billingYears[i] == year)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region LoadSearchParams method
        /// <summary>TBD</summary>
        public void LoadSearchParams()
        {
            try
            {
                Hashtable searchParams = IOService.LoadSearchParams(Security.GetCurrentUserId, SCREEN_NAME);
                if (searchParams.Count > 0)
                {
                    //Group By Fields
                    checkProfitCenter.Checked = (((string)searchParams[checkProfitCenter.ID]).ToLower() == "true");
                    checkRevenueType.Checked = Convert.ToBoolean(GetSearchParamValue(searchParams, checkRevenueType.ID, false));
                    checkMediaType.Checked = Convert.ToBoolean(GetSearchParamValue(searchParams, checkMediaType.ID, false));
                    checkMediaForm.Checked = Convert.ToBoolean(GetSearchParamValue(searchParams, checkMediaForm.ID, false));
                    checkAe.Checked = (((string)searchParams[checkAe.ID]).ToLower() == "true");
                    checkParentProductClass.Checked = Convert.ToBoolean(GetSearchParamValue(searchParams, checkParentProductClass.ID, false));
                    checkProductClass.Checked = Convert.ToBoolean(GetSearchParamValue(searchParams, checkProductClass.ID, false));
                    checkMediaTransit.Checked = (((string)searchParams[checkMediaTransit.ID]).ToLower() == "true");
                    checkMediaNonTransit.Checked = (((string)searchParams[checkMediaNonTransit.ID]).ToLower() == "true");
                    checkShipIn.Checked = (((string)searchParams[checkShipIn.ID]).ToLower() == "true");
                    checkShipOut.Checked = (((string)searchParams[checkShipOut.ID]).ToLower() == "true");
                    checkLocal.Checked = (((string)searchParams[checkLocal.ID]).ToLower() == "true");
                    checkExcludeMTA.Checked = (((string)searchParams[checkExcludeMTA.ID]).ToLower() == "true");
                    textEnteredFrom.Text = (string)searchParams[textEnteredFrom.ID];
                    //if no entry from date was specified...default to beginning of the current year
                    if (String.IsNullOrEmpty(textEnteredFrom.Text))
                    {
                        textEnteredFrom.Text = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
                    }
                    //Always default the Entry Thru date to current date (as per previous Helios functionality):
                    textEnteredThru.Text = DateTime.Now.ToShortDateString();
                    textStartingFrom.Text = (string)searchParams[textStartingFrom.ID];
                    textStartingTo.Text = (string)searchParams[textStartingTo.ID];
                    textEndingFrom.Text = (string)searchParams[textEndingFrom.ID];
                    textEndingTo.Text = (string)searchParams[textEndingTo.ID];
                    textInvoiceCreationDateFrom.Text = (string)searchParams[textInvoiceCreationDateFrom.ID];
                    textInvoiceCreationDateTo.Text = (string)searchParams[textInvoiceCreationDateTo.ID];
                    textBillingFrom.Text = (string)searchParams[textBillingFrom.ID];
                    textBillingTo.Text = (string)searchParams[textBillingTo.ID];
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownProductClass.ID]))
                    {
                        dropDownProductClass.SelectedValue = (string)searchParams[dropDownProductClass.ID];
                        dropDownProductClassDefault.Value = (string)searchParams[dropDownProductClass.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownProfitCenter.ID]))
                    {
                        dropDownProfitCenter.SelectedValue = (string)searchParams[dropDownProfitCenter.ID];
                        dropDownProfitCenterDefault.Value = (string)searchParams[dropDownProfitCenter.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownCustomerType.ID]))
                    {
                        dropDownCustomerType.SelectedValue = (string)searchParams[dropDownCustomerType.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownRevenue.ID]))
                    {
                        dropDownRevenue.SelectedValue = (string)searchParams[dropDownRevenue.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownNatOrLocal.ID]))
                    {
                        dropDownNatOrLocal.SelectedValue = (string)searchParams[dropDownNatOrLocal.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownCompany.ID]))
                    {
                        dropDownCompany.SelectedValue = (string)searchParams[dropDownCompany.ID];
                        dropDownCompanyDefault.Value = (string)searchParams[dropDownCompany.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownMarket.ID]))
                    {
                        dropDownMarket.SelectedValue = (string)searchParams[dropDownMarket.ID];
                        dropDownMarketDefault.Value = (string)searchParams[dropDownMarket.ID];
                        marketSalesType.Style["display"] = "block";
                        allMarketSalesType.Style["display"] = "none";
                    }
                    else
                    {
                        marketSalesType.Style["display"] = "none";
                        allMarketSalesType.Style["display"] = "block";
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownStartDateMonth.ID]))
                    {
                        dropDownStartDateMonth.SelectedValue = (string)searchParams[dropDownStartDateMonth.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownStartDateYear.ID]))
                    {
                        dropDownStartDateYear.SelectedValue = (string)searchParams[dropDownStartDateYear.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownMediaType.ID]))
                    {
                        dropDownMediaType.SelectedValue = (string)searchParams[dropDownMediaType.ID];
                        dropDownMediaTypeDefault.Value = (string)searchParams[dropDownMediaType.ID];
                    }
                    textProgram.Text = (string)searchParams[textProgram.ID];
                    textEnteredFrom.Text = (string)searchParams[textEnteredFrom.ID];
                    textContractNumber.Text = (string)searchParams[textContractNumber.ID];
                    radioSortByAdvertiser.Checked = (((string)searchParams[radioSortByAdvertiser.ID]).ToLower() == "true");
                    radioSortByContractNumber.Checked = (((string)searchParams[radioSortByContractNumber.ID]).ToLower() == "true");
                    radioAllSalesType.Checked = (((string)searchParams[radioAllSalesType.ID]).ToLower() == "true");
                    radioLocal.Checked = (((string)searchParams[radioLocal.ID]).ToLower() == "true");
                    radioShipIn.Checked = (((string)searchParams[radioShipIn.ID]).ToLower() == "true");
                    radioShipOut.Checked = (((string)searchParams[radioShipOut.ID]).ToLower() == "true");
                    radioConsolidated.Checked = (((string)searchParams[radioConsolidated.ID]).ToLower() == "true");
                    radioNonConsolidated.Checked = (((string)searchParams[radioNonConsolidated.ID]).ToLower() == "true");
                    //if none of the sales type options are selected, default to All
                    if (!(radioAllSalesType.Checked && radioLocal.Checked && radioShipIn.Checked && radioShipOut.Checked))
                    {
                        radioAllSalesType.Checked = true;
                    }
                    //if neither sort options are selected, default to Advertiser
                    if (!(radioSortByAdvertiser.Checked && radioSortByContractNumber.Checked))
                    {
                        radioSortByAdvertiser.Checked = true;
                    }
                    ae.Id.Text = (string)searchParams["ae.AEId"];
                    ae.Name.Text = (string)searchParams["ae.AEName"];
                    advertiser.Id.Text = (string)searchParams["advertiser.AdvertiserId"];
                    advertiser.Name.Text = (string)searchParams["advertiser.AdvertiserName"];
                    conAdvertiser.Id.Text = (string)searchParams["conAdvertiser.AdvertiserId"];
                    conAdvertiser.Name.Text = (string)searchParams["conAdvertiser.AdvertiserName"];
                    agency.Id.Text = (string)searchParams["agency.AgencyId"];
                    agency.Name.Text = (string)searchParams["agency.AgencyName"];
                    conAgency.Id.Text = (string)searchParams["conAgency.AgencyId"];
                    conAgency.Name.Text = (string)searchParams["conAgency.AgencyName"];
                    productClassSearch.Id.Text = (string)searchParams["productClassSearch.ProductClassId"];
                    productClassSearch.Name.Text = (string)searchParams["productClassSearch.ProductClassName"];
                    mediaFormSearch.Id.Text = (string)searchParams["mediaFormSearch.MediaFormId"];
                    mediaFormSearch.Name.Text = (string)searchParams["mediaFormSearch.MediaFormName"];
                }
                else
                {
                    //Set default values:
                    textEnteredFrom.Text = new DateTime(2002, 1, 1).ToShortDateString();
                    textEnteredThru.Text = DateTime.Now.ToShortDateString();
                    radioSortByAdvertiser.Checked = true;
                    radioAllSalesType.Checked = true;
                    checkShipIn.Checked = true;
                    checkShipOut.Checked = true;
                    checkLocal.Checked = true;
                    checkMediaNonTransit.Checked = true;
                    checkMediaTransit.Checked = true;
                    marketSalesType.Style["display"] = "block";
                    allMarketSalesType.Style["display"] = "none";
                    dropDownCompanyDefault.Value = Security.UserCompanyID;
                    //dropDownCompanyDefault.Value = "1";
                    dropDownMarketDefault.Value = Security.UserMarketID;
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                //Set default values:
                textEnteredFrom.Text = new DateTime(2002, 1, 1).ToShortDateString();
                textEnteredThru.Text = DateTime.Now.ToShortDateString();
                radioSortByAdvertiser.Checked = true;
                radioAllSalesType.Checked = true;
                checkShipIn.Checked = true;
                checkShipOut.Checked = true;
                checkLocal.Checked = true;
                checkMediaNonTransit.Checked = true;
                checkMediaTransit.Checked = true;
                marketSalesType.Style["display"] = "block";
                allMarketSalesType.Style["display"] = "none";
                dropDownCompanyDefault.Value = Security.UserCompanyID;
                //dropDownCompanyDefault.Value = "1";
                dropDownMarketDefault.Value = Security.UserMarketID;
            }
            finally
            {
                //Check the User's permissions
                //  Default the Company/Market if they are not in any of the AD groups
                if (!(Security.IsAdminUser() || Security.IsCorporateUser() || Security.IsDigitalUser() || Security.IsSalesFlashFullAccessUser()))
                {
                    if (!String.IsNullOrEmpty(Security.UserAEId))
                    {
                        ae.Name.Text = Security.GetFullUserNameFromId(Security.GetCurrentUserId);
                        ae.Id.Text = Security.UserAEId;
                    }
                    //dropDownCompanyDefault.Value = "1";
                    dropDownCompanyDefault.Value = Security.UserCompanyID;
                    dropDownMarketDefault.Value = Security.UserMarketID;
                    dropDownCompany.Enabled = false;
                    dropDownMarket.Enabled = false;
                    checkShipIn.Checked = true;
                    checkShipOut.Checked = true;
                    checkLocal.Checked = true;
                    checkMediaNonTransit.Checked = true;
                    checkMediaTransit.Checked = true;
                    marketSalesType.Style["display"] = "block";
                    allMarketSalesType.Style["display"] = "none";
                }
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
            //Response.End();
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
                textContractNumber.Attributes.Add("onkeyup", "return handleContractKeyPress(event,'" + search.ClientID + "');");
                PopulateStartDateMonths();
                PopulateStartDateYears();
                LoadSearchParams();
                userId.Value = Security.GetCurrentUserId;
            }
        }
        #endregion

        #region PopulateFlashReportYearDropDown method
        /// <summary>TBD</summary>
        /// <param name="allYears">TBD</param>
        /// <param name="selectedYear">TBD</param>
        private void PopulateFlashReportYearDropDown(int[] allYears, int selectedYear)
        {
            //dropDownFlashYear
            foreach (ListItem item in dropDownFlashYear.Items)
            {
                dropDownFlashYear.Items.Remove(item);
            }
            for (int i = 0; i < allYears.Length; i++)
            {
                dropDownFlashYear.Items.Add(new ListItem(Convert.ToString(allYears[i]), Convert.ToString(allYears[i])));
            }
            dropDownFlashYear.SelectedValue = Convert.ToString(selectedYear);
        }
        #endregion

        #region PopulateStartDateMonths method
        /// <summary>TBD</summary>
        private void PopulateStartDateMonths()
        {
            dropDownStartDateMonth.Items.Add(new ListItem("January", "1"));
            dropDownStartDateMonth.Items.Add(new ListItem("February", "2"));
            dropDownStartDateMonth.Items.Add(new ListItem("March", "3"));
            dropDownStartDateMonth.Items.Add(new ListItem("April", "4"));
            dropDownStartDateMonth.Items.Add(new ListItem("May", "5"));
            dropDownStartDateMonth.Items.Add(new ListItem("June", "6"));
            dropDownStartDateMonth.Items.Add(new ListItem("July", "7"));
            dropDownStartDateMonth.Items.Add(new ListItem("August", "8"));
            dropDownStartDateMonth.Items.Add(new ListItem("September", "9"));
            dropDownStartDateMonth.Items.Add(new ListItem("October", "10"));
            dropDownStartDateMonth.Items.Add(new ListItem("November", "11"));
            dropDownStartDateMonth.Items.Add(new ListItem("December", "12"));
            //0-based index, decrement the month by 1....
            dropDownStartDateMonth.SelectedValue = Convert.ToString(DateTime.Now.Month);
        }
        #endregion

        #region PopulateStartDateYears method
        /// <summary>TBD</summary>
        private void PopulateStartDateYears()
        {
            DataSet years = App.GetCachedDataSet(App.DataSetType.ContractLineYearsDataSetType);
            foreach (DataRow row in years.Tables[0].Rows)
            {
                dropDownStartDateYear.Items.Add(new ListItem(Convert.ToString(row["YEAR"]), Convert.ToString(row["YEAR"])));
            }
            //int startYear = 2002;
            //for (int i = DateTime.Now.Year; i >= startYear; i--)
            //{
            //    dropDownStartDateYear.Items.Add(new ListItem(Convert.ToString(i), Convert.ToString(i)));
            //}
            dropDownStartDateYear.SelectedValue = Convert.ToString(DateTime.Now.Year);
        }
        #endregion

        #region save_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void save_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExecuteSearch();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
        }
        #endregion

        #region SaveSearchParams method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        [System.Web.Services.WebMethod]
        public static void SaveSearchParams(Hashtable values)
        {
            IOService.SaveSearchParams(values, Security.GetCurrentUserId, SCREEN_NAME);
        }
        #endregion

        #region search_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void search_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExecuteSearch();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
        }
        #endregion

    }

}
