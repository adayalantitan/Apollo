<%@ WebHandler Language="C#" Class="Apollo.DigitalLibraryImageHandler" %>

using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace Apollo 
{
    public class DigitalLibraryImageHandler : IHttpHandler
    {
        /********** 
         * Usage
         *  Request Parameters:
         *      i: Image Id
         *          Required. 
         *              The unique ID of the desired image/document
         *      c: Contract Number
         *          Optional. 
         *              Will be used for naming of Download File
         *      x: File Extension
         *          Optional. 
         *              If left blank, .jpg will be used
         *      h: Height
         *          Optional. 
         *              If left blank, default will be 0.
         *              If Thumbnail Parameter (t) is 1, default Thumbnail Height will be supplied
         *              If Quality Parameter (q) is specified, one of the default Height measurements will be supplied
         *      w: Width
         *          Optional.
         *              If left blank, default will be 0.
         *              If Thumbnail Parameter (t) is 1, default Thumbnail Width will be supplied
         *              If Quality Parameter (q) is specified, one of the default Width measurements will be supplied
         *      d: DPI
         *          Optional.
         *              If left blank, default will be 0, image will not be resampled.
         *      q: Quality
         *          Optional. Defaults to blank. Can be one of the following values:
         *              e: Email Quality (800x533)
         *              p: Power Point Quality (960x640)
         *              h: Hi-Res Quality (1200x800)
         *              m: Mini (30x30)
         *              o: Original Quality
         *              i: iPad (1074 x 768 @ 132 dpi)
         *          If a quality is specified, Height (h) and Width (w) do not need to be provided
         *      t: Thumbnail Flag
         *          Optional. Will default to 0.
         *              If t = 1, a Thumbnail image will be returned
         *      s: Download Flag
         *          Optional. Will default to 0.
         *              If s = 1, the response type will be set to Octet-stream to allow for downloading of data.
         *      f: Fill Flag
         *          Optional. Will default to 0.
         *              If f = 1, the image will take up the full Width x Height value specified
         *      bg: Background Color
         *          Optional. Will default to #ffffff         
         *  ***********************
         *  COMMON REQUESTS:
         *  
         *  JPEG Thumbnail Request: 
         *      DigitalLibraryImageHandler.ashx?i=1234&t=1
         *  Placeholder Thumbnail Request (HTML, SWF, PDF...etc documents):
         *      DigitalLibraryImageHandler.ashx?i=1234&t=1&x=pdf (or html,swf...etc);
         *  Original Request:
         *      DigitalLibraryImageHandler.ashx?i=1234&q=o
         *  Original Request for Download:
         *      DigitalLibraryImageHandler.ashx?i=1234&q=o&c=20999999&s=1
         *  Other Quality Request:
         *      DigitalLibraryImageHandler.ashx?i=1234&q= (e for Email Quality, p for Powerpoint Quality, h for Hi-Res)         
         **********/
        public string BaseThumbnailFilePath = DigitalLibraryImaging.ThumbnailImageFilePath + "{0}/{1}_t.{2}";
        public string BaseFilePath = DigitalLibraryImaging.ImageFilePath + "{0}/{1}.{2}";
        public string BasePlaceholderImagePath = "/Images/dl/{0}.jpg";
        //public const int WIDTH_THUMBNAIL = 140;
        //public const int HEIGHT_THUMBNAIL = 140;
        public const int WIDTH_THUMBNAIL = 220;
        public const int HEIGHT_THUMBNAIL = 220;
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
        public const int WIDTH_IPAD = 1024;
        public const int HEIGHT_IPAD = 768;

        public struct Parameters
        {
            public int ImageId;            
            public int Height;
            public int Width;
            public int RequestedHeight;
            public int RequestedWidth;
            public int RequestedDpi;
            public string ContractNumber;            
            public string BackgroundColor;
            public string RequestedQuality;
            public string FileExtension;
            public string FileFolder;
            public string FileType;
            public bool WantThumb;
            public bool WantDownload;
            public bool WantResize;
            public bool WantResample;
            public bool WantOriginal;
            public bool WantMini;
            public bool WantFill;
            public bool WantWebImageCopy;

            public void FillParameters(HttpRequest requestValues)
            {
                WantWebImageCopy = ((requestValues["wwc"] ?? "0") == "1");
                ImageId = int.Parse(requestValues["i"] ?? "0");
                ContractNumber = requestValues["c"] ?? "0";
                BackgroundColor = requestValues["bg"] ?? "#ffffff";
                FileExtension = requestValues["x"] ?? "jpg";
                FileType = WebCommon.DetermineFileType(requestValues["ft"] ?? "I");
                RequestedHeight = int.Parse(requestValues["h"] ?? "0");
                RequestedWidth = int.Parse(requestValues["w"] ?? "0");
                RequestedDpi = int.Parse(requestValues["d"] ?? "0");
                RequestedQuality = requestValues["q"] ?? "";
                WantThumb = ((requestValues["t"] ?? "0") == "1");
                WantDownload = ((requestValues["s"] ?? "0") == "1");
                WantFill = ((requestValues["f"] ?? "0") == "1");

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
                        case "i":
                            Width = WIDTH_IPAD;
                            Height = HEIGHT_IPAD;
                            RequestedDpi = 132;
                            break;
                        default:
                            Width = RequestedWidth;
                            Height = RequestedHeight;                            
                            break;
                    }                    
                }

                WantResize = (Height != 0 && Width != 0);
                WantResample = (RequestedDpi != 0);
                WantOriginal = (!WantResize && !WantResample);
                FileFolder = DigitalLibraryImaging.GetDigitalLibraryFileFolder(ImageId);
            }
        }

        public void SendPlaceholderImage(HttpContext context, Parameters RequestParameters)
        {
            string thumbnailPath = string.Empty;
            switch (RequestParameters.FileExtension.ToLower())
            {
                case "doc":
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_doc");
                    break;
                case "flv":
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_flv");
                    break;
                case "htm":
                case "html":
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_html");
                    break;
                case "jpg":
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_jpg");
                    break;
                case "pdf":
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_pdf");
                    break;
                case "swf":
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_swf");
                    break;
                case "xls":
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_xls");
                    break;
                case "txt":
                default:
                    thumbnailPath = string.Format(BasePlaceholderImagePath, "thumbnail_txt");
                    break;
            }
            context.Response.ContentType = "image/jpeg";
            if (RequestParameters.WantMini)
            {
                using (StreamReader imageStream = new StreamReader(context.Server.MapPath(thumbnailPath)))
                {
                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(imageStream.BaseStream);
                    System.Drawing.Image image = DigitalLibraryImaging.GetImage(originalImage, RequestParameters.Width, RequestParameters.Height, RequestParameters.RequestedDpi, RequestParameters.WantOriginal);
                    image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    image.Dispose();
                }
            }
            else
            {
                context.Response.WriteFile(thumbnailPath);
            }
            context.Response.Flush();
        }
            
        public void ProcessRequest (HttpContext context)
        {            
            try
            {
                string thumbFilePath = BaseThumbnailFilePath;
                string filePath = BaseFilePath;
                
                Parameters RequestParameters = new Parameters();
                RequestParameters.FillParameters(context.Request);               
                                
                filePath = string.Format(filePath, RequestParameters.FileFolder, RequestParameters.ImageId, RequestParameters.FileExtension);
                thumbFilePath = string.Format(thumbFilePath, RequestParameters.FileFolder, RequestParameters.ImageId, RequestParameters.FileExtension);

                //If a thumbnail was requested, but the file is not a jpg, send the placeholder thumbnail
                if ((RequestParameters.WantThumb || RequestParameters.WantMini) && RequestParameters.FileExtension.ToLower() != "jpg")
                {
                    SendPlaceholderImage(context, RequestParameters);
                    return;                    
                }         
                //If a file was requested for download, but the file is not a jpg, send the file
                if (RequestParameters.WantDownload && RequestParameters.FileExtension.ToLower() != "jpg")
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("content-disposition", "attachment; filename=" + string.Format("{0}_{1}_{2}.{3}", RequestParameters.ContractNumber, RequestParameters.FileType, RequestParameters.ImageId, RequestParameters.FileExtension));
                    context.Response.WriteFile(filePath);
                    context.Response.Flush();
                    return;
                }
                //If a file was requested for display, but the file is not a jpg,
                //  determine the MimeType and write the contents of the file to the stream
                if (RequestParameters.FileExtension.ToLower() != "jpg")
                {
                    context.Response.Clear();
                    context.Response.ContentType = WebCommon.DetermineMimeType(RequestParameters.FileExtension);
                    context.Response.WriteFile(filePath);
                    context.Response.Flush();
                    return;
                }
                try
                {
                    //See if a physical-copy of the Thumbnail exists
                    //  if it does, send the physical image                    
                    if (RequestParameters.WantThumb && File.Exists(context.Server.MapPath(thumbFilePath)))
                    {
                        context.Response.ContentType = "image/jpeg";
                        context.Response.WriteFile(thumbFilePath);
                        context.Response.Flush();
                        return;
                    }
                    //If a physical copy of the image does not exist
                    //  send a placeholder
                    if (!File.Exists(context.Server.MapPath(filePath)))
                    {
                        SendPlaceholderImage(context, RequestParameters);
                        return;
                    }                    
                    
                    Image image = null;
                    string imagePath = context.Server.MapPath(filePath);                    

                    //The stream that the (resized) image is created on, must be left open before trying to save:
                    //  http://stackoverflow.com/questions/1053052/a-generic-error-occurred-in-gdi-jpeg-image-to-memorystream/1053123#1053123
                    using (StreamReader imageStream = new StreamReader(imagePath))
                    {
                        System.Drawing.Image originalImage = System.Drawing.Image.FromStream(imageStream.BaseStream);
                        image = DigitalLibraryImaging.GetImage(originalImage, RequestParameters.Width, RequestParameters.Height, RequestParameters.RequestedDpi, RequestParameters.WantOriginal, RequestParameters.WantFill, RequestParameters.BackgroundColor);

                        if (RequestParameters.WantWebImageCopy)
                        {
                            if (!File.Exists(string.Format(@"\\fsnyc01\public\Marketing\Apollo Web Images\{0}.jpg", RequestParameters.ImageId)))
                            {
                                image.Save(string.Format(@"\\fsnyc01\public\Marketing\Apollo Web Images\{0}.jpg", RequestParameters.ImageId));
                            }
                        }
                        else
                        {

                            if (RequestParameters.WantDownload)
                            {
                                context.Response.Clear();
                                context.Response.ContentType = "application/octet-stream";
                                context.Response.AddHeader("content-disposition", "attachment; filename=" + string.Format("{0}_Photo_{1}.jpg", RequestParameters.ContractNumber, RequestParameters.ImageId));
                            }
                            else
                            {
                                context.Response.ContentType = "image/jpeg";
                            }
                            //if a thumbnail was requested but didn't exist...create it
                            if (RequestParameters.WantThumb && !File.Exists(context.Server.MapPath(thumbFilePath)))
                            {
                                DigitalLibraryImaging.SaveThumbnailImage(RequestParameters.ImageId, image, context.Server.MapPath(DigitalLibraryImaging.ThumbnailImageFilePath + DigitalLibraryImaging.GetDigitalLibraryFileFolder(RequestParameters.ImageId)));
                            }
                            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            // Create an Encoder object based on the GUID
                            // for the Quality parameter category.
                            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 95L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            //image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                            image.Save(context.Response.OutputStream, jgpEncoder, myEncoderParameters);
                            image.Dispose();
                        }
                        context.Response.Flush();
                    }                    
                }                                    
                catch (InvalidOperationException)
                {
                    SendPlaceholderImage(context, RequestParameters);
                }                                                                                                                       
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The remote host closed the connection."))
                {
                    return;
                }
                WebCommon.LogExceptionInfo(ex);
                context.Response.ContentType = "text/plain";
                context.Response.Write("An error occurred while trying to retrieve the image.");
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
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