#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class collections_creditCheckingDashboard : System.Web.UI.Page
    {

        #region excelExport_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void excelExport_Click(object sender, ImageClickEventArgs e)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string reportName = "Credit Checking {0} {1}";
            string customerType = string.Empty;
            string company = string.Empty;
            string contractValue = string.Empty;
            string statusValue = string.Empty;
            if (!String.IsNullOrEmpty(textEntryDateFrom.Text))
            {
                spParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, Convert.ToDateTime(textEntryDateFrom.Text)));
            }
            if (!String.IsNullOrEmpty(textEntryDateTo.Text))
            {
                spParams.Add(Param.CreateParam("ENDDATE", SqlDbType.Date, Convert.ToDateTime(textEntryDateTo.Text)));
            }
            if (!String.IsNullOrEmpty(dropDownCustomerType.SelectedValue))
            {
                spParams.Add(Param.CreateParam("CUSTOMERTYPE", SqlDbType.VarChar, dropDownCustomerType.SelectedValue));
                customerType = dropDownCustomerType.Items[dropDownCustomerType.SelectedIndex].Text;
            }
            if (!String.IsNullOrEmpty(dropDownCustomerType.SelectedValue))
            {
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(dropDownCompany.SelectedValue)));
                company = (dropDownCompany.SelectedValue == "1" ? "Titan US" : "Titan Canada");
            }
            if (!String.IsNullOrEmpty(dropDownContractValue.SelectedValue))
            {
                spParams.Add(Param.CreateParam("CONTRACTVALUE", SqlDbType.VarChar, dropDownContractValue.SelectedValue));
                contractValue = dropDownContractValue.Items[dropDownContractValue.SelectedIndex].Text;
            }
            if (!String.IsNullOrEmpty(dropDownStatus.SelectedValue))
            {
                spParams.Add(Param.CreateParam("STATUS", SqlDbType.VarChar, dropDownStatus.SelectedValue));
                statusValue = dropDownStatus.Items[dropDownStatus.SelectedIndex].Text;
            }
            try
            {
                if (String.IsNullOrEmpty(customerType) && String.IsNullOrEmpty(contractValue) && String.IsNullOrEmpty(statusValue) && String.IsNullOrEmpty(company))
                {
                    reportName = "Credit Checking";
                }
                else
                {
                    reportName = string.Format("Credit Checking - {0} {1} {2} {3}", customerType, contractValue, statusValue, company);
                }
                DataSet reportData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    reportData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Collections_CreditCheckingDashboard", spParams));
                }
                WebCommon.WriteDebugMessage(string.Format("Credit Check Report Exported by: {0}", Security.GetCurrentUserId));
                WebCommon.ExportHtmlToExcel("CreditCheckingReport", WebCommon.DataTableToHtmlTable(reportData.Tables[0], reportName));
            }
            catch (System.Threading.ThreadAbortException)
            {
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
            if (!IsPostBack)
            {
                DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                textEntryDateFrom.Text = firstDayOfMonth.ToShortDateString();
                textEntryDateTo.Text = DateTime.Now.ToShortDateString();
            }
        }
        #endregion

    }

}
