#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ionic.Zip;
using Titan.Email;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class sales_sales_reports : System.Web.UI.Page
    {

        #region ReportOptions struct
        /// <summary>
        ///     Structure used for passing values to Parameterized thread methods
        ///     Parameterized Thread methods can only accept a parameter of type 'Object'
        ///     Use this structure to pass necessary information into the Thread.
        /// </summary>
        public struct ReportOptions
        {

            #region Member variables
            /// <summary>TBD</summary>
            int processYear;
            /// <summary>TBD</summary>
            ReportType selectedReportType;
            /// <summary>TBD</summary>
            string userId;
            int companyId;
            #endregion

            public int CompanyId { get { return companyId; } set { companyId = value; } }

            #region ProcessYear property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public int ProcessYear
            {
                get { return processYear; }
                set { processYear = value; }
            }
            #endregion

            #region SelectedReportType property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public ReportType SelectedReportType
            {
                get { return selectedReportType; }
                set { selectedReportType = value; }
            }
            #endregion

            #region UserId property
            /// <summary>TBD</summary>
            /// <value>TBD</value>
            public string UserId
            {
                get { return userId; }
                set { userId = value; }
            }
            #endregion

        }
        #endregion

        #region ReportType enumeration
        public enum ReportType
        {
            AccountingCommission,
            AccountingCommissionByAE,
            CommissionAnalysis,
            FlashCommissionByAE,
            CommissionByMarket,
            None,
        }
        #endregion

        #region Member variables
        /// <summary>Dummy user for archiving files</summary>
        private const string FOR_ARCHIVE_USER_NAME = "XXX_ARCHIVE_XXX";
        /// <summary>Status message to display when commissions are being processed.</summary>
        private const string PROCESS_STATUS_MESSAGE = "Commissions are currently being processed.";
        /// <summary>Status message to display when a user requests a report.</summary>
        private const string STATUS_MESSAGE = "<br/>Your Report is being generated. It will be emailed to you shortly.";
        #endregion

        #region ArchiveReports method
        /// <summary>Maintains a copy of a report when the processing routine is executed</summary>
        /// <param name="reportName">The name of the file to archive</param>
        /// <param name="report">The MemoryStream object containing the report data</param>
        public static void ArchiveReports(string reportName, MemoryStream report)
        {
            //Keep an archive of a report on the day it is generated (only keep one copy of a report per day)
            string archivePathForYear = string.Format(HostingEnvironment.MapPath(HostingEnvironment.ApplicationVirtualPath + "/admin/commission_reports/{0}"), DateTime.Now.Year);
            string archivePathForMonth = string.Format(archivePathForYear + "/{0}", DateTime.Now.Month);
            string archivePathForDay = string.Format(archivePathForMonth + "/{0}", DateTime.Now.Day);
            //Make sure the folder for the current year exists:
            if (!Directory.Exists(archivePathForYear))
            {
                Directory.CreateDirectory(archivePathForYear);
            }
            //Make sure the folder for the current month exists:
            if (!Directory.Exists(archivePathForMonth))
            {
                Directory.CreateDirectory(archivePathForMonth);
            }
            //Make sure the folder for the current day exists:
            if (!Directory.Exists(archivePathForDay))
            {
                //We only write out the files if the folder for the current day did not already exist
                Directory.CreateDirectory(archivePathForDay);
            }
            using (FileStream archive = File.Open(archivePathForDay + "/" + reportName, FileMode.Create, FileAccess.Write))
            {
                report.Seek(0, SeekOrigin.Begin);
                report.WriteTo(archive);
            }
        }
        #endregion

        #region EmailReport method
        /// <summary>Emails the report contained in the Attached MemoryStream to the specified user</summary>
        /// <param name="userId">The requestor/recipient of the report</param>
        /// <param name="attachmentName">The name of the Email attachment</param>
        /// <param name="report">MemoryStream object containing the report data</param>
        /// <param name="startTime">The DateTime that the report was requested</param>
        /// <param name="endTime">The DateTime that the report was completed</param>
        public void EmailReport(string userId, string attachmentName, MemoryStream report, DateTime startTime, DateTime endTime)
        {
            try
            {
                TimeSpan span = endTime.Subtract(startTime);
                string elapsedTime = span.Minutes + " minute" + ((span.Minutes != 1) ? "s, " : ", ") + span.Seconds + " second" + ((span.Seconds != 1) ? "s" : "") + ".";
                StringBuilder body = new StringBuilder();
                body.Append("Report requested on: ");
                body.AppendLine(startTime.ToShortDateString() + " at " + startTime.ToShortTimeString());
                //body.Append("<br/>Elapsed time: ");
                //body.AppendLine(elapsedTime);
                Message mail = new Message(WebCommon.SmtpAddress);
                mail.AddRecipient(Security.GetUserEmailFromId(userId));
                mail.Subject = "Titan 360 Commission Report";
                mail.From = ConfigurationManager.AppSettings["mailingName"].ToString();
                mail.Body = body.ToString();
                mail.AddAttachmentFromStream(report, attachmentName, "application/zip");
                WebCommon.WriteDebugMessage(string.Format("Emailing report at {0} to {1}, file size: {2}.", (startTime.ToShortDateString() + " at " + startTime.ToShortTimeString()), userId, report.Length));
                mail.SendEmail();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                EmailReportErrorMessage(userId, startTime);
            }
        }
        #endregion

        #region EmailReportErrorMessage method
        /// <summary>Sends an email to the requestor of the report indicating that report generation failed.</summary>
        /// <param name="userId">The requestor/recipient of the report</param>
        /// <param name="startTime">The DateTime that the report was requested</param>
        public void EmailReportErrorMessage(string userId, DateTime startTime)
        {
            try
            {
                StringBuilder body = new StringBuilder();
                body.Append("An error occurred while generating the report requested on: ");
                body.AppendLine(startTime.ToShortDateString() + " at " + startTime.ToShortTimeString());
                body.AppendLine("<br/>Details of this error have been logged. Please try your operation again.");
                body.AppendLine("<br/>If this problem persists, please contact technical support.");
                Message mail = new Message(WebCommon.SmtpAddress);
                mail.AddRecipient(Security.GetUserEmailFromId(userId));
                mail.Subject = "Titan 360 Commission Report - Error";
                mail.From = ConfigurationManager.AppSettings["mailingName"].ToString();
                mail.Body = body.ToString();
                mail.SendEmail();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
        }
        #endregion

        #region ExecuteACRAEReport method
        /// <summary>Executes the Accounting Commission By AE report.</summary>
        /// <param name="reportOptions">
        ///     Structure used for passing values to Parameterized thread methods
        ///     Parameterized Thread methods can only accept a parameter of type 'Object'
        ///     Use this structure to pass necessary information into the Thread.
        /// </param>
        public void ExecuteACRAEReport(object reportOptions)
        {
            DateTime startTime = DateTime.Now;
            string userId = ((ReportOptions)reportOptions).UserId;
            try
            {
                string[] selectedAEs = ((ReportOptions)reportOptions).CompanyId == 1 ? GetSelectedAEs(listACRAE) : GetSelectedAEs(listACRAECan);
                AccountingCommissionByAEReportBuilder report = new AccountingCommissionByAEReportBuilder(selectedAEs, Convert.ToDateTime(textACRAELineItemStartDate.Text), Convert.ToDateTime(textACRAELineItemEndDate.Text), ((ReportOptions)reportOptions).CompanyId);
                WebCommon.WriteDebugMessage("Accounting Commission by AE Report executed. Zipping and Emailing report.");
                using (MemoryStream reportStream = report.GenerateReport())
                {
                    DateTime endTime = DateTime.Now;
                    if (String.Compare(userId, FOR_ARCHIVE_USER_NAME, true) != 0)
                    {
                        EmailReport(userId, report.ReportAttachmentName, reportStream, startTime, endTime);
                    }
                    else
                    {
                        string reportName = "AccountingCommissionConslidatedByAE_Report.zip";
                        ArchiveReports(reportName, reportStream);
                    }
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                EmailReportErrorMessage(userId, startTime);
            }
        }
        #endregion

        #region ExecuteACRReport method
        /// <summary>Executes the Accounting Commission report.</summary>
        /// <param name="reportOptions">
        ///     Structure used for passing values to Parameterized thread methods
        ///     Parameterized Thread methods can only accept a parameter of type 'Object'
        ///     Use this structure to pass necessary information into the Thread.
        /// </param>
        public void ExecuteACRReport(object reportOptions)
        {
            DateTime startTime = DateTime.Now;
            string userId = ((ReportOptions)reportOptions).UserId;
            try
            {
                CommissionReportBuilder.ReportOption requestedOption;
                if (radioACRWantAllActiveAEs.Checked)
                {
                    requestedOption = CommissionReportBuilder.ReportOption.ActiveAEsOnly;
                }
                else if (radioACRWantAllActiveAEsWOHidden.Checked)
                {
                    requestedOption = CommissionReportBuilder.ReportOption.ActiveAEsWithoutHidden;
                }
                else
                {
                    requestedOption = CommissionReportBuilder.ReportOption.AllAEs;
                }
                AccountingCommissionReportBuilder report = new AccountingCommissionReportBuilder(Convert.ToDateTime(textACRLineItemStartDate.Text), Convert.ToDateTime(textACRLineItemEndDate.Text), requestedOption, ((ReportOptions)reportOptions).CompanyId);
                WebCommon.WriteDebugMessage("Accounting Commission Report executed. Zipping and Emailing report.");
                using (MemoryStream reportStream = report.GenerateReport())
                {
                    DateTime endTime = DateTime.Now;
                    if (String.Compare(userId, FOR_ARCHIVE_USER_NAME, true) != 0)
                    {
                        EmailReport(userId, report.ReportAttachmentName, reportStream, startTime, endTime);
                    }
                    else
                    {
                        string reportName = "AccountingCommissionConsolidated_Report.zip";
                        ArchiveReports(reportName, reportStream);
                    }
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                EmailReportErrorMessage(userId, startTime);
            }
        }
        #endregion

        #region ExecuteCommissionAnalysisReport method
        /// <summary>TBD</summary>
        /// <param name="reportOptions">TBD</param>
        public void ExecuteCommissionAnalysisReport(object reportOptions)
        {
            DateTime startTime = DateTime.Now;
            string userId = ((ReportOptions)reportOptions).UserId;
            try
            {
                CommissionReportBuilder.ReportOption requestedOption;
                if (radioCommAnalysisAllActiveAEs.Checked)
                {
                    requestedOption = CommissionReportBuilder.ReportOption.ActiveAEsOnly;
                }
                else if (radioCommAnalysisAllActiveAEsWOHidden.Checked)
                {
                    requestedOption = CommissionReportBuilder.ReportOption.ActiveAEsWithoutHidden;
                }
                else
                {
                    requestedOption = CommissionReportBuilder.ReportOption.AllAEs;
                }
                WebCommon.WriteDebugMessage("Generating Commission Analysis Report.");
                CommissionAnalysisReportBuilder report = new CommissionAnalysisReportBuilder(Convert.ToDateTime(textAsOfDate.Text), requestedOption, ((ReportOptions)reportOptions).CompanyId);
                using (MemoryStream reportStream = report.GenerateReport())
                {
                    DateTime endTime = DateTime.Now;
                    if (String.Compare(userId, FOR_ARCHIVE_USER_NAME, true) != 0)
                    {
                        EmailReport(userId, report.ReportAttachmentName, reportStream, startTime, endTime);
                    }
                    else
                    {
                        string reportName = "CommissionAnalysis_Report.zip";
                        ArchiveReports(reportName, reportStream);
                    }
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                EmailReportErrorMessage(userId, startTime);
            }
        }
        #endregion

        #region ExecuteCommissionByMarketReport method
        /// <summary>TBD</summary>
        /// <param name="reportOptions">TBD</param>
        public void ExecuteCommissionByMarketReport(object reportOptions)
        {
            DateTime startTime = DateTime.Now;
            string userId = ((ReportOptions)reportOptions).UserId;
            try
            {
                CommissionReportBuilder.ReportOption requestedOption;
                if (radioCommByMarketAllActiveAEs.Checked)
                {
                    requestedOption = CommissionReportBuilder.ReportOption.ActiveAEsOnly;
                }
                else if (radioCommByMarketAllActiveAEsWOHidden.Checked)
                {
                    requestedOption = CommissionReportBuilder.ReportOption.ActiveAEsWithoutHidden;
                }
                else
                {
                    requestedOption = CommissionReportBuilder.ReportOption.AllAEs;
                }
                WebCommon.WriteDebugMessage("Generating Commission By Market Report.");
                AECommissionByMarketReportBuilder report = new AECommissionByMarketReportBuilder(Convert.ToDateTime(textCommissionByMarketAsOFDate.Text), requestedOption, ((ReportOptions)reportOptions).CompanyId);
                using (MemoryStream reportStream = report.GenerateReport())
                {
                    DateTime endTime = DateTime.Now;
                    if (String.Compare(userId, FOR_ARCHIVE_USER_NAME, true) != 0)
                    {
                        EmailReport(userId, report.ReportAttachmentName, reportStream, startTime, endTime);
                    }
                    else
                    {
                        string reportName = "AECommissionByMarket_Report.zip";
                        ArchiveReports(reportName, reportStream);
                    }
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                EmailReportErrorMessage(userId, startTime);
            }
        }
        #endregion

        #region ExecuteFlashCommissionReport method
        /// <summary>Executes the Flash Commission report.</summary>
        /// <param name="reportOptions">
        ///     Structure used for passing values to Parameterized thread methods
        ///     Parameterized Thread methods can only accept a parameter of type 'Object'
        ///     Use this structure to pass necessary information into the Thread.
        /// </param>
        public void ExecuteFlashCommissionReport(object reportOptions)
        {
            DateTime startTime = DateTime.Now;
            string userId = ((ReportOptions)reportOptions).UserId;
            try
            {
                string[] selectedAEs = ((ReportOptions)reportOptions).CompanyId == 1 ? GetSelectedAEs(listFlashAE) : GetSelectedAEs(listFlashAECan);
                WebCommon.WriteDebugMessage("Generating Flash Report for " + listFlashAE.Items.Count + " AEs.");
                FlashCommissionReportBuilder report = new FlashCommissionReportBuilder(selectedAEs, Convert.ToDateTime(textFlashFromDate.Text), Convert.ToDateTime(textFlashThruDate.Text), ((ReportOptions)reportOptions).ProcessYear, radioFlashByInvoice.Checked, ((ReportOptions)reportOptions).CompanyId);
                using (MemoryStream reportStream = report.GenerateReport())
                {
                    DateTime endTime = DateTime.Now;
                    if (String.Compare(userId, FOR_ARCHIVE_USER_NAME, true) != 0)
                    {
                        EmailReport(userId, report.ReportAttachmentName, reportStream, startTime, endTime);
                        
                    }
                    else
                    {
                        string reportName = "FlashCommissionConsolidated_Report.zip";
                        ArchiveReports(reportName, reportStream);
                    }
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                EmailReportErrorMessage(userId, startTime);
            }
        }
        #endregion

        #region GetSelectedAEs method
        /// <summary>Builds a string array of the selected AE values</summary>
        /// <param name="list">The ListBox control used in the search criteria.</param>
        /// <returns>A string array containing the selected ListBox values</returns>
        private string[] GetSelectedAEs(ListBox list)
        {
            ArrayList selectedAes = new ArrayList();
            ArrayList selectedAeText = new ArrayList();
            bool wantAllAes = (list.Items[0].Selected);
            bool wantAllActiveAes = (list.Items[1].Selected);
            bool wantAllActiveAesWithoutHidden = (list.Items[2].Selected);
            string aeId = string.Empty;
            foreach (ListItem ae in list.Items)
            {
                //  skip the first two values (*, AA)
                //  wantAllAes - select everyone
                //  wantAllActiveAes && Value is Active - select only actives
                //  otherwise use selected
                if ((!((ae.Value == "*") || (ae.Value == "AA") || (ae.Value == "AAH")))
                && (wantAllAes
                || (wantAllActiveAes && ae.Value.Substring(0, 1) == "A")
                || (wantAllActiveAesWithoutHidden && ae.Value.Substring(0, 1) == "A" && ae.Value.Substring(2, 1) == "V")
                || ae.Selected))
                {
                    //Make sure the AE name is only chosen once, even if they have multiple IDs associated with it
                    aeId = ae.Value.Substring(4, (ae.Value.Length - 4));
                    if ((!selectedAes.Contains(aeId)) && (!selectedAeText.Contains(ae.Text)))
                    {
                        selectedAes.Add(aeId);
                        selectedAeText.Add(ae.Text);
                    }
                }
            }
            return (string[])selectedAes.ToArray(typeof(string));
        }
        #endregion

        #region HasArchiveForToday property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool HasArchiveForToday
        {
            get
            {
                //Keep an archive of a report on the day it is generated (only keep one copy of a report per day)
                string archivePathForYear = string.Format(HostingEnvironment.MapPath(HostingEnvironment.ApplicationVirtualPath + "/admin/commission_reports/{0}"), DateTime.Now.Year);
                string archivePathForMonth = string.Format(archivePathForYear + "/{0}", DateTime.Now.Month);
                string archivePathForDay = string.Format(archivePathForMonth + "/{0}", DateTime.Now.Day);
                //Make sure the folder for the current year exists:
                if (!Directory.Exists(archivePathForYear))
                {
                    return false;
                }
                //Make sure the folder for the current month exists:
                if (!Directory.Exists(archivePathForMonth))
                {
                    return false;
                }
                //Make sure the folder for the current day exists:
                if (!Directory.Exists(archivePathForDay))
                {
                    return false;
                }
                return true;
            }
        }
        #endregion

        #region IsProcessing property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        private bool IsProcessing
        {
            get
            {
                return (bool?)Cache["isProcessingCommissions"] ?? false;
            }
            set
            {
                Cache["isProcessingCommissions"] = value;
            }
        }
        #endregion

        #region LockDown method
        /// <summary>TBD</summary>
        private void LockDown()
        {
            if (IsProcessing)
            {
                runACRReport.Visible = false;
                runARCAEReport.Visible = false;
                runFlashReport.Visible = false;
                reportDone.Visible = false;
                isProcessing.Value = "1";
                ShowWorkingMessage(PROCESS_STATUS_MESSAGE);
            }
            else
            {
                isProcessing.Value = "0";
            }
        }
        #endregion

        #region Page_Error method
        /// <summary>Captures any unhandled exceptions that occur</summary>
        protected void Page_Error()
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            Server.ClearError();
            WebCommon.LogExceptionInfo(ex);
        }
        #endregion

        #region Page_Init method
        /// <summary>Event that fires when the page is first initialized.</summary>
        protected void Page_Init()
        {
            if (!Security.IsAdminUser() && !Security.IsCorporateUser())
            {
                WebCommon.ShowAlert("You are not authorized to use this portion of the application.");
                Server.Transfer("/Default.aspx");
            }
            aeDataSource.ConnectionString = WebCommon.ConnectionString;
            aeDataSourceCan.ConnectionString = WebCommon.ConnectionString;
        }
        #endregion

        #region Page_Load method
        /// <summary>Event fired when the page first loads.</summary>
        /// <param name="sender">Object firing the event.</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProcessYears();
                SetDefaultFieldValues();
                PopulateProcessingFields();
                LockDown();
            }
        }
        #endregion

        #region PopulateProcessingFields method
        /// <summary>Display the commission processing information</summary>
        private void PopulateProcessingFields()
        {
            DataSet processingInfo;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                processingInfo = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Commissions_GetProcessingStatus"));
            }
            IsProcessing = ((string)IO.GetDataRowValue(processingInfo.Tables[0].Rows[0], "IS_PROCESSING_COMMISSIONS", "") == "1");
            labelProcessingStatus.Text = ((string)IO.GetDataRowValue(processingInfo.Tables[0].Rows[0], "IS_PROCESSING_COMMISSIONS", "") == "0" ? "Done" : "Processing Commissions");
            labelProcessTime.Text = (string)IO.GetDataRowValue(processingInfo.Tables[0].Rows[0], "LAST_PROCESS_TIMESTAMP", "");
            labelProcessYear.Text = (string)IO.GetDataRowValue(processingInfo.Tables[0].Rows[0], "LAST_PROCESS_YEAR", "");
        }
        #endregion

        #region PopulateProcessYears method
        /// <summary>TBD</summary>
        private void PopulateProcessYears()
        {
            int currentYear = DateTime.Now.Year;
            int stopYear = 2006;
            for (int i = currentYear + 1; i >= stopYear; i--)
            {
                dropDownProcessYear.Items.Add(new ListItem(Convert.ToString(i), Convert.ToString(i)));
            }
            dropDownProcessYear.SelectedValue = Convert.ToString(currentYear);
        }
        #endregion

        #region ProcessCommissions method
        /// <summary>Calculates the commission values for contracts</summary>
        /// <param name="requestedReportType">TBD</param>
        private void ProcessCommissions(ReportType requestedReportType)
        {
            try
            {
                IsProcessing = true;
                ReportOptions reportOptions = new ReportOptions();
                reportOptions.UserId = Security.GetCurrentUserId;
                reportOptions.SelectedReportType = requestedReportType;
                Thread reportThread;
                reportThread = new Thread(new ParameterizedThreadStart(ProcessCommissionsThread));
                reportThread.Start(reportOptions);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                IsProcessing = false;
                WebCommon.LogExceptionInfo(ex);
            }
        }
        #endregion

        #region ProcessCommissionsThread method
        /// <summary>TBD</summary>
        /// <param name="reportOptions">TBD</param>
        private void ProcessCommissionsThread(object reportOptions)
        {
            try
            {
                //Allow sufficient time for this query to complete.
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = IO.CreateCommandFromStoredProc("Commissions_ProcessAndRebuildCommissions"))
                    {
                        cmd.Parameters.Add(Param.CreateParam("YEAR", SqlDbType.Int, Convert.ToInt32(dropDownProcessYear.SelectedValue)));
                        cmd.Parameters.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, 1));
                        cmd.CommandTimeout = 600;
                        io.ExecuteActionQuery(cmd);
                    }
                }
                if (((ReportOptions)reportOptions).SelectedReportType != ReportType.None)
                {
                    //First, run the report that the user requested
                    RunReportThread(((ReportOptions)reportOptions).SelectedReportType, "", false, (ReportOptions?)reportOptions);
                    //If this is the first time commissions have been processed for the day
                    //  archive the reports using default values
                    if (!HasArchiveForToday)
                    {
                        //Set the Default values for the archived reports:
                        SetDefaultFieldValues();
                        //Execute and Archive the Accounting Commission Report
                        RunReportThread(ReportType.AccountingCommission, "", true);
                        //Execute and Archive the Accounting Commission by AE Report
                        //  Simulate a selection of 'All' AEs for the Account Commission report
                        listACRAE.Items[0].Selected = true;
                        RunReportThread(ReportType.AccountingCommissionByAE, "", true);
                        //Execute and Archive the Flash Commission Report
                        //  Simulate a selection of 'All' AEs for the Flash report
                        listFlashAE.Items[0].Selected = true;
                        RunReportThread(ReportType.FlashCommissionByAE, PROCESS_STATUS_MESSAGE, true);
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            finally
            {
                IsProcessing = false;
                LockDown();
                PopulateProcessingFields();
            }
        }
        #endregion

        #region runACRReport_Click method
        /// <summary>Event fired when the Accounting Commission report button is clicked.</summary>
        /// <param name="sender">Object firing the event.</param>
        /// <param name="e">Event arguments</param>
        protected void runACRReport_Click(object sender, ImageClickEventArgs e)
        {
            if (checkProcessCommissions.Checked)
            {
                WebCommon.WriteDebugMessage("Processing Commissions and Executing Accounting Commission Report.\nFor User: ." + Security.GetCurrentUserId);
                ProcessCommissions(ReportType.AccountingCommission);
                LockDown();
            }
            else
            {
                WebCommon.WriteDebugMessage("Executing Accounting Commission Report.\nFor User: ." + Security.GetCurrentUserId);
                RunReportThread(ReportType.AccountingCommission, STATUS_MESSAGE, false);
            }
        }
        #endregion

        #region runAnalysisReport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void runAnalysisReport_Click(object sender, ImageClickEventArgs e)
        {
            if (checkProcessCommissions.Checked)
            {
                WebCommon.WriteDebugMessage("Processing Commissions and Executing Commission Analysis Report.\nFor User: ." + Security.GetCurrentUserId);
                ProcessCommissions(ReportType.CommissionAnalysis);
                LockDown();
            }
            else
            {
                WebCommon.WriteDebugMessage("Executing Commission Analysis Report.\nFor User: ." + Security.GetCurrentUserId);
                RunReportThread(ReportType.CommissionAnalysis, STATUS_MESSAGE, false);
            }
        }
        #endregion

        #region runARCAEReport_Click method
        /// <summary>Event fired when the Accounting Commission By AE report button is clicked.</summary>
        /// <param name="sender">Object firing the event.</param>
        /// <param name="e">Event arguments</param>
        protected void runARCAEReport_Click(object sender, ImageClickEventArgs e)
        {
            if (checkProcessCommissions.Checked)
            {
                WebCommon.WriteDebugMessage("Processing Commissions and Executing Accounting Commission by AE Report.\nFor User: ." + Security.GetCurrentUserId);
                ProcessCommissions(ReportType.AccountingCommissionByAE);
                LockDown();
            }
            else
            {
                WebCommon.WriteDebugMessage("Executing Accounting Commission by AE Report.\nFor User: ." + Security.GetCurrentUserId);
                RunReportThread(ReportType.AccountingCommissionByAE, STATUS_MESSAGE, false);
            }
        }
        #endregion

        #region runCommissionByMarketReport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void runCommissionByMarketReport_Click(object sender, ImageClickEventArgs e)
        {
            if (checkProcessCommissions.Checked)
            {
                WebCommon.WriteDebugMessage("Processing Commissions and Executing AE Commission By Market Report.\nFor User: ." + Security.GetCurrentUserId);
                ProcessCommissions(ReportType.CommissionByMarket);
                LockDown();
            }
            else
            {
                WebCommon.WriteDebugMessage("Executing AE Commission By Market Report.\nFor User: ." + Security.GetCurrentUserId);
                RunReportThread(ReportType.CommissionByMarket, STATUS_MESSAGE, false);
            }
        }
        #endregion

        #region runFlashReport_Click method
        /// <summary>Event fired when the Flash Commission report button is clicked.</summary>
        /// <param name="sender">Object firing the event.</param>
        /// <param name="e">Event arguments</param>
        protected void runFlashReport_Click(object sender, ImageClickEventArgs e)
        {
            if (checkProcessCommissions.Checked)
            {
                WebCommon.WriteDebugMessage("Processing Commissions and Executing Flash Commission Report.\nFor User: ." + Security.GetCurrentUserId);
                ProcessCommissions(ReportType.FlashCommissionByAE);
                LockDown();
            }
            else
            {
                WebCommon.WriteDebugMessage("Executing Flash Commission Report.\nFor User: ." + Security.GetCurrentUserId);
                RunReportThread(ReportType.FlashCommissionByAE, STATUS_MESSAGE, false);
            }
        }
        #endregion

        #region runProcessCommissions_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void runProcessCommissions_Click(object sender, ImageClickEventArgs e)
        {
            WebCommon.WriteDebugMessage("Processing Commissions.\nFor User: " + Security.GetCurrentUserId);
            ProcessCommissions(ReportType.None);
        }
        #endregion

        #region RunReportThread method (report, message, isProcessing)
        /// <summary>TBD</summary>
        /// <param name="report">TBD</param>
        /// <param name="message">TBD</param>
        /// <param name="isProcessing">TBD</param>
        private void RunReportThread(ReportType report, string message, bool isProcessing)
        {
            RunReportThread(report, message, isProcessing, null);
        }
        #endregion

        #region RunReportThread method (report, message, isProcessing, reportOptions)
        /// <summary>Launches a new thread and executes a commission report</summary>
        /// <param name="report">The type of report to run</param>
        /// <param name="message">The status message to display to the user</param>
        /// <param name="isProcessing">Whether or not this report is being run as part of the processing routine</param>
        /// <param name="reportOptions">TBD</param>
        private void RunReportThread(ReportType report, string message, bool isProcessing, ReportOptions? reportOptions)
        {
            Thread reportThread;
            ReportOptions theReportOptions;
            if (reportOptions == null)
            {
                theReportOptions = new ReportOptions();
                theReportOptions.UserId = (!isProcessing) ? Security.GetCurrentUserId : FOR_ARCHIVE_USER_NAME;
                theReportOptions.SelectedReportType = report;
                theReportOptions.ProcessYear = Convert.ToInt32(labelProcessYear.Text);
            }
            else
            {
                theReportOptions = (ReportOptions)reportOptions;
            }
            switch (report)
            {
                case ReportType.AccountingCommission:
                    theReportOptions.CompanyId = Convert.ToInt32(dropDownACRptCompany.SelectedValue);
                    reportThread = new Thread(new ParameterizedThreadStart(ExecuteACRReport));
                    break;
                case ReportType.AccountingCommissionByAE:
                    theReportOptions.CompanyId = Convert.ToInt32(dropDownACByAERptCompany.SelectedValue);
                    reportThread = new Thread(new ParameterizedThreadStart(ExecuteACRAEReport));
                    break;
                case ReportType.FlashCommissionByAE:                    
                    theReportOptions.CompanyId = Convert.ToInt32(dropDownFlashRptCompany.SelectedValue);
                    reportThread = new Thread(new ParameterizedThreadStart(ExecuteFlashCommissionReport));
                    break;
                case ReportType.CommissionAnalysis:                    
                    theReportOptions.CompanyId = Convert.ToInt32(dropDownCommAnlRptCompany.SelectedValue);
                    reportThread = new Thread(new ParameterizedThreadStart(ExecuteCommissionAnalysisReport));
                    break;
                case ReportType.CommissionByMarket:                    
                    theReportOptions.CompanyId = Convert.ToInt32(dropDownAECommByMktRptCompany.SelectedValue);
                    reportThread = new Thread(new ParameterizedThreadStart(ExecuteCommissionByMarketReport));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("report", "report did not contain a valid ReportTypes value");
            }
            reportThread.Start(theReportOptions);
            ShowWorkingMessage(message);
        }
        #endregion

        #region SetDefaultFieldValues method
        /// <summary>Initialize the search filters</summary>
        private void SetDefaultFieldValues()
        {
            textACRLineItemStartDate.Text = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
            textACRLineItemEndDate.Text = DateTime.Now.ToShortDateString();
            textACRAELineItemStartDate.Text = new DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
            textACRAELineItemEndDate.Text = DateTime.Now.ToShortDateString();
            textCommissionByMarketAsOFDate.Text = DateTime.Now.ToShortDateString();
            textFlashFromDate.Text = new DateTime(2002, 1, 1).ToShortDateString();
            textFlashThruDate.Text = DateTime.Now.ToShortDateString();
            radioFlashByInvoice.Checked = true;
            radioFlashByContract.Checked = false;
        }
        #endregion

        #region ShowWorkingMessage method
        /// <summary>Displays a message when a report is requested and a new thread is started.</summary>
        /// <param name="message">The message to display to the user.</param>
        private void ShowWorkingMessage(string message)
        {
            labelMessage.Text = message;
            reportDone.Style["display"] = "block";
        }
        #endregion

        protected void dropDownACByAERptCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            aeDataSource.FilterParameters["COMPANY_ID"].DefaultValue = "2";
            aeDataSource.DataBind();
            listACRAE.DataBind();
        }
}

}
