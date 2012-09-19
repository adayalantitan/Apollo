#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Titan.DataIO;
using ExcelXml = Titan.ExcelXml;
#endregion

/// <summary>
/// Summary description for StationDomAvails
/// </summary>
namespace Apollo.StationDomAvails
{

    /// <summary>TBD</summary>
    public class Avails
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public Avails()
        {
        }
        #endregion

        #region Parameterized constructor (locationId, locationMarket, locationDesc, fourWeekRate, prodInstallRate, prodInstallRateAdditional, locationStatusId, locationStatus, locationComments, reserveWinterMonths, bookingId, bookingDesc, bookingStartDate, bookingEndDate, bookingStatusId, bookinStatus, bookingComments)
        /// <summary>TBD</summary>
        /// <param name="locationId">TBD</param>
        /// <param name="locationMarket">TBD</param>
        /// <param name="locationDesc">TBD</param>
        /// <param name="fourWeekRate">TBD</param>
        /// <param name="prodInstallRate">TBD</param>
        /// <param name="prodInstallRateAdditional">TBD</param>
        /// <param name="locationStatusId">TBD</param>
        /// <param name="locationStatus">TBD</param>
        /// <param name="locationComments">TBD</param>
        /// <param name="reserveWinterMonths">TBD</param>
        /// <param name="bookingId">TBD</param>
        /// <param name="bookingDesc">TBD</param>
        /// <param name="bookingStartDate">TBD</param>
        /// <param name="bookingEndDate">TBD</param>
        /// <param name="bookingStatusId">TBD</param>
        /// <param name="bookinStatus">TBD</param>
        /// <param name="bookingComments">TBD</param>
        public Avails(int locationId, string locationMarket, string locationDesc, string fourWeekRate, string prodInstallRate, string prodInstallRateAdditional
            , int locationStatusId, string locationStatus, string locationComments, bool reserveWinterMonths, int bookingId, string bookingDesc
            , DateTime bookingStartDate, DateTime bookingEndDate, int bookingStatusId
            , string bookinStatus, string bookingComments)
        {
            this.locationId = locationId;
            this.locationMarket = locationMarket;
            this.locationDesc = locationDesc;
            this.fourWeekRate = fourWeekRate;
            this.prodInstallRate = prodInstallRate;
            this.prodInstallRateAdditional = prodInstallRateAdditional;
            this.locationStatusId = locationStatusId;
            this.locationStatus = locationStatus;
            this.locationComments = locationComments;
            this.locationBookings = locationBookings;
            this.locationReserveWinterMonths = reserveWinterMonths;
            this.bookingId = bookingId;
            this.bookingDesc = bookingDesc;
            this.bookingStartDate = bookingStartDate;
            this.bookingEndDate = bookingEndDate;
            this.bookingStatusId = bookingStatusId;
            this.bookingStatus = bookingStatus;
            this.bookingComments = bookingComments;
        }
        #endregion

        #region Parameterized constructor (locationId, locationMarket, locationDesc, fourWeekRate, prodInstallRate, prodInstallRateAdditional, locationStatusId, locationStatus, locationComments, reserveWinterMonths, locationBookings)
        /// <summary>TBD</summary>
        /// <param name="locationId">TBD</param>
        /// <param name="locationMarket">TBD</param>
        /// <param name="locationDesc">TBD</param>
        /// <param name="fourWeekRate">TBD</param>
        /// <param name="prodInstallRate">TBD</param>
        /// <param name="prodInstallRateAdditional">TBD</param>
        /// <param name="locationStatusId">TBD</param>
        /// <param name="locationStatus">TBD</param>
        /// <param name="locationComments">TBD</param>
        /// <param name="reserveWinterMonths">TBD</param>
        /// <param name="locationBookings">TBD</param>
        public Avails(int locationId, string locationMarket, string locationDesc, string fourWeekRate, string prodInstallRate, string prodInstallRateAdditional
            , int locationStatusId, string locationStatus, string locationComments, bool reserveWinterMonths, List<Booking> locationBookings)
        {
            this.locationId = locationId;
            this.locationMarket = locationMarket;
            this.locationDesc = locationDesc;
            this.fourWeekRate = fourWeekRate;
            this.prodInstallRate = prodInstallRate;
            this.prodInstallRateAdditional = prodInstallRateAdditional;
            this.locationStatusId = locationStatusId;
            this.locationStatus = locationStatus;
            this.locationComments = locationComments;
            this.locationBookings = locationBookings;
            this.locationReserveWinterMonths = reserveWinterMonths;
        }
        #endregion

        #region bookingComments property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string bookingComments { get; set; }
        #endregion

        #region bookingDesc property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string bookingDesc { get; set; }
        #endregion

        #region bookingEndDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime bookingEndDate { get; set; }
        #endregion

        #region bookingId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingId { get; set; }
        #endregion

        #region bookingStartDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime bookingStartDate { get; set; }
        #endregion

        #region bookingStatus property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string bookingStatus { get; set; }
        #endregion

        #region bookingStatusId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingStatusId { get; set; }
        #endregion

