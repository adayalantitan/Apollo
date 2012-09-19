#region Using Statements
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.IO;
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
    public class DigitalLibraryService : System.Web.Services.WebService
    {
        public class PhotoGalleryImage
        {
            public PhotoGalleryImage()
            {
            }
            public PhotoGalleryImage(string path, string fileName)
            {
                this.path = path;
                this.fileName = fileName;    
            }
            public string path { get; set; }
            public string fileName { get; set; }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<PhotoGalleryImage> GetPhotoGalleryImageList(string path)
        {
            string basePath = @"\\fsnyc03\deptdata$\PhotoGallery\g2data\albums\{0}";
            List<PhotoGalleryImage> imageList = new List<PhotoGalleryImage>();
            DirectoryInfo directoryInfo = new DirectoryInfo(string.Format(basePath, path));
            string extension = "";
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                extension = file.Extension.Substring(file.Extension.LastIndexOf('.') + 1);
                if (String.Compare(extension, "jpg", true) == 0)
                {
                    imageList.Add(new PhotoGalleryImage(path.Replace("\\", "\\\\"), file.Name));
                }
            }
            return imageList;
        }

        #region DigitalLibraryImage class
        /// <summary>TBD</summary>
        public class DigitalLibrarySearchItem
        {
            public DigitalLibrarySearchItem()
            {
            }
            [Column("TAGGED_TOTAL")]
            public int TaggedTotal { get; set; }
            [Column("UNTAGGED_TOTAL")]
            public int UntaggedTotal { get; set; }
            [Column("START_ROW")]
            public int StartRow { get; set; }
            [Column("END_ROW")]
            public int EndRow { get; set; }
            [Column("ID")]
            public int Id { get; set; }
            [Column("FILE_EXTENSION")]
            public string FileExtension { get; set; }
            [Column("FILE_TYPE")]
            public string FileType { get; set; }
            [Column("DATE_UPLOADED")]
            public DateTime DateUploaded { get; set; }
            [Column("CONTRACT_NUMBER")]
            public Int64 ContractNumber { get; set; }
            [Column("ADVERTISER")]
            public string Advertiser { get; set; }
            [Column("MARKET_DESCRIPTION")]
            public string MarketDescription { get; set; }
            [Column("MEDIA_TYPE_DESCRIPTION")]
            public string MediaTypeDescription { get; set; }
            [Column("AE_1_NAME")]
            public string AE1Name { get; set; }
            [Column("IS_DELETED")]
            public string IsDeleted { get; set; }
            [Column("IS_TAGGED")]
            public string IsTagged { get; set; }
            [Column("STATION_NAME")]
            public string StationName { get; set; }
            [Column("IMAGE_LAT")]
            public decimal ImageLat { get; set; }
            [Column("IMAGE_LONG")]
            public decimal ImageLng { get; set; }
            [Column("IS_WEB_IMAGE")]
            public bool IsWebImage { get; set; }
            public static List<DigitalLibrarySearchItem> ExecuteDigitalLibrarySearch(List<System.Data.SqlClient.SqlParameter> spParams)
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    return io.RetrieveEntitiesFromCommand<DigitalLibrarySearchItem>(IO.CreateCommandFromStoredProc("DigitalLibrary_GetDashboard_Tagged", spParams));
                }
            }
        }
        #endregion

        #region GenerateDetailsJSONString delegate
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public delegate string GenerateDetailsJSONString(DataRow row);
        #endregion

        #region GenerateDetailsString delegate
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public delegate string GenerateDetailsString(DataRow row);
        #endregion

        #region GenerateImageDetailHtml delegate
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public delegate string GenerateImageDetailHtml(DataRow row);
        #endregion

        #region Default constructor
        /// <summary>TBD</summary>
        public DigitalLibraryService()
        {
        }
        #endregion

        #region ActivateDigitalLibraryFile method
        /// <summary>TBD</summary>
        /// <param name="fileId">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void ActivateDigitalLibraryFile(int fileId)
        {
            ToggleDigitalLibraryActivation(fileId, true);
        }
        #endregion

        #region ActivateSelected method
        /// <summary>TBD</summary>
        /// <param name="selectedIds">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void ActivateSelected(string selectedIds)
        {
            string[] ids = selectedIds.Split(';');
            DataTable selected = GetDigitalLibraryIdTable();
            foreach (string id in ids)
            {
                selected.Rows.Add(id.Split(':')[0]);
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_UPDATETAGGROUP",
                    Param.CreateParam("IDGROUP", SqlDbType.Structured, selected),
                    Param.CreateParam("ISDELETED", SqlDbType.Int, 0)));
            }
        }
        #endregion

        #region AddNonRevContract method
        /// <summary>TBD</summary>
        /// <param name="spParams">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public int AddNonRevContract(Hashtable values)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("PRODUCTCLASSID", SqlDbType.Int, values["PRODUCTCLASSID"]));
            spParams.Add(Param.CreateParam("AGENCYID", SqlDbType.VarChar, values["AGENCYID"]));
            spParams.Add(Param.CreateParam("ADVERTISERID", SqlDbType.VarChar, values["ADVERTISERID"]));
            spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, values["AEID"]));
            spParams.Add(Param.CreateParam("PROGRAM", SqlDbType.VarChar, values["PROGRAM"]));
            spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, values["MEDIAFORMID"]));
            spParams.Add(Param.CreateParam("PROFITCENTERID", SqlDbType.Int, values["PROFITCENTERID"]));
            spParams.Add(Param.CreateParam("QUANTITY", SqlDbType.Int, values["QUANTITY"]));
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, values["COMPANYID"]));
            int contractNumber;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                contractNumber = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_ADDNONREVENUECONTRACT", spParams)));
            }
            return contractNumber;
        }
        #endregion

        public class NonRevenueContract
        {
            public NonRevenueContract()
            {                
            }
            public int ProductClassId { get; set; }
            public string AEId { get; set; }
            public string AgencyId { get; set; }
            public string AdvertiserId { get; set; }
            public int MediaFormId { get; set; }
            public int ProfitCenterId { get; set; }
            public string Program { get; set; }
            public int CompanyId { get; set; }
            public int Quantity { get; set; }
            public int ContractNumber { get; set; }
            public void Save()
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("PRODUCTCLASSID", SqlDbType.Int, this.ProductClassId));
                spParams.Add(Param.CreateParam("AGENCYID", SqlDbType.VarChar, this.AgencyId));
                spParams.Add(Param.CreateParam("ADVERTISERID", SqlDbType.VarChar, this.AdvertiserId));
                spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, this.AEId));
                spParams.Add(Param.CreateParam("PROGRAM", SqlDbType.VarChar, this.Program));
                spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, this.MediaFormId));
                spParams.Add(Param.CreateParam("PROFITCENTERID", SqlDbType.Int, this.ProfitCenterId));
                spParams.Add(Param.CreateParam("QUANTITY", SqlDbType.Int, this.Quantity));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, this.CompanyId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    this.ContractNumber = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_ADDNONREVENUECONTRACT", spParams)));
                }
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public NonRevenueContract AddNonRevenueContract(NonRevenueContract contract)
        {
            try
            {
                contract.Save();
                return contract;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to add the Non-revenue contract.");
            }
        }

        public class NonRevenueContractLine
        {
            public NonRevenueContractLine()
            {
            }
            public string ContractNumber { get; set; }
            public int Quantity { get; set; }
            public string IsBonus { get; set; }
            public int CompanyId { get; set; }
            public int ProfitCenterId { get; set; }
            public int MediaFormId { get; set; }
            public void Save()
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(this.ContractNumber)));
                spParams.Add(Param.CreateParam("PROFITCENTERID", SqlDbType.Int, this.ProfitCenterId));
                spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, this.MediaFormId));
                spParams.Add(Param.CreateParam("QUANTITY", SqlDbType.Int, this.Quantity));
                spParams.Add(Param.CreateParam("ISBONUS", SqlDbType.VarChar, this.IsBonus));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, this.CompanyId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_ADDNONREVENUELINEITEM", spParams));
                }
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void AddNonRevenueContractLine(NonRevenueContractLine contractLine)
        {
            try
            {
                contractLine.Save();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occured while trying to add the Non-revenue contract line.");
            }
        }

        #region AddNonRevContractLine method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void AddNonRevContractLine(Hashtable values)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(values["CONTRACTNUMBER"])));
            spParams.Add(Param.CreateParam("PROFITCENTERID", SqlDbType.Int, Convert.ToInt32(values["PROFITCENTERID"])));
            spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, Convert.ToInt32(values["MEDIAFORMID"])));
            spParams.Add(Param.CreateParam("QUANTITY", SqlDbType.Int, Convert.ToInt32(values["QUANTITY"])));
            spParams.Add(Param.CreateParam("ISBONUS", SqlDbType.VarChar, values["ISBONUS"]));
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32(values["COMPANYID"])));
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_ADDNONREVENUELINEITEM", spParams));
            }
        }
        #endregion

        #region BuildAEPhotoFilmStripItem method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public static string BuildAEPhotoFilmStripItem(DataRow row)
        {
            StringBuilder photoFilmStrip = new StringBuilder();
            string imageId = Convert.ToString(row["ID"]);
            string advertiser = HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "ADVERTISER", HttpUtility.HtmlDecode("&nbsp;")));
            string contractNumber = HttpUtility.HtmlEncode(Convert.ToString(IO.GetDataRowValue(row, "CONTRACT_NUMBER", "000")));
            photoFilmStrip.AppendFormat(@"<li class=""frame""><img src=""http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i={0}&w=220&h=220&x=JPG&f=1&bg=#000000"" alt=""{1} - {2}"" title=""{1} - {2}"" /></li>", imageId, advertiser, contractNumber);
            return photoFilmStrip.ToString();
        }
        #endregion

        #region BuildAEPhotoPanelItem method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public static string BuildAEPhotoPanelItem(DataRow row)
        {
            StringBuilder photoPanel = new StringBuilder();
            string imageId = Convert.ToString(row["ID"]);
            string advertiser = HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "ADVERTISER", HttpUtility.HtmlDecode("&nbsp;")));
            string contractNumber = HttpUtility.HtmlEncode(Convert.ToString(IO.GetDataRowValue(row, "CONTRACT_NUMBER", "000")));
            string market = HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "MARKET_DESCRIPTION", HttpUtility.HtmlDecode("&nbsp;")));
            string downloadLink = "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i={0}&q={1}&s=1&c={2}";
            photoPanel.Append(@"<div class=""panel"">");
            photoPanel.AppendFormat(@"<img src=""http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i={0}&h=400&w=600&x=JPG&f=1&bg=#000000"" alt=""{0}"" />", imageId);
            photoPanel.AppendFormat(@"<div class=""panel-overlay""><h2>{0} - {1} - {2}</h2>", advertiser, contractNumber, market);
            photoPanel.AppendFormat(@"<p>Download:<br/><a href=""{0}"">Email Quality (800x533)</a>&nbsp;&nbsp;&nbsp;", string.Format(downloadLink, imageId, "e", contractNumber));
            photoPanel.AppendFormat(@"<a href=""{0}"">Power Point Quality (960x640)</a>&nbsp;&nbsp;&nbsp;", string.Format(downloadLink, imageId, "p", contractNumber));
            photoPanel.AppendFormat(@"<a href=""{0}"">Hi-Res Quality (1200x800)</a>&nbsp;&nbsp;&nbsp;", string.Format(downloadLink, imageId, "h", contractNumber));
            photoPanel.AppendFormat(@"<a href=""{0}"">Original</a></p></div>", string.Format(downloadLink, imageId, "o", contractNumber));
            photoPanel.Append(@"</div>");
            return photoPanel.ToString();
        }
        #endregion

        #region BuildTaggerContractDetails method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <param name="contractData">TBD</param>
        /// <param name="isForLookup">TBD</param>
        private void BuildTaggerContractDetails(ref Hashtable values, DataTable contractData, bool isForLookup)
        {
            values.Add("CONTRACT_NUMBER", IO.GetDataRowValue(contractData.Rows[0], "CONTRACT_NUMBER", ""));
            values.Add("AGENCY", IO.GetDataRowValue(contractData.Rows[0], "AGENCY", ""));
            values.Add("ADVERTISER", IO.GetDataRowValue(contractData.Rows[0], "ADVERTISER", ""));
            values.Add("AE_1_NAME", IO.GetDataRowValue(contractData.Rows[0], "AE_1_NAME", ""));
            values.Add("AE_2_NAME", IO.GetDataRowValue(contractData.Rows[0], "AE_2_NAME", ""));
            values.Add("AE_3_NAME", IO.GetDataRowValue(contractData.Rows[0], "AE_3_NAME", ""));
            values.Add("COMPANY", IO.GetDataRowValue(contractData.Rows[0], "COMPANY", ""));
            values.Add("COMPANY_ID", IO.GetDataRowValue(contractData.Rows[0], "COMPANY_ID", ""));
            StringBuilder tableBuilder = new StringBuilder();
            tableBuilder.Append(@"<table class=""pg-paging"">");
            tableBuilder.Append(@"<thead><tr>");
            if (isForLookup)
            {
                tableBuilder.Append(@"<th><input type='checkbox' onclick='SelectAllLines(this)' /></th>");
            }
            tableBuilder.Append(@"<th style=""width:12%"">Market</th>");
            //if (WebCommon.IsDigitalUser())
            //{
            //    tableBuilder.Append(@"<th style=""width:15%"">Sub-Market</th>");
            //}
            tableBuilder.Append(@"<th style=""width:20%"">Line Message</th>");
            tableBuilder.Append(@"<th style=""width:13%"">Profit Center</th><th style=""width:10%"">Media Type</th><th style=""width:10%"">Media Form</th><th style=""width:10%"">Start Date</th><th style=""width:10%"">End Date</th><th style=""width:10%"">Quantity</th><th style=""width:10%"">&nbsp;</th></tr></thead><tbody>");
            foreach (DataRow row in contractData.Rows)
            {
                tableBuilder.AppendFormat(@"<tr onmouseover=""mouseincolor(this);"" onmouseout=""mouseoutcolor(this);"" onclick=""dlContractRowClick('check_{0}')"">", IO.GetDataRowValue(row, "LINE_ITEM_NUMBER", ""));
                if (isForLookup)
                {
                    tableBuilder.AppendFormat(@"<td><input class='lineItemTag' type='checkbox' id='check_{0}' onclick=""dlContractRowClick('check_{0}')"" /></td>", IO.GetDataRowValue(row, "LINE_ITEM_NUMBER", ""));
                }
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "MARKET", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "LINE_MESSAGE", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "PROFIT_CENTER", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "MEDIA_TYPE", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "MEDIA_FORM", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "START_DATE", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "END_DATE", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", IO.GetDataRowValue(row, "QUANTITY", ""));
                tableBuilder.AppendFormat("<td>{0}</td>", ((string)IO.GetDataRowValue(row, "REASON", "") == "B") ? "Bonus" : (((string)IO.GetDataRowValue(row, "REASON", "") == "A") ? "Added" : IO.GetDataRowValue(row, "REASON", "")));
                tableBuilder.AppendLine("</tr>");
            }
            tableBuilder.Append("</tbody></table>");
            values.Add("LINE_ITEMS", tableBuilder.ToString());
        }
        #endregion

        #region DeactivateDigitalLibraryFile method
        /// <summary>TBD</summary>
        /// <param name="fileId">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeactivateDigitalLibraryFile(int fileId)
        {
            ToggleDigitalLibraryActivation(fileId, false);
        }
        #endregion

        #region DeactivateSelected method
        /// <summary>TBD</summary>
        /// <param name="selectedIds">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeactivateSelected(string selectedIds)
        {
            string[] ids = selectedIds.Split(';');
            DataTable selected = GetDigitalLibraryIdTable();
            foreach (string id in ids)
            {
                selected.Rows.Add(id.Split(':')[0]);
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_UPDATEISDELETEDGROUP",
                    Param.CreateParam("IDGROUP", SqlDbType.Structured, selected),
                    Param.CreateParam("ISDELETED", SqlDbType.Int, 1)));
            }
        }
        #endregion

        #region ExecuteAEDashboardSearch method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        public static string ExecuteAEDashboardSearch(string aeId)
        {
            try
            {
                DataSet searchResults;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    searchResults = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_GetAEDashboard", Param.CreateParam("AEID", SqlDbType.VarChar, aeId)));
                }
                if (searchResults.Tables[0].Rows.Count == 0)
                {
                    return string.Empty;
                }
                if (searchResults == null)
                {
                    return string.Empty;
                }
                return GenerateAEDashboardSearchResults(searchResults);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred");
            }
        }
        #endregion

        #region ExecuteAEDashboardSearchJS method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public string ExecuteAEDashboardSearchJS(string aeId)
        {
            return ExecuteAEDashboardSearch(aeId);
        }
        #endregion

        #region ExecuteDigitalLibrarySearch method
        /// <summary>TBD</summary>
        /// <param name="isPaging">TBD</param>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string ExecuteDigitalLibrarySearch(bool isPaging, Hashtable values)
        {
            try
            {
                int newPageNumber = Convert.ToInt32(values["pageNumber"]);
                int currentPageNumber = Convert.ToInt32(values["previousPageNumber"]);
                int numPerPage = Convert.ToInt32(values["dropDownPerPageCount"]);
                DataSet searchResults = (DataSet)WebCommon.GetSessionValue("digitalLibrarySearchResults");                
                //make sure we have searchResults and we're not paging
                if (!isPaging || searchResults == null)
                {
                    Session["digitalLibrarySearchResults"] = null;
                    searchResults = InnerExecuteDigitalLibrarySearch(values);
                    WebCommon.SetSessionState("digitalLibrarySearchResults", searchResults);
                }
                else
                {
                    int totalCount = GetTotalCount(searchResults);
                    if (searchResults.Tables[0].Rows.Count > 0)
                    {
                        int recordSetStartIndex = Convert.ToInt32(searchResults.Tables[0].Rows[0]["START_ROW"]);
                        int recordSetEndIndex = Math.Min(Convert.ToInt32(searchResults.Tables[0].Rows[0]["END_ROW"]), totalCount);
                        if (RequiresNewDataSet(totalCount, numPerPage, currentPageNumber, newPageNumber, recordSetStartIndex, recordSetEndIndex))
                        {
                            Session["digitalLibrarySearchResults"] = null;
                            searchResults = InnerExecuteDigitalLibrarySearch(values);
                            WebCommon.SetSessionState("digitalLibrarySearchResults", searchResults);
                        }
                    }
                }
                //OutputResults(searchResults);
                return GenerateDigitalLibrarySearchResults(searchResults, numPerPage, newPageNumber, Convert.ToString(values["selectedImages"]));
                //return GenerateDigitalLibrarySearchResults(InnerExecuteDigitalLibrarySearch(values), numPerPage, newPageNumber, Convert.ToString(values["selectedImages"]));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred");
            }
        }
        #endregion

        #region ExecuteStationImageSearch method
        /// <summary>TBD</summary>
        /// <param name="isPaging">TBD</param>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public string ExecuteStationImageSearch(bool isPaging, Hashtable values)
        {
            try
            {
                DataSet searchResults = (DataSet)WebCommon.GetSessionValue("stationImageSearchResults");
                int newPageNumber = Convert.ToInt32(values["pageNumber"]);
                int currentPageNumber = Convert.ToInt32(values["previousPageNumber"]);
                int numPerPage = Convert.ToInt32(values["dropDownPerPageCount"]);
                //make sure we have searchResults and we're not paging
                if (!isPaging || searchResults == null)
                {
                    Session["stationImageSearchResults"] = null;
                    searchResults = InnerExecuteStationSearch(values);
                    WebCommon.SetSessionState("stationImageSearchResults", searchResults);
                }
                else
                {
                    int totalCount = GetTotalCount(searchResults);
                    int recordSetStartIndex = Convert.ToInt32(searchResults.Tables[0].Rows[0]["START_ROW"]);
                    int recordSetEndIndex = Math.Min(Convert.ToInt32(searchResults.Tables[0].Rows[0]["END_ROW"]), totalCount);
                    if (RequiresNewDataSet(totalCount, numPerPage, currentPageNumber, newPageNumber, recordSetStartIndex, recordSetEndIndex))
                    {
                        Session["stationImageSearchResults"] = null;
                        searchResults = InnerExecuteStationSearch(values);
                        WebCommon.SetSessionState("stationImageSearchResults", searchResults);
                    }
                }
                //OutputResults(searchResults);
                return GenerateStationSearchResults(searchResults, numPerPage, newPageNumber, Convert.ToString(values["selectedImages"]));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred");
            }
        }
        #endregion

        #region GenerateAEDashboardSearchResults method
        /// <summary>TBD</summary>
        /// <param name="searchResults">TBD</param>
        /// <returns>TBD</returns>
        public static string GenerateAEDashboardSearchResults(DataSet searchResults)
        {
            StringBuilder dashboard = new StringBuilder();
            StringBuilder photoPanel = new StringBuilder();
            StringBuilder photoFilmStrip = new StringBuilder();
            dashboard.Append(@"<div id=""photos"" class=""galleryview"">");
            photoFilmStrip.Append(@"<ul class=""filmstrip"">");
            foreach (DataRow row in searchResults.Tables[0].Rows)
            {
                photoPanel.Append(BuildAEPhotoPanelItem(row));
                photoFilmStrip.Append(BuildAEPhotoFilmStripItem(row));
            }
            photoFilmStrip.Append(@"</ul>");
            dashboard.Append(photoPanel.ToString());
            dashboard.Append(photoFilmStrip.ToString());
            dashboard.AppendLine(@"</div>");
            return dashboard.ToString();
        }
        #endregion

        #region GenerateDigitalLibraryDetailsJSONString method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateDigitalLibraryDetailsJSONString(DataRow row)
        {
            string jsonString;
            jsonString = string.Format("{{imageId:{0},contractNumber:{1},fileType:'{2}',fileExtension:'{3}',isTagged:'{4}',isDeleted:'{5}',imageLat:{6},imageLong:{7}}}", Convert.ToString(row["ID"]), IO.GetDataRowValue(row, "CONTRACT_NUMBER", "000"), IO.GetDataRowValue(row, "FILE_TYPE", ""), IO.GetDataRowValue(row, "FILE_EXTENSION", ""), IO.GetDataRowValue(row, "IS_TAGGED", "N"), IO.GetDataRowValue(row, "IS_DELETED", "N"), IO.GetDataRowValue(row, "IMAGE_LAT", -1), IO.GetDataRowValue(row, "IMAGE_LONG", -1));
            return jsonString;
        }
        #endregion

        #region GenerateDigitalLibraryDetailsString method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateDigitalLibraryDetailsString(DataRow row)
        {
            string details;
            details = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", IO.GetDataRowValue(row, "CONTRACT_NUMBER", "000"), IO.GetDataRowValue(row, "FILE_TYPE", ""), IO.GetDataRowValue(row, "FILE_EXTENSION", ""), IO.GetDataRowValue(row, "IS_TAGGED", "N"), IO.GetDataRowValue(row, "IS_DELETED", "N"), IO.GetDataRowValue(row, "IS_WEB_IMAGE", "0"));
            return details;
        }
        #endregion

        #region GenerateDigitalLibraryImageHtml method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        private string GenerateDigitalLibraryImageHtml(DataRow row)
        {
            StringBuilder data = new StringBuilder();
            //Advertiser
            //TODO: PULL THIS LIST FROM DB
            string advertiser = Convert.ToString(IO.GetDataRowValue(row, "ADVERTISER", ""));
            string[] internalAdvertisers = new string[4];
            internalAdvertisers[0] = "Apple";
            internalAdvertisers[1] = "Phillips De Pury & Company";
            internalAdvertisers[2] = "Hearst Creative Communications";
            internalAdvertisers[3] = "Walmart";
            bool isInternalImage = false;
            foreach (string internalAdvertiser in internalAdvertisers)
            {
                isInternalImage = (String.Compare(advertiser, internalAdvertiser, true) == 0);
                if (isInternalImage) { break; }
            }
            data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "ADVERTISER", HttpUtility.HtmlDecode("&nbsp;"))));
            //Media Form Type
            data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", (HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "MEDIA_TYPE_DESCRIPTION", HttpUtility.HtmlDecode("&nbsp;"))) + " " + HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "MEDIA_FORM_DESCRIPTION", HttpUtility.HtmlDecode("&nbsp;")))));
            //Market
            data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "MARKET_DESCRIPTION", HttpUtility.HtmlDecode("&nbsp;"))));
            //Station
            if (!String.IsNullOrEmpty((string)IO.GetDataRowValue(row, "STATION_NAME", "")))
            {
                data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "STATION_NAME", HttpUtility.HtmlDecode("&nbsp;"))));
            }
            //AE Name
            data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "AE_1_NAME", HttpUtility.HtmlDecode("&nbsp;"))));
            //Show the Date taken for Digital Users only
            //if (Security.IsAdminUser() || Security.IsDigitalUser())
            //{
                data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", HttpUtility.HtmlEncode(string.Format("{0} {1}", ((DateTime)row["DATE_UPLOADED"]).ToShortDateString(), ((DateTime)row["DATE_UPLOADED"]).ToShortTimeString())));
            //}
            //Map Icon
            try
            {
                if (Convert.ToDecimal(IO.GetDataRowValue(row, "IMAGE_LAT", -1)) != -1 && Convert.ToDecimal(IO.GetDataRowValue(row, "IMAGE_LONG", -1)) != -1)
                //if (false)
                {
                    data.AppendFormat(@"<tr style=""height:26px""><td align=""right""><a rel=""DigitalLibraryMapToolTipHandler.ashx?lat={0}&long={1}"" href=""#"" title="""" class=""mapIconLink""><img src=""/Images/dl/map_thumb.png"" alt=""See on Map"" title=""See on Map"" /></a></td></tr>", IO.GetDataRowValue(row, "IMAGE_LAT", -1), IO.GetDataRowValue(row, "IMAGE_LONG", -1));
                }
                else
                {
                    if (isInternalImage)
                    {
                        data.Append(@"<tr style=""height:26px""><td align=""center""><span style=""color:red"">FOR INTERNAL USE ONLY</span></td></tr>");
                    }
                    else
                    {
                        data.Append(@"<tr style=""height:26px""><td align=""right"">&nbsp;</td></tr>");
                    }
                }
            }
            catch
            {
            }
            return data.ToString();
        }
        #endregion

        #region GenerateDigitalLibrarySearchResults method
        /// <summary>Displays the search results</summary>
        /// <param name="searchResults">The dataset containing the stored procedure output</param>
        /// <param name="perPageCount">TBD</param>
        /// <param name="pageNumber">TBD</param>
        /// <param name="selectedImages">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateDigitalLibrarySearchResults(DataSet searchResults, int perPageCount, int pageNumber, string selectedImages)
        {
            return InnerGenerateSearchResults(searchResults, perPageCount, pageNumber, selectedImages, GenerateDigitalLibraryImageHtml, GenerateDigitalLibraryDetailsString, GenerateDigitalLibraryDetailsJSONString);
        }
        #endregion

        #region GeneratePager method
        /// <summary>Generates the Paging mechanism for the digital library search Results</summary>
        /// <param name="totalItems">The total number of records returned by the search</param>
        /// <param name="perPageCount">TBD</param>
        /// <param name="pageNumber">TBD</param>
        /// <returns>An HTML paging mechanism</returns>
        private string GeneratePager(int totalItems, int perPageCount, int pageNumber)
        {
            StringBuilder pager = new StringBuilder();
            int totalPages = (totalItems + (perPageCount - 1)) / perPageCount;
            int currentPage = pageNumber;
            if (totalPages <= 1 || totalItems <= perPageCount)
            {
                return string.Empty;
            }
            if (currentPage - 4 > 1)
            {
                //First
                pager.Append(@"<span class=""pagerLink"" onclick=""PageBump('1')""><&nbsp;&nbsp;First</span>");
                //Prev
                pager.AppendFormat(@"<span class=""sep"">|</span><span class=""pagerLink"" onclick=""PageBump('{0}')"">Prev</span><span class=""sep"">|</span>", (currentPage - 1));
            }
            for (int i = Math.Max((currentPage - 4), 1); i <= Math.Min(totalPages, (currentPage + 5)); i++)
            {
                if (i <= totalPages)
                {
                    if (i != Math.Max((currentPage - 4), 1))
                    {
                        pager.Append(@"<span class=""sep"">|</span>");
                    }
                    if (i != currentPage)
                    {
                        pager.AppendFormat(@"<span class=""pagerLink"" onclick=""PageBump('{0}')"">{0}</span>", i);
                    }
                    else
                    {
                        pager.AppendFormat(@"<b>{0}</b>", i);
                    }
                }
            }
            if ((currentPage + 5) < totalPages)
            {
                //Next
                pager.AppendFormat(@"<span class=""sep"">|</span><span class=""pagerLink"" onclick=""PageBump('{0}')"">Next</span><span class=""sep"">|</span>", (currentPage + 1));
                //Last
                pager.AppendFormat(@"<span class=""pagerLink"" onclick=""PageBump('{0}')"">Last&nbsp;&nbsp;></span>", totalPages);
            }
            //Jumper
            pager.AppendFormat(@" Jump to page:<input type=""text"" id=""jumpToInput"" style=""width:20px"" totalpages=""{0}""/> / {0}", totalPages);
            return pager.ToString();
        }
        #endregion

        #region GenerateStationDetailsJSONString method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateStationDetailsJSONString(DataRow row)
        {
            string jsonString;
            jsonString = string.Format("{{imageId:{0},stationName:'{1}',fileType:'{2}',fileExtension:'{3}',isTagged:'{4}',isDeleted:'{5}'}}", Convert.ToString(row["ID"]), IO.GetDataRowValue(row, "STATION_NAME", ""), IO.GetDataRowValue(row, "FILE_TYPE", ""), IO.GetDataRowValue(row, "FILE_EXTENSION", ""), IO.GetDataRowValue(row, "IS_TAGGED", "N"), IO.GetDataRowValue(row, "IS_DELETED", "N"));
            return jsonString;
        }
        #endregion

        #region GenerateStationDetailsString method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateStationDetailsString(DataRow row)
        {
            string details;
            details = string.Format("{0}-{1}-{2}-{3}-{4}", IO.GetDataRowValue(row, "STATION_NAME", ""), IO.GetDataRowValue(row, "FILE_TYPE", ""), IO.GetDataRowValue(row, "FILE_EXTENSION", ""), IO.GetDataRowValue(row, "IS_TAGGED", "N"), IO.GetDataRowValue(row, "IS_DELETED", "N"));
            return details;
        }
        #endregion

        #region GenerateStationImageHtml method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <returns>TBD</returns>
        private string GenerateStationImageHtml(DataRow row)
        {
            StringBuilder data = new StringBuilder();
            //Market
            data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "MARKET", HttpUtility.HtmlDecode("&nbsp;"))));
            //Station Name
            data.AppendFormat(@"<tr><td align=""center"">{0}</td></tr>", HttpUtility.HtmlEncode((string)IO.GetDataRowValue(row, "STATION_NAME", HttpUtility.HtmlDecode("&nbsp;"))));
            return data.ToString();
        }
        #endregion

        #region GenerateStationSearchResults method
        /// <summary>TBD</summary>
        /// <param name="searchResults">TBD</param>
        /// <param name="perPageCount">TBD</param>
        /// <param name="pageNumber">TBD</param>
        /// <param name="selectedImages">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateStationSearchResults(DataSet searchResults, int perPageCount, int pageNumber, string selectedImages)
        {
            return InnerGenerateSearchResults(searchResults, perPageCount, pageNumber, selectedImages, GenerateStationImageHtml, GenerateStationDetailsString, GenerateStationDetailsJSONString);
        }
        #endregion

        #region GetDigitalLibraryIdTable method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static DataTable GetDigitalLibraryIdTable()
        {
            DataTable digitalLibrary = new DataTable("digitalLibrary");
            digitalLibrary.Columns.Add(new DataColumn("ID"));
            return digitalLibrary;
        }
        #endregion

        #region GetDigitalLibraryTagTable method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static DataTable GetDigitalLibraryTagTable()
        {
            DataTable digitalLibrary = new DataTable("digitalLibrary");
            digitalLibrary.Columns.Add(new DataColumn("ID"));
            digitalLibrary.Columns.Add(new DataColumn("CONTRACT_NUMBER"));
            digitalLibrary.Columns.Add(new DataColumn("LINE_ITEM_NUMBER"));
            digitalLibrary.Columns.Add(new DataColumn("IS_HERO_QUALITY"));
            digitalLibrary.Columns.Add(new DataColumn("IS_MARKETING_QUALITY"));
            digitalLibrary.Columns.Add(new DataColumn("TAKEN_BY"));
            digitalLibrary.Columns.Add(new DataColumn("NOTES"));
            digitalLibrary.Columns.Add(new DataColumn("SALES_MARKET_OVERRIDE"));
            digitalLibrary.Columns.Add(new DataColumn("STATION_MARKET_ID"));
            digitalLibrary.Columns.Add(new DataColumn("STATION_ID"));
            digitalLibrary.Columns.Add(new DataColumn("ETHNICITY_ID"));
            digitalLibrary.Columns.Add(new DataColumn("COMPANY_ID"));
            return digitalLibrary;
        }
        #endregion

        #region GetImageProperties method
        /// <summary>Retrieve data related to a Digital Library document</summary>
        /// <param name="values">TBD</param>
        /// <returns>An HTML-formatted Table containing File and Exif properties</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string GetImageProperties(Hashtable values)
        {
            return DigitalLibraryImaging.GetImageProperties(values);
        }
        #endregion

        #region GetNewBatch method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Hashtable GetNewBatch()
        {            
            int batchId;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                batchId = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_AddBatch", Param.CreateParam("UPLOADEDBY", SqlDbType.VarChar, Security.GetCurrentUserId))));
            }
            Hashtable results = new Hashtable();
            results.Add("BATCHID", batchId);
            results.Add("USERID", Security.GetCurrentUserId);
            return results;
        }
        #endregion

        #region GetTotalCount method
        /// <summary>TBD</summary>
        /// <param name="searchResults">TBD</param>
        /// <returns>TBD</returns>
        public static int GetTotalCount(DataSet searchResults)
        {
            int totalTaggedRows = 0;
            int totalUntaggedRows = 0;
            DataRow[] rows = searchResults.Tables[0].Select("TAGGED_TOTAL <> 0");
            if (rows.Length != 0)
            {
                totalTaggedRows = Convert.ToInt32(IO.GetDataRowValue(rows[0], "TAGGED_TOTAL", "0"));
            }
            rows = searchResults.Tables[0].Select("UNTAGGED_TOTAL <> 0");
            if (rows.Length != 0)
            {
                totalUntaggedRows = Convert.ToInt32(IO.GetDataRowValue(rows[0], "UNTAGGED_TOTAL", "0"));
            }
            return (totalTaggedRows + totalUntaggedRows);
        }
        #endregion

        public string BuildSearchText(string rawText)
        {
            StringBuilder searchText = new StringBuilder();
            string[] splitText = rawText.Split(' ');
            for (int i = 0; i < splitText.Length; i++)
            {
                if (i != 0)
                {//not first or last term, put AND
                    //searchText.Append(" AND ");
                    searchText.Append(" OR ");
                }
                //searchText.AppendFormat(@"{0}", Regex.Replace(splitText[i].Trim().Replace("&", "n"), @"^\d+", new MatchEvaluator(SearchTextMatch)));
                searchText.Append(splitText[i].Trim().Replace("&", "n"));
            }
            if (!String.IsNullOrEmpty(searchText.ToString()))
            {
                WebCommon.WriteDebugMessage(string.Format("{0} - Search String Executed: {1}", Security.GetCurrentUserId, searchText.ToString()));
            }
            return searchText.ToString();
        }

        public string SearchTextMatch(Match m)
        {
            return string.Format(@"""{0}""", m.ToString());
        }

        #region InnerExecuteDigitalLibrarySearch method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        private DataSet InnerExecuteDigitalLibrarySearch(Hashtable values)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("PAGENUMBER", SqlDbType.Int, Convert.ToInt32(values["pageNumber"])));
            spParams.Add(Param.CreateParam("NUMPERPAGE", SqlDbType.Int, Convert.ToInt32(values["dropDownPerPageCount"])));
            if (!String.IsNullOrEmpty(Convert.ToString(values["textSearch"]).Trim()))
            {
                spParams.Add(Param.CreateParam("SEARCHTEXT", SqlDbType.VarChar, BuildSearchText(Convert.ToString(values["textSearch"]).Trim())));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textContractNumber"]).Trim()))
            {
                spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(values["textContractNumber"])));
            }
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32((String.IsNullOrEmpty(Convert.ToString(values["dropDownCompany"]))) ? "1" : Convert.ToString(values["dropDownCompany"]))));
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownMarket"])))
            {
                spParams.Add(Param.CreateParam("MARKETID", SqlDbType.VarChar, Convert.ToString(values["dropDownMarket"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownSubMarket"])))
            {
                spParams.Add(Param.CreateParam("SALESMARKETID", SqlDbType.Int, Convert.ToInt32(values["dropDownSubMarket"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownProfitCenter"])))
            {
                spParams.Add(Param.CreateParam("PROFITCENTERID", SqlDbType.Int, Convert.ToInt32(values["dropDownProfitCenter"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["stationSearch.StationId"])))
            {
                spParams.Add(Param.CreateParam("STATIONID", SqlDbType.Int, Convert.ToInt32(values["stationSearch.StationId"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownProductClass"])))
            {
                spParams.Add(Param.CreateParam("PARENTPRODUCTCLASSID", SqlDbType.Int, Convert.ToInt32(values["dropDownProductClass"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["productClassSearch.ProductClassId"])))
            {
                spParams.Add(Param.CreateParam("PRODUCTCLASSID", SqlDbType.Int, Convert.ToInt32(values["productClassSearch.ProductClassId"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownMediaType"])))
            {
                spParams.Add(Param.CreateParam("MEDIATYPEID", SqlDbType.VarChar, Convert.ToString(values["dropDownMediaType"])));
            }
            if (Convert.ToBoolean(values["radioConsolidated"]))
            {
                spParams.Add(Param.CreateParam("USECONSOLIDATED", SqlDbType.Int, "1"));
            }
            else
            {
                spParams.Add(Param.CreateParam("USECONSOLIDATED", SqlDbType.Int, "0"));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["mediaFormSearch.MediaFormId"])))
            {
                spParams.Add(Param.CreateParam("MEDIAFORMID", SqlDbType.Int, Convert.ToInt32(values["mediaFormSearch.MediaFormId"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["agency.AgencyId"])))
            {
                spParams.Add(Param.CreateParam("AGENCYID", SqlDbType.VarChar, Convert.ToString(values["agency.AgencyId"])));
            } 
            else if (!String.IsNullOrEmpty(Convert.ToString(values["conAgency.AgencyId"])))
            {
                spParams.Add(Param.CreateParam("AGENCYID", SqlDbType.VarChar, Convert.ToString(values["conAgency.AgencyId"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["advertiser.AdvertiserId"])))
            {
                spParams.Add(Param.CreateParam("ADVERTISERID", SqlDbType.VarChar, Convert.ToString(values["advertiser.AdvertiserId"])));
            }
            else if (!String.IsNullOrEmpty(Convert.ToString(values["conAdvertiser.AdvertiserId"])))
            {
                spParams.Add(Param.CreateParam("ADVERTISERID", SqlDbType.VarChar, Convert.ToString(values["conAdvertiser.AdvertiserId"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["aeSearch.AEId"])))
            {
                spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, Convert.ToString(values["aeSearch.AEId"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textProgram"])))
            {
                spParams.Add(Param.CreateParam("PROGRAM", SqlDbType.VarChar, Convert.ToString(values["textProgram"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textStartDateFrom"])))
            {
                spParams.Add(Param.CreateParam("CONTRACTSTARTDATEFROM", SqlDbType.Date, Convert.ToDateTime(values["textStartDateFrom"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textStartDateTo"])))
            {
                spParams.Add(Param.CreateParam("CONTRACTSTARTDATETO", SqlDbType.Date, Convert.ToDateTime(values["textStartDateTo"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textEndDateFrom"])))
            {
                spParams.Add(Param.CreateParam("CONTRACTENDDATEFROM", SqlDbType.Date, Convert.ToDateTime(values["textEndDateFrom"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textEndDateTo"])))
            {
                spParams.Add(Param.CreateParam("CONTRACTENDDATETO", SqlDbType.Date, Convert.ToDateTime(values["textEndDateTo"])));
            }
            if (Convert.ToBoolean(values["checkPhoto"]))
            {
                spParams.Add(Param.CreateParam("MARKETING", SqlDbType.VarChar, "Y"));
            }
            if (Convert.ToBoolean(values["checkHero"]))
            {
                spParams.Add(Param.CreateParam("HERO", SqlDbType.VarChar, "Y"));
            }
            if (Convert.ToBoolean(values["checkExcludeMTA"]))
            {
                spParams.Add(Param.CreateParam("EXCLUDEMTA", SqlDbType.Int, "1"));
            }
            if (Convert.ToBoolean(values["checkExcludeTwinAmerica"]))
            {
                spParams.Add(Param.CreateParam("EXCLUDETWINAMERICA", SqlDbType.Int, "1"));
            }
            if (Convert.ToBoolean(values["checkExcludeDecamp"]))
            {
                spParams.Add(Param.CreateParam("EXCLUDEDECAMP", SqlDbType.Int, "1"));
            }
            if (Convert.ToBoolean(values["checkExcludeBillboards"]))
            {
                spParams.Add(Param.CreateParam("EXCLUDEBILLBOARDS", SqlDbType.Int, "1"));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownUploadedBy"])))
            {
                spParams.Add(Param.CreateParam("UPLOADEDBY", SqlDbType.VarChar, Convert.ToString(values["dropDownUploadedBy"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textOriginalName"]).Trim()))
            {
                spParams.Add(Param.CreateParam("ORIGINALNAME", SqlDbType.VarChar, Convert.ToString(values["textOriginalName"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownDocType"])))
            {
                spParams.Add(Param.CreateParam("DOCUMENTTYPE", SqlDbType.VarChar, Convert.ToString(values["dropDownDocType"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textNotes"]).Trim()))
            {
                spParams.Add(Param.CreateParam("NOTES", SqlDbType.VarChar, Convert.ToString(values["textNotes"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textUploadedFrom"])))
            {
                spParams.Add(Param.CreateParam("UPLOADEDDATEFROM", SqlDbType.DateTime, Convert.ToDateTime(values["textUploadedFrom"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["textUploadedTo"])))
            {
                spParams.Add(Param.CreateParam("UPLOADEDDATETO", SqlDbType.DateTime, Convert.ToDateTime(values["textUploadedTo"])));
            }            
            if (Convert.ToBoolean(values["checkBestOfPhotos"]))
            {
                spParams.Add(Param.CreateParam("ISWEBIMAGE", SqlDbType.Int, 1));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownEthnicity"])))
            {
                spParams.Add(Param.CreateParam("ETHNICITYID", SqlDbType.Int, Convert.ToInt32(values["dropDownEthnicity"])));
            }
            string storedProcName = (Convert.ToBoolean(values["radioTagged"]) || Convert.ToBoolean(values["radioWebImages"])) ? "DigitalLibrary_GetDashboard_Tagged" : "DigitalLibrary_GetDashboard_Untagged";
            //var test = DigitalLibrarySearchItem.ExecuteDigitalLibrarySearch(spParams);
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc(storedProcName,spParams));
            }
        }
        #endregion

        #region InnerExecuteStationSearch method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        private DataSet InnerExecuteStationSearch(Hashtable values)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("PAGENUMBER", SqlDbType.Int, Convert.ToInt32(values["pageNumber"])));
            spParams.Add(Param.CreateParam("NUMPERPAGE", SqlDbType.Int, Convert.ToInt32(values["dropDownPerPageCount"])));
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, Convert.ToInt32((String.IsNullOrEmpty(Convert.ToString(values["dropDownCompany"]))) ? "1" : Convert.ToString(values["dropDownCompany"]))));
            if (!String.IsNullOrEmpty(Convert.ToString(values["dropDownMarket"])))
            {
                spParams.Add(Param.CreateParam("MARKETID", SqlDbType.VarChar, Convert.ToString(values["dropDownMarket"])));
            }
            if (!String.IsNullOrEmpty(Convert.ToString(values["stationSearch.StationId"])))
            {
                spParams.Add(Param.CreateParam("STATIONID", SqlDbType.Int, Convert.ToInt32(values["stationSearch.StationId"])));
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_GetStationDashboard", spParams));
            }
        }
        #endregion

        #region InnerGenerateSearchResults method
        /// <summary>TBD</summary>
        /// <param name="searchResults">TBD</param>
        /// <param name="perPageCount">TBD</param>
        /// <param name="pageNumber">TBD</param>
        /// <param name="selectedImages">TBD</param>
        /// <param name="htmlMethod">TBD</param>
        /// <param name="detailsMethod">TBD</param>
        /// <param name="jsonDetailsMethod">TBD</param>
        /// <returns>TBD</returns>
        public string InnerGenerateSearchResults(DataSet searchResults, int perPageCount, int pageNumber, string selectedImages, GenerateImageDetailHtml htmlMethod, GenerateDetailsString detailsMethod, GenerateDetailsJSONString jsonDetailsMethod)
        {
            StringBuilder results = new StringBuilder();
            string baseImagePath = (WebCommon.DetermineEnvironment() == WebCommon.WebEnvironment.Prod ? "DigitalLibraryImageHandler.ashx?i={0}&t=1&x={1}" : @"http://apollo.titan360.com/DigitalLibraryImageHandler.ashx?i={0}&t=1&x={1}");
            string imagePath = string.Empty, details = string.Empty;
            int resultCount = 0, recordCount = 1, computedIndex = 0;
            int i = 0, id = 0;
            int startRecord = Math.Max((pageNumber * perPageCount) - perPageCount, 1);
            bool isChecked = false;
            if (searchResults == null || searchResults.Tables.Count == 0 || searchResults.Tables[0].Rows.Count == 0)
            {
                resultCount = 0;
            }
            else
            {
                resultCount = GetTotalCount(searchResults);
            }
            //If this is not page 1, subtract 1 from the endRecord
            int endRecord = Math.Min((perPageCount * pageNumber), resultCount) - ((pageNumber != 1) ? 1 : 0);
            results.Append(@"<table class=""digitalLibraryTable"" cellpadding=""2"" cellspacing=""2"">");
            if (resultCount <= 0)
            {
                results.Append(@"<tr><td colspan=""4"" align=""left"">No Record found.</td></tr>");
            }
            else
            {
                results.AppendFormat(@"<tr><td colspan=""4"" align=""right"" class=""nav"">Search Results - {0} Item{1} Returned.&nbsp;&nbsp;&nbsp;{2}</td></tr>", resultCount, ((resultCount == 1) ? "" : "s"), GeneratePager(resultCount, perPageCount, pageNumber));
                foreach (DataRow row in searchResults.Tables[0].Rows)
                {
                    computedIndex = Convert.ToInt32(row["START_ROW"]) + i++;
                    if (computedIndex >= startRecord && computedIndex <= endRecord)
                    {
                        id = Convert.ToInt32(row["ID"]);
                        details = detailsMethod(row);
                        isChecked = (selectedImages.Contains(Convert.ToString(id)));
                        imagePath = string.Format(baseImagePath, id, IO.GetDataRowValue(row, "FILE_EXTENSION", ""));
                        results.AppendFormat(@"<td class=""dlImageCell"" width=""25%"" align=""center"" id=""td{0}"" onmouseover=""ToggleCellSelection(this.id,true,true)"" onmouseout=""ToggleCellSelection(this.id,false,true)"" onclick=""CellCheckSelectionHandler(this.id);"" style=""{1}"">", id, (isChecked ? "border:1px solid #ec008c;" : ""));
                        results.Append(@"<table id=""digitalLibraryImages"" class=""digitalLibraryImage"" align=""center""><tr><td align=""center"" style=""cursor:pointer;"">");
                        results.AppendFormat(@"<a class=""dlImagePopup"" href=""#"" rel=""/digital/DigitalLibraryToolTipHandler.ashx?fileId={0}""><img class=""digitalLibraryImageDisplay"" onclick=""PopupImage({0},{3});"" id=""{0}"" src=""{1}"" alt=""{0} {2}"" /></a>", id, imagePath, IO.GetDataRowValue(row, "FILE_EXTENSION", "&nbsp;"), jsonDetailsMethod(row));
                        results.AppendFormat(@"<input type=""hidden"" id=""hdnDetails{0}"" value=""{1}"" />", id, details);
                        results.AppendFormat(@"<input type=""hidden"" id=""hdnJSONDetails{0}"" value=""{1}"" />", id, jsonDetailsMethod(row));
                        results.AppendFormat(@"</td></tr><tr><td align=""center""><input type=""checkbox"" onclick=""ClickCell(this)"" id=""check{0}"" {1} /></td></tr>", id, (isChecked ? " checked" : ""));
                        //Call the Delegate
                        results.Append(htmlMethod(row));
                        results.Append(@"</table></td>");
                        if (recordCount++ % 4 == 0)
                        {
                            results.Append("</tr><tr>");
                        }
                    }
                }
                results.AppendFormat(@"</tr><tr><td colspan=""4"" align=""right"" class=""nav"">Search Results - {0} Item{1} Returned.&nbsp;&nbsp;&nbsp;{2}</td></tr>", resultCount, ((resultCount == 1) ? "" : "s"), GeneratePager(resultCount, perPageCount, pageNumber));
            }
            results.AppendLine("</tr></table>");
            return results.ToString();
        }
        #endregion

        #region LoadContractData method
        /// <summary>TBD</summary>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Hashtable LoadContractData(int contractNumber, int companyId)
        {
            try
            {
                DataSet contractData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    contractData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_GETCONTRACTDETAIL",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, companyId)));
                }
                if (contractData == null || contractData.Tables[0].Rows.Count <= 0)
                {//if no data was returned, or the table is empty, pass null back
                    return null;
                }
                Hashtable values = new Hashtable();
                BuildTaggerContractDetails(ref values, contractData.Tables[0], true);
                return values;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to load the contract data.");
            }
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public ContractDetail GetContractData(int contractNumber, int companyId)
        {
            try
            {
                DataSet contractData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    contractData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_GETCONTRACTDETAIL",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, companyId)));
                }
                if (contractData == null || contractData.Tables[0].Rows.Count <= 0)
                {
                    return new ContractDetail();
                }
                ContractDetail contractDetail = new ContractDetail();
                contractDetail.ContractNumber = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "CONTRACT_NUMBER", ""));
                contractDetail.Agency = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "AGENCY", ""));
                contractDetail.Advertiser = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "ADVERTISER", ""));
                contractDetail.AE1 = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "AE_1_NAME", ""));
                contractDetail.AE2 = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "AE_2_NAME", ""));
                contractDetail.AE3 = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "AE_3_NAME", ""));
                contractDetail.Program = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "PROGRAM", ""));
                contractDetail.Company = Convert.ToString(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "COMPANY", ""));
                contractDetail.CompanyId = Convert.ToInt32(IO.GetDataRowValue(contractData.Tables[0].Rows[0], "COMPANY_ID", 1));
                contractDetail.ContractDetailLines = new List<ContractDetailLine>();
                foreach (DataRow row in contractData.Tables[0].Rows)
                {
                    contractDetail.ContractDetailLines.Add(new ContractDetailLine
                    {
                        Market = Convert.ToString(IO.GetDataRowValue(row, "MARKET", "")),
                        SalesMarket = Convert.ToString(IO.GetDataRowValue(row, "SALES_MARKET", "")),
                        LineMessage = Convert.ToString(IO.GetDataRowValue(row, "LINE_MESSAGE", "")),
                        ProfitCenter = Convert.ToString(IO.GetDataRowValue(row, "PROFIT_CENTER", "")),
                        MediaType = Convert.ToString(IO.GetDataRowValue(row, "MEDIA_TYPE", "")),
                        MediaForm = Convert.ToString(IO.GetDataRowValue(row, "MEDIA_FORM", "")),
                        StartDateDisplay = Convert.ToString(IO.GetDataRowValue(row, "START_DATE", "")),
                        EndDateDisplay = Convert.ToString(IO.GetDataRowValue(row, "END_DATE", "")),
                        Quantity = Convert.ToInt32(IO.GetDataRowValue(row, "QUANTITY", 0)),
                        Reason = (Convert.ToString(IO.GetDataRowValue(row, "REASON", "")) == "B" ? "Bonus" : (Convert.ToString(IO.GetDataRowValue(row, "REASON", "")) == "A" ? "Added" : Convert.ToString(IO.GetDataRowValue(row, "REASON", "")))),
                        LineItemNumber = Convert.ToInt32(IO.GetDataRowValue(row, "LINE_ITEM_NUMBER", 0))
                    });
                }
                return contractDetail;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return new ContractDetail();
            }
        }

        public class ContractDetail
        {
            public ContractDetail()
            {
            }
            public string ContractNumber { get; set; }
            public string Agency { get; set; }
            public string Advertiser { get; set; }
            public string AE1 { get; set; }
            public string AE2 { get; set; }
            public string AE3 { get; set; }
            public string Program { get; set; }
            public string Company { get; set; }
            public int CompanyId { get; set; }
            public List<ContractDetailLine> ContractDetailLines { get; set; }
        }
        public class ContractDetailLine
        {
            public ContractDetailLine()
            {
            }
            public string Market { get; set; }
            public string SalesMarket { get; set; }
            public string LineMessage { get; set; }
            public string ProfitCenter { get; set; }
            public string MediaType { get; set; }
            public string MediaForm { get; set; }
            public string StartDateDisplay { get; set; }
            public string EndDateDisplay { get; set; }
            public int Quantity { get; set; }
            public string Reason { get; set; }
            public int LineItemNumber { get; set; }
        }

        #region LoadTagData method
        /// <summary>TBD</summary>
        /// <param name="taggingData">TBD</param>
        /// <param name="isContractLookup">TBD</param>
        /// <returns>TBD</returns>
        public Hashtable LoadTagData(DataSet taggingData, bool isContractLookup)
        {
            //The taggingData DataSet will have two tables
            //The first table will contain information related to the file itself
            //The second table will contain information related to tagging information
            Hashtable values = null;
            if (taggingData == null || taggingData.Tables[0].Rows.Count <= 0)
            {//if no data was returned, or the first table is empty, pass null back
                return null;
            }
            bool isTagged = (((string)IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "IS_TAGGED", "")).ToUpper() == "Y");
            values = new Hashtable();
            values.Add("IS_HERO_QUALITY", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "IS_HERO_QUALITY", "N"));
            values.Add("IS_MARKETING_QUALITY", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "IS_MARKETING_QUALITY", "N"));
            values.Add("TAKEN_BY", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "TAKEN_BY", ""));
            values.Add("NOTES", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "NOTES", ""));
            values.Add("FILE_TYPE", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "FILE_TYPE", ""));
            values.Add("IS_TAGGED", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "IS_TAGGED", ""));
            values.Add("IS_DELETED", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "IS_DELETED", ""));
            values.Add("SALES_MARKET_OVERRIDE", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "SALES_MARKET_OVERRIDE", ""));
            values.Add("STATION_MARKET_ID", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "STATION_MARKET_ID", ""));
            values.Add("STATION_ID", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "STATION_ID", ""));
            values.Add("ETHNICITY_ID", IO.GetDataRowValue(taggingData.Tables[0].Rows[0], "ETHNICITY_ID", ""));
            if (isTagged && taggingData.Tables[1].Rows.Count > 0)
            {
                BuildTaggerContractDetails(ref values, taggingData.Tables[1], false);
            }
            else
            {
                values.Add("CONTRACT_NUMBER", "");
                values.Add("AGENCY", "");
                values.Add("ADVERTISER", "");
                values.Add("AE_1_NAME", "");
                values.Add("AE_2_NAME", "");
                values.Add("AE_3_NAME", "");
                values.Add("COMPANY", "");
                values.Add("COMPANY_ID", "");
                //values.Add("SALES_MARKET_OVERRIDE", "");
                values.Add("LINE_ITEMS", "");
            }
            return values;
        }
        #endregion

        #region LoadTaggingData method
        /// <summary>TBD</summary>
        /// <param name="fileId">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Hashtable LoadTaggingData(int fileId, string contractNumber)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("FILEID", SqlDbType.Int, fileId));
            if (!String.IsNullOrEmpty(contractNumber))
            {
                spParams.Add(Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, Convert.ToInt32(contractNumber)));
            }
            DataSet taggingData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                taggingData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_GETTAGDETAIL", spParams));
            }
            return LoadTagData(taggingData, true);
        }
        #endregion

        #region RemoveUploadedFile method
        /// <summary>Deletes a Digital Libary file with the specified id</summary>
        /// <param name="fileId">The id of the file to delete</param>
        /// <param name="fileExtension">The extension of the file to delete</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void RemoveUploadedFile(int fileId, string fileExtension)
        {
            //First delete the file record from the Database
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_DeleteRecord", Param.CreateParam("ID", SqlDbType.Int, fileId)));
            }
            //Then delete the file (and thumbnail, if there is one) from the disk
            DigitalLibraryImaging.DeleteDigitalLibraryFile(fileId, fileExtension);
        }
        #endregion

        #region RequiresNewDataSet method
        /// <summary>TBD</summary>
        /// <param name="totalCount">TBD</param>
        /// <param name="numPerPage">TBD</param>
        /// <param name="currentPageNumber">TBD</param>
        /// <param name="newPageNumber">TBD</param>
        /// <param name="recordSetStartIndex">TBD</param>
        /// <param name="recordSetEndIndex">TBD</param>
        /// <returns>TBD</returns>
        public static bool RequiresNewDataSet(int totalCount, int numPerPage, int currentPageNumber, int newPageNumber, int recordSetStartIndex, int recordSetEndIndex)
        {
            //If the total number of records returned by the query is less than 1000, there is no reason to re-query
            if (totalCount <= 1000)
            {
                return false;
            }
            int newStartIndex = Math.Max(((newPageNumber * numPerPage) - numPerPage), 1);
            int newEndIndex = (newPageNumber * numPerPage);
            //if any of the records needed by the new page exceed the Start/End indices, we need to re-query
            if ((newStartIndex < recordSetStartIndex) || (newEndIndex > recordSetEndIndex))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region TagSelected method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void TagSelected(Hashtable values)
        {
            try
            {
                DataTable tag = GetDigitalLibraryTagTable();
                string[] ids = Convert.ToString(values["ID"]).Split(';');
                string[] lineItems = Convert.ToString(values["LINE_ITEM_NUMBER"]).Split(';');
                int contractNumber = Convert.ToInt32(values["CONTRACT_NUMBER"]);
                string isHeroQuality = ((bool)values["IS_HERO_QUALITY"]) ? "Y" : "N";
                string isMarketingQuality = ((bool)values["IS_MARKETING_QUALITY"]) ? "Y" : "N";
                string takenBy = (string)values["TAKEN_BY"];
                string notes = (string)values["NOTES"];
                string salesMarketOverride = (string)values["SALES_MARKET_OVERRIDE"];
                string stationMarketId = (string)values["STATION_MARKET_ID"];
                string stationId = (string)values["STATION_ID"];
                string ethnicityId = (string)values["ETHNICITY_ID"];
                int companyId = Convert.ToInt32(values["COMPANYID"]);
                
                foreach (string id in ids)
                {
                    //Multiple line items may have been selected
                    foreach (string lineItemNumber in lineItems)
                    {
                        tag.Rows.Add(id.Split(':')[0], contractNumber, lineItemNumber, isHeroQuality, isMarketingQuality, takenBy, notes, salesMarketOverride, stationMarketId, stationId, ethnicityId, companyId);
                        //tag.Rows.Add(id.Split(':')[0], contractNumber, lineItemNumber, isHeroQuality, isMarketingQuality, takenBy, notes, salesMarketOverride, stationMarketId, stationId, companyId);
                    }
                }
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_ADDTAGGROUP", Param.CreateParam("TAGGROUP", SqlDbType.Structured, tag)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred while trying to tag your image(s).");
            }
        }
        #endregion

        #region TagSelectedSingle method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool TagSelectedSingle(Hashtable values)
        {
            try
            {
                DataTable tag = GetDigitalLibraryTagTable();
                int id = Convert.ToInt32(values["ID"]);
                string[] lineItems = Convert.ToString(values["LINE_ITEM_NUMBER"]).Split(';');
                int contractNumber = Convert.ToInt32(values["CONTRACT_NUMBER"]);
                string isHeroQuality = ((bool)values["IS_HERO_QUALITY"]) ? "Y" : "N";
                string isMarketingQuality = ((bool)values["IS_MARKETING_QUALITY"]) ? "Y" : "N";
                string takenBy = (string)values["TAKEN_BY"];
                string notes = (string)values["NOTES"];
                string salesMarketOverride = (string)values["SALES_MARKET_OVERRIDE"];
                string stationMarketId = (string)values["STATION_MARKET_ID"];
                string stationId = (string)values["STATION_ID"];
                string ethnicityId = (string)values["ETHNICITY_ID"];
                int companyId = 1;
                try
                {
                    companyId = Convert.ToInt32(values["COMPANY_ID"]);
                }
                catch
                {
                }
                //Multiple line items may have been selected
                foreach (string lineItemNumber in lineItems)
                {
                    tag.Rows.Add(id, contractNumber, lineItemNumber, isHeroQuality, isMarketingQuality, takenBy, notes, salesMarketOverride, stationMarketId, stationId, ethnicityId, companyId);
                    //tag.Rows.Add(id, contractNumber, lineItemNumber, isHeroQuality, isMarketingQuality, takenBy, notes, salesMarketOverride, stationMarketId, stationId, companyId);
                }
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_ADDTAGGROUP", Param.CreateParam("TAGGROUP", SqlDbType.Structured, tag)));
                }
                return true;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred while trying to tag your image(s).");
            }
        }
        #endregion

        #region ToggleDigitalLibraryActivation method
        /// <summary>TBD</summary>
        /// <param name="fileId">TBD</param>
        /// <param name="activate">TBD</param>
        private void ToggleDigitalLibraryActivation(int fileId, bool activate)
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_UpdateIsDeleted",
                    Param.CreateParam("ID", SqlDbType.Int, fileId),
                    Param.CreateParam("ISDELETED", SqlDbType.Int, Convert.ToInt32(((activate) ? 0 : 1)))));
            }
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void UnmarkWebImage(string imageId)
        {
            try
            {
                DataTable tag = GetDigitalLibraryIdTable();
                tag.Rows.Add(imageId);
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_TOGGLEWEBIMAGEGROUP",
                        Param.CreateParam("IDGROUP", SqlDbType.Structured, tag),
                        Param.CreateParam("ISWEBIMAGE", SqlDbType.Int, 0)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred while trying to mark your selection(s) as Web Images.");
            }
        }

        #region ToggleWebImage method
        /// <summary>TBD</summary>
        /// <param name="imageIds">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void ToggleWebImage(string imageIds)
        {
            try
            {
                DataTable tag = GetDigitalLibraryIdTable();
                string[] ids = imageIds.Split(';');
                foreach (string id in ids)
                {
                    tag.Rows.Add(id.Split(':')[0]);
                }
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_TOGGLEWEBIMAGEGROUP",
                        Param.CreateParam("IDGROUP", SqlDbType.Structured, tag),
                        Param.CreateParam("ISWEBIMAGE", SqlDbType.Int, 1)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred while trying to mark your selection(s) as Web Images.");
            }
        }
        #endregion

        #region UntagDigitalLibraryFile method
        /// <summary>TBD</summary>
        /// <param name="fileId">TBD</param>
        [System.Web.Services.WebMethod]
        public void UntagDigitalLibraryFile(int fileId)
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_DELETETAG", Param.CreateParam("ID", SqlDbType.Int, fileId)));
            }
        }
        #endregion

        #region UpdateSelectedTag method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void UpdateSelectedTag(Hashtable values)
        {
            try
            {
                DataTable tag = GetDigitalLibraryTagTable();
                string[] ids = Convert.ToString(values["ID"]).Split(';');
                string isHeroQuality = ((bool)values["IS_HERO_QUALITY"]) ? "Y" : "N";
                string isMarketingQuality = ((bool)values["IS_MARKETING_QUALITY"]) ? "Y" : "N";
                string takenBy = (string)values["TAKEN_BY"];
                string notes = (string)values["NOTES"];
                string salesMarketOverride = (string)values["SALES_MARKET_OVERRIDE"];
                string stationMarketId = (string)values["STATION_MARKET_ID"];
                string stationId = (string)values["STATION_ID"];
                string ethnicityId = (string)values["ETHNICITY_ID"];
                int companyId = 1;
                foreach (string id in ids)
                {
                    tag.Rows.Add(id.Split(':')[0], -1, -1, isHeroQuality, isMarketingQuality, takenBy, notes, salesMarketOverride, stationMarketId, stationId, ethnicityId, companyId);
                }
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_UPDATETAGGROUP",
                        Param.CreateParam("TAGGROUP", SqlDbType.Structured, tag)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error has occurred while trying to tag your image(s).");
            }
        }
        #endregion

    }

}
