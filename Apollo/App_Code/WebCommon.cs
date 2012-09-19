#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>Class to define commonly used methods</summary>
    public abstract class WebCommon
    {

        #region WebEnvironment enumeration
        public enum WebEnvironment
        {
            LocalDev,
            Dev,
            QA,
            Prod,
        }
        #endregion

        #region Member variables
        /// <summary>The Default style to apply to Infragistics controls</summary>
        //private const string DEFAULT_STYLE = "Office2007Black";
        private const string DEFAULT_STYLE = "Claymation";
        /// <summary>End of Line constant</summary>
        private const string EOL = "\r\n";
        #endregion

        #region BuildExceptionMessage method
        /// <summary>static method to build exception message</summary>
        /// <param name="ex">Exception object</param>
        /// <returns>Exception message in string format</returns>
        private static string BuildExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(DateTime.Now.ToString());
            sb.Append("] ");
            Exception original = ex;
            while (ex != null)
            {
                sb.Append(ex.GetType().FullName);
                sb.Append(": ");
                sb.AppendLine(ex.Message);
                ex = ex.InnerException;
            }
            sb.AppendLine(original.StackTrace);
            return sb.ToString();
        }
        #endregion

        #region ConnectionString property
        /// <summary>Encapsulates the connection strings contained in the web.config</summary>
        /// <value>Returns the connection string for the current environment</value>
        public static string ConnectionString
        {
            get
            {
                //return ConfigurationManager.ConnectionStrings["localDev"].ToString();
                switch (DetermineEnvironment())
                {
                    case WebEnvironment.LocalDev:
                    case WebEnvironment.Dev:
                        return ConfigurationManager.ConnectionStrings["devConnString"].ToString();
                    case WebEnvironment.QA:
                        return ConfigurationManager.ConnectionStrings["stageConnString"].ToString();
                    default:
                        return ConfigurationManager.ConnectionStrings["prodConnString"].ToString();
                }
            }
        }
        #endregion

        #region DataTableToHtmlTable method
        /// <summary>TBD</summary>
        /// <param name="table">TBD</param>
        /// <param name="name">TBD</param>
        /// <returns>TBD</returns>
        public static string DataTableToHtmlTable(DataTable table, string name)
        {
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append(@"<table border=""1"">");
            htmlTable.AppendFormat(@"<tr><td colspan=""{0}"" class=""titleRow"">{1}</td></tr>", table.Columns.Count, name);
            htmlTable.Append("<tr>");
            foreach (DataColumn column in table.Columns)
            {
                htmlTable.AppendFormat("<th>{0}</th>", column.ColumnName);
            }
            htmlTable.Append("</tr>");
            foreach (DataRow row in table.Rows)
            {
                htmlTable.Append("<tr>");
                foreach (DataColumn column in table.Columns)
                {
                    htmlTable.AppendFormat("<td>{0}</td>", row[column.ColumnName]);
                }
                htmlTable.Append("</tr>");
            }
            htmlTable.AppendLine("</table>");
            return htmlTable.ToString();
        }
        #endregion

        #region DeleteFile method
        /// <summary>Deletes the file at the specified path if it exists</summary>
        /// <param name="path">The path to the file to be deleted</param>
        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception("An attempt to delete a file failed.", ex));
            }
        }
        #endregion

        public static string SmtpAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["titanSmtpAddress"].ToString();
            }
        }

        #region DetermineEnvironment method
        /// <summary>Determine which web environment the application is running in</summary>
        /// <returns>A WebEnvironment enumeration value of the current environment</returns>
        public static WebEnvironment DetermineEnvironment()
        {
            string currentMachine = System.Environment.MachineName;
            if (currentMachine.IndexOf("WEBSQL") != -1 || currentMachine.IndexOf("WEBSTAGE") != -1)
            {
                return WebEnvironment.QA;
            }
            else if (currentMachine.IndexOf("WEBPROD") != -1)
            {
                return WebEnvironment.Prod;
            }
            else
            {
                return WebEnvironment.Dev;
            }
        }
        #endregion

        #region DetermineFileType method
        /// <summary>Converts the file type abbreviation to a user-readable string</summary>
        /// <param name="typeAbbreviation">The file-type abbreviation</param>
        /// <returns>User-readable file-type</returns>
        public static string DetermineFileType(string typeAbbreviation)
        {
            switch (typeAbbreviation.ToUpper())
            {
                case "I":
                    return "Photo";
                case "C":
                    return "Contract";
                case "R":
                    return "Completion Report";
                case "P":
                    return "Copy Receipt";
                case "M":
                    return "Manual Invoice";
                default:
                    return "Photo";
            }
        }
        #endregion

        #region DetermineMimeType method
        /// <summary>Determines the file mime-type based on file extension</summary>
        /// <param name="fileExtension">The file extension</param>
        /// <returns>The HTML mime-type for the file extension</returns>
        public static string DetermineMimeType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case "doc":
                    return "application/msword";
                case "flv":
                case "swf":
                    return "application/x-shockwave-flash";
                case "htm":
                case "html":
                    return "text/html";
                case "jpg":
                    return "image/jpeg";
                case "pdf":
                    return "application/pdf";
                case "xls":
                    return "application/excel";
                case "txt":
                default:
                    return "text/plain";
            }
        }
        #endregion

        #region ExportHtmlToExcel method
        /// <summary>Streams an HTML-formatted string to the content stream</summary>
        /// <param name="fileNamePrefix">TBD</param>
        /// <param name="html">HTML-formatted string</param>
        public static void ExportHtmlToExcel(string fileNamePrefix, string html)
        {
            string extension = "xls";
            string fileName = string.Format("{0}_{1}{2}{3}{4}{5}{6}.{7}", fileNamePrefix, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.Write(html);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region GetPanelTypeTable method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static DataTable GetPanelTypeTable()
        {
            DataTable digitalLibrary = new DataTable("digitalLibrary");
            digitalLibrary.Columns.Add(new DataColumn("PANEL_TYPE"));
            return digitalLibrary;
        }
        #endregion

        #region GetSessionValue method (key)
        /// <summary>Retrieves an object stored in Session state</summary>
        /// <param name="key">The session object to retrieve</param>
        /// <returns>The object retrieved from Session state</returns>
        public static object GetSessionValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return HttpContext.Current.Session[key];
        }
        #endregion

        #region GetSessionValue method (key, defaultValue)
        /// <summary>TBD</summary>
        /// <param name="key">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        public static object GetSessionValue(string key, object defaultValue)
        {
            if (key == null)
            {
                WebCommon.SetSessionState(key, defaultValue);
            }
            return HttpContext.Current.Session[key];
        }
        #endregion        

        #region GetUserSearchParamsTable method
        /// <summary>Creates a DataTable with columns used to save User Search Params</summary>
        /// <returns>DataTable with User Search Param columns</returns>
        public static DataTable GetUserSearchParamsTable()
        {
            DataTable searchParams = new DataTable("searchParams");
            searchParams.Columns.Add(new DataColumn("USER_ID"));
            searchParams.Columns.Add(new DataColumn("SCREEN"));
            searchParams.Columns.Add(new DataColumn("CONTROL"));
            searchParams.Columns.Add(new DataColumn("VALUE"));
            return searchParams;
        }
        #endregion

        #region InTestingEnvironment method
        /// <summary>Method to return true/false for the Testing envrionment</summary>
        /// <returns>true/false</returns>
        public static bool InTestingEnvironment()
        {
            //Determine whether or not application is running in a Dev, Test, or QA environment
            string currentServer = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            return (currentServer.Contains("localhost") || currentServer.Contains("dv.") || currentServer.Contains("qa.") || currentServer.Contains("stage."));
        }
        #endregion

        public static string DigitalAvailsConnectionString
        {
            get
            {
                switch (DetermineEnvironment())
                {
                    case WebEnvironment.LocalDev:
                    case WebEnvironment.Dev:
                        return ConfigurationManager.ConnectionStrings["devDigitalAvailsConnString"].ToString();
                    case WebEnvironment.QA:
                        return ConfigurationManager.ConnectionStrings["stageDigitalAvailsConnString"].ToString();
                    default:
                        return ConfigurationManager.ConnectionStrings["prodDigitalAvailsConnString"].ToString();
                }
            }
        }

        public static string PendingConnectionString
        {
            get
            {
                switch (DetermineEnvironment())
                {
                    case WebEnvironment.LocalDev:
                    case WebEnvironment.Dev:
                        return ConfigurationManager.ConnectionStrings["devPendingConnString"].ToString();
                    case WebEnvironment.QA:
                        return ConfigurationManager.ConnectionStrings["stagePendingConnString"].ToString();
                    default:
                        return ConfigurationManager.ConnectionStrings["prodPendingConnString"].ToString();
                }
            }
        }

        #region CRMConnectionString property
        /// <summary>Encapsulates the connection strings contained in the web.config</summary>
        /// <value>Returns the connection string for the current environment</value>
        public static string CRMConnectionString
        {
            get
            {
                switch (DetermineEnvironment())
                {
                    case WebEnvironment.LocalDev:
                    case WebEnvironment.Dev:
                        return ConfigurationManager.ConnectionStrings["devCrmConnString"].ToString();
                    case WebEnvironment.QA:
                        return ConfigurationManager.ConnectionStrings["stageCrmConnString"].ToString();
                    default:
                        return ConfigurationManager.ConnectionStrings["prodCrmConnString"].ToString();
                }
            }
        }
        #endregion

        #region TorontoConnectionString property
        /// <summary>Encapsulates the connection strings contained in the web.config</summary>
        /// <value>Returns the connection string for the current environment</value>
        public static string TorontoConnectionString
        {
            get
            {
                switch (DetermineEnvironment())
                {
                    case WebEnvironment.LocalDev:
                    case WebEnvironment.Dev:
                        return ConfigurationManager.ConnectionStrings["devTorontoConnString"].ToString();
                    case WebEnvironment.QA:
                        return ConfigurationManager.ConnectionStrings["stageTorontoConnString"].ToString();
                    default:
                        return ConfigurationManager.ConnectionStrings["prodTorontoConnString"].ToString();
                }
            }
        }
        #endregion

        #region KPIConnectionString property
        /// <summary>Encapsulates the connection strings contained in the web.config</summary>
        /// <value>Returns the connection string for the current environment</value>
        public static string KPIConnectionString
        {
            get
            {
                switch (DetermineEnvironment())
                {
                    case WebEnvironment.LocalDev:
                    case WebEnvironment.Dev:
                        return ConfigurationManager.ConnectionStrings["devKPIConnString"].ToString();
                    case WebEnvironment.QA:
                        return ConfigurationManager.ConnectionStrings["stageKPIConnString"].ToString();
                    default:
                        return ConfigurationManager.ConnectionStrings["prodKPIConnString"].ToString();
                }
            }
        }
        #endregion

        #region GetGridData method
        /// <summary>
        ///     Builds an Xml-style string to pass to the jQuery-Grid
        /// </summary>
        /// <param name="data">The DataTable containing the display data</param>
        /// <param name="filter">A dynamic filter used to filter the DataTable</param>
        /// <param name="page">The page to display</param>
        /// <param name="rowsPerPage">How many rows to display in a single page</param>
        /// <returns>Xml-formatted string</returns>
        public static string GetGridData(DataTable data, string filter, int page, int rowsPerPage)
        {
            StringBuilder xml = new StringBuilder();
            DataRow[] filteredRows = data.Select(filter);
            int totalPages = (filteredRows.Length == 0) ? 0 : Convert.ToInt32(Math.Ceiling((double)filteredRows.Length / rowsPerPage));
            int startRecord = (page * rowsPerPage) - rowsPerPage;
            int stopRecord = Convert.ToInt32(Math.Min((startRecord + rowsPerPage), filteredRows.Length));
            int i;
            int j = 0;
            string currentContractNumber = string.Empty, previousContractNumber = string.Empty;
            int rowCount = 0;
            xml.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
            xml.AppendLine(@"<rows>");
            xml.AppendFormat(@"<page>{0}</page>", page);
            xml.AppendFormat(@"<total>{0}</total>", totalPages);
            if (filteredRows.Length != 0)
            {
                for (i = startRecord; i < stopRecord; i++)
                {
                    xml.AppendFormat(@"<row id='{0}'>", i);
                    foreach (DataColumn column in data.Columns)
                    {
                        xml.AppendFormat(@"<cell><![CDATA[{0}]]></cell>", Convert.ToString(filteredRows[i][j++]));
                    }
                    xml.AppendLine("</row>");
                    rowCount = filteredRows.Length;
                    j = 0;
                }
            }
            xml.AppendFormat(@"<records>{0}</records>", rowCount);
            xml.AppendLine(@"</rows>");
            return xml.ToString();
        }
        #endregion

        #region LogExceptionInfo method
        /// <summary>Captures an Exception that occurs in the application and logs it in the Database</summary>
        /// <param name="ex">The exception caught by an external method</param>
        public static void LogExceptionInfo(Exception ex)
        {
            string userId;
            try
            {
                userId = Security.GetCurrentUserId;
            }
            catch
            {
                userId = string.Empty;
            }            
            try
            {
                string exceptionInfo = string.Format("{0}: {1}", userId, ex.ToString());
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("ADDEXCEPTION",
                        Param.CreateParam("exception_info", SqlDbType.NText, exceptionInfo)));
                }
            }
            catch
            {
                HttpContext.Current.Server.Transfer("Default.aspx?error=critical");
            }
        }
        #endregion

        #region MailAddress property
        /// <summary>The mailing 'To' address used by the web app to send emails.</summary>
        /// <value>A valid Titan email address</value>
        public string MailAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["mailingName"].ToString();
            }
        }
        #endregion

        #region MaximumFlatRatePercentage property
        /// <summary>Returns the maximum allowable flat rate percentage allowed.</summary>
        /// <value>The maximum flat rate percentage allowed by the system</value>
        public static int MaximumFlatRatePercentage
        {
            //maxFlatRatePercentage
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["maxFlatRatePercentage"].ToString());
            }
        }
        #endregion

        #region MaximumRatePercentage property
        /// <summary>Returns the maximum allowable rate percentage allowed.</summary>
        /// <value>The maximum rate percentage allowed by the system</value>
        public static int MaximumRatePercentage
        {
            //maxRatePercentage
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["maxRatePercentage"].ToString());
            }
        }
        #endregion

        #region SaveFile method
        /// <summary>TBD</summary>
        /// <param name="file">TBD</param>
        /// <param name="fileId">TBD</param>
        /// <param name="filePath">TBD</param>
        /// <param name="fileExtension">TBD</param>
        public static void SaveFile(HttpPostedFile file, int fileId, string filePath, string fileExtension)
        {
            string path = "{0}/{1}.{2}";
            //Make sure the folder exists
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            file.SaveAs(string.Format(path, filePath, fileId, fileExtension));
        }
        #endregion

        public static void SaveFileFromStream(Stream file, int fileId, string filePath, string fileExtension)
        {
            byte[] fileBytes = new byte[Convert.ToInt32(file.Length)];
            file.Read(fileBytes, 0, Convert.ToInt32(file.Length));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (FileStream digitalLibrarySaveLocation = File.Open(string.Format("{0}/{1}.{2}", filePath, fileId, fileExtension), FileMode.Create, FileAccess.Write))
            {
                digitalLibrarySaveLocation.Write(fileBytes, 0, fileBytes.Length);
            }
        }

        #region SetSessionState method
        /// <summary>Stores an object in the current Session state</summary>
        /// <param name="key">The key to store the desired object under</param>
        /// <param name="theValue">The object to store</param>
        public static void SetSessionState(string key, object theValue)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (theValue == null)
            {
                throw new ArgumentNullException("theValue");
            }
            HttpContext.Current.Session[key] = theValue;
        }
        #endregion

        #region ShowAlert method
        /// <summary>Registers a JavaScript alert function in the current page.</summary>
        /// <param name="msg">The message to be displayed/alerted to the user.</param>
        public static void ShowAlert(string msg)
        {
            string friendlyMsg = msg.Replace("'", "\\'");
            string js = string.Format(@"<script type='text/javascript'>alert('{0}');</script>", friendlyMsg);
            Page page = HttpContext.Current.CurrentHandler as Page;
            if ((page != null) && (!page.ClientScript.IsClientScriptBlockRegistered("alert")))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Page), "alert", js);
            }
        }
        #endregion

        #region StyleSetName property
        /// <summary>Gets/Sets the StyleSet name to be used by Infragistics controls</summary>
        /// <value>String value containing the StyleSet name</value>
        public static string StyleSetName
        {
            get
            {
                if (!String.IsNullOrEmpty((string)GetSessionValue("styleSetName")))
                {
                    return (string)GetSessionValue("styleSetName");
                }
                return DEFAULT_STYLE;
            }
            set
            {
                SetSessionState("styleSetName", value);
            }
        }
        #endregion

        #region WriteDebugMessage method
        /// <summary>TBD</summary>
        /// <param name="debugMessage">TBD</param>
        public static void WriteDebugMessage(string debugMessage)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("ADDDEBUGMSG",
                        Param.CreateParam("DEBUGMSG", SqlDbType.NText, debugMessage)));
                }
            }
            catch (Exception ex)
            {
                LogExceptionInfo(ex);
            }
        }
        #endregion

        public static string Enquote(string s)
        {
            if (s == null || s.Length == 0)
            {
                //return "\"\"";
                return string.Empty;
            }
            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            //sb.Append('"');
            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>'))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else
                {
                    if (c < ' ')
                    {
                        //t = "000" + Integer.toHexString(c); 
                        string tmp = new string(c, 1);
                        t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            //sb.Append('"');
            return sb.ToString();
        } 

    }

}
