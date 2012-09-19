using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Titan.DataIO;
namespace Apollo
{
    /// <summary>
    /// Summary description for KPIService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class KPIService : System.Web.Services.WebService
    {
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public KPIReport LoadKPIReport(int companyId, int reportMonth, int reportYear)
        {
            try
            {                
                DataSet reportData;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    reportData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("KPI_REPORT_LOADREPORTDATA",
                        Param.CreateParam("COMPANYID", SqlDbType.Int, companyId),
                        Param.CreateParam("KPIREPORTMONTH", SqlDbType.Int, reportMonth),
                        Param.CreateParam("KPIREPORTYEAR", SqlDbType.Int, reportYear)));
                }
                if (reportData.Tables.Count == 0 || reportData.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                KPIReport report = new KPIReport();
                report.kpiReportId = Convert.ToInt32(reportData.Tables[0].Rows[0]["KPI_REPORT_ID"]);
                report.createDate = Convert.ToDateTime(reportData.Tables[0].Rows[0]["CREATE_DATE"]);
                report.lastUpdate = Convert.ToDateTime(reportData.Tables[0].Rows[0]["LAST_UPDATE"]);
                report.reportDate = Convert.ToDateTime(reportData.Tables[0].Rows[0]["REPORT_DATE"]);
                report.createdBy = Convert.ToString(reportData.Tables[0].Rows[0]["CREATED_BY"]);
                report.reportYear = Convert.ToInt32(reportData.Tables[0].Rows[0]["REPORT_YEAR"]);
                report.reportMonth = Convert.ToInt32(reportData.Tables[0].Rows[0]["REPORT_MONTH"]);
                report.targetCollectionAmount = Convert.ToInt32(reportData.Tables[0].Rows[0]["TARGET_COLLECTION_AMOUNT"]);
                report.targetDSOAmount = Convert.ToInt32(reportData.Tables[0].Rows[0]["TARGET_DSO_AMOUNT"]);
                report.invoicingStartDate = Convert.ToDateTime(reportData.Tables[0].Rows[0]["INVOICING_START_DATE"]);
                report.companyId = Convert.ToInt32(reportData.Tables[0].Rows[0]["COMPANY_ID"]);
                report.invoicingWeek5Date = Convert.ToDateTime(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "INVOICING_WEEK_5_DATE", DateTime.MinValue));
                report.projectedBillingWeek1 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PROJECTED_BILLING_WEEK_1", 0));
                report.projectedBillingWeek2 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PROJECTED_BILLING_WEEK_2", 0));
                report.projectedBillingWeek3 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PROJECTED_BILLING_WEEK_3", 0));
                report.projectedBillingWeek4 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PROJECTED_BILLING_WEEK_1", 0));
                report.projectedBillingWeek5 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PROJECTED_BILLING_WEEK_1", 0));
                report.actualInvoicingWeek1 = Convert.ToDateTime(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "ACTUAL_INVOICING_WEEK_1", DateTime.MinValue));
                report.actualInvoicingWeek2 = Convert.ToDateTime(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "ACTUAL_INVOICING_WEEK_2", DateTime.MinValue));
                report.actualInvoicingWeek3 = Convert.ToDateTime(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "ACTUAL_INVOICING_WEEK_3", DateTime.MinValue));
                report.actualInvoicingWeek4 = Convert.ToDateTime(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "ACTUAL_INVOICING_WEEK_4", DateTime.MinValue));
                report.actualInvoicingWeek5 = Convert.ToDateTime(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "ACTUAL_INVOICING_WEEK_5", DateTime.MinValue));
                report.didNotPostWeek1 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "DID_NOT_POST_WEEK_1", 0));
                report.didNotPostWeek2 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "DID_NOT_POST_WEEK_2", 0));
                report.didNotPostWeek3 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "DID_NOT_POST_WEEK_3", 0));
                report.didNotPostWeek4 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "DID_NOT_POST_WEEK_4", 0));
                report.didNotPostWeek5 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "DID_NOT_POST_WEEK_5", 0));
                report.catchUpWeek1 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "CATCH_UP_WEEK_1", 0));
                report.catchUpWeek2 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "CATCH_UP_WEEK_2", 0));
                report.catchUpWeek3 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "CATCH_UP_WEEK_3", 0));
                report.catchUpWeek4 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "CATCH_UP_WEEK_4", 0));
                report.catchUpWeek5 = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "CATCH_UP_WEEK_5", 0));
                report.creditAppsReceived = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "CREDIT_APPS_RECEIVED", 0));
                report.creditAppsChecked = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "CREDIT_APPS_CHECKED", 0));
                report.paymentGivenNum = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_GIVEN_NUM", 0));
                report.paymentGivenNumPercent = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_GIVEN_NUM_PERCENT", 0));
                report.paymentGivenAmount = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_GIVEN_AMOUNT", 0));
                report.paymentGivenAmountPercent = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_GIVEN_AMOUNT_PERCENT", 0));
                report.paymentMissingNum = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_MISSING_NUM", 0));
                report.paymentMissingNumPercent = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_MISSING_NUM_PERCENT", 0));
                report.paymentMissingAmount = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_MISSING_AMOUNT", 0));
                report.paymentMissingAmountPercent = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "PAYMENT_MISSING_AMOUNT_PERCENT", 0));
                report.waivedNum = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "WAIVED_NUM", 0));
                report.waivedNumPercent = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "WAIVED_NUM_PERCENT", 0));
                report.waivedAmount = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "WAIVED_AMOUNT", 0));
                report.waivedAmountPercent = Convert.ToInt32(IO.GetDataRowValue(reportData.Tables[0].Rows[0], "WAIVED_AMOUNT_PERCENT", 0));
                return report;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to load the report data.");
            }
        }

        #region CountWorkDays method
        /// <summary>
        ///     From: http://www.experts-exchange.com/Programming/Languages/C_Sharp/Q_24535798.html
        ///     Compares two DateTime values and determines the number of Workdays in between.
        ///         Allows for inclusion of known non-workdays.
        /// </summary>
        /// <param name="startDate">Start Date value</param>
        /// <param name="endDate">End Date value</param>
        /// <param name="excludedDates">Any holidays or non-workdays</param>
        /// <returns>
        ///     The number (int) of Workdays between the dates specified.
        /// </returns>
        private static int CountWorkDays(DateTime startDate, DateTime endDate, List<DateTime> excludedDates)
        {
            int dayCount = 0;
            int inc = 1;
            bool endDateIsInPast = startDate > endDate;
            DateTime tmpDate = startDate;
            DateTime finiDate = endDate;
            if (endDateIsInPast)
            {
                // Swap dates around
                tmpDate = endDate;
                finiDate = startDate;
                // Set increment value to -1, so it DayCount decrements rather
                // than increments
                inc = -1;
            }
            while (tmpDate <= finiDate)
            {
                if (!excludedDates.Contains(tmpDate) && tmpDate.DayOfWeek != DayOfWeek.Saturday && tmpDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    dayCount += inc;
                }
                // Move onto next day
                tmpDate = tmpDate.AddDays(1);
            }
            return dayCount;
        }
        #endregion

        public class WorkDayInfo
        {
            public WorkDayInfo()
            {
            }
            public WorkDayInfo(int workingDays, int workingDaysElapsed)
            {
                this.workingDays = workingDays;
                this.workingDaysElapsed = workingDaysElapsed;
            }
            public int workingDays { get; set; }
            public int workingDaysElapsed { get; set; }
        }

        #region GetWorkDays method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="monthStartDateStr">TBD</param>
        /// <param name="reportDateStr">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public WorkDayInfo GetWorkDays(int companyId, string monthStartDateStr, string reportDateStr)
        {
            WorkDayInfo workDays = new WorkDayInfo();
            DateTime monthStartDate, reportDate;
            List<DateTime> bankingHolidays;
            try
            {
                monthStartDate = DateTime.Parse(monthStartDateStr);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("Month Start Date was not in the correct format.");
            }
            try
            {
                reportDate = DateTime.Parse(reportDateStr);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("Report Date was not in the correct format.");
            }
            try
            {
                bankingHolidays = GetBankingHolidaysForMonth(companyId, monthStartDate);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve banking holidays.");
            }
            workDays.workingDays = CountWorkDays(monthStartDate, monthStartDate.AddMonths(1).AddDays(-1), bankingHolidays);
            workDays.workingDaysElapsed = CountWorkDays(monthStartDate, reportDate, bankingHolidays);
            return workDays;
        }
        #endregion

        public class KPIReport
        {
            public KPIReport()
            {
            }
            public KPIReport(int kpiReportId, DateTime createDate, DateTime lastUpdate, string createdBy, int reportYear, int reportMonth, DateTime reportDate
                , int targetCollectionAmount, int targetDSOAmount, DateTime invoicingStartDate, DateTime invoicingWeek5Date
                , int projectedBillingWeek1, int projectedBillingWeek2, int projectedBillingWeek3, int projectedBillingWeek4, int projectedBillingWeek5
                , DateTime actualInvoicingWeek1, DateTime actualInvoicingWeek2, DateTime actualInvoicingWeek3, DateTime actualInvoicingWeek4, DateTime actualInvoicingWeek5
                , int didNotPostWeek1, int didNotPostWeek2, int didNotPostWeek3, int didNotPostWeek4, int didNotPostWeek5
                , int catchUpWeek1, int catchUpWeek2, int catchUpWeek3, int catchUpWeek4, int catchUpWeek5
                , int creditAppsReceived, int creditAppsChecked, int paymentGivenNum, int paymentGivenNumPercent, int paymentGivenAmount, int paymentGivenAmountPercent
                , int paymentMissingNum, int paymentMissingNumPercent, int paymentMissingAmount, int paymentMissingAmountPercent
                , int waivedNum, int waivedNumPercent, int waivedAmount, int waivedAmountPercent, int companyId)
            {
                this.kpiReportId = kpiReportId;
                this.createDate = createDate;
                this.lastUpdate = lastUpdate;
                this.createdBy = createdBy;
                this.reportYear = reportYear;
                this.reportMonth = reportMonth;
                this.reportDate = reportDate;
                this.targetCollectionAmount = targetCollectionAmount;
                this.targetDSOAmount = targetDSOAmount;
                this.invoicingStartDate = invoicingStartDate;
                this.invoicingWeek5Date = invoicingWeek5Date;
                this.projectedBillingWeek1 = projectedBillingWeek1;
                this.projectedBillingWeek2 = projectedBillingWeek2;
                this.projectedBillingWeek3 = projectedBillingWeek3;
                this.projectedBillingWeek4 = projectedBillingWeek4;
                this.projectedBillingWeek5 = projectedBillingWeek5;
                this.actualInvoicingWeek1 = actualInvoicingWeek1;
                this.actualInvoicingWeek2 = actualInvoicingWeek2;
                this.actualInvoicingWeek3 = actualInvoicingWeek3;
                this.actualInvoicingWeek4 = actualInvoicingWeek4;
                this.actualInvoicingWeek5 = actualInvoicingWeek5;
                this.didNotPostWeek1 = didNotPostWeek1;
                this.didNotPostWeek2 = didNotPostWeek2;
                this.didNotPostWeek3 = didNotPostWeek3;
                this.didNotPostWeek4 = didNotPostWeek4;
                this.didNotPostWeek5 = didNotPostWeek5;
                this.catchUpWeek1 = catchUpWeek1;
                this.catchUpWeek2 = catchUpWeek2;
                this.catchUpWeek3 = catchUpWeek3;
                this.catchUpWeek4 = catchUpWeek4;
                this.catchUpWeek5 = catchUpWeek5;
                this.paymentGivenNum = paymentGivenNum;
                this.paymentGivenNumPercent = paymentGivenNumPercent;
                this.paymentGivenAmount = paymentGivenAmount;
                this.paymentGivenAmountPercent = paymentGivenAmountPercent;
                this.paymentMissingNum = paymentMissingNum;
                this.paymentMissingNumPercent = paymentMissingNumPercent;
                this.waivedNum = waivedNum;
                this.waivedNumPercent = waivedNumPercent;
                this.waivedAmount = waivedAmount;
                this.waivedAmountPercent = waivedAmountPercent;
                this.companyId = companyId;
            }
            public int kpiReportId { get; set; }
            public DateTime createDate { get; set; }
            public DateTime lastUpdate { get; set; }
            public string createdBy { get; set; }
            public int reportYear { get; set; }
            public int reportMonth { get; set; }
            public DateTime reportDate { get; set; }
            public int targetCollectionAmount { get; set; }
            public int targetDSOAmount { get; set; }
            public DateTime invoicingStartDate { get; set; }
            public DateTime invoicingWeek5Date { get; set; }
            public int projectedBillingWeek1 { get; set; }
            public int projectedBillingWeek2 { get; set; }
            public int projectedBillingWeek3 { get; set; }
            public int projectedBillingWeek4 { get; set; }
            public int projectedBillingWeek5 { get; set; }
            public DateTime actualInvoicingWeek1 { get; set; }
            public DateTime actualInvoicingWeek2 { get; set; }
            public DateTime actualInvoicingWeek3 { get; set; }
            public DateTime actualInvoicingWeek4 { get; set; }
            public DateTime actualInvoicingWeek5 { get; set; }
            public int didNotPostWeek1 { get; set; }
            public int didNotPostWeek2 { get; set; }
            public int didNotPostWeek3 { get; set; }
            public int didNotPostWeek4 { get; set; }
            public int didNotPostWeek5 { get; set; }
            public int catchUpWeek1 { get; set; }
            public int catchUpWeek2 { get; set; }
            public int catchUpWeek3 { get; set; }
            public int catchUpWeek4 { get; set; }
            public int catchUpWeek5 { get; set; }
            public int creditAppsReceived { get; set; }
            public int creditAppsChecked { get; set; }
            public int paymentGivenNum { get; set; }
            public int paymentGivenNumPercent { get; set; }
            public int paymentGivenAmount { get; set; }
            public int paymentGivenAmountPercent { get; set; }
            public int paymentMissingNum { get; set; }
            public int paymentMissingNumPercent { get; set; }
            public int paymentMissingAmount { get; set; }
            public int paymentMissingAmountPercent { get; set; }
            public int waivedNum { get; set; }
            public int waivedNumPercent { get; set; }
            public int waivedAmount { get; set; }
            public int waivedAmountPercent { get; set; }
            public int companyId { get; set; }
        }

        #region AgedARItem class
        /// <summary>TBD</summary>
        public class AgedARItem
        {

            #region Default constructor
            /// <summary>TBD</summary>
            public AgedARItem()
            {
            }
            #endregion

            #region Parameterized constructor (customerType, sales, debits, credits, payments, total)
            /// <summary>TBD</summary>
            /// <param name="customerType">TBD</param>
            /// <param name="sales">TBD</param>
            /// <param name="debits">TBD</param>
            /// <param name="credits">TBD</param>
            /// <param name="payments">TBD</param>
            /// <param name="total">TBD</param>
            public AgedARItem(string customerType, double sales, double debits, double credits, double payments, double total)
            {
                this.customerType = customerType;
                this.sales = sales;
                this.debits = debits;
                this.credits = credits;
                this.payments = payments;
                this.total = total;
            }
            #endregion

            #region credits property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double credits { get; set; }
            #endregion

            #region customerType property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public string customerType { get; set; }
            #endregion

            #region debits property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double debits { get; set; }
            #endregion

            #region payments property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double payments { get; set; }
            #endregion

            #region sales property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double sales { get; set; }
            #endregion

            #region total property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double total { get; set; }
            #endregion

        }
        #endregion

        #region AgedTrialBalanceRecord class
        /// <summary>TBD</summary>
        public class AgedTrialBalanceRecord
        {

            #region Default constructor
            /// <summary>TBD</summary>
            public AgedTrialBalanceRecord()
            {
            }
            #endregion

            #region Parameterized constructor (type, identifier, sales, debits, credits, payments, total)
            /// <summary>TBD</summary>
            /// <param name="type">TBD</param>
            /// <param name="identifier">TBD</param>
            /// <param name="sales">TBD</param>
            /// <param name="debits">TBD</param>
            /// <param name="credits">TBD</param>
            /// <param name="payments">TBD</param>
            /// <param name="total">TBD</param>
            public AgedTrialBalanceRecord(string type, string identifier, double sales, double debits, double credits, double payments, double total)
            {
                this.type = type;
                this.identifier = identifier;
                this.sales = sales;
                this.debits = debits;
                this.credits = credits;
                this.payments = payments;
                this.total = total;
            }
            #endregion

            #region credits property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double credits { get; set; }
            #endregion

            #region debits property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double debits { get; set; }
            #endregion

            #region identifier property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public string identifier { get; set; }
            #endregion

            #region payments property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double payments { get; set; }
            #endregion

            #region sales property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double sales { get; set; }
            #endregion

            #region total property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double total { get; set; }
            #endregion

            #region type property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public string type { get; set; }
            #endregion

        }
        #endregion

        #region CustomerData class
        /// <summary>TBD</summary>
        public class CustomerData
        {

            #region Default constructor
            /// <summary>TBD</summary>
            public CustomerData()
            {
            }
            #endregion

            #region Parameterized constructor (customerType, customerCount, amountCollectable, balanceOver150, sales, credits, debits)
            /// <summary>TBD</summary>
            /// <param name="customerType">TBD</param>
            /// <param name="customerCount">TBD</param>
            /// <param name="amountCollectable">TBD</param>
            /// <param name="balanceOver150">TBD</param>
            /// <param name="sales">TBD</param>
            /// <param name="credits">TBD</param>
            /// <param name="debits">TBD</param>
            public CustomerData(string customerType, int customerCount, double amountCollectable, double balanceOver150, double sales, double credits, double debits)
            {
                this.customerType = customerType;
                this.customerCount = customerCount;
                this.amountCollectable = amountCollectable;
                this.balanceOver150 = balanceOver150;
                this.sales = sales;
                this.credits = credits;
                this.debits = debits;
            }
            #endregion

            #region amountCollectable property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double amountCollectable { get; set; }
            #endregion

            #region balanceOver150 property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double balanceOver150 { get; set; }
            #endregion

            #region credits property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double credits { get; set; }
            #endregion

            #region customerCount property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public int customerCount { get; set; }
            #endregion

            #region customerType property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public string customerType { get; set; }
            #endregion

            #region debits property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double debits { get; set; }
            #endregion

            #region sales property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double sales { get; set; }
            #endregion

        }
        #endregion

        #region MarketBreakdownRecord class
        /// <summary>TBD</summary>
        public class MarketBreakdownRecord
        {

            #region Default constructor
            /// <summary>TBD</summary>
            public MarketBreakdownRecord()
            {
            }
            #endregion

            #region Parameterized constructor (marketName, amountCollectable, balanceOver150, balanceOver120, balanceOver60, fullBalance, salesPast90, creditsPast90, debitsPast90)
            /// <summary>TBD</summary>
            /// <param name="marketName">TBD</param>
            /// <param name="amountCollectable">TBD</param>
            /// <param name="balanceOver150">TBD</param>
            /// <param name="balanceOver120">TBD</param>
            /// <param name="balanceOver60">TBD</param>
            /// <param name="fullBalance">TBD</param>
            /// <param name="salesPast90">TBD</param>
            /// <param name="creditsPast90">TBD</param>
            /// <param name="debitsPast90">TBD</param>
            public MarketBreakdownRecord(string marketName, double amountCollectable, double balanceOver150, double balanceOver120, double balanceOver60, double fullBalance, double salesPast90, double creditsPast90, double debitsPast90)
            {
                this.marketName = marketName;
                this.amountCollectable = amountCollectable;
                this.balanceOver150 = balanceOver150;
                this.balanceOver120 = balanceOver120;
                this.balanceOver60 = balanceOver60;
                this.fullBalance = fullBalance;
                this.salesPast90 = salesPast90;
                this.creditsPast90 = creditsPast90;
                this.debitsPast90 = debitsPast90;
            }
            #endregion

            #region amountCollectable property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double amountCollectable { get; set; }
            #endregion

            #region balanceOver120 property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double balanceOver120 { get; set; }
            #endregion

            #region balanceOver150 property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double balanceOver150 { get; set; }
            #endregion

            #region balanceOver60 property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double balanceOver60 { get; set; }
            #endregion

            #region creditsPast90 property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double creditsPast90 { get; set; }
            #endregion

            #region debitsPast90 property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double debitsPast90 { get; set; }
            #endregion

            #region fullBalance property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double fullBalance { get; set; }
            #endregion

            #region marketName property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public string marketName { get; set; }
            #endregion

            #region salesPast90 property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double salesPast90 { get; set; }
            #endregion

        }
        #endregion

        #region SalesByDocAndMonth class
        /// <summary>TBD</summary>
        public class SalesByDocAndMonth
        {

            #region Default constructor
            /// <summary>TBD</summary>
            public SalesByDocAndMonth()
            {
            }
            #endregion

            #region Parameterized constructor (docType, month, amount)
            /// <summary>TBD</summary>
            /// <param name="docType">TBD</param>
            /// <param name="month">TBD</param>
            /// <param name="amount">TBD</param>
            public SalesByDocAndMonth(string docType, DateTime month, double amount)
            {
                this.docType = docType;
                this.month = month;
                this.amount = amount;
            }
            #endregion

            #region amount property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double amount { get; set; }
            #endregion

            #region docType property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public string docType { get; set; }
            #endregion

            #region month property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public DateTime month { get; set; }
            #endregion

        }
        #endregion

        #region SalesData class
        /// <summary>TBD</summary>
        public class SalesData
        {

            #region Default constructor
            /// <summary>TBD</summary>
            public SalesData()
            {
            }
            #endregion

            #region Parameterized constructor (docDate, amount)
            /// <summary>TBD</summary>
            /// <param name="docDate">TBD</param>
            /// <param name="amount">TBD</param>
            public SalesData(DateTime docDate, double amount)
            {
                this.docDate = docDate;
                this.amount = amount;
            }
            #endregion

            #region amount property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public double amount { get; set; }
            #endregion

            #region docDate property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public DateTime docDate { get; set; }
            #endregion

        }
        #endregion

        #region AggregateAgedRowBalance method
        /// <summary>TBD</summary>
        /// <param name="transactionType">TBD</param>
        /// <param name="rowAmount">TBD</param>
        /// <param name="sales">TBD</param>
        /// <param name="debits">TBD</param>
        /// <param name="credits">TBD</param>
        /// <param name="payments">TBD</param>
        public static void AggregateAgedRowBalance(int transactionType, double rowAmount, ref double sales, ref double debits, ref double credits, ref double payments)
        {
            switch (transactionType)
            {
                case 1://Sales Invoice
                    sales += rowAmount;
                    break;
                case 3://Debit Record
                    debits += rowAmount;
                    break;
                case 7://Credit Record
                    credits += rowAmount;
                    break;
                case 9://Payment Record
                    payments += rowAmount;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region CollectedToDate method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <param name="startOfMonth">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public double CollectedToDate(int companyId, string reportDate, string startOfMonth)
        {
            List<SqlParameter> spParams = new List<SqlParameter>();
            double amountCollected = 0.00;
            string storedProcName = (companyId == 1 ? "KPI_DATA_AMOUNTCOLLECTED" : "KPI_DATA_AMOUNTCOLLECTED_TOC");
            spParams.Add(Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate)));
            spParams.Add(Param.CreateParam("REPORTMONTH", SqlDbType.Int, Convert.ToDateTime(startOfMonth).Month));
            spParams.Add(Param.CreateParam("REPORTYEAR", SqlDbType.Int, Convert.ToDateTime(startOfMonth).Year));
            try
            {
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    amountCollected = Convert.ToDouble(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc(storedProcName, spParams)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Amount Collected.");
            }
            return amountCollected;
        }
        #endregion

        #region GetAgedARBalances method
        /// <summary>TBD</summary>
        /// <param name="agingDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<AgedARItem> GetAgedARBalances(string agingDate)
        {
            try
            {
                DataSet agedBalancesData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    agedBalancesData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("KPI_Data_HATB_Wrapper", Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(agingDate))));
                }
                if (agedBalancesData == null)
                {
                    throw new Exception("No data was retrieved for Aged Balances.");
                }
                return RetrieveAgedARBalancesJSON(agedBalancesData);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Aged Balances");
            }
        }
        #endregion

        #region RetrieveAgedARBalancesJSON method
        /// <summary>TBD</summary>
        /// <param name="agedBalanceData">TBD</param>
        /// <returns>TBD</returns>
        public List<AgedARItem> RetrieveAgedARBalancesJSON(DataSet agedBalanceData)
        {
            double largeSales = 0.0, largeCredits = 0.0, largeDebits = 0.0, largePayments = 0.0;
            double smallSales = 0.0, smallCredits = 0.0, smallDebits = 0.0, smallPayments = 0.0;
            double directSales = 0.0, directCredits = 0.0, directDebits = 0.0, directPayments = 0.0;
            double largeTotal = 0.0, smallTotal = 0.0, directTotal = 0.0;
            int transactionType = 0;
            double rowAmount = 0.0;
            string customerClass = string.Empty;
            foreach (DataRow row in agedBalanceData.Tables[0].Rows)
            {
                transactionType = Convert.ToInt32(row["RMDTYPAL"]);
                rowAmount = Convert.ToDouble(row["CURTRXAM"]);
                customerClass = Convert.ToString(row["CUSTCLAS"]).Trim().ToUpper();
                switch (customerClass)
                {
                    case "LARGE AGENCY":
                        AggregateAgedRowBalance(transactionType, rowAmount, ref largeSales, ref largeDebits, ref largeCredits, ref largePayments);
                        break;
                    case "SMALL AGENCY":
                        AggregateAgedRowBalance(transactionType, rowAmount, ref smallSales, ref smallDebits, ref smallCredits, ref smallPayments);
                        break;
                    case "DIRECT":
                        AggregateAgedRowBalance(transactionType, rowAmount, ref directSales, ref directDebits, ref directCredits, ref directPayments);
                        break;
                    default:
                        break;
                }
            }
            largeTotal = largeSales + largeDebits + largeCredits + largePayments;
            smallTotal = smallSales + smallDebits + smallCredits + smallPayments;
            directTotal = directSales + directDebits + directCredits + directPayments;
            List<AgedARItem> arBalances = new List<AgedARItem>();
            arBalances.Add(new AgedARItem("Direct", directSales, directDebits, directCredits, directPayments, directTotal));
            arBalances.Add(new AgedARItem("Large Agency", largeSales, largeDebits, largeCredits, largePayments, largeTotal));
            arBalances.Add(new AgedARItem("Small Agency", smallSales, smallDebits, smallCredits, smallPayments, smallTotal));
            return arBalances;
        }
        #endregion

        #region GetAgedARBalancesNew method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="agingDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<AgedTrialBalanceRecord> GetAgedARBalancesNew(int companyId, string agingDate)
        {
            List<AgedTrialBalanceRecord> agedTrialBalances = new List<AgedTrialBalanceRecord>();
            string storedProcName = (companyId == 1 ? "KPI_Data_HATB_Wrapper_new" : "KPI_Data_HATB_Wrapper_TOC");
            try
            {
                DataSet agedBalancesData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    agedBalancesData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName, Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(agingDate))));
                }
                if (agedBalancesData == null)
                {
                    throw new Exception("No data was retrieved for Aged Balances.");
                }
                foreach (DataRow row in agedBalancesData.Tables[0].Rows)
                {
                    agedTrialBalances.Add(new AgedTrialBalanceRecord("customer"
                        , Convert.ToString(IO.GetDataRowValue(row, "CUSTOMER_TYPE", ""))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "SALES", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "DEBITS", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "CREDITS", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "PAYMENTS", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "TOTAL", "0.00"))
                    ));
                }
                foreach (DataRow row in agedBalancesData.Tables[1].Rows)
                {
                    agedTrialBalances.Add(new AgedTrialBalanceRecord("market"
                        , Convert.ToString(IO.GetDataRowValue(row, "MARKET", ""))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "SALES", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "DEBITS", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "CREDITS", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "PAYMENTS", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "TOTAL", "0.00"))
                    ));
                }
                return agedTrialBalances;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Aged Balances");
            }
        }
        #endregion

        #region GetBankingHolidaysForMonth method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<DateTime> GetBankingHolidaysForMonth(int companyId, DateTime reportDate)
        {
            List<DateTime> bankingHolidays = new List<DateTime>();
            List<SqlParameter> spParams = new List<SqlParameter>();
            spParams.Add(Param.CreateParam("MONTH", SqlDbType.Int, reportDate.Month));
            spParams.Add(Param.CreateParam("YEAR", SqlDbType.Int, reportDate.Year));
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, companyId));
            DataSet bankingHolidayData = null;
            using (IO io = new IO(WebCommon.KPIConnectionString))
            {
                bankingHolidayData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("KPI_HOLIDAYS_GETBANKINGHOLIDAYS", spParams));
            }
            if (bankingHolidayData != null)
            {
                foreach (DataRow row in bankingHolidayData.Tables[0].Rows)
                {
                    bankingHolidays.Add(Convert.ToDateTime(row["BANKING_HOLIDAY_DATE"]));
                }
            }
            return bankingHolidays;
        }
        #endregion

        #region GetCustomerTypeBreakdown method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<CustomerData> GetCustomerTypeBreakdown(int companyId, string reportDate)
        {
            StringBuilder customerBreakdownJSON = new StringBuilder();
            string storedProcName = (companyId == 1 ? "KPI_Data_BreakdownByCustType" : "KPI_Data_BreakdownByCustType_TOC");
            try
            {
                DataSet customerBreakdownData = null;
                List<CustomerData> customerData = new List<CustomerData>();
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    customerBreakdownData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName, Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (customerBreakdownData == null)
                {
                    throw new Exception("No Customer Breakdown data was retrieved.");
                }
                foreach (DataRow row in customerBreakdownData.Tables[0].Rows)
                {
                    customerData.Add(new CustomerData(Convert.ToString(IO.GetDataRowValue(row, "Customer Type", ""))
                        , Convert.ToInt32(IO.GetDataRowValue(row, "Count of Customer Type", "0"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Amount Collectable", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Balance Over 150 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Sales Past 90 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Credits Past 90 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Debits Past 90 Days", "0.00"))
                    ));
                }
                return customerData;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Customer Breakdown.");
            }
        }
        #endregion

        #region GetMarketBreakdown method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<MarketBreakdownRecord> GetMarketBreakdown(int companyId, string reportDate)
        {
            List<MarketBreakdownRecord> marketBreakdown = new List<MarketBreakdownRecord>();
            string storedProcName = (companyId == 1 ? "KPI_Data_BreakdownByMarket" : "KPI_Data_BreakdownByMarket_TOC");
            try
            {
                DataSet marketBreakdownData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    marketBreakdownData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName, Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (marketBreakdownData == null)
                {
                    throw new Exception("No Market Breakdown data was retrieved.");
                }
                foreach (DataRow row in marketBreakdownData.Tables[0].Rows)
                {
                    marketBreakdown.Add(new MarketBreakdownRecord(Convert.ToString(IO.GetDataRowValue(row, "MARKET", string.Empty))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Amount Collectable", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Balance Over 150 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Balance Over 120 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Balance Over 60 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Balance", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Sales Past 90 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Credits Past 90 Days", "0.00"))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Debits Past 90 Days", "0.00"))
                    ));
                }
                return marketBreakdown;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Market Breakdown.");
            }
        }
        #endregion

        #region GetSalesByDocTypeAndMonth method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<SalesByDocAndMonth> GetSalesByDocTypeAndMonth(int companyId, string reportDate)
        {
            List<SalesByDocAndMonth> sales = new List<SalesByDocAndMonth>();
            string storedProcName = (companyId == 1 ? "KPI_Data_SalesDataByDocTypeAndMonth" : "KPI_Data_SalesDataByDocTypeAndMonth_TOC");
            try
            {
                DataSet salesByDocData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    salesByDocData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName, Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (salesByDocData == null)
                {
                    throw new Exception("No data was retrieved for Previous Month Sales Data.");
                }
                foreach (DataRow row in salesByDocData.Tables[0].Rows)
                {
                    sales.Add(new SalesByDocAndMonth(Convert.ToString(IO.GetDataRowValue(row, "Document Type", ""))
                        , Convert.ToDateTime(IO.GetDataRowValue(row, "Period", ""))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Amount", 0.00))));
                }
                return sales;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Sales Data for Previous Months.");
            }
        }
        #endregion

        #region GetSalesData method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<SalesData> GetSalesData(int companyId, string reportDate)
        {
            try
            {
                List<SalesData> sales = new List<SalesData>();
                DataSet salesData = null;
                string storedProcName = (companyId == 1 ? "KPI_Data_SalesDataForMonth" : "KPI_Data_SalesDataForMonth_TOC");
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    salesData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName, Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (salesData == null)
                {
                    throw new Exception("No Sales Data could be retrieved.");
                }
                foreach (DataRow row in salesData.Tables[0].Rows)
                {
                    sales.Add(new SalesData(Convert.ToDateTime(IO.GetDataRowValue(row, "Document Date", ""))
                        , Convert.ToDouble(IO.GetDataRowValue(row, "Amount", 0.00))));
                }
                return sales;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve Sales Data.");
            }
        }
        #endregion

        public class BadDebtARBalance
        {
            public BadDebtARBalance()
            {
            }
            public BadDebtARBalance(double badDebtAmount, double arBalance)
            {
                this.badDebtAmount = badDebtAmount;
                this.arBalance = arBalance;
            }
            public double badDebtAmount { get; set; }
            public double arBalance { get; set; }
        }

        #region RetrieveBadDebtAndARBalance method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public BadDebtARBalance RetrieveBadDebtAndARBalance(int companyId, string reportDate)
        {
            BadDebtARBalance badDebtArBalance;
            try
            {
                DataSet badDebtArBalData = null;
                string storedProcName = (companyId == 1 ? "KPI_Data_BadDebt_ARBalance" : "KPI_Data_BadDebt_ARBalance_TOC");
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    badDebtArBalData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName, Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (badDebtArBalData == null)
                {
                    throw new Exception("No data was retrieved for Bad Debt and AR Balance.");
                }
                badDebtArBalance = new BadDebtARBalance(Convert.ToDouble(IO.GetDataRowValue(badDebtArBalData.Tables[0].Rows[0], "Bad Debt", "0.00"))
                    , Convert.ToDouble(IO.GetDataRowValue(badDebtArBalData.Tables[0].Rows[0], "Period AR Balance", "0.00")));
                return badDebtArBalance;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve Bad Debt and AR Balances.");
            }
        }
        #endregion

        public class MarketTargetReport
        {
            public MarketTargetReport()
            {
            }
            public MarketTargetReport(string marketId, DateTime reportDate, MarketTarget marketTargetData, List<MarketTargetReportItem> marketTargetReportData)
            {
                this.marketId = marketId;
                this.reportDate = reportDate;
                this.marketTargetData = marketTargetData;
                this.marketTargetReportData = marketTargetReportData;
            }
            public string marketId { get; set; }
            public DateTime reportDate { get; set; }
            public MarketTarget marketTargetData { get; set; }
            public List<MarketTargetReportItem> marketTargetReportData { get; set; }            
        }

        public class MarketTargetReportItem
        {
            public MarketTargetReportItem()
            {
            }
            public MarketTargetReportItem(string marketId, string customerType
                , double bucket1Amount, double bucket2Amount, double bucket3Amount
                , double bucket4Amount, double bucket5Amount, double bucket6Amount)
            {
                this.marketId = marketId;
                this.customerType = customerType;
                this.bucket1Amount = bucket1Amount;
                this.bucket2Amount = bucket2Amount;
                this.bucket3Amount = bucket3Amount;
                this.bucket4Amount = bucket4Amount;
                this.bucket5Amount = bucket5Amount;
                this.bucket6Amount = bucket6Amount;
            }
            public string customerType { get; set; }
            public string marketId { get; set; }
            public double bucket1Amount { get; set; }
            public double bucket2Amount { get; set; }
            public double bucket3Amount { get; set; }
            public double bucket4Amount { get; set; }
            public double bucket5Amount { get; set; }
            public double bucket6Amount { get; set; }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public MarketTargetReport GetMarketTargetReport(string marketId, int companyId, DateTime reportDate)
        {
            try
            {
                MarketTargetReport marketTargetReport = new MarketTargetReport();
                marketTargetReport.marketId = marketId;
                marketTargetReport.reportDate = reportDate;
                marketTargetReport.marketTargetData = GetMarketTargets(marketId, companyId);

                DataSet marketArData;
                string storedProcName = (String.Compare(marketId, "TOR", true) == 0 ? "KPI_Data_HATB_Wrapper_By_Market_TOC" : "KPI_Data_HATB_Wrapper_By_Market_USA");
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    marketArData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("MARKETID", SqlDbType.VarChar, marketId),
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, reportDate)));
                }

                marketTargetReport.marketTargetReportData = new List<MarketTargetReportItem>();
                foreach (DataRow row in marketArData.Tables[0].Rows)
                {
                    marketTargetReport.marketTargetReportData.Add(new MarketTargetReportItem(marketId
                        , Convert.ToString(row["CUSTOMER_TYPE"])
                        , Convert.ToDouble(row["Current"])
                        , Convert.ToDouble(row["31-60"])
                        , Convert.ToDouble(row["61-90"])
                        , Convert.ToDouble(row["91-120"])
                        , Convert.ToDouble(row["120-150"])
                        , Convert.ToDouble(row["Over 150"])));
                }

                return marketTargetReport;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Market Target Report.");
            }
        }

        public class MarketTarget
        {
            public MarketTarget()
            {
            }
            public MarketTarget(int targetId, string marketId
                ,int directBucket1,int directBucket2, int directBucket3, int directBucket4, int directBucket5, int directBucket6
                ,int agencyBucket1,int agencyBucket2, int agencyBucket3, int agencyBucket4, int agencyBucket5, int agencyBucket6
                ,string updatedBy, int companyId)
            {
                this.targetId = targetId;
                this.marketId = marketId;
                this.directBucket1 = directBucket1;
                this.directBucket2 = directBucket2;
                this.directBucket3 = directBucket3;
                this.directBucket4 = directBucket4;
                this.directBucket5 = directBucket5;
                this.directBucket6 = directBucket6;
                this.agencyBucket1 = agencyBucket1;
                this.agencyBucket2 = agencyBucket2;
                this.agencyBucket3 = agencyBucket3;
                this.agencyBucket4 = agencyBucket4;
                this.agencyBucket5 = agencyBucket5;
                this.agencyBucket6 = agencyBucket6;
                this.updatedBy = updatedBy;
                this.companyId = companyId;
            }
            public int targetId {get;set;}
            public string marketId { get; set; }
            public int directBucket1 { get; set; }
            public int directBucket2 { get; set; }
            public int directBucket3 { get; set; }
            public int directBucket4 { get; set; }
            public int directBucket5 { get; set; }
            public int directBucket6 { get; set; }
            public int agencyBucket1 { get; set; }
            public int agencyBucket2 { get; set; }
            public int agencyBucket3 { get; set; }
            public int agencyBucket4 { get; set; }
            public int agencyBucket5 { get; set; }
            public int agencyBucket6 { get; set; }
            public string updatedBy { get; set; }
            public int companyId { get; set; }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public MarketTarget GetMarketTargets(string marketId, int companyId)
        {
            try
            {                
                DataSet marketTargetData;

                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    marketTargetData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("KPI_GETMARKETTARGETS",
                        Param.CreateParam("MARKETID", SqlDbType.VarChar, marketId),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, companyId)));
                }

                if (marketTargetData.Tables.Count == 0 || marketTargetData.Tables[0].Rows.Count == 0)
                {
                    return null;
                }

                MarketTarget marketTargets = new MarketTarget();
                marketTargets.targetId = Convert.ToInt32(marketTargetData.Tables[0].Rows[0]["TARGET_ID"]);
                marketTargets.marketId = marketId;
                marketTargets.directBucket1 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "DIRECT_BUCKET_1", 0));
                marketTargets.directBucket2 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "DIRECT_BUCKET_2", 0));
                marketTargets.directBucket3 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "DIRECT_BUCKET_3", 0));
                marketTargets.directBucket4 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "DIRECT_BUCKET_4", 0));
                marketTargets.directBucket5 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "DIRECT_BUCKET_5", 0));
                marketTargets.directBucket5 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "DIRECT_BUCKET_6", 0));
                marketTargets.agencyBucket1 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "AGENCY_BUCKET_1", 0));
                marketTargets.agencyBucket2 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "AGENCY_BUCKET_2", 0));
                marketTargets.agencyBucket3 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "AGENCY_BUCKET_3", 0));
                marketTargets.agencyBucket4 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "AGENCY_BUCKET_4", 0));
                marketTargets.agencyBucket5 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "AGENCY_BUCKET_5", 0));
                marketTargets.agencyBucket5 = Convert.ToInt32(IO.GetDataRowValue(marketTargetData.Tables[0].Rows[0], "AGENCY_BUCKET_6", 0));
                marketTargets.updatedBy = Convert.ToString(marketTargetData.Tables[0].Rows[0]["UPDATED_BY"]);
                marketTargets.companyId = Convert.ToInt32(marketTargetData.Tables[0].Rows[0]["COMPANY_ID"]);
                return marketTargets;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the targets for the Market.");
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public int AddMarketTargets(MarketTarget marketTargets)
        {
            try
            {
                List<SqlParameter> spParams = new List<SqlParameter>();
                spParams.Add(Param.CreateParam("MARKETID", SqlDbType.VarChar, marketTargets.marketId));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, marketTargets.companyId));
                spParams.Add(Param.CreateParam("UPDATEDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
                spParams.Add(Param.CreateParam("DIRECTBUCKET1", SqlDbType.Int, marketTargets.directBucket1));
                spParams.Add(Param.CreateParam("DIRECTBUCKET2", SqlDbType.Int, marketTargets.directBucket2));
                spParams.Add(Param.CreateParam("DIRECTBUCKET3", SqlDbType.Int, marketTargets.directBucket3));
                spParams.Add(Param.CreateParam("DIRECTBUCKET4", SqlDbType.Int, marketTargets.directBucket4));
                spParams.Add(Param.CreateParam("DIRECTBUCKET5", SqlDbType.Int, marketTargets.directBucket5));
                spParams.Add(Param.CreateParam("DIRECTBUCKET6", SqlDbType.Int, marketTargets.directBucket6));
                spParams.Add(Param.CreateParam("AGENCYBUCKET1", SqlDbType.Int, marketTargets.agencyBucket1));
                spParams.Add(Param.CreateParam("AGENCYBUCKET2", SqlDbType.Int, marketTargets.agencyBucket2));
                spParams.Add(Param.CreateParam("AGENCYBUCKET3", SqlDbType.Int, marketTargets.agencyBucket3));
                spParams.Add(Param.CreateParam("AGENCYBUCKET4", SqlDbType.Int, marketTargets.agencyBucket4));
                spParams.Add(Param.CreateParam("AGENCYBUCKET5", SqlDbType.Int, marketTargets.agencyBucket5));
                spParams.Add(Param.CreateParam("AGENCYBUCKET6", SqlDbType.Int, marketTargets.agencyBucket6));
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    marketTargets.targetId = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("KPI_ADDMARKETTARGETS", spParams)));
                }

                return marketTargets.targetId;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to add the targets for the Market.");
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void UpdateMarketTargets(MarketTarget marketTargets)
        {
            try
            {
                List<SqlParameter> spParams = new List<SqlParameter>();
                spParams.Add(Param.CreateParam("TARGETID", SqlDbType.Int, marketTargets.targetId));
                spParams.Add(Param.CreateParam("UPDATEDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
                spParams.Add(Param.CreateParam("DIRECTBUCKET1", SqlDbType.Int, marketTargets.directBucket1));
                spParams.Add(Param.CreateParam("DIRECTBUCKET2", SqlDbType.Int, marketTargets.directBucket2));
                spParams.Add(Param.CreateParam("DIRECTBUCKET3", SqlDbType.Int, marketTargets.directBucket3));
                spParams.Add(Param.CreateParam("DIRECTBUCKET4", SqlDbType.Int, marketTargets.directBucket4));
                spParams.Add(Param.CreateParam("DIRECTBUCKET5", SqlDbType.Int, marketTargets.directBucket5));
                spParams.Add(Param.CreateParam("DIRECTBUCKET6", SqlDbType.Int, marketTargets.directBucket6));
                spParams.Add(Param.CreateParam("AGENCYBUCKET1", SqlDbType.Int, marketTargets.agencyBucket1));
                spParams.Add(Param.CreateParam("AGENCYBUCKET2", SqlDbType.Int, marketTargets.agencyBucket2));
                spParams.Add(Param.CreateParam("AGENCYBUCKET3", SqlDbType.Int, marketTargets.agencyBucket3));
                spParams.Add(Param.CreateParam("AGENCYBUCKET4", SqlDbType.Int, marketTargets.agencyBucket4));
                spParams.Add(Param.CreateParam("AGENCYBUCKET5", SqlDbType.Int, marketTargets.agencyBucket5));
                spParams.Add(Param.CreateParam("AGENCYBUCKET6", SqlDbType.Int, marketTargets.agencyBucket6));
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("KPI_UPDATEMARKETTARGETS", spParams));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to update the targets for the Market.");
            }
        }
    }
}