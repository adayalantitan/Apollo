/* Utility */
var shiftSelectedCells = [];
var selectedCellCampaignDetail;
var numberOfMonths = 12;
var startMonth, startYear;
var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var mondays = [];
var isPrintPreview = false;
function DateDisplay(date) { return (date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear()); }
function DetermineQuarter() {
    var currentMonth = new Date().getMonth();
    switch (parseInt(currentMonth, 10)) {
        case 0:
        case 1:
        case 2:
            return 1;
        case 3:
        case 4:
        case 5:
            return 2;
        case 6:
        case 7:
        case 8:
            return 3;
        default:
            return 4;
    }
}
function DetermineStartMonth(quarter) {
    switch (parseInt(quarter, 10)) {
        case 1: return 0;
        case 2: return 3;
        case 3: return 6;
        default: return 9;
    }
}
function CellHoverIn() { $(this).addClass("cellHover"); }
function CellHoverOut() { $(this).removeClass("cellHover"); }
function GetSelectedCell() { return GetCell(GetSelectedRow(), selectedCell.year, selectedCell.month, selectedCell.week); }
function GetCell(row, year, month, week) { return row.children("td[year='" + year + "'][month='" + month + "'][week='" + week + "']"); }
function GetSelectedRow() { return GetRow(selectedCell.stationSpotId); }
function GetRow(stationSpotId) { return $("tr[stationSpotId='" + stationSpotId + "']"); }
function GetWeeksBetweenDates(startDate, endDate) {
    var weeksInMs = 1000 * 60 * 60 * 24 * 7;
    var diffInMs = endDate.getTime() - startDate.getTime();
    return Math.ceil(diffInMs / weeksInMs);
}
function GetWeekStartDates(startMonth, startYear, numOfMonths) {
    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
    mondays = [];
    var startDate = new Date(startYear, startMonth, 1);
    var endDate = new Date(startYear, startMonth, 1);
    if (startMonth + numOfMonths > 11) {
        endDate.setMonth(startMonth - 12 + numOfMonths);
        endDate.setFullYear(endDate.getFullYear() + 1);
    } else {
        endDate.setMonth(startMonth + numOfMonths);
    }
    //find the first monday of the start month
    while (days[startDate.getDay()] != "Monday") {
        startDate.setDate(startDate.getDate() + 1);
    }
    //find the first monday of the end month
    while (days[endDate.getDay()] != "Monday") {
        endDate.setDate(endDate.getDate() + 1);
    }
    //find all the mondays
    while (startDate.getTime() < endDate.getTime()) {
        mondays.push(new Date(startDate.toDateString()));
        startDate.setDate(startDate.getDate() + 7);
    }
}
function GetNextQuarter() {
    var currentQuarter = parseInt($("#dropDownQuarter").val(), 10) + 1;
    var currentYear = parseInt($("#dropDownYear").val(), 10);
    if (currentQuarter > 4) {
        currentYear++;
        currentQuarter = 1;
    }
    $("#dropDownQuarter").val(currentQuarter);
    $("#dropDownYear").val(currentYear);
    GetAvails();
}
function GetPrevQuarter() {
    var currentQuarter = parseInt($("#dropDownQuarter").val(), 10) - 1;
    var currentYear = parseInt($("#dropDownYear").val(), 10);
    if (currentQuarter < 1) {
        currentYear--;
        currentQuarter = 4;
    }
    $("#dropDownQuarter").val(currentQuarter);
    $("#dropDownYear").val(currentYear);
    GetAvails();
}
function GetAvails() {
    if (!ValidateStationSelection()) { $("#stationInfo").css({ "display": "none" }); return; }
    numberOfMonths = parseInt($("#dropDownNumberOfMonths").val(), 10);
    startMonth = parseInt($("#dropDownMonth").val(), 10);
    startYear = parseInt($("#dropDownYear").val(), 10);
    var stationOptions = { stationId: $("#dropDownStation").val(), startMonth: startMonth, startYear: startYear, numberOfMonths: numberOfMonths };
    Apollo.DigitalAvailsService.GetStationInfoWithMatrix(stationOptions, GetStationInfoWithMatrixCallback, ErrorCallback);
}


