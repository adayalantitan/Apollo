#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for IOService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class IOService : System.Web.Services.WebService
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public IOService()
        {
        }
        #endregion

        #region AddCustomerXref method
        /// <summary>
        ///     Adds a Customer Consolidation/Xref record
        /// </summary>
        /// <param name="id">The row Id being modified</param>
        /// <param name="newCustomerId">The New Customer Id used for Consolidation</param>
        /// <param name="newCustomerName">The New Customer Name used for Consolidation</param>
        /// <param name="customerId">The Customer Id being Consolidated</param>
        /// <param name="companyId">The Company that the Customer is associated with</param>
        /// <returns>True if successful/False if fails</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddCustomerXref(int id, string newCustomerId, string newCustomerName, string reportDisplayId, string reportDisplayName, string customerId, string companyId)
        {            
            try
            {
                string enteredBy = Security.GetCurrentUserId;
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("CUSTOMERID", SqlDbType.VarChar, customerId));
                spParams.Add(Param.CreateParam("NEWCUSTOMERID", SqlDbType.VarChar, newCustomerId));
                spParams.Add(Param.CreateParam("NEWCUSTOMERNAME", SqlDbType.VarChar, newCustomerName));
                spParams.Add(Param.CreateParam("REPORTDISPLAYID", SqlDbType.VarChar, reportDisplayId));
                spParams.Add(Param.CreateParam("REPORTDISPLAYNAME", SqlDbType.VarChar, reportDisplayName));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)));
                spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, enteredBy));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_ADDCUSTOMERXREF", spParams));
                }
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
        }
        #endregion

        #region AddEthnicity method
        /// <summary>TBD</summary>
        /// <param name="ethnicityDesc">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddEthnicity(string ethnicityDesc)
        {
            return UpsertEthnicity(string.Empty, ethnicityDesc);
        }
        #endregion

        #region AddSearchParamRow method
        /// <summary>TBD</summary>
        /// <param name="searchParams">TBD</param>
        /// <param name="userId">TBD</param>
        /// <param name="screen">TBD</param>
        /// <param name="value">TBD</param>
        /// <param name="fieldId">TBD</param>
        private static void AddSearchParamRow(ref DataTable searchParams, string userId, string screen, object value, object fieldId)
        {
            searchParams.Rows.Add(userId, screen, fieldId, value);
        }
        #endregion

        #region AddUpdateProdOverride method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <param name="overrideProdPerc">TBD</param>
        /// <param name="overrideProdAmount">TBD</param>
        /// <param name="overrideId">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddUpdateProdOverride(int id, string overrideProdPerc, string overrideProdAmount, string overrideId, string contractNumber, string companyId)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("OVERRIDEID", SqlDbType.Int, Convert.ToInt32(overrideId)));
                spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(contractNumber)));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)));
                if (!String.IsNullOrEmpty(overrideProdPerc))
                {
                    spParams.Add(Param.CreateParam("OVERRIDEPERCENTAGE", SqlDbType.Decimal, Convert.ToDouble(overrideProdPerc)));
                }
                if (!String.IsNullOrEmpty(overrideProdAmount))
                {
                    spParams.Add(Param.CreateParam("OVERRIDEAMOUNT", SqlDbType.Decimal, Convert.ToDouble(overrideProdAmount)));
                }
                spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_UPSERTPRODUCTIONOVERRIDE", spParams));
                }
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
        }
        #endregion

        #region AddUpdateTOCProdCost method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <param name="prodCostAmount">TBD</param>
        /// <param name="prodCostId">TBD</param>
        /// <param name="uidCmpgn">TBD</param>
        /// <param name="prodType">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <!--<param name="overrideProdPerc">TBD</param>-->
        /// <!--<param name="overrideProdAmount">TBD</param>-->
        /// <!--<param name="overrideId">TBD</param>-->
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddUpdateTOCProdCost(int id, string prodCostAmount, string prodCostId, string uidCmpgn, string prodType, string contractNumber, string companyId)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("PRODCOSTID", SqlDbType.Int, Convert.ToInt32(prodCostId)));
                spParams.Add(Param.CreateParam("UIDCMPGN", SqlDbType.Int, Convert.ToInt32(uidCmpgn)));
                spParams.Add(Param.CreateParam("PRODTYPE", SqlDbType.VarChar, prodType));
                spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(contractNumber)));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)));
                spParams.Add(Param.CreateParam("PRODCOSTAMOUNT", SqlDbType.Decimal, Convert.ToDouble(prodCostAmount)));
                spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("COMMISSIONS_UPSERTTOCPRODUCTIONCOST", spParams));
                }
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
        }
        #endregion

        #region BuildSearchParamsDataTable method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <param name="userId">TBD</param>
        /// <param name="screen">TBD</param>
        /// <returns>TBD</returns>
        private static DataTable BuildSearchParamsDataTable(Hashtable values, string userId, string screen)
        {
            DataTable searchParams = WebCommon.GetUserSearchParamsTable();
            foreach (DictionaryEntry key in values)
            {
                AddSearchParamRow(ref searchParams, userId, screen, key.Value, key.Key);
            }
            return searchParams;
        }
        #endregion

        #region GetAdvertiserGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetAdvertiserGrid(int page, int rows, string sidx, string sord, bool _search, string companyId)
        {
            StringBuilder xml = new StringBuilder();
            string filter = string.Empty;
            string companyIdFilter = string.Empty;
            string customerIdFilter = string.Empty;
            string customerNameFilter = string.Empty;
            if (!String.IsNullOrEmpty(companyId))
            {
                companyIdFilter = string.Format(" AND COMPANY_ID = {0}", companyId);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                //Possible search filters:
                //  CUSTOMER_ID, CUSTOMER_NAME, COMPANY_ID
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]) && !String.IsNullOrEmpty(companyIdFilter))
                {
                    companyIdFilter = string.Format(" AND COMPANY_ID = {0}", HttpContext.Current.Request["COMPANY_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_ID"]))
                {
                    customerIdFilter = string.Format(" AND CUSTOMER_ID LIKE '%{0}%'", HttpContext.Current.Request["CUSTOMER_ID"]);
                }
                /*
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_FULL_NAME"]))
                {
                customerNameFilter = string.Format(" AND (CUSTOMER_NAME LIKE '%{0}%' OR CUSTOMER_FULL_NAME LIKE '%{0}%')", HttpContext.Current.Request["CUSTOMER_FULL_NAME"]);
                }*/
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_NAME"]))
                {
                    customerNameFilter = string.Format(" AND (CUSTOMER_NAME LIKE '%{0}%')", HttpContext.Current.Request["CUSTOMER_NAME"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2}", companyIdFilter, customerIdFilter, customerNameFilter);
            //if the text box contained a search string, apply it:
            DataSet ds = GetCachedDataSet(App.DataSetType.AdvertiserDataSetType);
            //DataSet ds = App.GetCachedDataSet(App.DataSetType.AdvertiserDataSetType);
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetAEDrawPaymentGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetAEDrawPaymentGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            string filter = string.Empty;
            string aeEffectiveDateFilter = string.Empty;
            string aeNameFilter = string.Empty;
            string aeIdFilter = string.Empty;
            string companyFilter = string.Empty;
            string marketFilter = string.Empty;
            string typeFilter = string.Empty;
            string statusFilter = string.Empty;
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_EFFECTIVE_DATE"]))
                {
                    aeEffectiveDateFilter = string.Format(" AND ACCOUNT_EXECUTIVE_EFFECTIVE_DATE = #{0}#", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_EFFECTIVE_DATE"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_NAME"]))
                {
                    aeNameFilter = string.Format(" AND ACCOUNT_EXECUTIVE_NAME LIKE '%{0}%'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_ID"]))
                {
                    aeIdFilter = string.Format(" AND ACCOUNT_EXECUTIVE_ID LIKE '%{0}%'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_ID"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]))
                {
                    companyFilter = string.Format(" AND COMPANY_NAME LIKE '%{0}%'", HttpContext.Current.Request["COMPANY_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["MARKET_ID"]))
                {
                    marketFilter = string.Format(" AND MARKET_ID = {0}", HttpContext.Current.Request["MARKET_ID"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_TYPE"]))
                {
                    typeFilter = string.Format(" AND ACCOUNT_EXECUTIVE_TYPE = '{0}'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_TYPE"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACTIVE"]))
                {
                    statusFilter = string.Format(" AND ACTIVE = '{0}'", HttpContext.Current.Request["ACTIVE"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2} {3} {4} {5} {6}", aeEffectiveDateFilter, aeNameFilter, aeIdFilter, companyFilter, marketFilter, typeFilter, statusFilter);
            //if the text box contained a search string, apply it:
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MasterData_GetAEMaintenanceList"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetAEGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public XmlDocument GetAEGrid(int page, int rows, string sidx, string sord, bool _search, string companyId)
        {
            StringBuilder xml = new StringBuilder();
            string filter = string.Empty;
            string companyIdFilter = string.Empty;
            string aeIdFilter = string.Empty;
            string aeNameFilter = string.Empty;
            if (!String.IsNullOrEmpty(companyId))
            {
                companyIdFilter = string.Format(" AND COMPANY_ID = {0}", companyId);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                //Possible search filters:
                //  ACCOUNT_EXECUTIVE_ID, ACCOUNT_EXECUTIVE_NAME, COMPANY_ID
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]) && !String.IsNullOrEmpty(companyIdFilter))
                {
                    companyIdFilter = string.Format(" AND COMPANY_NAME LIKE '%{0}%'", HttpContext.Current.Request["COMPANY_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_ID"]))
                {
                    aeIdFilter = string.Format(" AND ACCOUNT_EXECUTIVE_ID LIKE '%{0}%'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_ID"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_NAME"]))
                {
                    aeNameFilter = string.Format(" AND ACCOUNT_EXECUTIVE_NAME LIKE '%{0}%'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_NAME"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2}", companyIdFilter, aeIdFilter, aeNameFilter);
            //if the text box contained a search string, apply it:
            DataSet ds = GetCachedDataSet(App.DataSetType.AEDataSetType);
            //DataSet ds = App.GetCachedDataSet(App.DataSetType.AEDataSetType);
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetAEReportingGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetAEReportingGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            XmlDocument xmlDoc = new XmlDocument();
            StringBuilder searchFilter = new StringBuilder();
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_ID"]))
                {
                    searchFilter.AppendFormat(" AND ACCOUNT_EXECUTIVE_ID LIKE '%{0}%'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_ID"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_NAME"]))
                {
                    searchFilter.AppendFormat(" AND ACCOUNT_EXECUTIVE_NAME LIKE '%{0}%'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACTIVE"]))
                {
                    searchFilter.AppendFormat(" AND ACTIVE LIKE '%{0}%'", HttpContext.Current.Request["ACTIVE"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACCOUNT_EXECUTIVE_MARKET_ID"]))
                {
                    searchFilter.AppendFormat(" AND ACCOUNT_EXECUTIVE_MARKET_ID LIKE '%{0}%'", HttpContext.Current.Request["ACCOUNT_EXECUTIVE_MARKET_ID"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]))
                {
                    searchFilter.AppendFormat(" AND COMPANY_NAME LIKE '%{0}%'", HttpContext.Current.Request["COMPANY_NAME"]);
                }
            }
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MasterData_GetAEReportingGrid"));
                }
                DataTable table = ds.Tables[0];
                //Sort the table based on the sort index and sort direction
                DataView sortedView = new DataView(table);
                if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
                {
                    if (String.Compare("HIDDEN_CHECK_BOX", sidx, true) == 0)
                    {
                        sidx = "HIDDEN";
                    }
                    sortedView.Sort = string.Format("{0} {1}", sidx, sord);
                }
                string filter = string.Format("(1=1) {0}", searchFilter.ToString());
                table = sortedView.ToTable();
                table.DefaultView.RowFilter = "";
                xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetAESalesMarketCheckGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetAESalesMarketCheckGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("App_GetAEMarketCheckGrid"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetLocalNationalCheckGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("App_GetLocalNationalCheckGrid"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            return xmlDoc;
        }

        #region GetAgencyGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public XmlDocument GetAgencyGrid(int page, int rows, string sidx, string sord, bool _search, string companyId)
        {
            StringBuilder xml = new StringBuilder();
            string filter = string.Empty;
            string companyIdFilter = string.Empty;
            string customerIdFilter = string.Empty;
            string customerNameFilter = string.Empty;
            if (!String.IsNullOrEmpty(companyId))
            {
                companyIdFilter = string.Format(" AND COMPANY_ID = {0}", companyId);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                //Possible search filters:
                //  CUSTOMER_ID, CUSTOMER_NAME, COMPANY_ID
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]) && !String.IsNullOrEmpty(companyIdFilter))
                {
                    companyIdFilter = string.Format(" AND COMPANY_ID = {0}", HttpContext.Current.Request["COMPANY_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_ID"]))
                {
                    customerIdFilter = string.Format(" AND CUSTOMER_ID LIKE '%{0}%'", HttpContext.Current.Request["CUSTOMER_ID"]);
                }
                /*
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_FULL_NAME"]))
                {
                customerNameFilter = string.Format(" AND (CUSTOMER_NAME LIKE '%{0}%' OR CUSTOMER_FULL_NAME LIKE '%{0}%')", HttpContext.Current.Request["CUSTOMER_FULL_NAME"]);
                }*/
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_NAME"]))
                {
                    customerNameFilter = string.Format(" AND (CUSTOMER_NAME LIKE '%{0}%')", HttpContext.Current.Request["CUSTOMER_NAME"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2}", companyIdFilter, customerIdFilter, customerNameFilter);
            //if the text box contained a search string, apply it:
            DataSet ds = GetCachedDataSet(App.DataSetType.AgencyDataSetType);
            //DataSet ds = App.GetCachedDataSet(App.DataSetType.AgencyDataSetType);
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetAuditGrid method
        /// <summary>TBD</summary>
        /// <param name="fromDate">TBD</param>
        /// <param name="toDate">TBD</param>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetAuditGrid(string fromDate, string toDate, int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            string filter = string.Empty;
            string tableKeyFilter = string.Empty;
            string tableNameFilter = string.Empty;
            string fromDateFilter = string.Empty;
            string toDateFilter = string.Empty;
            string dateChangedFilter = string.Empty;
            string actionFilter = string.Empty;
            DataSet searchResults;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                searchResults = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Admin_GetAuditRecords"));
            }
            if (!String.IsNullOrEmpty(fromDate))
            {
                fromDateFilter = string.Format(" AND DATE_CHANGED >= #{0}#", fromDate);
            }
            if (!String.IsNullOrEmpty(toDate))
            {
                toDateFilter = string.Format(" AND DATE_CHANGED <= #{0}#", toDate);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["SEARCH_ON"]))
                {
                    tableKeyFilter = string.Format(" AND TABLE_KEY LIKE'{0}%'", HttpContext.Current.Request["SEARCH_ON"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["TABLE_NAME_DISPLAY"]))
                {
                    tableNameFilter = string.Format(" AND TABLE_NAME = '{0}'", HttpContext.Current.Request["TABLE_NAME_DISPLAY"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACTION_TAKEN"]))
                {
                    actionFilter = string.Format(" AND ACTION_TAKEN = '{0}'", HttpContext.Current.Request["ACTION_TAKEN"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2} {3} {4}", tableKeyFilter, tableNameFilter, fromDateFilter, toDateFilter, actionFilter);
            //if the text box contained a search string, apply it:
            DataTable table = searchResults.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                //A sort index was specified, apply it
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(sortedView.ToTable(), filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetCachedDataSet method
        /// <summary>TBD</summary>
        /// <param name="dataSetType">TBD</param>
        /// <returns>TBD</returns>
        public static DataSet GetCachedDataSet(App.DataSetType dataSetType)
        {
            DataSet ds = null;
            try
            {
                ds = App.GetCachedDataSet(dataSetType);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                {
                    ds = App.GetDataSetFromKey(dataSetType);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                ds = App.GetDataSetFromKey(dataSetType);
            }
            return ds;
        }
        #endregion

        #region GetCommSplitsGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetCommSplitsGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
            spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ContractNumber"]))
                {
                    spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(HttpContext.Current.Request["ContractNumber"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Agency"]))
                {
                    spParams.Add(Param.CreateParam("AGENCY", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["Agency"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Advertiser"]))
                {
                    spParams.Add(Param.CreateParam("ADVERTISER", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["Advertiser"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["PROGRAM"]))
                {
                    spParams.Add(Param.CreateParam("PROGRAM", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["PROGRAM"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["AE1Name"]))
                {
                    spParams.Add(Param.CreateParam("AENAME", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["AE1Name"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Company"]))
                {
                    spParams.Add(Param.CreateParam("COMPANY", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["Company"])));
                }
            }
            //if the text box contained a search string, apply it:
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Commissions_GetCommSplitsGrid", spParams));
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetConsolidatedAdvertiserGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetConsolidatedAdvertiserGrid(int page, int rows, string sidx, string sord, bool _search, string companyId)
        {
            StringBuilder xml = new StringBuilder();
            string filter = string.Empty;
            string companyIdFilter = string.Empty;
            string customerIdFilter = string.Empty;
            string customerNameFilter = string.Empty;
            if (!String.IsNullOrEmpty(companyId))
            {
                companyIdFilter = string.Format(" AND COMPANY_ID = {0}", companyId);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                //Possible search filters:
                //  CUSTOMER_ID, CUSTOMER_NAME, COMPANY_ID
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]) && !String.IsNullOrEmpty(companyIdFilter))
                {
                    companyIdFilter = string.Format(" AND COMPANY_ID = {0}", HttpContext.Current.Request["COMPANY_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_ID"]))
                {
                    customerIdFilter = string.Format(" AND CUSTOMER_ID LIKE '%{0}%'", HttpContext.Current.Request["CUSTOMER_ID"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_NAME"]))
                {
                    customerNameFilter = string.Format(" AND (CUSTOMER_NAME LIKE '%{0}%')", HttpContext.Current.Request["CUSTOMER_NAME"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2}", companyIdFilter, customerIdFilter, customerNameFilter);
            //if the text box contained a search string, apply it:
            DataSet ds = GetCachedDataSet(App.DataSetType.ConsolidatedAdvertiserType);
            //DataSet ds = App.GetCachedDataSet(App.DataSetType.ConsolidatedAdvertiserType);
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetConsolidatedAgencyGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public XmlDocument GetConsolidatedAgencyGrid(int page, int rows, string sidx, string sord, bool _search, string companyId)
        {
            StringBuilder xml = new StringBuilder();
            string filter = string.Empty;
            string companyIdFilter = string.Empty;
            string customerIdFilter = string.Empty;
            string customerNameFilter = string.Empty;
            if (!String.IsNullOrEmpty(companyId))
            {
                companyIdFilter = string.Format(" AND COMPANY_ID = {0}", companyId);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                //Possible search filters:
                //  CUSTOMER_ID, CUSTOMER_NAME, COMPANY_ID
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]) && !String.IsNullOrEmpty(companyIdFilter))
                {
                    companyIdFilter = string.Format(" AND COMPANY_ID = {0}", HttpContext.Current.Request["COMPANY_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_ID"]))
                {
                    customerIdFilter = string.Format(" AND CUSTOMER_ID LIKE '%{0}%'", HttpContext.Current.Request["CUSTOMER_ID"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_NAME"]))
                {
                    customerNameFilter = string.Format(" AND (CUSTOMER_NAME LIKE '%{0}%')", HttpContext.Current.Request["CUSTOMER_NAME"]);
                }
            }
            //Always apply the Customer_Code filter (to distinguish between Agencies and Advertisers)
            //filter = string.Format("(1=1) AND (CUSTOMER_CODE = 'AG' OR CUSTOMER_CODE = 'LA') {0} {1} {2}", companyIdFilter, customerIdFilter, customerNameFilter);
            filter = string.Format("(1=1) {0} {1} {2}", companyIdFilter, customerIdFilter, customerNameFilter);
            //if the text box contained a search string, apply it:
            DataSet ds = GetCachedDataSet(App.DataSetType.ConsolidatedAdvertiserType);
            //DataSet ds = App.GetCachedDataSet(App.DataSetType.ConsolidatedAdvertiserType);
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetContractDetail method
        /// <summary>TBD</summary>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public ContractDetailData GetContractDetail(string contractNumber, string companyId)
        {
            DataSet contractDetailData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                contractDetailData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("ONLINEFLASH_GETCONTRACTDETAIL",
                    Param.CreateParam("contractNumber", SqlDbType.Int, Convert.ToInt32(contractNumber)),
                    Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(companyId))));
            }
            return GenerateContractDetail(contractDetailData);
        }
        #endregion

        #region GetContractsGrid method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="marketId">TBD</param>
        /// <param name="salesMarketId">TBD</param>
        /// <param name="profitCenterId">TBD</param>
        /// <param name="mediaTypeId">TBD</param>
        /// <param name="mediaFormId">TBD</param>
        /// <param name="panelSubId">TBD</param>
        /// <param name="program">TBD</param>
        /// <param name="advertiserId">TBD</param>
        /// <param name="agencyId">TBD</param>
        /// <param name="aeId">TBD</param>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetContractsGrid(string companyId, string marketId, string salesMarketId, string profitCenterId, string mediaTypeId, string mediaFormId, string panelSubId, string program, string advertiserId, string agencyId, string aeId, int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();            
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                if (!String.IsNullOrEmpty(companyId))
                {
                    spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)));
                }
                if (!String.IsNullOrEmpty(marketId))
                {
                    spParams.Add(Param.CreateParam("MARKETID", SqlDbType.VarChar, marketId));
                }
                if (!String.IsNullOrEmpty(salesMarketId))
                {
                    spParams.Add(Param.CreateParam("SUBMARKETID", SqlDbType.Int, Convert.ToInt32(salesMarketId)));
                }
                if (!String.IsNullOrEmpty(profitCenterId))
                {
                    spParams.Add(Param.CreateParam("PROFITCENTERID", SqlDbType.Int, Convert.ToInt32(profitCenterId)));
                }
                if (!String.IsNullOrEmpty(mediaTypeId))
                {
                    spParams.Add(Param.CreateParam("MEDIATYPEID", SqlDbType.VarChar, mediaTypeId));
                }
                if (!String.IsNullOrEmpty(mediaFormId))
                {
                    spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, Convert.ToInt32(mediaFormId)));
                }
                if (!String.IsNullOrEmpty(program))
                {
                    spParams.Add(Param.CreateParam("PROGRAM", SqlDbType.VarChar, program));
                }
                if (!String.IsNullOrEmpty(advertiserId))
                {
                    spParams.Add(Param.CreateParam("ADVERTISERID", SqlDbType.VarChar, advertiserId));
                }
                if (!String.IsNullOrEmpty(agencyId))
                {
                    spParams.Add(Param.CreateParam("AGENCYID", SqlDbType.VarChar, agencyId));
                }
                if (!String.IsNullOrEmpty(aeId))
                {
                    spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, aeId));
                }
                if (!String.IsNullOrEmpty(sidx))
                {
                    spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
                }
                if (!String.IsNullOrEmpty(sord))
                {
                    spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
                }
                //If the user searched any of the columns
                //  build the dynamic filter
                if (_search)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CONTRACT_NUMBER_LINK"]))
                    {
                        spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(HttpContext.Current.Request["CONTRACT_NUMBER_LINK"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CONTRACT_START_DATE"]))
                    {
                        spParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, Convert.ToDateTime(HttpContext.Current.Request["CONTRACT_START_DATE"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CONTRACT_END_DATE"]))
                    {
                        spParams.Add(Param.CreateParam("ENDDATE", SqlDbType.Date, Convert.ToDateTime(HttpContext.Current.Request["CONTRACT_END_DATE"])));
                    }
                }
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_GetContracts", spParams));
                }
                xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        public class ContractDetailData
        {
            public ContractDetailData()
            {
            }
            public string contractNumber { get; set; }
            public string contractEntryDate { get; set; }
            public string contractStartDate { get; set; }
            public string agency { get; set; }
            public string contactName { get; set; }
            public string agencyFee { get; set; }
            public string advertiser { get; set; }
            public string program { get; set; }
            public string agencyPoNumber { get; set; }
            public string ae1Name { get; set; }
            public string ae2Name { get; set; }
            public string ae3Name { get; set; }
            public string productClassDescription { get; set; }
            public string localOrNational { get; set; }
            public string hasAttachments { get; set; }
            public string lineItemTable { get; set; }
            public string transactionsTable { get; set; }
        }

        private static ContractDetailData GenerateContractDetail(DataSet contractDetailData)
        {
            ContractDetailData detailData = new ContractDetailData();
            detailData.contractNumber = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTRACT_NUMBER", ""));
            detailData.contractEntryDate = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTRACT_ENTRY_DATE", ""));
            detailData.contractStartDate = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTRACT_START_DATE", ""));
            detailData.agency = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AGENCY", ""));
            detailData.contactName = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "CONTACT_NAME", ""));
            detailData.agencyFee = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AGENCY_FEE", ""));
            detailData.advertiser = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "ADVERTISER", ""));
            detailData.program = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "PROGRAM", ""));
            detailData.agencyPoNumber = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AGENCY_PO_NUMBER", ""));
            detailData.ae1Name = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AE1_NAME", ""));
            detailData.ae2Name = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AE2_NAME", ""));
            detailData.ae3Name = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "AE3_NAME", ""));
            detailData.productClassDescription = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "PRODUCT_CLASS_DESCRIPTION", ""));
            detailData.localOrNational = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "LOCAL_OR_NATIONAL", ""));
            detailData.hasAttachments = Convert.ToString(IO.GetDataRowValue(contractDetailData.Tables[0].Rows[0], "HAS_ATTACHMENTS", "0"));
            detailData.lineItemTable = OnlineFlashReport.BuildDetailLineItemTable(contractDetailData);
            detailData.transactionsTable = OnlineFlashReport.BuildDetailTransactionsTable(contractDetailData);
            return detailData;
        }

        #region GetCustomerRollupGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetCustomerRollupGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                string searchField = string.Empty;
                string searchString = string.Empty;
                if (_search)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_ID"]))
                    {
                        spParams.Add(Param.CreateParam("customer_id", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["CUSTOMER_ID"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_NAME"]))
                    {
                        spParams.Add(Param.CreateParam("customer_name", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["CUSTOMER_NAME"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CONTACT_NAME"]))
                    {
                        spParams.Add(Param.CreateParam("contact_name", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["CONTACT_NAME"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["NEW_CUSTOMER_ID"]))
                    {
                        spParams.Add(Param.CreateParam("new_customer_id", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["NEW_CUSTOMER_ID"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["NEW_CUSTOMER_NAME"]))
                    {
                        spParams.Add(Param.CreateParam("new_customer_name", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["NEW_CUSTOMER_NAME"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["REPORT_DISPLAY_ID"]))
                    {
                        spParams.Add(Param.CreateParam("reportDisplayId", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["REPORT_DISPLAY_ID"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["REPORT_DISPLAY_NAME"]))
                    {
                        spParams.Add(Param.CreateParam("reportDisplayName", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["REPORT_DISPLAY_NAME"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]))
                    {
                        spParams.Add(Param.CreateParam("company_name", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["COMPANY_NAME"])));
                    }
                }
                if (!String.IsNullOrEmpty(sidx))
                {
                    spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
                }
                if (!String.IsNullOrEmpty(sord))
                {
                    spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
                }                
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GETCUSTOMERROLLUP", spParams));
                }
                xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetDiscrepancyCheckGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetDiscrepancyCheckGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("App_GetGLDiscrepancyCheckGrid"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetEthnicityGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetEthnicityGrid(int page, int rows, string sidx, string sord, bool _search)
        {            
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                if (!String.IsNullOrEmpty(sidx))
                {
                    spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
                }
                if (!String.IsNullOrEmpty(sord))
                {
                    spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
                }
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_GETETHNICITYGRID", spParams));
                }
                xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetExceptionGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetExceptionGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("App_GetExceptions"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetGrid method
        /// <summary>TBD</summary>
        /// <param name="context">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="marketId">TBD</param>
        /// <param name="profitCenterId">TBD</param>
        /// <param name="mediaTypeId">TBD</param>
        /// <param name="mediaFormId">TBD</param>
        /// <param name="parentProductClassId">TBD</param>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetGrid(string context, string companyId, string marketId, string profitCenterId, string mediaTypeId, string mediaFormId, string parentProductClassId, int page, int rows, string sidx, string sord, bool _search)
        {
            switch (context)
            {
                case "advertiser":
                    return GetAdvertiserGrid(page, rows, sidx, sord, _search, companyId);
                case "consolidatedAdvertiser":
                    return GetConsolidatedAdvertiserGrid(page, rows, sidx, sord, _search, companyId);
                case "agency":
                    return GetAgencyGrid(page, rows, sidx, sord, _search, companyId);
                case "consolidatedAgency":
                    return GetConsolidatedAgencyGrid(page, rows, sidx, sord, _search, companyId);
                case "ae":
                    return GetAEGrid(page, rows, sidx, sord, _search, companyId);
                case "mediaForm":
                    return GetMediaFormsGrid(page, rows, sidx, sord, _search, mediaTypeId, companyId);
                case "productClass":
                    return GetProductClassGrid(page, rows, sidx, sord, _search, parentProductClassId, companyId);
                case "station":
                    return GetStationGrid(page, rows, sidx, sord, _search, marketId);
                default:
                    return null;
            }
        }
        #endregion

        #region GetMediaFormRollupGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetMediaFormRollupGrid(int page, int rows, string sidx, string sord, bool _search)
        {            
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                if (_search)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["MEDIA_TYPE_DESCRIPTION"]))
                    {
                        spParams.Add(Param.CreateParam("MEDIA_TYPE_DESCRIPTION", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["MEDIA_TYPE_DESCRIPTION"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["MEDIA_FORM_DESCRIPTION"]))
                    {
                        spParams.Add(Param.CreateParam("MEDIA_FORM_DESCRIPTION", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["MEDIA_FORM_DESCRIPTION"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["ROLLUP_NAME"]))
                    {
                        spParams.Add(Param.CreateParam("ROLLUP_NAME", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["ROLLUP_NAME"])));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["SHOULD_BE"]))
                    {
                        spParams.Add(Param.CreateParam("SHOULD_BE", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["SHOULD_BE"])));
                    }
                }
                if (!String.IsNullOrEmpty(sidx))
                {
                    spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
                }
                if (!String.IsNullOrEmpty(sord))
                {
                    spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
                }
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GETMEDIAFORMROLLUPGRID", spParams));
                }
                xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetMediaFormsGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="mediaTypeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public XmlDocument GetMediaFormsGrid(int page, int rows, string sidx, string sord, bool _search, string mediaTypeId, string companyId)
        {
            string filter = string.Empty;
            string mediaTypeIdFilter = string.Empty;
            string mediaFormIdFilter = string.Empty;
            string mediaFormNameFilter = string.Empty;
            string companyIdFilter = string.Empty;
            if (!String.IsNullOrEmpty(mediaTypeId))
            {
                mediaTypeIdFilter = string.Format(" AND MEDIA_TYPE_ID = '{0}'", mediaTypeId);
            }
            if (!String.IsNullOrEmpty(companyId))
            {
                companyIdFilter = string.Format(" AND COMPANY_ID = {0}", companyId);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["MEDIA_TYPE_DESCRIPTION"]))
                {
                    mediaFormIdFilter = string.Format(" AND MEDIA_TYPE_ID = '{0}'", HttpContext.Current.Request["MEDIA_TYPE_DESCRIPTION"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["MEDIA_FORM_DESCRIPTION"]))
                {
                    mediaFormNameFilter = string.Format(" AND (MEDIA_FORM_DESCRIPTION LIKE '%{0}%')", HttpContext.Current.Request["MEDIA_FORM_DESCRIPTION"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2} {3}", mediaTypeIdFilter, mediaFormIdFilter, mediaFormNameFilter, companyIdFilter);
            //if the text box contained a search string, apply it:
            DataSet ds = GetCachedDataSet(App.DataSetType.NewMediaFormDataSetType);
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetProdCostGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetProdCostGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
                spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
                if (_search)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["PROD_TYPE"]))
                    {
                        spParams.Add(Param.CreateParam("PRODTYPE", SqlDbType.VarChar, HttpContext.Current.Request["PROD_TYPE"]));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CONTRACT_NUMBER"]))
                    {
                        spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, HttpContext.Current.Request["CONTRACT_NUMBER"]));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CUSTOMER_NAME"]))
                    {
                        spParams.Add(Param.CreateParam("CUSTOMERNAME", SqlDbType.VarChar, HttpContext.Current.Request["CUSTOMER_NAME"]));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["PROGRAM"]))
                    {
                        spParams.Add(Param.CreateParam("PROGRAM", SqlDbType.VarChar, HttpContext.Current.Request["PROGRAM"]));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["ENTERED_BY"]))
                    {
                        spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["ENTERED_BY"])));
                    }
                }
                //if the text box contained a search string, apply it:
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Commissions_GetProdCostGrid", spParams));
                }
                xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetProdOverridesGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetProdOverridesGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
                spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
                if (_search)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["CONTRACT_NUMBER"]))
                    {
                        spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, HttpContext.Current.Request["CONTRACT_NUMBER"]));
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["ENTERED_BY"]))
                    {
                        spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["ENTERED_BY"])));
                    }
                }
                //if the text box contained a search string, apply it:
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Commissions_GetProdOverrideGrid", spParams));
                }
                xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetProductClassGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="parentProductClassId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public XmlDocument GetProductClassGrid(int page, int rows, string sidx, string sord, bool _search, string parentProductClassId, string companyId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                string filter = string.Empty;
                string parentProductClassIdFilter = string.Empty;
                string parentDescFilter = string.Empty;
                string descFilter = string.Empty;
                string companyIdFilter = string.Empty;
                if (!String.IsNullOrEmpty(parentProductClassId))
                {
                    parentProductClassIdFilter = string.Format(" AND PARENT_ID = {0}", parentProductClassId);
                }
                if (!String.IsNullOrEmpty(companyId))
                {
                    companyIdFilter = string.Format(" AND COMPANY_ID = {0}", companyId);
                }
                //If the user searched any of the columns
                //  build the dynamic filter
                if (_search)
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["PARENT_DESCRIPTION"]))
                    {
                        parentDescFilter = string.Format(" AND PARENT_DESCRIPTION LIKE '%{0}%'", HttpContext.Current.Request["PARENT_DESCRIPTION"]);
                    }
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request["PRODUCT_CLASS_DESCRIPTION"]))
                    {
                        descFilter = string.Format(" AND (PRODUCT_CLASS_DESCRIPTION LIKE '%{0}%')", HttpContext.Current.Request["PRODUCT_CLASS_DESCRIPTION"]);
                    }
                }
                filter = string.Format("(1=1) {0} {1} {2} {3}", parentProductClassIdFilter, parentDescFilter, descFilter, companyIdFilter);
                //if the text box contained a search string, apply it:
                DataSet ds = GetCachedDataSet(App.DataSetType.ProductClassDLDataSetType);
                DataTable table = ds.Tables[0];
                //Sort the table based on the sort index and sort direction
                DataView sortedView = new DataView(table);
                if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
                {
                    sortedView.Sort = string.Format("{0} {1}", sidx, sord);
                }
                table = sortedView.ToTable();
                table.DefaultView.RowFilter = "";
                xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetProfitCenterCheckGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetProfitCenterCheckGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("App_GetProfitCenterCheckGrid"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetProfitCenterRateGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetProfitCenterRateGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            string filter = string.Empty;
            string profitCenterFilter = string.Empty;
            string marketFilter = string.Empty;
            string rateNameFilter = string.Empty;
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["PROFIT_CENTER_NAME"]))
                {
                    profitCenterFilter = string.Format(" AND PROFIT_CENTER_NAME LIKE '%{0}%'", HttpContext.Current.Request["PROFIT_CENTER_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["MARKET_DESCRIPTION"]))
                {
                    marketFilter = string.Format(" AND MARKET_DESCRIPTION LIKE '%{0}%'", HttpContext.Current.Request["MARKET_DESCRIPTION"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["RATE_DETAIL_NAME"]))
                {
                    rateNameFilter = string.Format(" AND RATE_DETAIL_NAME LIKE '%{0}%'", HttpContext.Current.Request["RATE_DETAIL_NAME"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2}", profitCenterFilter, marketFilter, rateNameFilter);
            //if the text box contained a search string, apply it:
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MasterData_GetProfitCenterRates"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetRateGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetRateGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            string filter = string.Empty;
            string rateNameFilter = string.Empty;
            string effectiveDateFilter = string.Empty;
            string statusFilter = string.Empty;
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["RATE_DETAIL_NAME"]))
                {
                    rateNameFilter = string.Format(" AND RATE_DETAIL_NAME LIKE '%{0}%'", HttpContext.Current.Request["RATE_DETAIL_NAME"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["RATE_EFFECTIVE_DATE"]))
                {
                    effectiveDateFilter = string.Format(" AND RATE_EFFECTIVE_DATE = #{0}#", HttpContext.Current.Request["RATE_EFFECTIVE_DATE"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ACTIVE"]))
                {
                    statusFilter = string.Format(" AND ACTIVE LIKE '%{0}%'", HttpContext.Current.Request["ACTIVE"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2}", rateNameFilter, effectiveDateFilter, statusFilter);
            //if the text box contained a search string, apply it:
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MasterData_GetRateDetail"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetStationGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="marketId">TBD</param>
        /// <returns>TBD</returns>
        public XmlDocument GetStationGrid(int page, int rows, string sidx, string sord, bool _search, string marketId)
        {
            string filter = string.Empty;
            string marketIdFilter = string.Empty;
            string marketDescFilter = string.Empty;
            string stationGroupFilter = string.Empty;
            string stationFilter = string.Empty;
            if (!String.IsNullOrEmpty(marketId))
            {
                marketIdFilter = string.Format(" AND MARKET_ID = '{0}'", marketId);
            }
            //If the user searched any of the columns
            //  build the dynamic filter
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["MARKET_DESCRIPTION"]))
                {
                    marketDescFilter = string.Format(" AND MARKET_DESCRIPTION LIKE '%{0}%'", HttpContext.Current.Request["MARKET_DESCRIPTION"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["STATION_GROUP"]))
                {
                    stationGroupFilter = string.Format(" AND STATION_GROUP LIKE '%{0}%'", HttpContext.Current.Request["STATION_GROUP"]);
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["STATION_NAME"]))
                {
                    stationFilter = string.Format(" AND (STATION_NAME LIKE '%{0}%')", HttpContext.Current.Request["STATION_NAME"]);
                }
            }
            filter = string.Format("(1=1) {0} {1} {2} {3}", marketIdFilter, marketDescFilter, stationFilter, stationGroupFilter);
            //if the text box contained a search string, apply it:
            DataSet ds = GetCachedDataSet(App.DataSetType.StationDataSetType);
            //DataSet ds = App.GetCachedDataSet(App.DataSetType.NewMediaFormDataSetType);
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetStatusGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetStatusGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("App_GetStatusMessages"));
            }
            DataTable table = ds.Tables[0];
            //Sort the table based on the sort index and sort direction
            DataView sortedView = new DataView(table);
            if (!String.IsNullOrEmpty(sidx) && !String.IsNullOrEmpty(sord))
            {
                sortedView.Sort = string.Format("{0} {1}", sidx, sord);
            }
            table = sortedView.ToTable();
            table.DefaultView.RowFilter = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetTable method
        /// <summary>TBD</summary>
        /// <param name="filter">TBD</param>
        /// <param name="dataSetKey">TBD</param>
        /// <returns>TBD</returns>
        private DataTable GetTable(string filter, App.DataSetType dataSetKey)
        {
            DataSet ds = App.GetCachedDataSet(dataSetKey);
            DataView view = new DataView(ds.Tables[0], filter, string.Empty, DataViewRowState.CurrentRows);
            return view.ToTable();
        }
        #endregion

        #region LoadSearchParams method
        /// <summary>TBD</summary>
        /// <param name="userId">TBD</param>
        /// <param name="screen">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static Hashtable LoadSearchParams(string userId, string screen)
        {
            DataSet searchParamsData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                searchParamsData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("APP_GETUSERPREFERENCES",
                    Param.CreateParam("USERID", SqlDbType.VarChar, userId),
                    Param.CreateParam("SCREEN", SqlDbType.VarChar, screen)));
            }
            Hashtable searchParams = new Hashtable();
            if (searchParamsData != null && searchParamsData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in searchParamsData.Tables[0].Rows)
                {
                    searchParams.Add(row["CONTROL"], row["VALUE"]);
                }
            }
            return searchParams;
        }
        #endregion

        #region SaveSearchParams method
        /// <summary>TBD</summary>
        /// <param name="searchParams">TBD</param>
        /// <param name="userId">TBD</param>
        /// <param name="screen">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static void SaveSearchParams(Hashtable searchParams, string userId, string screen)
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("APP_UPSERTUSERPREFERENCES", Param.CreateParam("USERPREFERENCES", SqlDbType.Structured, BuildSearchParamsDataTable(searchParams, userId, screen))));
            }
        }
        #endregion

        #region ToggleHiddenAE method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="isHidden">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool ToggleHiddenAE(string aeId, bool isHidden, int companyId)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, aeId));
                spParams.Add(Param.CreateParam("ISHIDDEN", SqlDbType.Int, (isHidden) ? -1 : 0));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, companyId));
                spParams.Add(Param.CreateParam("UPDATEDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MasterData_ToggleAEReporting", spParams));
                }
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
        }
        #endregion

        #region UpdateMediaFormRollup method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <param name="mediaFormId">TBD</param>
        /// <param name="rollupName">TBD</param>
        /// <param name="shouldBe">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool UpdateMediaFormRollup(int id, string mediaFormId, string rollupName, string shouldBe)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, Convert.ToInt32(mediaFormId)));
                spParams.Add(Param.CreateParam("ROLLUPNAME", SqlDbType.VarChar, rollupName));
                spParams.Add(Param.CreateParam("SHOULDBE", SqlDbType.VarChar, shouldBe));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_UPDATEMEDIAFORMROLLUP", spParams));
                }
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
        }
        #endregion

        #region UpsertEthnicity method
        /// <summary>TBD</summary>
        /// <param name="ethnicityId">TBD</param>
        /// <param name="ethnicityDesc">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool UpsertEthnicity(string ethnicityId, string ethnicityDesc)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("ETHNICITYID", SqlDbType.Int, ethnicityId));
                spParams.Add(Param.CreateParam("ETHNICITYDESC", SqlDbType.VarChar, ethnicityDesc));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_UPSERTETHNICITIES", spParams));
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

    }

}
