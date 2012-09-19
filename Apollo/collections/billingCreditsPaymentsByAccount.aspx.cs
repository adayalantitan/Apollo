#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class collections_billingCreditsPaymentsByAccount : System.Web.UI.Page
    {

        #region executeReport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void executeReport_Click(object sender, EventArgs e)
        {
            string customerId = Request.Form[dropDownSearchResults.UniqueID];            
            byte[] results = null;            
            string extension = "xls";
            string fileName = string.Format("{0}_{1}{2}{3}{4}{5}{6}.{7}", string.Format("BillingsCreditsAndPaymentsByAccount_{0}_", customerId), DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
            GetWebResponse(company.Value, customerId, out results);            
            WebCommon.WriteDebugMessage(string.Format("Billing, Credit, And Payment Report requested by: {0}, for Customer: {1}", Security.GetCurrentUserId, customerId));
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            Response.BinaryWrite(results);            
            Response.Flush();
        }
        #endregion

        #region GetWebResponse method
        /// <summary>TBD</summary>
        /// <param name="customerId">TBD</param>
        /// <param name="results">TBD</param>        
        public void GetWebResponse(string companyId, string customerId, out byte[] results)
        {
            SSRS_Reporting.ReportingService2005 reportingService = new SSRS_Reporting.ReportingService2005();
            SSRS_Reporting.ReportExecutionService reportExecService = new SSRS_Reporting.ReportExecutionService();
            reportingService.Credentials = CredentialCache.DefaultCredentials;
            reportExecService.Credentials = CredentialCache.DefaultCredentials;
            results = null;
            string historyId = null;
            string _historyId = null;
            string deviceInfo = null;
            string format = "EXCEL";
            string encoding = string.Empty;
            string mimeType = string.Empty;
            string extension = string.Empty;
            string[] streamIDs = null;
            string reportName = "/TOD - Custom Reports/Billing Credits and Payments by Account";
            bool _forRendering = false;
            SSRS_Reporting.Warning[] warnings = null;
            SSRS_Reporting.ParameterValue[] _values = null;
            SSRS_Reporting.DataSourceCredentials[] _credentials = null;
            SSRS_Reporting.ReportParameter[] _parameters = null;
            try
            {
                _parameters = reportingService.GetReportParameters(reportName, _historyId, _forRendering, _values, _credentials);
                SSRS_Reporting.ExecutionInfo ei = reportExecService.LoadReport(reportName, historyId);
                SSRS_Reporting.ParameterValue[] parameters = new SSRS_Reporting.ParameterValue[_parameters.Length];
                if (_parameters.Length > 0)
                {
                    parameters[0] = new SSRS_Reporting.ParameterValue();
                    parameters[0].Label = "companyId";
                    parameters[0].Name = "companyId";
                    parameters[0].Value = companyId;
                    parameters[1] = new SSRS_Reporting.ParameterValue();
                    parameters[1].Label = "lookupType";
                    parameters[1].Name = "lookupType";
                    parameters[1].Value = lookupType.Value;                    
                    parameters[2] = new SSRS_Reporting.ParameterValue();
                    parameters[2].Label = "searchText";
                    parameters[2].Name = "searchText";
                    parameters[2].Value = searchText.Value;
                    parameters[3] = new SSRS_Reporting.ParameterValue();
                    parameters[3].Label = "customerId";
                    parameters[3].Name = "customerId";
                    parameters[3].Value = customerId;                    
                }
                SSRS_Reporting.ExecutionInfo execInfo = reportExecService.SetExecutionParameters(parameters, "en-us");
                results = reportExecService.Render(format, deviceInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
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
