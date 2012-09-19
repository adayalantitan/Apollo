#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class sales_reports_daily_kpi : System.Web.UI.Page
    {

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

        #region CalculateRemainingBusinessDays method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private int CalculateRemainingBusinessDays()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            return CountWorkDays(startDate, endDate, null);
        }
        #endregion

        #region CollectedToDate method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <param name="startOfMonth">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static double CollectedToDate(int companyId, string reportDate, string startOfMonth)
        {
            double amountCollected = 0.00;
            string storedProcName = (companyId == 1 ? "KPI_DATA_AMOUNTCOLLECTED" : "KPI_DATA_AMOUNTCOLLECTED_TOC");
            try
            {
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    amountCollected = Convert.ToDouble(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, reportDate),
                        Param.CreateParam("REPORTMONTH", SqlDbType.Int, Convert.ToDateTime(startOfMonth).Month),
                        Param.CreateParam("REPORTYEAR", SqlDbType.Int, Convert.ToDateTime(startOfMonth).Year))));
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

        #region GetAgedARBalances method
        /// <summary>TBD</summary>
        /// <param name="agingDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string GetAgedARBalances(string agingDate)
        {
            try
            {
                DataSet agedBalancesData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    agedBalancesData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("KPI_Data_HATB_Wrapper",
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(agingDate))));
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

        #region GetAgedARBalancesNew method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="agingDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static List<AgedTrialBalanceRecord> GetAgedARBalancesNew(int companyId, string agingDate)
        {
            Hashtable spParams = new Hashtable();
            List<AgedTrialBalanceRecord> agedTrialBalances = new List<AgedTrialBalanceRecord>();
            string storedProcName = (companyId == 1 ? "KPI_Data_HATB_Wrapper_new" : "KPI_Data_HATB_Wrapper_TOC");
            spParams.Add("REPORTDATE", Convert.ToDateTime(agingDate));
            try
            {
                DataSet agedBalancesData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    agedBalancesData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(agingDate))));
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

        #region GetAgedARDataJSON method
        /// <summary>TBD</summary>
        /// <param name="customerType">TBD</param>
        /// <param name="sales">TBD</param>
        /// <param name="debits">TBD</param>
        /// <param name="credits">TBD</param>
        /// <param name="payments">TBD</param>
        /// <param name="total">TBD</param>
        /// <returns>TBD</returns>
        public static string GetAgedARDataJSON(string customerType, double sales, double debits, double credits, double payments, double total)
        {
            string format = "{{customerType:'{0}',sales:{1},debits:{2},credits:{3},payments:{4},total:{5}}}";
            return string.Format(format, customerType, sales, debits, credits, payments, total);
        }
        #endregion

        #region GetBankingHolidaysForMonth method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        public static List<DateTime> GetBankingHolidaysForMonth(int companyId, DateTime reportDate)
        {
            List<DateTime> bankingHolidays = new List<DateTime>();
            DataSet bankingHolidayData = null;
            using (IO io = new IO(WebCommon.KPIConnectionString))
            {
                bankingHolidayData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("KPI_HOLIDAYS_GETBANKINGHOLIDAYS",
                    Param.CreateParam("MONTH", SqlDbType.Int, reportDate.Month),
                    Param.CreateParam("YEAR", SqlDbType.Int, reportDate.Year)));
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

        #region GetCustDataJSON method
        /// <summary>TBD</summary>
        /// <param name="customerType">TBD</param>
        /// <param name="customerCount">TBD</param>
        /// <param name="amountCollectable">TBD</param>
        /// <param name="balanceOver150">TBD</param>
        /// <param name="sales">TBD</param>
        /// <param name="credits">TBD</param>
        /// <param name="debits">TBD</param>
        /// <returns>TBD</returns>
        public static string GetCustDataJSON(string customerType, int customerCount, double amountCollectable, double balanceOver150, double sales, double credits, double debits)
        {
            string format = "{{customerType:'{0}',customerTypeCount:{1},amountCollectable:{2},balanceOver150:{3},salesPast90:{4},creditsPast90:{5},debitsPast90:{6}}}";
            return string.Format(format, customerType, customerCount, amountCollectable, balanceOver150, sales, credits, debits);
        }
        #endregion

        #region GetCustomerTypeBreakdown method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string GetCustomerTypeBreakdown(int companyId, string reportDate)
        {
            StringBuilder customerBreakdownJSON = new StringBuilder();
            string storedProcName = (companyId == 1 ? "KPI_Data_BreakdownByCustType" : "KPI_Data_BreakdownByCustType_TOC");
            Hashtable spParams = new Hashtable();
            spParams.Add("REPORTDATE", Convert.ToDateTime(reportDate));
            try
            {
                DataSet customerBreakdownData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    customerBreakdownData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (customerBreakdownData == null)
                {
                    throw new Exception("No Customer Breakdown data was retrieved.");
                }
                foreach (DataRow row in customerBreakdownData.Tables[0].Rows)
                {
                    if (!String.IsNullOrEmpty(customerBreakdownJSON.ToString()))
                    {
                        customerBreakdownJSON.Append(",");
                    }
                    else
                    {
                        customerBreakdownJSON.Append("[");
                    }
                    customerBreakdownJSON.Append(GetCustDataJSON(Convert.ToString(IO.GetDataRowValue(row, "Customer Type", ""))
                    , Convert.ToInt32(IO.GetDataRowValue(row, "Count of Customer Type", "0"))
                    , Convert.ToDouble(IO.GetDataRowValue(row, "Amount Collectable", "0.00"))
                    , Convert.ToDouble(IO.GetDataRowValue(row, "Balance Over 150 Days", "0.00"))
                    , Convert.ToDouble(IO.GetDataRowValue(row, "Sales Past 90 Days", "0.00"))
                    , Convert.ToDouble(IO.GetDataRowValue(row, "Credits Past 90 Days", "0.00"))
                    , Convert.ToDouble(IO.GetDataRowValue(row, "Debits Past 90 Days", "0.00"))
                    ));
                }
                if (!String.IsNullOrEmpty(customerBreakdownJSON.ToString()))
                {
                    customerBreakdownJSON.Append("]");
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Customer Breakdown.");
            }
            return customerBreakdownJSON.ToString();
        }
        #endregion

        #region GetMarketBreakdown method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static List<MarketBreakdownRecord> GetMarketBreakdown(int companyId, string reportDate)
        {
            List<MarketBreakdownRecord> marketBreakdown = new List<MarketBreakdownRecord>();
            string storedProcName = (companyId == 1 ? "KPI_Data_BreakdownByMarket" : "KPI_Data_BreakdownByMarket_TOC");
            try
            {
                DataSet marketBreakdownData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    marketBreakdownData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
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

        #region GetPreviousBusinessDay method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private DateTime GetPreviousBusinessDay()
        {
            return DateTime.Now;
        }
        #endregion

        #region GetSalesByDocAndMonthJSON method
        /// <summary>TBD</summary>
        /// <param name="docType">TBD</param>
        /// <param name="month">TBD</param>
        /// <param name="amount">TBD</param>
        /// <returns>TBD</returns>
        public static string GetSalesByDocAndMonthJSON(string docType, DateTime month, double amount)
        {
            string format = "{{documentType:'{0}',month:'{1}',amount:{2}}}";
            return string.Format(format, docType, month.ToShortDateString(), amount);
        }
        #endregion

        #region GetSalesByDocTypeAndMonth method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string GetSalesByDocTypeAndMonth(int companyId, string reportDate)
        {
            StringBuilder salesByDoc = new StringBuilder();
            string storedProcName = (companyId == 1 ? "KPI_Data_SalesDataByDocTypeAndMonth" : "KPI_Data_SalesDataByDocTypeAndMonth_TOC");
            Hashtable spParams = new Hashtable();
            spParams.Add("REPORTDATE", Convert.ToDateTime(reportDate));
            try
            {
                DataSet salesByDocData = null;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    salesByDocData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (salesByDocData == null)
                {
                    throw new Exception("No data was retrieved for Previous Month Sales Data.");
                }
                foreach (DataRow row in salesByDocData.Tables[0].Rows)
                {
                    if (!String.IsNullOrEmpty(salesByDoc.ToString()))
                    {
                        salesByDoc.Append(",");
                    }
                    else
                    {
                        salesByDoc.Append("[");
                    }
                    salesByDoc.Append(GetSalesByDocAndMonthJSON(Convert.ToString(IO.GetDataRowValue(row, "Document Type", "")), Convert.ToDateTime(IO.GetDataRowValue(row, "Period", "")), Convert.ToDouble(IO.GetDataRowValue(row, "Amount", 0.00))));
                }
                if (!String.IsNullOrEmpty(salesByDoc.ToString()))
                {
                    salesByDoc.Append("]");
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Sales Data for Previous Months.");
            }
            return salesByDoc.ToString();
        }
        #endregion

        #region GetSalesData method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string GetSalesData(int companyId, string reportDate)
        {
            try
            {
                StringBuilder salesByDoc = new StringBuilder();
                DataSet salesData = null;
                string storedProcName = (companyId == 1 ? "KPI_Data_SalesDataForMonth" : "KPI_Data_SalesDataForMonth_TOC");
                Hashtable spParams = new Hashtable();
                spParams.Add("REPORTDATE", Convert.ToDateTime(reportDate));
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    salesData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, Convert.ToDateTime(reportDate))));
                }
                if (salesData == null)
                {
                    throw new Exception("No Sales Data could be retrieved.");
                }
                foreach (DataRow row in salesData.Tables[0].Rows)
                {
                    if (!String.IsNullOrEmpty(salesByDoc.ToString()))
                    {
                        salesByDoc.Append(",");
                    }
                    else
                    {
                        salesByDoc.Append("[");
                    }
                    salesByDoc.Append(GetSalesDataJSON(Convert.ToDateTime(IO.GetDataRowValue(row, "Document Date", "")), Convert.ToDouble(IO.GetDataRowValue(row, "Amount", 0.00))));
                }
                if (!String.IsNullOrEmpty(salesByDoc.ToString()))
                {
                    salesByDoc.Append("]");
                }
                return salesByDoc.ToString();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve Sales Data.");
            }
        }
        #endregion

        #region GetSalesDataJSON method
        /// <summary>TBD</summary>
        /// <param name="docDate">TBD</param>
        /// <param name="amount">TBD</param>
        /// <returns>TBD</returns>
        public static string GetSalesDataJSON(DateTime docDate, double amount)
        {
            string format = "{{documentDate:'{0}',amount:{1}}}";
            return string.Format(format, docDate.ToShortDateString(), amount);
        }
        #endregion

        #region GetWorkDays method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="monthStartDateStr">TBD</param>
        /// <param name="reportDateStr">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string GetWorkDays(int companyId, string monthStartDateStr, string reportDateStr)
        {
            string workDaysJSON = "[{{workingDays:{0},workingDaysElapsed:{1}}}]";
            int workingDaysInMonth = 0;
            int workingDaysElapsed = 0;
            DateTime monthStartDate, reportDate, monthEndDate;
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
            monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);
            workingDaysInMonth = CountWorkDays(monthStartDate, monthEndDate, bankingHolidays);
            workingDaysElapsed = CountWorkDays(monthStartDate, reportDate, bankingHolidays);
            return string.Format(workDaysJSON, workingDaysInMonth, workingDaysElapsed);
        }
        #endregion

        #region LoadReportData method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportMonth">TBD</param>
        /// <param name="reportYear">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string LoadReportData(int companyId, int reportMonth, int reportYear)
        {
            string reportData = string.Empty;
            try
            {
                DataSet kpiReportData;
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    kpiReportData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("KPI_REPORT_LOADREPORTDATA",
                        Param.CreateParam("KPIREPORTMONTH", SqlDbType.Int, reportMonth),
                        Param.CreateParam("KPIREPORTYEAR", SqlDbType.Int, reportYear)));
                }
                if (kpiReportData == null)
                {
                    throw new Exception("No Data could be retrieved.");
                }
                if (kpiReportData.Tables[0].Rows.Count == 0)
                {
                    return reportData;
                }
                reportData = Convert.ToString(kpiReportData.Tables[0].Rows[0]["KPI_REPORT_DATA"] ?? "");
                return string.Format("[{0}]", reportData);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while loading the report Data.");
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

        #region RetrieveAgedARBalancesJSON method
        /// <summary>TBD</summary>
        /// <param name="agedBalanceData">TBD</param>
        /// <returns>TBD</returns>
        public static string RetrieveAgedARBalancesJSON(DataSet agedBalanceData)
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
            StringBuilder agedBalancesJSON = new StringBuilder();
            agedBalancesJSON.Append("[");
            agedBalancesJSON.Append(GetAgedARDataJSON("Direct", directSales, directDebits, directCredits, directPayments, directTotal));
            agedBalancesJSON.Append(",");
            agedBalancesJSON.Append(GetAgedARDataJSON("Large Agency", largeSales, largeDebits, largeCredits, largePayments, largeTotal));
            agedBalancesJSON.Append(",");
            agedBalancesJSON.Append(GetAgedARDataJSON("Small Agency", smallSales, smallDebits, smallCredits, smallPayments, smallTotal));
            agedBalancesJSON.Append("]");
            return agedBalancesJSON.ToString();
        }
        #endregion

        #region RetrieveBadDebtAndARBalance method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static string RetrieveBadDebtAndARBalance(int companyId, string reportDate)
        {
            string format = "[{{badDebtAmount:{0},arBalance:{1}}}]";
            try
            {
                DataSet badDebtArBalData = null;
                string storedProcName = (companyId == 1 ? "KPI_Data_BadDebt_ARBalance" : "KPI_Data_BadDebt_ARBalance_TOC");
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    badDebtArBalData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("REPORTDATE", SqlDbType.Date, reportDate)));
                }
                if (badDebtArBalData == null)
                {
                    throw new Exception("No data was retrieved for Bad Debt and AR Balance.");
                }
                return string.Format(format
                    , Convert.ToDouble(IO.GetDataRowValue(badDebtArBalData.Tables[0].Rows[0], "Bad Debt", "0.00"))
                    , Convert.ToDouble(IO.GetDataRowValue(badDebtArBalData.Tables[0].Rows[0], "Period AR Balance", "0.00")));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve Bad Debt and AR Balances.");
            }
        }
        #endregion

        #region SaveReportData method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="reportMonth">TBD</param>
        /// <param name="reportYear">TBD</param>
        /// <param name="reportData">TBD</param>
        [System.Web.Services.WebMethod]
        public static void SaveReportData(int companyId, int reportMonth, int reportYear, string reportData)
        {
            try
            {
                using (IO io = new IO(WebCommon.KPIConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("KPI_REPORT_UPSERTREPORTDATA",
                        Param.CreateParam("KPIREPORTMONTH", SqlDbType.Int, reportMonth),
                        Param.CreateParam("KPIREPORTYEAR", SqlDbType.Int, reportYear),
                        Param.CreateParam("KPIREPORTCREATEDBY", SqlDbType.VarChar, Security.GetCurrentUserId),
                        Param.CreateParam("KPIREPORTDATA", SqlDbType.VarChar, reportData)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to save the report Data.");
            }
        }
        #endregion

    }

}
