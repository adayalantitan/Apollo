using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class digitalAvails_Default : System.Web.UI.Page
    {
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isDigitalAvailsAdmin.Value = (Security.IsDigitalAvailsAdminUser() ? "-1" : "0");
                isAtlantaEditor.Value = (Security.IsDigitalAvailsAtlantaUser() ? "-1" : "0");
                isChicagoEditor.Value = (Security.IsDigitalAvailsChicagoUser() ? "-1" : "0");
                isNewJerseyEditor.Value = (Security.IsDigitalAvailsNewJerseyUser() ? "-1" : "0");
                isPhiladelphiaEditor.Value = (Security.IsDigitalAvailsPhiladelphiaUser() ? "-1" : "0");
                isNewYorkEditor.Value = (Security.IsDigitalAvailsNewYorkUser() ? "-1" : "0");
                isTorontoEditor.Value = (Security.IsDigitalAvailsTorontoUser() ? "-1" : "0");
            }
        }

        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void cmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                string market = this.marketName.Value;
                string station = this.stationName.Value;
                int startMonth = Convert.ToInt32(this.startMonth.Value);
                int startYear = Convert.ToInt32(this.startYear.Value);
                int numberOfMonths = Convert.ToInt32(this.numberOfMonths.Value);
                int stationId = Convert.ToInt32(this.reportStationId.Value);
                DigitalAvails.DigitalAvailsReport stationReport = DigitalAvails.DigitalAvailsReport.GenerateStationReport(market, startMonth, startYear, numberOfMonths, stationId, true);
                XmlDocument xmlDoc = stationReport.DigitalAvailsReportWorkbook.GenerateExcelXml();
                if (xmlDoc != null)
                {
                    string extension = "xls";
                    string fileName = string.Format("{0}_DigitalAvails_{1}{2}{3}{4}.{5}", station.Replace(" ", "_"), DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Millisecond, extension);
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Save(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aePendingAlert", string.Format(@"alert(""No Data exists for {0}. Pending Report cannot be generated."");", Security.GetFullUserNameFromId(aeId)), true);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occured while trying to export the Market's Avails list.");
            }
        }
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void cmdExportMarket_Click(object sender, EventArgs e)
        {
            try
            {
                string market = this.marketName.Value;
                int startMonth = Convert.ToInt32(this.startMonth.Value);
                int startYear = Convert.ToInt32(this.startYear.Value);
                int numberOfMonths = Convert.ToInt32(this.numberOfMonths.Value);
                DigitalAvails.DigitalAvailsReport marketReport = DigitalAvails.DigitalAvailsReport.GenerateMarketReport(market, startMonth, startYear, numberOfMonths);
                XmlDocument xmlDoc = marketReport.DigitalAvailsReportWorkbook.GenerateExcelXml();
                if (xmlDoc != null)
                {
                    string extension = "xls";
                    string fileName = string.Format("{0}_DigitalAvails_{1}{2}{3}{4}.{5}", market.Replace(" ", "_"), DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Millisecond, extension);
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Save(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "aePendingAlert", string.Format(@"alert(""No Data exists for {0}. Pending Report cannot be generated."");", Security.GetFullUserNameFromId(aeId)), true);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occured while trying to export the Market's Avails list.");
            }
        }
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void cmdMarketUtilizationReport_Click(object sender, EventArgs e)
        {
            string reportMarket = this.marketUtilizationMarket.Value;
            int reportYear = Convert.ToInt32(this.marketUtilizationYear.Value);
            bool includeGB = (this.marketUtilizationIncludeGB.Value != "0");
            bool includeSAB = (this.marketUtilizationIncludeSAB.Value != "0");
            DigitalAvails.MarketUtilizationReport report = new DigitalAvails.MarketUtilizationReport(reportMarket, reportYear, includeGB, includeSAB);
            XmlDocument xmlDoc = report.ReportWorkbook.GenerateExcelXml();
            if (xmlDoc != null)
            {
                string extension = "xls";
                string fileName = string.Format("{0}_MarketUtilization_{1}{2}{3}{4}.{5}", reportMarket, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Millisecond, extension);
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Save(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
}
}