/* Callbacks */
function GetStationInfoWithMatrixCallback(results) {
    stationData = results;
    $("#stationInfo").css({ "display": "block" });
    $("#stationNameDisplay").html(stationData.Name);
    $("#stationDescDisplay").html(stationData.Description);
    $("#njtKey").css({ "display": (stationData.Market == "New Jersey" ? "block" : "none") });
    $("#martaKey").css({ "display": (stationData.Market.indexOf("Atlanta") != -1 ? "block" : "none") });
    GetWeekStartDates((startMonth - 1), startYear, numberOfMonths);
    CreateTableHeaders();
}
function ModifySpotCallback(spotConflicts) {
    if (spotConflicts !== undefined && spotConflicts.length > 0) {
        var message = "";
        for (var i = 0; i < spotConflicts.length; i++) {
            message += "Booking conflict on: " + DateDisplay(spotConflicts[i].SpotDate);
            if (spotConflicts[i].StationName !== undefined && spotConflicts[i].StationName != "") {
                message += "\t@ " + spotConflicts[i].StationName;
            }
            message += "\n";
        }
        alert("There were conflicts detected while attempting to book the spot(s).\n\nThe following dates could not be booked:\n\n" + message + "\n\nAll remaining dates were booked successfully.");
    }
    GetAvails();
    $("#spotEntryDialog, #spotCopyDialog, #spotUpdateDeleteDialog, #spotMultiStationDialog").dialog("close");
}


/* Validation */
function ValidateCopySpot() {
    var isValid = true;
    var errorMessages = "";
    if (!IsValidNumber($("#textNumberOfWeeksCopy").val())) {
        isValid = false;
        errorMessages += "You must enter a valid number for the Number of Weeks.\n";
    } else {
        if (parseInt($("#textNumberOfWeeksCopy").val(), 10) < 1) {
            isValid = false;
            errorMessages += "Number of Weeks cannot be less than 1.\n";
        }
    }
    if (!isValid) { alert(errorMessages); return false; }
    return true;
}
function ValidateSpotEntry() {
    var isValid = true;
    var errorMessages = "";
    if ($("#textCampaignName").val() == "") {
        isValid = false;
        errorMessages += "You must enter a Campaign name.\n";
    }
    if ($("#dropDownSpotType").val() == "") {
        isValid = false;
        errorMessages += "You must choose a Spot Type.\n";
    }
    if (!IsValidNumber($("#textNumberOfWeeks").val())) {
        isValid = false;
        errorMessages += "You must enter a valid number for the Number of Weeks.\n";
    } else {
        if (parseInt($("#textNumberOfWeeks").val(), 10) < 1) {
            isValid = false;
            errorMessages += "Number of Weeks cannot be less than 1.\n";
        }
    }
    if (!isValid) { alert(errorMessages); return false; }
    return true;
}
function ValidateMarketSelection() {
    if ($("#dropDownMarket").val() == "") { alert("Please choose a Market."); return false; }
    return true;
}
function ValidateStationSelection() {
    if (!ValidateMarketSelection()) { return false; }
    if ($("#dropDownStation").val() == "") { alert("Please choose a Location."); return false; }
    return true;
}
/**** End Validation ****/


