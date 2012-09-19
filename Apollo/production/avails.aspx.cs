#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class production_avails : System.Web.UI.Page
    {

        #region exportBookings_Click method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void exportBookings_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder excelHtml = new StringBuilder();
                excelHtml.Append("<html>");
                excelHtml.Append("<head>");
                excelHtml.Append(@"<style type=""text/css"">.datepicker, .button {}"
                + ".featureTable {font-size:10px !important;width:100%;/*white-space:nowrap;*/}"
                + ".featureTable tr {font-size:10px;}"
                + ".featureTable td.bookingCell {border:none !imporant;padding:none !important;cursor:pointer;}"
                + ".featureTable td.statusClass1, .featureTable td.statusClass2,"
                + ".featureTable td.statusClass3, .featureTable td.statusClass4,"
                + ".featureTable td.statusClass5 {border:none !imporant;padding:none !important;cursor:pointer;}"
                + ".featureTable td {border:1px solid #333333;text-align:left;font-size:10px;padding:1px;}"
                + ".featureTable th {border:1px solid #ffffff;text-align:center;padding:2px;font-size:10px;background-color:#000000;color:#ffffff;}"
                + ".bookingTableHeader td {border:1px solid #ffffff;text-align:center;padding:2px;font-size:10px;font-weight:bold;background-color:#000000;color:#ffffff;}"
                + ".requiredIndicator {color:#ff0000;font-weight:bold;font-style:italic;margin-left:5px;}"
                + ".hiddenButton {display:none;}"
                + ".statusClass1{background-color:#FFCC99;}"
                + ".statusClass2{background-color:#CCFFFF;}"
                + ".statusClass3{background-color:#FCF305;}"
                + ".statusClass4{background-color:#99CC00;}"
                + ".statusClass5{background-color:#DD0806;}"
                + ".statusClass6{background-color:#cccccc;}"
                + "#keyDisplay table {border:1px solid #333333;}"
                + "#keyDisplay table td {border:1px solid#333333;text-align:center;font-size:10px;padding:2px;}"
                + "</style>");
                excelHtml.Append("</head><body>");
                excelHtml.Append(@"<div id=""keyDisplay"" style=""margin:10px 0;display:none;""><table><thead><tr><th colspan='5'>Key</th></tr></thead><tbody>"
                    + @"<tr><td class=""statusClass1"">Specialty Media</td><td class=""statusClass2"">Under Construction</td>"
                    + @"<td class=""statusClass3"">Contracted</td><td class=""statusClass4"">Hold</td><td class=""statusClass5"">First Rights</td>"
                    + @"</tr><tr><td class=""statusClass6"" colspan=""5"">Winter Months<br />Cold Weather Restrictions may apply.</td></tr></tbody></table></div>");
                excelHtml.AppendFormat(exportData.Value);
                excelHtml.Append("</body></html>");
                WebCommon.ExportHtmlToExcel(string.Format("{0}_AvailsList", exportYear.Value), excelHtml.ToString());
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to export the Avails list.");
            }
        }
        #endregion

        #region GetUserAccessMarkets method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string GetUserAccessMarkets()
        {
            List<string> markets = new List<string>();
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomAdminGroup"]))
            {
                return "ALL";
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomAmtrakGroup"]))
            {
                markets.Add("Amtrak");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomBostonGroup"]))
            {
                markets.Add("Boston");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomCharlotteGroup"]))
            {
                markets.Add("Charlotte");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomChicagoGroup"]))
            {
                markets.Add("Chicago");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomDallasGroup"]))
            {
                markets.Add("Dallas");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomLosAngelesGroup"]))
            {
                markets.Add("Los Angeles");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomMinneapolisGroup"]))
            {
                markets.Add("Minneapolis");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomNewJerseyGroup"]))
            {
                markets.Add("New Jersey");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomNewYorkGroup"]))
            {
                markets.Add("New York");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomPhiladelphiaGroup"]))
            {
                markets.Add("Philadelphia");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomSanFranciscoGroup"]))
            {
                markets.Add("San Francisco");
            }
            if (TitanADService.CheckRoleForUser(Security.GetCurrentUserId.ToLower(), ConfigurationManager.AppSettings["stationDomSeattleGroup"]))
            {
                markets.Add("Seattle");
            }
            if (markets.Count == 0)
            {
                return string.Empty;
            }
            return String.Join(",", markets.ToArray());
        }
        #endregion

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string marketAccess = GetUserAccessMarkets();
                isEditUser.Value = (String.IsNullOrEmpty(marketAccess) ? "0" : "-1");
                editUserMarket.Value = marketAccess;
            }
        }
        #endregion

    }

}
