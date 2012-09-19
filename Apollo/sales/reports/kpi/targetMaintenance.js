function startWait(msg) { $.blockUI({ theme: true, draggable: false, title: 'Please Wait', message: '<p>' + msg + '...</p>' }); }
function endWait() { $.unblockUI(); }
function UpdateCallback() { alert("Data updated."); }
function ErrorCallback(e) { alert(e._message); try { endWait(); } catch (e) { } }
function OpenReportOptionsDialog() { $("#reportOptionsDialog").dialog("open"); }
function CancelReport() { $("#reportOptionsDialog").dialog("close"); }
function roundNumber(num, dec) { return Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec); }
function getAmountInThousands(amount) {
    if (Math.round(amount * 0.001) < 0) {
        return ("<span style='color:red'>" + getCurrencyString(Math.round(amount * 0.001)) + "</span>");
    } else {
        return getCurrencyString(Math.round(amount * 0.001));
    }
}
function getPercentage(amount, amountTotal) {
    if (amount < 0) { return "0 %"; }
    return (Math.round(100 * (amount / amountTotal)) + " %");
}
function getComparePercentage1(amount, amountTotal, comparePercentage) {
    var percent = Math.round(100 * (amount / amountTotal));
    /*
    if (percent <= comparePercentage) {
        return ("<span style='background-color:green;color:white;'>" + comparePercentage + " %</span>");
    } else {
        return ("<span style='background-color:red;color:white;'>" + comparePercentage + " %</span>");
    }
    */
    return ("" + comparePercentage + " %");
}
function getComparePercentage2(amount, amountTotal, comparePercentage) {
    var percent = Math.round(100 * (amount / amountTotal));
    /*
    if (percent >= comparePercentage) {
        return ("<span style='background-color:green;color:white;'>" + comparePercentage + " %</span>");
    } else {
        return ("<span style='background-color:red;color:white;'>" + comparePercentage + " %</span>");
    }
    */
    return ("" + comparePercentage + " %");
}
function getComparePercentage(amount, amountTotal, comparePercentage, reverse) {
    var percent = Math.round(100 * (amount / amountTotal));
    if (percent < comparePercentage && !reverse) {
        return ("<span style='background-color:green;color:white;'>" + comparePercentage + " %</span>");
    } else {
        return ("<span style='background-color:red;color:white;'>" + comparePercentage + " %</span>");
    }
}
function getCurrencyString(numericValue) {
    //TODO: Make this prettier
    if (numericValue == 0 || numericValue == '0') { return '0'; }
    var numericValueAsString = ("" + ((numericValue < 0) ? (numericValue * -1) : numericValue));
    var valueWithCommasRev = "";
    var valueWithCommas = "";
    var commaCounter = 0;
    if (numericValueAsString.length > 3) {
        for (var i = 0; i < numericValueAsString.length; i++) {
            valueWithCommasRev += numericValueAsString.charAt((numericValueAsString.length - 1) - i);
            commaCounter++;
            if (commaCounter == 3) { valueWithCommasRev += ","; commaCounter = 0; }
        }
        for (var i = 0; i < valueWithCommasRev.length; i++) {
            valueWithCommas += valueWithCommasRev.charAt((valueWithCommasRev.length - 1) - i);
        }
    } else { valueWithCommas = numericValueAsString; }
    return ((numericValue >= 0) ? ("$" + valueWithCommas) : ("($" + valueWithCommas + ")"));
}
function onMarketChange(sender, e) {
    if ($(this).val() == "") {
        $("#labelMarket").html("");
        $("#runReport").css({ "display": "none" });
        $("#marketDataTable, #functionButtons").css({ "display": "none" });
    }
    companyId = ($(this).val() == "TOR") ? 2 : 1;
    $("#labelMarket").html($("#dropDownMarket option:selected").text());
    $("#runReport").css({ "display": "block" });
    Apollo.KPIService.GetMarketTargets($("#dropDownMarket").val(), companyId, LoadMarketDataCallback, ErrorCallback);
}
function onDirectChange(sender, e) {
    if (!IsValidNumber($(this).val())) {
        alert("Please enter a valid numeric value.");
        $(this).css({ "background-color": "red" });
        $(this).val("");
        $(this).focus();
        return;
    }
    $(this).css({ "background-color": "green" });
}
function onAgencyChange(sender, e) {
    if (!IsValidNumber($(this).val())) {
        alert("Please enter a valid numeric value.");
        $(this).css({ "background-color": "red" });
        $(this).val("");
        $(this).focus();
        return;
    }
    $(this).css({ "background-color": "green" });
}
function LoadMarketDataCallback(marketData) {
    if (marketData == null) {
        selectedTargetId = -1;
        selectedMarketData = {};
        $("input[id^='textDirect']").val("");
        $("input[id^='textAgency']").val("");
    } else {
        selectedMarketData = marketData;
        selectedTargetId = marketData.targetId;
        $("#textDirectUnder60").val(marketData.directBucket1);
        $("#textDirect61To120").val(marketData.directBucket2);
        $("#textDirectOver120").val(marketData.directBucket3);
        $("#textAgencyUnder60").val(marketData.agencyBucket1);
        $("#textAgency61To120").val(marketData.agencyBucket2);
        $("#textAgencyOver120").val(marketData.agencyBucket3);
    }
    $("#marketDataTable, #functionButtons").css({ "display": "block" });
}  
function Validate() {
    if ($("#textDirectUnder60").val() == "" || $("#textDirect61To120").val() == "" || $("#textDirect61To120").val() == "") {
        alert("All Direct Target % must be completed.");
        return false;
    }
    var totalPercentage = 0;
    totalPercentage += parseInt($("#textDirectUnder60").val(), 10);
    totalPercentage += parseInt($("#textDirect61To120").val(), 10);
    totalPercentage += parseInt($("#textDirectOver120").val(), 10);
    if (totalPercentage != 100) {
        alert("Total Direct percentage must be equal to 100%");
        return false;
    }
    if ($("#textAgencyUnder60").val() == "" || $("#textAgency61To120").val() == "" || $("#textAgencyOver120").val() == "") {
        alert("All Agency Target % must be completed.");
        return false;
    }
    totalPercentage = 0;
    totalPercentage += parseInt($("#textAgencyUnder60").val(), 10);
    totalPercentage += parseInt($("#textAgency61To120").val(), 10);
    totalPercentage += parseInt($("#textAgencyOver120").val(), 10);
    if (totalPercentage != 100) {
        alert("Total Agency percentage must be equal to 100%");
        return false;
    }
    return true;
}
function Save() {
    if (!Validate()) { return; }
    selectedMarketData.companyId = companyId;
    selectedMarketData.marketId = $("#dropDownMarket").val();
    selectedMarketData.directBucket1 = parseInt($("#textDirectUnder60").val(), 10);
    selectedMarketData.directBucket2 = parseInt($("#textDirect61To120").val(), 10);
    selectedMarketData.directBucket3 = parseInt($("#textDirectOver120").val(), 10);
    selectedMarketData.directBucket4 = 0;
    selectedMarketData.directBucket5 = 0;
    selectedMarketData.directBucket6 = 0;
    selectedMarketData.agencyBucket1 = parseInt($("#textAgencyUnder60").val(), 10);
    selectedMarketData.agencyBucket2 = parseInt($("#textAgency61To120").val(), 10);
    selectedMarketData.agencyBucket3 = parseInt($("#textAgencyOver120").val(), 10);
    selectedMarketData.agencyBucket4 = 0;
    selectedMarketData.agencyBucket5 = 0;
    selectedMarketData.agencyBucket6 = 0;
    if (selectedTargetId == -1) {//Adding a new record
        Apollo.KPIService.AddMarketTargets(selectedMarketData, AddNewCallback, ErrorCallback);
    } else {
        Apollo.KPIService.UpdateMarketTargets(selectedMarketData, UpdateCallback, ErrorCallback);
    }
}
function AddNewCallback(targetId) {
    selectedMarketData.targetId = targetId;
    selectedTargetId = targetId;
    alert("Data saved.");
}
function PrintReport() {
    var todaysDate = GetDateAsString(new Date());
    var market = $("#dropDownMarket option:selected").text();
    var w = window.open('', 'printKPIdetails2', 'width=750,height=600,scrollbars=yes,resizable=yes,menubar=yes');
    w.document.write('<html><head>');
    w.document.write('  <title>' + market + ' Market AR %</title>');
    w.document.write('  <link rel="Stylesheet" href="/Styles/StyleSheet.css?v=07" />');
    w.document.write('  <link rel="Stylesheet" href="includes/jqueryUI/css/custom-theme/jquery-ui-1.8.20.custom.css" />');
    w.document.write('  <link rel="Stylesheet" href="includes/jqGrid/ui.jqgrid.css" />');
    w.document.write('  <link rel="stylesheet" href="includes/cluetip/jquery.cluetip.custom.css" type="text/css" />');
    w.document.write('  <style type="text/css">');
    w.document.write('     #marketArVsTargetData {width:750px;margin:0 auto;}');
    w.document.write('     #marketArVsTargetData tr {padding:2px;border-bottom:1px solid #333333;}');
    w.document.write('     #marketArVsTargetData th {font-weight:bold;font-size:12px;padding:3px;}');
    w.document.write('     #marketArVsTargetData td {padding:3px;}');
    w.document.write('     #marketArVsTargetData td {border-bottom:1px solid #333333;}');
    w.document.write('     #marketArVsTargetData span {text-align:right;}');
    w.document.write('     .numericCell {text-align:right;}');
    w.document.write('     .numeric{text-align:right;width:35px;}');
    w.document.write('     .dataEntry{background-color:green;color:white;font-weight:bold;font-size:10px;}');
    w.document.write('     h2 {font-size:14px !important;font-weight:bold;}');
    w.document.write('  </style>');
    w.document.write('</head>');
    w.document.write('<body><div style="margin:10px">');
    w.document.write('  <div style="width:750px;margin:3px auto;display:block;"><img src="/images/header_logo.jpg" alt="Titan" title="Titan" style="display:block;margin:0 auto;" /></div>');
    w.document.write('  <div style="width:750px;margin:5px auto;display:block;text-align:center;"><h2>' + market + ' AR %</h2></div>');
    w.document.write('  <div style="width:750px;margin:3px auto;display:block;text-align:center;"><h4>Data as of ' + todaysDate + '</h4><br/><br/></div>');
    w.document.write('  <div stlye="width:750px;margin:0 auto;display:block;"><table id="marketArVsTargetData">' + $("#marketArVsTargetData").html() + '</table></div>');
    w.document.write('</body>');
    w.document.write('</html>');
    w.document.close();
    w.focus();
}
function RunReportCallback(reportData) {
    var directTotal = reportData.marketTargetReportData[1].bucket1Amount + reportData.marketTargetReportData[1].bucket2Amount
                + reportData.marketTargetReportData[1].bucket3Amount + reportData.marketTargetReportData[1].bucket4Amount
                + reportData.marketTargetReportData[1].bucket5Amount + reportData.marketTargetReportData[1].bucket6Amount;
    var agencyTotal = reportData.marketTargetReportData[0].bucket1Amount + reportData.marketTargetReportData[0].bucket2Amount
                + reportData.marketTargetReportData[0].bucket3Amount + reportData.marketTargetReportData[0].bucket4Amount
                + reportData.marketTargetReportData[0].bucket5Amount + reportData.marketTargetReportData[0].bucket6Amount;
    var total = directTotal + agencyTotal;
    //Populate the Market Breakdown Table
    //Directs
    $("#labelDirectCurrent").html(getAmountInThousands(reportData.marketTargetReportData[1].bucket1Amount));
    $("#labelDirectCurrentPerc").html(getPercentage(reportData.marketTargetReportData[1].bucket1Amount, directTotal));
    $("#labelDirect3160").html(getAmountInThousands(reportData.marketTargetReportData[1].bucket2Amount));
    $("#labelDirect3160Perc").html(getPercentage(reportData.marketTargetReportData[1].bucket2Amount, directTotal));
    $("#labelDirect6190").html(getAmountInThousands(reportData.marketTargetReportData[1].bucket3Amount));
    $("#labelDirect6190Perc").html(getPercentage(reportData.marketTargetReportData[1].bucket3Amount, directTotal));
    $("#labelDirect91120").html(getAmountInThousands(reportData.marketTargetReportData[1].bucket4Amount));
    $("#labelDirect91120Perc").html(getPercentage(reportData.marketTargetReportData[1].bucket4Amount, directTotal));
    $("#labelDirect121150").html(getAmountInThousands(reportData.marketTargetReportData[1].bucket5Amount));
    $("#labelDirect121150Perc").html(getPercentage(reportData.marketTargetReportData[1].bucket5Amount, directTotal));
    $("#labelDirectOver150").html(getAmountInThousands(reportData.marketTargetReportData[1].bucket6Amount));
    $("#labelDirectOver150Perc").html(getPercentage(reportData.marketTargetReportData[1].bucket6Amount, directTotal));
    //Agencies
    $("#labelAgencyCurrent").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket1Amount));
    $("#labelAgencyCurrentPerc").html(getPercentage(reportData.marketTargetReportData[0].bucket1Amount, agencyTotal));
    $("#labelAgency3160").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket2Amount));
    $("#labelAgency3160Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket2Amount, agencyTotal));
    $("#labelAgency6190").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket3Amount));
    $("#labelAgency6190Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket3Amount, agencyTotal));
    $("#labelAgency91120").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket4Amount));
    $("#labelAgency91120Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket4Amount, agencyTotal));
    $("#labelAgency121150").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket5Amount));
    $("#labelAgency121150Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket5Amount, agencyTotal));
    $("#labelAgencyOver150").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket6Amount));
    $("#labelAgencyOver150Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket6Amount, agencyTotal));
    //Totals
    $("#labelTotalCurrent").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket1Amount + reportData.marketTargetReportData[1].bucket1Amount));
    $("#labelTotalCurrentPerc").html(getPercentage(reportData.marketTargetReportData[0].bucket1Amount + reportData.marketTargetReportData[1].bucket1Amount, total));
    $("#labelTotal3160").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket2Amount + reportData.marketTargetReportData[1].bucket2Amount));
    $("#labelTotal3160Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket2Amount + reportData.marketTargetReportData[1].bucket2Amount, total));
    $("#labelTotal6190").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket3Amount + reportData.marketTargetReportData[1].bucket3Amount));
    $("#labelTotal6190Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket3Amount + reportData.marketTargetReportData[1].bucket3Amount, total));
    $("#labelTotal91120").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket4Amount + reportData.marketTargetReportData[1].bucket4Amount));
    $("#labelTotal91120Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket4Amount + reportData.marketTargetReportData[1].bucket4Amount, total));
    $("#labelTotal121150").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket5Amount + reportData.marketTargetReportData[1].bucket5Amount));
    $("#labelTotal121150Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket5Amount + reportData.marketTargetReportData[1].bucket5Amount, total));
    $("#labelTotalOver150").html(getAmountInThousands(reportData.marketTargetReportData[0].bucket6Amount + reportData.marketTargetReportData[1].bucket6Amount));
    $("#labelTotalOver150Perc").html(getPercentage(reportData.marketTargetReportData[0].bucket6Amount + reportData.marketTargetReportData[1].bucket6Amount, total));
    //Grand Totals
    $("#labelDirectTotal").html(getAmountInThousands(directTotal));
    $("#labelAgencyTotal").html(getAmountInThousands(agencyTotal));
    $("#labelAllTotal").html(getAmountInThousands(total));

    //Populate the Market vs. Target Table
    //Direct
    $("#labelVsDirectUnder60").html(getPercentage(reportData.marketTargetReportData[1].bucket1Amount + reportData.marketTargetReportData[1].bucket2Amount, directTotal));
    $("#labelVsDirect61120").html(getPercentage(reportData.marketTargetReportData[1].bucket3Amount + reportData.marketTargetReportData[1].bucket4Amount, directTotal));
    $("#labelVsDirectOver120").html(getPercentage(reportData.marketTargetReportData[1].bucket5Amount + reportData.marketTargetReportData[1].bucket6Amount, directTotal));
    if (reportData.marketTargetData != null) {
        $("#labelVsDirectTargetUnder60").html(getComparePercentage2(reportData.marketTargetReportData[1].bucket1Amount + reportData.marketTargetReportData[1].bucket2Amount, directTotal, reportData.marketTargetData.directBucket1));
        $("#labelVsDirectTarget61120").html(getComparePercentage2(reportData.marketTargetReportData[1].bucket3Amount + reportData.marketTargetReportData[1].bucket4Amount, directTotal, reportData.marketTargetData.directBucket2));
        $("#labelVsDirectTargetOver120").html(getComparePercentage1(reportData.marketTargetReportData[1].bucket5Amount + reportData.marketTargetReportData[1].bucket6Amount, directTotal, reportData.marketTargetData.directBucket3));
    } else {
        $("#labelVsDirectTargetUnder60").html("0 %");
        $("#labelVsDirectTarget61120").html("0 %");
        $("#labelVsDirectTargetOver120").html("0 %");
    }
    //Agency
    $("#labelVsAgencyUnder60").html(getPercentage(reportData.marketTargetReportData[0].bucket1Amount + reportData.marketTargetReportData[0].bucket2Amount, agencyTotal));
    $("#labelVsAgency61120").html(getPercentage(reportData.marketTargetReportData[0].bucket3Amount + reportData.marketTargetReportData[0].bucket4Amount, agencyTotal));
    $("#labelVsAgencyOver120").html(getPercentage(reportData.marketTargetReportData[0].bucket5Amount + reportData.marketTargetReportData[0].bucket6Amount, agencyTotal));
    if (reportData.marketTargetData != null) {
        $("#labelVsAgencyTargetUnder60").html(getComparePercentage2(reportData.marketTargetReportData[0].bucket1Amount + reportData.marketTargetReportData[0].bucket2Amount, agencyTotal, reportData.marketTargetData.agencyBucket1));
        $("#labelVsAgencyTarget61120").html(getComparePercentage2(reportData.marketTargetReportData[0].bucket3Amount + reportData.marketTargetReportData[0].bucket4Amount, agencyTotal, reportData.marketTargetData.agencyBucket2));
        $("#labelVsAgencyTargetOver120").html(getComparePercentage1(reportData.marketTargetReportData[0].bucket5Amount + reportData.marketTargetReportData[0].bucket6Amount, agencyTotal, reportData.marketTargetData.agencyBucket3));
    } else {
        $("#labelVsAgencyTargetUnder60").html("0 %");
        $("#labelVsAgencyTarget61120").html("0 %");
        $("#labelVsAgencyTargetOver120").html("0 %");
    }
    endWait();
}