/* Events */
function OnCopyDialogSubmit(e) {
    if (!ValidateCopySpot()) { return; }
    var numberOfWeeks = parseInt($("#textNumberOfWeeksCopy").val(), 10);
    var spot = { stationId: stationData.StationId, stationSpotId: copiedCell.copyStationSpotId, spotTypeId: copiedCell.spotTypeId
        , year: copiedCell.year, month: copiedCell.month, week: copiedCell.week, spotDate: new Date(copiedCell.year, copiedCell.month - 1, copiedCell.day)
        , campaignName: copiedCell.campaignName, campaignNumber: copiedCell.campaignNumber, description: copiedCell.description        
    };
    if (numberOfWeeks >= 2) {
        Apollo.DigitalAvailsService.SaveMultipleSpots(spot, numberOfWeeks, ModifySpotCallback, ErrorCallback);
    } else {
        Apollo.DigitalAvailsService.SaveSpot(spot, ModifySpotCallback, ErrorCallback);
    }
}
function OnDialogSubmit(e) {
    if (!ValidateSpotEntry()) { return; }
    //remove the selected station from the additional station list
    $(".additionalStation").removeAttr("disabled");
    $("#additionalStationId_" + stationData.StationId).attr({ "checked": "checked", "disabled": "disabled" });
    $(".additionalStation[id!=additionalStationId_" + stationData.StationId + "], #toggleAllAdditionalStations").removeAttr("checked");
    $("#spotMultiStationDialog").dialog("destroy");
    $("#spotMultiStationDialog").dialog({ title: "Select affected Stations", modal: true, autoOpen: false, width: 400, height: 350 });
    $("#spotMultiStationDialog").dialog("option", "buttons", { "Cancel": function () { $(this).dialog("close"); }, "Add": AddSpot });
    $("#spotMultiStationDialog").dialog("open");
    // $("#spotEntryDialog").dialog("option", "buttons", { "Cancel": function () { $(this).dialog("close") }, "Add...": OnDialogSubmit });    
}
function AddSpot(e) {
    var campaignName = $("#textCampaignName").val();
    var campaignNumber = $("#textCampaignNumber").val();
    var spotType = parseInt($("#dropDownSpotType").val(), 10);
    var description = $("#textDescription").val();
    var numberOfWeeks = parseInt($("#textNumberOfWeeks").val(), 10);
    var spot = { stationId: stationData.StationId, stationSpotId: selectedCell.stationSpotId, spotTypeId: spotType
        , year: selectedCell.year, month: selectedCell.month, week: selectedCell.week
        , spotDate: new Date(selectedCell.year, selectedCell.month - 1, selectedCell.day)
        , campaignName: campaignName, campaignNumber: campaignNumber, description: description
    };
    //check for any other station selections
    var checkedStations = [];
    $(".additionalStation").each(function () {
        if ($(this).attr("checked") == "checked") {
            checkedStations.push(parseInt($(this).attr("id").split("_")[1], 10));
        }
    });
    //TODO: Refactor - Is it really necessary to have three different methods?
    if (checkedStations.length > 1) {
        Apollo.DigitalAvailsService.SaveMultiStationSpot(spot, numberOfWeeks, checkedStations, ModifySpotCallback, ErrorCallback);
    } else {
        if (numberOfWeeks >= 2) {
            Apollo.DigitalAvailsService.SaveMultipleSpots(spot, numberOfWeeks, ModifySpotCallback, ErrorCallback);
        } else {
            Apollo.DigitalAvailsService.SaveSpot(spot, ModifySpotCallback, ErrorCallback);
        }
    }
}
function OpenDeleteSubDialog(e) {
    OpenUpdateDeleteSubDialog(e, false);
}
function OpenUpdateSubDialog(e) {
    OpenUpdateDeleteSubDialog(e, true);
}
function OpenUpdateDeleteSubDialog(e, isUpdating) {
    $("#spotUpdateDeleteDialog").dialog();
    $("#spotUpdateDeleteDialog").dialog("destroy");
    $("#spotUpdateDeleteDialog").dialog({ title: (isUpdating ? "Update" : "Delete") + " Spots", modal: true, autoOpen: false, width: 400, height: 375 });
    var campaignName = $("#textCampaignName").val();
    var campaignNumber = $("#textCampaignNumber").val();
    var spotType = parseInt($("#dropDownSpotType").val(), 10);
    var description = $("#textDescription").val();
    var spot = { spotId: selectedCell.spotId
        , stationId: stationData.StationId, stationSpotId: selectedCell.stationSpotId, spotTypeId: spotType
        , year: selectedCell.year, month: selectedCell.month, week: selectedCell.week, spotDate: new Date(selectedCell.year, selectedCell.month - 1, selectedCell.day)
        , campaignName: campaignName, campaignNumber: campaignNumber, description: description
    };
    var prefix = (isUpdating ? "Update" : "Delete");
    if (isUpdating) {
        $("#spotUpdateDeleteDialog").dialog("option", "buttons", {
            "Cancel": function () { $(this).dialog("close"); }
            , "Update Spot": function (e) { Apollo.DigitalAvailsService.UpdateSpot(spot, ModifySpotCallback, ErrorCallback); }
            , "Update All": function (e) { OnUpdateDelete(e, spot, isUpdating, true, false, false); }
            , "Update All Subsequent": function (e) { OnUpdateDelete(e, spot, isUpdating, false, false, true); }
            , "Update All Previous": function (e) { OnUpdateDelete(e, spot, isUpdating, false, true, false); }            
        });
    } else {
        $("#spotUpdateDeleteDialog").dialog("option", "buttons", {
            "Cancel": function () { $(this).dialog("close"); }
            , "Delete Spot": function (e) { Apollo.DigitalAvailsService.DeleteSpot(selectedCell.spotId, DeleteSpotCallback, ErrorCallback); }
            , "Delete All": function (e) { OnUpdateDelete(e, spot, isUpdating, true, false, false); }
            , "Delete All Subsequent": function (e) { OnUpdateDelete(e, spot, isUpdating, false, false, true); }
            , "Delete All Previous": function (e) { OnUpdateDelete(e, spot, isUpdating, false, true, false); }            
        });
    }
    $("#spotUpdateDeleteDialog").dialog("open");
}
function OnUpdateDelete(e, spot, isUpdating, wantAll, wantPrevious, wantSubsequent) {
    var ids = [];
    if (wantAll) {
        ids = (selectedCellCampaignDetail.IDs == null ? [] : selectedCellCampaignDetail.IDs);
    } else if (wantPrevious) {
        ids = (selectedCellCampaignDetail.IDsBefore == null ? [] : selectedCellCampaignDetail.IDsBefore);
    } else if (wantSubsequent) {
        ids = (selectedCellCampaignDetail.IDsAfter == null ? [] : selectedCellCampaignDetail.IDsAfter);
    }
    if ($("#radioIncludeCurrentYes").attr("checked")) {
        ids.push(spot.spotId);
    }
    if (ids.length == 0) { alert("There are no " + (wantAll ? "" : (wantPrevious ? "previous " : "subsequent ")) + "spots to " + (isUpdating ? "update" : "delete")); }
    if (isUpdating) {
        Apollo.DigitalAvailsService.UpdateMultipleSpots(spot, ids, ModifySpotCallback, ErrorCallback);
    } else {
        Apollo.DigitalAvailsService.DeleteMultipleSpots(ids, DeleteSpotCallback, ErrorCallback);
    }
}
function OnDialogUpdate(e) {
    if (!ValidateSpotEntry()) { return; }
    OpenUpdateSubDialog();
    var campaignName = $("#textCampaignName").val();
    var campaignNumber = $("#textCampaignNumber").val();
    var spotType = parseInt($("#dropDownSpotType").val(), 10);
    var description = $("#textDescription").val();
    Apollo.DigitalAvailsService.UpdateSpot({ spotId: selectedCell.spotId
        , stationId: stationData.StationId, stationSpotId: selectedCell.stationSpotId, spotTypeId: spotType
        , year: selectedCell.year, month: selectedCell.month, week: selectedCell.week, spotDate: new Date(selectedCell.year, selectedCell.month - 1, selectedCell.day)
        , campaignName: campaignName, campaignNumber: campaignNumber, description: description        
    }, ModifySpotCallback, ErrorCallback);
}
function OnDialogDelete(e) {
    var cell = GetSelectedCell();
    cell.children().remove();
    Apollo.DigitalAvailsService.DeleteSpot(selectedCell.spotId, DeleteSpotCallback, ErrorCallback);
}
function DeleteSpotCallback() {
    $("#spotEntryDialog, #spotUpdateDeleteDialog").dialog("close");
    GetAvails();
}
function OnEndDateChange(dateText, inst) {
    this.value = dateText;
    $("#textNumberOfWeeks").val(GetWeeksBetweenDates(new Date(selectedCell.year, selectedCell.month - 1, selectedCell.day), new Date(dateText)));
}
function OnMarketChange(sender, e) {
    Apollo.DigitalAvailsService.GetSpotTypesByMarket($("#dropDownMarket").val(), "", AddToList, null, "dropDownSpotType");
    Apollo.DigitalAvailsService.GetStationByMarket($("#dropDownMarket").val(), "", GetStationByMarketCallback, null, "dropDownStation");
}
function OnUtilizationMarketChange(sender, e) {
    if ($("#dropDownUtilizationMarket").val() == "") { $("#dropDownUtilizationYear").hide();  return; }
    $("#dropDownUtilizationYear").show();
    Apollo.DigitalAvailsService.GetReportYearList($("#dropDownUtilizationMarket").val(), AddToList, null, "dropDownUtilizationYear");
}
function GetStationByMarketCallback(list) {
    AddToList(list, "dropDownStation");
    //Also need to build the list of station checkboxes
    var stationId, stationName;
    var stationCheck = "";
    $(".stationLi").remove();
    for (var i = 1; i < list.length; i++) {
        stationId = list[i].value;
        stationName = list[i].name;
        stationCheck += "<li class='stationLi' id='stationId_" + stationId + "'><input type='checkbox' id='additionalStationId_" + stationId + "' class='additionalStation' />" + stationName + "</li>";
    }
    $("#additionalStationList").append(stationCheck);
}
function ToggleAllAdditionalStations() {
    var show = $(this).attr("checked");
    $(".additionalStation").each(function () {
        if ($(this).attr("disabled") === undefined) { $(this).attr("checked", (show === undefined ? false : "checked")); }
    });
    //$(".additionalStation").attr("checked", (show === undefined ? false : "checked"));
}
function CellClickNew(event) {
    if (event.shiftKey) { return; }
    if (event.ctrlKey) { return; }
    //var cell = $(this);
    CellClick($(this));
}
function CellClick(cell) {
    if (!CanEdit()) { return; }
    var cell = $(cell);
    var spotNumDisplay = "";
    selectedCell = { year: parseInt(cell.attr("year"), 10), month: parseInt(cell.attr("month"), 10), week: parseInt(cell.attr("week"), 10), day: parseInt(cell.attr("day"), 10) };
    selectedCell.idPrefix = selectedCell.year + "_" + selectedCell.month + "_" + selectedCell.week;
    selectedCell.stationSpotId = parseInt(cell.parent("tr").attr("stationSpotId"), 10);
    selectedCell.stationSpotName = cell.parent("tr").attr("stationSpotName");
    spotNumDisplay = selectedCell.stationSpotName;
    $("#textTotalNumberOfWeeks").html("");
    $("#textDlgTotalNumberOfWeeks").html("");
    if (cell.html() != "" && cell.html() != "&nbsp;") {
        selectedCell.spotId = cell.find("input[id$='_spotId']").val();
        selectedCell.spotTypeId = cell.find("input[id$='_spotType']").val();
        selectedCell.campaignName = cell.find("span[id$='_campaignName']").text();  //cell.find("span[id$='_campaignName']").html();
        selectedCell.campaignNumber = cell.find("input[id$='_campaignNumber']").val();
        selectedCell.description = cell.find("input[id$='_description']").val();
        selectedCell.idPrefix = selectedCell.spotId + "_" + selectedCell.idPrefix;
        InitDialog(true);
        var spot = { stationId: stationData.StationId, stationSpotId: selectedCell.stationSpotId, spotTypeId: selectedCell.spotTypeId
            , year: selectedCell.year, month: selectedCell.month, week: selectedCell.week
            , spotDate: new Date(selectedCell.year, selectedCell.month - 1, selectedCell.day)
            , campaignName: selectedCell.campaignName, description: selectedCell.description
        };        
        Apollo.DigitalAvailsService.GetSpotCampaignDetail(spot, GetSpotCampaignDetailCallback, ErrorCallback);
    } else {
        InitDialog(false);
    }
    $("#textSpotNumber").html(spotNumDisplay);
    $("#spotEntryDialog").dialog("open");
    //Due to FF 4 bug with clearing out textareas, clear/populate elements after opening dialog
    $("#textCampaignName").val("");
    $("#textCampaignNumber").val("");
    $("#dropDownSpotType").val("");
    $("#textDescription").val("").text("");
    $("#textNumberOfWeeks").val("1");
    if (cell.html() != "" && cell.html() != "&nbsp;") {
        $("#textCampaignName").val(selectedCell.campaignName);
        $("#dropDownSpotType").val(selectedCell.spotTypeId);
        $("#textDescription").text(selectedCell.description);
        $("#textDescription").val(selectedCell.description);
        $("#textCampaignNumber").val(selectedCell.campaignNumber);
    }
}
function GetSpotCampaignDetailCallback(spotCampaignDetail) {
    selectedCellCampaignDetail = spotCampaignDetail;
    var totalWeeksText = "Total Weeks: " + spotCampaignDetail.TotalWeeks
        + "<br/>Weeks Before this Spot: " + spotCampaignDetail.WeeksBefore
        + "<br/>Weeks After this Spot: " + spotCampaignDetail.WeeksAfter;
    $("#textTotalNumberOfWeeks").html(totalWeeksText);
    $("#textDlgTotalNumberOfWeeks").html(totalWeeksText);
}
function OnSpotCellDrop(event, ui) {
    copiedCell = { year: parseInt($(this).attr("year"), 10), month: parseInt($(this).attr("month"), 10), week: parseInt($(this).attr("week"), 10), day: parseInt($(this).attr("day"), 10) };
    copiedCell.idPrefix = copiedCell.year + "_" + copiedCell.month + "_" + copiedCell.week;
    copiedCell.spotId = $(ui.draggable).find("input[id$='_spotId']").val();
    copiedCell.spotTypeId = $(ui.draggable).find("input[id$='_spotType']").val();
    copiedCell.campaignName = $(ui.draggable).find("span[id$='_campaignName']").html();
    copiedCell.campaignNumber = $(ui.draggable).find("input[id$='_campaignNumber']").val();
    copiedCell.description = $(ui.draggable).find("input[id$='_description']").val();
    copiedCell.idPrefix = copiedCell.spotId + "_" + copiedCell.idPrefix;
    copiedCell.copyStationSpotId = parseInt($(this).parent("tr").attr("stationSpotId"), 10);
    copiedCell.copySpotName = $(this).parent("tr").attr("stationSpotName");
    //Open Copy Dialog
    $("#textNumberOfWeeksCopy").val(1);
    $("#textCopyToPosition").html(copiedCell.spotName);
    $("#spotCopyDialog").dialog("open");
}
/**** End Events ****/

