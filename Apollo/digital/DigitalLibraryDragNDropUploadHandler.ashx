<%@ WebHandler Language="C#" Class="Apollo.DigitalLibraryDragNDropUploadHandler" %>

using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using Titan.DataIO;

namespace Apollo {
    public class DigitalLibraryDragNDropUploadHandler : IHttpHandler {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";            

            if (String.IsNullOrEmpty(context.Request["file"]))
            {//No file was sent. Log this occurance and send a friendly response.
                WebCommon.LogExceptionInfo(new Exception("No file was submitted to the UploadHandler."));
                context.Response.StatusCode = 500;
                context.Response.Write("An error occured - No file was submitted.");
                context.Response.End();                
            }

            if (String.IsNullOrEmpty(context.Request["fileName"]))
            {//No file name was specified. Log this occurance and send a friendly response.
                WebCommon.LogExceptionInfo(new Exception("No file name was specified to the UploadHandler."));
                context.Response.StatusCode = 500;
                context.Response.Write("An error occured - No file name was specified.");
                context.Response.End();                
            }
            //Make sure the uploaded file is not 0-length
            if (context.Request.ContentLength == 0)
            {//0-length file was received. Log this occurance and send a friendly response.
                WebCommon.LogExceptionInfo(new Exception(string.Format("A 0-length file ({0}) was passed to the UploadHandler.", context.Request["fileName"])));
                context.Response.StatusCode = 500;
                context.Response.Write("An error occured - The file received was Zero bytes in size.");
                context.Response.End();
            }
            try
            {
                //Get the Session ID            
                string uploadedBy = Security.GetCurrentUserId;
                //Allocate variables for fileupload
                string fileUploadPath = DigitalLibraryImaging.ImageFilePath + "{0}";
                string thumbnailUploadPath = DigitalLibraryImaging.ThumbnailImageFilePath;
                string originalFileName = context.Request["fileName"] ?? string.Empty;
                string fileBase64 = context.Request["file"] ?? string.Empty;
                fileBase64 = HttpUtility.HtmlDecode(fileBase64);
                fileBase64 = fileBase64.Split(',')[1];
                byte[] file = Convert.FromBase64String(fileBase64);
                string fileExtension = string.Empty;
                string fileType = string.Empty;
                string folderId = string.Empty;
                int fileSize = 0;
                int newFileId = 0;
                //Prepare variables for Database Insertion.                
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                fileExtension = originalFileName.Substring(originalFileName.LastIndexOf('.') + 1);
                fileSize = context.Request.ContentLength;
                spParams.Add(Param.CreateParam("ORIGINALNAME", System.Data.SqlDbType.VarChar, originalFileName));
                spParams.Add(Param.CreateParam("FILEEXTENSION", System.Data.SqlDbType.VarChar, fileExtension));
                spParams.Add(Param.CreateParam("FILESIZE", System.Data.SqlDbType.Int, fileSize));
                spParams.Add(Param.CreateParam("FILETYPE", System.Data.SqlDbType.VarChar, ((fileExtension.ToUpper() == "JPG") ? "I" : "D")));
                spParams.Add(Param.CreateParam("UPLOADEDBY", System.Data.SqlDbType.VarChar, uploadedBy));
                spParams.Add(Param.CreateParam("BATCHID", System.Data.SqlDbType.Int, 1));
                try
                {
                    if (fileExtension.ToUpper() == "JPG")
                    {
                        bool hasLatLong = false;
                        decimal[] imageLatLong;
                        using (System.IO.MemoryStream fileStream = new System.IO.MemoryStream(file))
                        {
                            imageLatLong = DigitalLibraryImaging.GetImageLatLong(System.Drawing.Image.FromStream(fileStream), out hasLatLong);
                        }
                        if (hasLatLong)
                        {
                            spParams.Add(Param.CreateParam("IMAGELAT", System.Data.SqlDbType.Decimal, imageLatLong[0]));
                            spParams.Add(Param.CreateParam("IMAGELONG", System.Data.SqlDbType.Decimal, imageLatLong[1]));
                        }
                    }
                }
                catch (System.ArgumentNullException)
                {
                }
                catch (Exception ex)
                {
                    WebCommon.LogExceptionInfo(ex);
                }
                try
                {
                    DateTime value;
                    using (System.IO.MemoryStream fileStream = new System.IO.MemoryStream(file))
                    {
                        value = DigitalLibraryImaging.GetImageTimestamp(System.Drawing.Image.FromStream(fileStream));
                        if (value != DateTime.MinValue)
                        {
                            spParams.Add(Param.CreateParam("DATETAKEN", System.Data.SqlDbType.DateTime, value));
                        }
                        //WebCommon.WriteDebugMessage(string.Format("Attempt to pull Date Taken from Exif: {0} {1}", value.ToShortDateString(), value.ToShortTimeString()));
                    }
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
                using (System.IO.MemoryStream fileStream = new System.IO.MemoryStream(file))
                {
                    WebCommon.SaveFileFromStream(fileStream, newFileId, context.Server.MapPath(string.Format(fileUploadPath, folderId)), fileExtension);
                }                
                //If this is an image, create and save a Thumbnail...this will reduce page load times and the need for on-the-fly resizing
                if (fileExtension.ToUpper() == "JPG")
                {
                    //Save File Thumbnail                                        
                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream(file));                    
                    System.Drawing.Image image = DigitalLibraryImaging.GetImage(originalImage, 220, 220, 0, false);
                    DigitalLibraryImaging.SaveThumbnailImage(newFileId, image, context.Server.MapPath(string.Format("{0}{1}", DigitalLibraryImaging.ThumbnailImageFilePath, DigitalLibraryImaging.GetDigitalLibraryFileFolder(newFileId))));
                }
                //Everything was successful, return the new File Id and Extension
                context.Response.StatusCode = 200;
                context.Response.Write(newFileId);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}