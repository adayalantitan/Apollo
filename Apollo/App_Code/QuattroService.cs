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
using System.Xml;
using AjaxControlToolkit;
using Titan.Email;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for QuattroService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class QuattroService : System.Web.Services.WebService
    {

        #region AddUpdateCCNNote method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <param name="uidCmpgn">TBD</param>
        /// <param name="attachmentId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="notes">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddUpdateCCNNote(int id, int uidCmpgn, int attachmentId, string companyId, string notes)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("QUATTRO_ADDUPDATECCNNOTE",
                        Param.CreateParam("UID_CMPGN", SqlDbType.Int, uidCmpgn),
                        Param.CreateParam("ATTACHMENTID", SqlDbType.Int, attachmentId),
                        Param.CreateParam("NOTETEXT", SqlDbType.VarChar, notes),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)),
                        Param.CreateParam("ADDEDBY", SqlDbType.VarChar, Security.GetFullUserNameFromId(Security.GetCurrentUserId))));
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

        #region AddUpdateUninvoicedContractNote method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <param name="uidCmpgn">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="notes">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool AddUpdateUninvoicedContractNote(int id, int uidCmpgn, string companyId, string notes)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("QUATTRO_ADDUPDATEUNINVOICEDNOTE",
                        Param.CreateParam("UID_CMPGN", SqlDbType.Int, uidCmpgn),
                        Param.CreateParam("NOTETEXT", SqlDbType.VarChar, notes),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId)),
                        Param.CreateParam("ADDEDBY", SqlDbType.VarChar, Security.GetFullUserNameFromId(Security.GetCurrentUserId))));
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

        #region ExecuteAttachmentSearch method
        /// <summary>TBD</summary>
        /// <param name="searchParams">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<Quattro.Attachment> ExecuteAttachmentSearch(Quattro.AttachmentSearchParams searchParams)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                if (searchParams.contractNumber != null && searchParams.contractNumber.Count > 0)
                {
                    spParams.Add(Param.CreateParam("CONTRACTNUMBERS", SqlDbType.Structured, GetContractNumberTable(searchParams.contractNumber)));
                }
                if (searchParams.invoiceNumber != null && searchParams.invoiceNumber.Count > 0)
                {
                    spParams.Add(Param.CreateParam("INVOICENUMBERS", SqlDbType.Structured, GetInvoiceNumberTable(searchParams.invoiceNumber)));
                }
                if (!String.IsNullOrEmpty(searchParams.advertiser))
                {
                    spParams.Add(Param.CreateParam("ADVERTISER", SqlDbType.VarChar, searchParams.advertiser));
                }
                if (searchParams.uidObjectType != -1 && searchParams.uidObjectType != 0)
                {
                    spParams.Add(Param.CreateParam("UIDOBJECTTYPE", SqlDbType.Int, searchParams.uidObjectType));
                }
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, searchParams.companyId));
                DataSet attachmentData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    attachmentData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_ExecuteAttachmentSearch", spParams));
                }
                List<Quattro.Attachment> attachments = new List<Quattro.Attachment>();
                foreach (DataRow row in attachmentData.Tables[0].Rows)
                {
                    attachments.Add(new Quattro.Attachment(Convert.ToString(row["ATTACHMENT_ID"])
                    , Convert.ToString(row["ATTACHMENT_EXT"])
                    , Convert.ToString(row["ATTACHMENT_NAME"])
                    , Convert.ToString(row["ATTACHMENT_TITLE"])
                    , Convert.ToString(row["ATTACHMENT_DESC"])
                    , Convert.ToString(row["ATTACHMENT_TYPE"])
                    , Convert.ToInt32(row["ATTACHMENT_TYPE_ID"])
                    , Convert.ToDateTime(row["ATTACHMENT_DATE"])
                    , Convert.ToString(row["CONTRACT_NUMBER"])
                    , searchParams.companyId));
                }
                return attachments;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to search for attachments.");
            }
        }
        #endregion

        #region GetAttachmentTypes method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetAttachmentTypes(int companyId)
        {
            try
            {
                List<CascadingDropDownNameValue> attachmentTypes = new List<CascadingDropDownNameValue>();
                DataSet attachmentTypeData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    attachmentTypeData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("QUATTRO_GETATTACHMENTTYPES", Param.CreateParam("COMPANYID", SqlDbType.Int, companyId)));
                }
                attachmentTypes.Add(new CascadingDropDownNameValue(" - All Attachment Types - ", "", true));
                foreach (DataRow row in attachmentTypeData.Tables[0].Rows)
                {
                    attachmentTypes.Add(new CascadingDropDownNameValue(Convert.ToString(row["str_name"]), Convert.ToString(row["uid_object_type"])));
                }
                return attachmentTypes.ToArray();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("The Attachment Type List could not be retrieved.");
            }
        }
        #endregion

        #region GetCampaignAuditGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetCampaignAuditGrid(int page, int rows, string sidx, string sord, bool _search, string contractNumber, string companyId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetCampaignAudit",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId))));
                }
                DataTable table = ds.Tables[0];
                //Sort the table based on the sort index and sort direction
                DataView sortedView = new DataView(table);
                table = sortedView.ToTable();
                table.DefaultView.RowFilter = "";
                xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetContractCCNReport method
        /// <summary>TBD</summary>
        /// <param name="fromDate">TBD</param>
        /// <param name="toDate">TBD</param>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetContractCCNReport(string fromDate, string toDate, int page, int rows, string sidx, string sord, bool _search)
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
            if (!String.IsNullOrEmpty(fromDate))
            {
                spParams.Add(Param.CreateParam("ATTACHMENTDATEFROM", SqlDbType.Date, Convert.ToDateTime(fromDate)));
            }
            if (!String.IsNullOrEmpty(toDate))
            {
                spParams.Add(Param.CreateParam("ATTACHMENTDATETO", SqlDbType.Date, Convert.ToDateTime(toDate)));
            }
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]))
                {
                    spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(HttpContext.Current.Request["COMPANY_NAME"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["CONTRACT_NUMBER"]))
                {
                    spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["CONTRACT_NUMBER"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ATTACHMENT_NAME"]))
                {
                    spParams.Add(Param.CreateParam("ATTACHMENTNAME", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["ATTACHMENT_NAME"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ATTACHMENT_TITLE"]))
                {
                    spParams.Add(Param.CreateParam("ATTACHMENTTITLE", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["ATTACHMENT_TITLE"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["ATTACHMENT_DESC"]))
                {
                    spParams.Add(Param.CreateParam("ATTACHMENTDESCRIPTION", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["ATTACHMENT_DESC"])));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["NOTE_TEXT"]))
                {
                    spParams.Add(Param.CreateParam("NOTETEXT", SqlDbType.VarChar, Convert.ToString(HttpContext.Current.Request["NOTE_TEXT"])));
                }
            }
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("QUATTRO_GETCONTRACTCCNS", spParams));
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

        #region GetContractNumberTable method
        /// <summary>TBD</summary>
        /// <param name="contractNumbers">TBD</param>
        /// <returns>TBD</returns>
        public DataTable GetContractNumberTable(List<String> contractNumbers)
        {
            DataTable contractNumberTable = new DataTable("contractNumbers");
            contractNumberTable.Columns.Add(new DataColumn("VARCHAR_PARAM"));
            foreach (string contractNumber in contractNumbers)
            {
                contractNumberTable.Rows.Add(contractNumber);
            }
            return contractNumberTable;
        }
        #endregion

        #region GetInvoiceAuditGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetInvoiceAuditGrid(int page, int rows, string sidx, string sord, bool _search, string contractNumber, string companyId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetInvoiceAudit",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId))));
                }
                DataTable table = ds.Tables[0];
                //Sort the table based on the sort index and sort direction
                DataView sortedView = new DataView(table);
                table = sortedView.ToTable();
                table.DefaultView.RowFilter = "";
                xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetInvoiceLineAuditGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetInvoiceLineAuditGrid(int page, int rows, string sidx, string sord, bool _search, string contractNumber, string companyId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetInvoiceLineAudit",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId))));
                }
                DataTable table = ds.Tables[0];
                //Sort the table based on the sort index and sort direction
                DataView sortedView = new DataView(table);
                table = sortedView.ToTable();
                table.DefaultView.RowFilter = "";
                xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetInvoiceLineGlOverrideAuditGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetInvoiceLineGlOverrideAuditGrid(int page, int rows, string sidx, string sord, bool _search, string contractNumber, string companyId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet ds;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetInvoiceLineGLOverrideAudit",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(companyId))));
                }
                DataTable table = ds.Tables[0];
                //Sort the table based on the sort index and sort direction
                DataView sortedView = new DataView(table);
                table = sortedView.ToTable();
                table.DefaultView.RowFilter = "";
                xmlDoc.LoadXml(WebCommon.GetGridData(table, string.Empty, page, rows));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
            return xmlDoc;
        }
        #endregion

        #region GetInvoiceNumberTable method
        /// <summary>TBD</summary>
        /// <param name="invoiceNumbers">TBD</param>
        /// <returns>TBD</returns>
        public DataTable GetInvoiceNumberTable(List<String> invoiceNumbers)
        {
            DataTable invoiceNumberTable = new DataTable("invoiceNumbers");
            invoiceNumberTable.Columns.Add(new DataColumn("INT_PARAM"));
            foreach (string invoiceNumber in invoiceNumbers)
            {
                invoiceNumberTable.Rows.Add(Convert.ToInt32(invoiceNumber));
            }
            return invoiceNumberTable;
        }
        #endregion

        #region GetInvoicesByContract method
        /// <summary>TBD</summary>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string GetInvoicesByContract(string contractNumber, string companyId)
        {
            try
            {
                StringBuilder invoiceNumberList = new StringBuilder();
                DataSet invoiceNumbers;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    invoiceNumbers = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("QUATTRO_GETINVOICENUMBERSBYCONTRACT",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.VarChar, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, companyId)));
                }
                if (invoiceNumbers.Tables[0].Rows.Count == 0)
                {
                    return string.Empty;
                }
                foreach (DataRow row in invoiceNumbers.Tables[0].Rows)
                {
                    if (!String.IsNullOrEmpty(invoiceNumberList.ToString()))
                    {
                        invoiceNumberList.Append(",");
                    }
                    invoiceNumberList.Append(Convert.ToString(row[0]));
                }
                return invoiceNumberList.ToString();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception(string.Format("An error occurred while trying to retrieve the Invoice Numbers for Contract: {0}", contractNumber));
            }
        }
        #endregion

        #region GetUninvoicedContractGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetUninvoicedContractGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            if (String.Compare("APPROVD_FOR_INVOICING", sidx, true) == 0)
            {
                spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, "APPROVAL_ID"));
            }
            else
            {
                spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
            }
            spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Entered By"]))
                {
                    spParams.Add(Param.CreateParam("enteredBy", SqlDbType.VarChar, HttpContext.Current.Request["Entered By"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Contract #"]))
                {
                    spParams.Add(Param.CreateParam("contractNumber", SqlDbType.VarChar, HttpContext.Current.Request["Contract #"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Campaign Type"]))
                {
                    spParams.Add(Param.CreateParam("campaignType", SqlDbType.VarChar, HttpContext.Current.Request["Campaign Type"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Customer"]))
                {
                    spParams.Add(Param.CreateParam("customer", SqlDbType.VarChar, HttpContext.Current.Request["Customer"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Number of Campaign Segments"]))
                {
                    spParams.Add(Param.CreateParam("numCmpgnSegments", SqlDbType.VarChar, HttpContext.Current.Request["Number of Campaign Segments"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Number of Barter Segments"]))
                {
                    spParams.Add(Param.CreateParam("numBarterSegments", SqlDbType.VarChar, HttpContext.Current.Request["Number of Barter Segments"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Market"]))
                {
                    spParams.Add(Param.CreateParam("market", SqlDbType.VarChar, HttpContext.Current.Request["Market"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Is Missing Segment Profit Center Split"]))
                {
                    spParams.Add(Param.CreateParam("isMissingSplit", SqlDbType.VarChar, HttpContext.Current.Request["Is Missing Segment Profit Center Split"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Is Banner"]))
                {
                    spParams.Add(Param.CreateParam("isBanner", SqlDbType.VarChar, HttpContext.Current.Request["Is Banner"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["COMPANY_NAME"]))
                {
                    spParams.Add(Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(HttpContext.Current.Request["COMPANY_NAME"])));
                }
            }
            //if the text box contained a search string, apply it:
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetUninvoicedContractBreakdown", spParams));
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        #region GetUninvoicedSegmentGrid method
        /// <summary>TBD</summary>
        /// <param name="page">TBD</param>
        /// <param name="rows">TBD</param>
        /// <param name="sidx">TBD</param>
        /// <param name="sord">TBD</param>
        /// <param name="_search">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public XmlDocument GetUninvoicedSegmentGrid(int page, int rows, string sidx, string sord, bool _search)
        {
            StringBuilder xml = new StringBuilder();
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("SORTBY", SqlDbType.VarChar, sidx));
            spParams.Add(Param.CreateParam("SORTDIR", SqlDbType.VarChar, sord));
            if (_search)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Entered By"]))
                {
                    spParams.Add(Param.CreateParam("enteredBy", SqlDbType.VarChar, HttpContext.Current.Request["Entered By"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Contract #"]))
                {
                    spParams.Add(Param.CreateParam("contractNumber", SqlDbType.VarChar, HttpContext.Current.Request["Contract #"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Contract Status"]))
                {
                    spParams.Add(Param.CreateParam("contractStatus", SqlDbType.VarChar, HttpContext.Current.Request["Contract Status"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Campaign Type"]))
                {
                    spParams.Add(Param.CreateParam("campaignType", SqlDbType.VarChar, HttpContext.Current.Request["Campaign Type"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Customer"]))
                {
                    spParams.Add(Param.CreateParam("customer", SqlDbType.VarChar, HttpContext.Current.Request["Customer"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Segment Status"]))
                {
                    spParams.Add(Param.CreateParam("segmentStatus", SqlDbType.VarChar, HttpContext.Current.Request["Segment Status"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Segment Sales Market"]))
                {
                    spParams.Add(Param.CreateParam("segmentSalesMarket", SqlDbType.VarChar, HttpContext.Current.Request["Segment Sales Market"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Has Segment Profit Center Split"]))
                {
                    spParams.Add(Param.CreateParam("isMissingSplit", SqlDbType.VarChar, HttpContext.Current.Request["Has Segment Profit Center Split"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Media Type"]))
                {
                    spParams.Add(Param.CreateParam("mediaType", SqlDbType.VarChar, HttpContext.Current.Request["Media Type"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Media Product"]))
                {
                    spParams.Add(Param.CreateParam("mediaProduct", SqlDbType.VarChar, HttpContext.Current.Request["Media Product"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Segment Space Reserved"]))
                {
                    spParams.Add(Param.CreateParam("segmentSpaceReserved", SqlDbType.VarChar, HttpContext.Current.Request["Segment Space Reserved"]));
                }
                if (!String.IsNullOrEmpty(HttpContext.Current.Request["Company Name"]))
                {
                    spParams.Add(Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(HttpContext.Current.Request["Company Name"])));
                }
            }
            //if the text box contained a search string, apply it:
            DataSet ds;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ds = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetUninvoicedSegmentInfo", spParams));
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(WebCommon.GetGridData(ds.Tables[0], string.Empty, page, rows));
            return xmlDoc;
        }
        #endregion

        #region IsCCNEMailSent method
        /// <summary>TBD</summary>
        /// <param name="uidCmpgn">TBD</param>
        /// <param name="attachmentId">TBD</param>
        /// <param name="isCompletion">TBD</param>
        /// <returns>TBD</returns>
        public bool IsCCNEMailSent(int uidCmpgn, int attachmentId, bool isCompletion)
        {
            try
            {
                string storedProcName = (isCompletion ? "QUATTRO_CCNREPORT_CHECKFORCOMPLETIONEMAIL" : "QUATTRO_CCNREPORT_CHECKFORNOTIFICATIONEMAIL");
                int result;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    result = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc(storedProcName,
                        Param.CreateParam("UIDCMPGN", SqlDbType.Int, uidCmpgn),
                        Param.CreateParam("ATTACHMENTID", SqlDbType.Int, attachmentId))));
                }
                return (result != 0);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
        }
        #endregion

        #region SendCCNCompletionEmail method
        /// <summary>TBD</summary>
        /// <param name="uidCmpgn">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="attachmentUserId">TBD</param>
        /// <param name="attachmentId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool SendCCNCompletionEmail(int uidCmpgn, int companyId, string contractNumber, string attachmentUserId, int attachmentId)
        {
            //Make sure a completion email hasn't already been sent.
            if (IsCCNEMailSent(uidCmpgn, attachmentId, true))
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("A completion email has already been sent for this contract: {0}. Please refresh your grid.", uidCmpgn)));
                return false;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("UIDCMPGN", SqlDbType.Int, uidCmpgn));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, companyId));
                spParams.Add(Param.CreateParam("ADDEDBY", SqlDbType.VarChar, Security.GetFullUserNameFromId(Security.GetCurrentUserId)));
                spParams.Add(Param.CreateParam("ISCOMPLETIONEMAILSENT", SqlDbType.Int, -1));
                spParams.Add(Param.CreateParam("ATTACHMENTID", SqlDbType.Int, attachmentId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("QUATTRO_CCNREPORT_TOGGLECOMPLETIONEMAILFLAG", spParams));
                }

                SendCCNEmail(true, contractNumber, attachmentUserId);
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to send the Completion email.");
            }
        }
        #endregion

        #region SendCCNEmail method
        /// <summary>TBD</summary>
        /// <param name="isCompletion">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="attachmentUserId">TBD</param>
        public void SendCCNEmail(bool isCompletion, string contractNumber, string attachmentUserId)
        {
            string subject = string.Format("{1}CCN - Contract #: {0}", contractNumber, (WebCommon.InTestingEnvironment() ? "DISREGARD - EMAIL ORIGINATED FROM TEST SYSTEM" : ""));
            string userName = Security.GetFullUserNameFromId(attachmentUserId);
            string userEmail = Security.GetUserEmailFromId(attachmentUserId);
            string body = string.Format(@"<span style=""font-size:12px;font-family:Arial;"">{0},<br/><br/>The CCN you have submitted for this contract has been {1} by the Contract Administration team.<br/><br/>Please make sure to notify all markets affected by this CCN.</span>", userName, (isCompletion ? "processed" : "received"));
            Message ccnEmail = new Message(WebCommon.SmtpAddress);
            //Email ccnEmail = new Email();
            if (WebCommon.InTestingEnvironment())
            {
                ccnEmail.AddRecipient("apps@titan360.com");
                ccnEmail.AddRecipient("TITAN-CONTRACT-ADMINISTRATION-TEAM@TITAN360.COM");
            }
            else
            {
                ccnEmail.AddRecipient(userEmail);
            }
            ccnEmail.From = "Apollo@titan360.com";
            ccnEmail.Subject = subject;
            ccnEmail.Body = body;
            ccnEmail.AddCC("TITAN-CONTRACT-ADMINISTRATION-TEAM@TITAN360.COM");
            ccnEmail.AddBCC("apps@titan360.com");
            ccnEmail.SendEmail();
        }
        #endregion

        #region SendCCNNotificationEmail method
        /// <summary>TBD</summary>
        /// <param name="uidCmpgn">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="attachmentUserId">TBD</param>
        /// <param name="attachmentId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool SendCCNNotificationEmail(int uidCmpgn, int companyId, string contractNumber, string attachmentUserId, int attachmentId)
        {
            //Make sure a notification email hasn't already been sent.
            if (IsCCNEMailSent(uidCmpgn, attachmentId, true))
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("A notification email has already been sent for this contract: {0}. Please refresh your grid.", uidCmpgn)));
                return false;
            }
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("UIDCMPGN", SqlDbType.Int, uidCmpgn));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, companyId));
                spParams.Add(Param.CreateParam("ADDEDBY", SqlDbType.VarChar, Security.GetFullUserNameFromId(Security.GetCurrentUserId)));
                spParams.Add(Param.CreateParam("ISNOTIFICATIONEMAILSENT", SqlDbType.Int, -1));
                spParams.Add(Param.CreateParam("ATTACHMENTID", SqlDbType.Int, attachmentId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("QUATTRO_CCNREPORT_TOGGLENOTIFICATIONEMAILFLAG", spParams));
                }
                SendCCNEmail(false, contractNumber, attachmentUserId);
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to send the Notification email.");
            }
        }
        #endregion

        #region ToggleUninvoicedApproval method
        /// <summary>TBD</summary>
        /// <param name="approvalId">TBD</param>
        /// <param name="uidCmpgn">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool ToggleUninvoicedApproval(int approvalId, int uidCmpgn)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            if (approvalId != -1)
            {
                spParams.Add(Param.CreateParam("APPROVALID", SqlDbType.Int, approvalId));
            }
            else
            {
                spParams.Add(Param.CreateParam("UID_CMPGN", SqlDbType.Int, uidCmpgn));
                spParams.Add(Param.CreateParam("APPROVEDBY", SqlDbType.VarChar, Security.GetFullUserNameFromId(Security.GetCurrentUserId)));
            }
            try
            {
                using(IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("QUATTRO_TOGGLEUNINVOICEDAPPROVAL", spParams));
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