/**** Campaign Search ****/
function LaunchCampaignSearch() {
    var dialog = $("#campaignSearchDialog");
    $("#textCampaignSearch").val("");
    $("#campaignSearchDialog").dialog("destroy");
    $("#campaignSearchDialog").dialog({ title: "Search for Campaign", modal: true, autoOpen: false, width: 550, height: 425 });
    $("#campaignSearchDialog").dialog("option", "buttons",
        {
            "Cancel": function () {
                $(this).dialog("close");
            },
            "Search": function () {
                if ($("#textCampaignSearch").val() == "") { alert("Enter a Campaign Name/Number to search for."); return; }
                Apollo.DigitalAvailsService.ExecuteCampaignSearch($("#textCampaignSearch").val(), ExecuteCampaignSearchCallback, ErrorCallback);
            }
        }
    );
    $("#campaignSearchDialog").dialog("open");
}
function ExecuteCampaignSearchCallback(searchResults) {
    var data = "";
    if (searchResults === undefined || searchResults == null || searchResults.length == 0) {
        data = "<tr><td colspan='7'>No results found.</td></tr>";
    } else {
        for (var i = 0; i < searchResults.length; i++) {
            data += "<tr><td>" + searchResults[i].NumberOfSpots + "</td>";
            data += "<td>" + searchResults[i].Market + "</td>";
            data += "<td>" + searchResults[i].StationName + "</td>";
            data += "<td>" + searchResults[i].CampaignName + "</td>";
            data += "<td>" + searchResults[i].CampaignNumber + "</td>";
            data += "<td>" + searchResults[i].MonthDisplay + "</td>";
            data += "<td>" + searchResults[i].Year + "</td></tr>";
        }
    }
    var table = "<table style='width:100%;'><thead><tr><th># of Spots</th><th>Market</th><th>Station</th><th>Campaign Name</th><th>Campaign #</th><th>Month</th><th>Year</th></tr></thead><tbody>" + data + "</tbody></table>";
    $("#campaignSearchResults").empty().append(table);
}
/**** End Campaign Search ****/