        #region fourWeekRate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string fourWeekRate { get; set; }
        #endregion

        #region GetAvails method
        /// <summary>TBD</summary>
        /// <param name="year">TBD</param>
        /// <param name="market">TBD</param>
        /// <returns>TBD</returns>
        public static List<Avails> GetAvails(int year, string market)
        {
            List<Avails> availsList = new List<Avails>();
            DataSet availsData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = IO.CreateCommandFromStoredProc("Production_GetAvailsList"))
                {
                    cmd.Parameters.Add(Param.CreateParam("YEAR", SqlDbType.Int, year));
                    if (!String.IsNullOrEmpty(market))
                    {
                        cmd.Parameters.Add(Param.CreateParam("MARKET", SqlDbType.VarChar, market));
                    }
                    availsData = io.ExecuteDataSetQuery(cmd);
                }
            }
            int prevLocationId = -1;
            foreach (DataRow row in availsData.Tables[0].Rows)
            {
                if (prevLocationId != Convert.ToInt32(row["LOCATION_ID"]))
                {
                    availsList.Add(new Avails(Convert.ToInt32(row["LOCATION_ID"])
                        , Convert.ToString(IO.GetDataRowValue(row, "LOCATION_MARKET", ""))
                        , Convert.ToString(IO.GetDataRowValue(row, "LOCATION_DESC", ""))
                        , Convert.ToString(IO.GetDataRowValue(row, "FOUR_WEEK_RATE", ""))
                        , Convert.ToString(IO.GetDataRowValue(row, "PROD_AND_INSTALL", 0))
                        , Convert.ToString(IO.GetDataRowValue(row, "PROD_AND_INSTALL_ADDITIONAL", ""))
                        , Convert.ToInt32(IO.GetDataRowValue(row, "LOCATION_STATUS_ID", -1))
                        , Convert.ToString(IO.GetDataRowValue(row, "LOCATION_STATUS_DESC", ""))
                        , Convert.ToString(IO.GetDataRowValue(row, "LOCATION_COMMENTS", ""))
                        , (Convert.ToInt32(IO.GetDataRowValue(row, "RESERVE_WINTER_MONTHS", 0)) != 0)
                        , new List<Booking>()
                    ));
                }
                if (Convert.ToInt32(IO.GetDataRowValue(row, "BOOKING_ID", -1)) != -1)
                {
                    availsList[availsList.Count - 1].locationBookings.Add(new Booking(Convert.ToInt32(row["LOCATION_ID"])
                        , Convert.ToInt32(row["BOOKING_ID"])
                        , Convert.ToString(IO.GetDataRowValue(row, "BOOKING_DESC", ""))
                        , Convert.ToDateTime(IO.GetDataRowValue(row, "BOOKING_START_DATE", DateTime.MinValue))
                        , Convert.ToDateTime(IO.GetDataRowValue(row, "BOOKING_END_DATE", DateTime.MaxValue))
                        , Convert.ToInt32(IO.GetDataRowValue(row, "BOOKING_STATUS_ID", -1))
                        , Convert.ToString(IO.GetDataRowValue(row, "BOOKING_STATUS_DESC", ""))
                        , Convert.ToString(IO.GetDataRowValue(row, "BOOKING_COMMENTS", ""))
                        , Convert.ToString(IO.GetDataRowValue(row, "AE_NTID", -1))
                    ));
                }
                prevLocationId = Convert.ToInt32(row["LOCATION_ID"]);
            }
            return availsList;
        }
        #endregion

        #region locationBookings property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<Booking> locationBookings { get; set; }
        #endregion

        #region locationComments property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationComments { get; set; }
        #endregion

        #region locationDesc property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationDesc { get; set; }
        #endregion

        #region locationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int locationId { get; set; }
        #endregion

        #region locationMarket property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationMarket { get; set; }
        #endregion

        #region locationReserveWinterMonths property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool locationReserveWinterMonths { get; set; }
        #endregion

        #region locationStatus property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationStatus { get; set; }
        #endregion

        #region locationStatusId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int locationStatusId { get; set; }
        #endregion

        #region prodInstallRate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string prodInstallRate { get; set; }
        #endregion

        #region prodInstallRateAdditional property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string prodInstallRateAdditional { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class Booking
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public Booking()
        {
        }
        #endregion

        #region Parameterized constructor (locationId, bookingId, bookingDesc, bookingStartDate, bookingEndDate, bookingStatusId, bookingStatus, bookingComments, aeNtId)
        /// <summary>TBD</summary>
        /// <param name="locationId">TBD</param>
        /// <param name="bookingId">TBD</param>
        /// <param name="bookingDesc">TBD</param>
        /// <param name="bookingStartDate">TBD</param>
        /// <param name="bookingEndDate">TBD</param>
        /// <param name="bookingStatusId">TBD</param>
        /// <param name="bookingStatus">TBD</param>
        /// <param name="bookingComments">TBD</param>
        /// <param name="aeNtId">TBD</param>
        public Booking(int locationId, int bookingId, string bookingDesc, DateTime bookingStartDate, DateTime bookingEndDate
            , int bookingStatusId, string bookingStatus, string bookingComments, string aeNtId)
        {
            this.locationId = locationId;
            this.bookingId = bookingId;
            this.bookingDesc = bookingDesc;
            this.bookingStartDate = bookingStartDate;
            this.bookingStartDateYear = this.bookingStartDate.Year;
            this.bookingStartDateMonth = this.bookingStartDate.Month - 1;
            this.bookingStartDateDay = this.bookingStartDate.Day;
            this.bookingEndDate = bookingEndDate;
            this.bookingEndDateYear = this.bookingEndDate.Year;
            this.bookingEndDateMonth = this.bookingEndDate.Month - 1;
            this.bookingEndDateDay = this.bookingEndDate.Day;
            this.bookingStatusId = bookingStatusId;
            this.bookingStatus = bookingStatus;
            this.bookingComments = bookingComments;
            this.aeNtId = aeNtId;
        }
        #endregion

        #region aeNtId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string aeNtId { get; set; }
        #endregion

        #region bookingComments property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string bookingComments { get; set; }
        #endregion

        #region bookingDesc property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string bookingDesc { get; set; }
        #endregion

        #region bookingEndDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime bookingEndDate { get; set; }
        #endregion

        #region bookingEndDateDay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingEndDateDay { get; set; }
        #endregion

        #region bookingEndDateMonth property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingEndDateMonth { get; set; }
        #endregion

        #region bookingEndDateYear property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingEndDateYear { get; set; }
        #endregion

        #region bookingId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingId { get; set; }
        #endregion

        #region bookingStartDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime bookingStartDate { get; set; }
        #endregion

        #region bookingStartDateDay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingStartDateDay { get; set; }
        #endregion

        #region bookingStartDateMonth property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingStartDateMonth { get; set; }
        #endregion

        #region bookingStartDateYear property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingStartDateYear { get; set; }
        #endregion

        #region bookingStatus property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string bookingStatus { get; set; }
        #endregion

        #region bookingStatusId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int bookingStatusId { get; set; }
        #endregion

        #region locationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int locationId { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class Location
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public Location()
        {
        }
        #endregion

        #region Parameterized constructor (locationId, locationMarket, locationDesc, fourWeekRate, prodInstallRate, prodInstallRateAdditional, locationStatusId, locationStatus, locationComments)
        /// <summary>TBD</summary>
        /// <param name="locationId">TBD</param>
        /// <param name="locationMarket">TBD</param>
        /// <param name="locationDesc">TBD</param>
        /// <param name="fourWeekRate">TBD</param>
        /// <param name="prodInstallRate">TBD</param>
        /// <param name="prodInstallRateAdditional">TBD</param>
        /// <param name="locationStatusId">TBD</param>
        /// <param name="locationStatus">TBD</param>
        /// <param name="locationComments">TBD</param>
        public Location(int locationId, string locationMarket, string locationDesc, string fourWeekRate, string prodInstallRate, string prodInstallRateAdditional
            , int locationStatusId, string locationStatus, string locationComments)
        {
            this.locationId = locationId;
            this.locationMarket = locationMarket;
            this.locationDesc = locationDesc;
            this.fourWeekRate = fourWeekRate;
            this.prodInstallRate = prodInstallRate;
            this.prodInstallRateAdditional = prodInstallRateAdditional;
            this.locationStatusId = locationStatusId;
            this.locationStatus = locationStatus;
            this.locationComments = locationComments;
        }
        #endregion

        #region fourWeekRate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string fourWeekRate { get; set; }
        #endregion

        #region locationComments property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationComments { get; set; }
        #endregion

        #region locationDesc property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationDesc { get; set; }
        #endregion

        #region locationId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int locationId { get; set; }
        #endregion

        #region locationMarket property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationMarket { get; set; }
        #endregion

        #region locationStatus property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string locationStatus { get; set; }
        #endregion

        #region locationStatusId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int locationStatusId { get; set; }
        #endregion

        #region prodInstallRate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string prodInstallRate { get; set; }
        #endregion

        #region prodInstallRateAdditional property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string prodInstallRateAdditional { get; set; }
        #endregion

        #region reserveWinterMonths property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool reserveWinterMonths { get; set; }
        #endregion

    }

    public class StationDomReport
    {
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

        #region ReportWorkbook property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public ExcelXml.Workbook ReportWorkbook { get; set; }
        #endregion

        #region ReportStyles property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<ExcelXml.Style> ReportStyles { get; set; }
        #endregion

        #region ReportColumns property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<ExcelXml.Column> ReportColumns { get; set; }
        #endregion

        /*

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

        */
    }

    /// <summary>TBD</summary>
    public class StationDomAvailsReport : StationDomReport
    {
        public StationDomAvailsReport()
        {
        }

        public void GetReportData()
        {
        }
    }

    public class StationDomAvailsComparisonReport : StationDomReport
    {
        public StationDomAvailsComparisonReport()
        {
        }

        public void GetReportData()
        {
        }
    }
}
