#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Ionic.Zip;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    ///     Helper class to generate the
    ///     Accounting Commission by AE Excel Report
    /// </summary>
    public class AccountingCommissionByAEReportBuilder : CommissionReportBuilder
    {

        #region ReportFields enumeration
        private enum ReportFields
        {
            Advertiser = 1, AdvertiserID, Agency,
            Program, ProfitCenter, ContractNumber, LineNumber,
            BillStartDate, ContractEntryDate, ContractLineDateStart, ContractLineDateEnd,
            NationalOrLocal, Multimarket, Total_last_year, Total_current_year,
            YearToDate_current_year, NewOrRenewal, NumberOfAE, Package,
            SplitPercentage, SplitAmount, RenewAmount, NewAmount,
            CommissionPercentNew, CommissionPercentRenewal,
            January, February, March, April, May, June,
            July, August, September, October, November, December,
            CommissionAmount, CommissionContractTotal,
            ContractLineBillAmount, AECommissionPercent,
        }
        #endregion

        #region TotalFields enumeration
        private enum TotalFields
        {
            SplitAmount = 20, RenewAmount, NewAmount,
            January = 25, February, March, April, May, June,
            July, August, September, October, November, December,
            CommissionAmount = 37,
        }
        #endregion

        #region Member variables
        /// <summary>TBD</summary>
        private string[] AEFilterList;
        /// <summary>TBD</summary>
        private string[] AENameList;
        /// <summary>TBD</summary>
        private string[] AEReports;
        /// <summary>TBD</summary>
        private int companyId;
        /// <summary>TBD</summary>
        private bool[] hasData;
        /// <summary>The name of the XML Element containing the Report Headers template</summary>
        public const string REPORT_HEADERS_TEMPLATE_NAME = "ReportHeaders";
        /// <summary>The name of the XML Element containing the Report Row template</summary>
        public const string REPORT_ROW_TEMPLATE_NAME = "ReportRow";
        /// <summary>Name of the Stored Procedure to retrieve report data</summary>
        public const string REPORT_STORED_PROC_NAME = "COMMISSIONREPORTS_GETACCOUNTINGREPORT";
        /// <summary>The name of the XML Element containing the Report template</summary>
        public const string REPORT_TEMPLATE_NAME = "AccountingCommissionByAE";
        /// <summary>The name of the XML Element containing the Report Total Row template</summary>
        public const string REPORT_TOTAL_ROW_TEMPLATE_NAME = "ReportTotalRow";
        /// <summary>TBD</summary>
        private string reportAttachmentName = "({0}) to ({1}) AccountingCommissionByAE_Consolidated_{2}.zip";
        /// <summary>TBD</summary>
        private string ReportFileName = "{0} AC - {1} ( as of {2}).xls";
        #endregion

        #region Parameterized constructor (selectedAEs, startDate, endDate, companyId)
        /// <summary>AccountingCommissionByAEReportBuilder constructor</summary>
        /// <param name="selectedAEs">List of AEs to generate the report for</param>
        /// <param name="startDate">TBD</param>
        /// <param name="endDate">TBD</param>
        /// <param name="companyId">TBD</param>
        public AccountingCommissionByAEReportBuilder(string[] selectedAEs, DateTime startDate, DateTime endDate, int companyId)
            : base(REPORT_TEMPLATE_NAME, startDate, endDate, ReportOption.AllAEs, companyId)
        {
            this.AEFilterList = selectedAEs;
            this.companyId = companyId;
        }
        #endregion

        #region BuildReportHeaders method
        /// <summary>Builds the headers of the Excel report</summary>
        /// <param name="aeName">TBD</param>
        public void BuildReportHeaders(string aeName)
        {
            if (ReportYear < 2002)
            {
                throw new ArgumentOutOfRangeException("year", ReportYear, "Year cannot be less than 2002");
            }
            string[] values = new string[3];
            values[0] = aeName;
            values[1] = Convert.ToString(ReportYear);
            values[2] = Convert.ToString(ReportYear - 1);
            base.BuildRow(REPORT_HEADERS_TEMPLATE_NAME, values);
        }
        #endregion

        #region GenerateReport method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public MemoryStream GenerateReport()
        {
            MemoryStream reportStream = new MemoryStream();
            string report = string.Empty;
            try
            {
                GetReportData(REPORT_STORED_PROC_NAME);
                InnerGenerateReport();
                using (ZipFile zip = new ZipFile())
                {
                    string fileName;
                    for (int i = 0; i < this.AEReports.Length; i++)
                    {
                        if (hasData[i])
                        {
                            fileName = string.Format(this.ReportFileName, ReportYear, this.AENameList[i], EndDate.ToShortDateString().Replace("/", "-"));
                            zip.AddEntry(fileName, ASCIIEncoding.ASCII.GetBytes(AEReports[i]));
                        }
                    }
                    zip.Save(reportStream);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new InvalidOperationException("An error occurred while generating the Accounting Commission report.", ex);
            }
            return reportStream;
        }
        #endregion

        #region InnerGenerateReport method
        /// <summary>TBD</summary>
        private void InnerGenerateReport()
        {
            if (ReportData == null)
            {
                throw new InvalidOperationException("ReportData cannot be null");
            }
            if (ReportData.Tables[0].Rows.Count == 0)
            {
                //No data for this report
                return;
            }
            AEReports = new string[this.AEFilterList.Length];
            AENameList = new string[this.AEFilterList.Length];
            hasData = new bool[this.AEFilterList.Length];
            string filterExpression = string.Empty;
            DataTable table = new DataTable();
            for (int i = 0; i < this.AEFilterList.Length; i++)
            {
                //Process Data and Build Report
                filterExpression = "AE_Id = '" + this.AEFilterList[i].Replace("'", "''") + "'";
                table = new DataTable();
                table = ReportData.Tables[0].Clone();
                foreach (DataRow row in ReportData.Tables[0].Select(filterExpression))
                {
                    table.ImportRow(row);
                }
                hasData[i] = (table.Rows.Count > 0);
                AENameList[i] = (hasData[i]) ? Convert.ToString(table.Rows[0]["Salesman"]) : string.Empty;
                if (hasData[i])
                {
                    StartReport();
                    BuildReportHeaders(this.AENameList[i]);
                    foreach (DataRow row in table.Rows)
                    {
                        BuildReportRow(REPORT_ROW_TEMPLATE_NAME, row, Enum.GetNames(typeof(ReportFields)));
                    }
                    //Build Summary Row
                    BuildReportTotalRow(REPORT_TOTAL_ROW_TEMPLATE_NAME, table, Enum.GetNames(typeof(TotalFields)));
                    FinalizeReport();
                    AEReports[i] = base.GetReportOutput();
                }
                else
                {
                    AEReports[i] = string.Empty;
                }
            }
        }
        #endregion

        #region ReportAttachmentName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ReportAttachmentName
        {
            get
            {
                return string.Format(reportAttachmentName, StartDate.ToShortDateString().Replace("/", "-"), EndDate.ToShortDateString().Replace("/", "-"), (CompanyId == 1 ? "USA" : "CAN"));
            }
        }
        #endregion

    }

    /// <summary>
    ///     Helper class to generate the
    ///     Accounting Commission Excel Report
    /// </summary>
    public class AccountingCommissionReportBuilder : CommissionReportBuilder
    {

        #region ReportFields enumeration
        private enum ReportFields
        {
            Salesman, Advertiser, AdvertiserID, Agency,
            Program, ProfitCenter, ContractNumber, LineNumber,
            BillStartDate, ContractEntryDate, ContractLineDateStart, ContractLineDateEnd,
            NationalOrLocal, Multimarket, Total_last_year, Total_current_year,
            YearToDate_current_year, NewOrRenewal, NumberOfAE, Package,
            SplitPercentage, SplitAmount, RenewAmount, NewAmount,
            CommissionPercentNew, CommissionPercentRenewal,
            January, February, March, April, May, June,
            July, August, September, October, November, December,
            CommissionAmount, CommissionContractTotal,
            ContractLineBillAmount, AECommissionPercent,
        }
        #endregion

        #region TotalFields enumeration
        private enum TotalFields
        {
            SplitAmount = 21, RenewAmount, NewAmount,
            January = 26, February, March, April, May, June,
            July, August, September, October, November, December,
            CommissionAmount = 38,
        }
        #endregion

        #region Member variables
        /// <summary>The name of the XML Element containing the Report Headers template</summary>
        public const string REPORT_HEADERS_TEMPLATE_NAME = "ReportHeaders";
        /// <summary>The name of the XML Element containing the Report Row template</summary>
        public const string REPORT_ROW_TEMPLATE_NAME = "ReportRow";
        /// <summary>Name of the Stored Procedure to retrieve report data</summary>
        public const string REPORT_STORED_PROC_NAME = "COMMISSIONREPORTS_GETACCOUNTINGREPORT";
        /// <summary>The name of the XML Element containing the Report template</summary>
        public const string REPORT_TEMPLATE_NAME = "AccountingCommission";
        /// <summary>The name of the XML Element containing the Report Total Row template</summary>
        public const string REPORT_TOTAL_ROW_TEMPLATE_NAME = "ReportTotalRow";
        /// <summary>TBD</summary>
        private string reportAttachmentName = "({0}) to ({1}) AccountingCommission_Consolidated_{2}.zip";
        /// <summary>TBD</summary>
        private string reportFileName = "{0} ({1}) to ({2}) AccountingCommission_Consolidated.xls";
        #endregion

        #region Parameterized constructor (startDate, endDate, requestedReportOption, companyId)
        /// <summary>AccountingCommissionReportBuilder constructor</summary>
        /// <param name="startDate">TBD</param>
        /// <param name="endDate">TBD</param>
        /// <param name="requestedReportOption">TBD</param>
        /// <param name="companyId">TBD</param>
        public AccountingCommissionReportBuilder(DateTime startDate, DateTime endDate, ReportOption requestedReportOption, int companyId)
            : base(REPORT_TEMPLATE_NAME, startDate, endDate, requestedReportOption, companyId)
        {
        }
        #endregion

        #region BuildReportHeaders method
        /// <summary>Builds the headers of the Excel report</summary>
        public void BuildReportHeaders()
        {
            if (ReportYear < 2002)
            {
                throw new ArgumentOutOfRangeException("year", ReportYear, "Year cannot be less than 2002");
            }
            string[] values = new string[2];
            values[0] = Convert.ToString(ReportYear - 1);
            values[1] = Convert.ToString(ReportYear);
            base.BuildRow(REPORT_HEADERS_TEMPLATE_NAME, values);
        }
        #endregion

        #region GenerateReport method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public MemoryStream GenerateReport()
        {
            string report = string.Empty;
            MemoryStream reportStream = new MemoryStream();
            try
            {
                reportFileName = string.Format(reportFileName, ReportYear, StartDate.ToShortDateString().Replace("/", "-"), EndDate.ToShortDateString().Replace("/", "-"));
                GetReportData(REPORT_STORED_PROC_NAME);
                InnerGenerateReport();
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry(reportFileName, ASCIIEncoding.ASCII.GetBytes(GetReportOutput()));
                    zip.Save(reportStream);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new InvalidOperationException("An error occurred while generating the Accounting Commission report.", ex);
            }
            return reportStream;
        }
        #endregion

        #region GetReportData method
        /// <summary>TBD</summary>
        /// <param name="storedProc">TBD</param>
        public override void GetReportData(string storedProc)
        {
            List<SqlParameter> reportParams = new List<SqlParameter>();
            reportParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, StartDate));
            reportParams.Add(Param.CreateParam("ENDDATE", SqlDbType.Date, EndDate));
            reportParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, CompanyId));
            switch (this.CommissionReportOption)
            {
                case ReportOption.ActiveAEsOnly:
                    reportParams.Add(Param.CreateParam("WANTACTIVEONLY", SqlDbType.Int, -1));
                    break;
                case ReportOption.ActiveAEsWithoutHidden:
                    reportParams.Add(Param.CreateParam("WANTACTIVEONLY", SqlDbType.Int, -1));
                    reportParams.Add(Param.CreateParam("NOHIDDEN", SqlDbType.Int, -1));
                    break;
                default: break;
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc(storedProc, reportParams))
                {
                    cmd.CommandTimeout = 200;
                    this.ReportData = io.ExecuteDataSetQuery(cmd);
                }
            }
        }
        #endregion

        #region InnerGenerateReport method
        /// <summary>TBD</summary>
        private void InnerGenerateReport()
        {
            if (ReportData == null)
            {
                throw new InvalidOperationException("ReportData cannot be null");
            }
            if (ReportData.Tables[0].Rows.Count == 0)
            {
                //No data for this report
                return;
            }
            StartReport();
            BuildReportHeaders();
            //Process Data and Build Report
            foreach (DataRow row in ReportData.Tables[0].Rows)
            {
                BuildReportRow(REPORT_ROW_TEMPLATE_NAME, row, Enum.GetNames(typeof(ReportFields)));
            }
            //Build Summary Row
            BuildReportTotalRow(REPORT_TOTAL_ROW_TEMPLATE_NAME, ReportData.Tables[0], Enum.GetNames(typeof(TotalFields)));
            FinalizeReport();
        }
        #endregion

        #region ReportAttachmentName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ReportAttachmentName
        {
            get
            {
                return string.Format(reportAttachmentName, StartDate.ToShortDateString().Replace("/", "-"), EndDate.ToShortDateString().Replace("/", "-"), (CompanyId == 1 ? "USA" : "CAN"));
            }
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class AECommissionByMarketReportBuilder : CommissionReportBuilder
    {

        #region ReportFields enumeration
        private enum ReportFields
        {
            AE, Market, ProfitCenter, January, February, March,
            April, May, June, July, August,
            September, October, November, December, Total,
        }
        #endregion

        #region TotalFields enumeration
        private enum TotalFields
        {
            January = 4, February, March, April,
            May, June, July, August, September,
            October, November, December, Total,
        }
        #endregion

        #region Member variables
        /// <summary>TBD</summary>
        private DateTime asOfDate;
        /// <summary>The name of the XML Element containing the Report Headers template</summary>
        public const string REPORT_HEADERS_TEMPLATE_NAME = "ReportHeaders";
        /// <summary>The name of the XML Element containing the Report Row template</summary>
        public const string REPORT_ROW_TEMPLATE_NAME = "ReportRow";
        /// <summary>Name of the Stored Procedure to retrieve report data</summary>
        public const string REPORT_STORED_PROC_NAME = "COMMISSIONREPORTS_GETAECOMMISSIONBYMARKET";
        /// <summary>The name of the XML Element containing the Report template</summary>
        public const string REPORT_TEMPLATE_NAME = "AECommissionByMarket";
        /// <summary>The name of the XML Element containing the Report Total Row template</summary>
        public const string REPORT_TOTAL_ROW_TEMPLATE_NAME = "ReportTotalRow";
        /// <summary>TBD</summary>
        private string reportAttachmentName = "AECommissionByMarket_as_of_{0}_{1}.zip";
        /// <summary>TBD</summary>
        private string reportFileName = "AECommissionByMarket_as_of_{0}.xls";
        #endregion

        #region Parameterized constructor (asOfDate, requestedReportOption, companyId)
        /// <summary>AccountingCommissionReportBuilder constructor</summary>
        /// <param name="asOfDate">TBD</param>
        /// <param name="requestedReportOption">TBD</param>
        /// <param name="companyId">TBD</param>
        public AECommissionByMarketReportBuilder(DateTime asOfDate, ReportOption requestedReportOption, int companyId)
            : base(REPORT_TEMPLATE_NAME, DateTime.Now, DateTime.Now, requestedReportOption, companyId)
        {
            this.asOfDate = asOfDate;
        }
        #endregion

        #region BuildReportHeaders method
        /// <summary>Builds the headers of the Excel report</summary>
        public void BuildReportHeaders()
        {
            if (ReportYear < 2002)
            {
                throw new ArgumentOutOfRangeException("year", ReportYear, "Year cannot be less than 2002");
            }
            string[] values = new string[1];
            values[0] = Convert.ToString(ReportYear);
            base.BuildRow(REPORT_HEADERS_TEMPLATE_NAME, values);
        }
        #endregion

        #region GenerateReport method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public MemoryStream GenerateReport()
        {
            string report = string.Empty;
            MemoryStream reportStream = new MemoryStream();
            try
            {
                reportFileName = string.Format(reportFileName, this.asOfDate.ToShortDateString().Replace("/", "-"));
                GetReportData(REPORT_STORED_PROC_NAME);
                InnerGenerateReport();
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry(reportFileName, ASCIIEncoding.ASCII.GetBytes(GetReportOutput()));
                    zip.Save(reportStream);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new InvalidOperationException("An error occurred while generating the Accounting Commission report.", ex);
            }
            return reportStream;
        }
        #endregion

        #region GetReportData method
        /// <summary>Executes the stored procedure that retrieves the report data</summary>
        /// <param name="storedProc">The name of the stored procedure to execute</param>
        public override void GetReportData(string storedProc)
        {
            List<SqlParameter> reportParams = new List<SqlParameter>();
            reportParams.Add(Param.CreateParam("ASOFDATE", SqlDbType.Date, this.asOfDate));
            reportParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, CompanyId));
            switch (this.CommissionReportOption)
            {
                case ReportOption.ActiveAEsOnly:
                    reportParams.Add(Param.CreateParam("WANTACTIVEONLY", SqlDbType.Int, -1));
                    break;
                case ReportOption.ActiveAEsWithoutHidden:
                    reportParams.Add(Param.CreateParam("WANTACTIVEONLY", SqlDbType.Int, -1));
                    reportParams.Add(Param.CreateParam("NOHIDDEN", SqlDbType.Int, -1));
                    break;
                default: break;
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc(storedProc, reportParams))
                {
                    cmd.CommandTimeout = 200;
                    this.ReportData = io.ExecuteDataSetQuery(cmd);
                }
            }
        }
        #endregion

        #region InnerGenerateReport method
        /// <summary>TBD</summary>
        private void InnerGenerateReport()
        {
            if (ReportData == null)
            {
                throw new InvalidOperationException("ReportData cannot be null");
            }
            if (ReportData.Tables[0].Rows.Count == 0)
            {
                //No data for this report
                return;
            }
            StartReport();
            BuildReportHeaders();
            //Process Data and Build Report
            foreach (DataRow row in ReportData.Tables[0].Rows)
            {
                BuildReportRow(REPORT_ROW_TEMPLATE_NAME, row, Enum.GetNames(typeof(ReportFields)));
            }
            //Build Summary Row
            BuildReportTotalRow(REPORT_TOTAL_ROW_TEMPLATE_NAME, ReportData.Tables[0], Enum.GetNames(typeof(TotalFields)));
            FinalizeReport();
        }
        #endregion

        #region ReportAttachmentName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ReportAttachmentName
        {
            get
            {
                return string.Format(reportAttachmentName, this.asOfDate.ToShortDateString().Replace("/", "-"), (CompanyId == 1 ? "USA" : "CAN"));
            }
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class CommissionAnalysisReportBuilder : CommissionReportBuilder
    {

        #region ReportFields enumeration
        private enum ReportFields
        {
            AEName, StartDate, AEMarket, AEBillingsAsOf,
            CommissionsAsOf, Draw, OverUnderDrawAsOf,
            FullYearAEBillingsAsOf, FullYearCommissionsAsOf,
            FullYearOverUnderDrawAsOf, FullYearAEBillings,
            FullYearCommissions, FullYearOverUnderDraw,
        }
        #endregion

        #region TotalFields enumeration
        private enum TotalFields
        {
            AEBillingsAsOf = 4, CommissionsAsOf,
            Draw, OverUnderDrawAsOf,
            FullYearAEBillingsAsOf, FullYearCommissionsAsOf,
            FullYearOverUnderDrawAsOf, FullYearAEBillings,
            FullYearCommissions, FullYearOverUnderDraw,
        }
        #endregion

        #region Member variables
        /// <summary>TBD</summary>
        private DateTime asOfDate;
        /// <summary>TBD</summary>
        private int asOfPreviousYear;
        /// <summary>TBD</summary>
        private int asOfYear;
        /// <summary>TBD</summary>
        private int companyId;
        /// <summary>TBD</summary>
        public const string REPORT_HEADERS_TEMPLATE_NAME = "ReportHeaders";
        /// <summary>TBD</summary>
        public const string REPORT_ROW_TEMPLATE_NAME = "ReportRow";
        /// <summary>TBD</summary>
        public const string REPORT_STORED_PROC_NAME = "COMMISSIONREPORTS_COMMISSIONANALYSIS";
        /// <summary>TBD</summary>
        public const string REPORT_TEMPLATE_NAME = "CommissionAnalysis";
        /// <summary>TBD</summary>
        public const string REPORT_TOTAL_ROW_TEMPLATE_NAME = "ReportTotalRow";
        /// <summary>TBD</summary>
        public const string REPORT_UNDER_DRAW_TOTAL_ROW_TEMPLATE_NAME = "TotalUnderDrawRow";
        /// <summary>TBD</summary>
        private string reportAttachmentName = "CommissionAnalysis_Report as of {0}_{1}.zip";
        /// <summary>TBD</summary>
        private string reportFileName = "CommissionAnalysis_Report as of {0}.xls";
        #endregion

        #region Parameterized constructor (asOfDate, requestedReportOption, companyId)
        /// <summary>TBD</summary>
        /// <param name="asOfDate">TBD</param>
        /// <param name="requestedReportOption">TBD</param>
        /// <param name="companyId">TBD</param>
        public CommissionAnalysisReportBuilder(DateTime asOfDate, ReportOption requestedReportOption, int companyId)
            : base(REPORT_TEMPLATE_NAME, DateTime.Now, DateTime.Now, requestedReportOption, companyId)
        {
            this.asOfDate = asOfDate;
            this.asOfYear = asOfDate.Year;
            this.asOfPreviousYear = asOfYear - 1;
            this.companyId = companyId;
        }
        #endregion

        #region BuildReportHeaders method
        /// <summary>Builds the headers of the Excel report</summary>
        public void BuildReportHeaders()
        {
            string[] values = new string[3];
            values[0] = this.asOfDate.ToShortDateString();
            values[1] = Convert.ToString(this.asOfYear);
            values[2] = Convert.ToString(this.asOfPreviousYear);
            base.BuildRow(REPORT_HEADERS_TEMPLATE_NAME, values);
        }
        #endregion

        #region BuildUnderDrawTotalRow method
        /// <summary>TBD</summary>
        /// <param name="templateName">TBD</param>
        /// <param name="reportData">TBD</param>
        /// <param name="field">TBD</param>
        public void BuildUnderDrawTotalRow(string templateName, DataTable reportData, string field)
        {
            string sum = string.Format("SUM([{0}])", field);
            string filter = string.Format("[{0}] < 0", field);
            string[] values = new string[1];
            values[0] = Convert.ToString(reportData.Compute(sum, filter)); //"SUM([" + field + "]", filter)); // string.Format("[{0}] < 0", field)));
            base.BuildRow(templateName, values);
        }
        #endregion

        #region GenerateReport method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public MemoryStream GenerateReport()
        {
            string report = string.Empty;
            MemoryStream reportStream = new MemoryStream();
            try
            {
                reportFileName = string.Format(reportFileName, this.asOfDate.ToShortDateString().Replace("/", "-"));
                GetReportData(REPORT_STORED_PROC_NAME);
                InnerGenerateReport();
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry(reportFileName, ASCIIEncoding.ASCII.GetBytes(GetReportOutput()));
                    zip.Save(reportStream);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new InvalidOperationException("An error occurred while generating the Commissions Analysis report.", ex);
            }
            return reportStream;
        }
        #endregion

        #region GetReportData method
        /// <summary>Executes the stored procedure that retrieves the report data</summary>
        /// <param name="storedProc">The name of the stored procedure to execute</param>
        public override void GetReportData(string storedProc)
        {
            List<SqlParameter> reportParams = new List<SqlParameter>();
            reportParams.Add(Param.CreateParam("ASOFDATE", SqlDbType.Date, this.asOfDate));
            reportParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, CompanyId));
            switch (this.CommissionReportOption)
            {
                case ReportOption.ActiveAEsOnly:
                    reportParams.Add(Param.CreateParam("WANTACTIVEONLY", SqlDbType.Int, -1));
                    break;
                case ReportOption.ActiveAEsWithoutHidden:
                    reportParams.Add(Param.CreateParam("WANTACTIVEONLY", SqlDbType.Int, -1));
                    reportParams.Add(Param.CreateParam("NOHIDDEN", SqlDbType.Int, -1));
                    break;
                default: break;
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc(storedProc, reportParams))
                {
                    cmd.CommandTimeout = 200;
                    this.ReportData = io.ExecuteDataSetQuery(cmd);
                }
            }
        }
        #endregion

        #region InnerGenerateReport method
        /// <summary>TBD</summary>
        private void InnerGenerateReport()
        {
            if (ReportData == null)
            {
                throw new InvalidOperationException("ReportData cannot be null");
            }
            if (ReportData.Tables[0].Rows.Count == 0)
            {
                //No data for this report
                return;
            }
            StartReport();
            BuildReportHeaders();
            //Process Data and Build Report
            foreach (DataRow row in ReportData.Tables[0].Rows)
            {
                BuildReportRow(REPORT_ROW_TEMPLATE_NAME, row, Enum.GetNames(typeof(ReportFields)));
            }
            //Build Summary Row
            //BuildReportTotalRow(REPORT_TOTAL_ROW_TEMPLATE_NAME, ReportData.Tables[0], Enum.GetNames(typeof(TotalFields)));
            BuildUnderDrawTotalRow(REPORT_UNDER_DRAW_TOTAL_ROW_TEMPLATE_NAME, ReportData.Tables[0], Enum.GetName(typeof(ReportFields), ReportFields.FullYearOverUnderDraw));
            FinalizeReport();
        }
        #endregion

        #region ReportAttachmentName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ReportAttachmentName
        {
            get
            {
                return string.Format(reportAttachmentName, asOfDate.ToShortDateString().Replace("/", "-"), (CompanyId == 1 ? "USA" : "CAN"));
            }
        }
        #endregion

    }

    /// <summary>Commission Report base class</summary>
    public class CommissionReportBuilder
    {

        #region ReportOption enumeration
        public enum ReportOption
        {
            ActiveAEsOnly,
            ActiveAEsWithoutHidden,
            AllAEs,
        }
        #endregion

        #region Member variables
        /// <summary>TBD</summary>
        protected StringBuilder CommissionReport;
        /// <summary>TBD</summary>
        public ReportOption CommissionReportOption = ReportOption.AllAEs;
        /// <summary>TBD</summary>
        protected int CompanyId;
        /// <summary>TBD</summary>
        protected int CurrentTemplateExpectedValuesCount;
        /// <summary>TBD</summary>
        protected string CurrentTemplateSection;
        /// <summary>TBD</summary>
        protected DateTime EndDate;
        /// <summary>TBD</summary>
        private const string EXPECTED_VALUE_COUNT_XPATH = "ExpectedValueCount/@value";
        /// <summary>TBD</summary>
        private const string REPORT_END_TEMPLATE_NAME = "ReportEnd";
        /// <summary>TBD</summary>
        private const string REPORT_START_TEMPLATE_NAME = "ReportStart";
        /// <summary>TBD</summary>
        protected DataSet ReportData;
        /// <summary>TBD</summary>
        protected string ReportRoot;
        /// <summary>TBD</summary>
        protected XmlDocument ReportTemplateDoc;
        /// <summary>TBD</summary>
        protected int ReportYear;
        /// <summary>TBD</summary>
        protected DateTime StartDate;
        /// <summary>TBD</summary>
        private const string TEMPLATE_XPATH = "Template";
        /// <summary>TBD</summary>
        protected string XmlDocRoot = "ReportTemplates/CommissionTemplates";
        #endregion

        #region Parameterized constructor (reportRoot, startDate, endDate, requestedReportOption, companyId)
        /// <summary>CommissionReportBuilder constructor</summary>
        /// <param name="reportRoot">The root XML element containg the report template</param>
        /// <param name="startDate">TBD</param>
        /// <param name="endDate">TBD</param>
        /// <param name="requestedReportOption">TBD</param>
        /// <param name="companyId">TBD</param>
        public CommissionReportBuilder(string reportRoot, DateTime startDate, DateTime endDate, ReportOption requestedReportOption, int companyId)
        {
            CommissionReport = new StringBuilder();
            ReportTemplateDoc = new XmlDocument();
            //ReportTemplateDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/CommissionReportTemplate.xml"));
            ReportTemplateDoc.Load(HostingEnvironment.MapPath(HostingEnvironment.ApplicationVirtualPath + "/App_Data/CommissionReportTemplate.xml"));
            this.ReportRoot = reportRoot;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.ReportYear = endDate.Year;
            this.CommissionReportOption = requestedReportOption;
            this.CompanyId = companyId;
        }
        #endregion

        #region BuildReportRow method
        /// <summary>Builds and appends a single row of the report</summary>
        /// <param name="templateName">The XML element containing the report row template</param>
        /// <param name="row">The DataRow containing the report data</param>
        /// <param name="reportFields">TBD</param>
        public virtual void BuildReportRow(string templateName, DataRow row, string[] reportFields)
        {
            if (String.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException("reportRowTemplateName");
            }
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }
            ArrayList fieldValueList = new ArrayList();
            foreach (string field in reportFields)
            {
                fieldValueList.Add(Convert.ToString(row[field]));
            }
            BuildRow(templateName, (string[])fieldValueList.ToArray(typeof(string)));
        }
        #endregion

        #region BuildReportTotalRow method (templateName, rows, totalFields)
        /// <summary>TBD</summary>
        /// <param name="templateName">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="totalFields">TBD</param>
        public virtual void BuildReportTotalRow(string templateName, DataRow[] rows, string[] totalFields)
        {
            DataTable table = new DataTable();
            foreach (DataRow row in rows)
            {
                table.Rows.Add(row);
            }
            BuildReportTotalRow(templateName, table, totalFields);
        }
        #endregion

        #region BuildReportTotalRow method (templateName, reportData, totalFields)
        /// <summary>TBD</summary>
        /// <param name="templateName">TBD</param>
        /// <param name="reportData">TBD</param>
        /// <param name="totalFields">TBD</param>
        public virtual void BuildReportTotalRow(string templateName, DataTable reportData, string[] totalFields)
        {
            if (String.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException("reportRowTemplateName");
            }
            if (reportData == null)
            {
                throw new ArgumentNullException("row");
            }
            if (totalFields == null || totalFields.Length <= 0)
            {
                throw new ArgumentOutOfRangeException("totalFields");
            }
            ArrayList fieldValueList = new ArrayList();
            foreach (string field in totalFields)
            {
                fieldValueList.Add(Convert.ToString(reportData.Compute("SUM([" + field + "])", "")));
            }
            BuildRow(templateName, (string[])fieldValueList.ToArray(typeof(string)));
        }
        #endregion

        #region BuildRow method
        /// <summary>Appends a single row to the Commission Report</summary>
        /// <param name="templateName">The XML Element containing the Commission Report template</param>
        /// <param name="values">The values to insert into the template</param>
        public void BuildRow(string templateName, string[] values)
        {
            GetReportTemplate(templateName);
            if (values != null && values.Length > CurrentTemplateExpectedValuesCount)
            {
                throw new ArgumentOutOfRangeException("values", values.Length, "Values array exceeds limit allowed by Template: " + CurrentTemplateExpectedValuesCount);
            }
            if (values == null)
            {
                CommissionReport.AppendLine(CurrentTemplateSection);
            }
            else
            {
                CommissionReport.AppendFormat(CurrentTemplateSection, values);
            }
        }
        #endregion

        #region FinalizeReport method
        /// <summary>Completes the Report output</summary>
        public void FinalizeReport()
        {
            BuildRow(REPORT_END_TEMPLATE_NAME, null);
        }
        #endregion

        #region GetReportData method
        /// <summary>Executes the stored procedure that retrieves the report data</summary>
        /// <param name="storedProc">The name of the stored procedure to execute</param>
        public virtual void GetReportData(string storedProc)
        {
            List<SqlParameter> reportParams = new List<SqlParameter>();
            reportParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, StartDate));
            reportParams.Add(Param.CreateParam("ENDDATE", SqlDbType.Date, EndDate));
            reportParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, CompanyId));
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc(storedProc, reportParams))
                {
                    cmd.CommandTimeout = 200;
                    this.ReportData = io.ExecuteDataSetQuery(cmd);
                }
            }
        }
        #endregion

        #region GetReportOutput method
        /// <summary>Converts the CommissionReport StringBuilder object to a string</summary>
        /// <returns>Returns a string representation of the Commission Report</returns>
        public string GetReportOutput()
        {
            return this.CommissionReport.ToString();
        }
        #endregion

        #region GetReportTemplate method
        /// <summary>Retreives the InnerText of an XML element in the report template XML file</summary>
        /// <param name="reportSection">The XML element containing the template text</param>
        public void GetReportTemplate(string reportSection)
        {
            //initialize the template string
            this.CurrentTemplateSection = string.Empty;
            //Build the XPath Query
            string xPathQuery = XmlDocRoot + "/" + ReportRoot + "/" + reportSection;
            //Retrieve the expected value count for this template section
            this.CurrentTemplateExpectedValuesCount = Convert.ToInt32(ReportTemplateDoc.SelectSingleNode(xPathQuery + "/" + EXPECTED_VALUE_COUNT_XPATH).Value);
            //Retrieve the template text
            this.CurrentTemplateSection = ReportTemplateDoc.SelectSingleNode(xPathQuery + "/" + TEMPLATE_XPATH).InnerText;
        }
        #endregion

        #region StartReport method
        /// <summary>Generates the start of the Report output</summary>
        public void StartReport()
        {
            this.CommissionReport = new StringBuilder();
            BuildRow(REPORT_START_TEMPLATE_NAME, null);
        }
        #endregion

    }

    /// <summary>
    ///     Helper class to generate the
    ///     Flash Commission Excel Report
    /// </summary>
    public class FlashCommissionReportBuilder : CommissionReportBuilder
    {

        #region AECommissionDataTypes enumeration
        private enum AECommissionDataTypes
        {
            CommissionEarned = 0,
            Draw,
            Payments,
            ProductionCommissionRelease,
            ProductionCommissionTotal,
        }
        #endregion

        #region CalculationMethod delegate
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        private delegate double CalculationMethod(DataRow row);
        #endregion

        #region MonthFields enumeration
        private enum MonthFields
        {
            January = 0, February, March, April, May, June,
            July, August, September, October, November, December,
            Total,
        }
        #endregion

        #region MonthlySummaryValues struct
        /// <summary>TBD</summary>
        private struct MonthlySummaryValues
        {

            #region Member variables
            /// <summary>TBD</summary>
            public int companyId;
            /// <summary>Array to store the AR Percentage for each Month</summary>
            public ArrayList MonthlyARPercentage;
            /// <summary>TBD</summary>
            public ArrayList MonthlyCommission;
            /// <summary>TBD</summary>
            public ArrayList MonthlyCommissionAccrual;
            /// <summary>TBD</summary>
            public ArrayList MonthlyDrawFulfillment;
            /// <summary>TBD</summary>
            public ArrayList MonthlyInitialHoldback;
            /// <summary>TBD</summary>
            public ArrayList MonthlyMediaCommission;
            /// <summary>TBD</summary>
            public ArrayList MonthlyOutstandingAR;
            /// <summary>TBD</summary>
            public ArrayList MonthlyProductionCommission;
            /// <summary>TBD</summary>
            public ArrayList MonthlyProductionCommissionHoldback;
            /// <summary>TBD</summary>
            public ArrayList MonthlyProductionCommissionPercentage;
            /// <summary>TBD</summary>
            public ArrayList MonthlyProductionCommissionRelease;
            /// <summary>TBD</summary>
            public ArrayList MonthlySales;
            /// <summary>TBD</summary>
            public double OutstandingARTotalUntilDrawFulfillment;
            /// <summary>TBD</summary>
            public int ProcessYear;
            /// <summary>TBD</summary>
            public ArrayList ProductionMonthlyOutstandingAR;
            /// <summary>TBD</summary>
            public double RevenueTotalUntilDrawFulfillment;
            public int monthDrawFulfilled;
            #endregion

            #region CalculateARPercentages method
            /// <summary>TBD</summary>
            /// <param name="endDate">TBD</param>
            private void CalculateARPercentages(DateTime endDate)
            {
                double arPercentage = 0.00;
                //Initialize January's AR percentage
                //     January's AR % = January's Outstanding AR / January's Revenue * 100
                this.MonthlyARPercentage.Add((Convert.ToDouble(this.MonthlyOutstandingAR[(int)MonthFields.January]) / Convert.ToDouble(this.MonthlySales[(int)MonthFields.January])) * 100);
                //Start calculating AR percentages through November
                //  We're actually calculating NEXT month's AR %
                for (int i = (int)MonthFields.January; i < (int)MonthFields.December; i++)
                {
                    if ((Convert.ToDouble(this.MonthlyCommissionAccrual[i + 1]) > 0) && (Convert.ToDouble(this.MonthlyCommissionAccrual[i]) <= 0))
                    {//if next month's commission accrual > 0 and this month's commission accrual <= 0...
                        //Then AR % = (Outstanding AR Total Until Draw Fulfillment + Next Month's Outstanding AR) / (Revenue Total Until Draw Fulfillment + Next Month's Revenue) * 100
                        arPercentage = (
                        (this.OutstandingARTotalUntilDrawFulfillment + Convert.ToDouble(this.MonthlyOutstandingAR[i + 1])) /
                        (this.RevenueTotalUntilDrawFulfillment + Convert.ToDouble(this.MonthlySales[i + 1]))
                        ) * 100;
                    }
                    else
                    {
                        //AR % = (Next Month's AR Total / Next Month's Revenue) * 100
                        arPercentage = (Convert.ToDouble(this.MonthlyOutstandingAR[i + 1]) / Convert.ToDouble(this.MonthlySales[i + 1])) * 100;
                    }
                    this.MonthlyARPercentage.Add(arPercentage);
                }
                arPercentage = ((Convert.ToDouble(this.MonthlyOutstandingAR[(int)MonthFields.Total]) == 0) || (this.GetSumUpToMonth(this.MonthlySales, (endDate.Year > ProcessYear ? 12 : endDate.Month)) == 0)) ?
                0.00 : (Convert.ToDouble(this.MonthlyOutstandingAR[(int)MonthFields.Total]) / this.GetSumUpToMonth(this.MonthlySales, (endDate.Year > ProcessYear ? 12 : endDate.Month))) * 100;
                //HACK HACK HACK - MUST REMOVE Below before processing 2010 commissions (hardcoded endDate.Month to 12)
                //arPercentage = ((Convert.ToDouble(this.MonthlyOutstandingAR[(int)MonthFields.Total]) == 0) || (this.GetSumUpToMonth(this.MonthlySales, 12) == 0)) ?
                //    0.00 : (Convert.ToDouble(this.MonthlyOutstandingAR[(int)MonthFields.Total]) / this.GetSumUpToMonth(this.MonthlySales, 12)) * 100;
                this.MonthlyARPercentage.Add(arPercentage);
            }
            #endregion

            #region GetSumUpToMonth method
            /// <summary>TBD</summary>
            /// <param name="list">TBD</param>
            /// <param name="month">TBD</param>
            /// <returns>TBD</returns>
            public double GetSumUpToMonth(ArrayList list, int month)
            {
                if (month < 0)
                {
                    throw new ArgumentOutOfRangeException("month", "Month must be greater than or equal to zero");
                }
                if (list == null)
                {
                    throw new ArgumentNullException("list");
                }
                double sum = 0.00;
                for (int i = 0; i < month; i++)
                {
                    sum += Convert.ToDouble(list[i]);
                }
                return sum;
            }
            #endregion

            #region Initialize method
            /// <summary>TBD</summary>
            /// <param name="reportTable">TBD</param>
            /// <param name="aeCommissionData">TBD</param>
            /// <param name="endDate">TBD</param>
            /// <param name="processYear">TBD</param>
            /// <param name="companyId">TBD</param>
            public void Initialize(DataTable reportTable, DataSet aeCommissionData, DateTime endDate, int processYear, int companyId)
            {
                this.MonthlySales = new ArrayList();
                this.MonthlyCommission = new ArrayList();
                this.MonthlyDrawFulfillment = new ArrayList();
                this.MonthlyCommissionAccrual = new ArrayList();
                this.MonthlyOutstandingAR = new ArrayList();
                this.MonthlyInitialHoldback = new ArrayList();
                this.MonthlyARPercentage = new ArrayList();
                this.ProductionMonthlyOutstandingAR = new ArrayList();
                this.MonthlyProductionCommissionPercentage = new ArrayList();
                this.MonthlyMediaCommission = new ArrayList();
                this.MonthlyProductionCommission = new ArrayList();
                this.MonthlyProductionCommissionRelease = new ArrayList();
                this.MonthlyProductionCommissionHoldback = new ArrayList();
                this.OutstandingARTotalUntilDrawFulfillment = 0.00;
                this.RevenueTotalUntilDrawFulfillment = 0.00;
                this.ProcessYear = processYear;
                this.companyId = companyId;
                PopulateValues(reportTable, aeCommissionData);
                CalculateARPercentages(endDate);
            }
            #endregion

            #region PopulateValues method
            /// <summary>TBD</summary>
            /// <param name="reportTable">TBD</param>
            /// <param name="aeCommissionData">TBD</param>
            private void PopulateValues(DataTable reportTable, DataSet aeCommissionData)
            {
                double initialDrawAmount = (aeCommissionData.Tables[(int)AECommissionDataTypes.Draw].Rows.Count == 0) ? 0.00 : Convert.ToDouble(aeCommissionData.Tables[(int)AECommissionDataTypes.Draw].Rows[0][0] ?? 0.00);
                double remainingDrawAmount = initialDrawAmount;
                double mediaCommissionAmount = 0.00;
                double commissionAccrualAmount = 0.00;
                double totalCommissionAccrual = 0.00;
                double initialHoldback = 0.00;
                double totalInitialHoldback = 0.00;
                //Production Commission Changes
                double productionCommissionAmount = 0.00;
                double productionCommissionAmountRelease = 0.00;
                double commissionAccrualWithProductionAmount = 0.00;
                double productionCommissionPercentage = 0.00;
                double monthlyOutstandingAR = 0.00;
                double monthlyProductionOutstandingAR = 0.00;
                double totalOutstandingAR = 0.00;
                double totalProductionOutstandingAR = 0.00;
                double productionHoldback = 0.00;
                double totalProductionHoldback = 0.00;
                bool monthDrawFulfilledSet = false;
                //reportTable represents the Sales data for a given AE
                for (int i = (int)MonthFields.January; i <= (int)MonthFields.December; i++)
                {
                    //Insert the month's Sales amount
                    this.MonthlySales.Add(Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(TotalFields), (int)TotalFields.January + i) + "])", "")));
                    //Insert the month's Outstanding AR
                    monthlyOutstandingAR = Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(OutstandingARTotalFields), (int)OutstandingARTotalFields.JanOutstanding + i) + "])", ""));
                    monthlyProductionOutstandingAR = Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(OutstandingProductionARTotalFields), (int)OutstandingProductionARTotalFields.JanProdOutstanding + i) + "])", ""));
                    totalProductionOutstandingAR += monthlyProductionOutstandingAR;
                    //Production Commission/Outstanding Production AR change is effective 3/1/2010
                    if (this.companyId == 1 && (this.ProcessYear > 2010 || (this.ProcessYear == 2010 && i >= 2)))
                    {
                        this.MonthlyOutstandingAR.Add(monthlyOutstandingAR - monthlyProductionOutstandingAR);
                        totalOutstandingAR += (monthlyOutstandingAR - monthlyProductionOutstandingAR);
                    }
                    else
                    {
                        this.MonthlyOutstandingAR.Add(monthlyOutstandingAR);
                        totalOutstandingAR += monthlyOutstandingAR;
                    }
                    //Insert the month's Production Outstanding AR
                    this.ProductionMonthlyOutstandingAR.Add(monthlyProductionOutstandingAR);
                    //aeCommissionTable will contain the remaining needed data:
                    //  Determine Media Commission amount for the month
                    mediaCommissionAmount = Convert.ToDouble(aeCommissionData.Tables[(int)AECommissionDataTypes.CommissionEarned].Rows[0][i]);
                    //  Determine Production Commission amount for the month
                    productionCommissionAmount = Convert.ToDouble(aeCommissionData.Tables[(int)AECommissionDataTypes.ProductionCommissionTotal].Rows[0][i]);
                    //productionCommissionAmountRelease = Convert.ToDouble(aeCommissionData.Tables[(int)AECommissionDataTypes.ProductionCommissionRelease].Rows[0][i]);
                    //productionHoldback = productionCommissionAmount - productionCommissionAmountRelease;
                    //totalProductionHoldback += productionHoldback;
                    //this.MonthlyProductionCommissionRelease.Add(Convert.ToString(productionCommissionAmountRelease));
                    //this.MonthlyProductionCommissionHoldback.Add(Convert.ToString(productionHoldback));
                    //  Determine what percentage of total commission for the month is production
                    productionCommissionPercentage = (productionCommissionAmount > 0) ? (productionCommissionAmount / (mediaCommissionAmount + productionCommissionAmount)) : 0.00;
                    this.MonthlyProductionCommissionPercentage.Add(productionCommissionPercentage);
                    //  Determine the Commission Accrual Amount for the month (including production)
                    //  Pre-Production Way: commissionAccrualAmount = mediaCommissionAmount - remainingDrawAmount;
                    commissionAccrualWithProductionAmount = (mediaCommissionAmount + productionCommissionAmount) - remainingDrawAmount;
                    //Include Production Commission Amount in Total Commission Accrual
                    //  Pre-Production Way: totalCommissionAccrual += (commissionAccrualAmount > 0) ? commissionAccrualAmount : 0.00;
                    totalCommissionAccrual += (commissionAccrualWithProductionAmount > 0) ? commissionAccrualWithProductionAmount : 0.00;
                    //The 25% initial holdback still applies to regular (non-Production) commission amounts
                    //  Pre-Production Way: initialHoldback = (commissionAccrualAmount > 0) ? commissionAccrualAmount * 0.25 : 0.00;
                    //  The 25% portion of the holdback only applies to the Media portion of the Commission Accrual
                    initialHoldback = (commissionAccrualWithProductionAmount > 0) ? ((commissionAccrualWithProductionAmount - (commissionAccrualWithProductionAmount * productionCommissionPercentage)) * (HOLDBACK_PERCENTAGE / 100.0)) : 0.00;
                    totalInitialHoldback += initialHoldback;
                    //commissionAccrualAmount = ((commissionAmount - remainingDrawAmount) > 0) ? (commissionAccrualAmount + (commissionAmount - remainingDrawAmount)) : (commissionAmount - remainingDrawAmount);
                    //  Pre-Production Way: this.MonthlyCommission.Add(commissionAmount);
                    this.MonthlyMediaCommission.Add(mediaCommissionAmount);
                    this.MonthlyProductionCommission.Add(productionCommissionAmount);
                    this.MonthlyCommission.Add(mediaCommissionAmount + productionCommissionAmount);
                    this.MonthlyDrawFulfillment.Add((i != (int)MonthFields.Total) ? (Convert.ToDouble((remainingDrawAmount > 0) ? remainingDrawAmount : 0.00)) : initialDrawAmount);
                    //  Pre-Production Way: this.MonthlyCommissionAccrual.Add(commissionAccrualAmount);
                    this.MonthlyCommissionAccrual.Add(commissionAccrualWithProductionAmount);
                    //  Pre-Production Way: this.MonthlyInitialHoldback.Add((commissionAccrualAmount > 0) ? commissionAccrualAmount * 0.25 : 0.00);
                    this.MonthlyInitialHoldback.Add(initialHoldback);
                    //  Pre-Production Way: remainingDrawAmount = (commissionAccrualAmount > 0) ? 0 : -1 * commissionAccrualAmount;
                    remainingDrawAmount = (commissionAccrualWithProductionAmount > 0) ? 0 : -1 * commissionAccrualWithProductionAmount;
                    //Check to see if Draw has been fulfilled
                    if (remainingDrawAmount <= 0 && !monthDrawFulfilledSet)
                    {
                        monthDrawFulfilled = i;
                        monthDrawFulfilledSet = true;
                    }
                    if (monthDrawFulfilledSet)
                    {
                        productionCommissionAmountRelease = Convert.ToDouble(aeCommissionData.Tables[(int)AECommissionDataTypes.ProductionCommissionRelease].Rows[0][i]);
                        productionHoldback = productionCommissionAmount - productionCommissionAmountRelease;
                    }
                    else
                    {
                        productionCommissionAmountRelease = 0.0;
                        productionHoldback = 0.0;
                    }
                    totalProductionHoldback += productionHoldback;
                    this.MonthlyProductionCommissionRelease.Add(Convert.ToString(productionCommissionAmountRelease));
                    this.MonthlyProductionCommissionHoldback.Add(Convert.ToString(productionHoldback));
                    //If the current month's commission Accrual amount is negative, add the current month's revenue to the running total
                    //  Pre-Production Way: RevenueTotalUntilDrawFulfillment += (commissionAccrualAmount < 0) ? Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(TotalFields), (int)TotalFields.January + i) + "])", "")) : 0.00;
                    RevenueTotalUntilDrawFulfillment += (commissionAccrualWithProductionAmount < 0) ? Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(TotalFields), (int)TotalFields.January + i) + "])", "")) : 0.00;
                    //If the current month's commission Accrual amount is negative, add the current month's outstanding AR to the running total
                    //      Use the TotalFields enum, starting at January and offset by the loop index
                    //Pre-Production Way: OutstandingARTotalUntilDrawFulfillment += (commissionAccrualAmount < 0) ? Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(OutstandingARTotalFields), ((int)OutstandingARTotalFields.JanOutstanding + i)) + "])", "")) : 0.00;
                    OutstandingARTotalUntilDrawFulfillment += (commissionAccrualWithProductionAmount < 0) ? Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(OutstandingARTotalFields), ((int)OutstandingARTotalFields.JanOutstanding + i)) + "])", "")) : 0.00;
                }
                //If, at the end of the year, the total commission accrual amount is still negative,
                //  display the December's commission accrual amount
                if (totalCommissionAccrual == 0)
                {
                    totalCommissionAccrual = commissionAccrualAmount;
                }
                //Add the Summary values
                //Total Monthly Sales:
                this.MonthlySales.Add(Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(TotalFields), (int)TotalFields.TotalAEAmount) + "])", "")));
                //Total Outstanding AR:
                this.MonthlyOutstandingAR.Add(totalOutstandingAR);
                //this.MonthlyOutstandingAR.Add(Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(OutstandingARTotalFields), (int)OutstandingARTotalFields.TotalOutstanding) + "])", "")));
                //Total Production Outstanding AR:
                this.ProductionMonthlyOutstandingAR.Add(totalProductionOutstandingAR);
                this.MonthlyProductionCommissionHoldback.Add(totalProductionHoldback);
                //this.ProductionMonthlyOutstandingAR.Add(Convert.ToDouble(reportTable.Compute("Sum([" + Enum.GetName(typeof(OutstandingProductionARTotalFields), (int)OutstandingProductionARTotalFields.TotalProdOutstanding) + "])", "")));
                //Total Media Commission Amount:
                //Total Production Commission Amount:
                //Total Commission Amount:
                this.MonthlyCommission.Add(Convert.ToDouble(aeCommissionData.Tables[(int)AECommissionDataTypes.CommissionEarned].Rows[0][(int)MonthFields.Total]));
                //Draw fulfillment (show the initial draw)
                this.MonthlyDrawFulfillment.Add(initialDrawAmount);
                //Total Commission Accrual
                this.MonthlyCommissionAccrual.Add(totalCommissionAccrual);
                //Total Initial Holdback
                this.MonthlyInitialHoldback.Add(totalInitialHoldback);
            }
            #endregion

        }
        #endregion

        #region OutstandingARTotalFields enumeration
        private enum OutstandingARTotalFields
        {
            JanOutstanding = 37, FebOutstanding, MarOutstanding, AprOutstanding,
            MayOutstanding, JunOutstanding, JulOutstanding, AugOutstanding,
            SepOutstanding, OctOutstanding, NovOutstanding, DecOutstanding,
            TotalOutstanding,
        }
        #endregion

        #region OutstandingProductionARTotalFields enumeration
        private enum OutstandingProductionARTotalFields
        {
            JanProdOutstanding = 50, FebProdOutstanding, MarProdOutstanding,
            AprProdOutstanding, MayProdOutstanding, JunProdOutstanding,
            JulProdOutstanding, AugProdOutstanding, SepProdOutstanding,
            OctProdOutstanding, NovProdOutstanding, DecProdOutstanding,
            TotalProdOutstanding,
        }
        #endregion

        #region ProductionTotalFields enumeration
        private enum ProductionTotalFields
        {
            JanuaryProd = 21, FebruaryProd, MarchProd, AprilProd,
            MayProd, JuneProd, JulyProd, AugustProd,
            SeptemberProd, OctoberProd, NovemberProd, DecemberProd,
            TotalProd,
        }
        #endregion

        #region ReportFields enumeration
        private enum ReportFields
        {
            Advertiser = 2, ContractNumber, ProfitCenter, Agency,
            NationalOrLocal, Program, NumberOfAEs,
            January, February, March, April, May, June,
            July, August, September, October, November, December,
            TotalAEAmount = 34, TotalBilling, CommissionAmount,
        }
        #endregion

        #region SecondaryReportFields enumeration
        private enum SecondaryReportFields
        {
            Advertiser = 1, ContractNumber, ProfitCenter, Agency,
            NationalOrLocal, NumberOfAEs = 8,
            JanOutstanding = 37, FebOutstanding, MarOutstanding, AprOutstanding,
            MayOutstanding, JunOutstanding, JulOutstanding, AugOutstanding,
            SepOutstanding, OctOutstanding, NovOutstanding, DecOutstanding,
            TotalOutstanding,
        }
        #endregion

        #region SecondaryTotalFields enumeration
        private enum SecondaryTotalFields
        {
            JanOutstanding = 37, FebOutstanding, MarOutstanding, AprOutstanding,
            MayOutstanding, JunOutstanding, JulOutstanding, AugOutstanding,
            SepOutstanding, OctOutstanding, NovOutstanding, DecOutstanding,
            TotalOutstanding,
        }
        #endregion

        #region TotalCalculationMethod delegate
        /// <summary>TBD</summary>
        /// <param name="table">TBD</param>
        /// <returns>TBD</returns>
        private delegate double TotalCalculationMethod(DataTable table);
        #endregion

        #region TotalFields enumeration
        private enum TotalFields
        {
            January = 8, February, March, April, May, June,
            July, August, September, October, November, December,
            TotalAEAmount = 34, TotalBilling, CommissionAmount,
        }
        #endregion

        #region Member variables
        /// <summary>TBD</summary>
        public const string AE_COMMISION_DATA_STORED_PROC_NAME = "CommissionReports_GetFlashAEData";
        /// <summary>TBD</summary>
        private DataSet AECommissionData;
        /// <summary>TBD</summary>
        private string[] AEFilterList;
        /// <summary>TBD</summary>
        private string[] AENameList;
        /// <summary>TBD</summary>
        private string[] AEReports;
        /// <summary>TBD</summary>
        private string[] AESecondaryReports;
        /// <summary>TBD</summary>
        public int companyId;
        /// <summary>TBD</summary>
        private bool[] hasData;
        /// <summary>TBD</summary>
        private MonthlySummaryValues MonthlySummaries = new MonthlySummaryValues();
        /// <summary>TBD</summary>
        private int processYear;
        /// <summary>The name of the XML Element containing the Report Headers template</summary>
        public const string REPORT_HEADERS_TEMPLATE_NAME = "ReportHeaders";
        /// <summary>The name of the XML Element containing the Report Row template</summary>
        public const string REPORT_ROW_TEMPLATE_NAME = "ReportRow";
        /// <summary>Name of the Stored Procedure to retrieve report data</summary>
        public const string REPORT_STORED_PROC_NAME = "CommissionReports_GetFlashReport";
        /// <summary>The name of the XML Element containing the Report template</summary>
        public const string REPORT_TEMPLATE_NAME = "FlashReport";
        /// <summary>The name of the XML Element containing the Report Total Row template</summary>
        public const string REPORT_TOTAL_ROW_TEMPLATE_NAME = "ReportTotalRow";
        /// <summary>TBD</summary>
        private string reportAttachmentName = "({0}) to ({1}) FlashCommission_Consolidated_{2}.zip";
        /// <summary>TBD</summary>
        private string ReportFileName = "{0} FC - {1} as of {2} (Consolidated).xls";
        /// <summary>TBD</summary>
        public const string SECONDARY_REPORT_TEMPLATE_NAME = "FlashARReport";
        /// <summary>TBD</summary>
        private string SecondaryReportFileName = "{0} FC - {1} as of {2} AR (Consolidated).xls";
        /// <summary>TBD</summary>
        private bool useInvoiceCreationDate = false;
        public const int HOLDBACK_PERCENTAGE = 25;
        #endregion

        #region Parameterized constructor (selectedAEs, startDate, endDate, processYear, useInvoiceCreationDate, companyId)
        /// <summary>TBD</summary>
        /// <param name="selectedAEs">TBD</param>
        /// <param name="startDate">TBD</param>
        /// <param name="endDate">TBD</param>
        /// <param name="processYear">TBD</param>
        /// <param name="useInvoiceCreationDate">TBD</param>
        /// <param name="companyId">TBD</param>
        public FlashCommissionReportBuilder(string[] selectedAEs, DateTime startDate, DateTime endDate, int processYear, bool useInvoiceCreationDate, int companyId)
            : base(REPORT_TEMPLATE_NAME, startDate, endDate, ReportOption.AllAEs, companyId)
        {
            this.AEFilterList = selectedAEs;
            this.processYear = processYear;
            this.useInvoiceCreationDate = useInvoiceCreationDate;
            this.companyId = companyId;
        }
        #endregion

        #region BuildCommissionSummary method
        /// <summary>TBD</summary>
        /// <param name="reportTable">TBD</param>
        private void BuildCommissionSummary(DataTable reportTable)
        {
            //Step 2:
            //Start Commission Summary Row
            //old Commission Percentage Calc: Commission Amount / AE Amount
            if ((this.processYear >= 2010 && this.companyId == 1) || (this.processYear >= 2011 && this.companyId == 2))
            {
                BuildReportTotalRowWithComputedField("CommissionSummaryRow", reportTable, Enum.GetNames(typeof(TotalFields)), CalculateTotalCommissionAmount);
            }
            else
            {
                BuildReportTotalRowWithComputedField("CommissionSummaryRowPre2010", reportTable, Enum.GetNames(typeof(TotalFields)), CalculateTotalCommissionAmount);
            }
            //new Commission Percentage Calc: Billing Amount / AE Amount
            //BuildReportTotalRowWithComputedField("CommissionSummaryRow", reportTable, Enum.GetNames(typeof(TotalFields)), CalculateTotalCommissionPercentage);
            //Step 3:
            //Build Commission Earned Row
            BuildReportRow("CommissionRow", AECommissionData.Tables[(int)AECommissionDataTypes.CommissionEarned].Rows[0], Enum.GetNames(typeof(MonthFields)));
            //Production Commission:
            if ((this.processYear >= 2010 && this.companyId == 1) || (this.processYear >= 2011 && this.companyId == 2))
            {
                BuildReportRow("ProductionCommissionRow", AECommissionData.Tables[(int)AECommissionDataTypes.ProductionCommissionTotal].Rows[0], Enum.GetNames(typeof(MonthFields)));
            }
            //Step 4 & 5:
            //Build Draw Fulfillment & Commission Accrual Rows
            ArrayList drawFulfillmentsDisplay = new ArrayList();
            ArrayList commissionAccrualDisplay = new ArrayList();
            //Iterate through each month and calculate the Draw Fulfillment/Commission Accrual
            for (int i = (int)MonthFields.January; i <= (int)MonthFields.Total; i++)
            {
                drawFulfillmentsDisplay.Add(Convert.ToString(MonthlySummaries.MonthlyDrawFulfillment[i]));
                commissionAccrualDisplay.Add(Convert.ToString(MonthlySummaries.MonthlyCommissionAccrual[i]));
            }
            base.BuildRow("DrawRow", (string[])drawFulfillmentsDisplay.ToArray(typeof(string)));
            base.BuildRow("CommissionAccrualRow", (string[])commissionAccrualDisplay.ToArray(typeof(string)));
        }
        #endregion

        #region BuildHoldbackSummary method
        /// <summary>TBD</summary>
        /// <param name="reportTable">TBD</param>
        private void BuildHoldbackSummary(DataTable reportTable)
        {
            //Step 6:
            //Start Holdback Summary Row w/Outstanding AR amounts:
            ArrayList outstandingARDisplay = new ArrayList();
            ArrayList arPercentageDisplay = new ArrayList();
            ArrayList initialHoldbackDisplay = new ArrayList();
            ArrayList currentHoldbackDisplay = new ArrayList();
            ArrayList holdbackReleaseDisplay = new ArrayList();
            double outstandingARTotal = 0.00;
            double currentHoldback = 0.00;
            double currentHoldbackTotal = 0.00;
            double holdbackRelease = 0.00;
            double holdbackReleaseTotal = 0.00;
            double initialHoldbackTotal = 0.00;
            double productionCommissionPercentage = 0.00;
            double monthlyCommissionAccrual = 0.00;
            double holdbackPercentage = HOLDBACK_PERCENTAGE / 100.0;
            for (int i = (int)MonthFields.January; i <= (int)MonthFields.December; i++)
            {
                //These fields are only displayed/aggregated through the last month of the report End Date
                if (i < (EndDate.Year > this.processYear ? 12 : EndDate.Month))
                {
                    outstandingARDisplay.Add(Convert.ToString(MonthlySummaries.MonthlyOutstandingAR[i]));
                    outstandingARTotal += Convert.ToDouble(MonthlySummaries.MonthlyOutstandingAR[i]);
                    monthlyCommissionAccrual = Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]);
                    productionCommissionPercentage = Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionPercentage[i]);
                    //Current Holdback = Commission Accrual * (AR % > 25 ? 0.25 : AR % / 100)
                    currentHoldback = (monthlyCommissionAccrual - (monthlyCommissionAccrual * productionCommissionPercentage)) * ((Convert.ToDouble(MonthlySummaries.MonthlyARPercentage[i]) > HOLDBACK_PERCENTAGE) ? holdbackPercentage : Convert.ToDouble(MonthlySummaries.MonthlyARPercentage[i]) / 100);
                    currentHoldbackTotal += (monthlyCommissionAccrual > 0) ? currentHoldback : 0.00;
                    //Holdback Release = if Commission Accrual is positive then Commission Accrual * 0.25 - Current Month's Holdback
                    //holdbackRelease = (monthlyCommissionAccrual > 0) ? (((monthlyCommissionAccrual - (monthlyCommissionAccrual * productionCommissionPercentage)) * 0.25) - currentHoldback) : 0.00;
                    holdbackRelease = (monthlyCommissionAccrual > 0) ? (((monthlyCommissionAccrual - (monthlyCommissionAccrual * productionCommissionPercentage)) * holdbackPercentage) - currentHoldback) : 0.00;
                    holdbackReleaseTotal += (monthlyCommissionAccrual > 0) ? holdbackRelease : 0.00;
                    //If the Commission Accrual for this month is positive, show the Outstanding AR %
                    arPercentageDisplay.Add((monthlyCommissionAccrual <= 0) ? "-" : Convert.ToString(Convert.ToDouble(MonthlySummaries.MonthlyARPercentage[i])));
                    //If the Commission Accrual for this month is positive, show the Initial Holdback
                    initialHoldbackDisplay.Add((monthlyCommissionAccrual <= 0) ? "-" : Convert.ToString(MonthlySummaries.MonthlyInitialHoldback[i]));
                    initialHoldbackTotal += (Convert.ToDouble(MonthlySummaries.MonthlyInitialHoldback[i]));
                    //If the Commission Accrual for this month is positive, show the Current Holdback
                    currentHoldbackDisplay.Add((monthlyCommissionAccrual <= 0) ? "-" : Convert.ToString(currentHoldback));
                    //If the Commission Accrual for this month is positive, show the Holdback Release
                    holdbackReleaseDisplay.Add((monthlyCommissionAccrual <= 0) ? "-" : Convert.ToString(holdbackRelease));
                }
                else
                {
                    outstandingARDisplay.Add("-");
                    arPercentageDisplay.Add("-");
                    initialHoldbackDisplay.Add("-");
                    currentHoldbackDisplay.Add("-");
                    holdbackReleaseDisplay.Add("-");
                    currentHoldback = 0.00;
                    holdbackRelease = 0.00;
                    outstandingARTotal += 0.00;
                    initialHoldbackTotal += 0.00;
                }
            }
            //Add the final month's AR %
            arPercentageDisplay.Add((Convert.ToDouble(MonthlySummaries.MonthlyARPercentage[(int)MonthFields.Total]) == 0.00) ? "-" : Convert.ToString(Convert.ToDouble(MonthlySummaries.MonthlyARPercentage[(int)MonthFields.Total])));
            outstandingARDisplay.Add(Convert.ToString(outstandingARTotal));
            initialHoldbackDisplay.Add(Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[(int)MonthFields.Total]) <= 0 ? "-" : Convert.ToString(initialHoldbackTotal));
            currentHoldbackDisplay.Add(Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[(int)MonthFields.Total]) <= 0 ? "-" : Convert.ToString(currentHoldbackTotal));
            holdbackReleaseDisplay.Add(Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[(int)MonthFields.Total]) <= 0 ? "-" : Convert.ToString(holdbackReleaseTotal));
            base.BuildRow("HoldbackSummaryRow", (string[])outstandingARDisplay.ToArray(typeof(string)));
            base.BuildRow("ARPercentageRow", (string[])arPercentageDisplay.ToArray(typeof(string)));
            base.BuildRow("InitialHoldbackRow", (string[])initialHoldbackDisplay.ToArray(typeof(string)));
            base.BuildRow("CurrentHoldbackRow", (string[])currentHoldbackDisplay.ToArray(typeof(string)));
            base.BuildRow("HoldbackReleaseRow", (string[])holdbackReleaseDisplay.ToArray(typeof(string)));
        }
        #endregion

        #region BuildPaymentSummary method
        /// <summary>TBD</summary>
        private void BuildPaymentSummary()
        {
            ArrayList mediaAmountDueDisplay = new ArrayList();
            ArrayList productionAmountDueDisplay = new ArrayList();
            ArrayList amountDueDisplay = new ArrayList();
            double arPercentage = 0.00;
            double amountDue = 0.00;
            double amountDueTotal = 0.00;
            double mediaAmountDue = 0.00;
            double mediaAmountDueTotal = 0.00;
            double productionAmountDue = 0.00;
            double productionAmountDueTotal = 0.00;
            for (int i = (int)MonthFields.January; i <= (int)MonthFields.December; i++)
            {
                arPercentage = Convert.ToDouble(MonthlySummaries.MonthlyARPercentage[i]);
                //productionAmountDue = Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionRelease[i]) * (i >= MonthlySummaries.monthDrawFulfilled ? 1 : Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionPercentage[i]));
                //productionAmountDue = Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionRelease[i]) * (i > MonthlySummaries.monthDrawFulfilled ? 1 : Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionPercentage[i]));
                if (i == MonthlySummaries.monthDrawFulfilled)
                {
                    productionAmountDue = Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) * Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionPercentage[i]);
                }
                else
                {
                    productionAmountDue = Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionRelease[i]);
                }
                productionAmountDueTotal += (i < (EndDate.Year > this.processYear ? 12 : EndDate.Month) && (productionAmountDue > 0 && Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) > 0)) ? productionAmountDue : 0.00;
                productionAmountDueDisplay.Add((i < (EndDate.Year > this.processYear ? 12 : EndDate.Month) && (productionAmountDue > 0 && Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) > 0)) ? Convert.ToString(productionAmountDue) : "-");
                mediaAmountDue = Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) - (Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) * Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionPercentage[i]));
                //mediaAmountDue *= ((arPercentage < (100.0 - HOLDBACK_PERCENTAGE)) ? (1 - ((arPercentage > 100) ? 100 : arPercentage) / 100) : (1 - (HOLDBACK_PERCENTAGE / 100.0)));
                mediaAmountDue *= ((arPercentage < HOLDBACK_PERCENTAGE) ? (1 - ((arPercentage > 100) ? 100 : arPercentage) / 100) : (1 - (HOLDBACK_PERCENTAGE / 100.0)));
                mediaAmountDueTotal += (i < (EndDate.Year > this.processYear ? 12 : EndDate.Month) && mediaAmountDue > 0) ? mediaAmountDue : 0.00;
                mediaAmountDueDisplay.Add((i < (EndDate.Year > this.processYear ? 12 : EndDate.Month) && mediaAmountDue > 0) ? Convert.ToString(mediaAmountDue) : "-");
                //amountDue = Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) * ((arPercentage < 25) ? (1 - ((arPercentage > 100) ? 100 : arPercentage) / 100) : 0.75);
                amountDue = productionAmountDue + mediaAmountDue;
                //amountDueDisplay.Add((i < (EndDate.Year > this.processYear ? 12 : EndDate.Month) && (Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) > 0)) ? Convert.ToString(amountDue) : "-");
                amountDueDisplay.Add((i < (EndDate.Year > this.processYear ? 12 : EndDate.Month) && amountDue > 0) ? Convert.ToString(amountDue) : "-");
                amountDueTotal += ((Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[i]) > 0) && (i < (EndDate.Year > this.processYear ? 12 : EndDate.Month))) ? amountDue : 0.00;
            }
            productionAmountDueDisplay.Add((Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[(int)MonthFields.Total]) <= 0) ? "-" : Convert.ToString(productionAmountDueTotal));
            mediaAmountDueDisplay.Add((Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[(int)MonthFields.Total]) <= 0) ? "-" : Convert.ToString(mediaAmountDueTotal));
            amountDueDisplay.Add((Convert.ToDouble(MonthlySummaries.MonthlyCommissionAccrual[(int)MonthFields.Total]) <= 0) ? "-" : Convert.ToString(amountDueTotal));
            if ((this.processYear >= 2010 && this.companyId == 1) || (this.processYear >= 2011 && this.companyId == 2))
            {
                base.BuildRow("PaymentSummaryRow", (string[])productionAmountDueDisplay.ToArray(typeof(string)));
                base.BuildRow("MediaAmountDueRow", (string[])mediaAmountDueDisplay.ToArray(typeof(string)));
                base.BuildRow("AmountDueRow", (string[])amountDueDisplay.ToArray(typeof(string)));
            }
            else
            {
                base.BuildRow("PaymentSummaryRowPre2010", (string[])amountDueDisplay.ToArray(typeof(string)));
            }
            base.BuildRow("PaymentsRow", new string[] { Convert.ToString(EndDate.Year) });
            base.BuildRow("AmountPayableRow", null);
        }
        #endregion

        #region BuildProductionSummary method
        /// <summary>TBD</summary>
        /// <param name="reportTable">TBD</param>
        private void BuildProductionSummary(DataTable reportTable)
        {
            ArrayList productionOutstandingARDisplay = new ArrayList();
            ArrayList productionHoldbackDisplay = new ArrayList();
            double productionOutstandingARTotal = 0.00;
            double productionHoldbackTotal = 0.00;
            for (int i = (int)MonthFields.January; i <= (int)MonthFields.December; i++)
            {
                //These fields are only displayed/aggregated through the last month of the report End Date
                if (i < (EndDate.Year > this.processYear ? 12 : EndDate.Month))
                {
                    //Production Commission/Outstanding Production AR change is effective 3/1/2010
                    if (this.companyId == 1 && (EndDate.Year > 2010 || (EndDate.Year == 2010 && i >= 2)) && i >= this.MonthlySummaries.monthDrawFulfilled)
                    {
                        productionOutstandingARDisplay.Add(Convert.ToString(MonthlySummaries.ProductionMonthlyOutstandingAR[i]));
                        productionHoldbackDisplay.Add(Convert.ToString(MonthlySummaries.MonthlyProductionCommissionHoldback[i]));
                        productionOutstandingARTotal += Convert.ToDouble(MonthlySummaries.ProductionMonthlyOutstandingAR[i]);
                        productionHoldbackTotal += Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionHoldback[i]);
                    }
                    else
                    {
                        productionOutstandingARDisplay.Add("-");
                        productionHoldbackDisplay.Add("-");
                        productionOutstandingARTotal += 0.00;
                        productionHoldbackTotal += 0.00;
                    }
                }
                else
                {
                    productionOutstandingARDisplay.Add("-");
                    productionHoldbackDisplay.Add("-");
                    productionOutstandingARTotal += 0.00;
                    productionHoldbackTotal += 0.00;
                }
            }
            productionOutstandingARDisplay.Add(Convert.ToDouble(MonthlySummaries.ProductionMonthlyOutstandingAR[(int)MonthFields.Total]) <= 0 ? "-" : Convert.ToString(productionOutstandingARTotal));
            productionHoldbackDisplay.Add(Convert.ToDouble(MonthlySummaries.MonthlyProductionCommissionHoldback[(int)MonthFields.Total]) <= 0 ? "-" : Convert.ToString(productionHoldbackTotal));
            base.BuildReportTotalRow("ProductionSummaryRow", reportTable, Enum.GetNames(typeof(ProductionTotalFields)));
            base.BuildRow("ProductionARRow", (string[])productionOutstandingARDisplay.ToArray(typeof(string)));
            base.BuildRow("ProductionHoldbackRow", (string[])productionHoldbackDisplay.ToArray(typeof(string)));
        }
        #endregion

        #region BuildReportHeaders method
        /// <summary>Builds the headers of the Excel report</summary>
        /// <param name="aeName">TBD</param>
        public void BuildReportHeaders(string aeName)
        {
            if (ReportYear < 2002)
            {
                throw new ArgumentOutOfRangeException("year", ReportYear, "Year cannot be less than 2002");
            }
            string[] values = new string[3];
            values[0] = Convert.ToString(ReportYear);
            values[1] = aeName;
            values[2] = EndDate.ToShortDateString();
            base.BuildRow(REPORT_HEADERS_TEMPLATE_NAME, values);
        }
        #endregion

        #region BuildReportRowWithComputedField method
        /// <summary>TBD</summary>
        /// <param name="templateName">TBD</param>
        /// <param name="row">TBD</param>
        /// <param name="reportFields">TBD</param>
        /// <param name="calc">TBD</param>
        private void BuildReportRowWithComputedField(string templateName, DataRow row, string[] reportFields, CalculationMethod calc)
        {
            ArrayList fieldValueList = new ArrayList();
            foreach (string field in reportFields)
            {
                fieldValueList.Add(Convert.ToString(row[field]));
            }
            //Add the computed field
            fieldValueList.Add(Convert.ToString(calc(row)));
            base.BuildRow(REPORT_ROW_TEMPLATE_NAME, (string[])fieldValueList.ToArray(typeof(string)));
        }
        #endregion

        #region BuildReportTotalRowWithComputedField method
        /// <summary>TBD</summary>
        /// <param name="templateName">TBD</param>
        /// <param name="reportData">TBD</param>
        /// <param name="totalFields">TBD</param>
        /// <param name="calc">TBD</param>
        private void BuildReportTotalRowWithComputedField(string templateName, DataTable reportData, string[] totalFields, TotalCalculationMethod calc)
        {
            ArrayList fieldValueList = new ArrayList();
            foreach (string field in totalFields)
            {
                fieldValueList.Add(Convert.ToString(reportData.Compute("SUM([" + field + "])", "")));
            }
            fieldValueList.Add(Convert.ToString(calc(reportData)));
            BuildRow(templateName, (string[])fieldValueList.ToArray(typeof(string)));
        }
        #endregion

        #region CalculateCommissionAmount method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        private double CalculateCommissionAmount(DataRow row)
        {
            if (Convert.ToInt32(row[(int)ReportFields.TotalAEAmount]) == 0)
            {
                return 0.00;
            }
            return Convert.ToDouble(row[(int)ReportFields.CommissionAmount]) * 100 / Convert.ToDouble(row[(int)ReportFields.TotalAEAmount]);
        }
        #endregion

        #region CalculateCommissionPercentage method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        private double CalculateCommissionPercentage(DataRow row)
        {
            if (Convert.ToInt32(row[(int)ReportFields.TotalBilling]) == 0)
            {
                return 0.00;
            }
            return Convert.ToDouble(row[(int)ReportFields.TotalBilling]) * 100 / Convert.ToDouble(row[(int)ReportFields.TotalAEAmount]);
        }
        #endregion

        #region CalculateTotalCommissionAmount method
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <returns>TBD</returns>
        private double CalculateTotalCommissionAmount(DataTable reportData)
        {
            if (Convert.ToInt32(reportData.Compute("SUM([" + Convert.ToString(Enum.Parse(typeof(ReportFields), "TotalAEAmount")) + "])", "")) == 0)
            {
                return 0.00;
            }
            return Convert.ToDouble(reportData.Compute("SUM([" + Convert.ToString(Enum.Parse(typeof(ReportFields), "CommissionAmount")) + "])", "")) * 100 / Convert.ToDouble(reportData.Compute("SUM([" + Convert.ToString(Enum.Parse(typeof(ReportFields), "TotalAEAmount")) + "])", ""));
        }
        #endregion

        #region CalculateTotalCommissionPercentage method
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <returns>TBD</returns>
        private double CalculateTotalCommissionPercentage(DataTable reportData)
        {
            if (Convert.ToInt32(reportData.Compute(string.Format("SUM([{0}])", Convert.ToString(Enum.Parse(typeof(ReportFields), "TotalBilling"))), "")) == 0)
            {
                return 0.00;
            }
            return Convert.ToDouble(reportData.Compute(string.Format("SUM([{0}])", Convert.ToString(Enum.Parse(typeof(ReportFields), "CommissionAmount"))), "")) * 100 / Convert.ToDouble(reportData.Compute(string.Format("SUM([{0}])", Convert.ToString(Enum.Parse(typeof(ReportFields), "TotalBilling"))), ""));
        }
        #endregion

        #region GenerateReport method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public MemoryStream GenerateReport()
        {
            MemoryStream reportStream = new MemoryStream();
            string report = string.Empty;
            WebCommon.WriteDebugMessage("Generating Flash Commission Report.");
            try
            {
                GetReportData(REPORT_STORED_PROC_NAME);
                //Need to do this twice:
                //  Once for the FC Report
                //  Once for the FC AR Report
                InnerGenerateReport();
                //  Run for the secondary Flash Commission AR Report:
                //  This uses a different section in the XML template
                base.ReportRoot = SECONDARY_REPORT_TEMPLATE_NAME;
                InnerGenerateSecondaryReport();
                using (ZipFile zip = new ZipFile())
                {
                    string fileName, secondaryFileName;
                    for (int i = 0; i < this.AEReports.Length; i++)
                    //for (int i = 0; i < this.AESecondaryReports.Length; i++)
                    {
                        if (hasData[i])
                        {
                            fileName = string.Format(this.ReportFileName, ReportYear, this.AENameList[i], EndDate.ToShortDateString().Replace("/", "-"));
                            secondaryFileName = string.Format(this.SecondaryReportFileName, ReportYear, this.AENameList[i], EndDate.ToShortDateString().Replace("/", "-"));
                            zip.AddEntry(fileName, ASCIIEncoding.ASCII.GetBytes(AEReports[i]));
                            zip.AddEntry(secondaryFileName, ASCIIEncoding.ASCII.GetBytes(AESecondaryReports[i]));
                        }
                    }
                    zip.Save(reportStream);
                }
                WebCommon.WriteDebugMessage("Flash Commission Report Generated.");
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new InvalidOperationException("An error occurred while generating the Flash Commission report.", ex);
            }
            return reportStream;
        }
        #endregion

        #region GetAECommisionEarnedAmountByMonth method
        /// <summary>TBD</summary>
        /// <param name="month">TBD</param>
        /// <returns>TBD</returns>
        private double GetAECommisionEarnedAmountByMonth(MonthFields month)
        {
            if (AECommissionData == null)
            {
                throw new InvalidOperationException("AE Commission Data does not exist");
            }
            object value = GetAmountByMonth(AECommissionDataTypes.CommissionEarned, month);
            return (value == null) ? 0.00 : Convert.ToDouble(value);
        }
        #endregion

        #region GetAECommissionData method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        private void GetAECommissionData(string aeId)
        {
            List<SqlParameter> spParams = new List<SqlParameter>();
            spParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, StartDate));
            spParams.Add(Param.CreateParam("ENDDATE", SqlDbType.Date, EndDate));
            spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, aeId));
            spParams.Add(Param.CreateParam("YEAR", SqlDbType.Int, ReportYear));
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, CompanyId));
            spParams.Add(Param.CreateParam("USEINVOICECREATIONDATE", SqlDbType.Int, (useInvoiceCreationDate ? -1 : 0)));
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                AECommissionData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(AE_COMMISION_DATA_STORED_PROC_NAME, spParams));
            }
        }
        #endregion

        #region GetAECommissionDataValue method
        /// <summary>TBD</summary>
        /// <param name="dataType">TBD</param>
        /// <param name="fieldIndex">TBD</param>
        /// <returns>TBD</returns>
        private object GetAECommissionDataValue(AECommissionDataTypes dataType, int fieldIndex)
        {
            if (AECommissionData == null)
            {
                throw new InvalidOperationException("AE Commission Data does not exist");
            }
            return AECommissionData.Tables[(int)dataType].Rows[0][fieldIndex];
        }
        #endregion

        #region GetAEDrawAmount method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private double GetAEDrawAmount()
        {
            if (AECommissionData == null)
            {
                throw new InvalidOperationException("AE Commission Data does not exist");
            }
            return (AECommissionData.Tables[1].Rows.Count == 0) ? 0.00 : Convert.ToDouble(GetAECommissionDataValue(AECommissionDataTypes.Draw, 0));
        }
        #endregion

        #region GetAEPaymentAmountByMonth method
        /// <summary>TBD</summary>
        /// <param name="month">TBD</param>
        /// <returns>TBD</returns>
        private double GetAEPaymentAmountByMonth(MonthFields month)
        {
            object value = GetAmountByMonth(AECommissionDataTypes.Payments, month);
            return (value == null) ? 0.00 : Convert.ToDouble(value);
        }
        #endregion

        #region GetAETotalCommissionEarnedAmount method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private double GetAETotalCommissionEarnedAmount()
        {
            return GetAECommisionEarnedAmountByMonth(MonthFields.Total);
        }
        #endregion

        #region GetAmountByMonth method
        /// <summary>TBD</summary>
        /// <param name="dataType">TBD</param>
        /// <param name="month">TBD</param>
        /// <returns>TBD</returns>
        private object GetAmountByMonth(AECommissionDataTypes dataType, MonthFields month)
        {
            return GetAECommissionDataValue(dataType, (int)month);
        }
        #endregion

        #region GetReportData method
        /// <summary>Executes the stored procedure that retrieves the report data</summary>
        /// <param name="storedProc">The name of the stored procedure to execute</param>
        public override void GetReportData(string storedProc)
        {
            List<SqlParameter> reportParams = new List<SqlParameter>();
            reportParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, StartDate));
            reportParams.Add(Param.CreateParam("ENDDATE", SqlDbType.Date, EndDate));
            reportParams.Add(Param.CreateParam("USEINVOICECREATIONDATE", SqlDbType.Int, (useInvoiceCreationDate ? -1 : 0)));
            reportParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, CompanyId));
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc(storedProc, reportParams))
                {
                    cmd.CommandTimeout = 200;
                    this.ReportData = io.ExecuteDataSetQuery(cmd);
                }
            }
        }
        #endregion

        #region InnerGenerateReport method
        /// <summary>TBD</summary>
        private void InnerGenerateReport()
        {
            if (ReportData == null)
            {
                throw new InvalidOperationException("ReportData cannot be null");
            }
            if (ReportData.Tables[0].Rows.Count == 0)
            {
                //No data for this report
                return;
            }
            AEReports = new string[this.AEFilterList.Length];
            AENameList = new string[this.AEFilterList.Length];
            hasData = new bool[this.AEFilterList.Length];
            string filterExpression = string.Empty;
            DataTable table = new DataTable();
            for (int i = 0; i < this.AEFilterList.Length; i++)
            {
                //Process Data and Build Report
                filterExpression = "AE_ID = '" + this.AEFilterList[i].Replace("'", "''") + "'";
                table = new DataTable();
                table = ReportData.Tables[0].Clone();
                foreach (DataRow row in ReportData.Tables[0].Select(filterExpression))
                {
                    table.ImportRow(row);
                }
                hasData[i] = (table.Rows.Count > 0);
                AENameList[i] = (hasData[i]) ? Convert.ToString(table.Rows[0]["Salesman"]) : string.Empty;
                if (hasData[i])
                {
                    GetAECommissionData(this.AEFilterList[i].Replace("'", "''"));
                    StartReport();
                    BuildReportHeaders(this.AENameList[i]);
                    //Step 1: Output Contract Row Data
                    foreach (DataRow row in table.Rows)
                    {
                        //old Commission Percentage Calc: Commission Amount / AE Amount
                        BuildReportRowWithComputedField(REPORT_ROW_TEMPLATE_NAME, row, Enum.GetNames(typeof(ReportFields)), CalculateCommissionAmount);
                        //new Commission Percentage Calc: Billing Amount / AE Amount
                        //BuildReportRowWithComputedField(REPORT_ROW_TEMPLATE_NAME, row, Enum.GetNames(typeof(ReportFields)), CalculateCommissionPercentage);
                    }
                    MonthlySummaries.Initialize(table, AECommissionData, EndDate, processYear, this.companyId);
                    BuildCommissionSummary(table);
                    BuildHoldbackSummary(table);
                    if ((this.processYear >= 2010 && this.companyId == 1) || (this.processYear >= 2011 && this.companyId == 2))
                    {
                        BuildProductionSummary(table);
                    }
                    BuildPaymentSummary();
                    FinalizeReport();
                    AEReports[i] = base.GetReportOutput();
                }
                else
                {
                    AEReports[i] = string.Empty;
                }
            }
        }
        #endregion

        #region InnerGenerateSecondaryReport method
        /// <summary>TBD</summary>
        private void InnerGenerateSecondaryReport()
        {
            if (ReportData == null)
            {
                throw new InvalidOperationException("ReportData cannot be null");
            }
            if (ReportData.Tables[0].Rows.Count == 0)
            {
                //No data for this report
                return;
            }
            AESecondaryReports = new string[this.AEFilterList.Length];
            AENameList = new string[this.AEFilterList.Length];
            hasData = new bool[this.AEFilterList.Length];
            string filterExpression = string.Empty;
            DataTable table = new DataTable();
            for (int i = 0; i < this.AEFilterList.Length; i++)
            {
                //Process Data and Build Report
                filterExpression = "AE_ID = '" + this.AEFilterList[i].Replace("'", "''") + "'";
                table = new DataTable();
                table = ReportData.Tables[0].Clone();
                foreach (DataRow row in ReportData.Tables[0].Select(filterExpression))
                {
                    table.ImportRow(row);
                }
                hasData[i] = (table.Rows.Count > 0);
                AENameList[i] = (hasData[i]) ? Convert.ToString(table.Rows[0]["Salesman"]) : string.Empty;
                StartReport();
                BuildReportHeaders(this.AENameList[i]);
                foreach (DataRow row in table.Rows)
                {
                    BuildReportRow(REPORT_ROW_TEMPLATE_NAME, row, Enum.GetNames(typeof(SecondaryReportFields)));
                }
                BuildReportTotalRow(REPORT_TOTAL_ROW_TEMPLATE_NAME, table, Enum.GetNames(typeof(SecondaryTotalFields)));
                FinalizeReport();
                AESecondaryReports[i] = base.GetReportOutput();
            }
        }
        #endregion

        #region ReportAttachmentName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ReportAttachmentName
        {
            get
            {
                return string.Format(reportAttachmentName, StartDate.ToShortDateString().Replace("/", "-"), EndDate.ToShortDateString().Replace("/", "-"), (CompanyId == 1 ? "USA" : "CAN"));
            }
        }
        #endregion

    }

}