function CreateTableHeaders() {
    if (isPrintPreview) {
        $("#availsTable").html("");
    } else {
        $("#availsTable").fadeOut(400, function () {
            $("#availsTable").html("");
        });
    }
    var dividerRow = "<tr class='dividerRow'><td colspan='" + (mondays.length + 1) + "' class='dividerCell'>&nbsp;</td></tr>";
    var dateOutput = "<tr><th>Date</th>";
    var monthOutput = "<tr><th style='border:none !important;'>&nbsp;</th>";
    var dataRowOutput = "";
    var currentMonth = mondays[0].getMonth();
    var mondayInMonthCount = 0;
    for (var i = 0; i < mondays.length; i++) {
        dateOutput += "<th>" + DateDisplay(mondays[i]) + "</th>";
        if (currentMonth == mondays[i].getMonth()) {
            mondayInMonthCount++;
        } else {
            monthOutput += "<th colspan='" + mondayInMonthCount + "' class='monthHeader'>" + months[currentMonth] + "</th>";
            currentMonth = mondays[i].getMonth();
            mondayInMonthCount = 1;
        }
        dataRowOutput += "<td class='spotCell' "
            + " onclick='CellClick(this);'"
            + " year='" + mondays[i].getFullYear() + "' month='" + (mondays[i].getMonth() + 1) + "' week='" + mondayInMonthCount + "' day='" + mondays[i].getDate() + "'>&nbsp;</td>";
    }
    monthOutput += "<th colspan='" + mondayInMonthCount + "' class='monthHeader'>" + months[currentMonth] + "</th></tr>";
    dateOutput += "</tr>";
    var stationOutput = "";
    var stationSpot;
    var style;
    for (var k = 0; k < stationData.StationSpots.length; k++) {        
        stationSpot = stationData.StationSpots[k];
        if (stationSpot.DividerBefore) { stationOutput += dividerRow; }
        style = GenerateSpotBackgroundStyleMoz(stationSpot.BackgroundColor) + GenerateSpotBackgroundStyleWebkit(stationSpot.BackgroundColor);
        stationOutput += "<tr stationSpotId='" + stationSpot.StationSpotId + "' stationSpotName='" + stationSpot.Name + "'><td class='spotLabel' style='" + style + "'>" + stationSpot.Name + "</td>" + dataRowOutput + "</tr>";
        if (stationSpot.DividerAfter) { stationOutput += dividerRow; }
    }

    var breakdownData = dividerRow + CreateSpotTypeBreakdown(stationData.TotalBreakdownSpots, stationData.SpotTypeBreakdown);
    if (isPrintPreview) {
        $("#availsTable").append(monthOutput).append(dateOutput).append(stationOutput).append(breakdownData);
        GetSpotsFromMatrix();
        return;
    }
    $("#availsTable").fadeIn(300, function () {
        $("#availsTable").append(monthOutput).append(dateOutput).append(stationOutput).append(breakdownData);
        GetSpotsFromMatrix();
        if (!CanEdit()) { return; }
        $(".spotCell").hover(CellHoverIn, CellHoverOut);
        $(".spotCell").each(function () {
            if ($(this).html() == "" || $(this).html() == "&nbsp;") {
                $(this).droppable({ accept: ".spotEntry", activeClass: "ui-state-hover", hoverClass: "ui-state-active", drop: OnSpotCellDrop });
            }
        });
        $(".spotEntry").draggable({ snap: ".spotCell", snapMode: "inner", snapTolerance: 5, containment: "#availsTable", distance: 15, revert: true
            , start: function (event, ui) { ui.helper.bind("click.prevent", function (event) { event.preventDefault(); }); }
            , stop: function (event, ui) { setTimeout(function () { ui.helper.unbind("click.prevent"); }, 300); }
        });
    });
}
function GenerateSpotBackgroundStyleMoz(backgroundColor) {
    if (isPrintPreview) { return "background-color:#" + backgroundColor + ";"; }
    return "background: -moz-linear-gradient(center top, #EEEEEF 0%, #" + backgroundColor + " 100%);";
}
function GenerateSpotBackgroundStyleWebkit(backgroundColor) {
    if (isPrintPreview) { return "background-color:#" + backgroundColor + ";"; }
    return "background-image:-webkit-gradient(linear,left top,left bottom,color-stop(0.0, #EEEEEF),color-stop(1.0, #" + backgroundColor + "));";
}
function CreateSpotTypeBreakdown(numberOfSpots, spotTypeBreakdownData) {
    var breakDownData = "";
    var spotTypeBreakdownRecord, isPercentage, style;
    var weeklyUsage = [];
    //Print out the Breakdown rows for each Spot Type
    for (var i = 0; i < spotTypeBreakdownData.length; i++) {
        spotTypeBreakdownRecord = spotTypeBreakdownData[i];
        style = GenerateSpotBackgroundStyleMoz(spotTypeBreakdownRecord.Color) + GenerateSpotBackgroundStyleWebkit(spotTypeBreakdownRecord.Color);
        breakDownData += "<tr class='spotBreakdownRow'><td class='spotLabel' style='" + style + "'>" + spotTypeBreakdownRecord.SpotType + "</td>";
        var isPercentage = spotTypeBreakdownRecord.SpotType.indexOf("%") != -1;
        for (var j = 0; j < spotTypeBreakdownRecord.SpotTypeCount.length; j++) {
            breakDownData += "<td class='spotBreakdownCell' style='" + style + "'>" + spotTypeBreakdownRecord.SpotTypeCount[j] + (isPercentage ? " %" : "") + "</td>";
            if (!isPercentage) {
                if (weeklyUsage[j] === undefined) {
                    weeklyUsage.push(spotTypeBreakdownRecord.SpotTypeCount[j]);
                } else {
                    weeklyUsage[j] += spotTypeBreakdownRecord.SpotTypeCount[j];
                }
            }
        }
        breakDownData += "</tr>";
    }
    style = GenerateSpotBackgroundStyleMoz("BBBBBB") + GenerateSpotBackgroundStyleWebkit("BBBBBB");
    breakDownData += "<tr class='spotBreakdownRow'><td class='spotLabel' style='" + style + "'>Total Remaining Spots</td>";
    for (var i = 0; i < weeklyUsage.length; i++) {
        breakDownData += "<td class='spotBreakdownCell' style='" + style + "'>" + (numberOfSpots - weeklyUsage[i]) + "</td>";
    }
    breakDownData += "</tr><tr class='spotBreakdownRow'><td class='spotLabel' style='" + style + "'>Total Available Spots</td>";
    for (var i = 0; i < spotTypeBreakdownData[0].SpotTypeCount.length; i++) {
        breakDownData += "<td class='spotBreakdownCell' style='" + style + "'>" + numberOfSpots + "</td>";
    }
    breakDownData += "</tr>";
    return breakDownData;
}
function GetSpotsFromMatrix() {
    if (stationData.SpotMatrix.length == 0) { return; }
    var spot, row, cell, html;
    for (var i = 0; i < stationData.SpotMatrix.length; i++) {
        spot = stationData.SpotMatrix[i];
        row = GetRow(spot.StationSpotId);
        cell = GetCell(row, spot.Year, spot.Month, spot.Week);
        html = GetSpotHtml(spot);
        cell.html(html);
    }
}
function GetSpotHtml(spot) {
    var idPrefix = spot.SpotId + "_" + spot.Year + "_" + spot.Month + "_" + spot.Week;
    var html = "<div class='spotEntry' id='" + idPrefix + "_spot' style='" + GenerateSpotBackgroundStyleMoz(spot.Color) + GenerateSpotBackgroundStyleWebkit(spot.Color) + "'>"
        + (spot.Description === undefined || spot.Description == null || spot.Description == "" ? "<div class='emptyNote'></div>" : "<div class='noteIndicator'></div>")
        + "<span id='" + idPrefix + "_campaignName'>" + spot.CampaignName + "</span>"
        + "<input type='hidden' id='" + idPrefix + "_spotId' value='" + spot.SpotId + "' />"
        + "<input type='hidden' id='" + idPrefix + "_spotType' value='" + spot.SpotTypeId + "' />"
        + "<input type='hidden' id='" + idPrefix + "_description' value='" + (spot.Description == null ? "" : spot.Description) + "' />"
        + "<input type='hidden' id='" + idPrefix + "_campaignNumber' value='" + (spot.CampaignNumber == null ? "" : spot.CampaignNumber) + "' />"
        + "</div>";
    return html;
}
function PrintPreview() {
    if (!ValidateStationSelection()) { return; }
    var w = window.open("", "printDigitalAvails", "width=600,height=600,scrollbars=1,resizable=1,menubar=1,statusbar=1,location=1,toolbar=1");
    w.document.write("<html><head>");
    w.document.write("<title>Digital Avails</title>");
    w.document.write("<link rel='Stylesheet' href='/Styles/StyleSheet.css' />");
    w.document.write("<link rel='Stylesheet' href='/digitalAvails/digitalAvailsPrint.css' />");
    w.document.write("</head>");
    w.document.write("<body>");
    w.document.write("<div style='margin:10px;'>");
    w.document.write($("#availsData").html());
    w.document.write("</div></body>");
    w.document.write("</html>");
    w.document.close();
    w.focus();
}