<%@ WebHandler Language="C#" Class="Apollo.DigitalLibraryUploadHandler" %>

using System;
using System.Web;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Titan.DataIO;

namespace Apollo 
{
    public class DigitalLibraryUploadHandler : IHttpHandler 
    {        
        public void ProcessRequest (HttpContext context) 
        {     
            // Get the data
            if (context.Request.Files["Filedata"] == null)
            {//No files were sent in the request object. This should never happen, but we need to account for it.
                WebCommon.LogExceptionInfo(new Exception("UploadHandler was invoked with 0 files in the Request.Files collection"));
                context.Response.StatusCode = 500;
                context.Response.Write("An error occured - Zero files were received.");
                context.Response.End();
            }

            //Get the Session ID
            string batchId = context.Request["BATCHID"] ?? "1";
            string uploadedBy = context.Request["USERID"] ?? "foo";
            //Get the posted file
            HttpPostedFile file = context.Request.Files["Filedata"];
            //Allocate variables for fileupload
            string fileUploadPath = DigitalLibraryImaging.ImageFilePath + "{0}";
            string thumbnailUploadPath = DigitalLibraryImaging.ThumbnailImageFilePath;            
            string originalFileName = string.Empty;
            string fileExtension = string.Empty;
            string fileType = string.Empty;
            string folderId = string.Empty;
            int fileSize = 0;
            int newFileId = 0;            
            //Make sure the uploaded file is not 0-length
            if (file.ContentLength <= 0)
            {//0-length file was received. Log this occurance and send a friendly response.
                WebCommon.LogExceptionInfo(new Exception(string.Format("A 0-length file ({0}) was passed to the UploadHandler.", file.FileName)));
                context.Response.StatusCode = 500;
                context.Response.Write("An error occured - The file received was Zero bytes in size.");
                context.Response.End();
            }
            try
            {
                //Prepare variables for Database Insertion.
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                originalFileName = file.FileName;
                fileExtension = originalFileName.Substring(originalFileName.LastIndexOf('.') + 1);
                fileSize = file.ContentLength;
                spParams.Add(Param.CreateParam("ORIGINALNAME", System.Data.SqlDbType.VarChar, originalFileName));
                spParams.Add(Param.CreateParam("FILEEXTENSION", System.Data.SqlDbType.VarChar, fileExtension));
                spParams.Add(Param.CreateParam("FILESIZE", System.Data.SqlDbType.Int, fileSize));
                spParams.Add(Param.CreateParam("FILETYPE", System.Data.SqlDbType.VarChar, ((fileExtension.ToUpper() == "JPG") ? "I" : "D")));
                spParams.Add(Param.CreateParam("BATCHID", System.Data.SqlDbType.Int, Convert.ToInt32(batchId)));
                spParams.Add(Param.CreateParam("UPLOADEDBY", System.Data.SqlDbType.VarChar, uploadedBy));
                try
                {
                    if (fileExtension.ToUpper() == "JPG")
                    {
                        bool hasLatLong = false;
                        decimal[] imageLatLong = DigitalLibraryImaging.GetImageLatLong(System.Drawing.Image.FromStream(file.InputStream), out hasLatLong);
                        if (hasLatLong)
                        {
                            spParams.Add(Param.CreateParam("IMAGELAT", System.Data.SqlDbType.Decimal, imageLatLong[0]));
                            spParams.Add(Param.CreateParam("IMAGELONG", System.Data.SqlDbType.Decimal, imageLatLong[1]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WebCommon.LogExceptionInfo(ex);
                }
                try
                {
                    DateTime value = DigitalLibraryImaging.GetImageTimestamp(System.Drawing.Image.FromStream(file.InputStream));
                    if (value != DateTime.MinValue)
                    {
                        spParams.Add(Param.CreateParam("DATETAKEN", System.Data.SqlDbType.DateTime, value));
                    }
                    //WebCommon.WriteDebugMessage(string.Format("Attempt to pull Date Taken from Exif: {0} {1}", value.ToShortDateString(), value.ToShortTimeString()));
                }
                catch (Exception ex)
                {
                    WebCommon.LogExceptionInfo(ex);
                }
                //Insert a new record into the database and get the new Sequential File ID
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    newFileId = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("DigitalLibrary_AddItem", spParams)));
                }
                //Determine the destination folder, based on File Id (File Id / 1000).
                folderId = DigitalLibraryImaging.GetDigitalLibraryFileFolder(newFileId);
                //Save File
                WebCommon.SaveFile(file, newFileId, context.Server.MapPath(string.Format(fileUploadPath, folderId)), fileExtension);
                //If this is an image, create and save a Thumbnail...this will reduce page load times and the need for on-the-fly resizing
                if (fileExtension.ToUpper() == "JPG")
                {
                    //Save File Thumbnail
                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(file.InputStream);
                    System.Drawing.Image image = DigitalLibraryImaging.GetImage(originalImage, 220, 220, 0, false);
                    DigitalLibraryImaging.SaveThumbnailImage(newFileId, image, context.Server.MapPath(string.Format("{0}{1}", DigitalLibraryImaging.ThumbnailImageFilePath, DigitalLibraryImaging.GetDigitalLibraryFileFolder(newFileId))));
                }
                //Everything was successful, return the new File Id and Extension
                context.Response.StatusCode = 200;
                context.Response.Write(string.Format("{0}:{1}:{2}:{3}:{4}:{5}", newFileId, fileExtension, DateTime.Now.ToShortDateString(), uploadedBy, fileSize, originalFileName));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                // If any kind of error occurs return a 500 Internal Server error
                context.Response.StatusCode = 500;
                context.Response.Write("An error occured.");
            }
            finally
            {
                context.Response.End();
            }
        }
     
        public bool IsReusable {
            get {
                return false;
            }
        }

    }
}