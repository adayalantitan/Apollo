<%@ WebHandler Language="C#" Class="Apollo.PhotoGalleryImageHandler" %>

using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Apollo
{
    public class PhotoGalleryImageHandler : IHttpHandler
    {
        public string BaseFilePath = @"\\fsnyc03\deptdata$\PhotoGallery\g2data\albums\{0}\{1}";        
        //public const int WIDTH_THUMBNAIL = 220;
        //public const int HEIGHT_THUMBNAIL = 220;
        public const int WIDTH_THUMBNAIL = 175;
        public const int HEIGHT_THUMBNAIL = 175;
        public const int WIDTH_DISPLAY = 640;
        public const int HEIGHT_DISPLAY = 480;
        public const int WIDTH_EMAIL = 800;
        public const int HEIGHT_EMAIL = 533;
        public const int WIDTH_POWERPOINT = 960;
        public const int HEIGHT_POWERPOINT = 640;
        public const int WIDTH_HI_RES = 1200;
        public const int HEIGHT_HI_RES = 800;
        public const int WIDTH_MINI = 75;
        public const int HEIGHT_MINI = 75;   
        
        public struct Parameters
        {
            public string ImageName;
            public int Height;
            public int Width;
            public int RequestedHeight;
            public int RequestedWidth;            
            public bool WantThumb;
            public bool WantDownload;
            public bool WantResize;            
            public bool WantOriginal;
            public bool WantMini;
            public string FolderName;
            public string BackgroundColor;
            public string RequestedQuality;

            public void FillParameters(HttpRequest requestValues)
            {
                ImageName = requestValues["i"] ?? "";
                RequestedHeight = int.Parse(requestValues["h"] ?? "0");
                RequestedWidth = int.Parse(requestValues["w"] ?? "0");                
                WantThumb = ((requestValues["t"] ?? "0") == "1");
                WantDownload = ((requestValues["s"] ?? "0") == "1");
                WantMini = ((requestValues["m"] ?? "0") == "1");
                FolderName = requestValues["p"] ?? "";
                BackgroundColor = requestValues["bg"] ?? "#ffffff";
                RequestedQuality = requestValues["q"] ?? "";

                //if Thumbnail was requested, but height/width were not specified:
                //  default height/width to standard thumbnail measurements
                if (WantThumb && RequestedHeight == 0 && RequestedWidth == 0)
                {
                    Width = WIDTH_THUMBNAIL;
                    Height = HEIGHT_THUMBNAIL;
                }
                else
                {
                    switch (RequestedQuality.ToLower())
                    {
                        case "e": //Email Quality
                            Width = WIDTH_EMAIL;
                            Height = HEIGHT_EMAIL;
                            break;
                        case "p": //Power Point Quality
                            Width = WIDTH_POWERPOINT;
                            Height = HEIGHT_POWERPOINT;
                            break;
                        case "h": //Hi-Res Quality
                            Width = WIDTH_HI_RES;
                            Height = HEIGHT_HI_RES;
                            break;
                        case "d": //Display Quality
                            Width = WIDTH_DISPLAY;
                            Height = HEIGHT_DISPLAY;
                            break;
                        case "m":
                            Width = WIDTH_MINI;
                            Height = HEIGHT_MINI;
                            WantMini = true;
                            break;
                        case "o": //Original
                            Width = 0;
                            Height = 0;
                            break;
                        default:
                            Width = RequestedWidth;
                            Height = RequestedHeight;
                            break;
                    }
                }

                WantResize = (Height != 0 && Width != 0);
                WantOriginal = !WantResize;                
            }
        }
        
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpCachePolicy cachePolicy = context.Response.Cache;                
                cachePolicy.SetCacheability(HttpCacheability.Public);
                //cachePolicy.VaryByParams["q;t;i;p"] = true;
                //cachePolicy.VaryByParams["i;p"] = true;
                cachePolicy.SetOmitVaryStar(true);
                cachePolicy.SetExpires(DateTime.Now + TimeSpan.FromDays(365));
                //cachePolicy.SetExpires(DateTime.Now.AddDays(-1));
                cachePolicy.SetValidUntilExpires(true);
                string filePath = BaseFilePath;
                
                Parameters RequestParameters = new Parameters();
                RequestParameters.FillParameters(context.Request);

                filePath = string.Format(filePath, RequestParameters.FolderName, RequestParameters.ImageName);
                Image image = null;

                //The stream that the (resized) image is created on, must be left open before trying to save:
                //  http://stackoverflow.com/questions/1053052/a-generic-error-occurred-in-gdi-jpeg-image-to-memorystream/1053123#1053123
                using (StreamReader imageStream = new StreamReader(filePath))
                {
                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(imageStream.BaseStream);
                    image = DigitalLibraryImaging.GetImage(originalImage, RequestParameters.Width, RequestParameters.Height, 0, RequestParameters.WantOriginal, false, RequestParameters.BackgroundColor);

                    if (RequestParameters.WantDownload)
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "application/octet-stream";
                        context.Response.AddHeader("content-disposition", "attachment; filename=" + string.Format("{0}", RequestParameters.ImageName));
                    }
                    else
                    {
                        context.Response.ContentType = "image/jpeg";
                    }
                    image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    image.Dispose();
                    context.Response.Flush();
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                context.Response.ContentType = "text/plain";
                context.Response.Write("An error occurred while trying to retrieve the image.");
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