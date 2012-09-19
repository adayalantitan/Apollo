#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using AjaxControlToolkit;
using Apollo.StationDomAvails;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for ProductionIOService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class ProductionIOService : System.Web.Services.WebService
    {

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeleteLocation(int locationId)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("Production_DeleteLocation", Param.CreateParam("LOCATIONID", SqlDbType.Int, locationId)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("An error occurred while trying to delete location id: {0}. Inner Exception: {1}", locationId, ex.InnerException)));
                throw new Exception("An error occurred while trying to delete the location.");
            }
        }

        #region AddUpdateBooking method
        /// <summary>TBD</summary>
        /// <param name="booking">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void AddUpdateBooking(Booking booking)
        {
            bool isAdding = (booking.bookingId == -1);
            int successCode = 0;
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("LOCATIONID", SqlDbType.Int, booking.locationId));
                spParams.Add(Param.CreateParam("BOOKINGID", SqlDbType.Int, booking.bookingId));
                spParams.Add(Param.CreateParam("BOOKINGDESC", SqlDbType.VarChar, booking.bookingDesc));
                spParams.Add(Param.CreateParam("STARTDATE", SqlDbType.DateTime, booking.bookingStartDate));
                spParams.Add(Param.CreateParam("ENDDATE", SqlDbType.DateTime, booking.bookingEndDate));
                spParams.Add(Param.CreateParam("AENTID", SqlDbType.VarChar, booking.aeNtId));
                if (booking.bookingStatusId != -1)
                {
                    spParams.Add(Param.CreateParam("BOOKINGSTATUSID", SqlDbType.Int, booking.bookingStatusId));
                }
                if (!String.IsNullOrEmpty(booking.bookingComments))
                {
                    spParams.Add(Param.CreateParam("BOOKINGCOMMENTS", SqlDbType.VarChar, booking.bookingComments));
                }
                spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    successCode = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("Production_AddUpdateBooking", spParams)));
                }
                if (successCode == -1)
                {
                    throw new Exception("Booking date collision");
                }

            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                if (successCode == -1)
                {
                    throw new Exception(string.Format("An error occurred while trying to {0} the Booking. The dates conflicted with an existing Booking.", (isAdding ? "Add" : "Update")));
                }
                else
                {
                    throw new Exception(string.Format("An error occurred while trying to {0} the Booking.", (isAdding ? "Add" : "Update")));
                }
            }            
        }
        #endregion

        #region AddUpdateLocation method
        /// <summary>TBD</summary>
        /// <param name="location">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void AddUpdateLocation(Location location)
        {
            bool isAdding = (location.locationId == -1);
            try
            {
                List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
                spParams.Add(Param.CreateParam("LOCATIONID", SqlDbType.Int, location.locationId));
                spParams.Add(Param.CreateParam("LOCATIONMARKET", SqlDbType.VarChar, location.locationMarket));
                spParams.Add(Param.CreateParam("LOCATIONDESC", SqlDbType.VarChar, location.locationDesc));
                spParams.Add(Param.CreateParam("FOURWEEKRATE", SqlDbType.VarChar, location.fourWeekRate));
                spParams.Add(Param.CreateParam("PRODINSTALLRATE", SqlDbType.VarChar, location.prodInstallRate));
                spParams.Add(Param.CreateParam("RESERVEWINTERMONTHS", SqlDbType.Int, (location.reserveWinterMonths ? -1 : 0)));
                if (location.locationStatusId != -1)
                {
                    spParams.Add(Param.CreateParam("LOCATIONSTATUSID", SqlDbType.Int, location.locationStatusId));
                }
                if (!String.IsNullOrEmpty(location.locationComments))
                {
                    spParams.Add(Param.CreateParam("LOCATIONCOMMENTS", SqlDbType.VarChar, location.locationComments));
                }
                if (!String.IsNullOrEmpty(location.prodInstallRateAdditional))
                {
                    spParams.Add(Param.CreateParam("PRODINSTALLRATEADDITIONAL", SqlDbType.VarChar, location.prodInstallRateAdditional));
                }
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("Production_AddUpdateLocation", spParams));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception(string.Format("An error occurred while trying to {0} the Location.", (isAdding ? "Add" : "Update")));
            }
        }
        #endregion

        #region DeleteBooking method
        /// <summary>TBD</summary>
        /// <param name="bookingId">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeleteBooking(int bookingId)
        {
            try
            {
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("PRODUCTION_DELETEBOOKING", Param.CreateParam("BOOKINGID", SqlDbType.Int, bookingId)));
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to delete the Booking.");
            }
        }
        #endregion

        #region GetAvails method
        /// <summary>TBD</summary>
        /// <param name="year">TBD</param>
        /// <param name="market">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<Avails> GetAvails(int year, string market)
        {
            try
            {
                List<Avails> availsList = Avails.GetAvails(year, market);                
                return availsList;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occured while trying to retrieve the Avails List.");
            }
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetBookingAEList(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Production_GetBookingAEList"));
            }
            values.Add(new CascadingDropDownNameValue(" - Select an AE - ", "-1", (defaultValue == "")));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["DISPLAY_NAME"]), Convert.ToString(row["NTID"]), (defaultValue == Convert.ToString(row["NTID"]))));
            }
            return values.ToArray();
        }

        #region GetBookingStatusList method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetBookingStatusList(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Production_GetBookingStatusList"));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Status - ", "-1", (defaultValue == "")));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["BOOKING_STATUS_DESC"]), Convert.ToString(row["BOOKING_STATUS_ID"]), (defaultValue == Convert.ToString(row["BOOKING_STATUS_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetLocationStatusList method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetLocationStatusList(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Production_GetLocationStatusList"));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Status - ", "-1", (defaultValue == "")));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["LOCATION_STATUS_DESC"]), Convert.ToString(row["LOCATION_STATUS_ID"]), (defaultValue == Convert.ToString(row["LOCATION_STATUS_ID"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region LoadBookingById method
        /// <summary>TBD</summary>
        /// <param name="bookingId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Booking LoadBookingById(int bookingId)
        {
            try
            {
                DataSet bookingData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    bookingData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("Production_GetBookingById", Param.CreateParam("BOOKINGID", SqlDbType.Int, bookingId)));
                }
                return new Booking(Convert.ToInt32(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "LOCATION_ID", -1))
                    , bookingId
                    , Convert.ToString(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "BOOKING_DESC", ""))
                    , Convert.ToDateTime(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "BOOKING_START_DATE", DateTime.MinValue))
                    , Convert.ToDateTime(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "BOOKING_END_DATE", DateTime.MaxValue))
                    , Convert.ToInt32(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "BOOKING_STATUS_ID", -1))
                    , Convert.ToString(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "BOOKING_STATUS_DESC", ""))
                    , Convert.ToString(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "BOOKING_COMMENTS", ""))
                    , Convert.ToString(IO.GetDataRowValue(bookingData.Tables[0].Rows[0], "AE_NTID", "-1"))
                );
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve Booking data.");
            }
        }
        #endregion

    }

}
