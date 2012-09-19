<%@ Page Title="Station Domination Avails | Titan 360" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="avails.aspx.cs" Inherits="Apollo.production_avails" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">    
    <style type="text/css">
        body {overflow:scroll !important;}
        .datepicker, .button {}
        input.ui-button {padding: 0.3em 0.6em !important}
        .ui-button {font-size:9px !important;height:2.5em !important;}
        .featureTable {font-size:10px !important;width:100%;/*white-space:nowrap;*/}
        .featureTable tr {font-size:10px;}
        .featureTable td.bookingCell {border:none !imporant;padding:none !important;cursor:pointer;}
        .featureTable td.statusClass1, .featureTable td.statusClass2,
        .featureTable td.statusClass3, .featureTable td.statusClass4,
        .featureTable td.statusClass5 {border:none !imporant;padding:none !important;cursor:pointer;}
        .featureTable td {border:1px solid #333333;text-align:left;font-size:10px;padding:1px;}
        .featureTable th {border:1px solid #ffffff;text-align:center;padding:2px;font-size:10px;background-color:#000000;color:#ffffff;}        
        .bookingTableHeader td {border:1px solid #ffffff;text-align:center;padding:2px;font-size:10px;font-weight:bold;background-color:#000000;color:#ffffff;}        
        .bookingTableFooter td {background-color:Black;color:Red;font-weight:bold;font-size:13px;padding:3px;}
        .bookingTableFooter a, .bookingTableFooter a:hover {color:White;text-decoration:none;}
        .requiredIndicator {color:#ff0000;font-weight:bold;font-style:italic;margin-left:5px;}
        .hiddenButton {display:none;}
        .statusClass1{background-color:#FFCC99;}
        .statusClass2{background-color:#CCFFFF;}
        .statusClass3{background-color:#FCF305;}
        .statusClass4{background-color:#99CC00;}
        .statusClass5{background-color:#DD0806;}        
        .statusClass6{background-color:#cccccc;}
        #keyDisplay table {border:1px solid #333333;}
        #keyDisplay table td {border:1px solid#333333;text-align:center;font-size:10px;padding:2px;}
        h3 {font-size:16px;}
        h4 {font-size:14px;}
        li {font-size:12px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <script type="text/javascript" language="javascript" src="avails.js?v=1.5"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $(".button").button();
            //$(".datepicker").datepicker();
            SetDateRangePicker("textStartDate", "textEndDate");
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
            if (!IsEditUser()) { return false; }
            if (IsAdminUser()) { return true; }
            if (($get("<%=editUserMarket.ClientID %>").value.indexOf(market) != -1) || market.toLowerCase() == "dallas") {
                return true;
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
            if (IsEditUser() && ($get("<%=editUserMarket.ClientID %>").value == "ALL")) {
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
                        <option value="2014">2014</option>
                        <option value="2015">2015</option>
                        <option value="2016">2016</option>
                        <option value="2017">2017</option>
                    </select>
                </div>
                <div style="float:left;margin:1px 10px 0 0;"><input type="button" class="button" id="displayBookings" value="View Bookings" onclick="DisplayBookings();" /></div>
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

