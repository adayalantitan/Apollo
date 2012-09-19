#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;
using ExcelXml = Titan.ExcelXml;
using Titan.DataIO;
#endregion

namespace Apollo.DigitalAvails
{

    /// <summary>TBD</summary>
    public class CampaignSearchResult
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public CampaignSearchResult()
        {
        }
        #endregion

        #region CampaignName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string CampaignName { get; set; }
        #endregion

        #region CampaignNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string CampaignNumber { get; set; }
        #endregion

        #region ExecuteCampaignSearch method
        /// <summary>TBD</summary>
        /// <param name="campaignSearchText">TBD</param>
        /// <returns>TBD</returns>
        public static List<CampaignSearchResult> ExecuteCampaignSearch(string campaignSearchText)
        {
            DataSet campaignSearchData;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                campaignSearchData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALAVAILS_EXECUTECAMPAIGNSEARCH", Param.CreateParam("CAMPAIGNSEARCHTEXT", SqlDbType.VarChar, campaignSearchText)));
            }
            List<CampaignSearchResult> searchResults = new List<CampaignSearchResult>();
            if (campaignSearchData.Tables.Count == 0 || campaignSearchData.Tables[0].Rows.Count == 0)
            {
                return searchResults;
            }
            CampaignSearchResult searchResult;
            foreach (DataRow row in campaignSearchData.Tables[0].Rows)
            {
                searchResult = new CampaignSearchResult();
                searchResult.NumberOfSpots = (int)row["NumberOfSpots"];
                searchResult.Market = (string)row["Market"];
                searchResult.StationName = (string)row["StationName"];
                searchResult.CampaignName = Convert.ToString(IO.GetDataRowValue(row, "CampaignName", ""));
                searchResult.CampaignNumber = Convert.ToString(IO.GetDataRowValue(row, "CampaignNumber", ""));
                searchResult.Month = (int)row["Month"];
                searchResult.Year = (int)row["Year"];
                searchResults.Add(searchResult);
            }
            return searchResults;
        }
        #endregion

        #region Market property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Market { get; set; }
        #endregion

        #region Month property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int Month { get; set; }
        #endregion

        #region MonthDisplay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string MonthDisplay
        {
            get
            {
                DateTime aDate = new DateTime(DateTime.Now.Year, this.Month, 1);
                return string.Format("{0:MMMM}", aDate);
            }
        }
        #endregion

        #region NumberOfSpots property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int NumberOfSpots { get; set; }
        #endregion

        #region StationName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string StationName { get; set; }
        #endregion

        #region Year property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int Year { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class DigitalAvailsReport
    {

        #region ReportDataTable enumeration
        private enum ReportDataTable
        {
        }
        #endregion

        #region ReportStyle enumeration
        private enum ReportStyle
        {
        }
        #endregion

        #region Default constructor
        /// <summary>TBD</summary>
        private DigitalAvailsReport()
        {
        }
        #endregion

        #region CreateStationWorksheet method
        /// <summary>TBD</summary>
        /// <param name="stationName">TBD</param>
        /// <returns>TBD</returns>
        public ExcelXml.Worksheet CreateStationWorksheet(string stationName)
        {
            ExcelXml.Worksheet stationWorksheet = new ExcelXml.Worksheet(stationName);
            stationWorksheet.Table.Columns = this.ReportColumns;
            ExcelXml.PageSetup stationPageSetup = new ExcelXml.PageSetup();
            stationPageSetup.Header.Margin = 0.3;
            stationPageSetup.Footer.Margin = 0.3;
            stationPageSetup.PageMargins.Bottom = 0.25;
            stationPageSetup.PageMargins.Left = 0.25;
            stationPageSetup.PageMargins.Right = 0.25;
            stationPageSetup.PageMargins.Top = 0.25;
            stationPageSetup.Layout.Orientation = ExcelXml.Orientation.Landscape;
            stationPageSetup.Layout.CenterHorizontal = true;
            stationPageSetup.Layout.CenterVertical = true;
            stationWorksheet.WorksheetOptions = new ExcelXml.WorksheetOptions(stationPageSetup, new ExcelXml.Print(null, null, null, null, null, true), false, 75, null, false
            , 4, 1, new ExcelXml.Pane(3), new ExcelXml.Pane(1), new ExcelXml.Pane(2), new ExcelXml.Pane(0, 2, 7));
            return stationWorksheet;
        }
        #endregion

        #region DigitalAvailsReportWorkbook property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public ExcelXml.Workbook DigitalAvailsReportWorkbook { get; set; }
        #endregion

        #region FindFirstMonday method
        /// <summary>TBD</summary>
        /// <param name="initialDate">TBD</param>
        /// <returns>TBD</returns>
        public static DateTime FindFirstMonday(DateTime initialDate)
        {
            DateTime firstMonday = new DateTime(initialDate.Year, initialDate.Month, initialDate.Day);
            if (firstMonday.DayOfWeek == DayOfWeek.Monday)
            {
                return firstMonday;
            }
            while (firstMonday.DayOfWeek != DayOfWeek.Monday)
            {
                firstMonday = firstMonday.AddDays(1);
            }
            return firstMonday;
        }
        #endregion

        #region GenerateMarketReport method
        /// <summary>TBD</summary>
        /// <param name="market">TBD</param>
        /// <param name="startMonth">TBD</param>
        /// <param name="startYear">TBD</param>
        /// <param name="numberOfMonths">TBD</param>
        /// <returns>TBD</returns>
        public static DigitalAvailsReport GenerateMarketReport(string market, int startMonth, int startYear, int numberOfMonths)
        {
            return GenerateStationReport(market, startMonth, startYear, numberOfMonths, -1, false);
        }
        #endregion

        #region GenerateSpotRow method
        /// <summary>TBD</summary>
        /// <param name="stationSpot">TBD</param>
        /// <param name="stationSpots">TBD</param>
        /// <returns>TBD</returns>
        public ExcelXml.Row GenerateSpotRow(StationSpot stationSpot, List<Spot> stationSpots)
        {
            List<ExcelXml.Cell> spotCells = new List<ExcelXml.Cell>();
            Spot currentSpot;
            //Add the Row Title for this station spot
            spotCells.Add(
            new ExcelXml.Cell("sStationSpotName", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(stationSpot.Name)), string.Empty, string.Empty, null, null, null));
            foreach (DateTime spotDate in this.ReportDates)
            {
                if (stationSpots.Where(s => s.SpotDate == spotDate).Count() > 0)
                {
                    currentSpot = stationSpots.Single(s => s.SpotDate == spotDate);
                    spotCells.Add(
                    new ExcelXml.Cell(string.Format("sSpotTypeId_{0}", currentSpot.SpotTypeId), string.Empty
                    , new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(currentSpot.CampaignName))
                    , (String.IsNullOrEmpty(currentSpot.Description) ? null : new ExcelXml.Comment(new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(currentSpot.Description), true), currentSpot.EnteredBy))
                    , string.Empty, string.Empty, null, null, null));
                }
                else
                {
                    spotCells.Add(
                    new ExcelXml.Cell(string.Empty, string.Empty
                    , new ExcelXml.Data(ExcelXml.Type.String, string.Empty)
                    , string.Empty, string.Empty, null, null, null));
                }
            }
            return new ExcelXml.Row(null, 45, false, null, null, null, spotCells.ToArray());
        }
        #endregion

        #region GenerateSpotTypeBreakdownRows method
        /// <summary>TBD</summary>
        /// <param name="numberOfSpots">TBD</param>
        /// <param name="spotTypeBreakdownData">TBD</param>
        /// <returns>TBD</returns>
        public List<ExcelXml.Row> GenerateSpotTypeBreakdownRows(int numberOfSpots, List<SpotTypeBreakdown> spotTypeBreakdownData)
        {
            List<ExcelXml.Row> spotTypeBreakdownRows = new List<ExcelXml.Row>();
            List<ExcelXml.Cell> spotTypeBreakdownCells;
            ExcelXml.Row spotTypeBreakdownRow;
            string spotTypeStyleBase = "sSpotTypeId_{0}";
            string spotTypeStylePercentageBase = "sSpotTypeId_{0}_Percentage";
            string styleBase;
            bool isPercentage;
            //TODO REFACTOR!
            foreach (SpotTypeBreakdown spotTypeBreakdown in spotTypeBreakdownData)
            {
                isPercentage = spotTypeBreakdown.SpotType.Contains("%");
                styleBase = (isPercentage ? spotTypeStylePercentageBase : spotTypeStyleBase);
                spotTypeBreakdownRow = new ExcelXml.Row(null, 20, null, null, null, null);
                spotTypeBreakdownCells = new List<ExcelXml.Cell>();
                spotTypeBreakdownCells.Add(new ExcelXml.Cell(string.Format(styleBase, spotTypeBreakdown.SpotTypeId), string.Empty, new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(spotTypeBreakdown.SpotType)), string.Empty, string.Empty, null, null, null));
                foreach (int spotTypeCount in spotTypeBreakdown.SpotTypeCount)
                {
                    spotTypeBreakdownCells.Add(new ExcelXml.Cell(string.Format(styleBase, spotTypeBreakdown.SpotTypeId), string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, HttpUtility.HtmlEncode(Convert.ToString(string.Format("{0}", (isPercentage ? spotTypeCount / 100.0 : spotTypeCount))))), string.Empty, string.Empty, null, null, null));
                }
                spotTypeBreakdownRow.Cells = spotTypeBreakdownCells;
                spotTypeBreakdownRows.Add(spotTypeBreakdownRow);
            }
            spotTypeBreakdownRow = new ExcelXml.Row(null, 20, null, null, null, null);
            spotTypeBreakdownCells = new List<ExcelXml.Cell>();
            spotTypeBreakdownCells.Add(new ExcelXml.Cell("sSpotTypeBreakdownTotal", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, "Total Available Spots"), string.Empty, string.Empty, null, null, null));
            for (int i = 0; i < spotTypeBreakdownData[0].SpotTypeCount.Count; i++)
            {
                spotTypeBreakdownCells.Add(new ExcelXml.Cell("sSpotTypeBreakdownTotal", string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, HttpUtility.HtmlEncode(Convert.ToString(numberOfSpots))), string.Empty, string.Empty, null, null, null));
            }
            spotTypeBreakdownRow.Cells = spotTypeBreakdownCells;
            spotTypeBreakdownRows.Add(spotTypeBreakdownRow);
            return spotTypeBreakdownRows;
        }
        #endregion

        #region GenerateStationReport method
        /// <summary>TBD</summary>
        /// <param name="market">TBD</param>
        /// <param name="startMonth">TBD</param>
        /// <param name="startYear">TBD</param>
        /// <param name="numberOfMonths">TBD</param>
        /// <param name="reportStationId">TBD</param>
        /// <param name="wantSingleStation">TBD</param>
        /// <returns>TBD</returns>
        public static DigitalAvailsReport GenerateStationReport(string market, int startMonth, int startYear, int numberOfMonths, int reportStationId, bool wantSingleStation)
        {
            DigitalAvailsReport report = new DigitalAvailsReport();
            report.Market = market;
            report.StartMonth = startMonth;
            report.StartYear = startYear;
            report.NumberOfMonths = numberOfMonths;
            report.ReportStationId = reportStationId;
            report.WantSingleStation = wantSingleStation;
            report.GetReportData();
            report.GenerateWorkbook();
            return report;
        }
        #endregion

        #region GenerateWorkbook method
        /// <summary>TBD</summary>
        public void GenerateWorkbook()
        {
            this.DigitalAvailsReportWorkbook = new ExcelXml.Workbook();
            InitializeReportColumns();
            InitializeReportStyles();
            InitializeHeaderRows();
            this.DigitalAvailsReportWorkbook.Styles = this.ReportStyles;
            //Each station in the data is going to get its own worksheet
            ExcelXml.Worksheet stationWorksheet;
            foreach (Station station in this.MarketStations)
            {
                if (this.WantSingleStation && station.StationId != this.ReportStationId)
                {
                    continue;
                }
                stationWorksheet = CreateStationWorksheet((station.Name.Length > 25 ? station.Name.Substring(0, 25) : station.Name).Replace("/", " & "));
                //Add station information
                stationWorksheet.Table.Rows.Add(new ExcelXml.Row(null, 75, false, null, null, null
                , new ExcelXml.Cell("sStationName", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(station.Name)), string.Empty, string.Empty, null, null, null)));
                if (!String.IsNullOrEmpty(station.Description))
                {
                    stationWorksheet.Table.Rows.Add(new ExcelXml.Row(null, 50, false, null, null, null
                    , new ExcelXml.Cell("sStationDescription", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(station.Description)), string.Empty, string.Empty, null, null, null)));
                }
                stationWorksheet.Table.Rows.AddRange(this.MonthDateHeaderRows);
                //process station data
                foreach (StationSpot stationSpot in station.StationSpots)
                {
                    //TODO: REFACTOR!
                    if (stationSpot.DividerBefore)
                    {
                        stationWorksheet.Table.Rows.Add(this.SpotDividerRow);
                    }
                    stationWorksheet.Table.Rows.Add(GenerateSpotRow(stationSpot, station.SpotMatrix.Where(s => s.StationSpotId == stationSpot.StationSpotId).ToList()));
                    if (stationSpot.DividerAfter)
                    {
                        stationWorksheet.Table.Rows.Add(this.SpotDividerRow);
                    }
                }
                stationWorksheet.Table.Rows.Add(this.SpotDividerRow);
                //Add the Spot Type Breakdown data
                stationWorksheet.Table.Rows.AddRange(GenerateSpotTypeBreakdownRows(station.TotalBreakdownSpots, station.SpotTypeBreakdown));
                this.DigitalAvailsReportWorkbook.Worksheets.Add(stationWorksheet);
            }
        }
        #endregion

        #region GetReportData method
        /// <summary>TBD</summary>
        public void GetReportData()
        {
            DataSet reportData;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                reportData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetSpotMatrixForMarket",
                Param.CreateParam("MARKET", SqlDbType.VarChar, this.Market),
                Param.CreateParam("STARTMONTH", SqlDbType.Int, this.StartMonth),
                Param.CreateParam("STARTYEAR", SqlDbType.Int, this.StartYear),
                Param.CreateParam("NUMBEROFMONTHS", SqlDbType.Int, this.NumberOfMonths)));
            }
            //}
            //Load all the spots for the market
            //this.MarketSpotMatrix = Spot.LoadSpotMatrix(reportData.Tables[2]);
            this.MarketSpotMatrix = Spot.LoadSpotMatrix(reportData.Tables[2]);
            //Load all the spot type breakdowns for the market
            //this.MarketSpotTypeBreakdown = SpotTypeBreakdown.LoadSpotTypeBreakdown(reportData.Tables[3]);
            this.MarketSpotTypeBreakdown = SpotTypeBreakdown.LoadSpotTypeBreakdown(reportData.Tables[3]);
            //Load all the station spots for the market
            //this.MarketStationSpots = StationSpot.LoadStationSpots(reportData.Tables[1]);
            this.MarketStationSpots = StationSpot.LoadStationSpots(reportData.Tables[1]);
            //Load the stations
            this.MarketStations = new List<Station>();
            Station station;
            int stationId;
            foreach (DataRow stationRow in reportData.Tables[0].Rows)
            {
                stationId = (int)stationRow["StationId"];
                station = new Station();
                station.LoadStationData(stationRow);
                station.SpotMatrix = this.MarketSpotMatrix.Where(m => m.StationId == stationId).ToList();
                station.SpotTypeBreakdown = this.MarketSpotTypeBreakdown.Where(b => b.StationId == stationId).OrderBy(b => b.BreakdownOrder).ToList();
                station.StationSpots = this.MarketStationSpots.Where(s => s.StationId == stationId).OrderBy(s => s.Order).ToList();
                this.MarketStations.Add(station);
            }
            //Sort the station list
            this.MarketStations = this.MarketStations.OrderBy(s => s.Name).ToList();
            //Retrieve the Spot Types for this market
            this.MarketSpotTypes = SpotType.GetSpotTypesByMarket(this.Market);
        }
        #endregion

        #region InitializeHeaderRows method
        /// <summary>TBD</summary>
        public void InitializeHeaderRows()
        {
            List<ExcelXml.Row> headerRows = new List<ExcelXml.Row>();
            ExcelXml.Row monthHeaderRow = new ExcelXml.Row(null, 20, null, null, null, string.Empty
            , new ExcelXml.Cell(string.Empty, string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Empty), string.Empty, string.Empty, null, null, null));
            ExcelXml.Row dateHeaderRow = new ExcelXml.Row(null, 18, null, null, null, string.Empty
            , new ExcelXml.Cell("sDateHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, "Date"), string.Empty, string.Empty, null, null, null));
            int prevMonth = this.ReportDates[0].Month;
            string prevMonthDisplay = this.ReportDates[0].ToString("MMMM");
            int monthSpanCount = 0;
            foreach (DateTime spotDate in this.ReportDates)
            {
                dateHeaderRow.Cells.Add(new ExcelXml.Cell("sDateHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, spotDate.ToShortDateString()), string.Empty, string.Empty, null, null, null));
                if (prevMonth == spotDate.Month)
                {
                    monthSpanCount++;
                }
                else
                {
                    monthHeaderRow.Cells.Add(new ExcelXml.Cell("sMonthHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, prevMonthDisplay), string.Empty, null, null, Convert.ToUInt32(monthSpanCount - 1), null));
                    monthSpanCount = 1;
                }
                prevMonthDisplay = spotDate.ToString("MMMM");
                prevMonth = spotDate.Month;
            }
            monthHeaderRow.Cells.Add(new ExcelXml.Cell("sMonthHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, prevMonthDisplay), string.Empty, null, null, Convert.ToUInt32(monthSpanCount - 1), null));
            this.MonthDateHeaderRows = new List<ExcelXml.Row>();
            this.MonthDateHeaderRows.Add(monthHeaderRow);
            this.MonthDateHeaderRows.Add(dateHeaderRow);
        }
        #endregion

        #region InitializeReportColumns method
        /// <summary>TBD</summary>
        public void InitializeReportColumns()
        {
            this.ReportColumns = new List<ExcelXml.Column>();
            this.ReportColumns.Add(new ExcelXml.Column(true, string.Empty, false, null, null, string.Empty, 125));
            DateTime reportStartDate = FindFirstMonday(new DateTime(this.StartYear, this.StartMonth, 1));
            DateTime reportEndDate = new DateTime(this.StartYear, this.StartMonth, 1);
            reportEndDate = reportEndDate.AddMonths(this.NumberOfMonths);
            DateTime currentDate = new DateTime(reportStartDate.Year, reportStartDate.Month, reportStartDate.Day);
            this.ReportDates = new List<DateTime>();
            while (currentDate < reportEndDate)
            {
                this.ReportColumns.Add(new ExcelXml.Column(false, string.Empty, false, null, null, "", 100));
                this.ReportDates.Add(currentDate);
                currentDate = currentDate.AddDays(7);
            }
        }
        #endregion

        #region InitializeReportStyles method
        /// <summary>TBD</summary>
        public void InitializeReportStyles()
        {
            this.ReportStyles = new List<ExcelXml.Style>();
            this.ReportStyles.Add(new ExcelXml.Style("Default", "Normal"
            , new ExcelXml.Alignment(null, ExcelXml.Vertical.Center, null, null, null, null, null)
            , new ExcelXml.Font(null, string.Empty, "Arial", null, null, null, null, null, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty));
            this.ReportStyles.Add(new ExcelXml.Style("sStationName", "StationName"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(true, "#00CCFF", "Arial", false, false, false, 18, false, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sStationDescription", "StationDescription"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 12, false, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sStationSpotName", "StationSpotName"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, false)
            , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 10, false, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sSpotEntry", "SpotEntry"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 10, false, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sSpotDivider", "SpotDivider"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, null)
            , new ExcelXml.Font(false, string.Empty, "Arial", null, null, null, 14, null, null, null, null, null)
            , new ExcelXml.Interior("#999999"), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sDateHeader", "DateHeader"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, null)
            , new ExcelXml.Font(true, string.Empty, "Arial", null, null, null, 10, null, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sMonthHeader", "MonthHeader"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, null)
            , new ExcelXml.Font(true, string.Empty, "Arial", null, null, null, 12, null, null, null, null, null)
            , new ExcelXml.Interior("#CCFFFF"), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sSpotTypeBreakdownTotal", "SpotTypeBreakdownTotal"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, true, null, null, true)
            , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 10, false, null, null, null, null)
            , new ExcelXml.Interior("#999999"), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            //create the styles for the spot types in this market
            foreach (SpotType spotType in this.MarketSpotTypes)
            {
                this.ReportStyles.Add(new ExcelXml.Style(string.Format("sSpotTypeId_{0}", spotType.SpotTypeId), string.Format("SpotTypeId_{0}", spotType.SpotTypeId)
                , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, true, null, null, true)
                , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 10, false, null, null, null, null)
                , new ExcelXml.Interior(string.Format("#{0}", spotType.Color)), null, string.Empty
                , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
                this.ReportStyles.Add(new ExcelXml.Style(string.Format("sSpotTypeId_{0}_Percentage", spotType.SpotTypeId), string.Format("SpotTypeId_{0}_Percentage", spotType.SpotTypeId)
                , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, true, null, null, true)
                , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 10, false, null, null, null, null)
                , new ExcelXml.Interior(string.Format("#{0}", spotType.Color)), ExcelXml.Format.Percent, ExcelXml.NumberFormatStrings.NUMBER_FORMAT_PERCENT
                , new ExcelXml.Border(ExcelXml.Position.All, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            }
        }
        #endregion

        #region Market property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Market { get; set; }
        #endregion

        #region MarketSpotMatrix property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<Spot> MarketSpotMatrix { get; set; }
        #endregion

        #region MarketSpotTypeBreakdown property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<SpotTypeBreakdown> MarketSpotTypeBreakdown { get; set; }
        #endregion

        #region MarketSpotTypes property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<SpotType> MarketSpotTypes { get; set; }
        #endregion

        #region MarketStations property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<Station> MarketStations { get; set; }
        #endregion

        #region MarketStationSpots property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<StationSpot> MarketStationSpots { get; set; }
        #endregion

        #region MonthDateHeaderRows property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<ExcelXml.Row> MonthDateHeaderRows { get; set; }
        #endregion

        #region NumberOfMonths property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int NumberOfMonths { get; set; }
        #endregion

        #region ReportColumns property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<ExcelXml.Column> ReportColumns { get; set; }
        #endregion

        #region ReportDates property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<DateTime> ReportDates { get; set; }
        #endregion

        #region ReportStationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int ReportStationId { get; set; }
        #endregion

        #region ReportStyles property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<ExcelXml.Style> ReportStyles { get; set; }
        #endregion

        #region SpotDividerRow property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public ExcelXml.Row SpotDividerRow
        {
            get
            {
                ExcelXml.Row dividerRow = new ExcelXml.Row(null, 15, false, null, null, string.Empty);
                dividerRow.Cells.Add(new ExcelXml.Cell("sSpotDivider", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Empty), string.Empty, string.Empty, null, null, null));
                for (int i = 0; i < this.ReportDates.Count; i++)
                {
                    dividerRow.Cells.Add(new ExcelXml.Cell("sSpotDivider", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Empty), string.Empty, string.Empty, null, null, null));
                }
                return dividerRow;
            }
        }
        #endregion

        #region StartMonth property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StartMonth { get; set; }
        #endregion

        #region StartYear property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StartYear { get; set; }
        #endregion

        #region WantSingleStation property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool WantSingleStation { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class MarketUtilizationReport
    {

        #region Member variables
        /// <summary>TBD</summary>
        private ExcelXml.Row HeaderRow;
        /// <summary>TBD</summary>
        private List<ExcelXml.Column> ReportColumns;
        /// <summary>TBD</summary>
        private List<ExcelXml.Style> ReportStyles;
        #endregion

        #region Parameterized constructor (reportMarket, reportYear, includeGuaranteedBonus, includeSpaceAvailBonus)
        /// <summary>TBD</summary>
        /// <param name="reportMarket">TBD</param>
        /// <param name="reportYear">TBD</param>
        /// <param name="includeGuaranteedBonus">TBD</param>
        /// <param name="includeSpaceAvailBonus">TBD</param>
        public MarketUtilizationReport(string reportMarket, int reportYear, bool includeGuaranteedBonus, bool includeSpaceAvailBonus)
        {
            this.ReportMarket = reportMarket;
            this.ReportYear = reportYear;
            this.IncludeGuaranteedBonus = includeGuaranteedBonus;
            this.IncludeSpaceAvailBonus = includeSpaceAvailBonus;
            this.GetReportData();
            this.GenerateReport();
        }
        #endregion

        #region GenerateExcelXml method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public XmlDocument GenerateExcelXml()
        {
            return this.ReportWorkbook.GenerateExcelXml();
        }
        #endregion

        #region GenerateReport method
        /// <summary>TBD</summary>
        private void GenerateReport()
        {
            InitializeReportStyles();
            InitializeReportColumns();
            InitializeHeaderRows();
            this.ReportWorkbook = new ExcelXml.Workbook();
            this.ReportWorkbook.Styles = this.ReportStyles;
            ExcelXml.Worksheet yearWorksheet = GenerateYearWorksheet(this.ReportYear);
            yearWorksheet.Table.Rows.Add(this.HeaderRow);
            foreach (MarketUtilizationReportItem reportItem in this.ReportItems)
            {
                yearWorksheet.Table.Rows.Add(reportItem.GenerateReportItemRow("sDataCell", "Default", "sDataCellPercent"));
            }
            yearWorksheet.Table.Rows.Add(GenerateTotalRow());
            this.ReportWorkbook.Worksheets.Add(yearWorksheet);
        }
        #endregion

        #region GenerateTotalRow method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private ExcelXml.Row GenerateTotalRow()
        {
            ExcelXml.Row totalRow = new ExcelXml.Row(null, 20, null, null, null, string.Empty);
            totalRow.Cells.Add(new ExcelXml.Cell("sTotalCell", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, "Total"), string.Empty, string.Empty, null, null, null));
            int stationCount = this.ReportItems.Count;
            for (var i = 0; i < 13; i++)
            {
                totalRow.Cells.Add(new ExcelXml.Cell("sTotalCell", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Empty), string.Format("=SUM(R[-{0}]C:R[-1]C)", stationCount), string.Empty, null, null, null));
                totalRow.Cells.Add(new ExcelXml.Cell("sTotalCell", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Empty), string.Format("=SUM(R[-{0}]C:R[-1]C)", stationCount), string.Empty, null, null, null));
                totalRow.Cells.Add(new ExcelXml.Cell("sTotalCellPercent", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Empty), "=IF(RC[-2]=0,0,RC[-2]/RC[-1])", string.Empty, null, null, null));
            }
            return totalRow;
        }
        #endregion

        #region GenerateYearWorksheet method
        /// <summary>TBD</summary>
        /// <param name="year">TBD</param>
        /// <returns>TBD</returns>
        private ExcelXml.Worksheet GenerateYearWorksheet(int year)
        {
            ExcelXml.Worksheet yearWorksheet = new ExcelXml.Worksheet(Convert.ToString(year));
            yearWorksheet.Table.Columns = this.ReportColumns;
            ExcelXml.PageSetup pageSetup = new ExcelXml.PageSetup();
            pageSetup.Header.Margin = 0.3;
            pageSetup.Footer.Margin = 0.3;
            pageSetup.PageMargins.Bottom = 0.25;
            pageSetup.PageMargins.Left = 0.25;
            pageSetup.PageMargins.Right = 0.25;
            pageSetup.PageMargins.Top = 0.25;
            pageSetup.Layout.Orientation = ExcelXml.Orientation.Landscape;
            pageSetup.Layout.CenterHorizontal = true;
            pageSetup.Layout.CenterVertical = true;
            yearWorksheet.WorksheetOptions = new ExcelXml.WorksheetOptions(pageSetup, new ExcelXml.Print(null, null, null, null, null, true), false, 75, null, false
            , 1, 1, new ExcelXml.Pane(3), new ExcelXml.Pane(1), new ExcelXml.Pane(2), new ExcelXml.Pane(0, 2, 7));
            return yearWorksheet;
        }
        #endregion

        #region GetReportData method
        /// <summary>TBD</summary>
        public void GetReportData()
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<SqlParameter>();
            spParams.Add(Param.CreateParam("market", SqlDbType.VarChar, this.ReportMarket));
            spParams.Add(Param.CreateParam("year", SqlDbType.Int, this.ReportYear));
            spParams.Add(Param.CreateParam("includeGuaranteedBonus", SqlDbType.Bit, this.IncludeGuaranteedBonus));
            spParams.Add(Param.CreateParam("includeSpaceAvailBonus", SqlDbType.Bit, this.IncludeSpaceAvailBonus));
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                this.ReportItems = io.RetrieveEntitiesFromCommand<MarketUtilizationReportItem>(IO.CreateCommandFromStoredProc("DigitalAvails_Reporting_MarketUtilization", spParams));
            }
        }
        #endregion

        #region IncludeGuaranteedBonus property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool IncludeGuaranteedBonus { get; set; }
        #endregion

        #region IncludeSpaceAvailBonus property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool IncludeSpaceAvailBonus { get; set; }
        #endregion

        #region InitializeHeaderRows method
        /// <summary>TBD</summary>
        private void InitializeHeaderRows()
        {
            this.HeaderRow = new ExcelXml.Row(null, 20, null, null, null, string.Empty);
            this.HeaderRow.Cells.Add(new ExcelXml.Cell("sHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, "Station"), string.Empty, string.Empty, null, null, null));
            DateTime startDate = new DateTime(this.ReportYear, 1, 1);
            for (int i = 0; i < 12; i++)
            {
                this.HeaderRow.Cells.Add(new ExcelXml.Cell("sHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Format("{0:MMM} {1} Utilized", startDate, this.ReportYear)), string.Empty, string.Empty, null, null, null));
                this.HeaderRow.Cells.Add(new ExcelXml.Cell("sHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Format("{0:MMM} {1} Available", startDate, this.ReportYear)), string.Empty, string.Empty, null, null, null));
                this.HeaderRow.Cells.Add(new ExcelXml.Cell("sHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Format("{0:MMM} {1} Utilization %", startDate, this.ReportYear)), string.Empty, string.Empty, null, null, null));
                startDate = startDate.AddMonths(1);
            }
            this.HeaderRow.Cells.Add(new ExcelXml.Cell("sHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Format("Full Year {0} Utilization", this.ReportYear)), string.Empty, string.Empty, null, null, null));
            this.HeaderRow.Cells.Add(new ExcelXml.Cell("sHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Format("Full Year {0} Availability", this.ReportYear)), string.Empty, string.Empty, null, null, null));
            this.HeaderRow.Cells.Add(new ExcelXml.Cell("sHeader", string.Empty, new ExcelXml.Data(ExcelXml.Type.String, string.Format("Full Year {0} Utilization %", this.ReportYear)), string.Empty, string.Empty, null, null, null));
        }
        #endregion

        #region InitializeReportColumns method
        /// <summary>TBD</summary>
        private void InitializeReportColumns()
        {
            this.ReportColumns = new List<ExcelXml.Column>();
            this.ReportColumns.Add(new ExcelXml.Column(false, string.Empty, false, null, null, "", 100));
            for (int i = 0; i < 36; i++)
            {
                this.ReportColumns.Add(new ExcelXml.Column(false, string.Empty, false, null, null, "", 125));
            }
        }
        #endregion

        #region InitializeReportStyles method
        /// <summary>TBD</summary>
        private void InitializeReportStyles()
        {
            this.ReportStyles = new List<ExcelXml.Style>();
            this.ReportStyles.Add(new ExcelXml.Style("Default", "Normal"
            , new ExcelXml.Alignment(null, ExcelXml.Vertical.Center, null, null, null, null, null)
            , new ExcelXml.Font(null, string.Empty, "Arial", null, null, null, null, null, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty));
            this.ReportStyles.Add(new ExcelXml.Style("sHeader", "Header"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Center, ExcelXml.Vertical.Center, null, null, null, null, null)
            , new ExcelXml.Font(true, string.Empty, "Arial", null, null, null, 10, null, null, null, null, null)
            , new ExcelXml.Interior(), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.Bottom, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sDataCell", "DataCell"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Left, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(null, string.Empty, "Arial", false, false, false, 11, false, null, null, null, ExcelXml.Family.Swiss)
            , new ExcelXml.Interior(), null, string.Empty));
            this.ReportStyles.Add(new ExcelXml.Style("sDataCellPercent", "DataCellPercent"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Right, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(null, string.Empty, "Arial", false, false, false, 11, false, null, null, null, ExcelXml.Family.Swiss)
            , new ExcelXml.Interior(), ExcelXml.Format.Percent, ExcelXml.NumberFormatStrings.NUMBER_FORMAT_PERCENT));
            this.ReportStyles.Add(new ExcelXml.Style("sDataCellNumeric", "DataCellNumeric"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Right, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(null, string.Empty, "Arial", false, false, false, 11, false, null, null, null, ExcelXml.Family.Swiss)
            , new ExcelXml.Interior(), ExcelXml.Format.GeneralNumber, ExcelXml.NumberFormatStrings.NUMBER_FORMAT_COMMA));
            this.ReportStyles.Add(new ExcelXml.Style("sTotalCell", "TotalCell"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Right, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 11, false, null, null, null, ExcelXml.Family.Swiss)
            , new ExcelXml.Interior(), null, string.Empty
            , new ExcelXml.Border(ExcelXml.Position.Top, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
            this.ReportStyles.Add(new ExcelXml.Style("sTotalCellPercent", "TotalCellPercent"
            , new ExcelXml.Alignment(ExcelXml.Horizontal.Right, ExcelXml.Vertical.Center, null, null, null, null, true)
            , new ExcelXml.Font(true, string.Empty, "Arial", false, false, false, 11, false, null, null, null, ExcelXml.Family.Swiss)
            , new ExcelXml.Interior(), ExcelXml.Format.Percent, ExcelXml.NumberFormatStrings.NUMBER_FORMAT_PERCENT
            , new ExcelXml.Border(ExcelXml.Position.Top, string.Empty, ExcelXml.LineStyle.Continuous, 1)));
        }
        #endregion

        #region ReportItems property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<MarketUtilizationReportItem> ReportItems { get; private set; }
        #endregion

        #region ReportMarket property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ReportMarket { get; set; }
        #endregion

        #region ReportWorkbook property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public ExcelXml.Workbook ReportWorkbook { get; private set; }
        #endregion

        #region ReportYear property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int ReportYear { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class MarketUtilizationReportItem
    {

        #region AprAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Apr Available")]
        public int AprAvailable { get; set; }
        #endregion

        #region AprUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Apr Utilization %")]
        public decimal AprUtilizationPercentage { get; set; }
        #endregion

        #region AprUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Apr Utilized")]
        public int AprUtilized { get; set; }
        #endregion

        #region AugAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Aug Available")]
        public int AugAvailable { get; set; }
        #endregion

        #region AugUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Aug Utilization %")]
        public decimal AugUtilizationPercentage { get; set; }
        #endregion

        #region AugUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Aug Utilized")]
        public int AugUtilized { get; set; }
        #endregion

        #region DecAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Dec Available")]
        public int DecAvailable { get; set; }
        #endregion

        #region DecUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Dec Utilization %")]
        public decimal DecUtilizationPercentage { get; set; }
        #endregion

        #region DecUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Dec Utilized")]
        public int DecUtilized { get; set; }
        #endregion

        #region FebAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Feb Available")]
        public int FebAvailable { get; set; }
        #endregion

        #region FebUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Feb Utilization %")]
        public decimal FebUtilizationPercentage { get; set; }
        #endregion

        #region FebUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Feb Utilized")]
        public int FebUtilized { get; set; }
        #endregion

        #region FullYearAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Full Year Availability")]
        public int FullYearAvailable { get; set; }
        #endregion

        #region FullYearUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Full Year Utilization %")]
        public decimal FullYearUtilizationPercentage { get; set; }
        #endregion

        #region FullYearUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Full Year Utilization")]
        public int FullYearUtilized { get; set; }
        #endregion

        #region GenerateReportItemRow method
        /// <summary>TBD</summary>
        /// <param name="stationStyleId">TBD</param>
        /// <param name="numericStyleId">TBD</param>
        /// <param name="percentageStyleId">TBD</param>
        /// <returns>TBD</returns>
        public ExcelXml.Row GenerateReportItemRow(string stationStyleId, string numericStyleId, string percentageStyleId)
        {
            List<ExcelXml.Cell> reportItemCells = new List<ExcelXml.Cell>();
            reportItemCells.Add(new ExcelXml.Cell(stationStyleId, string.Empty, new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(this.Station)), string.Empty, string.Empty, null, null, null));
            DateTime startDate = new DateTime(2000, 1, 1);
            for (var i = 0; i < 12; i++)
            {
                reportItemCells.Add(new ExcelXml.Cell(numericStyleId, string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, Convert.ToString(this.GetType().GetProperties().Single(p => p.Name == string.Format("{0:MMM}Utilized", startDate)).GetValue(this, null))), string.Empty, string.Empty, null, null, null));
                reportItemCells.Add(new ExcelXml.Cell(numericStyleId, string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, Convert.ToString(this.GetType().GetProperties().Single(p => p.Name == string.Format("{0:MMM}Available", startDate)).GetValue(this, null))), string.Empty, string.Empty, null, null, null));
                reportItemCells.Add(new ExcelXml.Cell(percentageStyleId, string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, Convert.ToString(this.GetType().GetProperties().Single(p => p.Name == string.Format("{0:MMM}UtilizationPercentage", startDate)).GetValue(this, null))), string.Empty, string.Empty, null, null, null));
                startDate = startDate.AddMonths(1);
            }
            //Full Year
            reportItemCells.Add(new ExcelXml.Cell(numericStyleId, string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, Convert.ToString(this.FullYearUtilized)), string.Empty, string.Empty, null, null, null));
            reportItemCells.Add(new ExcelXml.Cell(numericStyleId, string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, Convert.ToString(this.FullYearAvailable)), string.Empty, string.Empty, null, null, null));
            reportItemCells.Add(new ExcelXml.Cell(percentageStyleId, string.Empty, new ExcelXml.Data(ExcelXml.Type.Number, Convert.ToString(this.FullYearUtilizationPercentage)), string.Empty, string.Empty, null, null, null));
            return new ExcelXml.Row(true, null, false, null, null, null, reportItemCells.ToArray());
        }
        #endregion

        #region JanAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jan Available")]
        public int JanAvailable { get; set; }
        #endregion

        #region JanUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jan Utilization %")]
        public decimal JanUtilizationPercentage { get; set; }
        #endregion

        #region JanUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jan Utilized")]
        public int JanUtilized { get; set; }
        #endregion

        #region JulAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jul Available")]
        public int JulAvailable { get; set; }
        #endregion

        #region JulUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jul Utilization %")]
        public decimal JulUtilizationPercentage { get; set; }
        #endregion

        #region JulUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jul Utilized")]
        public int JulUtilized { get; set; }
        #endregion

        #region JunAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jun Available")]
        public int JunAvailable { get; set; }
        #endregion

        #region JunUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jun Utilization %")]
        public decimal JunUtilizationPercentage { get; set; }
        #endregion

        #region JunUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Jun Utilized")]
        public int JunUtilized { get; set; }
        #endregion

        #region MarAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Mar Available")]
        public int MarAvailable { get; set; }
        #endregion

        #region MarUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Mar Utilization %")]
        public decimal MarUtilizationPercentage { get; set; }
        #endregion

        #region MarUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Mar Utilized")]
        public int MarUtilized { get; set; }
        #endregion

        #region MayAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("May Available")]
        public int MayAvailable { get; set; }
        #endregion

        #region MayUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("May Utilization %")]
        public decimal MayUtilizationPercentage { get; set; }
        #endregion

        #region MayUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("May Utilized")]
        public int MayUtilized { get; set; }
        #endregion

        #region NovAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Nov Available")]
        public int NovAvailable { get; set; }
        #endregion

        #region NovUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Nov Utilization %")]
        public decimal NovUtilizationPercentage { get; set; }
        #endregion

        #region NovUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Nov Utilized")]
        public int NovUtilized { get; set; }
        #endregion

        #region OctAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Oct Available")]
        public int OctAvailable { get; set; }
        #endregion

        #region OctUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Oct Utilization %")]
        public decimal OctUtilizationPercentage { get; set; }
        #endregion

        #region OctUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Oct Utilized")]
        public int OctUtilized { get; set; }
        #endregion

        #region SepAvailable property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Sep Available")]
        public int SepAvailable { get; set; }
        #endregion

        #region SepUtilizationPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Sep Utilization %")]
        public decimal SepUtilizationPercentage { get; set; }
        #endregion

        #region SepUtilized property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Column("Sep Utilized")]
        public int SepUtilized { get; set; }
        #endregion

        #region Station property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Station { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class Spot
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public Spot()
        {
        }
        #endregion

        #region Parameterized constructor (spotId)
        /// <summary>TBD</summary>
        /// <param name="spotId">TBD</param>
        public Spot(int spotId)
        {
            this.SpotId = spotId;
            this.Load();
        }
        #endregion

        #region Parameterized constructor (spotId, stationId, stationSpotId, spotTypeId, color, spotDate, year, month, week, campaignName, campaignNumber, description, enteredBy, enteredOn, lastUpdatedBy, lastUpdatedOn)
        /// <summary>TBD</summary>
        /// <param name="spotId">TBD</param>
        /// <param name="stationId">TBD</param>
        /// <param name="stationSpotId">TBD</param>
        /// <param name="spotTypeId">TBD</param>
        /// <param name="color">TBD</param>
        /// <param name="spotDate">TBD</param>
        /// <param name="year">TBD</param>
        /// <param name="month">TBD</param>
        /// <param name="week">TBD</param>
        /// <param name="campaignName">TBD</param>
        /// <param name="campaignNumber">TBD</param>
        /// <param name="description">TBD</param>
        /// <param name="enteredBy">TBD</param>
        /// <param name="enteredOn">TBD</param>
        /// <param name="lastUpdatedBy">TBD</param>
        /// <param name="lastUpdatedOn">TBD</param>
        public Spot(int spotId, int stationId, int stationSpotId, int spotTypeId, string color, DateTime spotDate, int year, int month, int week, string campaignName, string campaignNumber, string description, string enteredBy, DateTime enteredOn, string lastUpdatedBy, DateTime lastUpdatedOn)
        {
            this.SpotId = spotId;
            this.StationId = stationId;
            this.StationSpotId = stationSpotId;
            this.SpotTypeId = spotTypeId;
            this.Color = color;
            this.SpotDate = spotDate;
            this.Year = year;
            this.Month = month;
            this.Week = week;
            this.CampaignName = campaignName;
            this.CampaignNumber = campaignNumber;
            this.Description = description;
            this.EnteredBy = enteredBy;
            this.EnteredOn = enteredOn;
            this.LastUpdatedBy = lastUpdatedBy;
            this.LastUpdatedOn = lastUpdatedOn;
        }
        #endregion

        #region Parameterized constructor (stationId, stationSpotId, spotTypeId, spotDate, year, month, week, campaignName, campaignNumber, description)
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        /// <param name="stationSpotId">TBD</param>
        /// <param name="spotTypeId">TBD</param>
        /// <param name="spotDate">TBD</param>
        /// <param name="year">TBD</param>
        /// <param name="month">TBD</param>
        /// <param name="week">TBD</param>
        /// <param name="campaignName">TBD</param>
        /// <param name="campaignNumber">TBD</param>
        /// <param name="description">TBD</param>
        public Spot(int stationId, int stationSpotId, int spotTypeId, DateTime spotDate, int year, int month, int week, string campaignName, string campaignNumber, string description)
        {
            this.StationId = stationId;
            this.StationSpotId = stationSpotId;
            this.SpotTypeId = spotTypeId;
            this.SpotDate = spotDate;
            this.Year = year;
            this.Month = month;
            this.Week = week;
            this.CampaignName = campaignName;
            this.CampaignNumber = campaignNumber;
            this.Description = description;
            this.EnteredBy = Security.GetCurrentUserId;
            this.EnteredOn = DateTime.Now;
            this.LastUpdatedBy = Security.GetCurrentUserId;
            this.LastUpdatedOn = DateTime.Now;
        }
        #endregion

        #region AddWeek method
        /// <summary>TBD</summary>
        public void AddWeek()
        {
            int prevMonth = this.Month;
            this.SpotDate = this.SpotDate.AddDays(7);
            this.Month = this.SpotDate.Month;
            this.Year = this.SpotDate.Year;
            this.Week = (this.Month == prevMonth ? (this.Week + 1) : 1);
        }
        #endregion

        #region CampaignName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string CampaignName { get; set; }
        #endregion

        #region CampaignNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string CampaignNumber { get; set; }
        #endregion

        #region Color property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Color { get; set; }
        #endregion

        #region DeleteSpot method
        /// <summary>TBD</summary>
        /// <param name="spotId">TBD</param>
        public static void DeleteSpot(int spotId)
        {
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("DigitalAvails_DeleteSpot", Param.CreateParam("SPOTID", SqlDbType.Int, spotId)));
            }
        }
        #endregion

        #region Description property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Description { get; set; }
        #endregion

        #region EnteredBy property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string EnteredBy { get; set; }
        #endregion

        #region EnteredOn property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime EnteredOn { get; set; }
        #endregion

        #region GetCell method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public ExcelXml.Cell GetCell()
        {
            ExcelXml.Cell cell = new ExcelXml.Cell("", "", new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(this.CampaignName)), "", "", null, null, null);
            if (!String.IsNullOrEmpty(this.Description))
            {
                cell.Comment = new ExcelXml.Comment(new ExcelXml.Data(ExcelXml.Type.String, HttpUtility.HtmlEncode(this.Description), true), this.EnteredBy);
            }
            return cell;
        }
        #endregion

        #region LastUpdatedBy property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string LastUpdatedBy { get; set; }
        #endregion

        #region LastUpdatedOn property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime LastUpdatedOn { get; set; }
        #endregion

        #region Load method
        /// <summary>TBD</summary>
        public void Load()
        {
        }
        #endregion

        #region LoadSpotMatrix method (stationId, startMonth, startYear, numberOfMonths)
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        /// <param name="startMonth">TBD</param>
        /// <param name="startYear">TBD</param>
        /// <param name="numberOfMonths">TBD</param>
        /// <returns>TBD</returns>
        public static List<Spot> LoadSpotMatrix(int stationId, int startMonth, int startYear, int numberOfMonths)
        {
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                return io.RetrieveEntitiesFromCommand<Spot>(IO.CreateCommandFromStoredProc("DIGITALAVAILS_GETSPOTMATRIX",
                Param.CreateParam("STATIONID", SqlDbType.Int, stationId),
                Param.CreateParam("STARTMONTH", SqlDbType.Int, startMonth),
                Param.CreateParam("STARTYEAR", SqlDbType.Int, startYear),
                Param.CreateParam("NUMBEROFMONTHS", SqlDbType.Int, numberOfMonths)));
            }
        }
        #endregion

        #region LoadSpotMatrix method (spotMatrixData)
        /// <summary>TBD</summary>
        /// <param name="spotMatrixData">TBD</param>
        /// <returns>TBD</returns>
        public static List<Spot> LoadSpotMatrix(DataTable spotMatrixData)
        {
            List<Spot> spotMatrix = new List<Spot>();
            foreach (DataRow spotRow in spotMatrixData.Rows)
            {
                spotMatrix.Add(new Spot((int)spotRow["SpotId"]
                , (int)spotRow["StationId"]
                , (int)spotRow["StationSpotId"]
                , (int)spotRow["SpotTypeId"]
                , (string)spotRow["Color"]
                , (DateTime)spotRow["SpotDate"]
                , (int)spotRow["Year"]
                , (int)spotRow["Month"]
                , (int)spotRow["Week"]
                , (string)spotRow["CampaignName"]
                , Convert.ToString(IO.GetDataRowValue(spotRow, "CampaignNumber", string.Empty))
                , Convert.ToString(IO.GetDataRowValue(spotRow, "Description", string.Empty))
                , (string)spotRow["EnteredBy"]
                , (DateTime)spotRow["EnteredOn"]
                , (string)spotRow["LastUpdatedBy"]
                , (DateTime)spotRow["LastUpdatedOn"]
                ));
            }
            return spotMatrix;
        }
        #endregion

        #region LoadSpotMatrixForMarket method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static List<Spot> LoadSpotMatrixForMarket()
        {
            return null;
        }
        #endregion

        #region Month property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int Month { get; set; }
        #endregion

        #region Save method
        /// <summary>TBD</summary>
        public void Save()
        {
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc("DigitalAvails_AddSpot"))
                {
                    cmd.Parameters.Add(Param.CreateParam("STATIONID", SqlDbType.Int, StationId));
                    cmd.Parameters.Add(Param.CreateParam("STATIONSPOTID", SqlDbType.Int, StationSpotId));
                    cmd.Parameters.Add(Param.CreateParam("SPOTTYPEID", SqlDbType.Int, SpotTypeId));
                    cmd.Parameters.Add(Param.CreateParam("SPOTDATE", SqlDbType.Date, SpotDate));
                    cmd.Parameters.Add(Param.CreateParam("YEAR", SqlDbType.Int, Year));
                    cmd.Parameters.Add(Param.CreateParam("MONTH", SqlDbType.Int, Month));
                    cmd.Parameters.Add(Param.CreateParam("WEEK", SqlDbType.Int, Week));
                    cmd.Parameters.Add(Param.CreateParam("CAMPAIGNNAME", SqlDbType.VarChar, CampaignName.Trim()));
                    if (!String.IsNullOrEmpty(Description.Trim()))
                    {
                        cmd.Parameters.Add(Param.CreateParam("DESCRIPTION", SqlDbType.VarChar, Description.Trim()));
                    }
                    if (!String.IsNullOrEmpty(CampaignNumber.Trim()))
                    {
                        cmd.Parameters.Add(Param.CreateParam("CAMPAIGNNUMBER", SqlDbType.VarChar, CampaignNumber));
                    }
                    cmd.Parameters.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, EnteredBy));
                    cmd.Parameters.Add(Param.CreateParam("ENTEREDON", SqlDbType.DateTime, EnteredOn));
                    cmd.Parameters.Add(Param.CreateParam("LASTUPDATEDBY", SqlDbType.VarChar, LastUpdatedBy));
                    cmd.Parameters.Add(Param.CreateParam("LASTUPDATEDON", SqlDbType.DateTime, LastUpdatedOn));
                    int result = Convert.ToInt32(io.ExecuteScalarQuery(cmd));
                    if (result < 0)
                    {
                        throw new SpotConflictException(string.Format("A Spot Conflict Exception occurred. Spot Date: {0}. Result: {1}.", this.SpotDate.ToShortDateString(), result));
                    }
                    this.SpotId = result;
                }
            }
        }
        #endregion

        #region SaveMultiStationSpot method
        /// <summary>TBD</summary>
        /// <param name="spot">TBD</param>
        /// <param name="stations">TBD</param>
        /// <returns>TBD</returns>
        public static List<SpotConflict> SaveMultiStationSpot(Spot spot, DataTable stations)
        {
            DataSet conflicts;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc("DigitalAvails_AddSpotToMultipleStations"))
                {
                    cmd.Parameters.Add(Param.CreateParam("STATIONIDS", SqlDbType.Structured, stations));
                    cmd.Parameters.Add(Param.CreateParam("STATIONSPOTID", SqlDbType.Int, spot.StationSpotId));
                    cmd.Parameters.Add(Param.CreateParam("SPOTTYPEID", SqlDbType.Int, spot.SpotTypeId));
                    cmd.Parameters.Add(Param.CreateParam("SPOTDATE", SqlDbType.Date, spot.SpotDate));
                    cmd.Parameters.Add(Param.CreateParam("YEAR", SqlDbType.Int, spot.Year));
                    cmd.Parameters.Add(Param.CreateParam("MONTH", SqlDbType.Int, spot.Month));
                    cmd.Parameters.Add(Param.CreateParam("WEEK", SqlDbType.Int, spot.Week));
                    cmd.Parameters.Add(Param.CreateParam("CAMPAIGNNAME", SqlDbType.VarChar, spot.CampaignName.Trim()));
                    if (!String.IsNullOrEmpty(spot.Description.Trim()))
                    {
                        cmd.Parameters.Add(Param.CreateParam("DESCRIPTION", SqlDbType.VarChar, spot.Description.Trim()));
                    }
                    if (!String.IsNullOrEmpty(spot.CampaignNumber.Trim()))
                    {
                        cmd.Parameters.Add(Param.CreateParam("CAMPAIGNNUMBER", SqlDbType.VarChar, spot.CampaignNumber));
                    }
                    cmd.Parameters.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, spot.EnteredBy));
                    cmd.Parameters.Add(Param.CreateParam("ENTEREDON", SqlDbType.DateTime, spot.EnteredOn));
                    cmd.Parameters.Add(Param.CreateParam("LASTUPDATEDBY", SqlDbType.VarChar, spot.LastUpdatedBy));
                    cmd.Parameters.Add(Param.CreateParam("LASTUPDATEDON", SqlDbType.DateTime, spot.LastUpdatedOn));
                    conflicts = io.ExecuteDataSetQuery(cmd);
                }
            }
            return SpotConflict.GetMultiStationSpotConflicts(conflicts);
        }
        #endregion

        #region SpotDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime SpotDate { get; set; }
        #endregion

        #region SpotId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int SpotId { get; set; }
        #endregion

        #region SpotTypeId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int SpotTypeId { get; set; }
        #endregion

        #region StationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationId { get; set; }
        #endregion

        #region StationSpotId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationSpotId { get; set; }
        #endregion

        #region Update method
        /// <summary>TBD</summary>
        public void Update()
        {
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc("DigitalAvails_UpdateSpot"))
                {
                    cmd.Parameters.Add(Param.CreateParam("SPOTID", SqlDbType.Int, SpotId));
                    cmd.Parameters.Add(Param.CreateParam("SPOTTYPEID", SqlDbType.Int, SpotTypeId));
                    cmd.Parameters.Add(Param.CreateParam("CAMPAIGNNAME", SqlDbType.VarChar, CampaignName.Trim()));
                    if (!String.IsNullOrEmpty(Description.Trim()))
                    {
                        cmd.Parameters.Add(Param.CreateParam("DESCRIPTION", SqlDbType.VarChar, Description.Trim()));
                    }
                    cmd.Parameters.Add(Param.CreateParam("LASTUPDATEDBY", SqlDbType.VarChar, LastUpdatedBy));
                    cmd.Parameters.Add(Param.CreateParam("LASTUPDATEDON", SqlDbType.DateTime, LastUpdatedOn));
                    io.ExecuteActionQuery(cmd);
                }
            }
        }
        #endregion

        #region Week property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int Week { get; set; }
        #endregion

        #region Year property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int Year { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class SpotCampaignDetail
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public SpotCampaignDetail()
        {
        }
        #endregion

        #region Parameterized constructor (campaignName, campaignNumber, startDate, endDate, totalWeeks, weeksBefore, weeksAfter)
        /// <summary>TBD</summary>
        /// <param name="campaignName">TBD</param>
        /// <param name="campaignNumber">TBD</param>
        /// <param name="startDate">TBD</param>
        /// <param name="endDate">TBD</param>
        /// <param name="totalWeeks">TBD</param>
        /// <param name="weeksBefore">TBD</param>
        /// <param name="weeksAfter">TBD</param>
        public SpotCampaignDetail(string campaignName, string campaignNumber
        , DateTime startDate, DateTime endDate, int totalWeeks, int weeksBefore, int weeksAfter)
        {
            this.CampaignName = campaignName;
            this.CampaignNumber = campaignNumber;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.TotalWeeks = totalWeeks;
            this.WeeksBefore = weeksBefore;
            this.WeeksAfter = weeksAfter;
        }
        #endregion

        #region CampaignName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string CampaignName { get; set; }
        #endregion

        #region CampaignNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string CampaignNumber { get; set; }
        #endregion

        #region EndDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime EndDate { get; set; }
        #endregion

        #region GetIdListFromDBString method
        /// <summary>TBD</summary>
        /// <param name="idList">TBD</param>
        /// <returns>TBD</returns>
        private static List<int> GetIdListFromDBString(string idList)
        {
            return Array.ConvertAll<string, int>(idList.Split(','), new Converter<string, int>(Convert.ToInt32)).ToList();
        }
        #endregion

        #region GetSpotCampaignDetail method
        /// <summary>TBD</summary>
        /// <param name="stationid">TBD</param>
        /// <param name="stationSpotId">TBD</param>
        /// <param name="spotDate">TBD</param>
        /// <param name="campaignName">TBD</param>
        /// <param name="campaignNumber">TBD</param>
        /// <returns>TBD</returns>
        public static SpotCampaignDetail GetSpotCampaignDetail(int stationid, int stationSpotId, DateTime spotDate, string campaignName, string campaignNumber)
        {
            DataSet spotCampaignData;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                using (SqlCommand cmd = IO.CreateCommandFromStoredProc("DigitalAvails_GetSpotCampaignDetail",
                Param.CreateParam("STATIONID", SqlDbType.Int, stationid),
                Param.CreateParam("STATIONSPOTID", SqlDbType.Int, stationSpotId),
                Param.CreateParam("SPOTDATE", SqlDbType.Date, spotDate),
                Param.CreateParam("CAMPAIGNNAME", SqlDbType.VarChar, campaignName)))
                {
                    if (!String.IsNullOrEmpty(campaignNumber))
                    {
                        cmd.Parameters.Add(Param.CreateParam("CAMPAIGNNUMBER", SqlDbType.VarChar, campaignNumber));
                    }
                    spotCampaignData = io.ExecuteDataSetQuery(cmd);
                }
            }
            DataRow row = spotCampaignData.Tables[0].Rows[0];
            SpotCampaignDetail spotCampaignDetail = new SpotCampaignDetail(campaignName
            , campaignNumber
            , (DateTime)row["StartDate"]
            , (DateTime)row["EndDate"]
            , (int)row["TotalWeeks"]
            , (int)row["WeeksBefore"]
            , (int)row["WeeksAfter"]);
            if (row["IdList"] != DBNull.Value)
            {
                spotCampaignDetail.IDs = GetIdListFromDBString((string)row["IdList"]);
            }
            if (row["IdBeforeList"] != DBNull.Value)
            {
                spotCampaignDetail.IDsBefore = GetIdListFromDBString((string)row["IdBeforeList"]);
            }
            if (row["IdAfterList"] != DBNull.Value)
            {
                spotCampaignDetail.IDsAfter = GetIdListFromDBString((string)row["IdAfterList"]);
            }
            return spotCampaignDetail;
        }
        #endregion

        #region IDs property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<int> IDs { get; set; }
        #endregion

        #region IDsAfter property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<int> IDsAfter { get; set; }
        #endregion

        #region IDsBefore property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<int> IDsBefore { get; set; }
        #endregion

        #region StartDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime StartDate { get; set; }
        #endregion

        #region TotalWeeks property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int TotalWeeks { get; set; }
        #endregion

        #region WeeksAfter property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int WeeksAfter { get; set; }
        #endregion

        #region WeeksBefore property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int WeeksBefore { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class SpotConflict
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public SpotConflict()
        {
        }
        #endregion

        #region Parameterized constructor (stationId, stationSpotId, stationName, spotDate)
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        /// <param name="stationSpotId">TBD</param>
        /// <param name="stationName">TBD</param>
        /// <param name="spotDate">TBD</param>
        public SpotConflict(int stationId, int stationSpotId, string stationName, DateTime spotDate)
        {
            this.StationId = stationId;
            this.StationSpotId = stationSpotId;
            this.StationName = stationName;
            this.SpotDate = spotDate;
        }
        #endregion

        #region Parameterized constructor (spotDate)
        /// <summary>TBD</summary>
        /// <param name="spotDate">TBD</param>
        public SpotConflict(DateTime spotDate)
        {
            this.SpotDate = spotDate;
        }
        #endregion

        #region GetMultiStationSpotConflicts method
        /// <summary>TBD</summary>
        /// <param name="conflicts">TBD</param>
        /// <returns>TBD</returns>
        public static List<SpotConflict> GetMultiStationSpotConflicts(DataSet conflicts)
        {
            List<SpotConflict> spotConflicts = new List<SpotConflict>();
            //TODO: Figure out why this is the third table returned
            foreach (DataRow row in conflicts.Tables[conflicts.Tables.Count - 1].Rows)
            {
                spotConflicts.Add(new SpotConflict((int)row["StationId"]
                , (int)row["StationSpotId"]
                , (string)row["StationName"]
                , (DateTime)row["SpotDate"]));
            }
            return spotConflicts;
        }
        #endregion

        #region SpotDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime SpotDate { get; set; }
        #endregion

        #region StationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationId { get; set; }
        #endregion

        #region StationName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string StationName { get; set; }
        #endregion

        #region StationSpotId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationSpotId { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class SpotConflictException : Exception
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public SpotConflictException()
        {
        }
        #endregion

        #region Parameterized constructor (message)
        /// <summary>TBD</summary>
        /// <param name="message">TBD</param>
        public SpotConflictException(string message)
            : base(message)
        {
        }
        #endregion

        #region Parameterized constructor (message, innerException)
        /// <summary>TBD</summary>
        /// <param name="message">TBD</param>
        /// <param name="innerException">TBD</param>
        public SpotConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class SpotType
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public SpotType()
        {
        }
        #endregion

        #region Parameterized constructor (spotTypeId, description, color, includeInBreakdown, breakdownOrder, associatedMarket)
        /// <summary>TBD</summary>
        /// <param name="spotTypeId">TBD</param>
        /// <param name="description">TBD</param>
        /// <param name="color">TBD</param>
        /// <param name="includeInBreakdown">TBD</param>
        /// <param name="breakdownOrder">TBD</param>
        /// <param name="associatedMarket">TBD</param>
        public SpotType(int spotTypeId, string description, string color, bool includeInBreakdown, int breakdownOrder, string associatedMarket)
        {
            this.SpotTypeId = spotTypeId;
            this.Description = description;
            this.Color = color;
            this.IncludeInBreakdown = includeInBreakdown;
            this.BreakdownOrder = breakdownOrder;
            this.AssociatedMarket = associatedMarket;
        }
        #endregion

        #region AssociatedMarket property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AssociatedMarket { get; set; }
        #endregion

        #region BreakdownOrder property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int BreakdownOrder { get; set; }
        #endregion

        #region Color property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Color { get; set; }
        #endregion

        #region Description property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Description { get; set; }
        #endregion

        #region GetSpotTypesByMarket method
        /// <summary>TBD</summary>
        /// <param name="associatedMarket">TBD</param>
        /// <returns>TBD</returns>
        public static List<SpotType> GetSpotTypesByMarket(string associatedMarket)
        {
            DataSet spotTypeData;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                spotTypeData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DigitalAvails_GetSpotTypesByMarket", Param.CreateParam("MARKET", SqlDbType.VarChar, associatedMarket)));
            }
            List<SpotType> spotTypes = new List<SpotType>();
            foreach (DataRow row in spotTypeData.Tables[0].Rows)
            {
                spotTypes.Add(new SpotType((int)row["SpotTypeId"]
                , (string)row["Description"]
                , Convert.ToString(IO.GetDataRowValue(row, "Color", ""))
                , Convert.ToBoolean(IO.GetDataRowValue(row, "IncludeInBreakdown", false))
                , Convert.ToInt32(IO.GetDataRowValue(row, "BreakdownOrder", -1))
                , Convert.ToString(IO.GetDataRowValue(row, "AssociatedMarket", associatedMarket))));
            }
            return spotTypes;
        }
        #endregion

        #region IncludeInBreakdown property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool IncludeInBreakdown { get; set; }
        #endregion

        #region SpotTypeId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int SpotTypeId { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class SpotTypeBreakdown
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public SpotTypeBreakdown()
        {
        }
        #endregion

        #region Parameterized constructor (spotType)
        /// <summary>TBD</summary>
        /// <param name="spotType">TBD</param>
        public SpotTypeBreakdown(string spotType)
        {
            this.SpotType = spotType;
            this.SpotTypeCount = new List<int>();
        }
        #endregion

        #region BreakdownOrder property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int BreakdownOrder { get; set; }
        #endregion

        #region Color property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Color { get; set; }
        #endregion

        #region LoadSpotTypeBreakdown method (stationId, startMonth, startYear, numberOfMonths)
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        /// <param name="startMonth">TBD</param>
        /// <param name="startYear">TBD</param>
        /// <param name="numberOfMonths">TBD</param>
        /// <returns>TBD</returns>
        public static List<SpotTypeBreakdown> LoadSpotTypeBreakdown(int stationId, int startMonth, int startYear, int numberOfMonths)
        {
            DataSet spotTypeBreakdownData;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                spotTypeBreakdownData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALAVAILS_GETSPOTTYPEBREAKDOWN",
                Param.CreateParam("STATIONID", SqlDbType.Int, stationId),
                Param.CreateParam("STARTMONTH", SqlDbType.Int, startMonth),
                Param.CreateParam("STARTYEAR", SqlDbType.Int, startYear),
                Param.CreateParam("NUMBEROFMONTHS", SqlDbType.Int, numberOfMonths)));
            }
            if (spotTypeBreakdownData.Tables.Count == 0 || spotTypeBreakdownData.Tables[0].Rows.Count == 0)
            {
                return new List<SpotTypeBreakdown>();
            }
            return LoadSpotTypeBreakdown(spotTypeBreakdownData.Tables[0]);
        }
        #endregion

        #region LoadSpotTypeBreakdown method (spotTypeBreakdownData)
        /// <summary>TBD</summary>
        /// <param name="spotTypeBreakdownData">TBD</param>
        /// <returns>TBD</returns>
        public static List<SpotTypeBreakdown> LoadSpotTypeBreakdown(DataTable spotTypeBreakdownData)
        {
            List<SpotTypeBreakdown> SpotTypeBreakdown = new List<SpotTypeBreakdown>();
            SpotTypeBreakdown breakDownRecord;
            foreach (DataRow row in spotTypeBreakdownData.Rows)
            {
                breakDownRecord = new SpotTypeBreakdown((string)row["SpotType"]);
                breakDownRecord.StationId = (int)row["StationId"];
                breakDownRecord.SpotTypeId = (int)row["SpotTypeId"];
                breakDownRecord.Color = (string)row["Color"];
                breakDownRecord.BreakdownOrder = (int)row["BreakdownOrder"];
                for (int i = 5; i < spotTypeBreakdownData.Columns.Count; i++)
                {
                    breakDownRecord.SpotTypeCount.Add((int)row[i]);
                }
                SpotTypeBreakdown.Add(breakDownRecord);
            }
            return SpotTypeBreakdown;
        }
        #endregion

        #region SpotType property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string SpotType { get; set; }
        #endregion

        #region SpotTypeCount property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<int> SpotTypeCount { get; set; }
        #endregion

        #region SpotTypeId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int SpotTypeId { get; set; }
        #endregion

        #region StationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationId { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class Station
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public Station()
        {
        }
        #endregion

        #region Parameterized constructor (stationOptions)
        /// <summary>TBD</summary>
        /// <param name="stationOptions">TBD</param>
        public Station(StationOptions stationOptions)
        {
            this.StationId = stationOptions.stationId;
            this.Load();
            this.SpotMatrix = Spot.LoadSpotMatrix(stationOptions.stationId, stationOptions.startMonth, stationOptions.startYear, stationOptions.numberOfMonths);
            this.SpotTypeBreakdown = DigitalAvails.SpotTypeBreakdown.LoadSpotTypeBreakdown(this.StationId, stationOptions.startMonth, stationOptions.startYear, stationOptions.numberOfMonths);
            this.StationSpots = StationSpot.LoadStationSpots(this.StationId);
        }
        #endregion

        #region Parameterized constructor (stationId)
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        public Station(int stationId)
        {
            this.StationId = stationId;
            this.Load();
        }
        #endregion

        #region Description property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Description { get; set; }
        #endregion

        #region Load method
        /// <summary>TBD</summary>
        public void Load()
        {
            DataRow stationRow;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                stationRow = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALAVAILS_GETSTATIONDETAIL", Param.CreateParam("STATIONID", SqlDbType.Int, this.StationId))).Tables[0].Rows[0];
            }
            LoadStationData(stationRow);
        }
        #endregion

        #region LoadStationData method
        /// <summary>TBD</summary>
        /// <param name="stationRow">TBD</param>
        public void LoadStationData(DataRow stationRow)
        {
            this.StationId = (int)stationRow["StationId"];
            this.Market = (string)stationRow["Market"];
            this.Name = (string)stationRow["Name"];
            this.Description = Convert.ToString(IO.GetDataRowValue(stationRow, "Description", ""));
            this.ScreenTypeId = (int)stationRow["ScreenTypeId"];
            this.ScreenType = (string)stationRow["ScreenType"];
            this.TotalSpots = (int)stationRow["TotalSpots"];
            this.TotalBreakdownSpots = (int)stationRow["TotalBreakdownSpots"];
        }
        #endregion

        #region Market property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Market { get; set; }
        #endregion

        #region Name property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Name { get; set; }
        #endregion

        #region ScreenType property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ScreenType { get; set; }
        #endregion

        #region ScreenTypeId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int ScreenTypeId { get; set; }
        #endregion

        #region SpotMatrix property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<Spot> SpotMatrix { get; set; }
        #endregion

        #region SpotTypeBreakdown property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<SpotTypeBreakdown> SpotTypeBreakdown { get; set; }
        #endregion

        #region StationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationId { get; set; }
        #endregion

        #region StationSpots property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<StationSpot> StationSpots { get; set; }
        #endregion

        #region TotalBreakdownSpots property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int TotalBreakdownSpots { get; set; }
        #endregion

        #region TotalSpots property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int TotalSpots { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class StationOptions
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public StationOptions()
        {
        }
        #endregion

        #region numberOfMonths property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int numberOfMonths { get; set; }
        #endregion

        #region startMonth property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int startMonth { get; set; }
        #endregion

        #region startYear property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int startYear { get; set; }
        #endregion

        #region stationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int stationId { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class StationSpot
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public StationSpot()
        {
        }
        #endregion

        #region Parameterized constructor (stationSpotId, stationId, backgroundColor, name, dividerBefore, dividerAfter, order, includeInBreakdown, active)
        /// <summary>TBD</summary>
        /// <param name="stationSpotId">TBD</param>
        /// <param name="stationId">TBD</param>
        /// <param name="backgroundColor">TBD</param>
        /// <param name="name">TBD</param>
        /// <param name="dividerBefore">TBD</param>
        /// <param name="dividerAfter">TBD</param>
        /// <param name="order">TBD</param>
        /// <param name="includeInBreakdown">TBD</param>
        /// <param name="active">TBD</param>
        public StationSpot(int stationSpotId, int stationId, string backgroundColor, string name
        , bool dividerBefore, bool dividerAfter, int order, bool includeInBreakdown, bool active)
        {
            this.StationSpotId = stationSpotId;
            this.StationId = stationId;
            this.BackgroundColor = backgroundColor;
            this.Name = name;
            this.DividerBefore = dividerBefore;
            this.DividerAfter = dividerAfter;
            this.Order = order;
            this.IncludeInBreakdown = includeInBreakdown;
            this.Active = active;
        }
        #endregion

        #region Active property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool Active { get; set; }
        #endregion

        #region BackgroundColor property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string BackgroundColor { get; set; }
        #endregion

        #region DividerAfter property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool DividerAfter { get; set; }
        #endregion

        #region DividerBefore property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool DividerBefore { get; set; }
        #endregion

        #region IncludeInBreakdown property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool IncludeInBreakdown { get; set; }
        #endregion

        #region LoadStationSpots method (stationId)
        /// <summary>TBD</summary>
        /// <param name="stationId">TBD</param>
        /// <returns>TBD</returns>
        public static List<StationSpot> LoadStationSpots(int stationId)
        {
            List<StationSpot> stationSpots = new List<StationSpot>();
            DataSet stationSpotData;
            using (IO io = new IO(WebCommon.DigitalAvailsConnectionString))
            {
                stationSpotData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("DIGITALAVAILS_GETSTATIONSPOTS", Param.CreateParam("STATIONID", SqlDbType.Int, stationId)));
            }
            if (stationSpotData.Tables.Count == 0 || stationSpotData.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            stationSpots = LoadStationSpots(stationSpotData.Tables[0]);
            return stationSpots;
        }
        #endregion

        #region LoadStationSpots method (stationSpotData)
        /// <summary>TBD</summary>
        /// <param name="stationSpotData">TBD</param>
        /// <returns>TBD</returns>
        public static List<StationSpot> LoadStationSpots(DataTable stationSpotData)
        {
            List<StationSpot> stationSpots = new List<StationSpot>();
            foreach (DataRow row in stationSpotData.Rows)
            {
                stationSpots.Add(new StationSpot((int)row["StationSpotId"]
                , (int)row["StationId"]
                , Convert.ToString(IO.GetDataRowValue(row, "BackgroundColor", "FFFFFF"))
                , (string)row["Name"]
                , (bool)row["DividerBefore"]
                , (bool)row["DividerAfter"]
                , (int)row["Order"]
                , (bool)row["IncludeInBreakdown"]
                , (bool)row["Active"]
                ));
            }
            return stationSpots;
        }
        #endregion

        #region Name property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string Name { get; set; }
        #endregion

        #region Order property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int Order { get; set; }
        #endregion

        #region StationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationId { get; set; }
        #endregion

        #region StationSpotId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int StationSpotId { get; set; }
        #endregion

    }

}
