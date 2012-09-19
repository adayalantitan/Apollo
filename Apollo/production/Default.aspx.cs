using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;

namespace Apollo
{
    public partial class production_Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string market;
                bool isEditUserBool = IsEditUser(out market);
                isEditUser.Value = (isEditUserBool ? "-1" : "0");
                editUserMarket.Value = market;
            }
        }

        private bool IsEditUser(out string market)
        {
            Hashtable editUsers = new Hashtable();
            editUsers.Add("mrahming", "");
            editUsers.Add("wyan", "");
            editUsers.Add("mcrawford", "");
            editUsers.Add("agottselig", "");
            editUsers.Add("epenafiel", "Amtrak");
            editUsers.Add("eherri", "Chicago");
            editUsers.Add("lmazzeo", "New Jersey");
            editUsers.Add("mcosta", "New Jersey");
            editUsers.Add("jsanabria", "");
            editUsers.Add("melman", "Boston");
            editUsers.Add("rcoggins", "Philadelphia");
            editUsers.Add("lditsworth", "Chicago");
            editUsers.Add("kpiscatelli", "Chicago");
            editUsers.Add("areid", "Seattle");
            editUsers.Add("slu", "Seattle");
            editUsers.Add("lle", "San Francisco");
            editUsers.Add("etaylor", "San Francisco");
            editUsers.Add("vviera", "San Francisco");
            editUsers.Add("ssalamida", "");
            if (editUsers.Contains(Security.GetCurrentUserId.ToLower()))
            {
                market = Convert.ToString(editUsers[Security.GetCurrentUserId.ToLower()]);
                return true;
            }
            market = string.Empty;
            return false;
        }

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
                    + "#keyDisplay table {border:1px solid #333333;}"
                    + "#keyDisplay table td {border:1px solid#333333;text-align:center;font-size:10px;padding:2px;}"
                    + "</style>");
                excelHtml.Append("</head>");
                excelHtml.AppendFormat("<body>{0}</body>", exportData.Value);
                excelHtml.Append("</html>");
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
    }
}