using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Titan.DataIO;

namespace Apollo
{
    public partial class sales_FlashDataDump : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dropDownStartYear.SelectedValue = Convert.ToString(DateTime.Now.Year - 2);
            }
        }
        protected void exportBillingDataDump_Click(object sender, EventArgs e)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("STARTDATE", System.Data.SqlDbType.Date, DateTime.Now));
                spParams.Add(Param.CreateParam("ENTRYDATESTART", System.Data.SqlDbType.Date, "1/1/2002"));
                spParams.Add(Param.CreateParam("ENTRYDATEEND", System.Data.SqlDbType.Date, DateTime.Now));
                spParams.Add(Param.CreateParam("COMPANYID", System.Data.SqlDbType.Int, Convert.ToInt32(dropDownCompany.SelectedValue)));
                spParams.Add(Param.CreateParam("TRANSITY", System.Data.SqlDbType.VarChar, "Y"));
                spParams.Add(Param.CreateParam("TRANSITN", System.Data.SqlDbType.VarChar, "N"));
                spParams.Add(Param.CreateParam("REVENUETYPE", System.Data.SqlDbType.VarChar, "M"));
                spParams.Add(Param.CreateParam("ORDERBY", System.Data.SqlDbType.VarChar, "CONTRACT_NUMBER"));
                spParams.Add(Param.CreateParam("SHOWCONSOLIDATED", System.Data.SqlDbType.Int, 1));
                spParams.Add(Param.CreateParam("EXCLUDEMTA", System.Data.SqlDbType.Int, 1));
                spParams.Add(Param.CreateParam("STARTYEAR", System.Data.SqlDbType.Int, Convert.ToInt32(dropDownStartYear.SelectedValue)));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    WebCommon.ExportHtmlToExcel("BillingFlashDataDump", WebCommon.DataTableToHtmlTable(io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("OnlineFlash_GetFlashDataDump", spParams)).Tables[0], "BillingFlashDataDump"));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
        }
        protected void exportRevenueDataDump_Click(object sender, EventArgs e)
        {
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("STARTDATE", System.Data.SqlDbType.Date, DateTime.Now));
                spParams.Add(Param.CreateParam("ENTRYDATESTART", System.Data.SqlDbType.Date, "1/1/2002"));
                spParams.Add(Param.CreateParam("ENTRYDATEEND", System.Data.SqlDbType.Date, DateTime.Now));
                spParams.Add(Param.CreateParam("COMPANYID", System.Data.SqlDbType.Int, Convert.ToInt32(dropDownCompany.SelectedValue)));
                spParams.Add(Param.CreateParam("TRANSITY", System.Data.SqlDbType.VarChar, "Y"));
                spParams.Add(Param.CreateParam("TRANSITN", System.Data.SqlDbType.VarChar, "N"));
                spParams.Add(Param.CreateParam("REVENUETYPE", System.Data.SqlDbType.VarChar, "M"));
                spParams.Add(Param.CreateParam("ORDERBY", System.Data.SqlDbType.VarChar, "CONTRACT_NUMBER"));
                spParams.Add(Param.CreateParam("SHOWCONSOLIDATED", System.Data.SqlDbType.Int, 1));
                spParams.Add(Param.CreateParam("EXCLUDEMTA", System.Data.SqlDbType.Int, 1));
                spParams.Add(Param.CreateParam("STARTYEAR", System.Data.SqlDbType.Int, Convert.ToInt32(dropDownStartYear.SelectedValue)));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    WebCommon.ExportHtmlToExcel("RevenueFlashDataDump", WebCommon.DataTableToHtmlTable(io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("OnlineFlash_GetRevFlashDataDump", spParams)).Tables[0], "RevenueFlashDataDump"));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
            }
        }
}
}