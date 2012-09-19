<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="targetMaintenance.aspx.cs" Inherits="Apollo.sales_reports_kpi_targetMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        #marketArBreakdownData, #marketArVsTargetData {width:95%}
        #marketDataTable {border:1px solid #333333;}
        #marketArBreakdownData tr, #marketArVsTargetData tr, #marketDataTable tr {padding:2px;border-bottom:1px solid #333333;}
        #marketArBreakdownData th, #marketArVsTargetData th, #marketDataTable th {font-weight:bold;font-size:12px;padding:3px;}
        #marketArBreakdownData td, #marketArVsTargetData td, #marketDataTable td {padding:3px;}
        #marketArBreakdownData td, #marketArVsTargetData td  {border-bottom:1px solid #333333;}
        #marketArBreakdownData span, #marketArVsTargetData span {text-align:right;}
        .numericCell {text-align:right;}
        .numeric{text-align:right;width:35px;}
        .dataEntry{background-color:green;color:white;font-weight:bold;font-size:10px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
    <script type="text/javascript" language="javascript" src="targetMaintenance.js?v=1.3"></script>
    <script type="text/javascript" language="javascript">
        var selectedTargetId, companyId, selectedMarketData;
        $(document).ready(function () {
            $("input[type='text'], select").val("");
            $("#dropDownMarket").bind("change", onMarketChange);
            $("input[id^='textDirect']").bind("change", onDirectChange);
            $("input[id^='textAgency']").bind("change", onAgencyChange);
            $("#marketDataTable, #functionButtons").css({ "display": "none" });
            $(".button").button();
            $("#reportOptionsDialog").dialog({ autoOpen: false, modal: true, width: 175, height: 100 });
            $("#textAgingDate").datepicker();
            selectedTargetId = -1;
        });        
        function RunReport() {
            if ($("#textAgingDate").val() == "") {
                alert("Please specify an Aging Date.");
                return;
            }
            Apollo.KPIService.GetMarketTargetReport($("#dropDownMarket").val(), companyId, $("#textAgingDate").val(), RunReportCallback, ErrorCallback);
            $("#reportOptionsDialog").dialog("close");
            startWait("Retrieving A/R Data...");
        }
    </script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
    <div style="margin:15px;">
        <fieldset style="width:350px;padding:10px;">
            <legend style="font-size:12px;font-weight:bold;">Market Target A/R % Maintenance</legend>
            <div>
                <div style="float:left;padding-top:5px;">
                    <select id="dropDownMarket">
                        <option value="" selected="selected">- Select Market -</option>
                        <option value="BOS">Boston</option>
                        <option value="NC">Charlotte</option>
                        <option value="CHI">Chicago</option>
                        <option value="DAL">Dallas</option>
                        <option value="LA">Los Angeles</option>
                        <option value="MIN">Minneapolis</option>
                        <option value="NJ">New Jersey</option>
                        <option value="NYO">New York</option>
                        <option value="PHI">Philadelphia</option>
                        <option value="SF">San Francisco</option>
                        <option value="SEA">Seattle</option>
                        <option value="TOR">Toronto</option>
                    </select>
                </div>
                <div style="float:right">
                    <input type="button" class="button" id="runReport" value="Run A/R vs. Target Report." onclick="OpenReportOptionsDialog();" style="display:none;" />
                </div>
                <div style="clear:both;"></div>
            </div>
            <div style="margin-top:15px;">
                <table id="marketDataTable" style="display:none;">
                    <thead><tr><th>Market</th><th>Cust. Type</th><th>Under 60 %</th><th>61 - 120 %</th><th>Over 120 %</th></tr></thead>
                    <tbody>
                        <tr id="directRow" style="border-bottom:1px solid #333333;">
                            <td rowspan="2"><span id="labelMarket" style="font-size:12px;font-weight:bold;"></span></td>
                            <td style="font-size:11px;font-weight:bold;">Direct</td>
                            <td><input type="text" id="textDirectUnder60" class="numeric dataEntry" /></td>
                            <td><input type="text" id="textDirect61To120" class="numeric dataEntry" /></td>
                            <td><input type="text" id="textDirectOver120" class="numeric dataEntry" /></td>
                        </tr>
                        <tr id="agencyRow">
                            <td style="font-size:11px;font-weight:bold;">Agency</td>
                            <td><input type="text" id="textAgencyUnder60" class="numeric dataEntry" /></td>
                            <td><input type="text" id="textAgency61To120" class="numeric dataEntry" /></td>
                            <td><input type="text" id="textAgencyOver120" class="numeric dataEntry" /></td>
                        </tr>
                    </tbody>
                </table>                
            </div>
            <div id="functionButtons" style="float:right;margin-top:10px;display:none;">
                <input type="button" value="Save" id="saveButton" class="button" onclick="Save();" />
            </div>
            <div style="clear:both;"></div>
        </fieldset>
        <fieldset style="margin-top:15px;width:600px;padding:10px;">
            <legend style="font-size:12px;font-weight:bold;">Market A/R % Breakdown</legend>
            <div>
                <table id="marketArBreakdownData">
                    <thead>
                        <tr><th>Aging Categories</th><th>Directs</th><th>&nbsp;</th><th>Agencies</th><th>&nbsp;</th><th>Totals</th><th>&nbsp;</th></tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Current</td>
                            <td class="numericCell"><span id="labelDirectCurrent"></span></td><td class="numericCell"><span id="labelDirectCurrentPerc"></span></td>
                            <td class="numericCell"><span id="labelAgencyCurrent"></span></td><td class="numericCell"><span id="labelAgencyCurrentPerc"></span></td>
                            <td class="numericCell"><span id="labelTotalCurrent"></span></td><td class="numericCell"><span id="labelTotalCurrentPerc"></span></td>
                        </tr>
                        <tr>
                            <td>31-60</td>
                            <td class="numericCell"><span id="labelDirect3160"></span></td><td class="numericCell"><span id="labelDirect3160Perc"></span></td>
                            <td class="numericCell"><span id="labelAgency3160"></span></td><td class="numericCell"><span id="labelAgency3160Perc"></span></td>
                            <td class="numericCell"><span id="labelTotal3160"></span></td><td class="numericCell"><span id="labelTotal3160Perc"></span></td>
                        </tr>
                        <tr>
                            <td>61-90</td>
                            <td class="numericCell"><span id="labelDirect6190"></span></td><td class="numericCell"><span id="labelDirect6190Perc"></span></td>
                            <td class="numericCell"><span id="labelAgency6190"></span></td><td class="numericCell"><span id="labelAgency6190Perc"></span></td>
                            <td class="numericCell"><span id="labelTotal6190"></span></td><td class="numericCell"><span id="labelTotal6190Perc"></span></td>
                        </tr>
                        <tr>
                            <td>91-120</td>
                            <td class="numericCell"><span id="labelDirect91120"></span></td><td class="numericCell"><span id="labelDirect91120Perc"></span></td>
                            <td class="numericCell"><span id="labelAgency91120"></span></td><td class="numericCell"><span id="labelAgency91120Perc"></span></td>
                            <td class="numericCell"><span id="labelTotal91120"></span></td><td class="numericCell"><span id="labelTotal91120Perc"></span></td>
                        </tr>
                        <tr>
                            <td>121-150</td>
                            <td class="numericCell"><span id="labelDirect121150"></span></td><td class="numericCell"><span id="labelDirect121150Perc"></span></td>
                            <td class="numericCell"><span id="labelAgency121150"></span></td><td class="numericCell"><span id="labelAgency121150Perc"></span></td>
                            <td class="numericCell"><span id="labelTotal121150"></span></td><td class="numericCell"><span id="labelTotal121150Perc"></span></td>
                        </tr>
                        <tr>
                            <td>Over 150</td>
                            <td class="numericCell"><span id="labelDirectOver150"></span></td><td class="numericCell"><span id="labelDirectOver150Perc"></span></td>
                            <td class="numericCell"><span id="labelAgencyOver150"></span></td><td class="numericCell"><span id="labelAgencyOver150Perc"></span></td>
                            <td class="numericCell"><span id="labelTotalOver150"></span></td><td class="numericCell"><span id="labelTotalOver150Perc"></span></td>
                        </tr>
                        <tr>
                            <td>Totals:</td>
                            <td class="numericCell"><span id="labelDirectTotal"></span></td><td class="numericCell">&nbsp;</td>
                            <td class="numericCell"><span id="labelAgencyTotal"></span></td><td class="numericCell">&nbsp;</td>
                            <td class="numericCell"><span id="labelAllTotal"></span></td><td class="numericCell">&nbsp;</td>
                        </tr>
                    </tbody>
                </table>                
            </div>
        </fieldset>
        <fieldset style="margin-top:15px;width:450px;padding:10px;">
            <legend style="font-size:12px;font-weight:bold;">Market A/R % vs. Target %</legend>
            <div>
                <table id="marketArVsTargetData">
                    <thead>
                        <tr><th>&nbsp;</th><th>&nbsp;</th><th>Under 60</th><th>61 - 120</th><th>Over 120</th><th>&nbsp;</th></tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td rowspan="2">Direct</td>
                            <td>Current</td><td class="numericCell"><span id="labelVsDirectUnder60"></span></td>
                            <td class="numericCell"><span id="labelVsDirect61120"></span></td>
                            <td class="numericCell"><span id="labelVsDirectOver120"></span></td>
                        </tr>
                        <tr>
                            <td>Target</td><td class="numericCell"><span id="labelVsDirectTargetUnder60"></span></td>
                            <td class="numericCell"><span id="labelVsDirectTarget61120"></span></td>
                            <td class="numericCell"><span id="labelVsDirectTargetOver120"></span></td>
                        </tr>
                        <tr>
                            <td rowspan="2">Agency</td>
                            <td>Current</td><td class="numericCell"><span id="labelVsAgencyUnder60"></span></td>
                            <td class="numericCell"><span id="labelVsAgency61120"></span></td>
                            <td class="numericCell"><span id="labelVsAgencyOver120"></span></td>
                        </tr>
                        <tr>
                            <td>Target</td><td class="numericCell"><span id="labelVsAgencyTargetUnder60"></span></td>
                            <td class="numericCell"><span id="labelVsAgencyTarget61120"></span></td>
                            <td class="numericCell"><span id="labelVsAgencyTargetOver120"></span></td>
                        </tr>
                    </tbody>
                </table>
                <div style="float:right;margin-top:10px;"><input type="button" class="button" value="Print" onclick="PrintReport();" /></div>
                <div style="clear:both;"></div>
            </div>
        </fieldset>
    </div>
    <div id="reportOptionsDialog">
        <label for="textAgingDate" style="margin-right:10px;">Aging Date:</label><input type="text" id="textAgingDate" style="width:65px;text-align:right" />
        <div>
            <div style="float:right;margin-top:10px;">
                <input type="button" class="button" value="Run Report" onclick="RunReport();" />
                <input type="button" class="button" value="Cancel" onclick="CancelReport();" />
            </div>
            <div style="clear:both;"></div>
        </div>
    </div>
</asp:Content>

