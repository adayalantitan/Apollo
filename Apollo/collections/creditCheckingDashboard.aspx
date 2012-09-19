<%@ Page Title="Credit Checking Dashboard | Titan 360" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="creditCheckingDashboard.aspx.cs" Inherits="Apollo.collections_creditCheckingDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
    <style type="text/css">
           /* Hack to get Ajax calendar to appear over the jqGrid */
        .ajax__calendar_container {z-index:9999 !important;}
    </style>
    <script type="text/javascript">                
        function ClearSearchFilters(){      
            $get("<%=textEntryDateFrom.ClientID %>").value = "";
            $get("<%=textEntryDateTo.ClientID %>").value = "";
            $get("<%=dropDownCustomerType.ClientID %>").value = "";
            $get("<%=dropDownCompany.ClientID %>").value = "";
            $get("<%=dropDownContractValue.ClientID %>").value = "";
            $("#list1").setGridParam({ postData: BuildPostParams() }).trigger("reloadGrid");
        }
        function ExecuteSearch(){
            if (!ValidateDates()){return;}
            $("#list1").setGridParam({ postData: BuildPostParams() }).trigger("reloadGrid");
        }
        function BuildPostParams() {
            var postParams = { fromDate: $get("<%=textEntryDateFrom.ClientID %>").value,
                toDate: $get("<%=textEntryDateTo.ClientID %>").value,
                customerType: $get("<%=dropDownCustomerType.ClientID %>").value,
                companyId: $get("<%=dropDownCompany.ClientID %>").value,
                contractValue: $get("<%=dropDownContractValue.ClientID %>").value,
                status: $get("<%=dropDownStatus.ClientID %>").value
            };
            return postParams;
        }          
        function ValidateDates(){
            var auditFrom = $get("<%=textEntryDateFrom.ClientID %>").value;
            var auditTo = $get("<%=textEntryDateTo.ClientID %>").value;
            if (auditFrom != "") {                
                if (!IsValidDate(auditFrom)) {
                    alert("Entry From Date is not in a valid format."); 
                    return false;
                }
            }
            if (auditTo != "") {
                if (!IsValidDate(auditTo)) {
                    alert("Entry To Date is not in a valid format."); 
                    return false;
                }
            }
            if (Date(auditTo) < Date(auditFrom)){
                alert ("Entry To Date can not be earlier than Entry From Date.");
                return false;
            }   
            return true;
        }
    </script>
    <div class="inventory_search">SEARCH FILTERS</div>
    <div class="horizontalSearchCriteriaArea">
        <div style="float:left;width:20%;text-align:left;">
            <ul class="horizontalSearchFilters">                
                <li>   
                    <div>
                        <div style="float:left">
                            Contract Entry Date:
                        </div>
                        <div style="float:left;padding-left:10px">
                            From: <asp:TextBox ID="textEntryDateFrom" runat="server" Width="65px" />
                            <img src="../Images/calendar.gif" id="calImage1" runat="server" alt="Click for Calendar" class="calendarPopup" />
                            <ajax:CalendarExtender ID="textEntryDateFrom_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="textEntryDateFrom" PopupButtonID="calImage1" />                                
                        </div>
                        <div style="float:left;padding-left:10px">
                            To: <asp:TextBox ID="textEntryDateTo" runat="server" Width="65px" />
                            <img src="../Images/calendar.gif" id="calImage2" runat="server" alt="Click for Calendar" class="calendarPopup" />
                            <ajax:CalendarExtender ID="textEntryDateTo_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="textEntryDateTo" PopupButtonID="calImage2" />                                
                        </div>
                        <div style="clear:both"></div>
                    </div> 
                </li>
            </ul>
        </div>
        <div style="float:left;width:225px;text-align:left;">
            <ul class="horizontalSearchFilters">
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Customer Type:</div>
                        <div class="horizontalSearchFilterRightColumn">          
                            <asp:DropDownList ID="dropDownCustomerType" runat="server" Width="135px">
                                <asp:ListItem Selected="True" Value="" Text=" * ALL" />
                                <asp:ListItem Value="New" Text="New" />
                                <asp:ListItem Value="Existing" Text="Existing" />
                            </asp:DropDownList>                                                                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="float:left;width:175px;text-align:left;">
            <ul class="horizontalSearchFilters">
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Company:</div>
                        <div class="horizontalSearchFilterRightColumn">          
                            <asp:DropDownList ID="dropDownCompany" runat="server" Width="80px">
                                <asp:ListItem Selected="True" Value="" Text=" * ALL" />
                                <asp:ListItem Value="1" Text="Titan US" />
                                <asp:ListItem Value="2" Text="Titan Canada" />
                            </asp:DropDownList>                                                                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="float:left;width:175px;text-align:left;">
            <ul class="horizontalSearchFilters">
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Status:</div>
                        <div class="horizontalSearchFilterRightColumn">          
                            <asp:DropDownList ID="dropDownStatus" runat="server" Width="75px">
                                <asp:ListItem Selected="True" Value="" Text=" * ALL" />
                                <asp:ListItem Value="Offer" Text="Offer" />
                                <asp:ListItem Value="Sold" Text="Sold" />
                            </asp:DropDownList>                                                                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="float:left;width:225px;text-align:left;">
            <ul class="horizontalSearchFilters">
                <li>
                    <div>
                        <div class="horizontalSearchFilterLeftColumn">Contract Value:</div>
                        <div class="horizontalSearchFilterRightColumn">          
                            <asp:DropDownList ID="dropDownContractValue" runat="server" Width="135px">
                                <asp:ListItem Selected="True" Value="" Text=" * ALL" />
                                <asp:ListItem Value="LessThen3500" Text=" <= $3,500" />
                                <asp:ListItem Value="MoreThen3500" Text=" > $3,500" />
                            </asp:DropDownList>                                                                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="float:left;width:10%;text-align:left;padding-top:10px;">                                         
            <img id="clear" runat="server" style="cursor:pointer" src="~/Images/but_clear.gif" onclick="ClearSearchFilters();" alt="Clear Search Filters" />
            <img id="search" runat="server" style="cursor:pointer" src="~/Images/but_search.gif" onclick="ExecuteSearch();" alt="Search" />                                                                                                                                
        </div>  
        <div style="clear:both"></div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportListPanel" Runat="Server">
    <style type="text/css">
        .ColTable td {text-align:left !important;}
    </style>
    <asp:ImageButton ID="excelExport" runat="server" 
        ImageUrl="/Images/but_excel.gif" AlternateText="Export To Excel" 
        style="margin-left:10px;" onclick="excelExport_Click" />    
    <script type="text/javascript">
        $(document).ready(function () {
            var lastsel;
            var postParams = BuildPostParams();
            $("#list1").jqGrid({
                url: "../services/CollectionsService.asmx/GetCreditCheckInfo"
                , datatype: "xml"
                , postData: postParams
                , colNames: ["Contract #", "Company", "Customer Class", "Parent Product Class", "Program Name", "Entry Date", "Status", "Adv. Code", "Advertiser", "Adv. Created On", "Agy. Code", "Agency", "Agy. Created On", "Contract Start", "Contract Finish", "Weeks", "AE", "Contract Amount", "Open Pmts", "Past 12 Sales - CR", "Customer Type", "Threshold", "Credit App. Prepay / CCA", "Credit Checked", "Notes"]
                , colModel: [{ name: "contractNumber", index: "ContractNumber", width: 80, search: true, searchoptions: { sopt: ["eq"] }, sort: true, align: "center" }
                    , { name: "company", index: "Company", width: 100, search: false, sort: true }
  		            , { name: "customerClass", index: "CustomerClassType", width: 100, search: true, searchoptions: { sopt: ["eq"] }, sort: true }
                    , { name: "parentProductClass", index: "ParentProductClass", width: 100, search: true, searchoptions: { sopt: ["eq"] }, sort: true }
                    , { name: "programName", index: "ProgramName", width: 125, search: true, searchoptions: { sopt: ["eq"] }, sort: true }
                    , { name: "contractEnteredOn", index: "ContractEnteredOn", width: 100, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: "status", index: "Status", width: 55, search: true, searchoptions: { sopt: ["eq"] }, sort: true, align: "center" }
                    , { name: "advertiserCode", index: "AdvertiserCode", width: 80, search: true, searchoptions: { sopt: ["eq"] }, sort: true, align: "center" }
                    , { name: "advertiser", index: "Advertiser", width: 125, search: true, searchoptions: { sopt: ["eq"] }, sort: true }
                    , { name: "advertiserCreatedOn", index: "AdvertiserCreatedOn", width: 100, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: "agencyCode", index: "AgencyCode", width: 80, search: true, searchoptions: { sopt: ["eq"] }, sort: true, align: "center" }
                    , { name: "agency", index: "Agency", width: 125, search: true, searchoptions: { sopt: ["eq"] }, sort: true }
                    , { name: "agencyCreatedOn", index: "AgencyCreatedOn", width: 100, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: "contractStart", index: "ContractStart", width: 100, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: "contractFinish", index: "ContractFinish", width: 100, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: "weeks", index: "Weeks", width: 55, search: false, sort: true, align: "right" }
                    , { name: "salesPerson", index: "SalesPerson", width: 125, search: true, searchoptions: { sopt: ["eq"] }, sort: true }
                    , { name: "contractAmount", index: "ContractAmount", width: 125, search: false, sort: true, align: "right", formatter: "currency", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$ "} }
                    , { name: "openPayments", index: "UnappliedPayments", width: 100, search: false, sort: true, align: "right", formatter: "currency", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$ "} }
                    , { name: "past12", index: "Past12SalesAndPayments", width: 100, search: false, sort: true, align: "right", formatter: "currency", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$ "} }
                    , { name: "customerType", index: "CustomerType", width: 100, search: true, searchoptions: { sopt: ["eq"] }, sort: true, align: "center" }
                    , { name: "threshold", index: "Threshold", hidden: true }
                    , { name: "creditAppReq", index: "CREDIT_APP_PREPAY", width: 95, search: true, searchoptions: { sopt: ["eq"] }, sort: true, editable: true, edittype: "select", editoptions: { value: ":;Y:Yes;N:No;CCA:CCA;DB:D&B;W:Waive"} }
                    , { name: "creditChecked", index: "CREDIT_CHECKED", width: 95, search: true, searchoptions: { sopt: ["eq"] }, sort: true, editable: true, edittype: "select", editoptions: { value: ":;A:Approved;W:Weak;C:Checking"} }
                    , { name: "notes", index: "NOTE_TEXT", search: true, sort: false, editable: true, edittype: "textarea", editoptions: { rows: "5", cols: "20"}}]
                , rowNum: 100
                , height: 600
                , width: "100%"
                , rowList: [25, 50, 100, 250, 500, 1000]
                , pager: "#pager1"
                , sortname: "ContractNumber"
                , sortorder: "ASC"
                , caption: "Credit Check Notes"
                , viewrecords: true
                , onSelectRow: function (id) {                    
                    if (id && id !== lastsel) {
                        $("#list1").restoreRow(lastsel);
                        lastsel = id;
                    }
                    var companyId = ($("#list1").getRowData(id)["company"] == "Titan US" ? 1 : 2);
                    var extraParams = { contractNumber: $("#list1").getRowData(id)["contractNumber"], companyId: companyId };
                    $("#list1").editRow(id, true, null, null, null, extraParams, afterEdit, null, null);
                }
                , editurl: "../services/CollectionsService.asmx/AddCreditCheckNote"
            });
            $("#list1").jqGrid("navGrid", "#pager1", { edit: false, add: false, del: false }).navButtonAdd("#pager1",
                { caption: "Toggle Columns", onClickButton: function () {
                    $("#list1").setColumns({ caption: "Show/Hide Columns", bSubmit: "Update", bCancel: "Cancel", colnameview: false, drag: false, ShrinkToFit: true });
                    return false;
                }
                });
        });
        function afterEdit(id, response) {
            if (response) {
                alert("Entry Saved");
            } else {
                alert("An error occurred while trying to save your Entry.");
            }
        }
    </script>
    <div style="width:95%">
        <div style="width:95%;margin:10px;z-index:-1"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
    <div id="popupFormEntry" style="visibility:hidden;display:none">
        <input type="hidden" id="popupContractNumber" value="" />
        <input type="hidden" id="popupCompanyId" value="" />
    </div>
</asp:Content>
