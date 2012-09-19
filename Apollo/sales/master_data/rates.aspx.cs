#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class sales_master_data_rates : System.Web.UI.Page
    {

        #region AddRate method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool AddRate(Hashtable values)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_ADDRATE",
                        Param.CreateParam("RATENAME", SqlDbType.VarChar, values["RATENAME"]),
                        Param.CreateParam("RATELOCALRENEW", SqlDbType.Decimal, values["RATELOCALRENEW"]),
                        Param.CreateParam("RATELOCALNEW", SqlDbType.Decimal, values["RATELOCALNEW"]),
                        Param.CreateParam("RATENATIONALRENEW", SqlDbType.Decimal, values["RATENATIONALRENEW"]),
                        Param.CreateParam("RATENATIONALNEW", SqlDbType.Decimal, values["RATENATIONALNEW"]),
                        Param.CreateParam("RATEEFFECTIVEDATE", SqlDbType.Date, values["RATEEFFECTIVEDATE"]),
                        Param.CreateParam("ACTIVE", SqlDbType.VarChar, values["ACTIVE"]),
                        Param.CreateParam("ISPACKAGE", SqlDbType.Int, values["ISPACKAGE"]),
                        Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
            return true;
        }
        #endregion

        #region AddRateDetail method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>        
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool AddRateDetail(Hashtable values)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_ADDRATEDETAIL",
                        Param.CreateParam("RATEID", SqlDbType.Int, values["RATENAME"]),
                        Param.CreateParam("RATELOCALRENEW", SqlDbType.Decimal, values["RATELOCALRENEW"]),
                        Param.CreateParam("RATELOCALNEW", SqlDbType.Decimal, values["RATELOCALNEW"]),
                        Param.CreateParam("RATENATIONALRENEW", SqlDbType.Decimal, values["RATENATIONALRENEW"]),
                        Param.CreateParam("RATENATIONALNEW", SqlDbType.Decimal, values["RATENATIONALNEW"]),
                        Param.CreateParam("RATEEFFECTIVEDATE", SqlDbType.Date, values["RATEEFFECTIVEDATE"]),
                        Param.CreateParam("ACTIVE", SqlDbType.VarChar, values["ACTIVE"]),
                        Param.CreateParam("ISPACKAGE", SqlDbType.Int, values["ISPACKAGE"]),
                        Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
            return true;
        }
        #endregion

        #region CheckExistingRateDetailEffectiveDate method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool CheckExistingRateDetailEffectiveDate(Hashtable values)
        {
            int count;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                count = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("MASTERDATA_CHECKEXISTINGRATEDETAILEFFECTIVEDATE",
                    Param.CreateParam("RATEID", SqlDbType.Int, values["RATEID"]),
                    Param.CreateParam("RATEEFFECTIVEDATE", SqlDbType.Date, values["RATEEFFECTIVEDATE"]))));
            }
            return (count > 0);
        }
        #endregion

        #region CheckExistingRateDetailName method
        /// <summary>TBD</summary>
        /// <param name="rateDetailName">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool CheckExistingRateDetailName(string rateDetailName)
        {
            int count;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                count = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("MASTERDATA_CHECKEXISTINGRATENAME", Param.CreateParam("RATENAME", SqlDbType.VarChar, rateDetailName))));
            }
            return (count > 0);
        }
        #endregion

        #region LoadDetailRecord method
        /// <summary>TBD</summary>
        /// <param name="rateDetailId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static Hashtable LoadDetailRecord(int rateDetailId)
        {            
            DataSet rateDetailData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                rateDetailData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GetRateDetailById", Param.CreateParam("rateDetailId", SqlDbType.Int, rateDetailId)));
            }
            //Bind to fields
            Hashtable results = new Hashtable();
            results.Add("RATE_DETAIL_ID", rateDetailData.Tables[0].Rows[0]["RATE_DETAIL_ID"].ToString());
            results.Add("RATE_ID", rateDetailData.Tables[0].Rows[0]["RATE_ID"].ToString());
            results.Add("RATE_DETAIL_NAME", rateDetailData.Tables[0].Rows[0]["RATE_DETAIL_NAME"].ToString());
            results.Add("RATE_EFFECTIVE_DATE", rateDetailData.Tables[0].Rows[0]["RATE_EFFECTIVE_DATE"].ToString());
            results.Add("RATE_LOCAL_RENEW", rateDetailData.Tables[0].Rows[0]["RATE_LOCAL_RENEW"].ToString());
            results.Add("RATE_LOCAL_NEW", rateDetailData.Tables[0].Rows[0]["RATE_LOCAL_NEW"].ToString());
            results.Add("RATE_NATIONAL_RENEW", rateDetailData.Tables[0].Rows[0]["RATE_NATIONAL_RENEW"].ToString());
            results.Add("RATE_NATIONAL_NEW", rateDetailData.Tables[0].Rows[0]["RATE_NATIONAL_NEW"].ToString());
            results.Add("ACTIVE", rateDetailData.Tables[0].Rows[0]["ACTIVE"].ToString());
            //results.Add("IS_PACKAGE", rateDetailData.Tables[0].Rows[0]["IS_PACKAGE"].ToString());
            return results;
        }
        #endregion

        #region MaximumRatePercentage method
        /// <summary>Gets the Maximum allowed rate percentage value</summary>
        /// <returns>Returns an integer value of the Maximum allowed rate percentage</returns>
        public int MaximumRatePercentage()
        {
            return WebCommon.MaximumRatePercentage;
        }
        #endregion

        #region Page_Init method
        /// <summary>TBD</summary>
        protected void Page_Init()
        {
            if (!Security.IsAdminUser() && !Security.IsCorporateUser())
            {
                WebCommon.ShowAlert("You are not authorized to use this portion of the application.");
                Server.Transfer("/Default.aspx");
            }
        }
        #endregion

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region UpdateRate method
        /// <summary>TBD</summary>
        /// <param name="values">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        public static bool UpdateRate(Hashtable values)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_UPDATERATE",
                        Param.CreateParam("RATEID", SqlDbType.Int, values["RATEID"]),
                        Param.CreateParam("RATENAME", SqlDbType.VarChar, values["RATENAME"]),
                        Param.CreateParam("RATEDETAILID", SqlDbType.Int, values["RATEDETAILID"]),
                        Param.CreateParam("RATELOCALRENEW", SqlDbType.Decimal, values["RATELOCALRENEW"]),
                        Param.CreateParam("RATELOCALNEW", SqlDbType.Decimal, values["RATELOCALNEW"]),
                        Param.CreateParam("RATENATIONALRENEW", SqlDbType.Decimal, values["RATENATIONALRENEW"]),
                        Param.CreateParam("RATENATIONALNEW", SqlDbType.Decimal, values["RATENATIONALNEW"]),
                        Param.CreateParam("ACTIVE", SqlDbType.VarChar, values["ACTIVE"]),
                        Param.CreateParam("ISPACKAGE", SqlDbType.Int, values["ISPACKAGE"]),
                        Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return false;
            }
            return true;
        }
        #endregion
    }
}
