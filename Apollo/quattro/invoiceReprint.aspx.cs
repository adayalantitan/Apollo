#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class quattro_invoiceReprint : System.Web.UI.Page
    {

        #region buttonServerPrint_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void buttonServerPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string[] invoiceNumbers = GetInvoiceNumbers();
                int companyId = Convert.ToInt32(dropDownCompany.SelectedValue);
                byte[] results = null;
                PdfMerge5 pdfMerger = new PdfMerge5();
                string extension = "pdf";
                string fileName = string.Format("{0}_{1}{2}{3}{4}{5}{6}.{7}", "InvoiceReprint", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                StringBuilder invoiceList = new StringBuilder();
                foreach (string invoiceNumber in invoiceNumbers)
                {
                    invoiceList.Append((String.IsNullOrEmpty(invoiceList.ToString()) ? "" : ",") + invoiceNumber);
                    GetWebResponse(invoiceNumber, companyId, out results);
                    pdfMerger.AddDocument(results);
                }
                WebCommon.WriteDebugMessage(string.Format("Invoice Reprint requested by: {0}. For Invoice(s): {1}", Security.GetCurrentUserId, invoiceList.ToString()));
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                pdfMerger.Merge(Response.OutputStream);
                Response.Flush();
                //pdfMerger.Merge(fileName);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "invoiceReprintErrorAlert", @"alert(""An error occurred while trying to reprint the invoice(s). Please make sure the Invoice(s) you entered exist and the Invoice Number(s) were entered in the correct format."");", true);
            }
        }
        #endregion

        #region GetInvoiceNumbers method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string[] GetInvoiceNumbers()
        {
            string[] invoiceNumbers = null;
            string invoiceNumberEntry = textInvoiceNumbers.Text.Trim().ToUpper().Replace("NYO-", "").Replace("TOR-", "");
            if (invoiceNumberEntry.IndexOf(',') != -1)
            {
                invoiceNumbers = invoiceNumberEntry.Split(',');
            }
            else if (invoiceNumberEntry.IndexOf(';') != -1)
            {
                invoiceNumbers = invoiceNumberEntry.Split(';');
            }
            else
            {
                invoiceNumbers = new string[1];
                invoiceNumbers[0] = invoiceNumberEntry;
            }
            return invoiceNumbers;
        }
        #endregion

        #region GetWebResponse method
        /// <summary>TBD</summary>
        /// <param name="invoiceNumber">TBD</param>
        /// <param name="results">TBD</param>
        public void GetWebResponse(string invoiceNumber, int companyId, out byte[] results)
        {
            SSRS_Reporting.ReportingService2005 reportingService = new SSRS_Reporting.ReportingService2005();
            SSRS_Reporting.ReportExecutionService reportExecService = new SSRS_Reporting.ReportExecutionService();
            reportingService.Credentials = CredentialCache.DefaultCredentials;
            reportExecService.Credentials = CredentialCache.DefaultCredentials;
            results = null;
            string historyId = null;
            string _historyId = null;
            string deviceInfo = null;
            string format = "PDF";
            string encoding = string.Empty;
            string mimeType = string.Empty;
            string extension = string.Empty;
            string[] streamIDs = null;
            string reportName = (companyId == 1 ? "/SDS_Test/_wip/InvoiceReprint_WIP" : "/SDS_Test/_wip/InvoiceReprintCanada_WIP");
            bool _forRendering = false;
            SSRS_Reporting.Warning[] warnings = null;
            SSRS_Reporting.ParameterValue[] _values = null;
            SSRS_Reporting.DataSourceCredentials[] _credentials = null;
            SSRS_Reporting.ReportParameter[] _parameters = null;
            try
            {
                _parameters = reportingService.GetReportParameters(reportName, _historyId, _forRendering, _values, _credentials);
                SSRS_Reporting.ExecutionInfo ei = reportExecService.LoadReport(reportName, historyId);
                SSRS_Reporting.ParameterValue[] parameters = new SSRS_Reporting.ParameterValue[1];
                if (_parameters.Length > 0)
                {
                    parameters[0] = new SSRS_Reporting.ParameterValue();
                    parameters[0].Label = "invoiceNumber";
                    parameters[0].Name = "invoiceNumber";
                    parameters[0].Value = invoiceNumber;
                }
                reportExecService.SetExecutionParameters(parameters, "en-us");
                results = reportExecService.Render(format, deviceInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
                //Response.Clear();
                //Response.ContentType = "application/octet-stream";
                //Response.AddHeader("content-disposition", "attachment; filename=test.PDF");
                //Response.Write(results);
                //Response.Flush();
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
        }
        #endregion

    }

}
