<%@ WebHandler Language="C#" Class="Apollo.DigitalLibraryToolTipHandler" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using Titan.DataIO;
namespace Apollo 
{
    public class DigitalLibraryToolTipHandler : IHttpHandler {
        
        public void ProcessRequest (HttpContext context) 
        {
            context.Response.Clear();
            context.Response.ContentType = "text/html";
            if (String.IsNullOrEmpty(context.Request["fileId"]))
            {
                context.Response.Write("Document details could not be retrieved");
                return;
            }
            int fileId = 0;
            if (!int.TryParse(context.Request["fileId"],out fileId))
            {
                context.Response.Write("Document details could not be retrieved");
                return;
            }
            if (fileId == 0)
            {
                context.Response.Write("Document details could not be retrieved");
                return;
            }
            StringBuilder toolTip = new StringBuilder();
            string popupDetails = string.Empty;
            string jsHashEntry = @"{0}|{1}";
            try
            {
                DataSet taggingData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    taggingData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALLIBRARY_GETTAGDETAIL", Param.CreateParam("FILEID", SqlDbType.Int, fileId)));
                }
                DataRow documentProperties = null;
                DataRow taggingProperties = null;
                if (taggingData == null || taggingData.Tables.Count == 0)
                {
                    context.Response.Write("Document details could not be retrieved");
                    return;
                }
                if (taggingData.Tables[0].Rows.Count >= 1)
                {
                    documentProperties = taggingData.Tables[0].Rows[0];
                }
                if (taggingData.Tables.Count > 1 && taggingData.Tables[1].Rows.Count >= 1)
                {
                    taggingProperties = taggingData.Tables[1].Rows[0];
                }
                popupDetails = string.Format(@"{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24}",
                    string.Format(jsHashEntry, "Contract #", IO.GetDataRowValue(taggingProperties, "CONTRACT_NUMBER", "")),
                    string.Format(jsHashEntry, "Document Type", WebCommon.DetermineFileType((string)IO.GetDataRowValue(documentProperties, "FILE_TYPE", "I"))),
                    string.Format(jsHashEntry, "Country", ((Convert.ToString(IO.GetDataRowValue(taggingProperties, "COMPANY_ID", "")) == "1") ? "Titan Outdoor (US)" : (Convert.ToString(IO.GetDataRowValue(taggingProperties, "COMPANY_ID", "")) == "2") ? "Titan Outdoor (Canada)" : string.Empty)),
                    string.Format(jsHashEntry, "AE 1", (string)IO.GetDataRowValue(taggingProperties, "AE_1_NAME", "")),
                    string.Format(jsHashEntry, "AE 2", (string)IO.GetDataRowValue(taggingProperties, "AE_2_NAME", "")),
                    string.Format(jsHashEntry, "AE 3", (string)IO.GetDataRowValue(taggingProperties, "AE_3_NAME", "")),
                    string.Format(jsHashEntry, "Agency", (string)IO.GetDataRowValue(taggingProperties, "AGENCY", "")),
                    string.Format(jsHashEntry, "Advertiser", (string)IO.GetDataRowValue(taggingProperties, "ADVERTISER", "")),
                    string.Format(jsHashEntry, "Hero Photo", (((string)IO.GetDataRowValue(documentProperties, "IS_HERO_QUALITY", "N") == "Y") ? "Yes" : "No")),
                    string.Format(jsHashEntry, "Marketing Photo", (((string)IO.GetDataRowValue(documentProperties, "IS_MARKETING_QUALITY", "N") == "Y") ? "Yes" : "No")),
                    string.Format(jsHashEntry, "Photo Taken By", (string)IO.GetDataRowValue(documentProperties, "TAKEN_BY", "")),
                    string.Format(jsHashEntry, "Market", (string)IO.GetDataRowValue(taggingProperties, "MARKET", "")),
                    string.Format(jsHashEntry, "Profit Center", (string)IO.GetDataRowValue(taggingProperties, "PROFIT_CENTER", "")),
                    string.Format(jsHashEntry, "Station", (string)IO.GetDataRowValue(documentProperties, "STATION_NAME", "")),
                    string.Format(jsHashEntry, "Line Message", (string)IO.GetDataRowValue(taggingProperties, "LINE_MESSAGE", "")),
                    string.Format(jsHashEntry, "Media Type", (string)IO.GetDataRowValue(taggingProperties, "MEDIA_TYPE", "")),
                    string.Format(jsHashEntry, "Media Form", (string)IO.GetDataRowValue(taggingProperties, "MEDIA_FORM", "")),
                    string.Format(jsHashEntry, "Quantity", Convert.ToString(IO.GetDataRowValue(taggingProperties, "QUANTITY", ""))),
                    string.Format(jsHashEntry, "Start Date", Convert.ToString(IO.GetDataRowValue(taggingProperties, "START_DATE", ""))),
                    string.Format(jsHashEntry, "End Date", Convert.ToString(IO.GetDataRowValue(taggingProperties, "END_DATE", ""))),
                    string.Format(jsHashEntry, "Notes", (string)IO.GetDataRowValue(documentProperties, "NOTES", "")),
                    string.Format(jsHashEntry, "Image ID", IO.GetDataRowValue(documentProperties, "ID", "")),
                    string.Format(jsHashEntry, "Uploaded By", Security.GetFullUserNameFromId((string)IO.GetDataRowValue(documentProperties, "UPLOADED_BY", ""))),
                    string.Format(jsHashEntry, "Original Name", (string)IO.GetDataRowValue(documentProperties, "ORIGINAL_NAME", "")),
                    string.Format(jsHashEntry, "Uploaded At", IO.GetDataRowValue(documentProperties, "DATE_UPLOADED", "")),
                    string.Format(jsHashEntry, "File Size", IO.GetDataRowValue(documentProperties, "FILE_SIZE", "")),
                    string.Format(jsHashEntry, "Status", (((string)IO.GetDataRowValue(documentProperties, "IS_DELETED", "") == "N") ? "Active" : "Inactive")),
                    string.Format(jsHashEntry, "Tagged", IO.GetDataRowValue(documentProperties, "IS_TAGGED", ""))
                    //string.Format(jsHashEntry, "PanelCode", IO.GetDataRowValue(documentProperties, "PANEL_CODE_OVERRIDE", ""))
                );

                toolTip.Append(@"<div class='thumb-popup'><table style='width:296px'>");
                toolTip.Append(@"<tr><td colspan='5' class='panel-title'>Tag Information</td></tr>");                
                foreach (string property in popupDetails.Split(';'))
                {
                    if (String.Compare(property.Split('|')[0], "image id", true) == 0)
                    {
                        toolTip.Append(@"<tr><td colspan='5' class='panel-title'>Document Properties</td></tr>");
                    }
                    if (!String.IsNullOrEmpty(property.Split('|')[1]))
                    {
                        toolTip.AppendFormat(@"<tr><td nowrap>{0}</td><td colspan='4'>{1}</td></tr>", property.Split('|')[0], HttpUtility.HtmlEncode(property.Split('|')[1]));
                    }
                }
                toolTip.Append(@"</table></div>");
                context.Response.Write(toolTip.ToString());
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("Document details could not be retrieved for File: {0}.", fileId), ex));
                context.Response.Write(string.Format("Document details could not be retrieved for File: {0}.", fileId));
            }
        }

        private string GetOutputRow(DataRow data, string columnName, string displayName, string defaultValue)
        {
            if (!String.IsNullOrEmpty((string)IO.GetDataRowValue(data, columnName, defaultValue)))
            {
                return string.Format(@"<tr><td nowrap>{0}</td><td colspan='4'>{1}</td></tr>", displayName, (string)IO.GetDataRowValue(data, columnName, defaultValue));
            }
            return string.Empty;
        }
     
        public bool IsReusable {
            get {
                return false;
            }
        }

    }
}