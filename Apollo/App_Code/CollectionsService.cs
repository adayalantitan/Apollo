using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Data;
using System.Collections;
using Titan.DataIO;

namespace Apollo
{
    /// <summary>
    /// Summary description for CollectionsService
    /// </summary>
    [WebService(Namespace = "")]
    [System.Web.Script.Services.ScriptService]
    public class CollectionsService : System.Web.Services.WebService
    {

        #region GetCreditCheckInfo method
        /// <summary>TBD</summary>
        /// <param name="fromDate">TBD</param>
        /// <param name="toDate">TBD</param>
        /// <param name="customerType">TBD</param>
        /// <param name="contractValue">TBD</param>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetCreditCheckInfo(string fromDate, string toDate, string customerType, string companyId, string contractValue, string status, int page, int rows, string sidx, string sord, bool _search)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string searchField = string.Empty;
            string searchString = string.Empty;
            string searchFilter = string.Empty;
            string filter = string.Empty;
            if (_search)
            {
                searchField = HttpContext.Current.Request["searchField"];
                searchString = HttpContext.Current.Request["searchString"];
                //spParams.Add(searchField, searchString);
                if (String.IsNullOrEmpty(searchString))
                {
                    searchFilter = string.Format("AND [{0}] IS NULL", searchField);
                }
                else
                {
                    searchFilter = string.Format("AND [{0}] LIKE '%{1}%'", searchField, searchString);
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
            if (!String.IsNullOrEmpty(fromDate))
            {
                spParams.Add(Param.CreateParam("STARTDATE", SqlDbType.Date, Convert.ToDateTime(fromDate)));
            }
            if (!String.IsNullOrEmpty(toDate))
            {
                spParams.Add(Param.CreateParam("ENDDATE", SqlDbType.Date, Convert.ToDateTime(toDate)));
            }
            if (!String.IsNullOrEmpty(customerType))
            {
                spParams.Add(Param.CreateParam("CUSTOMERTYPE", SqlDbType.VarChar, customerType));
            }
            if (!String.IsNullOrEmpty(companyId))
            {
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)));
            }
            if (!String.IsNullOrEmpty(contractValue))
            {
                spParams.Add(Param.CreateParam("CONTRACTVALUE", SqlDbType.VarChar, contractValue));
            }
            if (!String.IsNullOrEmpty(status))
            {
                spParams.Add(Param.CreateParam("STATUS", SqlDbType.VarChar, status));
            }
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Collections_CreditCheckingDashboard", spParams));
                }
                filter = string.Format("(1=1) {0}", searchFilter);
                //if the text box contained a search string, apply it:
                DataTable table = ds.Tables[0];
                //Sort the table based on the sort index and sort direction
                DataView sortedView = new DataView(table);
                xmlDoc.LoadXml(WebCommon.GetGridData(sortedView.ToTable(), filter, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region AddCreditCheckNote method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="creditAppReq">TBD</param>
        /// <param name="creditChecked">TBD</param>
        /// <param name="notes">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddCreditCheckNote(int id, string contractNumber, string companyId, string creditAppReq, string creditChecked, string notes)
        {
            try
            {
                string enteredBy = Security.GetCurrentUserId;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = IO.CreateCommandFromStoredProc("COLLECTIONS_ADDCREDITCHECKNOTE"))
                    {
                        cmd.Parameters.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(contractNumber)));
                        cmd.Parameters.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)));
                        cmd.Parameters.Add(Param.CreateParam("CREDITAPPPREPAY", SqlDbType.VarChar, creditAppReq));
                        cmd.Parameters.Add(Param.CreateParam("CREDITCHECKED", SqlDbType.VarChar, creditChecked));
                        cmd.Parameters.Add(Param.CreateParam("NOTES", SqlDbType.NText, notes));
                        cmd.Parameters.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, enteredBy));
                        io.ExecuteActionQuery(cmd);
                    }
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

        #region GetMTAInvoiceNotesGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetMTAInvoiceNotesGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string searchField = string.Empty;
            string searchString = string.Empty;
            string searchFilter = string.Empty;
            if (_search)
            {
                searchField = HttpContext.Current.Request["searchField"];
                searchString = HttpContext.Current.Request["searchString"];
                //spParams.Add(searchField, searchString);
                if (String.IsNullOrEmpty(searchString))
                {
                    searchFilter = string.Format("AND [{0}] IS NULL", searchField);
                }
                else
                {
                    searchFilter = string.Format("AND [{0}] LIKE '%{1}%'", searchField, searchString);
                }
            }
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = IO.CreateCommandFromStoredProc("Collections_GetMTAInvoiceNotesGrid"))
                    {
                        if (!String.IsNullOrEmpty(sidx))
                        {
                            cmd.Parameters.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
                        }
                        if (!String.IsNullOrEmpty(sord))
                        {
                            cmd.Parameters.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
                        }
                        ds = io.ExecuteDataSetQuery(cmd);
                    }
                }
                DataTable table = ds.Tables[0];
                string filter = string.Format("(1=1) {0}", searchFilter);
                xmlDoc.LoadXml(WebCommon.GetGridData(table, filter, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region AddMTAInvoiceNote method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <param name="invoiceNumber">TBD</param>
        /// <param name="notes">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddMTAInvoiceNote(int id, string invoiceNumber, string notes)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("COLLECTIONS_ADDMTAINVOICENOTE",
                        Param.CreateParam("INVOICENUMBER", SqlDbType.Int, Convert.ToInt32(invoiceNumber)),
                        Param.CreateParam("NOTES", SqlDbType.VarChar, notes),
                        Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId)));
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
    }
}