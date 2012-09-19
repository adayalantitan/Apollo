var editingLocationId;
function GenerateAvailsTable() {
    var yearSelected = $("#year").val();
    var marketSelected = $("#market").val();
    Apollo.ProductionIOService.GetAvails(yearSelected, marketSelected, GenerateAvailsTableCallback, ErrorCallback);
}
function GenerateAvailsTableCallback(results) {
    $("#bookingDisplayTable").remove();
    //$("#availsDisplay #winterMonthMessage").remove();
    $("#availsDisplay").html("");
    $("#availsDisplay").append("<table id='bookingDisplayTable' class='featureTable scrollableFixedHeaderTable'></table>");
    var prevMarket;
    var yearSelected = $("#year").val();
    var headerRow = "<thead><tr class='bookingTableHeader'>"
        + ((IsEditUser()) ? "<th colspan='" + (IsAdminUser() ? "3" : "2") + "'>&nbsp;</th>" : "")
        + "<th>Market</th><th>Location</th><th>4-Week Rate Card</th><th>Prod &amp; Install<br/>(must recover<br/>additional creative<br/>cost)</th><th>Prod &amp; Install<br/>(unlimited creatives)</th>"
        + "<th colspan='4'>Jan - " + yearSelected + "</th><th colspan='4'>Feb - " + yearSelected + "</th><th colspan='4'>Mar - " + yearSelected + "</th>"
        + "<th colspan='4'>Apr - " + yearSelected + "</th><th colspan='4'>May - " + yearSelected + "</th><th colspan='4'>Jun - " + yearSelected + "</th>"
        + "<th colspan='4'>Jul - " + yearSelected + "</th><th colspan='4'>Aug - " + yearSelected + "</th><th colspan='4'>Sep - " + yearSelected + "</th>"
        + "<th colspan='4'>Oct - " + yearSelected + "</th><th colspan='4'>Nov - " + yearSelected + "</th><th colspan='4'>Dec - " + yearSelected + "</th>"
        + "</tr></thead>";
    $("#bookingDisplayTable").append(headerRow);
    headerRow = "<tr class='bookingTableHeader'>"
        + ((IsEditUser()) ? "<td colspan='" + (IsAdminUser() ? "3" : "2") + "'>&nbsp;</td>" : "")
        + "<td>Market</td><td>Location</td><td>4-Week Rate Card</td><td>Prod &amp; Install<br/>(must recover<br/>additional creative<br/>cost)</td><td>Prod &amp; Install<br/>(unlimited creatives)</td>"
        + "<td colspan='4'>Jan - " + yearSelected + "</td><td colspan='4'>Feb - " + yearSelected + "</td><td colspan='4'>Mar - " + yearSelected + "</td>"
        + "<td colspan='4'>Apr - " + yearSelected + "</td><td colspan='4'>May - " + yearSelected + "</td><td colspan='4'>Jun - " + yearSelected + "</td>"
        + "<td colspan='4'>Jul - " + yearSelected + "</td><td colspan='4'>Aug - " + yearSelected + "</td><td colspan='4'>Sep - " + yearSelected + "</td>"
        + "<td colspan='4'>Oct - " + yearSelected + "</td><td colspan='4'>Nov - " + yearSelected + "</td><td colspan='4'>Dec - " + yearSelected + "</td>"
        + "</tr>";
    var footerRow = "<tr class='bookingTableFooter'><td colspan='56'>IMPORTANT NOTE:<br/>- All production costs are subject to tax.<br/>- Production cost accounts for (1) art mechanical per media format. Any additional art mechanicals will incur a <u>fee of $100</u>.<br/>- Files are due <u>30 days</u> prior to the starting date to guarantee on-time posting. Titan will not be responsible for late postings if files are delivered after the due date.<br/>- Station Saturation packages will incur additional installation charges. Cost varies with media. Please contact <a href='mailto:production@titan360.com' alt='Mail To: production@titan360.com'>production@titan360.com</a> for quote.</td></tr>";
    $("#bookingDisplayTable").append("<tbody>");
    var availsRow = "";
    prevMarket = results[0].locationMarket;
    for (var i = 0; i < results.length; i++) {
        if (prevMarket != results[i].locationMarket) {
            $("#bookingDisplayTable").append(footerRow);
            $("#bookingDisplayTable").append(headerRow);
            prevMarket = results[i].locationMarket;
        }
        availsRow = "<tr class='availRow'>"
            + (IsAdminUser() ? "<td style='text-align:center;'><input type='button' class='button' id='deleteLocation_" + i + "' value='Delete Location' onclick='DeleteLocation(" + results[i].locationId + ");' /></td>" : "")
            + ((IsEditUserForMarket(results[i].locationMarket)) ? "<td style='text-align:center;'><input type='button' class='button' id='editLocation_" + i + "' value='Edit Location' /></td><td style='text-align:center;'><input type='button' class='button' onclick='AddBooking(" + results[i].locationId + ");' value='Add Booking' /></td>" : (IsEditUser() ? "<td colspan='2'>&nbsp;</td>" : ""))
            + "<td style='text-align:center;'>" + results[i].locationMarket + "</td><td class='" + (results[i].locationStatusId != "" ? ("statusClass" + results[i].locationStatusId) : "") + "'>" + results[i].locationDesc + (results[i].locationReserveWinterMonths ? " ***" : "") + "</td>"
            + "<td style='text-align:right;'>" + results[i].fourWeekRate + "</td><td style='text-align:right;'>" + results[i].prodInstallRate + "</td><td style='text-align:right;'>" + results[i].prodInstallRateAdditional + "</td>"
        //+ "<td colspan='12'>" + results[i].bookingDesc + "</td>"
            + BuildBookingDisplay(results[i].locationBookings, results[i].locationReserveWinterMonths)
            + "</tr>";
        $("#bookingDisplayTable").append(availsRow);
        $("#editLocation_" + i).bind("click", results[i], EditLocation);
    }
    $("#bookingDisplayTable").append(footerRow);
    $("#bookingDisplayTable").append("</tbody>");
    $(".button").button();
    $("#keyDisplay").css({ "display": "block" });
}
function GetColSpan(daysInMonth, daysUsed) {
    /*
    var perc = parseInt((daysUsed / daysInMonth) * 100);
    if (perc >= 75) { return 3; }
    if (perc >= 50) { return 2; }
    return 1;
    */
    if (daysUsed >= 21) { return 3; }
    if (daysUsed >= 14) { return 2; }
    return 1;
    //return 3;
}
function AreDatesEqual(d1, d2) {
    return (d1.getFullYear() == d2.getFullYear()
                && d1.getMonth() == d2.getMonth()
                && d1.getDate() == d2.getDate())
}
function GetLastDayOfMonth(month, year) {
    switch (month) {
        case 3:
        case 5:
        case 8:
        case 10:
            return 30;
        case 1: return (year % 4 == 0 ? 29 : 28);
        default: return 31;
    }
}
function DisplayBookings() {
    GenerateAvailsTable();
}
function ClearLocation() {
    $("#dropDownMarket").val("");
    $("#textLocation").val("");
    $("#textFourWeekRate").val("");
    $("#textProdInstall").val("");
    $("#dropDownLocationStatus").val("");
    $("#textLocationComments").val("");
}
function ValidateLocation() {
    var validationErrors = "Please correct the following error(s):";
    var isValid = true;
    if ($("#dropDownMarket").val() == "") {
        validationErrors += "\n\tPlease choose a Market.";
        isValid = false;
    }
    if ($("#textLocation").val() == "") {
        validationErrors += "\n\tPlease enter a Location.";
        isValid = false;
    }
    if ($("#textFourWeekRate").val() == "") {
        validationErrors += "\n\tPlease enter a Four Week Rate Card Value.";
        isValid = false;
    }
    if ($("#textProdInstall").val() == "") {
        validationErrors += "\n\tPlease enter a Production & Installation Value.";
        isValid = false;
    }
    if (!isValid) { alert(validationErrors); }
    return isValid;
}
function AddLocation() {
    ClearLocation();
    $("#addLocationAdd").removeClass("hiddenButton");
    $("#addLocationUpdate").addClass("hiddenButton");
    $("#addLocationAdd").unbind("click");
    $("#addLocationAdd").click(function () {
        if (!ValidateLocation()) { return; }
        var updatedLocationObject = { locationId: -1
            , locationMarket: $("#dropDownMarket").val()
            , locationDesc: $("#textLocation").val()
            , fourWeekRate: $("#textFourWeekRate").val()
            , prodInstallRate: $("#textProdInstall").val()
            , prodInstallRateAdditional: $("#textProdInstallAddtional").val()
            , locationStatusId: $("#dropDownLocationStatus").val()
            , locationStatus: ""
            , locationComments: $("#textLocationComments").val()
            , reserveWinterMonths: ($("#radioReserveWinterMonthsYes").attr("checked") !== undefined)
        };
        Apollo.ProductionIOService.AddUpdateLocation(updatedLocationObject, AddUpdateLocationCallback, ErrorCallback);
    });
    OpenDialog("addEditLocationDialog");
}
function EditLocation(locationObject) {
    ClearLocation();
    editingLocationId = locationObject.data.locationId;
    $("#addLocationAdd").addClass("hiddenButton");
    $("#addLocationUpdate").removeClass("hiddenButton");
    $("#dropDownMarket").val(locationObject.data.locationMarket);
    $("#dropDownMarket").attr("disabled", !IsEditUserForMarket(locationObject.data.locationMarket));
    $("#textLocation").val(locationObject.data.locationDesc);
    $("#textFourWeekRate").val(locationObject.data.fourWeekRate);
    $("#textProdInstall").val(locationObject.data.prodInstallRate);
    $("#textProdInstallAdditional").val(locationObject.data.prodInstallRateAdditional);
    $("#dropDownLocationStatus").val(locationObject.data.locationStatusId);
    $("#textLocationComments").val(locationObject.data.locationComments);
    $("#addLocationUpdate").unbind("click");
    $("#addLocationUpdate").click(function () {
        if (!ValidateLocation()) { return; }
        var updatedLocationObject = { locationId: editingLocationId
            , locationMarket: $("#dropDownMarket").val()
            , locationDesc: $("#textLocation").val()
            , fourWeekRate: $("#textFourWeekRate").val()
            , prodInstallRate: $("#textProdInstall").val()
            , prodInstallRateAdditional: $("#textProdInstallAdditional").val()
            , locationStatusId: $("#dropDownLocationStatus").val()
            , locationStatus: ""
            , locationComments: $("#textLocationComments").val()
            , reserveWinterMonths: ($("#radioReserveWinterMonthsYes").attr("checked") !== undefined)
        };
        Apollo.ProductionIOService.AddUpdateLocation(updatedLocationObject, AddUpdateLocationCallback, ErrorCallback);
    });
    OpenDialog("addEditLocationDialog");
}
function DeleteLocation(locationId) {
    if (confirm("Are you sure you want to delete this location?")) {
        Apollo.ProductionIOService.DeleteLocation(locationId, DeleteLocationCallback, ErrorCallback);
    }
}
function DeleteLocationCallback() {
    DisplayBookings();
    alert("Location deleted.");
}
function AddUpdateLocationCallback() {
    DisplayBookings();
    alert("Location saved.");
    CloseDialog("addEditLocationDialog");
}
function ValidateBooking() {
    var validationErrors = "Please correct the following error(s):";
    var isValid = true;
    if ($("#textBookingDesc").val() == "") {
        validationErrors += "\n\tPlease enter a Booking Description.";
        isValid = false;
    }
    if ($("#textStartDate").val() == "") {
        validationErrors += "\n\tPlease enter a Start Date.";
        isValid = false;
    }
    if ($("#textEndDate").val() == "") {
        validationErrors += "\n\tPlease enter an End Date.";
        isValid = false;
    }
    if ($("#dropDownBookingAE").val() == "" || $("#dropDownBookingAE").val() == "-1") {
        validationErrors += "\n\tPlease choose an AE.";
        isValid = false;
    }
    var startDateAsDate = new Date($("#textStartDate").val());
    var endDateAsDate = new Date($("#textEndDate").val());
    if (AreDatesEqual(startDateAsDate, endDateAsDate)) { validationErrors += "\n\tBooking Dates cannot be equal."; isValid = false; }
    if (startDateAsDate > endDateAsDate) { validationErrors += "\n\tEnd Date must be greather than Start Date."; isValid = false; }
    if (!isValid) { alert(validationErrors); }
    return isValid;
}
function ClearBooking() {
    $("#textBookingDesc").val("");
    $("#textStartDate").val("");
    $("#textEndDate").val("");
    $("#dropDownBookingStatus").val("");
    $("#textBookingComments").val("");
}
function AddBooking(locationId) {
    ClearBooking();
    $("#addBookingUpdate").addClass("hiddenButton");
    $("#addBookingDelete").addClass("hiddenButton");
    $("#addBookingAdd").removeClass("hiddenButton");
    $("#addBookingAdd").unbind("click");
    $("#addBookingAdd").click(function () {
        if (!ValidateBooking()) { return; }
        var bookingObject = { locationId: locationId
            , bookingId: -1
            , bookingDesc: $("#textBookingDesc").val()
            , bookingStartDate: $("#textStartDate").val()
            , bookingEndDate: $("#textEndDate").val()
            , bookingStatusId: $("#dropDownBookingStatus").val()
            , bookingComments: $("#textBookingComments").val()
            , aeNtId: $("#dropDownBookingAE").val()
        };
        Apollo.ProductionIOService.AddUpdateBooking(bookingObject, AddUpdateBookingCallback, ErrorCallback);
    });
    $("#addBookingAdd").bind("click", locationId, SaveNewBooking);
    OpenDialog("addEditBookingDialog");
}
function SaveNewBooking(submittedData) {
    var locationId = submittedData.data.locationId;
}
function EditBookingById(bookingId) {
    Apollo.ProductionIOService.LoadBookingById(bookingId, EditBookingByIdCallback, ErrorCallback);
}
function EditBookingByIdCallback(bookingObject) {
    EditBookings(bookingObject);
}
function EditBookings(bookingObject) {
    ClearBooking();
    $("#addBookingUpdate").removeClass("hiddenButton");
    $("#addBookingAdd").addClass("hiddenButton");
    $("#addBookingDelete").removeClass("hiddenButton");
    OpenDialog("addEditBookingDialog");
    $("#textBookingDesc").val(bookingObject.bookingDesc);
    //$("#textStartDate").val(bookingObject.bookingStartDate.toDateString());
    $("#textStartDate").datepicker("setDate", new Date(bookingObject.bookingStartDateYear, bookingObject.bookingStartDateMonth, bookingObject.bookingStartDateDay));
    //$("#textEndDate").val(bookingObject.bookingEndDate.toDateString());
    $("#textEndDate").datepicker("setDate", new Date(bookingObject.bookingEndDateYear, bookingObject.bookingEndDateMonth, bookingObject.bookingEndDateDay));
    $("#dropDownBookingStatus").val(bookingObject.bookingStatusId);
    $("#textBookingComments").val(bookingObject.bookingComments);
    $("#dropDownBookingAE").val(bookingObject.aeNtId);
    if (IsEditUser()) {
        var locationId = bookingObject.locationId;
        var bookingId = bookingObject.bookingId;
        $("#addBookingUpdate").unbind("click");
        $("#addBookingUpdate").click(function () {
            if (!ValidateBooking()) { return; }
            var bookingObject = { locationId: locationId
                , bookingId: bookingId
                , bookingDesc: $("#textBookingDesc").val()
                , bookingStartDate: $("#textStartDate").val()
                , bookingEndDate: $("#textEndDate").val()
                , bookingStatusId: $("#dropDownBookingStatus").val()
                , bookingComments: $("#textBookingComments").val()
                , aeNtId: $("#dropDownBookingAE").val()
            };
            Apollo.ProductionIOService.AddUpdateBooking(bookingObject, AddUpdateBookingCallback, ErrorCallback);
        });
        $("#addBookingDelete").unbind("click");
        $("#addBookingDelete").click(function () {
            if (confirm("Are you sure you want to delete this Booking?")) {
                Apollo.ProductionIOService.DeleteBooking(bookingId, DeleteBookingCallback, ErrorCallback);
            }
        });
    } else {
        $("#addBookingUpdate").addClass("hiddenButton");
        $("#addBookingAdd").addClass("hiddenButton");
        $("#addBookingDelete").addClass("hiddenButton");
    }
    OpenDialog("addEditBookingDialog");
}
function DeleteBookingCallback() {
    alert("Booking deleted.");
    DisplayBookings();
    CloseDialog("addEditBookingDialog");
}
function AddUpdateBookingCallback() {
    alert("Booking saved.");
    DisplayBookings();
    CloseDialog("addEditBookingDialog");
}
function CloseDialog(dialogId) { $('#' + dialogId).dialog("close"); }
function OpenDialog(dialogId) { $('#' + dialogId).dialog("open"); }
function BuildBookingDisplay(bookings, reserveWinterMonths) {
    //if (bookings.length == 0) { return "<td colspan='48' style='text-align:center;'>No Bookings</td>"; }
    //Each booking area will have 48 cells, 4 for each month
    var bookingStartDate, bookingEndDate;
    var monthStartDate, monthEndDate;
    var row = "";
    var monthCell = "";
    var cellClass = "";
    var monthDisplay = {};

    for (var i = 0; i < 12; i++) {        
        var monthOpen = false;
        var monthOpenCols = 0;
        var totalOpenCols = 0;
        monthStartDate = new Date($("#year").val(), i, 1);
        monthEndDate = new Date(monthStartDate);
        monthEndDate.setMonth(monthEndDate.getMonth() + 1);
        monthEndDate.setDate(monthEndDate.getDate() - 1);
        monthCell = "";
        for (var j = 0; j < bookings.length; j++) {
            //bookingStartDate = bookings[j].bookingStartDate;
            bookingStartDate = new Date(bookings[j].bookingStartDateYear, bookings[j].bookingStartDateMonth, bookings[j].bookingStartDateDay);
            //bookingEndDate = bookings[j].bookingEndDate;
            bookingEndDate = new Date(bookings[j].bookingEndDateYear, bookings[j].bookingEndDateMonth, bookings[j].bookingEndDateDay);
            cellClass = (bookings[j].bookingStatusId != "" ? ("statusClass" + bookings[j].bookingStatusId) : "");
            //If the booking starts after the current month, break out of the loop
            if (bookingStartDate.getMonth() > monthStartDate.getMonth() && bookingStartDate.getYear() >= monthStartDate.getYear()) { break; }
            //If the booking ends before the current month, continue to the next booking
            if (bookingEndDate.getMonth() < monthEndDate.getMonth() && bookingEndDate.getYear() <= monthEndDate.getYear()) { continue; }
            /* Check to see if this booking spans the entire month */
            if (AreDatesEqual(bookingStartDate, monthStartDate) && AreDatesEqual(bookingEndDate, monthEndDate)) {
                monthCell = "<td colspan='4' style='text-align:center;' class='" + cellClass + "' onclick='EditBookingById(" + bookings[j].bookingId + ");' >" + bookings[j].bookingDesc + "</td>";
                continue;
            }
            if (AreDatesEqual(bookingStartDate, monthStartDate) && bookingEndDate > monthEndDate) {
                monthCell = "<td colspan='4' style='text-align:center;padding-right:none !important;border-right:none !important;' class='" + cellClass + "' onclick='EditBookingById(" + bookings[j].bookingId + ");' >" + bookings[j].bookingDesc + "</td>";
                continue;
            }
            if (bookingStartDate < monthStartDate && AreDatesEqual(bookingEndDate, monthEndDate)) {
                monthCell = "<td colspan='4' style='text-align:center;padding-left:none !important;border-left:none !important;' class='" + cellClass + "' onclick='EditBookingById(" + bookings[j].bookingId + ");' >" + bookings[j].bookingDesc + "</td>";
                continue;
            }
            if (bookingStartDate < monthStartDate && bookingEndDate > monthEndDate) {
                monthCell = "<td colspan='4' style='text-align:center;padding-left:none !important; padding-right:none !important;border-left:none !important;border-right:none !important;' class='" + cellClass + "' onclick='EditBookingById(" + bookings[j].bookingId + ");' >" + bookings[j].bookingDesc + "</td>";
                continue;
            }
            /* Check to see if this booking only runs during part of the month */
            if (bookingStartDate < monthStartDate && bookingEndDate.getMonth() == monthStartDate.getMonth()) {//Partial booking starting in previous month, ending in current
                //What percentage of the month is "used" - from the start of the month
                monthOpen = true;
                monthOpenCols = GetColSpan(monthEndDate.getDate(), (bookingEndDate.getDate() - monthStartDate.getDate()));
                totalOpenCols += monthOpenCols;
                monthCell = "<td colspan='" + monthOpenCols + "' style='text-align:center;padding-left:none !important; padding-right:none !important;border-left:none !important;border-right:none !important;' class='" + cellClass + "' onclick='EditBookingById(" + bookings[j].bookingId + ");' >" + bookings[j].bookingDesc + "</td>";
                //monthCell += "<td colspan='" + (4 - GetColSpan(monthEndDate.getDate(), bookingEndDate.getDate())) + "'>&nbsp;</td>";
                continue;
            }
            if (bookingStartDate.getMonth() == monthStartDate.getMonth() && (AreDatesEqual(bookingStartDate, monthStartDate) || bookingStartDate > monthStartDate) && bookingEndDate >= monthEndDate) {//Partial booking starting in current month, ending in future month
                //What percentage of the month is "used"
                monthOpen = false;
                monthOpenCols = GetColSpan(monthEndDate.getDate(), (monthEndDate.getDate() - bookingStartDate.getDate()));
                totalOpenCols += monthOpenCols;
                if (monthCell == "") {
                    monthCell = "<td colspan='" + (4 - monthOpenCols) + "'>&nbsp;</td>";
                } else {
                    if (totalOpenCols < 4) {
                        monthCell += "<td colspan='" + (4 - totalOpenCols) + "'>&nbsp;</td>";
                    }
                }
                monthCell += "<td colspan='" + monthOpenCols + "' style='text-align:center;padding-left:none !important; padding-right:none !important;border-left:none !important;border-right:none !important;' class='" + cellClass + "' onclick='EditBookingById(" + bookings[j].bookingId + ");' >" + bookings[j].bookingDesc + "</td>";
                //monthOpen = (totalOpenCols < 4);
                continue;
            }
            //if ((AreDatesEqual(bookingStartDate, monthStartDate) && bookingEndDate < monthEndDate) || (bookingStartDate > monthStartDate && AreDatesEqual(bookingEndDate, monthEndDate))) {//Booking starting in current month and ending in current month
            if (bookingStartDate >= monthStartDate && bookingEndDate <= monthEndDate) {//Booking starting in current month and ending in current month
                monthOpen = true;
                monthOpenCols = GetColSpan(monthEndDate.getDate(), (bookingEndDate.getDate() - bookingStartDate.getDate()));
                totalOpenCols += monthOpenCols;
                monthCell += "<td colspan='" + monthOpenCols + "' style='text-align:center;padding-left:none !important; padding-right:none !important;' class='" + cellClass + "' onclick='EditBookingById(" + bookings[j].bookingId + ");' >" + bookings[j].bookingDesc + "</td>";
                monthOpen = (totalOpenCols < 4);
                continue;
            }
        }
        //if we get here and still have an open month...close it
        if (monthOpen) { monthCell += "<td colspan='" + (4 - totalOpenCols) + "'>&nbsp;</td>"; }
        if (monthCell == "" && (i == 0 || i == 1 || i == 10 || i == 11) && reserveWinterMonths) {
            row += "<td colspan='4' class='statusClass6'>Cold Weather Restrictions may apply</td>";
        } else {
            row += (monthCell == "" ? "<td colspan='4'>&nbsp;</td>" : monthCell);
        }
    }
    return row;
}