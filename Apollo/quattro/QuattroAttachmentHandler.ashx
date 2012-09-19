<%@ WebHandler Language="C#" Class="Apollo.QuattroAttachmentHandler" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Titan.DataIO;
namespace Apollo
{
    public class QuattroAttachmentHandler : IHttpHandler {
        
        public void ProcessRequest (HttpContext context) 
        {
            context.Response.Clear();
            context.Response.ContentType = "text/html";
            int contractNumber = -1;
            int companyId = -1;
            try
            {
                if (String.IsNullOrEmpty(context.Request["contractNumber"]) || String.IsNullOrEmpty(context.Request["companyId"]))
                {
                    context.Response.Write("Contract attachment details could not be retrieved.");
                    return;
                }
                if (!int.TryParse(Regex.Replace(context.Request["contractNumber"], @"[^\d]+", ""), out contractNumber))
                {
                    context.Response.Write("Contract attachment details could not be retrieved.");
                    return;
                }
                if (!int.TryParse(context.Request["companyId"], out companyId))
                {
                    context.Response.Write("Contract attachment details could not be retrieved.");
                    return;
                }
                if (contractNumber == -1 || companyId == -1)
                {
                    context.Response.Write("Contract attachment details could not be retrieved.");
                    return;
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                context.Response.Write("Contract attachment details could not be retrieved.");
                return;
            }
            StringBuilder attachmentTable = new StringBuilder();
            try
            {
                DataSet attachmentData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    attachmentData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Quattro_GetContractAttachments",
                        Param.CreateParam("CONTRACTNUMBER", SqlDbType.Int, contractNumber),
                        Param.CreateParam("COMPANYID", SqlDbType.Int, companyId)));
                }
                if (attachmentData.Tables[0].Rows.Count == 0)
                {
                    context.Response.Write(string.Format("No attachments exist for Contract #: ", contractNumber));
                    return;
                }
                attachmentTable.Append(@"<table class=""attachmentDetailTable"" cellspacing=""10""><tr>");
                attachmentTable.AppendFormat(@"<th colspan=""5"">Contract: {0} Attachment Info.</th></tr>", Convert.ToString(contractNumber));
                attachmentTable.Append(@"<th style=""width:20%;text-align:center;border-bottom:1px solid #999999;"" nowrap>Attachment Title</th>");
                attachmentTable.Append(@"<th style=""width:20%;text-align:center;border-bottom:1px solid #999999;"" nowrap>Attachment Type</th>");
                attachmentTable.Append(@"<th style=""width:20%;text-align:center;border-bottom:1px solid #999999;"" nowrap>Attachment Date</th>");
                attachmentTable.Append(@"<th style=""width:40%;text-align:center;border-bottom:1px solid #999999;"" nowrap>Attachment Description</th>");
                attachmentTable.Append("<th>&nbsp;</th></tr>");
                int count = 0;
                string cellBorderStyle = "";
                foreach (DataRow row in attachmentData.Tables[0].Rows)
                {
                    attachmentTable.Append("<tr>");
                    attachmentTable.AppendFormat(@"<td style=""width:20%;text-align:left;{1};white-space:nowrap"">{0}</td>", IO.GetDataRowValue(row, "ATTACHMENT_TITLE", "&nbsp;"), ((count != 0) ? cellBorderStyle : string.Empty));
                    attachmentTable.AppendFormat(@"<td style=""width:20%;text-align:center;{1};white-space:nowrap"">{0}</td>", IO.GetDataRowValue(row, "ATTACHMENT_TYPE", "&nbsp;"), ((count != 0) ? cellBorderStyle : string.Empty));
                    attachmentTable.AppendFormat(@"<td style=""width:20%;text-align:center;{1};white-space:nowrap"">{0}</td>", IO.GetDataRowValue(row, "ATTACHMENT_DATE", "&nbsp;"), ((count != 0) ? cellBorderStyle : string.Empty));
                    attachmentTable.AppendFormat(@"<td style=""width:40%;text-align:left;{1}"">{0}</td>", IO.GetDataRowValue(row, "ATTACHMENT_DESC", "&nbsp;"), ((count != 0) ? cellBorderStyle : string.Empty));
                    attachmentTable.AppendFormat(@"<td style=""text-align:center;{2}""><a href=""/quattro/quattro_attachments{3}/{0}.{1}"" alt=""View Attachment"" target=""_blank"">View</a></td>", row["ATTACHMENT_ID"], row["ATTACHMENT_EXT"], ((count != 0) ? cellBorderStyle : string.Empty), (companyId == 1 ? "" : "_toc"));
                    attachmentTable.Append("</tr>");
                    count++;
                }
                attachmentTable.Append("</table>");
                context.Response.Write(attachmentTable.ToString());
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                context.Response.Write("Contract attachment details could not be retrieved.");
                return;
            }                                    
        }
     
        public bool IsReusable {
            get {
                return false;
            }
        }

    }
}