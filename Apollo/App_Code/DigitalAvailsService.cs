#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using AjaxControlToolkit;
using Apollo.DigitalAvails;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for DigitalAvailsService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class DigitalAvailsService : System.Web.Services.WebService
    {
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetReportMarketList()
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetActiveReportMarkets"));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Market - ", "", true));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["ReportMarket"]), Convert.ToString(row["ReportMarket"]), false));
            }
            return values.ToArray();
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetReportYearList(string reportMarket)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetReportYearList", Param.CreateParam("reportMarket", SqlDbType.VarChar, reportMarket)));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Year - ", "", false));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["Year"]), Convert.ToString(row["Year"]), ((int)row["Year"] == DateTime.Now.Year)));
            }
            return values.ToArray();
        }

        #region GetMarketList method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetMarketList(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetMarkets"));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Market - ", "", (defaultValue == "")));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["Market"]), Convert.ToString(row["Market"]), (defaultValue == Convert.ToString(row["Market"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetSpotTypesByMarket method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetSpotTypesByMarket(string market, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetSpotTypesByMarket", Param.CreateParam("MARKET", SqlDbType.VarChar, market)));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Spot Type - ", "", (defaultValue == "")));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["Description"]), Convert.ToString(row["SpotTypeId"]), (defaultValue == Convert.ToString(row["SpotTypeId"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetSpotTypes method
        /// <summary>TBD</summary>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetSpotTypes(string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetSpotTypes"));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Spot Type - ", "", (defaultValue == "")));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["Description"]), Convert.ToString(row["SpotTypeId"]), (defaultValue == Convert.ToString(row["SpotTypeId"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetStationByMarket method
        /// <summary>TBD</summary>
        /// <param name="market">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public CascadingDropDownNameValue[] GetStationByMarket(string market, string defaultValue)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            DataSet results;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                results = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetStationsByMarket", Param.CreateParam("MARKET", SqlDbType.VarChar, market)));
            }
            values.Add(new CascadingDropDownNameValue(" - Select a Location - ", "", (defaultValue == "")));
            foreach (DataRow row in results.Tables[0].Rows)
            {
                values.Add(new CascadingDropDownNameValue(Convert.ToString(row["Name"]), Convert.ToString(row["StationId"]), (defaultValue == Convert.ToString(row["StationId"]))));
            }
            return values.ToArray();
        }
        #endregion

        #region GetStationInfo method
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Station GetStationInfo(int stationId)
        {
            try
            {
                Station station = new Station(stationId);
                return station;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("An error occurred while trying to load info for StationId: {0}", stationId), ex));
                throw new Exception("An error occurred while trying to load the Station info.");
            }
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<CampaignSearchResult> ExecuteCampaignSearch(string campaignSearchText)
        {
            try
            {
                return CampaignSearchResult.ExecuteCampaignSearch(campaignSearchText);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("An error occurred while trying to execute a Campaign search for the following text: {0}.", campaignSearchText), ex));
                throw new Exception("An error occurred while trying to execute the Campaign search.");
            }
        }

        #region GetStationInfoWithMatrix method
        /// <summary>TBD</summary>
        /// <param name="stationOptions">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Station GetStationInfoWithMatrix(StationOptions stationOptions)
        {
            try
            {
                Station station = new Station(stationOptions);
                return station;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("An error occurred while trying to load info for StationId: {0}", stationOptions.stationId), ex));
                throw new Exception("An error occurred while trying to load the Station info.");
            }
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public SpotCampaignDetail GetSpotCampaignDetail(Spot spot)
        {
            try
            {
                return SpotCampaignDetail.GetSpotCampaignDetail(spot.StationId, spot.StationSpotId, spot.SpotDate, spot.CampaignName, spot.CampaignNumber);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occured while trying to retrieve the Campaign detail for the selected spot.");
            }
        }

        #region InnerSaveSpot method
        /// <summary>TBD</summary>
        /// <param name="spot">TBD</param>
        /// <returns>TBD</returns>
        public Spot InnerSaveSpot(Spot spot)
        {
            spot.EnteredBy = Security.GetCurrentUserId;
            spot.EnteredOn = DateTime.Now;
            spot.LastUpdatedBy = Security.GetCurrentUserId;
            spot.LastUpdatedOn = DateTime.Now;
            spot.Save();
            return spot;
        }
        #endregion

        #region LoadSpot method
        /// <summary>TBD</summary>
        /// <param name="spotId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Spot LoadSpot(int spotId)
        {
            try
            {
                Spot spot = new Spot(spotId);
                return spot;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to load the spot.");
            }
        }
        #endregion

        #region LoadSpotsByStationId method
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        /// <param name="startMonth">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<Spot> LoadSpotsByStationId(int stationId, int startMonth)
        {
            return null;
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Spot UpdateMultipleSpots(Spot currentSpot, List<int> spotIdsToUpdate)
        {
            try
            {
                currentSpot.LastUpdatedBy = Security.GetCurrentUserId;
                currentSpot.LastUpdatedOn = DateTime.Now;
                foreach (int spotId in spotIdsToUpdate)
                {
                    currentSpot.SpotId = spotId;
                    currentSpot.Update();
                }
                return currentSpot;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("Could not Update the following list of IDs: {0}", String.Join(",", Array.ConvertAll<int, string>(spotIdsToUpdate.ToArray(), new Converter<int, string>(Convert.ToString))), ex)));
                throw new Exception("An error occurred while trying to delete the spot(s).");
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeleteMultipleSpots(List<int> spotIdsToDelete)
        {
            try
            {
                foreach (int spotId in spotIdsToDelete)
                {
                    Spot.DeleteSpot(spotId);
                }
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(new Exception(string.Format("Could not Delete the following list of IDs: {0}", String.Join(",", Array.ConvertAll<int, string>(spotIdsToDelete.ToArray(), new Converter<int, string>(Convert.ToString))), ex)));
                throw new Exception("An error occurred while trying to delete the spot(s).");
            }
        }

        #region GetStationIdTable method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static DataTable GetStationIdTable()
        {
            DataTable stations = new DataTable("stations");
            stations.Columns.Add(new DataColumn("INT_PARAM"));
            return stations;
        }
        #endregion

        public static DataTable PopulateStationIdTable(List<int> stationIds)
        {
            DataTable stations = GetStationIdTable();
            foreach (int stationId in stationIds)
            {
                stations.Rows.Add(stationId);
            }
            return stations;
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<SpotConflict> SaveMultiStationSpot(Spot spot, int numberOfWeeks, List<int> stationIds)
        {
            try
            {
                spot.EnteredBy = Security.GetCurrentUserId;
                spot.EnteredOn = DateTime.Now;
                spot.LastUpdatedBy = Security.GetCurrentUserId;
                spot.LastUpdatedOn = DateTime.Now;
                DataTable stations = PopulateStationIdTable(stationIds);
                List<SpotConflict> conflicts = Spot.SaveMultiStationSpot(spot, stations);
                int prevMonth = spot.Month;
                for (int i = 1; i < numberOfWeeks; i++)
                {
                    spot.SpotDate = spot.SpotDate.AddDays(7);                    
                    spot.Year = spot.SpotDate.Year;
                    spot.Month = spot.SpotDate.Month;
                    spot.Week = (spot.SpotDate.Month == prevMonth ? spot.Week + 1 : 1);
                    prevMonth = spot.SpotDate.Month;
                    foreach (SpotConflict conflict in Spot.SaveMultiStationSpot(spot, stations))
                    {
                        conflicts.Add(conflict);
                    }
                    //conflicts.Add(Spot.SaveMultiStationSpot(spot, stations));
                }                
                return conflicts;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to add the spot.");
            }
        }

        #region SaveMultipleSpots method
        /// <summary>TBD</summary>
        /// <param name="spot">TBD</param>
        /// <param name="numberOfWeeks">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<SpotConflict> SaveMultipleSpots(Spot spot, int numberOfWeeks)
        {
            try
            {
                DateTime spotDate = spot.SpotDate;
                List<Spot> spots = new List<Spot>();
                List<SpotConflict> spotConflicts = new List<SpotConflict>();
                //This *should* not need to be checked as a user can not add a spot into a non-empty cell
                spots.Add(InnerSaveSpot(spot));
                Spot newSpot;
                int prevMonth = spot.Month;
                int week = spot.Week;
                for (int i = 1; i < numberOfWeeks; i++)
                {
                    spotDate = spotDate.AddDays(7);
                    week = (spotDate.Month == prevMonth ? week + 1 : 1);
                    newSpot = new Spot(spot.StationId, spot.StationSpotId, spot.SpotTypeId
                        , spotDate, spotDate.Year, spotDate.Month, week
                        , spot.CampaignName, spot.CampaignNumber, spot.Description);
                    prevMonth = spotDate.Month;
                    try
                    {
                        spots.Add(InnerSaveSpot(newSpot));
                    }
                    catch (SpotConflictException)
                    {
                        spotConflicts.Add(new SpotConflict(spotDate));
                    }
                }
                return spotConflicts;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to save the spot.");
            }
        }
        #endregion

        #region SaveSpot method
        /// <summary>TBD</summary>
        /// <param name="spot">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public SpotConflict SaveSpot(Spot spot)
        {
            try
            {
                //return InnerSaveSpot(spot);
                SpotConflict spotContflicts = new SpotConflict();
                try
                {
                    InnerSaveSpot(spot);
                }
                catch (SpotConflictException)
                {
                    spotContflicts = new SpotConflict(spot.SpotDate);
                }
                return spotContflicts;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to save the spot.");
            }
        }
        #endregion

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeleteSpot(int spotId)
        {
            try
            {
                Spot.DeleteSpot(spotId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to delete the spot.");
            }
        }

        #region UpdateSpot method
        /// <summary>TBD</summary>
        /// <param name="spot">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public Spot UpdateSpot(Spot spot)
        {
            try
            {
                spot.LastUpdatedBy = Security.GetCurrentUserId;
                spot.LastUpdatedOn = DateTime.Now;
                spot.Update();
                return spot;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to update the spot.");
            }
        }
        #endregion

    }

}
