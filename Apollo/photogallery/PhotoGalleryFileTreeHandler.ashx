<%@ WebHandler Language="C#" Class="Apollo.PhotoGalleryFileTreeHandler" %>

using System;
using System.Web;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using Titan.DataIO;

namespace Apollo
{
    public class PhotoGalleryFileTreeHandler : IHttpHandler
    {
        //public const string BASE_PATH = @"\\fsnyc03\PhotoGallery\g2data\albums\";
        public const string BASE_PATH = "\\";
        
        public void ProcessRequest(HttpContext context)
        {
            HttpCachePolicy cachePolicy = context.Response.Cache;
            cachePolicy.SetCacheability(HttpCacheability.Public);
            //cachePolicy.VaryByParams["dir"] = true;
            cachePolicy.SetOmitVaryStar(true);
            cachePolicy.SetExpires(DateTime.Now + TimeSpan.FromDays(60));
            //cachePolicy.SetExpires(DateTime.Now.AddDays(-1));
            cachePolicy.SetValidUntilExpires(true);
            context.Response.ContentType = "text/html";
            string directory = context.Request["dir"] ?? BASE_PATH;
            if (directory == string.Empty)
            {
                directory = BASE_PATH;
            }
            try
            {
                context.Response.Write(GetDirectoryDetails(HttpUtility.UrlDecode(directory)));
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                context.Response.Write("An error occurred.");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string GetDirectoryDetails(string directory)
        {
            DataSet fileInfo;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                fileInfo = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("PHOTOGALLERY_RETRIEVEITEMLIST", Param.CreateParam("PARENTPATH", SqlDbType.VarChar, directory)));
            }
            StringBuilder directoryDetails = new StringBuilder();            
            //DirectoryInfo directoryInfo = new DirectoryInfo(directory);                       
            directoryDetails.Append(@"<ul class=""jqueryFileTree"" style=""display:none;"">");

            //List each Subdirectory
            string folderPath = "";
            string extension = string.Empty;
            foreach (DataRow row in fileInfo.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["IS_FOLDER"]) == -1)
                {
                    //folderPath = string.Format("{0}\\{1}\\", row["PATH"], row["NAME"]);                    
                    folderPath = string.Format("{0}{1}\\", row["PATH"], row["NAME"]);
                    directoryDetails.AppendFormat(@"<li class=""directory collapsed {2}""><a href=""#"" rel=""{1}"" onclick=""ShowFolderContents('{3}','{2}');"">{0}</a></li>", row["NAME"], folderPath, (Convert.ToInt32(row["IS_PARENT_FOLDER"]) == 0 ? "containerFolder" : "parentFolder"), HttpUtility.HtmlEncode(folderPath).Replace("\\", "\\\\"));
                }
                else
                {
                    folderPath = Convert.ToString(row["PATH"]);
                    extension = Convert.ToString(row["NAME"]).Substring(Convert.ToString(row["NAME"]).LastIndexOf('.') + 1);
                    if (String.Compare(extension, "JPG", true) == 0)
                    {
                        directoryDetails.AppendFormat(@"<li class=""file ext_{0}""><a href=""#"" rel=""{2}\\{1}"">{1}</a></li>", extension.ToLower(), row["NAME"], folderPath.Replace("\\", "\\\\"));
                    }
                }                
            }
            directoryDetails.Append("</ul>");            
            return directoryDetails.ToString();
        }

        private string GetStartDirectory(string userId)
        {
            return string.Empty;
        }

    }
}