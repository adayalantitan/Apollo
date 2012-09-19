#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for AutoCompleteService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class AutoCompleteService : System.Web.Services.WebService
    {

        public class SelectListItem
        {
            public bool Selected { get; set; }
            public object Text { get; set; }
            public object Value { get; set; }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet=false)]
        public List<SelectListItem> ContractAutoComplete(string companyId, string term)
        {
            try
            {
                //Hashtable paramList = new Hashtable();
                //paramList.Add("COMPANYID", Convert.ToInt32(companyId));
                List<System.Data.SqlClient.SqlParameter> paramList = new List<System.Data.SqlClient.SqlParameter>();
                paramList.Add(Param.CreateParam("companyId", SqlDbType.Int, companyId));
                Context.Response.ContentType = "application/json";
                return GetSelectList(term, 10, "AutoCompleteContracts", paramList);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return new List<SelectListItem>();
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public List<SelectListItem> AdvertiserAutoComplete(string companyId, string term)
        {
            try
            {
                //Hashtable spParams = new Hashtable();
                //spParams.Add("COMPANYID", Convert.ToInt32(companyId));
                List<System.Data.SqlClient.SqlParameter> paramList = new List<System.Data.SqlClient.SqlParameter>();
                paramList.Add(Param.CreateParam("companyId", SqlDbType.Int, companyId));
                Context.Response.ContentType = "application/json";
                return GetSelectList(term, 10, "DigitalLibrary_AutocompleteAdvertisers", paramList);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return new List<SelectListItem>();
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public List<SelectListItem> AgencyAutoComplete(string companyId, string term)
        {
            try
            {
                //Hashtable paramList = new Hashtable();
                //paramList.Add("COMPANYID", Convert.ToInt32(companyId));
                List<System.Data.SqlClient.SqlParameter> paramList = new List<System.Data.SqlClient.SqlParameter>();
                paramList.Add(Param.CreateParam("companyId", SqlDbType.Int, companyId));
                Context.Response.ContentType = "application/json";
                return GetSelectList(term, 10, "DigitalLibrary_AutocompleteAgencies", paramList);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return new List<SelectListItem>();
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public List<SelectListItem> AEAutoComplete(string companyId, string term)
        {
            try
            {
                //Hashtable paramList = new Hashtable();
                //paramList.Add("COMPANYID", Convert.ToInt32(companyId));

                List<System.Data.SqlClient.SqlParameter> paramList = new List<System.Data.SqlClient.SqlParameter>();
                paramList.Add(Param.CreateParam("companyId", SqlDbType.Int, companyId));

                Context.Response.ContentType = "application/json";
                return GetSelectList(term, 10, "AutoCompleteAEs", paramList);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return new List<SelectListItem>();
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public List<SelectListItem> MediaFormAutoComplete(string companyId, string mediaTypeId, string term)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> paramList = new List<System.Data.SqlClient.SqlParameter>();
                paramList.Add(Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(companyId)));
                if (!String.IsNullOrEmpty(mediaTypeId))
                {
                    paramList.Add(Param.CreateParam("MEDIATYPEID", SqlDbType.VarChar, mediaTypeId));
                }

                Context.Response.ContentType = "application/json";
                return GetSelectList(term, 10, "AutoCompleteMediaForms", paramList);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return new List<SelectListItem>();
            }
        }

        #region Default constructor
        /// <summary>TBD</summary>
        public AutoCompleteService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }
        #endregion

        #region GetAdvertisers method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAdvertisers(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "AUTOCOMPLETE_GETADVERTISERS", spParams);
            }
            return result;
        }
        #endregion

        #region GetAdvertisersDL method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAdvertisersDL(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "DIGITALLIBRARY_GETADVERTISERSAUTOCOMPLETE", spParams);
            }
            return result;
        }
        #endregion

        #region GetAdvertisersDLNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAdvertisersDLNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "DIGITALLIBRARY_GETADVERTISERSAUTOCOMPLETE", null);
        }
        #endregion

        #region GetAdvertisersNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAdvertisersNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AUTOCOMPLETE_GETADVERTISERS", null);
        }
        #endregion

        #region GetAEs method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAEs(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "AutoComplete_GetAEs", spParams);
            }
            return result;
        }
        #endregion

        #region GetAEsNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAEsNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AutoComplete_GetAEs", null);
        }
        #endregion

        #region GetAgencies method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAgencies(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "AUTOCOMPLETE_GETAGENCIES", spParams);
            }
            return result;
        }
        #endregion

        #region GetAgenciesDL method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAgenciesDL(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "DIGITALLIBRARY_GETAGENCIESAUTOCOMPLETE", spParams);
            }
            return result;
        }
        #endregion

        #region GetAgenciesDLNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAgenciesDLNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "DIGITALLIBRARY_GETAGENCIESAUTOCOMPLETE", null);
        }
        #endregion

        #region GetAgenciesNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetAgenciesNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AUTOCOMPLETE_GETAGENCIES", null);
        }
        #endregion

        #region GetCompanies method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetCompanies(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            //Hashtable values = new Hashtable();
            values.Add(new CascadingDropDownNameValue("Titan Outdoor (US)", "1", (defaultValue == "1")));
            values.Add(new CascadingDropDownNameValue("Titan Outdoor (Canada)", "2", (defaultValue == "2")));
            //values.Add("Titan Outdoor (US)", "1");
            //values.Add("Titan Outdoor (Canada)", "2");
            return values.ToArray();
        }
        #endregion

        #region GetConCustomers method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetConCustomers(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "AUTOCOMPLETE_GETCONSOLIDATEDCUSTOMERS", spParams);
            }
            return result;
        }
        #endregion

        #region GetConCustomersNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetConCustomersNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AUTOCOMPLETE_GETCONSOLIDATEDCUSTOMERS", null);
        }
        #endregion

        #region GetContracts method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetContracts(string prefixText, int count)
        {
            return GetText(prefixText, count, "AutoComplete_GetContracts", null);
        }
        #endregion

        #region GetContractsWithCountry method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetContractsWithCountry(string prefixText, int count)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("SHOWCOUNTRY", SqlDbType.Int, 1));
            return GetText(prefixText, count, "AutoComplete_GetContracts", spParams);
        }
        #endregion

        #region GetCustomersNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCustomersNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AUTOCOMPLETE_GETCUSTOMERS", null);
        }
        #endregion

        #region GetGLProfitCenters method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetGLProfitCenters(string companyId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            string filter = string.Format("(1=1) {0}",
            (String.IsNullOrEmpty(companyId) ? "" : string.Format("AND COMPANY_ID = {0}", companyId))
            );
            DataView pcView = new DataView(App.GetCachedDataSet(App.DataSetType.ProfitCenterDataSetType).Tables[0]);
            pcView.Sort = "PROFIT_CENTER_NAME";
            DataTable pcViewSorted = pcView.ToTable();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            foreach (DataRow row in pcViewSorted.Select(filter))
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["PROFIT_CENTER_NAME"]), Convert.ToString(row["PROFIT_CENTER_ID"]), (defaultValue == Convert.ToString(row["PROFIT_CENTER_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetMarkets method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetMarkets(string companyId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            string filter = string.Format("(1=1) {0}", (String.IsNullOrEmpty(companyId) ? "" : string.Format("AND COMPANY_ID = {0}", companyId)));
            DataView marketView = new DataView(App.GetCachedDataSet(App.DataSetType.MarketDataSetType).Tables[0]);
            marketView.Sort = "MARKET_DESCRIPTION";
            DataTable marketViewSorted = marketView.ToTable();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            foreach (DataRow row in marketViewSorted.Select(filter))
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["MARKET_DESCRIPTION"]), Convert.ToString(row["MARKET_ID"]), (defaultValue == Convert.ToString(row["MARKET_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetMediaForms method
        /// <summary>TBD</summary>
        /// <param name="mediaTypeId">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetMediaForms(string mediaTypeId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            string filter = string.Format("(1=1) {0}", (String.IsNullOrEmpty(mediaTypeId) ? "" : string.Format("AND MEDIA_TYPE_ID = '{0}'", mediaTypeId)));
            DataSet mediaForms = App.GetCachedDataSet(App.DataSetType.NewMediaFormDataSetType);            
            DataView productView = new DataView(mediaForms.Tables[0]);
            productView.Sort = "MEDIA_FORM_DESCRIPTION";
            DataTable productViewSorted = productView.ToTable();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            foreach (DataRow row in productViewSorted.Select(filter))
            {
                values.Add(new CascadingDropDownNameValue(string.Format("{0} - {1}", Convert.ToString(row["MEDIA_TYPE_DESCRIPTION"]), Convert.ToString(row["MEDIA_FORM_DESCRIPTION"])), Convert.ToString(row["MEDIA_FORM_ID"]), (defaultValue == Convert.ToString(row["MEDIA_FORM_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetMediaFormsAutoComplete method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetMediaFormsAutoComplete(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  mediaTypeId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("MEDIATYPEID", SqlDbType.VarChar, contextKey.Split(':')[1]));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "AUTOCOMPLETE_GETMEDIAFORMS", spParams);
            }
            return result;
        }
        #endregion

        #region GetMediaFormsNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetMediaFormsNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AUTOCOMPLETE_GETMEDIAFORMS", null);
        }
        #endregion

        #region GetCommissionSplitApprovers method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetCommissionSplitApprovers(string companyId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet customerData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                customerData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("GETAPPROVEDBYIDS", Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(companyId))));
            }
            values.Add(new CascadingDropDownNameValue(" - Select an Approver - ", "", true));
            foreach (DataRow row in customerData.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(IO.GetDataRowValue(row, "NAME", "")), Convert.ToString(IO.GetDataRowValue(row, "ApprovedById", "")), false));
            }
            return values.ToArray();
        }
        #endregion

        #region GetMediaTypes method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetMediaTypes(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            foreach (DataRow row in App.GetCachedDataSet(App.DataSetType.MediaTypeDataSetType).Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["MEDIA_TYPE_DESCRIPTION"]), Convert.ToString(row["MEDIA_TYPE_ID"]), (defaultValue == Convert.ToString(row["MEDIA_TYPE_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetNewMediaTypes method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetNewMediaTypes(string companyId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            DataSet mediaTypes = App.GetCachedDataSet(App.DataSetType.NewMediaTypeDataSetType);
            string filter = string.Format("COMPANY_ID = '{0}'", companyId);
            DataView mtView = new DataView(mediaTypes.Tables[0]);
            mtView.Sort = "MEDIA_TYPE_DESCRIPTION";
            DataTable mtViewSorted = mtView.ToTable();
            foreach(DataRow row in mtViewSorted.Select(filter))
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["MEDIA_TYPE_DESCRIPTION"]), Convert.ToString(row["MEDIA_TYPE_ID"]), (defaultValue == Convert.ToString(row["MEDIA_TYPE_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetParentProductClasses method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetParentProductClasses(string companyId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataView productView = new DataView(App.GetCachedDataSet(App.DataSetType.ParentProductClassDataSetType).Tables[0]);
            string filter = string.Format("COMPANY_ID = '{0}'", companyId);
            productView.Sort = "PRODUCT_CLASS_DESCRIPTION";
            DataTable productViewSorted = productView.ToTable();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            foreach (DataRow row in productViewSorted.Select(filter))
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["PRODUCT_CLASS_DESCRIPTION"]), Convert.ToString(row["PRODUCT_CLASS_ID"]), (defaultValue == Convert.ToString(row["PRODUCT_CLASS_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetProductClasses method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetProductClasses(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("PARENTPRODUCTCLASSID", SqlDbType.Int, Convert.ToInt32(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "AUTOCOMPLETE_GETPRODUCTCLASSES", spParams);
            }
            return result;
        }
        #endregion

        #region GetProductClassesNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetProductClassesNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AUTOCOMPLETE_GETPRODUCTCLASSES", null);
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetAEDD(string companyId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            string filter = string.Format("(1=1) {0}",
                (String.IsNullOrEmpty(companyId) ? "" : string.Format("AND COMPANY_ID = {0}", companyId))
            );
            DataView pcView = new DataView(App.GetCachedDataSet(App.DataSetType.AEDataSetType).Tables[0]);
            pcView.Sort = "ACCOUNT_EXECUTIVE_NAME";
            DataTable pcViewSorted = pcView.ToTable();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            foreach (DataRow row in pcViewSorted.Select(filter))
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["ACCOUNT_EXECUTIVE_NAME"]), Convert.ToString(row["ACCOUNT_EXECUTIVE_ID"]), (defaultValue == Convert.ToString(row["ACCOUNT_EXECUTIVE_ID"]))));
            }
            return values.ToArray();
        }

        #region GetProfitCentersDD method
        /// <summary>TBD</summary>
        /// <param name="companyId">TBD</param>
        /// <param name="marketId">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetProfitCentersDD(string companyId, string marketId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            string filter = string.Format("(1=1) {0} {1}",
            (String.IsNullOrEmpty(companyId) ? "" : string.Format("AND COMPANY_ID = {0}", companyId)),
            (String.IsNullOrEmpty(marketId) ? "" : string.Format("AND MARKET_ID = '{0}'", marketId))
            );
            DataView pcView = new DataView(App.GetCachedDataSet(App.DataSetType.ProfitCenterDataSetType).Tables[0]);
            pcView.Sort = "PROFIT_CENTER_NAME";
            DataTable pcViewSorted = pcView.ToTable();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            foreach (DataRow row in pcViewSorted.Select(filter))
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["PROFIT_CENTER_NAME"]), Convert.ToString(row["PROFIT_CENTER_ID"]), (defaultValue == Convert.ToString(row["PROFIT_CENTER_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetRevContracts method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetRevContracts(string prefixText, int count)
        {
            return GetText(prefixText, count, "AutoComplete_GetRevContracts", null);
        }
        #endregion

        #region GetRevContractsWithCountry method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetRevContractsWithCountry(string prefixText, int count)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("SHOWCOUNTRY", SqlDbType.Int, 1));
            return GetText(prefixText, count, "AutoComplete_GetRevContracts", spParams);
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetCustomerList(string companyId, string lookupType, string searchText, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet customerData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                customerData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("REPORTS_CUSTOMERLOOKUP",
                    Param.CreateParam("companyId", SqlDbType.Int, Convert.ToInt32(companyId)),
                    Param.CreateParam("lookuptype", SqlDbType.VarChar, lookupType),
                    Param.CreateParam("searchtext", SqlDbType.VarChar, searchText)));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Customer - ", "", true));
            foreach (DataRow row in customerData.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(IO.GetDataRowValue(row, "CustomerName", "")), Convert.ToString(IO.GetDataRowValue(row, "ID", "")), false));
            }
            return values.ToArray();
        }

        #region GetSalesMarket method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetSalesMarket(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            values.Add(new CascadingDropDownNameValue(" * ALL", "", (defaultValue == "")));
            values.Add(new CascadingDropDownNameValue("Double Banner", "8888", (defaultValue == "8888")));
            values.Add(new CascadingDropDownNameValue("Illuminated Bus Shelter", "4444", (defaultValue == "4444")));
            values.Add(new CascadingDropDownNameValue("Illuminated Phone Kiosk", "9999", (defaultValue == "9999")));
            values.Add(new CascadingDropDownNameValue("Interactive Media", "7777", (defaultValue == "7777")));
            values.Add(new CascadingDropDownNameValue("Lucy Bus", "5555", (defaultValue == "5555")));            
            values.Add(new CascadingDropDownNameValue("Night Photos", "6666", (defaultValue == "6666")));
            values.Add(new CascadingDropDownNameValue("Ticker Kiosk", "2222", (defaultValue == "2222")));
            return values.ToArray();
        }
        #endregion

        #region GetEthnicities method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetEthnicities(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataView ethnicityView;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                ethnicityView = new DataView(io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_GETETHNICITIES")).Tables[0]);
            }
            ethnicityView.Sort = "ETHNICITY_DESC";
            DataTable ethnicityViewSorted = ethnicityView.ToTable();
            values.Add(new CascadingDropDownNameValue(" * ALL ", "", (defaultValue == "")));
            foreach (DataRow row in ethnicityViewSorted.Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["ETHNICITY_DESC"]), Convert.ToString(row["ETHNICITY_ID"]), (defaultValue == Convert.ToString(row["ETHNICITY_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetStationList method
        /// <summary>TBD</summary>
        /// <param name="marketId">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetStationList(string marketId, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataView stationView = new DataView(App.GetCachedDataSet(App.DataSetType.StationDataSetType).Tables[0]);
            stationView.Sort = "STATION_NAME";
            DataTable stationViewSorted = stationView.ToTable();
            if (String.IsNullOrEmpty(marketId))
            {
                values.Add(new CascadingDropDownNameValue(" * ALL ", "", (defaultValue == "")));
                foreach (DataRow row in stationViewSorted.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(Convert.ToString(row["STATION_NAME"]), Convert.ToString(row["STATION_ID"]), (defaultValue == Convert.ToString(row["STATION_ID"]))));
                }
            }
            else
            {
                string filter = string.Format("(1=1) {0}", string.Format("AND MARKET_CODE = '{0}'", marketId));
                if (((DataRow[])stationViewSorted.Select(filter)).Length == 0)
                {
                    values.Add(new CascadingDropDownNameValue(" <No Stations for this Market> ", "", (defaultValue == "")));
                }
                else
                {
                    values.Add(new CascadingDropDownNameValue(" * ALL ", "", (defaultValue == "")));
                    foreach (DataRow row in stationViewSorted.Select(filter))
                    {
                        values.Add(new CascadingDropDownNameValue(Convert.ToString(row["STATION_NAME"]), Convert.ToString(row["STATION_ID"]), (defaultValue == Convert.ToString(row["STATION_ID"]))));
                    }
                }
            }
            return values.ToArray();
        }
        #endregion

        #region GetStations method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="contextKey">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetStations(string prefixText, int count, string contextKey)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            string[] result;
            try
            {
                //ContextKey will be in the format:
                //  companyId:#
                if (!String.IsNullOrEmpty(contextKey))
                {
                    if (!String.IsNullOrEmpty(contextKey.Split(':')[1]))
                    {
                        spParams.Add(Param.CreateParam("MARKETID", SqlDbType.VarChar, Convert.ToString(contextKey.Split(':')[1])));
                    }
                }
            }
            finally
            {
                result = GetText(prefixText, count, "AUTOCOMPLETE_GETSTATIONS", spParams);
            }
            return result;
        }
        #endregion

        #region GetStationsNonFiltered method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetStationsNonFiltered(string prefixText, int count)
        {
            return GetText(prefixText, count, "AUTOCOMPLETE_GETSTATIONS", null);
        }
        #endregion

        private List<SelectListItem> GetSelectList(string term, int count, string storedProcName)
        {
            return GetSelectList(term, count, storedProcName, new List<System.Data.SqlClient.SqlParameter>());
        }

        private List<SelectListItem> GetSelectList(string term, int count, string storedProcName, List<System.Data.SqlClient.SqlParameter> parameters)
        {
            parameters.Add(Param.CreateParam("term", SqlDbType.VarChar, term));
            parameters.Add(Param.CreateParam("count", SqlDbType.Int, count));
            List<SelectListItem> selectList = new List<SelectListItem>();
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                selectList = io.RetrieveEntitiesFromCommand<SelectListItem>(IO.CreateCommandFromStoredProc(storedProcName, parameters));
            }
            if (selectList.Count == 0)
            {
                selectList.Add(new SelectListItem { Text = "No results found", Value = "" });
            }
            return selectList;
        }

        #region GetText method
        /// <summary>TBD</summary>
        /// <param name="prefixText">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="storedProcName">TBD</param>
        /// <param name="spParams">TBD</param>
        /// <returns>TBD</returns>
        private string[] GetText(string prefixText, int count, string storedProcName, List<System.Data.SqlClient.SqlParameter> spParams)
        {
            ArrayList resultSet = new ArrayList();
            if (spParams == null)
            {
                spParams = new List<System.Data.SqlClient.SqlParameter>();
            }
            spParams.Add(Param.CreateParam("prefix", SqlDbType.VarChar, prefixText));
            spParams.Add(Param.CreateParam("count", SqlDbType.Int, count));
            DataSet results;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName, spParams));
            }
            foreach (DataRow row in results.Tables[0].Rows)
            {
                resultSet.Add(row[0].ToString());
            }
            return (string[])resultSet.ToArray(typeof(string));
        }
        #endregion

    }

}
