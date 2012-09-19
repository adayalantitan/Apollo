<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="daily_kpi.aspx.cs" Inherits="Apollo.sales_reports_daily_kpi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
    <style type="text/css">
        .hiddenRow {display:none}
    </style>
    <script type="text/javascript" language="javascript">
        var monthList = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
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
            this.kpiReportData = { globalReportParams: { reportMonth: 0, reportYear: 0, reportDate: '', targetCollectionAmount: 0, targetDSOAmount: 0, invoicingStartDate: '', invoicingWeek5Date: '' }
                , projectedBillingAmounts: { projectedBillingWeek1: 0, projectedBillingWeek2: 0, projectedBillingWeek3: 0, projectedBillingWeek4: 0, projectedBillingWeek5: 0 }
                , actualInvoicingDates: { actualInvoicingWeek1: '', actualInvoicingWeek2: '', actualInvoicingWeek3: '', actualInvoicingWeek4: '', actualInvoicingWeek5: '' }
                , didNotPostAmounts: { didNotPostWeek1: 0, didNotPostWeek2: 0, didNotPostWeek3: 0, didNotPostWeek4: 0, didNotPostWeek5: 0 }
                , catchUpAmounts: { catchUpWeek1: 0, catchUpWeek2: 0, catchUpWeek3: 0, catchUpWeek4: 0, catchUpWeek: 0 }
                , creditApps: { creditAppsReceived: 0, creditAppsChecked: 0 }
                , paymentsGiven: { paymentGivenNum: 0, paymentGivenNumPercent: 0, paymentGivenAmount: 0, paymentGivenAmountPercent: 0 }
                , paymentsMissing: { paymentMissingNum: 0, paymentMissingNumPercent: 0, paymentMissingAmount: 0, paymentMissingAmountPercent: 0 }
                , waived: { waivedNum: 0, waivedNumPercent: 0, waivedAmount: 0, waivedAmountPercent: 0 }
            };
        }        
        KPIReportObject.prototype.ReportDateAsString = function() { return ((this.reportDate.getMonth() + 1) + "/" + this.reportDate.getDate() + "/" + this.reportDate.getFullYear()); }
        KPIReportObject.prototype.StartOfReportMonthAsString = function() { return (this.reportMonth + "/1/" + this.reportYear); }
        KPIReportObject.prototype.ReportDataAsString = function() {
            var reportDataAsString = "";
            reportDataAsString = "{"
                + "globalReportParams:{reportMonth:" + this.kpiReportData.globalReportParams.reportMonth
                    + ",reportYear:" + this.kpiReportData.globalReportParams.reportYear
                    + ",reportDate:'" + this.kpiReportData.globalReportParams.reportDate + "'"
                    + ",targetCollectionAmount:" + this.kpiReportData.globalReportParams.targetCollectionAmount
                    + ",targetDSOAmount:" + this.kpiReportData.globalReportParams.targetDSOAmount
                    + ",invoicingStartDate:'" + this.kpiReportData.globalReportParams.invoicingStartDate + "'"
                    + ",invoicingWeek5Date:'" + this.kpiReportData.globalReportParams.invoicingWeek5Date + "'}"
                + ",projectedBillingAmounts:{projectedBillingWeek1:" + this.kpiReportData.projectedBillingAmounts.projectedBillingWeek1
                    + ",projectedBillingWeek2:" + this.kpiReportData.projectedBillingAmounts.projectedBillingWeek2
                    + ",projectedBillingWeek3:" + this.kpiReportData.projectedBillingAmounts.projectedBillingWeek3
                    + ",projectedBillingWeek4:" + this.kpiReportData.projectedBillingAmounts.projectedBillingWeek4
                    + ",projectedBillingWeek5:" + this.kpiReportData.projectedBillingAmounts.projectedBillingWeek5 + "}"
                + ",actualInvoicingDates:{actualInvoicingWeek1:'" + this.kpiReportData.actualInvoicingDates.actualInvoicingWeek1 + "'"
                    + ",actualInvoicingWeek2:'" + this.kpiReportData.actualInvoicingDates.actualInvoicingWeek2 + "'"
                    + ",actualInvoicingWeek3:'" + this.kpiReportData.actualInvoicingDates.actualInvoicingWeek3 + "'"
                    + ",actualInvoicingWeek4:'" + this.kpiReportData.actualInvoicingDates.actualInvoicingWeek4 + "'"
                    + ",actualInvoicingWeek5:'" + this.kpiReportData.actualInvoicingDates.actualInvoicingWeek5 + "'}"
                + ",didNotPostAmounts:{didNotPostWeek1:" + this.kpiReportData.didNotPostAmounts.didNotPostWeek1
                    + ",didNotPostWeek2:" + this.kpiReportData.didNotPostAmounts.didNotPostWeek2
                    + ",didNotPostWeek3:" + this.kpiReportData.didNotPostAmounts.didNotPostWeek3
                    + ",didNotPostWeek4:" + this.kpiReportData.didNotPostAmounts.didNotPostWeek4
                    + ",didNotPostWeek5:" + this.kpiReportData.didNotPostAmounts.didNotPostWeek5 + "}"
                + ",catchUpAmounts:{catchUpWeek1:" + this.kpiReportData.catchUpAmounts.catchUpWeek1
                    + ",catchUpWeek2:" + this.kpiReportData.catchUpAmounts.catchUpWeek2
                    + ",catchUpWeek3:" + this.kpiReportData.catchUpAmounts.catchUpWeek3
                    + ",catchUpWeek4:" + this.kpiReportData.catchUpAmounts.catchUpWeek4
                    + ",catchUpWeek5:" + this.kpiReportData.catchUpAmounts.catchUpWeek5 + "}"
                + ",creditApps:{creditAppsReceived:" + this.kpiReportData.creditApps.creditAppsReceived
                    + ",creditAppsChecked:" + this.kpiReportData.creditApps.creditAppsChecked + "}"
                + ",paymentsGiven:{paymentGivenNum:" + this.kpiReportData.paymentsGiven.paymentGivenNum
                    + ",paymentGivenNumPercent:" + this.kpiReportData.paymentsGiven.paymentGivenNumPercent
                    + ",paymentGivenAmount:" + this.kpiReportData.paymentsGiven.paymentGivenAmount
                    + ",paymentGivenAmountPercent:" + this.kpiReportData.paymentsGiven.paymentGivenAmountPercent + "}"
                + ",paymentsMissing:{paymentMissingNum:" + this.kpiReportData.paymentsMissing.paymentMissingNum
                    + ",paymentMissingNumPercent:" + this.kpiReportData.paymentsMissing.paymentMissingNumPercent
                    + ",paymentMissingAmount:" + this.kpiReportData.paymentsMissing.paymentMissingAmount
                    + ",paymentMissingAmountPercent:" + this.kpiReportData.paymentsMissing.paymentMissingAmountPercent + "}"
                + ",waived:{waivedNum:" + this.kpiReportData.waived.waivedNum
                    + ",waivedNumPercent:" + this.kpiReportData.waived.waivedNumPercent
                    + ",waivedAmount:" + this.kpiReportData.waived.waivedAmount
                    + ",waivedAmountPercent:" + this.kpiReportData.waived.waivedAmountPercent + "}"
                + "}";
            return reportDataAsString;
        }
        var kpiReportObject = new KPIReportObject();
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
        function startWait(msg) { $.blockUI({ theme: true, draggable: false, title: 'Please Wait', message: '<p>'+msg+'...</p>' }); }
        function endWait() { $.unblockUI(); }
        $(function() {
            $("#kpiTabs").tabs();
            $("#" + "<%=textReportDate.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            $("#" + "<%=textInvoiceThru1.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            $("#" + "<%=textInvoiceThru5.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            $("#" + "<%=textActualInvoice1.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            $("#" + "<%=textActualInvoice2.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            $("#" + "<%=textActualInvoice3.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            $("#" + "<%=textActualInvoice4.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            $("#" + "<%=textActualInvoice5.ClientID %>").datepicker({ dateFormat: 'm/d/yy' });
            PopulateMonthDropDown();
            PopulateYearDropDown();
            kpiReportObject.reportDate = new Date();
            kpiReportObject.reportMonth = $('#dropDownReportMonth').val();
            kpiReportObject.reportYear = $('#dropDownReportYear').val();
            $get('<%=textReportDate.ClientID %>').value = kpiReportObject.ReportDateAsString();
            $get('reportDateDisplay').innerHTML = kpiReportObject.ReportDateAsString();
            $get('labelARAsOfRptDate').innerHTML = kpiReportObject.ReportDateAsString();
            $("#labelARAsOfRptDate2").html(kpiReportObject.ReportDateAsString());
            //PopulateDataTables(kpiReportObject.StartOfReportMonthAsString(), kpiReportObject.ReportDateAsString());
        }); 
        function ReloadReportData() {
            $('#dropDownReportMonth').val(kpiReportObject.kpiReportData.globalReportParams.reportMonth);
            $('#dropDownReportYear').val(kpiReportObject.kpiReportData.globalReportParams.reportYear);
            kpiReportObject.reportDate = new Date(kpiReportObject.kpiReportData.globalReportParams.reportDate);
            $get('<%=textTargetCash.ClientID %>').value = kpiReportObject.kpiReportData.globalReportParams.targetCollectionAmount;
            $get('<%=textTargetDso.ClientID %>').value = kpiReportObject.kpiReportData.globalReportParams.targetDSOAmount;
            $get('<%=textInvoiceThru1.ClientID %>').value = kpiReportObject.kpiReportData.globalReportParams.invoicingStartDate;
            if (kpiReportObject.kpiReportData.globalReportParams.invoicingWeek5Date === undefined) {
                $get('<%=textInvoiceThru5.ClientID %>').value = "";
            } else {
                $get('<%=textInvoiceThru5.ClientID %>').value = kpiReportObject.kpiReportData.globalReportParams.invoicingWeek5Date;
            }            
            $get('<%=textProjectedBilling1.ClientID %>').value = kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek1;
            $get('<%=textProjectedBilling2.ClientID %>').value = kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek2;
            $get('<%=textProjectedBilling3.ClientID %>').value = kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek3;
            $get('<%=textProjectedBilling4.ClientID %>').value = kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek4;
            $get('<%=textProjectedBilling5.ClientID %>').value = kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek5;

            $get('<%=textActualInvoice1.ClientID %>').value = kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek1;
            $get('<%=textActualInvoice2.ClientID %>').value = kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek2;
            $get('<%=textActualInvoice3.ClientID %>').value = kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek3;
            $get('<%=textActualInvoice4.ClientID %>').value = kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek4;
            $get('<%=textActualInvoice5.ClientID %>').value = kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek5;

            $get('<%=textDidNotPost1.ClientID %>').value = kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek1;
            $get('<%=textDidNotPost2.ClientID %>').value = kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek2;
            $get('<%=textDidNotPost3.ClientID %>').value = kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek3;
            $get('<%=textDidNotPost4.ClientID %>').value = kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek4;
            $get('<%=textDidNotPost5.ClientID %>').value = kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek5;

            $get('<%=textCatchUp1.ClientID %>').value = kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek1;
            $get('<%=textCatchUp2.ClientID %>').value = kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek2;
            $get('<%=textCatchUp3.ClientID %>').value = kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek3;
            $get('<%=textCatchUp4.ClientID %>').value = kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek4;
            $get('<%=textCatchUp5.ClientID %>').value = kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek5;

            $get('<%=textCreditAppPercRec.ClientID %>').value = kpiReportObject.kpiReportData.creditApps.creditAppsReceived;
            $get('<%=textCreditAppPercChk.ClientID %>').value = kpiReportObject.kpiReportData.creditApps.creditAppsChecked;
            
            $get('<%=textPmtGivenNum.ClientID %>').value = kpiReportObject.kpiReportData.paymentsGiven.paymentGivenNum;
            $get('<%=textPmtGivenNumPerc.ClientID %>').value = kpiReportObject.kpiReportData.paymentsGiven.paymentGivenNumPercent;
            $get('<%=textPmtGivenAmt.ClientID %>').value = kpiReportObject.kpiReportData.paymentsGiven.paymentGivenAmount;
            $get('<%=textPmtGivenAmtPerc.ClientID %>').value = kpiReportObject.kpiReportData.paymentsGiven.paymentGivenAmountPercent;
            
            $get('<%=textPmtMissingNum.ClientID %>').value = kpiReportObject.kpiReportData.paymentsMissing.paymentMissingNum;
            $get('<%=textPmtMissingNumPerc.ClientID %>').value = kpiReportObject.kpiReportData.paymentsMissing.paymentMissingNumPercent;
            $get('<%=textPmtMissingAmt.ClientID %>').value = kpiReportObject.kpiReportData.paymentsMissing.paymentMissingAmount;
            $get('<%=textPmtMissingAmtPerc.ClientID %>').value = kpiReportObject.kpiReportData.paymentsMissing.paymentMissingAmountPercent;
            
            $get('<%=textWaivedNum.ClientID %>').value = kpiReportObject.kpiReportData.waived.waivedNum;
            $get('<%=textWaivedNumPerc.ClientID %>').value = kpiReportObject.kpiReportData.waived.waivedNumPercent;
            $get('<%=textWaivedAmt.ClientID %>').value = kpiReportObject.kpiReportData.waived.waivedAmount;
            $get('<%=textWaivedAmtPerc.ClientID %>').value = kpiReportObject.kpiReportData.waived.waivedAmountPercent;
        }
        function PackageReportData() {
            if (kpiReportObject.kpiReportData === undefined) { kpiReportObject.InitializeReportData(); }                                                            
            //Populate JSON Object
            kpiReportObject.kpiReportData.globalReportParams.reportMonth = parseInt($('#dropDownReportMonth').val(), 10);
            kpiReportObject.kpiReportData.globalReportParams.reportYear = parseInt($('#dropDownReportYear').val(), 10);            
            kpiReportObject.kpiReportData.globalReportParams.reportDate = GetFieldValue('<%=textReportDate.ClientID %>', kpiReportObject.ReportDateAsString());
            kpiReportObject.kpiReportData.globalReportParams.targetCollectionAmount = parseFloat(GetFieldValue('<%=textTargetCash.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.globalReportParams.targetDSOAmount = parseFloat(GetFieldValue('<%=textTargetDso.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.globalReportParams.invoicingStartDate = GetFieldValue('<%=textInvoiceThru1.ClientID %>', '');
            kpiReportObject.kpiReportData.globalReportParams.invoicingWeek5Date = GetFieldValue('<%=textInvoiceThru5.ClientID %>', '');
            kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek1 = parseFloat(GetFieldValue('<%=textProjectedBilling1.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek2 = parseFloat(GetFieldValue('<%=textProjectedBilling2.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek3 = parseFloat(GetFieldValue('<%=textProjectedBilling3.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek4 = parseFloat(GetFieldValue('<%=textProjectedBilling4.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.projectedBillingAmounts.projectedBillingWeek5 = parseFloat(GetFieldValue('<%=textProjectedBilling5.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek1 = GetFieldValue('<%=textActualInvoice1.ClientID %>', '');
            kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek2 = GetFieldValue('<%=textActualInvoice2.ClientID %>', '');
            kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek3 = GetFieldValue('<%=textActualInvoice3.ClientID %>', '');
            kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek4 = GetFieldValue('<%=textActualInvoice4.ClientID %>', '');
            kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek5 = GetFieldValue('<%=textActualInvoice5.ClientID %>', '');
            kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek1 = parseFloat(GetFieldValue('<%=textDidNotPost1.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek2 = parseFloat(GetFieldValue('<%=textDidNotPost2.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek3 = parseFloat(GetFieldValue('<%=textDidNotPost3.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek4 = parseFloat(GetFieldValue('<%=textDidNotPost4.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.didNotPostAmounts.didNotPostWeek5 = parseFloat(GetFieldValue('<%=textDidNotPost5.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek1 = parseFloat(GetFieldValue('<%=textCatchUp1.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek2 = parseFloat(GetFieldValue('<%=textCatchUp2.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek3 = parseFloat(GetFieldValue('<%=textCatchUp3.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek4 = parseFloat(GetFieldValue('<%=textCatchUp4.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.catchUpAmounts.catchUpWeek5 = parseFloat(GetFieldValue('<%=textCatchUp5.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.creditApps.creditAppsReceived = parseFloat(GetFieldValue('<%=textCreditAppPercRec.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.creditApps.creditAppsChecked = parseFloat(GetFieldValue('<%=textCreditAppPercChk.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.paymentsGiven.paymentGivenNum = parseInt(GetFieldValue('<%=textPmtGivenNum.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.paymentsGiven.paymentGivenNumPercent = parseInt(GetFieldValue('<%=textPmtGivenNumPerc.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.paymentsGiven.paymentGivenAmount = parseInt(GetFieldValue('<%=textPmtGivenAmt.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.paymentsGiven.paymentGivenAmountPercent = parseInt(GetFieldValue('<%=textPmtGivenAmtPerc.ClientID %>', 0), 10);

            kpiReportObject.kpiReportData.paymentsMissing.paymentMissingNum = parseInt(GetFieldValue('<%=textPmtMissingNum.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.paymentsMissing.paymentMissingNumPercent = parseInt(GetFieldValue('<%=textPmtMissingNumPerc.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.paymentsMissing.paymentMissingAmount = parseInt(GetFieldValue('<%=textPmtMissingAmt.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.paymentsMissing.paymentMissingAmountPercent = parseInt(GetFieldValue('<%=textPmtMissingAmtPerc.ClientID %>', 0), 10);

            kpiReportObject.kpiReportData.waived.waivedNum = parseInt(GetFieldValue('<%=textWaivedNum.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.waived.waivedNumPercent = parseInt(GetFieldValue('<%=textWaivedNumPerc.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.waived.waivedAmount = parseInt(GetFieldValue('<%=textWaivedAmt.ClientID %>', 0), 10);
            kpiReportObject.kpiReportData.waived.waivedAmountPercent = parseInt(GetFieldValue('<%=textWaivedAmtPerc.ClientID %>', 0), 10);
        }
        function SaveReportData() {
            startWait('Saving');
            PackageReportData();
            PageMethods.SaveReportData(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.reportMonth, kpiReportObject.reportYear, kpiReportObject.ReportDataAsString(), SaveCallbackSuccess, ErrorCallback);
        }
        function LoadReportData() {
            startWait('Loading');
            PageMethods.LoadReportData(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.reportMonth, kpiReportObject.reportYear, LoadCallbackSuccess, ErrorCallback);
        }
        function SaveCallbackSuccess() {
            endWait();
            alert('Report Data has been Saved.');
        }
        function LoadCallbackSuccess(reportData) {
            if (reportData == '') {
                PopulateDataTables(kpiReportObject.StartOfReportMonthAsString(), kpiReportObject.ReportDateAsString());
                onInvoiceTableChange();
                return;
            }
            try {                
                var reportDataAsObject = new Object();
                kpiReportObject.kpiReportData = new Object();                
                reportDataAsObject = eval(reportData);
                kpiReportObject.kpiReportData = reportDataAsObject[0];
                //Is the user requesting the report data as of a different date?
                if (kpiReportObject.reportDate != new Date(GetFieldValue('<%=textReportDate.ClientID %>', kpiReportObject.ReportDateAsString()))) {
                    kpiReportObject.reportDate = new Date(GetFieldValue('<%=textReportDate.ClientID %>', kpiReportObject.ReportDateAsString()));
                    kpiReportObject.kpiReportData.globalReportParams.reportDate = kpiReportObject.ReportDateAsString();
                }
            } catch (e) {
                alert('Could not bind report data.');
            }
            try {
                ReloadReportData();
                PopulateDataTables(kpiReportObject.StartOfReportMonthAsString(), kpiReportObject.ReportDateAsString());                
            } catch (e) {
                alert('An error occurred while trying to load the report data.');
            }
        }        
        function GetFieldValue(fieldId, defaultValue) {
            var theValue = $get(fieldId).value;
            return ((theValue == '') ? defaultValue : theValue);
        }
        function GetFieldHTML(fieldId, defaultValue) {
            var theHtml = $get(fieldId).innerHTML;
            return ((theHtml == '') ? defaultValue : theHtml);
        }        
        function onTargetDSOChange(txt) {
            $get('labelTargetDSO').innerHTML = $get('<%=textTargetDso.ClientID %>').value;
        }
        function onTargetCollectionChange(txt) {
            var targetCollectionAmount = $get('<%=textTargetCash.ClientID %>').value;
            if (targetCollectionAmount == '') { return; }

            var remainderToGo = (((Math.round(targetCollectionAmount - (Math.round(kpiReportObject.amountCollectedToDate * 0.001)))) < 0) ? -1 : 1) * Math.round(targetCollectionAmount - (Math.round(kpiReportObject.amountCollectedToDate * 0.001)));
            var remainderToGoDailyAverage = ((kpiReportObject.workingDays - kpiReportObject.workingDaysElapsed) != 0) ? Math.round(remainderToGo / (kpiReportObject.workingDays - kpiReportObject.workingDaysElapsed)) : 0;
            var targetCollectedPercentage = Math.round(((Math.round(kpiReportObject.amountCollectedToDate * 0.001)) / targetCollectionAmount) * 100);
            var daysPassedPerc = Math.round((kpiReportObject.workingDaysElapsed / kpiReportObject.workingDays) * 100);

            $get('labelTargetCollection').innerHTML = getCurrencyString(targetCollectionAmount);
            //Calculate
            $get('labelTargetDailyAverage').innerHTML = getCurrencyString(Math.round(targetCollectionAmount / kpiReportObject.workingDays));
            $get('labelRemainderToGo').innerHTML = getCurrencyString(remainderToGo);
            $get('labelRemainderToGoDailyAverage').innerHTML = getCurrencyString(remainderToGoDailyAverage);

            $get('labelTargetCollectionStatus').innerHTML = ((Math.round(targetCollectionAmount - (Math.round(kpiReportObject.amountCollectedToDate * 0.001)))) < 0) ? "Exceeding<br/>Target By" : "Remainder<br/>To Go";
            $get('labelTargetCollectedPercentage').innerHTML = ((targetCollectedPercentage)+"% ");

            //Set formatting
            if (Math.round(targetCollectionAmount - (Math.round(kpiReportObject.amountCollectedToDate * 0.001))) > 0) {
                $('#targetCollectionStatusHeader').css({ 'background-color': '#000000', 'color': '#ffffff' });
            } else {
                $('#targetCollectionStatusHeader').css({ 'background-color': 'green', 'color': '#ffffff' });
            }
            if (targetCollectedPercentage > daysPassedPerc) {
                $('#progressPercHeader').css({ 'background-color': 'green', 'color': '#ffffff' });
                $('#targetColledPercCell').css({ 'background-color': 'green', 'color': '#ffffff' });
            } else {
                $('#progressPercHeader').css({ 'background-color': 'yellow', 'color': '#000000' });
                $('#targetColledPercCell').css({ 'background-color': 'yellow', 'color': '#000000' });
            }
        }
        function onInvoiceTableChange() {
            if ($get('<%=textInvoiceThru1.ClientID %>').value == '') { return; }

            var populateWeek5Values = false;
            //Setup Invoicing Dates
            var lastDayOfMonth = new Date(kpiReportObject.StartOfReportMonthAsString());           
            lastDayOfMonth.setMonth(lastDayOfMonth.getMonth()+1);
            lastDayOfMonth.setDate(lastDayOfMonth.getDate()-1);
            
            var invoiceThru1Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var invoiceThru2Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);            
            var invoiceThru3Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var invoiceThru4Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var invoiceThru5Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);            

            var scheduledInvoice1Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice2Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice3Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice4Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice5Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);

            invoiceThru2Date.setDate(invoiceThru2Date.getDate() + 7);
            invoiceThru3Date.setDate(invoiceThru3Date.getDate() + 14);
            invoiceThru4Date.setDate(invoiceThru4Date.getDate() + 21);
            invoiceThru5Date.setDate(invoiceThru5Date.getDate() + 28);

            scheduledInvoice1Date.setDate(scheduledInvoice1Date.getDate() + 3);
            scheduledInvoice2Date.setDate(scheduledInvoice2Date.getDate() + 10);
            scheduledInvoice3Date.setDate(scheduledInvoice3Date.getDate() + 17);
            scheduledInvoice4Date.setDate(scheduledInvoice4Date.getDate() + 24);
            scheduledInvoice5Date.setDate(scheduledInvoice5Date.getDate() + 31);

            //Check to see if the user completed the 5th Week Invoice Date manually
            if ($get('<%=textInvoiceThru5.ClientID %>').value != '') {                
                populateWeek5Values = true;
                invoiceThru5Date = new Date($get('<%=textInvoiceThru5.ClientID %>').value);
                scheduledInvoice5Date = new Date(invoiceThru5Date.toDateString());
                scheduledInvoice5Date.setDate(scheduledInvoice5Date.getDate() + 3);
                //scheduledInvoice5Date.setDate(invoiceThru5Date.getDate() + 3);
                //scheduledInvoice5Date = 
            } else if (invoiceThru5Date <= lastDayOfMonth) {//need to do something special here, to provide for possible-5 week billing
                populateWeek5Values = true;
                $get('<%=textInvoiceThru5.ClientID %>').value = GetDateAsString(invoiceThru5Date);
                $get('labelSchedInvoice5').innerHTML = GetDateAsString(scheduledInvoice5Date);
            } else {
                $get('<%=textInvoiceThru5.ClientID %>').value = "";
                $get('labelSchedInvoice5').innerHTML = "";
            }

            $get('invoicingWeek1').style.visibility = (invoiceThru1Date > lastDayOfMonth) ? "hidden" : "visible";
            $get('invoicingWeek2').style.visibility = (invoiceThru2Date > lastDayOfMonth) ? "hidden" : "visible";
            $get('invoicingWeek3').style.visibility = (invoiceThru3Date > lastDayOfMonth) ? "hidden" : "visible";
            //$get('invoicingWeek4').style.visibility = (invoiceThru4Date > lastDayOfMonth) ? "hidden" : "visible";
            if (invoiceThru4Date > lastDayOfMonth) {
                $("#invoicingWeek4").addClass("hiddenRow");
                //$get('invoicingWeek4').style.display = "none";
            } else {
                $("#invoicingWeek4").removeClass("hiddenRow");
                //$get('invoicingWeek4').style.visibility = "visible";
            }
            //$get('invoicingWeek4').style.display = (invoiceThru4Date > lastDayOfMonth) ? "none" : "inline";
            //$get('invoicingWeek5').style.visibility = (invoiceThru5Date > lastDayOfMonth) ? "hidden" : "visible";            

            $get('labelInvoiceThru2').innerHTML = (invoiceThru2Date > lastDayOfMonth) ? "" : GetDateAsString(invoiceThru2Date);
            $get('labelInvoiceThru3').innerHTML = (invoiceThru3Date > lastDayOfMonth) ? "" : GetDateAsString(invoiceThru3Date);
            $get('labelInvoiceThru4').innerHTML = (invoiceThru4Date > lastDayOfMonth) ? "" : GetDateAsString(invoiceThru4Date);
            //$get('labelInvoiceThru5').innerHTML = (invoiceThru5Date > lastDayOfMonth) ? "" : GetDateAsString(invoiceThru5Date);

            $get('labelSchedInvoice1').innerHTML = GetDateAsString(scheduledInvoice1Date);
            $get('labelSchedInvoice2').innerHTML = GetDateAsString(scheduledInvoice2Date);
            $get('labelSchedInvoice3').innerHTML = GetDateAsString(scheduledInvoice3Date);
            $get('labelSchedInvoice4').innerHTML = GetDateAsString(scheduledInvoice4Date);
            if (populateWeek5Values) { $get('labelSchedInvoice5').innerHTML = GetDateAsString(scheduledInvoice5Date); }

            //Calculate Invoicing Totals
            var week1ProjectedTotal = parseFloat(GetFieldValue('<%=textProjectedBilling1.ClientID %>', 0), 10);
            var week2ProjectedTotal = parseFloat(GetFieldValue('<%=textProjectedBilling2.ClientID %>', 0), 10);
            var week3ProjectedTotal = parseFloat(GetFieldValue('<%=textProjectedBilling3.ClientID %>', 0), 10);
            var week4ProjectedTotal = parseFloat(GetFieldValue('<%=textProjectedBilling4.ClientID %>', 0), 10);
            var week5ProjectedTotal = parseFloat(GetFieldValue('<%=textProjectedBilling5.ClientID %>', 0), 10);
            kpiReportObject.projectedSalesTotal = week1ProjectedTotal + week2ProjectedTotal + week3ProjectedTotal + week4ProjectedTotal + week5ProjectedTotal;
            var week1PercentOfTotal = (kpiReportObject.projectedSalesTotal != 0) ? Math.round(100 * (week1ProjectedTotal / kpiReportObject.projectedSalesTotal)) : 0;
            var week2PercentOfTotal = (kpiReportObject.projectedSalesTotal != 0) ? Math.round(100 * (week2ProjectedTotal / kpiReportObject.projectedSalesTotal)) : 0;
            var week3PercentOfTotal = (kpiReportObject.projectedSalesTotal != 0) ? Math.round(100 * (week3ProjectedTotal / kpiReportObject.projectedSalesTotal)) : 0;
            var week4PercentOfTotal = (kpiReportObject.projectedSalesTotal != 0) ? Math.round(100 * (week4ProjectedTotal / kpiReportObject.projectedSalesTotal)) : 0;
            var week5PercentOfTotal = (kpiReportObject.projectedSalesTotal != 0) ? Math.round(100 * (week5ProjectedTotal / kpiReportObject.projectedSalesTotal)) : 0;

            $get('labelInvoiceProjectedTotal').innerHTML = getCurrencyString(kpiReportObject.projectedSalesTotal);
            $get('labelPercOfTot1').innerHTML = week1PercentOfTotal;
            $get('labelPercOfTot2').innerHTML = week2PercentOfTotal;
            $get('labelPercOfTot3').innerHTML = week3PercentOfTotal;
            $get('labelPercOfTot4').innerHTML = week4PercentOfTotal;
            if (populateWeek5Values) { $get('labelPercOfTot5').innerHTML = week5PercentOfTotal; }

            //Calculate Actual Totals
            var actualBilling1Date = GetFieldValue('<%=textActualInvoice1.ClientID %>', '');
            var actualBilling2Date = GetFieldValue('<%=textActualInvoice2.ClientID %>', '');
            var actualBilling3Date = GetFieldValue('<%=textActualInvoice3.ClientID %>', '');
            var actualBilling4Date = GetFieldValue('<%=textActualInvoice4.ClientID %>', '');
            var actualBilling5Date = GetFieldValue('<%=textActualInvoice5.ClientID %>', '');

            try {
                if (new Date(actualBilling1Date) > scheduledInvoice1Date) {
                    $('#' + '<%=textActualInvoice1.ClientID %>').css({ 'background-color': 'red', 'color': 'white' });
                    $('#actualInvoice1Cell').css({ 'background-color': 'red', 'color': 'white', 'font-weight': 'bold' });
                } else {
                    $('#' + '<%=textActualInvoice1.ClientID %>').css({ 'background-color': 'green', 'color': 'white' });
                    $('#actualInvoice1Cell').css({ 'background-color': 'green', 'color': 'white', 'font-weight': 'bold' });
                }
            } catch (e) { }

            try {
                if (new Date(actualBilling2Date) > scheduledInvoice2Date) {
                    $('#' + '<%=textActualInvoice2.ClientID %>').css({ 'background-color': 'red', 'color': 'white' });
                    $('#actualInvoice2Cell').css({ 'background-color': 'red', 'color': 'white', 'font-weight': 'bold' });
                } else {
                    $('#' + '<%=textActualInvoice2.ClientID %>').css({ 'background-color': 'green', 'color': 'white' });
                    $('#actualInvoice2Cell').css({ 'background-color': 'green', 'color': 'white', 'font-weight': 'bold' });
                }
            } catch (e) { }

            try {
                if (new Date(actualBilling3Date) > scheduledInvoice3Date) {
                    $('#' + '<%=textActualInvoice3.ClientID %>').css({ 'background-color': 'red', 'color': 'white' });
                    $('#actualInvoice3Cell').css({ 'background-color': 'red', 'color': 'white', 'font-weight': 'bold' });
                } else {
                    $('#' + '<%=textActualInvoice3.ClientID %>').css({ 'background-color': 'green', 'color': 'white' });
                    $('#actualInvoice3Cell').css({ 'background-color': 'green', 'color': 'white', 'font-weight': 'bold' });
                }
            } catch (e) { }

            try {
                if (new Date(actualBilling4Date) > scheduledInvoice4Date) {
                    $('#' + '<%=textActualInvoice4.ClientID %>').css({ 'background-color': 'red', 'color': 'white' });
                    $('#actualInvoice4Cell').css({ 'background-color': 'red', 'color': 'white', 'font-weight': 'bold' });
                } else {
                    $('#' + '<%=textActualInvoice4.ClientID %>').css({ 'background-color': 'green', 'color': 'white' });
                    $('#actualInvoice4Cell').css({ 'background-color': 'green', 'color': 'white', 'font-weight': 'bold' });
                }
            } catch (e) { }

            try {
                if (new Date(actualBilling5Date) > scheduledInvoice5Date) {
                    $('#' + '<%=textActualInvoice5.ClientID %>').css({ 'background-color': 'red', 'color': 'white' });
                    $('#actualInvoice5Cell').css({ 'background-color': 'red', 'color': 'white', 'font-weight': 'bold' });
                } else {
                    $('#' + '<%=textActualInvoice5.ClientID %>').css({ 'background-color': 'green', 'color': 'white' });
                    $('#actualInvoice5Cell').css({ 'background-color': 'green', 'color': 'white', 'font-weight': 'bold' });
                }
            } catch (e) { }

            var actualBilling1 = (actualBilling1Date == '') ? 0 : GetActualBillingAmountForWeek(new Date(kpiReportObject.StartOfReportMonthAsString()), new Date(actualBilling1Date), true);
            var actualBilling2 = (actualBilling2Date == '') ? 0 : GetActualBillingAmountForWeek(new Date(actualBilling1Date), new Date(actualBilling2Date), false);
            var actualBilling3 = (actualBilling3Date == '') ? 0 : GetActualBillingAmountForWeek(new Date(actualBilling2Date), new Date(actualBilling3Date), false);
            var actualBilling4 = (actualBilling4Date == '') ? 0 : GetActualBillingAmountForWeek(new Date(actualBilling3Date), new Date(actualBilling4Date), false);
            var actualBilling5 = (actualBilling5Date == '') ? 0 : GetActualBillingAmountForWeek(new Date((actualBilling4Date == '' ? actualBilling3Date : actualBilling4Date)), new Date(actualBilling5Date), false);
            
            var totalActualBilling = actualBilling1 + actualBilling2 + actualBilling3 + actualBilling4 + actualBilling5;

            $get('labelActualBillingTotal').innerHTML = getCurrencyString(totalActualBilling);
            $get('labelActBilling1').innerHTML = getCurrencyString(actualBilling1);
            $get('labelActBilling2').innerHTML = getCurrencyString(actualBilling2);
            $get('labelActBilling3').innerHTML = getCurrencyString(actualBilling3);
            $get('labelActBilling4').innerHTML = getCurrencyString(actualBilling4);
            if (populateWeek5Values) { $get('labelActBilling5').innerHTML = getCurrencyString(actualBilling5); }

            BuildTargetsTable();
        }
        function onBillingTableChange() {
            var actualBilling1 = parseFloat(GetFieldHTML('labelActBilling1',0),10);
            var actualBilling2 = parseFloat(GetFieldHTML('labelActBilling2',0),10);
            var actualBilling3 = parseFloat(GetFieldHTML('labelActBilling3',0),10);
            var actualBilling4 = parseFloat(GetFieldHTML('labelActBilling4',0),10);
            var actualBilling5 = parseFloat(GetFieldHTML('labelActBilling5',0),10);            
            
            var didNotPost1 = parseFloat(GetFieldValue('<%=textDidNotPost1.ClientID %>', 0),10);
            var didNotPost2 = parseFloat(GetFieldValue('<%=textDidNotPost2.ClientID %>', 0),10);
            var didNotPost3 = parseFloat(GetFieldValue('<%=textDidNotPost3.ClientID %>', 0),10);
            var didNotPost4 = parseFloat(GetFieldValue('<%=textDidNotPost4.ClientID %>', 0),10);
            var didNotPost5 = parseFloat(GetFieldValue('<%=textDidNotPost5.ClientID %>', 0),10);

            var catchUp1 = parseFloat(GetFieldValue('<%=textCatchUp1.ClientID %>', 0),10);
            var catchUp2 = parseFloat(GetFieldValue('<%=textCatchUp2.ClientID %>', 0),10);
            var catchUp3 = parseFloat(GetFieldValue('<%=textCatchUp3.ClientID %>', 0),10);
            var catchUp4 = parseFloat(GetFieldValue('<%=textCatchUp4.ClientID %>', 0),10);
            var catchUp5 = parseFloat(GetFieldValue('<%=textCatchUp5.ClientID %>', 0), 10);

            var cumulativeChange1 = didNotPost1;
            var cumulativeChange2 = (actualBilling2 > 0) ? (cumulativeChange1+didNotPost2-catchUp2) : 0;
            var cumulativeChange3 = (actualBilling3 > 0) ? (cumulativeChange2+didNotPost3-catchUp3) : 0;
            var cumulativeChange4 = (actualBilling4 > 0) ? (cumulativeChange3+didNotPost4-catchUp4) : 0;
            var cumulativeChange5 = (actualBilling5 > 0) ? (cumulativeChange4+didNotPost5-catchUp5) : 0;

            $get('labelCumltiveChg1').innerHTML = getCurrencyString(cumulativeChange1);
            $get('labelCumltiveChg2').innerHTML = getCurrencyString(cumulativeChange2);
            $get('labelCumltiveChg3').innerHTML = getCurrencyString(cumulativeChange3);
            $get('labelCumltiveChg4').innerHTML = getCurrencyString(cumulativeChange4);
            $get('labelCumltiveChg5').innerHTML = getCurrencyString(cumulativeChange5);
        }
        function onBelow3500Change() {
            var pmtGivenNum = parseInt(GetFieldValue('<%=textPmtGivenNum.ClientID %>', 0),10);
            var pmtGivenNumPerc = parseInt(GetFieldValue('<%=textPmtGivenNumPerc.ClientID %>', 0),10);
            var pmtGivenAmt = parseInt(GetFieldValue('<%=textPmtGivenAmt.ClientID %>', 0),10);
            var pmtGivenAmtPerc = parseInt(GetFieldValue('<%=textPmtGivenAmtPerc.ClientID %>', 0),10);

            var pmtMissingNum = parseInt(GetFieldValue('<%=textPmtMissingNum.ClientID %>', 0),10);
            var pmtMissingNumPerc = parseInt(GetFieldValue('<%=textPmtMissingNumPerc.ClientID %>', 0),10);
            var pmtMissingAmt = parseInt(GetFieldValue('<%=textPmtMissingAmt.ClientID %>', 0),10);
            var pmtMissingAmtPerc = parseInt(GetFieldValue('<%=textPmtMissingAmtPerc.ClientID %>', 0),10);

            var waivedNum = parseInt(GetFieldValue('<%=textWaivedNum.ClientID %>', 0),10);
            var waivedNumPerc = parseInt(GetFieldValue('<%=textWaivedNumPerc.ClientID %>', 0),10);
            var waivedAmt = parseInt(GetFieldValue('<%=textWaivedAmt.ClientID %>', 0),10);
            var waivedAmtPerc = parseInt(GetFieldValue('<%=textWaivedAmtPerc.ClientID %>', 0), 10);
            
            var totalNum = pmtGivenNum + pmtMissingNum + waivedNum;
            var totalNumPerc = pmtGivenNumPerc + pmtMissingNumPerc + waivedNumPerc;
            var totalAmt = pmtGivenAmt + pmtMissingAmt + waivedAmt;
            var totalAmtPerc = pmtGivenAmtPerc + pmtMissingAmtPerc + waivedAmtPerc;

            $get('labelTotalNum').innerHTML = totalNum;
            $get('labelTotalNumPerc').innerHTML = totalNumPerc;
            $get('labelTotalAmt').innerHTML = totalAmt;
            $get('labelTotalAmtPerc').innerHTML = totalAmtPerc;
        }
        function PopulateMarketKPIData() {
            var arReserve = Math.round((kpiReportObject.badDebtARBalanceData[0].badDebtAmount * 0.001) * 100) / 100;
            var sales = {};
            var totalNetSales = 0;
            //calculate the total net sales for all the markets
            for (var market in kpiReportObject.marketBreakdownData) {
                sales[market] = Math.round((kpiReportObject.marketBreakdownData[market].salesPast90 + kpiReportObject.marketBreakdownData[market].creditsPast90 + kpiReportObject.marketBreakdownData[market].debitsPast90) * 0.001);
                totalNetSales += sales[market];
            }
            $(".mktDsoRow").remove();
            var collectableTotal = 0;
            var balanceTotal = 0;
            var totalAR = 0;
            var fullBalanceTotal = 0;
            var balanceOver120Total = 0;
            for (var market in kpiReportObject.agedBalanceDataByMarket) {
                var netSales = sales[market];

                //var balance = Math.round(kpiReportObject.marketBreakdownData[market].balanceOver150 * 0.001);
                var balance = Math.round(kpiReportObject.marketBreakdownData[market].balanceOver120 * 0.001);
                balanceTotal += balance;
                
                var collectable = Math.round(kpiReportObject.marketBreakdownData[market].amountCollectable * 0.001);
                collectableTotal += collectable;

                var ar = Math.round(Math.round(kpiReportObject.agedBalanceDataByMarket[market].total * 0.001) - (arReserve * (netSales / totalNetSales)));
                totalAR += ar;

                var fullBalance = kpiReportObject.marketBreakdownData[market].fullBalance;
                fullBalanceTotal += fullBalance;

                var balanceOver120 = kpiReportObject.marketBreakdownData[market].balanceOver120;
                balanceOver120Total += balanceOver120;

                var percBalOver120 = Math.round((balanceOver120 / fullBalance) * 100);

                var row = "<tr class='mktDsoRow'><td class='columnTitleRight'>" + GetMarketDisplayName(market) + "</td>"
                    //+ "<td class='numericCell'>" + getCurrencyString(collectable) + "</td>"
                    + "<td class='numericCell'>" + getCurrencyString(balance) + "</td>"
                    + "<td class='numericCell'>" + percBalOver120 + "% </td>"
                    + "<td class='numericCell'>" + Math.round((ar * 91) / netSales) + "</td>"                    
                    //+ "<td>&nbsp;</td><td class='columnTitleRight'>" + GetMarketDisplayName(market) + "</td>"                    
                    //+ "<td class='numericCell'>" + getCurrencyString(netSales) + "</td>"
                    //+ "<td class='numericCell'>" + getCurrencyString(ar) + "</td>"
                    + "<td colspan='6'>&nbsp;</td>"
                    + "</tr>";
                $("#kpiDSOTableByMarket").append(row);
            }
            var percentBalanceOver120Total = Math.round((balanceOver120Total / fullBalanceTotal) * 100);
            var totalRow = "<tr class='mktDsoRow'><td class='columnTitleRight'>Total</td>"
                //+ "<td class='numericCell' style='border-top:1px solid #000000;'>" + getCurrencyString(collectableTotal) + "</td>"
                + "<td class='numericCell' style='border-top:1px solid #000000;'>" + getCurrencyString(balanceTotal) + "</td>"
                + "<td class='numericCell' style='border-top:1px solid #000000;'>" + percentBalanceOver120Total + "% </td>"
                + "<td class='numericCell' style='border-top:1px solid #000000;'>" + Math.round((totalAR * 91) / totalNetSales) + "</td>"
                //+ "<td>&nbsp;</td><td class='columnTitleRight'>Total</td>"
                //+ "<td class='numericCell' style='border-top:1px solid #000000;'>" + Math.round((totalAR * 91) / totalNetSales) + "</td>"
                //+ "<td class='numericCell' style='border-top:1px solid #000000;'>" + getCurrencyString(totalNetSales) + "</td>"
                //+ "<td class='numericCell' style='border-top:1px solid #000000;'>" + getCurrencyString(totalAR) + "</td>"
                + "<td colspan='6'>&nbsp;</td></tr>";

            $("#kpiDSOTableByMarket").append(totalRow);
        }
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
        function PopulateKPIData() {
            //Customer Breakdown Data
            //  Row 0: Direct, Row 1: Large, Row 2: Small
            //Aged Balance Data
            //  Row 0: Direct, Row 1: Large, Row 2: Small            
            
            //Large Account Data
            var largeAcounts = kpiReportObject.customerBreakdownData[1].customerTypeCount;
            var largeNetSales = Math.round((kpiReportObject.customerBreakdownData[1].salesPast90 + kpiReportObject.customerBreakdownData[1].creditsPast90 + kpiReportObject.customerBreakdownData[1].debitsPast90) * 0.001);
            var largeBalance = Math.round(kpiReportObject.customerBreakdownData[1].balanceOver150 * 0.001);
            var largeCollectable = Math.round(kpiReportObject.customerBreakdownData[1].amountCollectable * 0.001);
            //Small Account Data
            var smallAcounts = kpiReportObject.customerBreakdownData[2].customerTypeCount;
            var smallNetSales = Math.round((kpiReportObject.customerBreakdownData[2].salesPast90 + kpiReportObject.customerBreakdownData[2].creditsPast90 + kpiReportObject.customerBreakdownData[2].debitsPast90) * 0.001);
            var smallBalance = Math.round(kpiReportObject.customerBreakdownData[2].balanceOver150 * 0.001);
            var smallCollectable = Math.round(kpiReportObject.customerBreakdownData[2].amountCollectable * 0.001);
            //Direct Account Data
            var directAcounts = kpiReportObject.customerBreakdownData[0].customerTypeCount;
            var directNetSales = Math.round((kpiReportObject.customerBreakdownData[0].salesPast90 + kpiReportObject.customerBreakdownData[0].creditsPast90 + kpiReportObject.customerBreakdownData[0].debitsPast90) * 0.001);
            var directBalance = Math.round(kpiReportObject.customerBreakdownData[0].balanceOver150 * 0.001);            
            var directCollectable = Math.round(kpiReportObject.customerBreakdownData[0].amountCollectable * 0.001);
            //Totals Data
            var totalAccounts = largeAcounts + smallAcounts + directAcounts;
            var totalNetSales = largeNetSales + smallNetSales + directNetSales;
            var totalBalance = Math.round(largeBalance + smallBalance + directBalance);
            var totalCollectable = Math.round(largeCollectable + smallCollectable + directCollectable);
            
            //Calculate AR
            var arReserve = Math.round((kpiReportObject.badDebtARBalanceData[0].badDebtAmount * 0.001) * 100) / 100;
            var largeAR = Math.round(Math.round(kpiReportObject.agedBalanceData[1].total * 0.001) - (arReserve * (largeNetSales / totalNetSales)));
            var smallAR = Math.round(Math.round(kpiReportObject.agedBalanceData[2].total * 0.001) - (arReserve * (smallNetSales / totalNetSales)));
            var directAR = Math.round(Math.round(kpiReportObject.agedBalanceData[0].total * 0.001) - (arReserve * (directNetSales / totalNetSales)));
            var totalAR = Math.round(largeAR + smallAR + directAR);                        

            //Populate Large Values Table
            $get('labelLargeNumberAccounts').innerHTML = largeAcounts;
            $get('labelLargeCollectable').innerHTML = getCurrencyString(largeCollectable);
            $get('labelLargeBalance').innerHTML = getCurrencyString(largeBalance);
            $get('labelLargeDSO').innerHTML = Math.round((largeAR * 91) / largeNetSales);
            $get('labelLargeNetSales').innerHTML = getCurrencyString(largeNetSales);
            $get('labelLargeAR').innerHTML = getCurrencyString(largeAR);

            //Populate Small Values Table
            $get('labelSmallNumberAccounts').innerHTML = smallAcounts;
            $get('labelSmallCollectable').innerHTML = getCurrencyString(smallCollectable);
            $get('labelSmallBalance').innerHTML = getCurrencyString(smallBalance);
            $get('labelSmallDSO').innerHTML = Math.round((smallAR * 91) / smallNetSales);
            $get('labelSmallNetSales').innerHTML = getCurrencyString(smallNetSales);
            $get('labelSmallAR').innerHTML = getCurrencyString(smallAR);
                 
            //Populate Direct Values Table
            $get('labelDirectNumberAccounts').innerHTML = directAcounts;
            $get('labelDirectCollectable').innerHTML = getCurrencyString(directCollectable);
            $get('labelDirectBalance').innerHTML = getCurrencyString(directBalance);
            $get('labelDirectDSO').innerHTML = Math.round((directAR * 91) / directNetSales);
            $get('labelDirectNetSales').innerHTML = getCurrencyString(directNetSales);
            $get('labelDirectAR').innerHTML = getCurrencyString(directAR);

            //Populate Total Values Table
            $get('labelTotalAccounts').innerHTML = totalAccounts;
            $get('labelTotalCollectable').innerHTML = getCurrencyString(totalCollectable);
            $get('labelTotalBalance').innerHTML = getCurrencyString(totalBalance);
            $get('labelTotalDSO').innerHTML = Math.round((totalAR * 91) / totalNetSales);
            $get('labelTotalNetSales').innerHTML = getCurrencyString(totalNetSales);
            $get('labelTotalAR').innerHTML = getCurrencyString(totalAR);
            
            //Populate Collected Target Info.
            $get('labelCollectedToDate').innerHTML = getCurrencyString(Math.round(kpiReportObject.amountCollectedToDate * 0.001));
            $get('labelCollectedDailyAverage').innerHTML = getCurrencyString(Math.round((kpiReportObject.amountCollectedToDate * 0.001) / kpiReportObject.workingDaysElapsed));

            //Populate Days Passed Info.
            $get('labelDaysPassedPercentage').innerHTML = ((Math.round((kpiReportObject.workingDaysElapsed / kpiReportObject.workingDays) * 100)) + "% ");
            /*
            if (parseInt($("#dropDownCompany").val(), 10) == 1) {
                $("#kpiDSOTableByMarket").css({ "display": "block" });
                PopulateMarketKPIData();
            } else {
                $("#kpiDSOTableByMarket").css({ "display": "none" });
            }
            */
        }
        function Recalculate() {            
            kpiReportObject.reportDate = new Date($get('<%=textReportDate.ClientID %>').value);            
            kpiReportObject.reportMonth = $('#dropDownReportMonth').val();
            kpiReportObject.reportYear = $('#dropDownReportYear').val();
            kpiReportObject.kpiReportData.globalReportParams.reportDate = kpiReportObject.ReportDateAsString();
            $get('reportDateDisplay').innerHTML = kpiReportObject.ReportDateAsString();
            $get('labelARAsOfRptDate').innerHTML = kpiReportObject.ReportDateAsString();
            $("#labelARAsOfRptDate2").html(kpiReportObject.ReportDateAsString());
            LoadReportData();
            //PopulateDataTables(kpiReportObject.StartOfReportMonthAsString(), kpiReportObject.ReportDateAsString());
            //onInvoiceTableChange();
        }
        function Save() {
            alert('Save Clicked.');
        }
        function BuildTargetsTable() {
            var dsoByWeek = [70, 72, 75, 79, 71];            
            var month = monthList[kpiReportObject.reportDate.getMonth()];
            var monthMinusOne = ((kpiReportObject.reportDate.getMonth() - 1) < 0) ? monthList[11] : monthList[(kpiReportObject.reportDate.getMonth() - 1)];
            var begARBalance = Math.round((kpiReportObject.badDebtARBalanceData[0].arBalance * 0.000001) * 100) / 100;
            var arReserve = Math.round((kpiReportObject.badDebtARBalanceData[0].badDebtAmount * 0.000001) * 100) / 100;
            var sumMonthlySales = 0.0;            
            for (var x in kpiReportObject.monthValueHash) {
                sumMonthlySales += kpiReportObject.monthValueHash[x];                
            }
            //If a projected Sales Total was entered by the User...
            //  replace the Month's Sales amount with the Projected Sales total            
            if (kpiReportObject.projectedSalesTotal != 0) {
                sumMonthlySales -= kpiReportObject.monthValueHash[kpiReportObject.StartOfReportMonthAsString()];
                sumMonthlySales += (kpiReportObject.projectedSalesTotal * 1000);
            }
            sumMonthlySales = Math.round((sumMonthlySales * 0.000001) * 100) / 100;
            var arBalBeforeColl = Math.round((begARBalance - arReserve + (Math.round((kpiReportObject.projectedSalesTotal * 0.001) * 100) / 100)) * 100) / 100;
            var arBalToDSO1 = Math.round((dsoByWeek[0] / 91 * sumMonthlySales) * 100) / 100;
            var arBalToDSO2 = Math.round((dsoByWeek[1] / 91 * sumMonthlySales) * 100) / 100;
            var arBalToDSO3 = Math.round((dsoByWeek[2] / 91 * sumMonthlySales) * 100) / 100;
            var arBalToDSO4 = Math.round((dsoByWeek[3] / 91 * sumMonthlySales) * 100) / 100;
            var arBalToDSO5 = Math.round((dsoByWeek[4] / 91 * sumMonthlySales) * 100) / 100;
            var targetsDiv = $get('<%=targetsTable.ClientID %>');
            var targetsTable = "<table class='kpiDataTable'><thead><th>&nbsp;</th><th>" + monthMinusOne.substring(0, 3) + "</th><th colspan='5' align='center'>" + month.substring(0, 3) + "</th></thead><tbody>";
            targetsTable += "<tr><td colspan='2'>&nbsp;</td><td>" + dsoByWeek[0] + "</td><td>" + dsoByWeek[1] + "</td><td>" + dsoByWeek[2] + "</td><td>" + dsoByWeek[3] + "</td><td>" + dsoByWeek[4] + "</td></tr>";
            targetsTable += "<tr><td>Beg. AR Balance</td><td>" + begARBalance + "</td><td colspan='5'>&nbsp;</td></tr>";
            targetsTable += "<tr><td> - A/R Reserve</td><td>" + arReserve + "</td><td colspan='5'>&nbsp;</td></tr>";
            targetsTable += "<tr><td> + " + month.substring(0, 3) + ". Sales</td><td>" + (Math.round((kpiReportObject.projectedSalesTotal * 0.001) * 100) / 100) + "</td><td colspan='5'>&nbsp;</td></tr>";
            targetsTable += "<tr><td>A/R Bal before Coll</td><td>" + arBalBeforeColl + "</td><td colspan='5'>&nbsp;</td></tr>";
            targetsTable += "<tr><td>A/R Bal to get to DSO Target</td><td>&nbsp;</td><td>" + arBalToDSO1 + "</td>";
            targetsTable += "<td>" + arBalToDSO2 + "</td><td>" + arBalToDSO3 + "</td>";
            targetsTable += "<td>" + arBalToDSO4 + "</td><td>" + arBalToDSO5 + "</td></tr>";
            targetsTable += "<tr><td>Est Reduction in A\/R</td><td>&nbsp;</td><td>" + (Math.round((arBalBeforeColl - arBalToDSO1) * 100) / 100) + "</td>";
            targetsTable += "<td>" + (Math.round((arBalBeforeColl - arBalToDSO2) * 100) / 100) + "</td><td>" + (Math.round((arBalBeforeColl - arBalToDSO3) * 100) / 100) + "</td>";
            targetsTable += "<td>" + (Math.round((arBalBeforeColl - arBalToDSO4) * 100) / 100) + "</td><td>" + (Math.round((arBalBeforeColl - arBalToDSO5) * 100) / 100) + "</td></tr>";
            targetsTable += "<tr><td>Avg. per Day</td><td>&nbsp;</td><td>" + (Math.round(((arBalBeforeColl - arBalToDSO1) / kpiReportObject.workingDays) * 100) / 100) + "</td>";
            targetsTable += "<td>" + (Math.round(((arBalBeforeColl - arBalToDSO2) / kpiReportObject.workingDays) * 100) / 100) + "</td>";
            targetsTable += "<td>" + (Math.round(((arBalBeforeColl - arBalToDSO3) / kpiReportObject.workingDays) * 100) / 100) + "</td>";
            targetsTable += "<td>" + (Math.round(((arBalBeforeColl - arBalToDSO4) / kpiReportObject.workingDays) * 100) / 100) + "</td>";
            targetsTable += "<td>" + (Math.round(((arBalBeforeColl - arBalToDSO5) / kpiReportObject.workingDays) * 100) / 100) + "</td></tr>";
            targetsTable += "</tbody></table>";
            targetsDiv.innerHTML = targetsTable;
        }
        function GetActualBillingAmountForWeek(weekStartDate, weekEndDate, startOfMonth) {
            var billingAmount = 0;
            var documentDate;
            for (var x in kpiReportObject.salesData) {
                documentDate = new Date(kpiReportObject.salesData[x].documentDate);
                billingAmount += (startOfMonth) ? ((documentDate >= weekStartDate && documentDate <= weekEndDate) ? kpiReportObject.salesData[x].amount : 0) : ((documentDate > weekStartDate && documentDate <= weekEndDate) ? kpiReportObject.salesData[x].amount : 0);
            }
            return Math.round(billingAmount * 0.001);
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
            for (var i = yearStart-2; i <= yearEnd; i++) {                
                ddl2[ddl2.length] = new Option(i, i, (i == yearStart), (i == yearStart));
            }
        }
        function ErrorCallback(e) {
            endWait();
            alert(e._message);
        }
        function PopulateDataTables() {
            PageMethods.GetSalesData(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.ReportDateAsString(), SalesDataCallback, ErrorCallback);
        }
        function SalesDataCallback(salesDataResults) {
            kpiReportObject.salesData = eval(salesDataResults);
            var theDiv = $get('<%=salesDataTable.ClientID %>');            
            var myTable = "<table class='kpiDataTable'><thead><th>Date</th><th>Amount</th></thead><tbody>";
            for (var x in kpiReportObject.salesData) {
                myTable += "<tr><td>" + kpiReportObject.salesData[x].documentDate + "</td>";
                myTable += "<td>" + kpiReportObject.salesData[x].amount + "</td></tr>";                
            }
            myTable += "</tbody></table>";
            theDiv.innerHTML = myTable;
            PageMethods.CollectedToDate(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.ReportDateAsString(), kpiReportObject.StartOfReportMonthAsString(), CollectedToDateCallback, ErrorCallback);
        }
        function CollectedToDateCallback(collectedToDateResults) {
            kpiReportObject.amountCollectedToDate = collectedToDateResults;
            PageMethods.GetWorkDays(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.StartOfReportMonthAsString(), kpiReportObject.ReportDateAsString(), GetWorkDaysCallback, ErrorCallback);
        }
        function GetWorkDaysCallback(workDaysDataResults) {
            kpiReportObject.workDaysData = eval(workDaysDataResults);
            kpiReportObject.workingDays = kpiReportObject.workDaysData[0].workingDays;
            kpiReportObject.workingDaysElapsed = kpiReportObject.workDaysData[0].workingDaysElapsed;
            $get('workingDaysDisplay').innerHTML = kpiReportObject.workingDays;
            $get('workingDaysElapsedDisplay').innerHTML = (kpiReportObject.workingDaysElapsed > kpiReportObject.workingDays) ? kpiReportObject.workingDays : kpiReportObject.workingDaysElapsed;
            $get('workingDaysToGoDisplay').innerHTML = (kpiReportObject.workingDaysElapsed > kpiReportObject.workingDays) ? 0 : (kpiReportObject.workingDays - kpiReportObject.workingDaysElapsed);
            PageMethods.GetSalesByDocTypeAndMonth(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.StartOfReportMonthAsString(), SalesByMonthCallbackSuccess, ErrorCallback);
        }
        function SalesByMonthCallbackSuccess(salesByMonthDataResults) {
            kpiReportObject.salesByMonthData = eval(salesByMonthDataResults);
            var theDiv = $get('<%=salesByMonthTable.ClientID %>');                        
            kpiReportObject.monthValueHash = new Object();
            var myTable = "<table class='kpiDataTable'><thead><th>Document Type</th><th>Month</th><th>Amount</th></thead><tbody>";
            for (var x in kpiReportObject.salesByMonthData) {
                myTable += "<tr><td>" + kpiReportObject.salesByMonthData[x].documentType + "</td>";
                myTable += "<td>" + kpiReportObject.salesByMonthData[x].month + "</td>";
                myTable += "<td>" + kpiReportObject.salesByMonthData[x].amount + "</td></tr>";
                if (kpiReportObject.monthValueHash[kpiReportObject.salesByMonthData[x].month] === undefined) {
                    kpiReportObject.monthValueHash[kpiReportObject.salesByMonthData[x].month] = kpiReportObject.salesByMonthData[x].amount;
                } else {
                    kpiReportObject.monthValueHash[kpiReportObject.salesByMonthData[x].month] += kpiReportObject.salesByMonthData[x].amount;
                }                
            }
            var sumMonthlySales = 0;    
            myTable += "</tbody></table>";
            theDiv.innerHTML = myTable;
            //Build the trend table
            theDiv = $get('<%=trendTable.ClientID %>');
            myTable = "<table class='kpiDataTable'><thead><th>Month</th><th>Trend Amount</th></thead><tbody>";
            for (var x in kpiReportObject.monthValueHash) {
                myTable += "<tr><td>" + x + "</td>";
                myTable += "<td>" + Math.round((kpiReportObject.monthValueHash[x]*0.000001)*100)/100 + "</td></tr>";
                sumMonthlySales += kpiReportObject.monthValueHash[x];
            }
            sumMonthlySales = Math.round((sumMonthlySales * 0.000001) * 100) / 100;
            myTable += "<tr><td align='right'><b>Total</b></td><td>" + sumMonthlySales + "</td></tr></tbody></table>";
            theDiv.innerHTML = myTable;
            PageMethods.RetrieveBadDebtAndARBalance(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.ReportDateAsString(), GetBadDebtARBalanceCallback, ErrorCallback);
        }
        function GetBadDebtARBalanceCallback(badDebtARBalanceResults) {
            kpiReportObject.badDebtARBalanceData = eval(badDebtARBalanceResults);
            //PageMethods.GetAgedARBalances(kpiReportObject.ReportDateAsString(), AgedARBalanceCallbackSuccess, ErrorCallback);
            PageMethods.GetAgedARBalancesNew(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.ReportDateAsString(), AgedARBalanceNewCallbackSuccess, ErrorCallback);
        }
        function AgedARBalanceCallbackSuccess(agedBalanceDataResults) {
            kpiReportObject.agedBalanceData = eval(agedBalanceDataResults);
            var theDiv = $get('<%=agedARTable.ClientID %>');
            var myTable = "<table class='kpiDataTable'><thead><th>Customer Type</th><th>Sales</th><th>Debits</th><th>Credits</th><th>Payments</th><th>Total</th></thead><tbody>";
            for (var x in kpiReportObject.agedBalanceData) {
                myTable += "<tr><td>" + kpiReportObject.agedBalanceData[x].customerType + "</td><td>" + kpiReportObject.agedBalanceData[x].sales + "</td>";
                myTable += "<td>" + kpiReportObject.agedBalanceData[x].debits + "</td><td>" + kpiReportObject.agedBalanceData[x].credits + "</td>";
                myTable += "<td>" + kpiReportObject.agedBalanceData[x].payments + "</td><td>" + kpiReportObject.agedBalanceData[x].total + "</td></tr>";
            }
            myTable += "</tbody></table>";
            theDiv.innerHTML = myTable;
            PageMethods.GetCustomerTypeBreakdown(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.ReportDateAsString(), GetCustomerBreakdownCallback, ErrorCallback);
        }
        function AgedARBalanceNewCallbackSuccess(agedBalanceData) {
            var theDiv = $get('<%=agedARTable.ClientID %>');
            var myTable = "<table class='kpiDataTable'><thead><th>Customer Type</th><th>Sales</th><th>Debits</th><th>Credits</th><th>Payments</th><th>Total</th></thead><tbody>";
            for (var i = 0; i < 3; i++) {
                kpiReportObject.agedBalanceDataByCust.push(agedBalanceData[i]);
                myTable += "<tr><td>" + agedBalanceData[i].identifier + "</td><td>" + agedBalanceData[i].sales + "</td>";
                myTable += "<td>" + agedBalanceData[i].debits + "</td><td>" + agedBalanceData[i].credits + "</td>";
                myTable += "<td>" + agedBalanceData[i].payments + "</td><td>" + agedBalanceData[i].total + "</td></tr>";
            }
            kpiReportObject.agedBalanceData = kpiReportObject.agedBalanceDataByCust;
            theDiv.innerHTML = myTable;
            /*
            myTable = "<table class='kpiDataTable'><thead><th>Market</th><th>Sales</th><th>Debits</th><th>Credits</th><th>Payments</th><th>Total</th></thead><tbody>";
            for (var j = 3; j < agedBalanceData.length; j++) {
                kpiReportObject.agedBalanceDataByMarket[agedBalanceData[j].identifier] = agedBalanceData[j];
                //kpiReportObject.agedBalanceDataByMarket.push(agedBalanceData[j]);
                myTable += "<tr><td>" + agedBalanceData[j].identifier + "</td><td>" + agedBalanceData[j].sales + "</td>";
                myTable += "<td>" + agedBalanceData[j].debits + "</td><td>" + agedBalanceData[j].credits + "</td>";
                myTable += "<td>" + agedBalanceData[j].payments + "</td><td>" + agedBalanceData[j].total + "</td></tr>";
            }
            $("#marketARTable").html(myTable);
            */
            PageMethods.GetCustomerTypeBreakdown(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.ReportDateAsString(), GetCustomerBreakdownCallback, ErrorCallback);
        }
        function GetCustomerBreakdownCallback(breakdownData) {
            kpiReportObject.customerBreakdownData = eval(breakdownData);
            //customerBreakdownData = eval(breakdownData);            
            var theDiv = $get('<%=customerBreakdownTable.ClientID %>');
            var myTable = "<table class='kpiDataTable'><thead><th>Customer Type</th><th>Count of Customer</th><th>Amount Collectable</th><th>Balance Over 150</th><th>Sales in Past 90 Days</th><th>Credits in Past 90 Days</th><th>Debits in Past 90 Days</th></thead><tbody>";
            for (var x in kpiReportObject.customerBreakdownData) {
                myTable += "<tr><td>" + kpiReportObject.customerBreakdownData[x].customerType + "</td>";
                myTable += "<td>" + kpiReportObject.customerBreakdownData[x].customerTypeCount + "</td>";
                myTable += "<td>" + kpiReportObject.customerBreakdownData[x].amountCollectable + "</td>";
                myTable += "<td>" + kpiReportObject.customerBreakdownData[x].balanceOver150 + "</td>";
                myTable += "<td>" + kpiReportObject.customerBreakdownData[x].salesPast90 + "</td>";
                myTable += "<td>" + kpiReportObject.customerBreakdownData[x].creditsPast90 + "</td>";
                myTable += "<td>" + kpiReportObject.customerBreakdownData[x].debitsPast90 + "</td></tr>";
            }
            myTable += "</tbody></table>";
            theDiv.innerHTML = myTable;

            //PageMethods.GetMarketBreakdown(parseInt($("#dropDownCompany").val(), 10), kpiReportObject.ReportDateAsString(), GetMarketBreakdownCallback, ErrorCallback);
            PopulateKPIData();
            onInvoiceTableChange();
            onTargetCollectionChange();
            onTargetDSOChange();
            onBillingTableChange();
            onBelow3500Change();
            BuildTargetsTable();
            endWait();
        }
        function GetMarketBreakdownCallback(marketBreakdownData) {
            //kpiReportObject.marketBreakdownData = marketBreakdownData;
            var myTable = "<table class='kpiDataTable'><thead><th>Market</th><th>Amount Collectable</th><th>Balance Over 150</th><th>Sales in Past 90 Days</th><th>Credits in Past 90 Days</th><th>Debits in Past 90 Days</th></thead><tbody>";
            for (var i = 0; i < marketBreakdownData.length; i++) {
                kpiReportObject.marketBreakdownData[marketBreakdownData[i].marketName] = marketBreakdownData[i];
                myTable += "<tr><td>" + marketBreakdownData[i].marketName + "</td>";
                myTable += "<td>" + marketBreakdownData[i].amountCollectable + "</td>";
                myTable += "<td>" + marketBreakdownData[i].balanceOver150 + "</td>";
                myTable += "<td>" + marketBreakdownData[i].salesPast90 + "</td>";
                myTable += "<td>" + marketBreakdownData[i].creditsPast90 + "</td>";
                myTable += "<td>" + marketBreakdownData[i].debitsPast90 + "</td></tr>";
            }
            myTable += "</tbody></table>";
            
            $("#marketBreakdownTable").html(myTable);            
            //theDiv.innerHTML = myTable;
            PopulateKPIData();
            onInvoiceTableChange();
            onTargetCollectionChange();
            onTargetDSOChange();
            onBillingTableChange();
            onBelow3500Change();
            BuildTargetsTable();
            endWait();
        }
        function PrintPreview() {
            PackageReportData();
            var scheduledInvoice1Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice2Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice3Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice4Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            var scheduledInvoice5Date = new Date($get('<%=textInvoiceThru1.ClientID %>').value);
            scheduledInvoice1Date.setDate(scheduledInvoice1Date.getDate() + 3);
            scheduledInvoice2Date.setDate(scheduledInvoice2Date.getDate() + 10);
            scheduledInvoice3Date.setDate(scheduledInvoice3Date.getDate() + 17);
            scheduledInvoice4Date.setDate(scheduledInvoice4Date.getDate() + 24);
            scheduledInvoice5Date.setDate(scheduledInvoice5Date.getDate() + 31);
            
            var todaysDate = GetDateAsString(new Date());
            var w = window.open('', 'printKPIdetails', 'width=600,height=600,scrollbars=yes,resizable=yes,menubar=yes');
            w.document.write('<html><head>');
            w.document.write('  <title>KPI</title>');
            w.document.write('  <link rel="Stylesheet" href="/Styles/StyleSheet.css" />');
            //w.document.write('  <link rel="Stylesheet" href="/includes/jqueryUI/css/custom-theme/jquery-ui-1.8.20.custom.css" />');
            //w.document.write('  <link rel="Stylesheet" href="/includes/jqGrid/ui.jqgrid.css" />');
            //w.document.write('  <link rel="stylesheet" href="/includes/cluetip/jquery.cluetip.custom.css" type="text/css" />');
            w.document.write('  <style type="text/css">');
            w.document.write('     .dsoTable {width:750px;margin:5px auto;background:#ffffff;}');
            w.document.write('     .dsoTable tr {border-top:1px solid #cccccc;border-top:1px solid #cccccc;}');
            w.document.write('     .dsoTable .sectionRow td {border-bottom:1px solid #cccccc !important;}');
            w.document.write('     .dsoTable .sectionTitle {font-size:14px;font-weight:bold;white-space:nowrap;}');
            w.document.write('     .dsoTable .columnTitle {font-size:12px;font-weight:bold;white-space:nowrap;text-align:center}');
            w.document.write('     .dsoTable .columnTitleRight {font-size:12px;font-weight:bold;white-space:nowrap;text-align:right}');
            w.document.write('     .dsoTable .comment {font-size:10px;font-style:italic}');
            w.document.write('     .numericCell {text-align:right;padding-right:4px;}');
            w.document.write('     .numericCellRed {text-align:right;padding-right:4px;background-color:red;color:white;font-weight:bold;}');
            w.document.write('     .numericCellGreen {text-align:right;padding-right:4px;background-color:green;color:white;font-weight:bold;}');
            w.document.write('  </style>');
            w.document.write('</head>');
            w.document.write('<body><div style="margin:10px">');          
            w.document.write('  <div style="width:750px;margin:3px auto;display:block;"><img src="/images/header_logo.jpg" alt="Titan" title="Titan" style="display:block;margin:0 auto;" /></div>');
            w.document.write('  <div style="width:750px;margin:3px auto;display:block;text-align:center;"><h4>KPI - Accounts Receivable - as of ' + todaysDate + '</h4>(amounts in thousands)<br/><br/></div>');
            w.document.write('  <table class="dsoTable">' + $get('kpiHeaderTable').innerHTML + '</table>');
            w.document.write('  <table class="dsoTable">' + $get('kpiCollectionTable').innerHTML + '</table>');
            w.document.write('  <table class="dsoTable">' + $get('kpiDSOTable').innerHTML + '</table>');
            w.document.write('<table class="dsoTable" id="kpiBillingsTable">'
                + '<tr><td colspan="10" style="font-size:16px;font-weight:bold;text-align:center;color:#ffffff;background-color:#000000;">Updated Weekly on Mondays</td></tr>'
                + '<tr><td colspan="10">&nbsp;</td></tr><tr><td colspan="10" align="left" style="font-size:16px;font-weight:bold;padding-left:25px;">Weekly Billings</td></tr>'
                + '<tr><td class="columnTitle" width="10%">Invoice</td><td class="columnTitle" width="10%">Projected</td><td class="columnTitle" width="10%">&nbsp;</td>'
                    + '<td class="columnTitle" colspan="2" width="20%">Invoice Processing</td><td class="columnTitle" width="12%">Actual</td>'
                    + '<td class="columnTitle" width="12%">Did Not</td><td class="columnTitle" width="12%">Catch-up from</td><td class="columnTitle" width="12%">Cumulative</td>'
                    + '<td class="columnTitle" width="2%">&nbsp;</td></tr><tr><td class="columnTitle" width="10%">Thru</td><td class="columnTitle" width="10%">Billing</td>'
                    + '<td class="columnTitle" width="10%">% of Tot</td><td class="columnTitle" width="10%">Scheduled</td><td class="columnTitle" width="10%">Actual</td>'
                    + '<td class="columnTitle" width="12%">Billing</td><td class="columnTitle" width="12%">Post</td><td class="columnTitle" width="12%">Pr. Per.</td>'
                    + '<td class="columnTitle" width="12%">Change</td><td class="columnTitle" width="2%">&nbsp;</td></tr>'
                + '<tr id="invoicingWeek1">'
                    + '<td class="numericCell" width="10%">' + $get("<%=textInvoiceThru1.ClientID %>").value + '</td>'
                    + '<td class="numericCell" width="10%">' + getCurrencyString($get("<%=textProjectedBilling1.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="10%">' + $get("labelPercOfTot1").innerHTML + '% </td>'
                    + '<td class="numericCell" width="10%">' + $get("labelSchedInvoice1").innerHTML + '</td>'
                    + '<td class="' + ((kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek1=='') ? "numericCell" : ((new Date(kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek1) > scheduledInvoice1Date) ? "numericCellRed" : "numericCellGreen")) + '" width="10%">' + $get("<%=textActualInvoice1.ClientID %>").value + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelActBilling1").innerHTML + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textDidNotPost1.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textCatchUp1.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelCumltiveChg1").innerHTML + '</td><td width="2%">&nbsp;</td></tr>'
                + '<tr id="invoicingWeek2">'
                    + '<td class="numericCell" width="10%">' + $get("labelInvoiceThru2").innerHTML + '</td>'
                    + '<td class="numericCell" width="10%">' + getCurrencyString($get("<%=textProjectedBilling2.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="10%">' + $get("labelPercOfTot2").innerHTML + '% </td>'
                    + '<td class="numericCell" width="10%">' + $get("labelSchedInvoice2").innerHTML + '</td>'
                    + '<td class="' + ((kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek2 == '') ? "numericCell" : ((new Date(kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek2) > scheduledInvoice2Date) ? "numericCellRed" : "numericCellGreen")) + '" width="10%">' + $get("<%=textActualInvoice2.ClientID %>").value + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelActBilling2").innerHTML + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textDidNotPost2.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textCatchUp2.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelCumltiveChg2").innerHTML + '</td>'
                    + '<td width="2%">&nbsp;</td></tr>'
                + '<tr id="invoicingWeek3">'
                    + '<td class="numericCell" width="10%">' + $get("labelInvoiceThru3").innerHTML + '</td>'
                    + '<td class="numericCell" width="10%">' + getCurrencyString($get("<%=textProjectedBilling3.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="10%">' + $get("labelPercOfTot3").innerHTML + '% </td>'
                    + '<td class="numericCell" width="10%">' + $get("labelSchedInvoice3").innerHTML + '</td>'
                    + '<td class="' + ((kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek3 == '') ? "numericCell" : ((new Date(kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek3) > scheduledInvoice3Date) ? "numericCellRed" : "numericCellGreen")) + '" width="10%">' + $get("<%=textActualInvoice3.ClientID %>").value + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelActBilling3").innerHTML + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textDidNotPost3.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textCatchUp3.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelCumltiveChg3").innerHTML + '</td>'
                    + '<td width="2%">&nbsp;</td></tr>'
                + (($get("labelInvoiceThru4").innerHTML == '') ? '' : '<tr id="invoicingWeek4">'
                    + '<td class="numericCell" width="10%">' + $get("labelInvoiceThru4").innerHTML + '</td>'
                    + '<td class="numericCell" width="10%">' + getCurrencyString($get("<%=textProjectedBilling4.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="10%">' + $get("labelPercOfTot4").innerHTML + '% </td>'
                    + '<td class="numericCell" width="10%">' + $get("labelSchedInvoice4").innerHTML + '</td>'
                    + '<td class="' + ((kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek4=='') ? "numericCell" : ((new Date(kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek4) > scheduledInvoice4Date) ? "numericCellRed" : "numericCellGreen")) +  '" width="10%">' + $get("<%=textActualInvoice4.ClientID %>").value + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelActBilling4").innerHTML + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textDidNotPost4.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textCatchUp4.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelCumltiveChg4").innerHTML + '</td>'
                    + '<td width="2%">&nbsp;</td></tr>') +
                (($get("<%=textInvoiceThru5.ClientID %>").value == '') ? '' : ('<tr id="invoicingWeek5">'
                    + '<td class="numericCell" width="10%">' + $get("<%=textInvoiceThru5.ClientID %>").value + '</td>'
                    + '<td class="numericCell" width="10%">' + getCurrencyString($get("<%=textProjectedBilling5.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="10%">' + $get("labelPercOfTot5").innerHTML + '% </td>'
                    + '<td class="numericCell" width="10%">' + $get("labelSchedInvoice5").innerHTML + '</td>'
                    + '<td class="' + ((kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek5=='') ? "numericCell" : ((new Date(kpiReportObject.kpiReportData.actualInvoicingDates.actualInvoicingWeek5) > scheduledInvoice5Date) ? "numericCellRed" : "numericCellGreen")) +  '" width="10%">' + $get("<%=textActualInvoice5.ClientID %>").value + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelActBilling5").innerHTML + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textDidNotPost5.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + getCurrencyString($get("<%=textCatchUp5.ClientID %>").value) + '</td>'
                    + '<td class="numericCell" width="12%">' + $get("labelCumltiveChg5").innerHTML + '</td>'
                    + '<td width="2%">&nbsp;</td></tr>'))
                + '<tr id="invoicingTotal"><td width="10%" class="numericCell">&nbsp;</td>'
                    + '<td class="numericCell" width="10%" style="border-top:1px solid #000000">' + $get("labelInvoiceProjectedTotal").innerHTML + '</td>'
                    + '<td width="10%" class="numericCell">&nbsp;</td><td width="10%" class="numericCell">&nbsp;</td>'
                    + '<td width="10%" class="numericCell">&nbsp;</td>'
                    + '<td class="numericCell" width="12%" style="border-top:1px solid #000000">' + $get("labelActualBillingTotal").innerHTML + '</td>'
                    + '<td width="12%" class="numericCell">&nbsp;</td><td width="12%" class="numericCell">&nbsp;</td>'
                    + '<td width="12%" class="numericCell">&nbsp;</td>'
                    + '<td width="2%">&nbsp;</td></tr></table>');
            if (parseInt($("#dropDownCompany").val(), 10) == 1) {
                w.document.write('  <table class="dsoTable" id="kpiCredCheckTable">');
                w.document.write('      <tr><td colspan="10" align="left" style="font-size:14px;font-weight:bold;padding:10px 0 10px 25px;">Credit Applications</td></tr>');
                w.document.write('      <tr><td colspan="3">Received as a % of Total # Required:</td>');
                w.document.write('      <td class="numericCell">' + $get("<%=textCreditAppPercRec.ClientID %>").value + '% </td>');
                w.document.write('      <td>&nbsp;</td><td colspan="3">Checked as a % of Total # Required</td>');
                w.document.write('      <td class="numericCell">' + $get("<%=textCreditAppPercChk.ClientID %>").value + '% </td>');
                w.document.write('      <td>&nbsp;</td></tr>');
                w.document.write('      <tr><td colspan="10" align="left" style="font-size:14px;font-weight:bold;padding:10px 0 10px 25px;">Below $3,500</td></tr>'
                + '<tr><td width="20%">&nbsp;</td><td width="7%" class="numericCell">#</td><td width="7%" class="numericCell">% of Tot</td><td>&nbsp;</td>'
                + '<td width="7%" class="numericCell">$</td><td width="7%" class="numericCell">% of Tot</td><td>&nbsp;</td>'
                + '<td rowspan="5" colspan="3" style="padding:0 25px;"><div style="border:1px solid #333333;text-align:left;padding-left:10px;">'
                + '<br />1) New customers need to pay in advance or give credit card<br /><br />'
                + '2) Existing customers need to have 12 months sales >$10K or pay by credit card<br /><br /></div></td></tr>'
                + '<tr><td class="numericCell" width="20%">Payment Given</td><td width="7%" class="numericCell">' + $get("<%=textPmtGivenNum.ClientID %>").value + '</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textPmtGivenNumPerc.ClientID %>").value + '% </td><td>&nbsp;</td> ');
                w.document.write('      <td width="7%" class="numericCell">' + getCurrencyString($get("<%=textPmtGivenAmt.ClientID %>").value) + '</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textPmtGivenAmtPerc.ClientID %>").value + '% </td><td>&nbsp;</td>');
                w.document.write('      </tr><tr><td class="numericCell" width="20%">Payment Missing</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textPmtMissingNum.ClientID %>").value + '</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textPmtMissingNumPerc.ClientID %>").value + '% </td><td>&nbsp;</td>');
                w.document.write('      <td width="7%" class="numericCell">' + getCurrencyString($get("<%=textPmtMissingAmt.ClientID %>").value) + '</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textPmtMissingAmtPerc.ClientID %>").value + '% </td><td>&nbsp;</td>');
                w.document.write('      </tr><tr><td class="numericCell" width="20%">Waived</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textWaivedNum.ClientID %>").value + '</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textWaivedNumPerc.ClientID %>").value + '% </td><td>&nbsp;</td>');
                w.document.write('      <td width="7%" class="numericCell">' + getCurrencyString($get("<%=textWaivedAmt.ClientID %>").value) + '</td>');
                w.document.write('      <td width="7%" class="numericCell">' + $get("<%=textWaivedAmtPerc.ClientID %>").value + '% </td><td>&nbsp;</td>');
                w.document.write('      </tr><tr><td class="numericCell" width="20%">Total</td>');
                w.document.write('      <td width="7%" class="numericCell" style="border-top:1px solid #000000">' + $get("labelTotalNum").innerHTML + '</td>');
                w.document.write('      <td width="7%" class="numericCell" style="border-top:1px solid #000000">' + $get("labelTotalNumPerc").innerHTML + '% </td><td>&nbsp;</td>');
                w.document.write('      <td width="7%" class="numericCell" style="border-top:1px solid #000000">' + getCurrencyString($get("labelTotalAmt").innerHTML) + '</td>');
                w.document.write('      <td width="7%" class="numericCell" style="border-top:1px solid #000000">' + $get("labelTotalAmtPerc").innerHTML + '% </td><td>&nbsp;</td>');
                w.document.write('      </tr><tr><td colspan="10">&nbsp;</td></tr>');
                w.document.write('  </table>');
            }
            w.document.write('</div>');
            w.document.write('</body>');
            w.document.write('</html>');
            w.focus();
            /*
            if (parseInt($("#dropDownCompany").val(), 10) == 1) {
                var w2 = window.open('', 'printKPIdetails2', 'width=600,height=600,scrollbars=yes,resizable=yes,menubar=yes');
                w2.document.write('<html><head>');
                w2.document.write('  <title>KPI</title>');
                w2.document.write('  <link rel="Stylesheet" href="/Styles/StyleSheet.css?v=07" />');
                w2.document.write('  <link rel="Stylesheet" href="includes/jqueryUI/css/custom-theme/jquery-ui-1.8.20.custom.css" />');
                w2.document.write('  <link rel="Stylesheet" href="includes/jqGrid/ui.jqgrid.css" />');
                w2.document.write('  <link rel="stylesheet" href="includes/cluetip/jquery.cluetip.custom.css" type="text/css" />');
                w2.document.write('  <style type="text/css">');
                w2.document.write('     .dsoTable {width:750px;margin:5px auto;background:#ffffff;}');
                w2.document.write('     .dsoTable tr {border-top:1px solid #cccccc;border-top:1px solid #cccccc;}');
                w2.document.write('     .dsoTable .sectionRow td {border-bottom:1px solid #cccccc !important;}');
                w2.document.write('     .dsoTable .sectionTitle {font-size:14px;font-weight:bold;white-space:nowrap;}');
                w2.document.write('     .dsoTable .columnTitle {font-size:12px;font-weight:bold;white-space:nowrap;text-align:center}');
                w2.document.write('     .dsoTable .columnTitleRight {font-size:12px;font-weight:bold;white-space:nowrap;text-align:right}');
                w2.document.write('     .dsoTable .comment {font-size:10px;font-style:italic}');
                w2.document.write('     .numericCell {text-align:right;padding-right:4px;}');
                w2.document.write('     .numericCellRed {text-align:right;padding-right:4px;background-color:red;color:white;font-weight:bold;}');
                w2.document.write('     .numericCellGreen {text-align:right;padding-right:4px;background-color:green;color:white;font-weight:bold;}');
                w2.document.write('  </style>');
                w2.document.write('</head>');
                w2.document.write('<body><div style="margin:10px">');
                w2.document.write('  <div style="width:750px;margin:3px auto;display:block;"><img src="/images/header_logo.jpg" alt="Titan" title="Titan" style="display:block;margin:0 auto;" /></div>');
                w2.document.write('  <div style="width:750px;margin:3px auto;display:block;text-align:center;"><h4>KPI - Accounts Receivable - as of ' + todaysDate + '</h4>(amounts in thousands)<br/><br/></div>');
                w2.document.write('  <table class="dsoTable">' + $get('kpiDSOTableByMarket').innerHTML + '</table>');
                w2.document.write('</body>');
                w2.document.write('</html>');
                w2.document.close();
                w2.focus();
            }
            */
        }                 
    </script>   
    <div class="horizontalSearchCriteriaArea" style="width:98% !important;">
        <div>
            <div style="float:left;margin:5px 10px 0 5px;width:50px;"><label for="dropDownCompany">Company:</label></div>
            <div style="float:left;margin:5px 0 0 165px;">
                <select id="dropDownCompany">
                    <option selected="selected" value="1">Titan US</option>
                    <option value="2">Titan Canada</option>
                </select>
            </div>
            <div style="clear:both"></div>
        </div>
        <div style="clear:both"></div>
        <div style="float:left;width:35%;text-align:left;">                
            <ul class="horizontalSearchFilters" style="width:100% !important;">
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Report Month:</div>
                        <div class="horizontalSearchFilterRightColumn">
                            <div>
                                <div style="float:left">
                                    <select id="dropDownReportMonth"></select>
                                </div>
                                <div style="float:left;margin-left:10px;">
                                    <select id="dropDownReportYear"></select>
                                </div>
                                <div style="clear:both"></div>
                            </div>                                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>                
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Target Cash Collection for Report Month:</div>
                        <div class="horizontalSearchFilterRightColumn"><asp:TextBox ID="textTargetCash" runat="server" BackColor="LightGreen" onchange="onTargetCollectionChange(this);" /></div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>            
        </div>
        <div style="float:left;width:35%;text-align:left;">
            <ul class="horizontalSearchFilters" style="width:100% !important;">
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Report Date:</div>
                        <div class="horizontalSearchFilterRightColumn">
                            <asp:TextBox ID="textReportDate" runat="server" BackColor="LightGreen" Width="65px" />
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>                
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Target DSO for Report Month:</div>
                        <div class="horizontalSearchFilterRightColumn"><asp:TextBox ID="textTargetDso" runat="server" BackColor="LightGreen" onchange="onTargetDSOChange(this);" /></div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="float:left;width:25%;text-align:center;padding-top:10px;">            
            <div>
                <div style="float:left">
                    <img src="/Images/but_recalc.png" alt="Recalculate" title="Recalculate" style="cursor:pointer;" onclick="Recalculate();" />
                    <br />
                    <img src="/Images/but_save.png" alt="Save" title="Save" style="cursor:pointer;margin:10px 0;" onclick="SaveReportData();" />
                </div>
                <div style="float:left;margin-left:10px;">                    
                    <img src="/Images/but_print.png" alt="Print" title="Print" style="cursor:pointer;" onclick="PrintPreview();" />            
                </div>
                <div style="clear:both"></div>
            </div>
        </div>
        <div style="clear:both"></div>                
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportListPanel" Runat="Server">
    <style type="text/css">
        .dsoTable {width:750px;margin:5px auto;background:#ffffff;}
        .dsoTable tr {border-top:1px solid #cccccc;border-top:1px solid #cccccc;}
        .dsoTable .sectionRow td {border-bottom:1px solid #cccccc !important;}
        .dsoTable .sectionTitle {font-size:14px;font-weight:bold;white-space:nowrap;}
        .dsoTable .columnTitle {font-size:12px;font-weight:bold;white-space:nowrap;text-align:center;}
        .dsoTable .columnTitleRight {font-size:12px;font-weight:bold;white-space:nowrap;text-align:right}
        .dsoTable .comment {font-size:10px;font-style:italic}
        .kpiDataTable {border:1px solid #cccccc;text-align:right;}
        .kpiDataTable th {border-bottom:1px solid #cccccc;padding:5px;}
        .kpiDataTable tr {border:1px solid #cccccc;padding:5px 0;}
        .kpiDataTable td {padding:0 5px;}
        .numericCell {text-align:right;padding-right:4px}
        button { outline: 0; margin:0; padding: .4em 1em .5em; text-decoration:none !important; cursor:pointer; position: relative; text-align: center; }
    </style>
    <div id="kpiTabs" style="margin:25px 25px;">
        <ul>
            <li><a href="#tabs-kpi">KPI</a></li>
            <li><a href="#tabs-targets">Targets</a></li>
            <li><a href="#tabs-sourceData">Source Data</a></li>
            <!--li><a href="#tabs-sourceDataByMarket">Source Data by Market</a></li-->
        </ul>
        <div style="margin:0 auto;" id="tabs-kpi">            
            <table class="dsoTable" id="kpiHeaderTable">
                <tr><td colspan="7" width="70%">&nbsp;</td>
                    <td width="10%" style="border:1px solid #000000;text-align:center;"><b>in Month</b></td>
                    <td width="10%" style="border:1px solid #000000;text-align:center;"><b>Elapsed</b></td>
                    <td width="10%" style="border:1px solid #000000;text-align:center;"><b>To Go</b></td></tr>
                <tr><td width="15%" style="background-color:#FFFF00"><b>Data as of:</b></td><td style="background-color:#FFFF00"><span id="reportDateDisplay"></span></td><td colspan="4" width="45%">&nbsp;</td>
                    <td width="10%">Days:</td><td width="10%" class="numericCell"><span id="workingDaysDisplay"></span></td>
                    <td width="10%" class="numericCell"><span id="workingDaysElapsedDisplay"></span></td>
                    <td width="10%" class="numericCell"><span id="workingDaysToGoDisplay"></span></td></tr>
            </table>                    
            <table class="dsoTable" id="kpiCollectionTable">
                <tr><td colspan="8" style="font-size:16px;font-weight:bold;text-align:center;color:#ffffff;background-color:#000000;">Updated Daily</td></tr> 
                <tr><td colspan="8">&nbsp;</td></tr>
                <tr><td colspan="8" align="left" style="font-size:16px;font-weight:bold;padding-left:25px;">Cash Collection &amp; Overall DSO for the Month</td></tr>
                <tr>
                    <td colspan="2">&nbsp;</td>            
                    <td class="columnTitle">Target<br />for the Month</td>            
                    <td class="columnTitle">Collected<br />to Date</td>
                    <td class="columnTitle" id="targetCollectionStatusHeader"><span id="labelTargetCollectionStatus">Remainder<br />To Go</span></td>
                    <td>&nbsp;</td>
                    <td colspan="2" class="columnTitle" id="progressPercHeader">Progress<br />(as a %)</td>                                
                </tr>
                <tr>
                    <td colspan="2" align="right" class="columnTitle">This Month</td>            
                    <td class="numericCell"><span id="labelTargetCollection"></span></td>
                    <td class="numericCell"><span id="labelCollectedToDate"></span></td>
                    <td class="numericCell"><span id="labelRemainderToGo"></span></td>
                    <td>&nbsp;</td>
                    <td align="right" class="columnTitle">Days Passed:</td>
                    <td class="numericCell"><span id="labelDaysPassedPercentage"></span></td>                    
                </tr>
                <tr>
                    <td colspan="2" align="right" class="columnTitle">Daily Average</td>            
                    <td class="numericCell"><span id="labelTargetDailyAverage"></span></td>
                    <td class="numericCell"><span id="labelCollectedDailyAverage"></span></td>                        
                    <td class="numericCell"><span id="labelRemainderToGoDailyAverage"></span></td>
                    <td>&nbsp;</td>
                    <td align="right" class="columnTitle">Target Collected:</td>
                    <td id="targetColledPercCell" class="numericCell"><span id="labelTargetCollectedPercentage"></span></td>                    
                </tr>
                <tr><td colspan="8">&nbsp;</td></tr>
                <tr>
                    <td colspan="2">&nbsp;</td>            
                    <td class="columnTitle">Target<br />for the Month</td>                    
                    <td colspan="5">&nbsp;</td>                                        
                </tr>        
                <tr>
                    <td colspan="2" align="right" class="columnTitle">DSO*</td>            
                    <td class="numericCell"><span id="labelTargetDSO"></span></td>                    
                    <td colspan="5">&nbsp;</td>            
                </tr>                
            </table>
            <table class="dsoTable" id="kpiDSOTable">                
                <tr style="padding-top:10px">
                    <td>&nbsp;</td>            
                    <td colspan="3" class="sectionTitle">A/R by Client Type</td>            
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td colspan="3" class="sectionTitle">DSO by Client Type</td>            
                    <td>&nbsp;</td>            
                </tr>
                <tr>
                    <td>&nbsp;</td>            
                    <td class="columnTitle"># of<br />Accounts</td>
                    <td class="columnTitle"><br />Collectable</td>
                    <td class="columnTitle">Balance<br />&gt; 150 days</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>            
                    <td class="columnTitle">Rolling<br />DSO</td>
                    <td class="columnTitle">Net Sales<br />(last 91 days)</td>
                    <td class="columnTitle">A/R<br />as of <span id="labelARAsOfRptDate"></span></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>                    
                    <td class="columnTitleRight">Large (60+ & <150 Days)</td>            
                    <td class="numericCell"><span id="labelLargeNumberAccounts"></span></td>
                    <td class="numericCell"><span id="labelLargeCollectable"></span></td>
                    <td class="numericCell"><span id="labelLargeBalance"></span></td>            
                    <td>&nbsp;</td>
                    <td class="columnTitleRight">Large Agency</td>
                    <td class="numericCell"><span id="labelLargeDSO"></span></td>    
                    <td class="numericCell"><span id="labelLargeNetSales"></span></td>
                    <td class="numericCell"><span id="labelLargeAR"></span></td>            
                    <td>&nbsp;</td>
                </tr>
                <tr>            
                    <td class="columnTitleRight">Small (60+ & <150 Days)</td>
                    <td class="numericCell"><span id="labelSmallNumberAccounts"></span></td>
                    <td class="numericCell"><span id="labelSmallCollectable"></span></td>
                    <td class="numericCell"><span id="labelSmallBalance"></span></td>            
                    <td>&nbsp;</td>
                    <td class="columnTitleRight">Small Agency</td>
                    <td class="numericCell"><span id="labelSmallDSO"></span></td>
                    <td class="numericCell"><span id="labelSmallNetSales"></span></td>
                    <td class="numericCell"><span id="labelSmallAR"></span></td>      
                    <td>&nbsp;</td>
                </tr>
                <tr>            
                    <td class="columnTitleRight">Direct (30+ & <150 Days)</td>
                    <td class="numericCell"><span id="labelDirectNumberAccounts"></span></td>
                    <td class="numericCell"><span id="labelDirectCollectable"></span></td>
                    <td class="numericCell"><span id="labelDirectBalance"></span></td>            
                    <td>&nbsp;</td>
                    <td class="columnTitleRight">Direct</td>
                    <td class="numericCell"><span id="labelDirectDSO"></span></td>
                    <td class="numericCell"><span id="labelDirectNetSales"></span></td>
                    <td class="numericCell"><span id="labelDirectAR"></span></td>      
                    <td>&nbsp;</td>
                </tr>
                <tr>            
                    <td class="columnTitleRight">Total</td>
                    <td class="numericCell" style="border-top:1px solid #000000"><span id="labelTotalAccounts"></span></td>
                    <td class="numericCell" style="border-top:1px solid #000000"><span id="labelTotalCollectable"></span></td>
                    <td class="numericCell" style="border-top:1px solid #000000"><span id="labelTotalBalance"></span></td>            
                    <td>&nbsp;</td>
                    <td class="columnTitleRight">Total</td>
                    <td class="numericCell" style="border-top:1px solid #000000"><span id="labelTotalDSO"></span></td>
                    <td class="numericCell" style="border-top:1px solid #000000"><span id="labelTotalNetSales"></span></td>
                    <td class="numericCell" style="border-top:1px solid #000000"><span id="labelTotalAR"></span></td>      
                    <td>&nbsp;</td>
                </tr>
                <tr><td colspan="10">&nbsp;</td></tr>
            </table>
            <table class="dsoTable" id="kpiDSOTableByMarket" style="display:none">
                <tr><td colspan="10">&nbsp;</td></tr>
                <tr style="padding-top:10px">
                    <td colspan="10">&nbsp;</td>            
                    <!--td colspan="3" class="sectionTitle">DSO by Market</td>            
                    <td colspan="6" style="width:50px;">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td colspan="3" class="sectionTitle">DSO by Market</td>
                    <td>&nbsp;</td-->
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <!--td class="columnTitle"><br />Collectable</td-->
                    <td class="columnTitle" style="text-align:right">A/R Balance<br />&gt; 120 days</td>
                    <td class="columnTitle" style="text-align:right">% of Balance<br />&gt; 120 days</td>
                    <td class="columnTitle" style="text-align:right">DSO</td>                    
                    <td colspan="6" style="width:50px;">&nbsp;</td>                                        
                    <!--td class="columnTitle">Net Sales<br />(last 91 days)</td-->
                    <!--td class="columnTitle">A/R<br />as of <span id="labelARAsOfRptDate2"></span></td-->
                    <!--td colspan="2">&nbsp;</td-->
                </tr>                
            </table>            
            <table class="dsoTable" id="kpiBillingsTable">
                <tr><td colspan="10" style="font-size:16px;font-weight:bold;text-align:center;color:#ffffff;background-color:#000000;">Updated Weekly on Mondays</td></tr> 
                <tr><td colspan="10">&nbsp;</td></tr>
                <tr><td colspan="10" align="left" style="font-size:16px;font-weight:bold;padding-left:25px;">Weekly Billings</td></tr>        
                <tr>
                    <td class="columnTitle" width="10%">Invoice</td>
                    <td class="columnTitle" width="10%">Projected</td>
                    <td class="columnTitle" width="10%">&nbsp;</td>
                    <td class="columnTitle" colspan="2" width="20%">Invoice Processing</td>            
                    <td class="columnTitle" width="12%">Actual</td>
                    <td class="columnTitle" width="12%">Did Not</td>
                    <td class="columnTitle" width="12%">Catch-up from</td>
                    <td class="columnTitle" width="12%">Cumulative</td>
                    <td class="columnTitle" width="2%">&nbsp;</td>            
                </tr>
                <tr>
                    <td class="columnTitle" width="10%">Thru</td>
                    <td class="columnTitle" width="10%">Billing</td>
                    <td class="columnTitle" width="10%">% of Tot</td>
                    <td class="columnTitle" width="10%">Scheduled</td>
                    <td class="columnTitle" width="10%">Actual</td>
                    <td class="columnTitle" width="12%">Billing</td>
                    <td class="columnTitle" width="12%">Post</td>
                    <td class="columnTitle" width="12%">Pr. Per.</td>
                    <td class="columnTitle" width="12%">Change</td>
                    <td class="columnTitle" width="2%">&nbsp;</td>            
                </tr>
                <tr id="invoicingWeek1">
                    <td class="numericCell" width="10%"><asp:TextBox ID="textInvoiceThru1" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="65px" style="text-align:right;font-size:10px;font-family:Arial;" /></td>
                    <td class="numericCell" width="10%"><asp:TextBox ID="textProjectedBilling1" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="55px" style="text-align:right" /></td>
                    <td class="numericCell" width="10%"><span id="labelPercOfTot1"></span></td>
                    <td class="numericCell" width="10%"><span id="labelSchedInvoice1"></span></td>
                    <td class="numericCell" width="10%" id="actualInvoice1Cell"><asp:TextBox ID="textActualInvoice1" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="60px" style="text-align:right;font-size:10px;font-family:Arial;" /></td>
                    <td class="numericCell" width="12%"><span id="labelActBilling1"></span></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textDidNotPost1" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textCatchUp1" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><span id="labelCumltiveChg1"></span></td>            
                    <td width="2%">&nbsp;</td>
                </tr>
                <tr id="invoicingWeek2">
                    <td class="numericCell" width="10%"><span id="labelInvoiceThru2"></span></td>
                    <td class="numericCell" width="10%"><asp:TextBox ID="textProjectedBilling2" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="55px" style="text-align:right" /></td>
                    <td class="numericCell" width="10%"><span id="labelPercOfTot2"></span></td>
                    <td class="numericCell" width="10%"><span id="labelSchedInvoice2"></span></td>
                    <td class="numericCell" width="10%" id="actualInvoice2Cell"><asp:TextBox ID="textActualInvoice2" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="60px" style="text-align:right;font-size:10px;font-family:Arial;" /></td>
                    <td class="numericCell" width="12%"><span id="labelActBilling2"></span></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textDidNotPost2" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textCatchUp2" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><span id="labelCumltiveChg2"></span></td>            
                    <td width="2%">&nbsp;</td>
                </tr>
                <tr id="invoicingWeek3">
                    <td class="numericCell" width="10%"><span id="labelInvoiceThru3"></span></td>
                    <td class="numericCell" width="10%"><asp:TextBox ID="textProjectedBilling3" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="55px" style="text-align:right" /></td>
                    <td class="numericCell" width="10%"><span id="labelPercOfTot3"></span></td>
                    <td class="numericCell" width="10%"><span id="labelSchedInvoice3"></span></td>
                    <td class="numericCell" width="10%" id="actualInvoice3Cell"><asp:TextBox ID="textActualInvoice3" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="60px" style="text-align:right;font-size:10px;font-family:Arial;" /></td>
                    <td class="numericCell" width="12%"><span id="labelActBilling3"></span></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textDidNotPost3" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textCatchUp3" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><span id="labelCumltiveChg3"></span></td>               
                    <td width="2%">&nbsp;</td> 
                </tr>
                <tr id="invoicingWeek4">
                    <td class="numericCell" width="10%"><span id="labelInvoiceThru4"></span></td>
                    <td class="numericCell" width="10%"><asp:TextBox ID="textProjectedBilling4" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="55px" style="text-align:right" /></td>
                    <td class="numericCell" width="10%"><span id="labelPercOfTot4"></span></td>
                    <td class="numericCell" width="10%"><span id="labelSchedInvoice4"></span></td>
                    <td class="numericCell" width="10%" id="actualInvoice4Cell"><asp:TextBox ID="textActualInvoice4" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="60px" style="text-align:right;font-size:10px;font-family:Arial;" /></td>
                    <td class="numericCell" width="12%"><span id="labelActBilling4"></span></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textDidNotPost4" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textCatchUp4" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><span id="labelCumltiveChg4"></span></td>               
                    <td width="2%">&nbsp;</td>   
                </tr>
                <tr id="invoicingWeek5">
                    <td class="numericCell" width="10%"><asp:TextBox ID="textInvoiceThru5" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="65px" style="text-align:right;font-size:10px;font-family:Arial" /></td>
                    <td class="numericCell" width="10%"><asp:TextBox ID="textProjectedBilling5" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="55px" style="text-align:right" /></td>
                    <td class="numericCell" width="10%"><span id="labelPercOfTot5"></span></td>
                    <td class="numericCell" width="10%" id="actualInvoice5Cell"><span id="labelSchedInvoice5"></span></td>
                    <td class="numericCell" width="10%"><asp:TextBox ID="textActualInvoice5" runat="server" BackColor="LightGreen" onchange="onInvoiceTableChange();" width="60px" style="text-align:right;font-size:10px;font-family:Arial;" /></td>
                    <td class="numericCell" width="12%"><span id="labelActBilling5"></span></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textDidNotPost5" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><asp:TextBox ID="textCatchUp5" runat="server" width="65px" style="text-align:right" onchange="onBillingTableChange();" /></td>
                    <td class="numericCell" width="12%"><span id="labelCumltiveChg5"></span></td>               
                    <td width="2%">&nbsp;</td>   
                </tr>
                <tr id="invoicingTotal">
                    <td width="10%" class="numericCell">&nbsp;</td>
                    <td class="numericCell" width="10%" style="border-top:1px solid #000000"><span id="labelInvoiceProjectedTotal"></span></td>
                    <td width="10%" class="numericCell">&nbsp;</td>
                    <td width="10%" class="numericCell">&nbsp;</td>
                    <td width="10%" class="numericCell">&nbsp;</td>
                    <td class="numericCell" width="12%" style="border-top:1px solid #000000"><span id="labelActualBillingTotal"></span></td>
                    <td width="12%" class="numericCell">&nbsp;</td>
                    <td width="12%" class="numericCell">&nbsp;</td>
                    <td width="12%" class="numericCell">&nbsp;</td>
                    <td width="2%">&nbsp;</td>            
                </tr> 
            </table>            
            <table class="dsoTable" id="kpiCredCheckTable">                               
                <tr><td colspan="10" align="left" style="font-size:14px;font-weight:bold;padding:10px 0 10px 25px;">Credit Applications</td></tr>                
                <tr>                
                    <td colspan="3">Received as a % of Total # Required:</td>            
                    <td><asp:TextBox ID="textCreditAppPercRec" runat="server" BackColor="LightGreen" width="35px" style="text-align:right" /></td>
                    <td>&nbsp;</td>
                    <td colspan="3">Checked as a % of Total # Required</td>            
                    <td><asp:TextBox ID="textCreditAppPercChk" runat="server" BackColor="LightGreen" width="35px" style="text-align:right" /></td>                
                    <td>&nbsp;</td>
                </tr>                       
                <tr><td colspan="10" align="left" style="font-size:14px;font-weight:bold;padding:10px 0 10px 25px;">Below $3,500</td></tr>                        
                <tr>
                    <td width="20%">&nbsp;</td>                    
                    <td width="7%" class="numericCell">#</td>
                    <td width="7%" class="numericCell">% of Tot</td>
                    <td>&nbsp;</td>
                    <td width="7%" class="numericCell">$</td>
                    <td width="7%" class="numericCell">% of Tot</td>                 
                    <td>&nbsp;</td>
                    <td rowspan="5" colspan="3" style="padding:0 25px;">
                        <div style="border:1px solid #333333;text-align:left;padding-left:10px;">
                            <br />
                            1) New customers need to pay in advance or give credit card
                            <br /><br />
                            2) Existing customers need to have 12 months sales >$10K or pay by credit card
                            <br /><br />
                        </div>
                    </td>                            
                </tr>
                <tr>                
                    <td class="numericCell" width="20%">Payment Given</td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtGivenNum" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtGivenNumPerc" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td>&nbsp;</td>                    
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtGivenAmt" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtGivenAmtPerc" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>                            
                    <td>&nbsp;</td>
                </tr>
                <tr>                
                    <td class="numericCell" width="20%">Payment Missing</td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtMissingNum" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtMissingNumPerc" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td>&nbsp;</td>                    
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtMissingAmt" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textPmtMissingAmtPerc" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>                            
                    <td>&nbsp;</td>
                </tr>
                <tr>                
                    <td class="numericCell" width="20%">Waived</td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textWaivedNum" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textWaivedNumPerc" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td>&nbsp;</td>                    
                    <td width="7%" class="numericCell"><asp:TextBox ID="textWaivedAmt" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>
                    <td width="7%" class="numericCell"><asp:TextBox ID="textWaivedAmtPerc" runat="server" BackColor="LightGreen" onchange="onBelow3500Change();" width="35px" style="text-align:right" /></td>                            
                    <td>&nbsp;</td>
                </tr>
                <tr>                
                    <td class="numericCell" width="20%">Total</td>
                    <td class="numericCell" width="7%"><span id="labelTotalNum"></span></td>
                    <td class="numericCell" width="7%"><span id="labelTotalNumPerc"></span></td>
                    <td>&nbsp;</td>                    
                    <td class="numericCell" width="7%"><span id="labelTotalAmt"></span></td>
                    <td class="numericCell" width="7%"><span id="labelTotalAmtPerc"></span></td>
                    <td>&nbsp;</td>                
                </tr>
                <tr><td colspan="10">&nbsp;</td></tr>
            </table>            
        </div>
        <div id="tabs-targets">
            <div id="trendTable" runat="server">
            </div>
            <br /><br />
            <div id="targetsTable" runat="server">
            </div>            
        </div>
        <div id="tabs-sourceData">
            <div id="agedARTable" runat="server" style="text-align:left">                
            </div>
            <br /><br />
            <div id="customerBreakdownTable" runat="server" style="text-align:left;">
            </div>
            <br /><br />
            <div id="salesByMonthTable" runat="server" style="text-align:left">                
            </div>            
            <br /><br />
            <div id="salesDataTable" runat="server" style="text-align:left">                
            </div>  
        </div>
        <div id="tabs-sourceDataByMarket">
            <div id="marketBreakdownTable" style="text-align:left;"></div>
            <br /><br />
            <div id="marketARTable" style="text-align:left;"></div>
        </div>
    </div>        
</asp:Content>

