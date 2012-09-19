<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Apollo.digitalAvails_Default" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="digitalAvails.css" type="text/css" rel="Stylesheet" />
    <!--[if IE]><!-->
    <style type="text/css">
        body {overflow:auto !important;}
    </style> 
    <!--<![endif]-->    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="gridOptions">
        <div>
            <div style="float:left;width:50px;"><label for="dropDownMarket">Market:</label></div>
            <div style="float:left;"><select id="dropDownMarket"></select></div>
            <div style="float:left;margin-left:10px;"><label for="dropDownStation">Location:</label></div>
            <div style="float:left;margin-left:5px;"><select id="dropDownStation"><option value="">- Select a Location -</option></select></div>
            <div style="clear:both;"></div>
        </div>            
        <div style="margin-top:10px;border-top:1px solid #333333;padding-top:5px;">
            <div style="float:left;width:75px;"><label for="dropDownMonth">Start Month:</label></div>
            <div style="float:left;">
                <select id="dropDownMonth">
                    <option value="1">January</option><option value="2">February</option><option value="3">March</option>
                    <option value="4">April</option><option value="5">May</option><option value="6">June</option>
                    <option value="7">July</option><option value="8">August</option><option value="9">September</option>
                    <option value="10">October</option><option value="11">November</option><option value="12">December</option>
                </select>
            </div>
            <div style="float:left;margin-left:10px;"><label for="dropDownYear">Year:</label></div>
            <div style="float:left;margin-left:5px;">
                <select id="dropDownYear">
                    <option value="2009">2009</option><option value="2010">2010</option><option value="2011">2011</option>
                    <option value="2012">2012</option><option value="2013">2013</option><option value="2014">2014</option>
                    <option value="2015">2015</option>                    
                </select>
            </div>
            <div style="float:left;margin-left:10px;"><label for="dropDownNumberOfMonths"># of Months:</label></div>
            <div style="float:left;margin-left:5px;">
                <select id="dropDownNumberOfMonths">
                    <option value="1">1</option><option value="2">2</option><option value="3" selected="selected">3</option>
                    <option value="4">4</option><option value="5">5</option><option value="6">6</option>
                    <option value="7">7</option><option value="8">8</option><option value="9">9</option>
                    <option value="10">10</option><option value="11">11</option><option value="12">12</option>
                </select>
            </div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:15px;text-align:right;">
            <input type="button" class="button" id="generateSalesSummary" onclick="GetSalesSummary();" value="Get Digital Sales Summary" style="margin-right:10px;display:none;" />
            <input type="button" class="button" id="generatePrintPreview" onclick="PrintPreview();" value="Print" style="margin-right:10px;" />            
            <asp:Button ID="cmdExport" runat="server" CssClass="button" OnClientClick="return ExportToExcel();" onclick="cmdExport_Click" Text="Export" style="margin-right:10px;" />            
            <asp:Button ID="cmdExportMarket" runat="server" CssClass="button" OnClientClick="return ExportMarketToExcel();" Text="Export Market Data" onclick="cmdExportMarket_Click" style="display:none;" />
            <input type="button" class="button" id="exportMarketData" onclick="ExportMarketToExcel();" value="Export Market Data" />
            <input type="button" class="button" id="viewMarketData" onclick="ViewMarketData();" value="View Market Data" />
            <br /><br />
            <input type="button" class="button" id="executeMarketUtilizationReport" onclick="LaunchMarketUtilizationReport();" value="Market Utilization Report" />
            <asp:Button ID="cmdMarketUtilizationReport" runat="server" OnClientClick="LaunchMarketUtilizationReport();" style="display:none;" onclick="cmdMarketUtilizationReport_Click" />
            <input type="button" class="button" id="executeCampaignSearch" onclick="LaunchCampaignSearch();" value="Campaign Search" />
            <input type="button" class="button" id="regenerateAvails" onclick="GetAvails();" value="Get Avails" />
        </div>
        <div style="clear:both;"></div>
    </div>
    <div id="availsData">
        <div id="stationInfo">
            <span class="stationName" id="stationNameDisplay"></span>
            <span class="stationDesc" id="stationDescDisplay"></span>
            <table id="displayKey">
                <tr>
                    <td>Key:</td>
                    <td style="background-color:#FFFF99;">Paid</td>
                    <td style="background-color:#FF99CC;">GB</td>
                    <td style="background-color:#CCFFCC;">SAB</td>
                    <td style="background-color:#DD0806;">Hold</td>
                    <td style="background-color:#FFCC99;" id="njtKey">NJT</td>
                    <td style="background-color:#FFCC99;" id="martaKey">MARTA</td>
                </tr>
            </table>
        </div>
        <div id="avails"><table id="availsTable" class="featureTable" style="width:95%;margin:0 auto;" cellspacing="5"></table></div>
    </div>
    <div id="spotEntryDialog" class="dialog" style="display:none;">
        <div style="margin-bottom:10px;">
            <div style="float:left;width:100px;text-align:right;">Spot #:</div>
            <div style="float:left;margin-left:10px;"><span id="textSpotNumber" style="width:125px;"></span></div>
            <div style="clear:both;"></div>
        </div>
        <div id="dateRangeSelection" style="display:none;">
            <div style="float:left;">
                <div style="float:left;width:100px;text-align:right;">Start Date:</div>
                <div style="float:left;margin-left:10px;"><input type="text" id="textSpotStartDate" class="datepicker" style="width:65px;text-align:right;" /></div>
                <div style="clear:both;"></div>
            </div>
            <div style="float:left;margin-left:10px">
                <div style="float:left;width:50px;text-align:right;">End Date:</div>
                <div style="float:left;margin-left:10px;"><input type="text" id="textSpotEndDate" class="datepicker" style="width:65px;text-align:right;" /></div>
                <div style="clear:both;"></div>
            </div>
            <div style="clear:both;"></div>
        </div>
        <div id="singleDate" style="display:none;">
            <div style="float:left;width:100px;text-align:right;">Spot Date:</div>
            <div style="float:left;margin-left:10px;"><input type="text" id="textSpotDate" style="width:65px;text-align:right;" disabled="disabled" /></div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:100px;text-align:right;">Campaign Name:</div>
            <div style="float:left;margin-left:10px;"><input type="text" id="textCampaignName" style="width:150px;" tabindex="1" /></div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:100px;text-align:right;">Campaign #:</div>
            <div style="float:left;margin-left:10px;"><input type="text" id="textCampaignNumber" style="width:150px;" /></div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:100px;text-align:right">Spot Type:</div>
            <div style="float:left;margin-left:10px;"><select id="dropDownSpotType"></select></div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:100px;text-align:right;margin-top:10px;">Description:</div>
            <div style="float:left;margin-left:10px;margin-top:10px;"><textarea id="textDescription" cols="30" rows="5"></textarea></div>
            <div style="clear:both;"></div>
        </div>
        <div id="numberOfWeeksEntry" style="margin-top:10px;">
            <div style="float:left;width:100px;text-align:right;"># of Weeks:</div>
            <div style="float:left;margin-left:10px;"><input type="text" id="textNumberOfWeeks" style="width:25px;text-align:right;" /></div>
            <div style="clear:both;"></div>
        </div>
        <div id="numberOfWeeksDetected" style="margin-top:10px;">
            <div style="float:left;width:100px;text-align:right;">Total # of Weeks<br />for Campaign:</div>
            <div style="float:left;margin-left:10px;"><span id="textTotalNumberOfWeeks" style="width:200px;text-align:right;" /></div>
            <div style="clear:both;"></div>
        </div>
    </div>
    <div id="spotCopyDialog" class="dialog" style="display:none;">
        <div>
            <div>
                <div style="float:left;width:100px;text-align:right;">Copy To Spot #:</div>
                <div style="float:left;margin-left:10px;"><span id="textCopyToPosition" style="width:25px;text-align:right;"></span></div>
                <div style="clear:both;"></div>
            </div>
            <div style="margin-top:10px;">
                <div style="float:left;width:100px;text-align:right;"># of Weeks to Copy:</div>
                <div style="float:left;margin-left:10px;"><input type="text" id="textNumberOfWeeksCopy" style="width:25px;text-align:right;" /></div>
                <div style="clear:both;"></div>
            </div>
        </div>
    </div>
    <div id="spotUpdateDeleteDialog" class="dialog" style="display:none;">
        <div>
            <div style="float:left;width:175px;text-align:right;">Total # of Weeks for Campaign:</div>
            <div style="float:left;margin-left:10px;"><span id="textDlgTotalNumberOfWeeks" style="text-align:right;" /></div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:175px;text-align:right;">Include current spot in changes?</div>
            <div style="float:left;margin-left:10px;">
                <input type="radio" id="radioIncludeCurrentYes" name="radioIncludeCurrent" value="Yes" checked="checked" />Yes
                <input type="radio" id="radioIncludeCurrentNo" name="radioIncludeCurrent" value="No" style="margin-left:10px;" />No
            </div> 
            <div style="clear:both;"></div>
        </div>
    </div>
    <div id="spotMultiStationDialog" class="dialog" style="display:none;">
        <div style="height:300px;overflow-y:scroll;">
            <div style="float:left;width:150px;text-align:right;">Select Additional Stations:</div>
            <div style="float:left;margin-left:10px;">
                <ul id="additionalStationList">
                    <li><input type="checkbox" id="toggleAllAdditionalStations" />All</li>
                </ul>
            </div>
            <div style="clear:both;"></div>
        </div>
    </div>
    <div id="marketExportDialog" class="dialog" style="display:none;">
        <div>
            <div style="float:left;width:150px;text-align:right;">Start Year:</div>
            <div style="float:left;margin-left:10px;">
                <select id="dropDownExportStartYear">
                    <option value="2009">2009</option><option value="2010">2010</option><option value="2011">2011</option>
                    <option value="2012">2012</option><option value="2013">2013</option><option value="2014">2014</option>
                    <option value="2015">2015</option>                    
                </select>
            </div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:150px;text-align:right;">Start Month:</div>
            <div style="float:left;margin-left:10px;">
                <select id="dropDownExportStartMonth">
                    <option value="1">January</option><option value="2">February</option><option value="3">March</option>
                    <option value="4">April</option><option value="5">May</option><option value="3">June</option>
                    <option value="7">July</option><option value="8">August</option><option value="9">September</option>
                    <option value="10">October</option><option value="11">November</option><option value="12">December</option>
                </select>                
            </div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:150px;text-align:right;"># of Months:</div>
            <div style="float:left;margin-left:10px;">
                <select id="dropDownExportNumberOfMonths">
                    <option value="1">1</option><option value="2">2</option><option value="3" selected="selected">3</option>
                    <option value="4">4</option><option value="5">5</option><option value="6">6</option>
                    <option value="7">7</option><option value="8">8</option><option value="9">9</option>
                    <option value="10">10</option><option value="11">11</option><option value="12">12</option>
                    <option value="24">24</option>
                </select>
            </div>
            <div style="clear:both;"></div>
        </div>
    </div>
    <div id="campaignSearchDialog" class="dialog" style="display:none;">
        <div style="float:left;width:175px;">Campaign Name or Number:</div>
        <div style="float:left;margin-left:10px;"><input type="text" id="textCampaignSearch" style="width:200px;" /></div>
        <div style="clear:both;"></div>
        <div id="campaignSearchResults" style="margin-top:10px;"></div>
    </div>
    <div id="marketUtilizationDialog" class="dialog" style="display:none;">
        <div>
            <div style="float:left;width:75px;">Market:</div>
            <div style="float:left;margin-left:10px;"><select id="dropDownUtilizationMarket"></select></div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:10px;">
            <div style="float:left;width:75px;">Year:</div>
            <div style="float:left;margin-left:10px;"><select id="dropDownUtilizationYear" style="display:none;"></select></div>
            <div style="clear:both;margin-top:10px;"></div>
        </div>
        <div style="margin-top:10px;">
            <input type="checkbox" id="checkUtilizationGB" /><label for="checkUtilizationGB" style="margin-left:5px;">Include Guaranteed Bonus?</label>
        </div>
        <div style="margin-top:10px;">
            <input type="checkbox" id="checkUtilizationSAB" /><label for="checkUtilizationSAB" style="margin-left:5px;">Include Space-Avail Bonus?</label>
        </div>
    </div>
    <script type="text/javascript" src="digitalAvails.js?v=1.15"></script>
    <script type="text/javascript">
        var selectedCell = {};
        var copiedCell = {};
        var stationData;
        function InitDialog(isUpdating) {
            $("#spotEntryDialog").dialog("destroy");
            $("#spotEntryDialog").dialog({ title: "Add Spot", modal: true, autoOpen: false, width: 450, height: 425 });
            var selectedDate = new Date(selectedCell.year, selectedCell.month - 1, selectedCell.day);
            $("#textSpotDate").val(DateDisplay(selectedDate));
            $("#numberOfWeeksEntry").css({ "display": (isUpdating ? "none" : "block") });
            $("#dateRangeSelection").css({ "display": (isUpdating ? "none" : "block") });
            $("#singleDate").css({ "display": (isUpdating ? "block" : "none") });
            if (isUpdating) {
                $("#spotEntryDialog").dialog("option", "buttons", { "Cancel": function () { $(this).dialog("close") }, "Delete...": OpenDeleteSubDialog, "Update...": OpenUpdateSubDialog });
            } else {
                $("#textSpotStartDate").datepicker({ defaultDate: selectedDate, minDate: selectedDate, changeMonth: true, changeYear: true }).val(DateDisplay(selectedDate));
                $("#textSpotEndDate").datepicker({ minDate: selectedDate, onSelect: OnEndDateChange, changeMonth: true, changeYear: true }).val("");
                $("#spotEntryDialog").dialog("option", "buttons", { "Cancel": function () { $(this).dialog("close") }, "Add...": OnDialogSubmit });
            }
        }
        function CanEdit() {
            if ($get("<%=isDigitalAvailsAdmin.ClientID %>").value == "-1") { return true; }
            if (stationData.Market.indexOf("Atlanta") != -1 && $get("<%=isAtlantaEditor.ClientID %>").value == "-1") { return true; }
            if (stationData.Market.indexOf("Chicago") != -1 && $get("<%=isChicagoEditor.ClientID %>").value == "-1") { return true; }
            if (stationData.Market.indexOf("New Jersey") != -1 && $get("<%=isNewJerseyEditor.ClientID %>").value == "-1") { return true; }
            if ((stationData.Market.indexOf("Philadelphia") != -1 || stationData.Market.indexOf("Amtrak") != -1) && $get("<%=isPhiladelphiaEditor.ClientID %>").value == "-1") { return true; }
            if (stationData.Market.indexOf("New York") != -1 && $get("<%=isNewYorkEditor.ClientID %>").value == "-1") { return true; }
            if ((stationData.Market.indexOf("Toronto") != -1 || stationData.Market.indexOf("Montreal") != -1 || stationData.Market.indexOf("Vancouver") != -1) && $get("<%=isTorontoEditor.ClientID %>").value == "-1") { return true; }
            return false;
        }
        function ExportToExcel() {
            if (!ValidateStationSelection()) { return false; }
            if (stationData === undefined || stationData.StationId === undefined || stationData.Name === undefined || stationData.Name == "") { alert("Please run avails for selected location."); return; }
            $get("<%=marketName.ClientID %>").value = $("#dropDownMarket option:selected").text();
            $get("<%=reportStationId.ClientID %>").value = stationData.StationId;
            $get("<%=stationName.ClientID %>").value = stationData.Name;
            $get("<%=wantSingleStation.ClientID %>").value = "-1";
            $get("<%=startMonth.ClientID %>").value = $("#dropDownMonth").val();  //DetermineStartMonth($("#dropDownQuarter").val()) + 1;
            $get("<%=startYear.ClientID %>").value = $("#dropDownYear").val();
            $get("<%=numberOfMonths.ClientID %>").value = $("#dropDownNumberOfMonths").val();
            return true;
        }
        function ExportMarketToExcel() {
            if (!ValidateMarketSelection()) { return false; }
            //Open dialog
            $("#dropDownExportStartYear").val(new Date().getFullYear());
            $("#dropDownExportNumberOfMonths").val(12);
            $("#marketExportDialog").dialog("destroy");
            $("#marketExportDialog").dialog({ title: "Export Market Data", modal: true, autoOpen: false, width: 350, height: "auto" });
            $("#marketExportDialog").dialog("option", "buttons",
                {
                    "Cancel": function () {
                        $(this).dialog("close");
                        $get("<%=marketName.ClientID %>").value = "";
                        $get("<%=startMonth.ClientID %>").value = "";
                        $get("<%=startYear.ClientID %>").value = "";
                        $get("<%=numberOfMonths.ClientID %>").value = "";
                    },
                    "OK": function () {
                        $get("<%=marketName.ClientID %>").value = $("#dropDownMarket option:selected").text();
                        $get("<%=startMonth.ClientID %>").value = $("#dropDownExportStartMonth").val();
                        $get("<%=startYear.ClientID %>").value = $("#dropDownExportStartYear").val();
                        $get("<%=numberOfMonths.ClientID %>").value = $("#dropDownExportNumberOfMonths").val();
                        $get("<%=cmdExportMarket.ClientID %>").click();
                        $get("<%=wantSingleStation.ClientID %>").value = "0";
                        $get("<%=reportStationId.ClientID %>").value = "-1";
                        $(this).dialog("close");
                    }
                }
            );
            $("#marketExportDialog").dialog("open");
        }
        function LaunchMarketUtilizationReport() {
            var dialog = $("#marketUtilizationDialog");
            $("#marketUtilizationDialog").dialog("destroy");
            $("#marketUtilizationDialog").dialog({ title: "Market Utilization Report", modal: true, autoOpen: false, width: 275, height: "auto" });
            $("#marketUtilizationDialog").dialog("option", "buttons", {
                "Cancel": function () {
                    $(this).dialog("close");
                },
                "Run Report": function () {
                    if ($("#dropDownUtilizationMarket").val() == "") { alert("Choose a Market to run the report for."); return; }
                    if ($("#dropDownUtilizationYear").val() == "") { alert("Choose a Year to run the report for."); return; }
                    $get("<%=marketUtilizationMarket.ClientID %>").value = $("#dropDownUtilizationMarket").val();
                    $get("<%=marketUtilizationYear.ClientID %>").value = $("#dropDownUtilizationYear").val();
                    $get("<%=marketUtilizationIncludeSAB.ClientID %>").value = ($("#checkUtilizationSAB").is(":checked") ? "-1" : "0");
                    $get("<%=marketUtilizationIncludeGB.ClientID %>").value = ($("#checkUtilizationGB").is(":checked") ? "-1" : "0");
                    $get("<%=cmdMarketUtilizationReport.ClientID %>").click();
                    $(this).dialog("close");
                }
            });
            dialog.dialog("open");
        }
        function ViewMarketData() {
            var market = $("#dropDownMarket").val();
            if (market == "") {
                alert("Please choose a market.");
                return;
            }
            var numberOfMonths = parseInt($("#dropDownNumberOfMonths").val(), 10);
            var startMonth = parseInt($("#dropDownMonth").val(), 10);
            var startYear = parseInt($("#dropDownYear").val(), 10);
            var url = "DigitalAvailsPrintPreview.aspx?market=" + market;
            if (numberOfMonths != "") { url += "&numMonths=" + numberOfMonths; }
            if (startMonth != "") { url += "&startMonth=" + startMonth; }
            if (startYear != "") { url += "&year=" + startYear; }
            var w = window.open(url, "printDigitalAvails", "width=600,height=600,scrollbars=1,resizable=1,menubar=1,statusbar=1,location=1,toolbar=1");
            w.focus();
        }
        $(document).ready(function () {
            //$("#dropDownQuarter").val(DetermineQuarter());
            $("#dropDownYear").val(new Date().getFullYear());
            $("#dropDownMonth").val(new Date().getMonth() + 1);
            $(".button").button();
            Apollo.DigitalAvailsService.GetMarketList("", AddToList, null, "dropDownMarket");
            Apollo.DigitalAvailsService.GetReportMarketList(AddToList, null, "dropDownUtilizationMarket");
            $("#dropDownMarket").bind("change", OnMarketChange);
            $("#dropDownUtilizationMarket").bind("change", OnUtilizationMarketChange);
            $("#spotCopyDialog").dialog({ title: "Copy Spot", modal: true, autoOpen: false, width: 300, height: 225 });
            $("#spotCopyDialog").dialog("option", "buttons", { "Cancel": function () { $(this).dialog("close"); }, "Submit": OnCopyDialogSubmit });
            $("#toggleAllAdditionalStations").bind('click', null, ToggleAllAdditionalStations);
        });
    </script>
    <asp:HiddenField runat="server" ID="isAtlantaEditor" Value="0" />
    <asp:HiddenField runat="server" ID="isChicagoEditor" Value="0" />
    <asp:HiddenField runat="server" ID="isNewJerseyEditor" Value="0" />
    <asp:HiddenField runat="server" ID="isPhiladelphiaEditor" Value="0" />
    <asp:HiddenField runat="server" ID="isNewYorkEditor" Value="0" />
    <asp:HiddenField runat="server" ID="isTorontoEditor" Value="0" />
    <asp:HiddenField runat="server" ID="isDigitalAvailsAdmin" Value="0" />
    <asp:HiddenField runat="server" ID="excelExport" Value="" />
    <asp:HiddenField runat="server" ID="stationName" Value="" />
    <asp:HiddenField runat="server" ID="reportStationId" Value="" />
    <asp:HiddenField runat="server" ID="wantSingleStation" Value="" />
    <asp:HiddenField runat="server" ID="marketName" Value="" />
    <asp:HiddenField runat="server" ID="startMonth" Value="" />
    <asp:HiddenField runat="server" ID="startYear" Value="" />
    <asp:HiddenField runat="server" ID="numberOfMonths" Value="" />
    <asp:HiddenField runat="server" ID="marketUtilizationMarket" Value="" />
    <asp:HiddenField runat="server" ID="marketUtilizationYear" Value="" />
    <asp:HiddenField runat="server" ID="marketUtilizationIncludeSAB" Value="" />
    <asp:HiddenField runat="server" ID="marketUtilizationIncludeGB" Value="" />
</asp:Content>