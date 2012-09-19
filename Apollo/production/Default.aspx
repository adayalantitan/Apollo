<%@ Page Title="Station Domination Avails | Titan 360" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Apollo.production_Default" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">    
    <link rel="Stylesheet" href="avails.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <script type="text/javascript" src="avails.js?v=1.2"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".button").button();
            $(".datepicker").datepicker();
            $("#addEditLocationDialog").dialog({ autoOpen: false, width: 500, height: 510, modal: true });
            $("#addEditBookingDialog").dialog({ autoOpen: false, width: 400, height: 325, modal: true });
            $("#addNewLocation").css({ "display": (IsAdminUser() ? "block" : "none") });
            Apollo.ProductionIOService.GetLocationStatusList("", AddToList, null, "dropDownLocationStatus");
            Apollo.ProductionIOService.GetBookingStatusList("", AddToList, null, "dropDownBookingStatus");
            Apollo.ProductionIOService.GetBookingAEList("", AddToList, null, "dropDownBookingAE");
            $("#instructions").html((IsEditUser() ? "Click on a booking to View/Edit Dates, Comments, Location...etc." : "Click on a booking to View Details."));
            $("#availsDisplay").html($("#winterMonthMessage").html());
            var year = new Date().getFullYear();
            $("#year").val(year);
        });
        function IsEditUserForMarket(market) {
            if ($get("<%=isEditUser.ClientID %>").value == "-1") {
                if (($get("<%=editUserMarket.ClientID %>").value == "") || ($get("<%=editUserMarket.ClientID %>").value == market) || market.toLowerCase() == "dallas") {
                    return true;
                }
            }
            return false;
        }
        function IsEditUser() {
            if ($get("<%=isEditUser.ClientID %>").value == "-1") {
                return true;
            }
            return false;
        }
        function IsAdminUser() {
            if (($get("<%=isEditUser.ClientID %>").value == "-1") && ($get("<%=editUserMarket.ClientID %>").value == "")) {
                return true;
            }
            return false;
        }
        function ExportToExcel() {
            var selectedYear = $("#year").val();
            var exportData = trimValue($("#availsDisplay").html());
            if (exportData == "") {
                alert("No data has been requested. Please Click View Bookings before trying to export.");
                return false;
            }
            $get("<%=exportYear.ClientID %>").value = selectedYear;
            $get("<%=exportData.ClientID %>").value = exportData;
            return true;
        }
        function GenerateAvailsTableNew() {
            var yearSelected = $("#year").val();
            var marketSelected = $("#market").val();
            Apollo.ProductionIOService.GetAvails(yearSelected, marketSelected, GenerateAvailsTableCallbackNew, ErrorCallback);
        }
        function GenerateAvailsTableCallbackNew(bookings) {
            $("#bookingDisplayTable").remove();
            $("#availsDisplay").html("");
            $("#availsDisplay").append("<table id='bookingDisplayTable' class='featureTable scrollableFixedHeaderTable'></table>");
            var prevMarket;
            var yearSelected = $("#year").val();
            var headerRow = "<thead><tr class='bookingTableHeader'>";
            if (IsEditUser()) { headerRow += "<th colspan='" + (IsAdminUser() ? "3" : "2") + "'>&nbsp;</th>"; }
            headerRow += "<th>Market</th><th>Location</th><th>4-Week Rate Card</th><th>Prod &amp; Install<br/>(must recover<br/>additional creative<br/>cost)</th><th>Prod &amp; Install<br/>(unlimited creatives)</th>";
            headerRow += PopulateHeaders(yearSelected);
            headerRow += "</tr></thead>";
            $("#bookingDisplayTable").append(headerRow);
            var footerRow = "<tr class='bookingTableFooter'><td colspan='400'>IMPORTANT NOTE:<br/>- All production costs are subject to tax.<br/>- Production cost accounts for (1) art mechanical per media format. Any additional art mechanicals will incur a <u>fee of $100</u>.<br/>- Files are due <u>30 days</u> prior to the starting date to guarantee on-time posting. Titan will not be responsible for late postings if files are delivered after the due date.<br/>- Station Saturation packages will incur additional installation charges. Cost varies with media. Please contact <a href='mailto:production@titan360.com' alt='Mail To: production@titan360.com'>production@titan360.com</a> for quote.</td></tr>";
            $("#bookingDisplayTable").append("<tbody>");
            var availsRow = "";
            prevMarket = bookings[0].locationMarket;
            for (var i = 0; i < bookings.length; i++) {
                if (prevMarket != bookings[i].locationMarket) {
                    $("#bookingDisplayTable").append(footerRow);
                    $("#bookingDisplayTable").append(headerRow);
                    prevMarket = bookings[i].locationMarket;
                }
                availsRow = "<tr class='availRow'>";
                if (IsAdminUser()) { availsRow += "<td style='text-align:center;'><input type='button' class='button' id='deleteLocation_" + i + "' value='Delete Location' onclick='DeleteLocation(" + bookings[i].locationId + ");' /></td>"; }
                if (IsEditUserForMarket(bookings[i].locationMarket)) {
                    availsRow += "<td style='text-align:center;'><input type='button' class='button' id='editLocation_" + i + "' value='Edit Location' /></td><td style='text-align:center;'><input type='button' class='button' onclick='AddBooking(" + bookings[i].locationId + ");' value='Add Booking' /></td>";
                } else {
                    if (IsEditUser()) { availsRow += "<td colspan='2'>&nbsp;</td>"; }
                }
                availsRow += "<td style='text-align:center;'>" + bookings[i].locationMarket + "</td><td class='" + (bookings[i].locationStatusId != "" ? ("statusClass" + bookings[i].locationStatusId) : "") + "'>" + bookings[i].locationDesc + (bookings[i].locationReserveWinterMonths ? " ***" : "") + "</td>"
                availsRow += "<td style='text-align:right;'>" + bookings[i].fourWeekRate + "</td><td style='text-align:right;'>" + bookings[i].prodInstallRate + "</td><td style='text-align:right;'>" + bookings[i].prodInstallRateAdditional + "</td>"
                if (bookings[i].locationBookings.length == 0) {
                    //availsRow += PopulateBlankTable(yearSelected, true);
                    availsRow += GenerateNormalCellBlock(new Date(yearSelected, 0, 1), new Date(yearSelected, 11, 31));
                } else {
                    availsRow += PopulateBookings(yearSelected, bookings[i].locationBookings);
                }
                //availsRow += BuildBookingDisplay(bookings[i].locationBookings, bookings[i].locationReserveWinterMonths)                
                availsRow += "</tr>";
                $("#bookingDisplayTable").append(availsRow);
                $("#editLocation_" + i).bind("click", bookings[i], EditLocation);
            }
            $("#bookingDisplayTable").append(footerRow);
            $("#bookingDisplayTable").append("</tbody>");
            $(".button").button();
            $("#keyDisplay").css({ "display": "block" });
        }
        var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        function DateDisplay(date) { return (date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear()); }
        function GetDaysBetweenDates(startDate, endDate) {
            var daysInMs = 1000 * 60 * 60 * 24;
            var diffInMs = endDate.getTime() - startDate.getTime();
            if (diffInMs < 0) { return 0; }
            return Math.round(diffInMs / daysInMs);
        }        
        function Populate() {
            var tableHtml = "<table style='width:100%;margin:10px;'>";
            tableHtml += PopulateHeaders();
            if (bookings.length == 0) { tableHtml += PopulateBlankTable(); return; }
            tableHtml += PopulateBookings();
            tableHtml += "</table>";
            document.getElementById("domTableDiv").innerHTML = tableHtml;
        }
        function PopulateHeaders(year) {
            var monthEndDay;
            var headerRowHtml = "";
            for (var i = 0; i < 12; i++) {
                monthStartDate = new Date(year, i, 1);
                monthEndDate = new Date(monthStartDate);
                monthEndDate.setMonth(monthEndDate.getMonth() + 1);
                monthEndDate.setDate(monthEndDate.getDate() - 1);
                //Find the end date of the current month
                monthEndDay = monthEndDate.getDate();
                headerRowHtml += "<th month='" + (i + 1) + "' year='2012' colspan='" + monthEndDay + "' style='text-align:center;width:" + (2 * monthEndDay) + " !important;'>" + months[i] + " - " + year + "</th>";
            }
            return headerRowHtml;
        }
        function PopulateBookings(year, bookings) {
            var rowHtml = "";
            /* Situations to consider:
            -- Booking array MUST be sorted by Start Date; no bookings may overlap
					
            1. Booking starts in Previous Year
            - For display purposes - Default booking start to first day of current year
            2. Booking ends in Next Year
            - For display purposes - Default booking end to last day of current year
            3. Booking does not start on first day of month/gap between end of previous booking and current booking
            - Draw normal cell(s) up to booking start/between previous booking end and current booking start
            */
            var currentDateDiff = 0;
            var currentDate = new Date(year, 0, 1);
            var currentBooking = {};
            //Go through the bookings (they will be in order)
            for (var k = 0; k < bookings.length; k++) {
                currentBooking = bookings[k];
                //Does this booking start in the previous year?
                if (currentBooking.bookingStartDate.getFullYear() < year) { currentBooking.bookingStartDate = new Date(year, 0, 1); }
                //Does this booking end in the next year?
                if (currentBooking.bookingEndDate.getFullYear() > year) { currentBooking.bookingEndDate = new Date(year, 11, 31); }
                //Is there a gap between the current date and the booking start date?
                rowHtml += GenerateNormalCellBlock(currentDate, currentBooking.bookingStartDate);
                //Populate the booking cells
                rowHtml += GenerateBookingCell(currentBooking);
                //set the currentDate to the end date of the booking
                currentDate = new Date(currentBooking.bookingEndDate.toDateString());
                currentDate.setDate(currentDate.getDate() + 1);
            }
            //are there any days left through end of year?
            //rowHtml += GenerateNormalCellBlock(currentDate, new Date(year + 1, 0, 1));
            rowHtml += GenerateNormalCellBlock(currentDate, new Date(year, 11, 31));
            return rowHtml;
        }

        function PopulateBlankTable(year, reserveWinterMonths) {
            var monthStartDate, monthEndDate;
            var monthEndDay;
            var rowHtml = "";
            for (var i = 0; i < 12; i++) {
                monthStartDate = new Date(year, i, 1);
                monthEndDate = new Date(monthStartDate);
                monthEndDate.setMonth(monthEndDate.getMonth() + 1);
                monthEndDate.setDate(monthEndDate.getDate() - 1);
                //Find the end date of the current month
                monthEndDay = monthEndDate.getDate();
                for (var j = 1; j <= monthEndDay; j++) {
                    rowHtml += GenerateNormalCell((i + 1), j, year, (j == 1), (j == monthEndDay));
                }
            }
            return rowHtml;
        }

        function GenerateNormalCellBlock(fromDate, toDate) {
            var html = "";
            var dateDiff = GetDaysBetweenDates(fromDate, toDate);
            if (dateDiff == 0) { return html; }
            //Populate the row with normal cells
            html += "<td colspan='" + dateDiff + "' style='width:2px !important;border-bottom:1px solid #000000;'>&nbsp;</td>";
            return html;
        }

        function GenerateNormalCell(month, day, year, isStart, isEnd) {
            //border:1px solid #cccccc;
            return "<td month='" + month + "' year='" + year + "' day='" + day + "' style='width:2px !important;border-bottom:1px solid #000000;" + (isStart ? "border-left:1px solid #000000;" : (isEnd ? "border-right:1px solid #000000;" : "")) + "'>&nbsp;</td>";
        }

        function GenerateBookingCell(booking) {
            var span = GetDaysBetweenDates(booking.bookingStartDate, booking.bookingEndDate) + 1;
            var dateDisplay = DateDisplay(booking.bookingStartDate) + " - " + DateDisplay(booking.bookingEndDate);
            return "<td bookingId='" + booking.bookingId + "' colspan='" + span + "' style='text-align:center;width:" + (2 * span) + "px !important;border:1px solid #000000;white-space:nowrap !important;'>" + booking.bookingDesc + "<br/>" + dateDisplay + "</td>";
        }
    </script>
    <asp:HiddenField ID="isEditUser" runat="server" />    
    <asp:HiddenField ID="editUserMarket" runat="server" />
    <asp:HiddenField ID="exportYear" runat="server" />
    <asp:HiddenField ID="exportData" runat="server" />
    <div id="winterMonthMessage" style="display:none;">
        <h3>The following packages have restriction during the winter months, please check with the production department prior to selling the current packages.</h3>
        <h4 style="margin-top:10px;">Chicago</h4>
        <ul style="margin-left:10px;list-style-type:none;">
            <li>Sox 35th SD</li>
            <li>Addison SD</li>
            <li>Belmont SD</li>
            <li>Fullerton SD</li>
            <li>Merchandise Market SD</li>
            <li>Midway SD</li>
            <li>State ST Head Houses SD</li>
        </ul>
        <h4 style="margin-top:10px;">Boston:</h4>
        <ul style="margin-left:10px;list-style-type:none;">
            <li>Harvard Elevator</li>
            <li>Harvard Windows</li>
            <li>Kenmore Tower</li>
            <li>Kenmore Windows</li>
        </ul>
        <h4 style="margin-top:10px;">New Jersey:</h4>
        <ul style="margin-left:10px;list-style-type:none;">
            <li>Hoboken SD</li>
            <li>Metro Park SD</li>
            <li>Middle Town SD</li>
            <li>Newark Penn SD</li>
            <li>Secaucus Junction SD</li>
            <li>Bridgewater Shelter Ext</li>
            <li>Bridgewater Shelter Int</li>
            <li>Hamilton Shelter</li>
            <li>Hoboken Column Wrap</li>
            <li>Hoboken Wallscapes</li>
            <li>Metro Park Wallscapes</li>
            <li>Metro Park Windows</li>
            <li>Metro Park Stairway displays</li>
            <li>Mt Arlington Windows</li>
            <li>Plainfield Wallscape</li>
            <li>Rt 23 Windows</li>
        </ul>
        <h4 style="margin-top:10px;">Philadelphia:</h4>
        <ul style="margin-left:10px;list-style-type:none;">
            <li>FTC SD</li>
            <li>Market Street Furniture SD</li>
            <li>Spring Garden SD</li>
        </ul>
        <h4 style="margin-top:10px;">Minneapolis:</h4>
        <ul style="margin-left:10px;list-style-type:none;">
            <li>Metro Dome SD</li>
            <li>Nicollet Mall SD</li>
            <li>Target Field SD</li>
            <li>Warehouse SD</li>
        </ul>
    </div>
    <div id="addEditLocationDialog">
        <fieldset style="padding:10px">
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="dropDownMarket">Market:</label></div>                    
                <div style="float:left;">
                    <select id="dropDownMarket">
                        <option value="" selected="selected"> - Select A Market - </option>
                        <option value="Amtrak">Amtrak</option>
                        <option value="Boston">Boston</option>
                        <option value="Chicago">Chicago</option>
                        <option value="Dallas">Dallas</option>
                        <option value="Minneapolis">Minneapolis</option>
                        <option value="New Jersey">New Jersey</option>
                        <option value="New York">New York</option>
                        <option value="Philadelphia">Philadelphia</option>
                        <option value="Pittsburgh">Pittsburgh</option>
                        <option value="San Francisco">San Francisco</option>
                        <option value="Seattle">Seattle</option>                    
                        <option value="Washington DC">Washington DC</option>
                    </select>
                    <span class="requiredIndicator">*</span>
                </div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textLocation">Location:</label></div>                    
                <div style="float:left;"><input type="text" id="textLocation" style="width:150px;" /><span class="requiredIndicator">*</span></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textFourWeekRate">4-Week Rate:</label></div>                    
                <div style="float:left;"><input type="text" id="textFourWeekRate" style="width:100px;" /><span class="requiredIndicator">*</span></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textProdInstall">Prod &amp; Install<br />(must recover additional creative cost):</label></div>                    
                <div style="float:left;"><input type="text" id="textProdInstall" style="width:100px;" /><span class="requiredIndicator">*</span></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textProdInstallAdditional">Prod &amp; Install<br />(unlimited creatives):</label></div>                    
                <div style="float:left;"><input type="text" id="textProdInstallAdditional" style="width:100px;" /></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="dropDownLocationStatus">Status:</label></div>                    
                <div style="float:left;"><select id="dropDownLocationStatus"></select></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;">Reserve Winter Months:</div>
                <div style="float:left;">
                    <input type="radio" id="radioReserveWinterMonthsYes" name="reserveWinterMonths" checked="checked" /><label for="radioReserveWinterMonthsYes">Yes</label>
                    <input type="radio" id="radioReserveWinterMonthsNo" name="reserveWinterMonths" checked="checked" /><label for="radioReserveWinterMonthsNo">No</label>
                </div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textLocationComments">Comments:</label></div>                    
                <div style="float:left;"><textarea id="textLocationComments" cols="40" rows="5"></textarea></div>
                <div style="clear:both"></div>
            </div>            
            <div style="margin-top:10px">
                <div style="float:left;margin-right:10px;"><span class="requiredIndicator">*</span> - indicates a required field.</div>
                <div style="clear:both"></div>
            </div>              
        </fieldset>            
        <div>
            <div style="float:right;margin-top:10px;">                    
                <a href="#" class="button" id="addLocationAdd">Add Location</a>
                <a href="#" class="button" id="addLocationUpdate">Update Location</a>
                <a href="#" class="button" id="addLocationCancel" onclick="CloseDialog('addEditLocationDialog');">Cancel</a>
            </div>
            <div style="clear:both"></div>
        </div>  
    </div>
    <div id="addEditBookingDialog">
        <fieldset style="padding:10px">
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textBookingDesc">Booking Desc:</label></div>                    
                <div style="float:left;"><input type="text" id="textBookingDesc" style="width:100px;" /><span class="requiredIndicator">*</span></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textStartDate">Start Date:</label></div>                    
                <div style="float:left;"><input type="text" id="textStartDate" style="width:100px;" class="datepicker" /><span class="requiredIndicator">*</span></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textEndDate">End Date:</label></div>                    
                <div style="float:left;"><input type="text" id="textEndDate" style="width:100px;" class="datepicker" /><span class="requiredIndicator">*</span></div>
                <div style="clear:both"></div>
            </div>            
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="dropDownBookingStatus">Status:</label></div>                    
                <div style="float:left;"><select id="dropDownBookingStatus"></select></div>
                <div style="clear:both"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="dropDownBookingAE">AE:</label></div>
                <div style="float:left;"><select id="dropDownBookingAE"></select></div>
                <div style="clear:both;"></div>
            </div>
            <div style="margin-bottom:10px">
                <div style="float:left;margin-right:10px;width:75px;"><label for="textBookingComments">Comments:</label></div>                    
                <div style="float:left;"><textarea id="textBookingComments" cols="40" rows="5"></textarea></div>
                <div style="clear:both"></div>
            </div>            
            <div style="margin-top:10px">
                <div style="float:left;margin-right:10px;"><span class="requiredIndicator">*</span> - indicates a required field.</div>
                <div style="clear:both"></div>
            </div>              
        </fieldset>            
        <div>
            <div style="float:right;margin-top:10px;">                    
                <a href="#" class="button" id="addBookingAdd">Add Booking</a>
                <a href="#" class="button" id="addBookingUpdate">Update Booking</a>
                <a href="#" class="button" id="addBookingDelete">Delete Booking</a>
                <a href="#" class="button" id="addBookingCancel" onclick="CloseDialog('addEditBookingDialog');">Cancel</a>
            </div>
            <div style="clear:both"></div>
        </div>  
    </div>
    <div style="margin:25px;">
        <span style="font-size:16px;font-weight:bold;">Station Domination & Specialty Media Avail's</span>
        <div id="bookingDisplay">
            <div style="margin-bottom:15px;">
                <div style="float:left;margin:5px 10px 0 0;">
                    <label for="market" style="margin-right:5px;">Market:</label>
                    <select id="market">
                        <option value="" selected="selected"> - All Markets - </option>
                        <option value="Amtrak">Amtrak</option>
                        <option value="Boston">Boston</option>
                        <option value="Chicago">Chicago</option>
                        <option value="Dallas">Dallas</option>
                        <option value="Minneapolis">Minneapolis</option>
                        <option value="New Jersey">New Jersey</option>
                        <option value="New York">New York</option>
                        <option value="Philadelphia">Philadelphia</option>
                        <option value="Pittsburgh">Pittsburgh</option>
                        <option value="San Francisco">San Francisco</option>
                        <option value="Seattle">Seattle</option>                    
                        <option value="Washington DC">Washington DC</option>                   
                    </select>
                </div>
                <div style="float:left;margin:5px 10px 0 0;">
                    <label for="year" style="margin-right:5px;">Year:</label>
                    <select id="year">
                        <option value="2009">2009</option>
                        <option value="2010">2010</option>
                        <option value="2011">2011</option>
                        <option value="2012">2012</option>
                        <option value="2013">2013</option>
                    </select>
                </div>
                <div style="float:left;margin:1px 10px 0 0;"><input type="button" class="button" id="displayBookings" value="View Bookings" onclick="GenerateAvailsTableNew();" /></div>
                <div style="float:left;margin:1px 10px 0 0;"><input type="button" class="button" id="addNewLocation" value="Add Location" onclick="AddLocation();" /></div>
                <div style="float:left;margin-top:1px;"><asp:Button ID="exportBookings" 
                        runat="server" CssClass="button" OnClientClick="return ExportToExcel();" 
                        Text="Export to Excel" onclick="exportBookings_Click" /></div>
                <div style="clear:both"></div>
            </div>
            <div id="keyDisplay" style="margin:10px 0;display:none;">
                <div style="float:left;margin-right:10px;">
                    <table>
                        <thead><tr><th colspan='5'>Key</th></tr></thead>
                        <tbody>
                            <tr>
                                <td class="statusClass1">Specialty Media</td><td class="statusClass2">Under Construction</td>
                                <td class="statusClass3">Contracted</td><td class="statusClass4">Hold</td>
                                <td class="statusClass5">First Rights</td>
                            </tr>
                            <tr>
                                <td class="statusClass6" colspan="5">Winter Months<br />Cold Weather Restrictions may apply.</td>
                            </tr>
                        </tbody>                    
                    </table>
                </div>
                <div style="float:left;">
                    <span id="instructions"></span>
                </div>
                <div style="clear:both;"></div>
            </div>            
            <div id="availsDisplay" style="width:95%">                
            </div>
        </div>
    </div>
</asp:Content>

