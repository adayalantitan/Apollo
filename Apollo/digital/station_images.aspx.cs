#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class digital_station_images : System.Web.UI.Page
    {

        #region Member variables
        /// <summary>TBD</summary>
        public const string SCREEN_NAME = "station_images";
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
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                WebCommon.ShowAlert("The requested files could not be downloaded. Please try again.");
                Response.Flush();
            }
        }
        #endregion

        #region LoadSearchParams method
        /// <summary>Loads the user's search params</summary>
        public void LoadSearchParams()
        {
            try
            {
                Hashtable searchParams = IOService.LoadSearchParams(Security.GetCurrentUserId, SCREEN_NAME);
                if (searchParams.Count > 0)
                {
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
                    if (!String.IsNullOrEmpty((string)searchParams[dropDownPerPageCount.ID]))
                    {
                        dropDownPerPageCount.SelectedValue = (string)searchParams[dropDownPerPageCount.ID];
                    }
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
                    stationSearch.Id.Text = (string)searchParams["stationSearch.Id"];
                    stationSearch.Name.Text = (string)searchParams["stationSearch.Name"];
                }
                else
                {
                    //Set default values:
                    dropDownCompany.SelectedValue = "1";
                    dropDownCompanyDefault.Value = "1";
                    dropDownPerPageCount.SelectedValue = "20";
                    pageNumber.Value = "1";
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                //Set default values in case of an Exception:
                dropDownCompany.SelectedValue = "1";
                dropDownCompanyDefault.Value = "1";
                dropDownPerPageCount.SelectedValue = "20";
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
                LoadSearchParams();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "StartupSearch", "setTimeout('RefreshPage(false)',1000);", true);
            }
        }
        #endregion

        #region SaveSearchParams method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        [System.Web.Services.WebMethod]
        public static void SaveSearchParams(Hashtable values)
        {
            IOService.SaveSearchParams(values, Security.GetCurrentUserId, SCREEN_NAME);
        }
        #endregion

    }

}
