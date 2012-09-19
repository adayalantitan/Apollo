#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Ionic.Zip;
#endregion

namespace Apollo
{

    /// <summary>Digital Library Code-Behind</summary>
    public partial class digital_digitalLibrary : System.Web.UI.Page
    {

        #region Member variables
        /// <summary>TBD</summary>
        public const string SCREEN_NAME = "digitalLibrary";
        #endregion

        #region BindUploadedBy method
        /// <summary>TBD</summary>
        private void BindUploadedBy()
        {
            DataSet ds = App.GetCachedDataSet(App.DataSetType.UploadedByDataSetType);
            string userId, userName;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!String.IsNullOrEmpty((string)row["UPLOADED_BY_ID"] ?? ""))
                {
                    userId = (string)row["UPLOADED_BY_ID"];
                    userName = (String.IsNullOrEmpty(Security.GetFullUserNameFromId(userId))) ? userId : Security.GetFullUserNameFromId(userId);
                    dropDownUploadedBy.Items.Add(new ListItem(userName, userId));
                }
            }
        }
        #endregion

        #region clear_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void clear_Click(object sender, ImageClickEventArgs e)
        {
        }
        #endregion

        #region downloadImages_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void downloadImages_Click(object sender, ImageClickEventArgs e)
        {
            string[] downloadList = selectedImages.Value.Split(';');
            string quality = (radioEmail.Checked) ? "e" : "o";
            int width = (quality == "e") ? 800 : 0;
            int height = (quality == "e") ? 533 : 0;
            bool wantOriginal = (quality == "o");
            string id = string.Empty;
            string contractNumber = string.Empty;
            string country = (Request.Form[dropDownCompany.UniqueID] == "1") ? "USA" : "CAN";
            string fileType = string.Empty;
            string fileExtension = string.Empty;
            string fileName = "{0}_{1}_{2}_{3}.{4}";
            string baseFilePath = DigitalLibraryImaging.ImageFilePath + "{0}/{1}.{2}";
            string path = string.Empty;
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (string download in downloadList)
                    {
                        id = download.Split(':')[0];
                        contractNumber = download.Split(':')[1].Split('-')[0];
                        fileType = WebCommon.DetermineFileType(download.Split(':')[1].Split('-')[1]);
                        fileExtension = download.Split(':')[1].Split('-')[2];
                        path = Server.MapPath(string.Format(baseFilePath, DigitalLibraryImaging.GetDigitalLibraryFileFolder(Convert.ToInt32(id)), id, fileExtension));
                        if (File.Exists(path))
                        {
                            if (fileType.ToUpper() == "PHOTO")
                            {
                                using (StreamReader imageStream = new StreamReader(path))
                                {
                                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(imageStream.BaseStream);
                                    System.Drawing.Image image = DigitalLibraryImaging.GetImage(originalImage, width, height, 0, wantOriginal);
                                    MemoryStream zipImage = new MemoryStream();
                                    image.Save(zipImage, ImageFormat.Jpeg);
                                    zip.AddEntry(string.Format(fileName, country, contractNumber, fileType, id, fileExtension), zipImage.ToArray());
                                }
                            }
                            else
                            {
                                using (FileStream file = File.OpenRead(path))
                                {
                                    MemoryStream zipFile = new MemoryStream();
                                    zipFile.SetLength(file.Length);
                                    file.Read(zipFile.GetBuffer(), 0, (int)file.Length);
                                    zipFile.Flush();
                                    zip.AddEntry(string.Format(fileName, country, contractNumber, fileType, id, fileExtension), zipFile.ToArray());
                                }
                            }
                        }
                    }
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=Titan_Digital_Library_" + DateTime.Now.ToShortDateString().Replace('/', '-') + ".zip");
                    zip.Save(Response.OutputStream);
                }
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                WebCommon.ShowAlert("The requested files could not be downloaded. Please try again.");
                Response.Flush();
            }
        }
        #endregion

        public object GetSearchParamValue(Hashtable searchParams, string key, object defaultValue)
        {
            if (searchParams.ContainsKey(key))
            {
                return searchParams[key];
            }
            return defaultValue;
        }

        public void ClearFilters()
        {
            radioTagged.Checked = true;
            radioUntagged.Checked = false;
            radioWebImages.Checked = false;
            checkHero.Checked = false;
            checkPhoto.Checked = false;
            checkExcludeMTA.Checked = false;
            checkExcludeTwinAmerica.Checked = false;
            textStartDateFrom.Text = "";
            textStartDateTo.Text = "";
            textEndDateFrom.Text = "";
            textEndDateTo.Text = "";
            textUploadedFrom.Text = "";
            textUploadedTo.Text = "";
            dropDownCompany.SelectedValue = "";
            dropDownCompanyDefault.Value = "";
            dropDownMarket.SelectedValue = "";
            dropDownMarketDefault.Value = "";
            dropDownSubMarket.SelectedValue = "";
            dropDownSubMarketDefault.Value = "";
            dropDownProfitCenter.SelectedValue = "";
            dropDownProfitCenterDefault.Value = "";
            dropDownProductClass.SelectedValue = "";
            dropDownProductClassDefault.Value = "";
            dropDownMediaType.SelectedValue = "";
            dropDownMediaTypeDefault.Value = "";
            dropDownUploadedBy.SelectedValue = "";
            dropDownDocType.SelectedValue = "";
            dropDownPerPageCount.SelectedValue = "100";
            dropDownEthnicity.SelectedValue = "";
            dropDownEthnicityDefault.Value = "";
            textSearch.Text = "";
            textContractNumber.Text = "";
            textProgram.Text = "";
            textOriginalName.Text = "";
            textNotes.Text = "";
            mediaFormSearch.Id.Text = "";
            mediaFormSearch.Name.Text = "";
            agencySearch.Id.Text = "";
            agencySearch.Name.Text = "";
            advertiserSearch.Id.Text = "";
            advertiserSearch.Name.Text = "";
            aeSearch.Id.Text = "";
            aeSearch.Name.Text = "";
            productClassSearch.Id.Text = "";
            productClassSearch.Name.Text = "";
            stationSearch.Id.Text = "";
            stationSearch.Name.Text = "";
            pageNumber.Value = "1";
            previousPageNumber.Value = "1";
        }

        #region LoadSearchParams method
        /// <summary>Loads the user's search params</summary>
        public void LoadSearchParams()
        {
            try
            {
                Hashtable searchParams = IOService.LoadSearchParams(Security.GetCurrentUserId, SCREEN_NAME);
                if (searchParams.Count > 0)
                {
                    //Group By Fields
                    if ((Security.IsDigitalUser() || Security.IsCorporateUser() || Security.IsAdminUser()))
                    {
                        radioTagged.Checked = (((string)searchParams[radioTagged.ID]).ToLower() == "true");
                        radioUntagged.Checked = !radioTagged.Checked;
                        radioWebImages.Checked = (((string)searchParams[radioWebImages.ID]).ToLower() == "true");
                    }
                    else
                    {
                        radioTagged.Checked = true;
                        radioUntagged.Checked = false;
                        radioWebImages.Checked = false;
                    }
                    checkHero.Checked = (((string)searchParams[checkHero.ID]).ToLower() == "true");
                    checkPhoto.Checked = (((string)searchParams[checkPhoto.ID]).ToLower() == "true");
                    checkExcludeMTA.Checked = (((string)searchParams[checkExcludeMTA.ID]).ToLower() == "true");
                    checkExcludeTwinAmerica.Checked = Convert.ToBoolean(GetSearchParamValue(searchParams, checkExcludeTwinAmerica.ID, false));
                    textStartDateFrom.Text = (string)searchParams[textStartDateFrom.ID];
                    textStartDateTo.Text = (string)searchParams[textStartDateTo.ID];
                    textEndDateFrom.Text = (string)searchParams[textEndDateFrom.ID];
                    textEndDateTo.Text = (string)searchParams[textEndDateTo.ID];
                    textUploadedFrom.Text = (string)searchParams[textUploadedFrom.ID];
                    textUploadedTo.Text = (string)searchParams[textUploadedTo.ID];
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownCompany.ID]))
                    {
                        dropDownCompany.SelectedValue = (string)searchParams[dropDownCompany.ID];
                        dropDownCompanyDefault.Value = (string)searchParams[dropDownCompany.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownMarket.ID]))
                    {
                        dropDownMarket.SelectedValue = (string)searchParams[dropDownMarket.ID];
                        dropDownMarketDefault.Value = (string)searchParams[dropDownMarket.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownSubMarket.ID]))
                    {
                        dropDownSubMarket.SelectedValue = (string)searchParams[dropDownSubMarket.ID];
                        dropDownSubMarketDefault.Value = (string)searchParams[dropDownSubMarket.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownProfitCenter.ID]))
                    {
                        dropDownProfitCenter.SelectedValue = (string)searchParams[dropDownProfitCenter.ID];
                        dropDownProfitCenterDefault.Value = (string)searchParams[dropDownProfitCenter.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownProductClass.ID]))
                    {
                        dropDownProductClass.SelectedValue = (string)searchParams[dropDownProductClass.ID];
                        dropDownProductClassDefault.Value = (string)searchParams[dropDownProductClass.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownMediaType.ID]))
                    {
                        dropDownMediaType.SelectedValue = (string)searchParams[dropDownMediaType.ID];
                        dropDownMediaTypeDefault.Value = (string)searchParams[dropDownMediaType.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownUploadedBy.ID]))
                    {
                        dropDownUploadedBy.SelectedValue = (string)searchParams[dropDownUploadedBy.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownDocType.ID]))
                    {
                        dropDownDocType.SelectedValue = (string)searchParams[dropDownDocType.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownPerPageCount.ID]))
                    {
                        dropDownPerPageCount.SelectedValue = (string)searchParams[dropDownPerPageCount.ID];
                    }
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownEthnicity.ID]))
                    {
                        dropDownEthnicity.SelectedValue = (string)searchParams[dropDownEthnicity.ID];
                        dropDownEthnicityDefault.Value = (string)searchParams[dropDownEthnicity.ID];
                    }
                    textSearch.Text = (string)searchParams[textSearch.ID];
                    textContractNumber.Text = (string)searchParams[textContractNumber.ID];
                    textProgram.Text = (string)searchParams[textProgram.ID];
                    textOriginalName.Text = (string)searchParams[textOriginalName.ID];
                    textNotes.Text = (string)searchParams[textNotes.ID];
                    mediaFormSearch.Id.Text = (string)searchParams["mediaFormSearch.MediaFormId"];
                    mediaFormSearch.Name.Text = (string)searchParams["mediaFormSearch.MediaFormName"];
                    agencySearch.Id.Text = (string)searchParams["agencySearch.AgencyId"];
                    agencySearch.Name.Text = (string)searchParams["agencySearch.AgencyName"];
                    advertiserSearch.Id.Text = (string)searchParams["advertiserSearch.AdvertiserId"];
                    advertiserSearch.Name.Text = (string)searchParams["advertiserSearch.AdvertiserName"];
                    aeSearch.Id.Text = (string)searchParams["aeSearch.AEId"];
                    aeSearch.Name.Text = (string)searchParams["aeSearch.AEName"];
                    productClassSearch.Id.Text = (string)searchParams["productClassSearch.ProductClassId"];
                    productClassSearch.Name.Text = (string)searchParams["productClassSearch.ProductClassName"];
                    stationSearch.Id.Text = (string)searchParams["stationSearch.StationId"];
                    stationSearch.Name.Text = (string)searchParams["stationSearch.StationName"];
                    if (!String.IsNullOrEmpty((string)searchParams[pageNumber.ID]))
                    {
                        pageNumber.Value = (string)searchParams[pageNumber.ID];
                        previousPageNumber.Value = (string)searchParams[pageNumber.ID];
                    }
                    else
                    {
                        pageNumber.Value = "1";
                        previousPageNumber.Value = "1";
                    }
                }
                else
                {
                    //Set default values:
                    dropDownCompany.SelectedValue = "1";
                    dropDownCompanyDefault.Value = "1";
                    dropDownUploadedBy.SelectedValue = "";
                    dropDownPerPageCount.SelectedValue = "20";
                    dropDownSubMarketDefault.Value = "";
                    radioTagged.Checked = true;
                    radioUntagged.Checked = !radioTagged.Checked;
                    pageNumber.Value = "1";
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                //Set default values in case of an Exception:
                dropDownCompany.SelectedValue = "1";
                dropDownCompanyDefault.Value = "1";
                dropDownUploadedBy.SelectedValue = "";
                dropDownPerPageCount.SelectedValue = "20";
                dropDownSubMarketDefault.Value = "";
                radioTagged.Checked = true;
                radioUntagged.Checked = !radioTagged.Checked;
                pageNumber.Value = "1";
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
                isPaging.Value = "0";
                BindUploadedBy();
                LoadSearchParams();
                LoadQueryParams();
                isPhoto.Value = ((Security.IsDigitalUser() || Security.IsAdminUser() || Security.IsCorporateUser()) ? "1" : "0");
                SecureIt();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "StartupSearch", "setTimeout('RefreshPage(true)',1000);", true);
            }
        }
        #endregion

        public void LoadQueryParams()
        {
            if (Request.QueryString.Count > 0)
            {
                //Clear Filters
                ClearFilters();
                textContractNumber.Text = Request.QueryString["contractNumber"];
                if (String.IsNullOrEmpty(Request.QueryString["uploadFromDate"]))
                {
                    return;
                }
                string uploadFromDateText = Request.QueryString["uploadFromDate"];
                if (uploadFromDateText.Length < 14)
                {
                    return;
                }
                DateTime uploadFromDate;// = new DateTime(Convert.ToInt32(uploadFromDateText.Substring(0, 4)), Convert.ToInt32(uploadFromDateText.Substring(4, 2)), Convert.ToInt32(uploadFromDateText.Substring(6, 2)), 10, 0, 0);
                if (!DateTime.TryParse(uploadFromDateText, out uploadFromDate))
                {
                    uploadFromDate = new DateTime(Convert.ToInt32(uploadFromDateText.Substring(0, 4))
                        , Convert.ToInt32(uploadFromDateText.Substring(4, 2))
                        , Convert.ToInt32(uploadFromDateText.Substring(6, 2))
                        , Convert.ToInt32(uploadFromDateText.Substring(8, 2))
                        , Convert.ToInt32(uploadFromDateText.Substring(10, 2))
                        , Convert.ToInt32(uploadFromDateText.Substring(12, 2)));
                }
                DateTime uploadToDate = uploadFromDate.AddDays(1);
                if (uploadToDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    uploadToDate = uploadToDate.AddDays(2);
                }
                if (uploadToDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    uploadToDate = uploadToDate.AddDays(1);
                }
                textUploadedFrom.Text = string.Format("{0} {1}", uploadFromDate.ToShortDateString(), uploadFromDate.ToString("hh:mm:ss tt"));
                textUploadedTo.Text = string.Format("{0} {1}", uploadToDate.ToShortDateString(), uploadToDate.ToString("hh:mm:ss tt"));
            }
        }

        #region SaveSearchParams method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        [System.Web.Services.WebMethod]
        public static void SaveSearchParams(Hashtable values)
        {
            IOService.SaveSearchParams(values, Security.GetCurrentUserId, SCREEN_NAME);
        }
        #endregion

        #region SecureIt method
        /// <summary>TBD</summary>
        private void SecureIt()
        {
            bool isDigitalLibraryUser = (Security.IsDigitalUser() || Security.IsCorporateUser() || Security.IsAdminUser());
            bool isTiffany = (String.Compare(Security.GetCurrentUserId, "TNSMITH", true) == 0);
            //this.isDigitalLibraryUser.Value = (isDigitalLibraryUser) ? "-1" : "0";
            //digitalUpload.Style["display"] = (isDigitalLibraryUser) ? "inline" : "none";
            //digitalTag.Style["display"] = (isDigitalLibraryUser) ? "inline" : "none";            
            radioUntagged.Style["display"] = (isDigitalLibraryUser) ? "inline" : "none";
            radioTagged.Style["display"] = (isDigitalLibraryUser) ? "inline" : "none";
            radioWebImages.Style["display"] = (isTiffany || Security.IsAdminUser() || Security.IsMarketingUser()) ? "inline" : "none";
            //webImage.Style["display"] = (isTiffany || Security.IsAdminUser()) ? "inline" : "none";
            //canTagWebImage.Value = (isTiffany || Security.IsAdminUser()) ? "-1" : "0";
        }
        #endregion
    }
}
