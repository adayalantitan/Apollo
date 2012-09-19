#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for Image
    /// </summary>
    public abstract class DigitalLibraryImaging
    {

        #region ConvertToDecimalDegrees method
        /// <summary>TBD</summary>
        /// <param name="latLongValue">TBD</param>
        /// <param name="isNegative">TBD</param>
        /// <returns>TBD</returns>
        public static decimal ConvertToDecimalDegrees(string latLongValue, bool isNegative)
        {
            //Degrees are contained in the JPEG as:
            //  ##° ##.##' ###"
            //Position 0 contains degrees
            //Position 1 contains minutes
            //Position 2 contains seconds
            string[] latLongSplit = latLongValue.Split(' ');
            decimal latLongDecimal;
            for (int i = 0; i < latLongSplit.Length; i++)
            {
                latLongSplit[i] = Regex.Replace(latLongSplit[i], @"[^\d\.]+", "");
            }
            latLongDecimal = (isNegative ? -1 : 1) * (Convert.ToInt32(latLongSplit[0]) + (Convert.ToDecimal(latLongSplit[1]) / 60) + (Convert.ToDecimal(latLongSplit[2]) / 3600));
            return latLongDecimal;
        }
        #endregion

        #region DeleteDigitalLibraryFile method
        /// <summary>Deletes a Digital Libary file with the specified id</summary>
        /// <param name="fileId">The id of the file to delete</param>
        /// <param name="fileExtension">The extension of the file to delete</param>
        public static void DeleteDigitalLibraryFile(int fileId, string fileExtension)
        {
            string path = string.Format("{0}{1}/{2}.{3}", DigitalLibraryImaging.ImageFilePath, GetDigitalLibraryFileFolder(fileId), fileId, fileExtension);
            WebCommon.DeleteFile(HttpContext.Current.Server.MapPath(path));
            //if this is an image, delete the corresponding thumbnail as well
            if (fileExtension.ToUpper() == "JPG")
            {
                string thumbPath = string.Format("{0}{1}/{2}_t.JPG", DigitalLibraryImaging.ThumbnailImageFilePath, GetDigitalLibraryFileFolder(fileId), fileId);
                WebCommon.DeleteFile(HttpContext.Current.Server.MapPath(thumbPath));
            }
        }
        #endregion

        #region GenerateMapRow method
        /// <summary>TBD</summary>
        /// <param name="latLongValues">TBD</param>
        /// <returns>TBD</returns>
        public static string GenerateMapRow(Hashtable latLongValues)
        {
            decimal latDecimal, longDecimal;
            //http://maps.google.com/maps/api/staticmap?zoom=14&size=300x300&maptype=roadmap&markers=color:blue|label:S|40.702147,-74.015794012318&sensor=false
            //string rowData = @"<tr><td>{0}&nbsp;</td><td>&nbsp;<span id=""imageLat"">{1}</span></td></tr><tr><td>{2}&nbsp;</td><td>&nbsp;<span id=""imageLong"">{3}</span></td></tr><tr><td>Mapping&nbsp;</td><td><iframe width=""300"" height=""300"" frameborder=""0"" scrolling=""no"" marginheight=""0"" marginwidth=""0"" src=""http://maps.google.com/maps?q={4}%40{1},+{3}&amp;output=embed""></iframe><br /><small><a href=""http://maps.google.com/maps?q=loc:{1},{3}&amp;source=embed&amp;z=14"" style=""color:#0000FF;text-align:left"" target=""_blank"">View Larger Map</a></small></td></tr>";
            string rowData = @"<tr><td>{0}&nbsp;</td><td>&nbsp;<span id=""imageLat"">{1}</span></td></tr><tr><td>{2}&nbsp;</td><td>&nbsp;<span id=""imageLong"">{3}</span></td></tr><tr><td>Mapping&nbsp;</td><td><img src=""http://maps.google.com/maps/api/staticmap?zoom=14&size=300x300&maptype=roadmap&markers=color:blue|label:X|{1},{3}&sensor=false"" title=""Image Location"" alt=""Image Location"" /><br /><small><a href=""http://maps.google.com/maps?q=loc:{1},{3}&amp;z=14"" style=""color:#0000FF;text-align:left"" target=""_blank"">View Larger Map</a></small></td></tr>";
            latDecimal = ConvertToDecimalDegrees(Convert.ToString(latLongValues["GPSLATITUDE"]), Convert.ToString(latLongValues["GPSLATITUDEREF"]).ToUpper().Contains("SOUTH"));
            longDecimal = ConvertToDecimalDegrees(Convert.ToString(latLongValues["GPSLONGITUDE"]), Convert.ToString(latLongValues["GPSLONGITUDEREF"]).ToUpper().Contains("WEST"));
            return string.Format(rowData, "Latitude (Decimal)", latDecimal, "Longitude (Decimal)", longDecimal, latLongValues["ID"]);
        }
        #endregion

        #region GetDigitalLibraryFileFolder method
        /// <summary>Determines the file folder based on the file id</summary>
        /// <param name="imageId">The image id of the requested file</param>
        /// <returns>The file folder derived from the image id (ImageId / 1000)</returns>
        public static string GetDigitalLibraryFileFolder(int imageId)
        {
            return Convert.ToString(Math.Floor(Convert.ToDouble((imageId / 1000))));
        }
        #endregion

        #region GetImage method (originalImage, width, height, resolution, wantOriginal)
        /// <summary>Returns an image from the digital library folder based on the provided parameters</summary>
        /// <param name="originalImage">The original image object</param>
        /// <param name="width">The new width (leave as 0 if no resize is needed)</param>
        /// <param name="height">The new height (leave as 0 if no resize is needed)</param>
        /// <param name="resolution">The new resolution (leave as 0 if no resample is needed)</param>
        /// <param name="wantOriginal">Flag indicating whether the original image is requested</param>
        /// <returns>A System.Drawing.Image object containing the requested image</returns>
        public static System.Drawing.Image GetImage(System.Drawing.Image originalImage, int width, int height, int resolution, bool wantOriginal)
        {
            return GetImage(originalImage, width, height, resolution, wantOriginal, false, "#ffffff");
        }
        #endregion

        #region GetImage method (originalImage, width, height, resolution, wantOriginal, wantFill, backgroundColor)
        /// <summary>Returns an image from the digital library folder based on the provided parameters</summary>
        /// <param name="originalImage">The original image object</param>
        /// <param name="width">The new width (leave as 0 if no resize is needed)</param>
        /// <param name="height">The new height (leave as 0 if no resize is needed)</param>
        /// <param name="resolution">The new resolution (leave as 0 if no resample is needed)</param>
        /// <param name="wantOriginal">Flag indicating whether the original image is requested</param>
        /// <param name="wantFill">Flag indicating whether to use the full Width/Height specified</param>
        /// <param name="backgroundColor">Hex-value of background color</param>
        /// <returns>A System.Drawing.Image object containing the requested image</returns>
        public static System.Drawing.Image GetImage(System.Drawing.Image originalImage, int width, int height, int resolution, bool wantOriginal, bool wantFill, string backgroundColor)
        {
            width = (width == 0) ? originalImage.Width : width;
            height = (height == 0) ? originalImage.Height : height;
            if (wantOriginal)
            {
                return originalImage;
            }
            return ScaleImage(originalImage, width, height, resolution, backgroundColor, wantFill);
        }
        #endregion

        #region GetImageLatLong method (imageId, hasLatLong)
        /// <summary>TBD</summary>
        /// <param name="imageId">TBD</param>
        /// <param name="hasLatLong">TBD</param>
        /// <returns>TBD</returns>
        public static decimal[] GetImageLatLong(int imageId, out bool hasLatLong)
        {            
            decimal[] imageLatLong = InnerGetImageLatLong(new Exif.ExifTagCollection(HostingEnvironment.MapPath(ImageFilePath + GetDigitalLibraryFileFolder(imageId) + "/" + imageId + ".jpg")), out hasLatLong);
            //TEMP
            //if (imageLatLong[0] != -1 && imageLatLong[1] != -1)
            //{
            //    try
            //    {
            //        Hashtable spParams = new Hashtable();
            //        spParams.Add("IMAGEID", imageId);
            //        spParams.Add("IMAGELAT", imageLatLong[0]);
            //        spParams.Add("IMAGELONG", imageLatLong[1]);
            //        DataIO.ExecuteActionQuery(DataIO.GetCommandFromStoredProc("DIG_UPDATELATLONG", spParams));
            //    }
            //    catch (Exception ex)
            //    {
            //        WebCommon.LogExceptionInfo(ex);
            //    }
            //}
            return imageLatLong;
        }
        #endregion

        #region GetImageLatLong method (image, hasLatLong)
        /// <summary>TBD</summary>
        /// <param name="image">TBD</param>
        /// <param name="hasLatLong">TBD</param>
        /// <returns>TBD</returns>
        public static decimal[] GetImageLatLong(System.Drawing.Image image, out bool hasLatLong)
        {
            return InnerGetImageLatLong(new Exif.ExifTagCollection(image), out hasLatLong);
        }
        #endregion

        public static DateTime GetImageTimestamp(System.Drawing.Image image)
        {
            Exif.ExifTagCollection imageTagCollection = new Exif.ExifTagCollection(image);
            string dateTimeOriginalValue = string.Empty;
            foreach (Exif.ExifTag tag in imageTagCollection)
            {
                if (string.Compare(tag.FieldName, "DateTimeOriginal", true) == 0)
                {
                    dateTimeOriginalValue = tag.Value;
                    //WebCommon.WriteDebugMessage(string.Format("Date Taken found in Exif: {0}", tag.Value));
                    break;
                }
            }
            DateTime dateTimeOriginal = DateTime.MinValue;
            if (String.IsNullOrEmpty(dateTimeOriginalValue))
            {
                return DateTime.MinValue;
            }
            if (!DateTime.TryParseExact(dateTimeOriginalValue, "yyyy:MM:dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTimeOriginal))
            {
                return DateTime.MinValue;
            }
            return dateTimeOriginal;
        }

        #region GetImageProperties method
        /// <summary>Retrieve data related to a Digital Library document</summary>
        /// <param name="values">Value collection from</param>
        /// <returns>An HTML-formatted Table containing File and Exif properties</returns>
        public static string GetImageProperties(Hashtable values)
        {
            StringBuilder properties = new StringBuilder();
            int imageId = Convert.ToInt32(values["ID"]);
            Hashtable spParams = new Hashtable();
            Hashtable latLongValues = new Hashtable();
            bool hasLatLong = false;
            spParams.Add("ID", imageId);
            latLongValues.Add("ID", imageId);
            DataSet imageProperties;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                imageProperties = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_GetImageProperties", Param.CreateParam("ID", SqlDbType.Int, Convert.ToInt32(values["ID"]))));
            }
            //Load properties from Digital Library table
            properties.AppendLine(@"<table class=""vs_property"">");
            properties.AppendLine(@"<tr><td colspan=""2"" class=""title"">File</td></tr>");
            properties.AppendFormat(@"<tr><td>Uploaded By&nbsp;</td><td>{0}&nbsp;</td></tr>", Security.GetFullUserNameFromId((string)IO.GetDataRowValue(imageProperties.Tables[0].Rows[0], "UPLOADED_BY", "&nbsp;")));
            properties.AppendFormat(@"<tr><td>Original Name&nbsp;</td><td>{0}&nbsp;</td></tr>", IO.GetDataRowValue(imageProperties.Tables[0].Rows[0], "ORIGINAL_NAME", "&nbsp;"));
            properties.AppendFormat(@"<tr><td>Uploaded At&nbsp;</td><td>{0}&nbsp;</td></tr>", IO.GetDataRowValue(imageProperties.Tables[0].Rows[0], "DATE_UPLOADED", "&nbsp;"));
            properties.AppendFormat(@"<tr><td>Size&nbsp;</td><td>{0}&nbsp;</td></tr>", IO.GetDataRowValue(imageProperties.Tables[0].Rows[0], "FILE_SIZE", "&nbsp;"));
            properties.AppendFormat(@"<tr><td>Status&nbsp;</td><td>{0}&nbsp;</td></tr>", IO.GetDataRowValue(imageProperties.Tables[0].Rows[0], "STATUS", "&nbsp;"));
            if (((string)values["FILE_TYPE"]).ToUpper() == "I")
            {
                //Load properties from Exif Tag
                properties.AppendLine(@"<tr><td colspan=""2"" class=""title"">Image Header Information (exif)</td></tr>");
                if (!File.Exists(HostingEnvironment.MapPath(ImageFilePath + GetDigitalLibraryFileFolder(imageId) + "/" + imageId + ".jpg")))
                {
                    properties.AppendLine(@"<tr><td colspan=""2"">Image Header Information (exif) could not be retrieved for this image.</td></tr>");
                }
                else
                {
                    try
                    {
                        Exif.ExifTagCollection imageTagCollection = new Exif.ExifTagCollection(HostingEnvironment.MapPath(ImageFilePath + GetDigitalLibraryFileFolder(imageId) + "/" + imageId + ".jpg"));
                        foreach (Exif.ExifTag tag in imageTagCollection)
                        {
                            if (tag.FieldName.ToUpper() != "MAKERNOTE")
                            {
                                properties.AppendFormat(@"<tr><td>{0}&nbsp;</td><td>&nbsp;{1}</td></tr>", HttpUtility.HtmlEncode(tag.FieldName), HttpUtility.HtmlEncode(tag.Value));
                            }
                            if ((string.Compare(tag.FieldName, "GPSLATITUDEREF", true) == 0)
                            || (string.Compare(tag.FieldName, "GPSLATITUDE", true) == 0)
                            || (string.Compare(tag.FieldName, "GPSLONGITUDEREF", true) == 0)
                            || (string.Compare(tag.FieldName, "GPSLONGITUDE", true) == 0))
                            {
                                latLongValues.Add(tag.FieldName.ToUpper(), tag.Value);
                                hasLatLong = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WebCommon.LogExceptionInfo(new Exception(string.Format("EXIF Tags could not be retrieved for Image: {0}", values["ID"]), ex));
                        properties.AppendLine(@"<tr><td colspan=""2"">Image Header Information (exif) could not be retrieved for this image.</td></tr>");
                    }
                }
            }
            if (hasLatLong)
            {
                properties.AppendLine(GenerateMapRow(latLongValues));
            }
            properties.AppendLine(@"<table>");
            return properties.ToString();
        }
        #endregion

        #region ImageFilePath property
        /// <summary>The digital library image path</summary>
        /// <value>Returns the digital library image path</value>
        public static string ImageFilePath
        {
            get
            {
                return "/digital/dlib/";
            }
        }
        #endregion

        #region InnerGetImageLatLong method
        /// <summary>TBD</summary>
        /// <param name="imageTagCollection">TBD</param>
        /// <param name="hasLatLong">TBD</param>
        /// <returns>TBD</returns>
        private static decimal[] InnerGetImageLatLong(Exif.ExifTagCollection imageTagCollection, out bool hasLatLong)
        {
            decimal[] imageLatLong = new decimal[2];
            hasLatLong = false;
            Hashtable latLongValues = new Hashtable();
            foreach (Exif.ExifTag tag in imageTagCollection)
            {
                if ((string.Compare(tag.FieldName, "GPSLATITUDEREF", true) == 0)
                    || (string.Compare(tag.FieldName, "GPSLATITUDE", true) == 0)
                    || (string.Compare(tag.FieldName, "GPSLONGITUDEREF", true) == 0)
                    || (string.Compare(tag.FieldName, "GPSLONGITUDE", true) == 0))
                {
                    latLongValues.Add(tag.FieldName.ToUpper(), tag.Value);
                    hasLatLong = true;
                }
            }
            if (hasLatLong)
            {
                //Lat first
                imageLatLong[0] = ConvertToDecimalDegrees(Convert.ToString(latLongValues["GPSLATITUDE"]), Convert.ToString(latLongValues["GPSLATITUDEREF"]).ToUpper().Contains("SOUTH"));
                //Then Long
                imageLatLong[1] = ConvertToDecimalDegrees(Convert.ToString(latLongValues["GPSLONGITUDE"]), Convert.ToString(latLongValues["GPSLONGITUDEREF"]).ToUpper().Contains("WEST"));
            }
            else
            {
                imageLatLong[0] = -1;
                imageLatLong[1] = -1;
            }
            return imageLatLong;
        }
        #endregion

        #region SaveThumbnailImage method
        /// <summary>TBD</summary>
        /// <param name="imageId">TBD</param>
        /// <param name="thumbNailImage">TBD</param>
        /// <param name="thumbFilePath">TBD</param>
        public static void SaveThumbnailImage(int imageId, System.Drawing.Image thumbNailImage, string thumbFilePath)
        {
            //Make sure the folder exists
            if (!Directory.Exists(thumbFilePath))
            {
                Directory.CreateDirectory(thumbFilePath);
            }
            using (FileStream archive = File.Open(thumbFilePath + "/" + imageId + "_t.JPG", FileMode.Create, FileAccess.Write))
            {
                thumbNailImage.Save(archive, ImageFormat.Jpeg);
            }
        }
        #endregion

        #region ScaleImage method
        /// <summary>Resizes/Resamples an image</summary>
        /// <param name="originalImage">The image to resize/resample</param>
        /// <param name="width">The desired width</param>
        /// <param name="height">The desired height</param>
        /// <param name="resolution">The desired resolution (in DPI)</param>
        /// <param name="backgroundColor">The background color (hex format e.g: #ffffff)</param>
        /// <param name="wantFill">TBD</param>
        /// <returns>Returns a System.Drawing.Image object containing the resized/resampled image</returns>
        public static System.Drawing.Image ScaleImage(System.Drawing.Image originalImage, int width, int height, int resolution, string backgroundColor, bool wantFill)
        {
            int sourceWidth = originalImage.Width;
            int sourceHeight = originalImage.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            int startX = 0;
            int startY = 0;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((height - (sourceHeight * nPercent)) / 2);
            }
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap bmPhoto;
            if (wantFill)
            {
                bmPhoto = new Bitmap(width, height);
                startX = Convert.ToInt16((width - destWidth) / 2);
                startY = Convert.ToInt16((height - destHeight) / 2);
            }
            else
            {
                bmPhoto = new Bitmap(destWidth, destHeight);
            }
            if (resolution != 0)
            {
                bmPhoto.SetResolution(resolution, resolution);
            }
            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                Color c = ColorTranslator.FromHtml(backgroundColor);
                grPhoto.Clear(c);
                grPhoto.CompositingQuality = CompositingQuality.HighQuality;
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.DrawImage(originalImage, new System.Drawing.Rectangle(startX, startY, destWidth, destHeight),
                new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);
            }
            return bmPhoto;
        }
        #endregion

        #region ThumbnailImageFilePath property
        /// <summary>The digital library thumbnail image path</summary>
        /// <value>Returns the digital library thumbnail image path</value>
        public static string ThumbnailImageFilePath
        {
            get
            {
                return "/digital/thumb/";
            }
        }
        #endregion

        /*
        public static Document GetPoPPdf(int imageId)
        {
            Hashtable spParams = new Hashtable();
            spParams.Add("FILEID", imageId);
            DataSet imageTaggingData = DataIO.ExecuteDataSetQuery(DataIO.GetCommandFromStoredProc("DIGITALLIBRARY_GETTAGDETAIL", spParams));

            string filePath = string.Format("{0}{1}/{2}.jpg", DigitalLibraryImaging.ImageFilePath, DigitalLibraryImaging.GetDigitalLibraryFileFolder(imageId), imageId);
            string logoFilePath = "/Images/titan_logo_pop.png";

            iTextSharp.text.Jpeg docImage;
            iTextSharp.text.ImgRaw logoImage;

            using (StreamReader imageStream = new StreamReader(HostingEnvironment.MapPath(HostingEnvironment.ApplicationVirtualPath + filePath)))
            {
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(imageStream.BaseStream);
                System.Drawing.Image image = DigitalLibraryImaging.GetImage(originalImage, 700, 520, 96, false, false, string.Empty);
                docImage = new Jpeg(iTextSharp.text.Image.GetInstance(image, BaseColor.WHITE));
                docImage.SetDpi(96, 96);
            }
            using (StreamReader imageStream = new StreamReader(HostingEnvironment.MapPath(HostingEnvironment.ApplicationVirtualPath + "/" + logoFilePath)))
            {
                logoImage = new ImgRaw(iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(imageStream.BaseStream), BaseColor.WHITE));
                logoImage.SetDpi(96, 96);
            }
            //Image docImage = iTextSharp.text.Image.GetInstance(image, BaseColor.WHITE);
            //docImage.Alignment = Image.ALIGN_CENTER;                

            Document myDoc;
            bool isLandscape = false;
            if (docImage.Width > docImage.Height)
            {//Landscape
                myDoc = new Document(PageSize.LETTER.Rotate(), 36f, 36f, 36f, 0f);
                isLandscape = true;
            }
            else
            {
                myDoc = new Document(PageSize.LETTER, 36f, 36f, 36f, 0f);
                docImage.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            }
            myDoc.Open();

            iTextSharp.text.Font titanFont = FontFactory.GetFont(FontFactory.HELVETICA, 24f, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 187, 237));
            Phrase mediaPhrase = new Phrase(string.Format("{0} {1}", IO.GetDataRowValue(imageTaggingData.Tables[1].Rows[0], "MEDIA_TYPE", ""), IO.GetDataRowValue(imageTaggingData.Tables[1].Rows[0], "MEDIA_FORM", "")).ToLower(), titanFont);
            Phrase marketPhrase = new Phrase(string.Format("{0}", IO.GetDataRowValue(imageTaggingData.Tables[1].Rows[0], "MARKET", "")).ToLower(), titanFont);
            Phrase advertiserPhrase = new Phrase(string.Format("{0}", IO.GetDataRowValue(imageTaggingData.Tables[1].Rows[0], "ADVERTISER", "")), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18f, iTextSharp.text.Font.BOLDITALIC, new BaseColor(128, 128, 128)));
            PdfPTable topTable = new PdfPTable(2);
            float[] totalWidths = { PageSize.LETTER.Width / 2, PageSize.LETTER.Width / 2 };
            topTable.SetWidthPercentage(totalWidths, PageSize.LETTER);
            topTable.SpacingBefore = 0f;
            topTable.SpacingAfter = 10f;
            PdfPCell cell = new PdfPCell(mediaPhrase);
            cell.HorizontalAlignment = iTextSharp.text.Image.ALIGN_LEFT;
            cell.VerticalAlignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            cell.BorderColor = new BaseColor(255, 255, 255);
            topTable.AddCell(cell);
            cell = new PdfPCell(marketPhrase);
            cell.HorizontalAlignment = iTextSharp.text.Image.ALIGN_RIGHT;
            cell.VerticalAlignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            cell.BorderColor = new BaseColor(255, 255, 255);
            topTable.AddCell(cell);

            PdfPTable bottomTable = new PdfPTable(2);
            bottomTable.SetWidthPercentage(totalWidths, PageSize.LETTER);
            bottomTable.SpacingBefore = 10f;
            bottomTable.SpacingAfter = 0f;

            cell = new PdfPCell(advertiserPhrase);
            cell.HorizontalAlignment = iTextSharp.text.Image.ALIGN_LEFT;
            cell.VerticalAlignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            cell.BorderColor = new BaseColor(255, 255, 255);
            bottomTable.AddCell(cell);

            cell = new PdfPCell(logoImage);
            cell.HorizontalAlignment = iTextSharp.text.Image.ALIGN_RIGHT;
            cell.VerticalAlignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            cell.BorderColor = new BaseColor(255, 255, 255);
            bottomTable.AddCell(cell);

            myDoc.Add(topTable);

            if (isLandscape)
            {
                myDoc.Add(docImage);
            }
            else
            {
                float[] singleWidth = { PageSize.LETTER.Width };
                PdfPTable imageTable = new PdfPTable(1);
                imageTable.SetWidthPercentage(singleWidth, PageSize.LETTER);
                cell = new PdfPCell(docImage);
                cell.HorizontalAlignment = iTextSharp.text.Image.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                cell.BorderColor = new BaseColor(255, 255, 255);
                cell.FixedHeight = PageSize.LETTER.Height - 180f;
                imageTable.AddCell(cell);
                myDoc.Add(imageTable);
            }

            myDoc.Add(bottomTable);
            myDoc.Close();
            return myDoc;
        }
         * */

    }

}
