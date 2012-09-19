var monthList = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var minDate = new Date(0001, 0, 1);
var minDateAsString = "1/1/1";
function GetMarketDisplayName(market) {
    switch (market) {
        case "BOS": return "Boston";
        case "CHI": return "Chicago";
        case "DAL": return "Dallas";
        case "LA": return "Los Angeles";
        case "MIN": return "Minneapolis";
        case "NJ": return "New Jersey";
        case "NYO": return "New York";
        case "PHI": return "Philadelphia";
        case "SEA": return "Seattle";
        case "SF": return "San Francisco";
    }
}
function PopulateMonthDropDown() {
    var ddl2 = document.getElementById('dropDownReportMonth');
    var currentDate = new Date();
    for (var i = 0; i < 12; i++) {
        ddl2[ddl2.length] = new Option(monthList[i], i + 1, (currentDate.getMonth() == i), (currentDate.getMonth() == i));
    }
}
function PopulateYearDropDown() {
    var ddl2 = document.getElementById('dropDownReportYear');
    var currentDate = new Date();
    var yearStart = currentDate.getFullYear();
    var yearEnd = yearStart + 10;
    for (var i = yearStart - 2; i <= yearEnd; i++) {
        ddl2[ddl2.length] = new Option(i, i, (i == yearStart), (i == yearStart));
    }
}
function ErrorCallback(e) {
    endWait();
    alert(e._message);
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
function startWait(msg) { $.blockUI({ theme: true, draggable: false, title: 'Please Wait', message: '<p>' + msg + '...</p>' }); }
function endWait() { $.unblockUI(); }
function GetFieldValue(fieldId, defaultValue) {
    var theValue = $get(fieldId).value;
    return ((theValue == '') ? defaultValue : theValue);
}
function GetFieldHTML(fieldId, defaultValue) {
    var theHtml = $get(fieldId).innerHTML;
    return ((theHtml == '') ? defaultValue : theHtml);
}
function onTargetDSOChange(txt) { $("#labelTargetDSO").html($("#textTargetDso").val()); }
function KPIReportObject() {
    this.kpiReportData = {};
    this.InitializeReportData();
    //Data Objects
    this.customerBreakdownData = {};
    this.marketBreakdownData = {};
    this.agedBalanceData = {};
    this.agedBalanceDataByMarket = {};
    this.agedBalanceDataByCust = [];
    this.salesByMonthData = {};
    this.trendData = {};
    this.badDebtARBalanceData = {};
    this.workDaysData = {};
    this.monthValueHash = {};
    this.salesData = {};
    //Member Vars
    this.amountCollectedToDate = 0;
    this.workingDays = 0;
    this.workingDaysElapsed = 0;
    this.reportDate;
    this.projectedSalesTotal = 0;
    this.reportMonth;
    this.reportYear;
}
KPIReportObject.prototype.InitializeReportData = function () {
    this.kpiReportData = { reportMonth: 0, reportYear: 0, reportDate: '', targetCollectionAmount: 0, targetDSOAmount: 0, invoicingStartDate: "", invoicingWeek5Date: ""
        , projectedBillingWeek1: 0, projectedBillingWeek2: 0, projectedBillingWeek3: 0, projectedBillingWeek4: 0, projectedBillingWeek5: 0
        , actualInvoicingWeek1: "", actualInvoicingWeek2: "", actualInvoicingWeek3: "", actualInvoicingWeek4: "", actualInvoicingWeek5: ""
        , didNotPostWeek1: 0, didNotPostWeek2: 0, didNotPostWeek3: 0, didNotPostWeek4: 0, didNotPostWeek5: 0
        , catchUpWeek1: 0, catchUpWeek2: 0, catchUpWeek3: 0, catchUpWeek4: 0, catchUpWeek: 0
        , creditAppsReceived: 0, creditAppsChecked: 0
        , paymentGivenNum: 0, paymentGivenNumPercent: 0, paymentGivenAmount: 0, paymentGivenAmountPercent: 0
        , paymentMissingNum: 0, paymentMissingNumPercent: 0, paymentMissingAmount: 0, paymentMissingAmountPercent: 0
        , waivedNum: 0, waivedNumPercent: 0, waivedAmount: 0, waivedAmountPercent: 0
    };
}
KPIReportObject.prototype.ReportDateAsString = function () { return ((this.reportDate.getMonth() + 1) + "/" + this.reportDate.getDate() + "/" + this.reportDate.getFullYear()); }
KPIReportObject.prototype.StartOfReportMonthAsString = function () { return (this.reportMonth + "/1/" + this.reportYear); }