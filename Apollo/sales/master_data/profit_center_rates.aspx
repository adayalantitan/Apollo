<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="profit_center_rates.aspx.cs" Inherits="Apollo.sales_master_data_profit_center_rates" Title="Profit Center Rates | Titan 360" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
        var errorDivId = '<%=errorDiv.ClientID %>';
        function ValidateRateDetailEntry() {
            validationMessageStack = "";
            ClearErrorDiv(errorDivId);
            if (!ValidateRequiredFields()) {
                alert(validationMessageStack);
                return false;
            }
            SetErrorDiv(validationMessageStack, errorDivId);
        }
        function PopupModalWindow(profitCenterId, profitCenterName, marketName, isTransit, company, status) {
            $get('<%=profitCenterId.ClientID %>').value = profitCenterId;
            $get('<%=rateDateOriginal.ClientID %>').value = "";
            $get('<%=labelProfitCenterName.ClientID %>').innerHTML = profitCenterName;
            $get('<%=labelMarketName.ClientID %>').innerHTML = marketName;
            $get('<%=labelStatus.ClientID %>').innerHTML = status;
            $get('<%=labelCompany.ClientID %>').innerHTML = company;
            $get('<%=labelTransit.ClientID %>').innerHTML = isTransit;
            ClearRateDetailFields();
            ClearErrorDiv(errorDivId);
            PopulateDrawPaymentHistory(profitCenterId);
        }
        function PopulateDrawPaymentHistory(profitCenterId) {
            PageMethods.GetRateHistory(profitCenterId, PopulateDrawPaymentHistoryCallback);
        }
        function PopulateDrawPaymentHistoryCallback(historyTable) {
            $get('rateHistoryTableHeaders').style.display = ((historyTable != "") ? "block" : "none");
            $get('rateHistoryTable').innerHTML = historyTable;            
            var modalPopup = $find('rateAddEditModalPopupExtBehavior');
            modalPopup.show();
        }
        function EditRateDetailRecord(rateProfitCenterXrefId, rateDetailId, effectiveDate) {
            $get('<%=rateProfitCenterXrefId.ClientID %>').value = rateProfitCenterXrefId;
            $get('<%=dropDownRateName.ClientID %>').value = rateDetailId;
            $get('<%=textEffectiveFrom.ClientID %>').value = effectiveDate;
        }
        function CheckExistingEffectiveDate() {
            PageMethods.CheckExistingEffectiveDate(BuildDateCheckHash(), CheckExistingEffectiveDateCallback);
        }
        function HasEffectiveDateChanged() {
            var oldDate = new Date($get('<%=rateDateOriginal.ClientID%>').value);
            var newDate = new Date($get('<%=textEffectiveFrom.ClientID%>').value);
            return (oldDate.toString() != newDate.toString());
        }
        function CheckExistingEffectiveDateCallback(isExistingRate) {
            if (isExistingRate) {
                AddErrorMessage("A Rate with that Effective Date already exists.");
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);
                return;
            }
            PageMethods.AddProfitCenterRateXrefRecord(BuildHash(), AddRateDetailRecordCallback);
        }
        function AddRateDetailRecord() {
            validationMessageStack = "";
            ClearErrorDiv(errorDivId);
            if (!ValidateRequiredFields()) {
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);
                return;
            }
            if (HasEffectiveDateChanged()) {
                CheckExistingEffectiveDate();
                return;
            } else {
                PageMethods.AddProfitCenterRateXrefRecord(BuildHash(), AddRateDetailRecordCallback);
                return;
            }
        }
        function AddRateDetailRecordCallback(wasInsertSuccessful) {
            if (wasInsertSuccessful) {
                alert('Record added');
                ClearRateDetailFields();
                ClearErrorDiv(errorDivId);
                PopulateDrawPaymentHistory($get('<%=profitCenterId.ClientID %>').value);
                $('#list1').trigger('reloadGrid');                
                return;
            }
        }
        function BuildDateCheckHash() {
            var hash = new Object();
            hash["PROFITCENTERID"] = $get('<%=profitCenterId.ClientID %>').value;            
            hash["EFFECTIVEDATE"] = $get('<%=textEffectiveFrom.ClientID %>').value;
            return hash;          
        }
        function BuildHash() {
            var hash = new Object();
            hash["PROFITCENTERID"] = $get('<%=profitCenterId.ClientID %>').value;
            hash["RATEID"] = $get('<%=dropDownRateName.ClientID %>').value;
            hash["EFFECTIVEDATE"] = $get('<%=textEffectiveFrom.ClientID %>').value;
            return hash;          
        }
        function DeleteRateDetailRecord(rateProfitCenterXrefId) {
            if (confirm('Are you sure you wish to delete this record?')) {
                PageMethods.DeleteProfitCenterRateXrefRecord(rateProfitCenterXrefId, DeleteRateDetailRecordCallback);
            }
        }
        function DeleteRateDetailRecordCallback(wasDeleteSuccessful) {
            if (wasDeleteSuccessful) {
                alert('Record deleted');
                ClearRateDetailFields();
                ClearErrorDiv(errorDivId);
                PopulateDrawPaymentHistory($get('<%=profitCenterId.ClientID %>').value);
                $('#list1').trigger('reloadGrid');                
            }
        }
        function ValidateRequiredFields() {
            var enteredRateName = $get('<%=dropDownRateName.ClientID %>').value;
            var enteredEffectiveFrom = $get('<%=textEffectiveFrom.ClientID %>').value;            
            var isValid = true;

            if (enteredRateName == "") {
                AddErrorMessage("Rate Name is a required field.");
                isValid = false;
            }
            if (enteredEffectiveFrom == "") {
                AddErrorMessage("Effective From is a required field.");
                isValid = false;
            }
            if (!IsValidDate(enteredEffectiveFrom)) {
                AddErrorMessage("Effective From must be in MM/DD/YYYY format.");
                isValid = false;
            }
            return isValid;
        }
        function ClearRateDetailFields() {
            $get('<%=dropDownRateName.ClientID %>').value = "";
            $get('<%=textEffectiveFrom.ClientID %>').value = "";
            $get('<%=rateDateOriginal.ClientID %>').value = "";
        }
        function HideModalPopup() {
            var modalPopup = $find('rateAddEditModalPopupExtBehavior');
            modalPopup.hide();
            $get('<%=rateAddEdit.ClientID %>').style.display = "none";
        }
        function ShowRateDetail(rateId,rateName,effectiveDate) {
            var hash = new Object();
            hash["RATEID"] = rateId;
            $get('<%=labelRatePopupTitle.ClientID %>').innerHTML = rateName + " (Effective " + effectiveDate + ")";
            PageMethods.LoadRateDetails(hash, ShowRateDetailCallback);
        }        
        function ShowRateDetailCallback(historyItem) {
            $get('<%=detailsArea.ClientID %>').innerHTML = historyItem;
            $get('<%=detailsArea.ClientID %>').style.display = "block";
            $get('<%=modalPlaceHolder.ClientID %>').click();
        }
        function CloseDetailsPopup() {
            $get('<%=detailsArea.ClientID %>').style.display = "none";
        }
        $(document).ready(function() {
            $("#list1").jqGrid({
                url: '../../services/IOService.asmx/GetProfitCenterRateGrid'
                , datatype: "xml"
                , colNames: ['Profit Center Id', 'Profit Center', 'Market', 'Rate Name', 'Is Transit', 'Company', 'Active', 'Company ID', 'Market ID']
                , colModel: [{ name: 'profitCenterId', index: 'PROFIT_CENTER_ID', hidden: true }
                    , { name: 'profitCenterName', index: 'PROFIT_CENTER_NAME', width: 125, search: true, sort: true }                    
                    , { name: 'marketDescription', index: 'MARKET_DESCRIPTION', width: 125, search: true }
                    , { name: 'rateDetailName', index: 'RATE_DETAIL_NAME', width: 125, search: true }
                    , { name: 'isTransit', index: 'IS_TRANSIT', hidden: true }
                    , { name: 'company', index: 'COMPANY', width: 100, search: true }
                    , { name: 'active', index: 'ACTIVE', width: 50, search: true }
                    , { name: 'companyId', index: 'COMPANY_ID', hidden: true }
                    , { name: 'marketId', index: 'MARKET_ID', hidden: true }]
                , rowNum: 25
                , height: 500
                , width: 800
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'PROFIT_CENTER_NAME'
                , sortorder: 'ASC'
                , viewrecords: true
                , toolbar: [true, 'top']
                , caption: "Profit Center Rate Maintenance"
                , onSelectRow: function(id) {
                    if (id) {
                        var profitCenterId = $('#list1').getRowData(id)['profitCenterId'];
                        var profitCenterName = $('#list1').getRowData(id)['profitCenterName'];
                        var marketName = $('#list1').getRowData(id)['marketDescription'];
                        var isTransit = $('#list1').getRowData(id)['isTransit'];
                        var company = $('#list1').getRowData(id)['company'];
                        var status = $('#list1').getRowData(id)['active'];                        
                        PopupModalWindow(profitCenterId, profitCenterName, marketName, isTransit, company, status);
                    }
                }
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false });
            $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
        });
    </script>
    <div style="margin:0 auto;">
        <div style="width:95%;margin:50px;"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
    <asp:Button ID="showModalPopup" runat="server" style="display:none" />
    <asp:Button ID="cancelDummy1" runat="server" style="display:none" />
    <ajax:ModalPopupExtender ID="rateAddEditModalPopupExt" runat="server" PopupDragHandleControlID="rateAddEditPopupTitle" CancelControlID="cancelDummy1" BehaviorID="rateAddEditModalPopupExtBehavior" 
        PopupControlID="rateAddEdit" TargetControlID="showModalPopup" BackgroundCssClass="popupbg" DropShadow="true" />    
        <div id="rateAddEdit" runat="server" style="width:575px;border:1px solid #333333;background-color:White;padding:10px;display:none">
        <asp:UpdatePanel ID="rateAddEditUpdPnl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>        
                <div>        
                    <asp:HiddenField ID="profitCenterId" runat="server" />
                    <asp:HiddenField ID="rateDateOriginal" runat="server" />
                    <asp:HiddenField ID="rateProfitCenterXrefId" runat="server" />
                    <div class="breadcrumb_current" id="rateAddEditPopupTitle">Master Data - Edit Profit Center Rate</div>                    
                    <img ID="saveRate" runat="server" src="~/Images/but_save.gif" 
                        style="cursor:pointer;margin:5px 3px;" onclick="AddRateDetailRecord();" />
                    <img ID="cancel" runat="server" src="~/Images/but_cancel.gif" 
                        style="cursor:pointer;margin:5px 3px;" onclick="HideModalPopup();" />
                    <div id="errorDiv" runat="server" class="errorDisplay" style="margin:4px"></div>
                    <div style="width:100%;border:1px solid #06347a;padding:2px;">
                        <div class="spanColumn">
                            <div style="width:50%;float:left;">
                                Profit Center Information
                            </div>
                            <div style="width:50%;float:left;">                         
                            </div>
                            <div style="clear:both"></div>
                        </div>
                        <div style="width:50%;float:left">
                            <ul class="formFields">
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn">
                                            Profit Center:
                                        </div>
                                        <div class="formFieldsRightColumn">
                                            <asp:Label ID="labelProfitCenterName" runat="server" />
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn">
                                            Market:
                                        </div>
                                        <div class="formFieldsRightColumn">      
                                            <asp:Label ID="labelMarketName" runat="server" />                                  
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn">
                                            Status:
                                        </div>
                                        <div class="formFieldsRightColumn">  
                                            <asp:Label ID="labelStatus" runat="server" />
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>                                
                            </ul>
                        </div>
                        <div style="width:50%;float:left">
                            <ul class="formFields">
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn">
                                            Company:
                                        </div>
                                        <div class="formFieldsRightColumn">
                                            <asp:Label ID="labelCompany" runat="server" />
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn">
                                            Transit:
                                        </div>
                                        <div class="formFieldsRightColumn">      
                                            <asp:Label ID="labelTransit" runat="server" />                                  
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>                               
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn" style="background-color:#ffffff !important;">                            
                                            <br />
                                        </div>
                                        <div class="formFieldsRightColumn">
                                            <br />
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div style="clear:both"></div>
                        <div class="spanColumn">
                            <div style="width:50%;float:left;">
                                Rates
                            </div>
                            <div style="width:50%;float:left;">                         
                            </div>
                            <div style="clear:both"></div>
                        </div>
                        <div style="width:50%;float:left">
                            <ul class="formFields">
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn">
                                            <span class="requiredIndicator">*</span>&nbsp;Rate:
                                        </div>
                                        <div class="formFieldsRightColumn">
                                            <asp:DropDownList ID="dropDownRateName" runat="server" DataSourceID="rateNameDataSource"
                                                DataTextField="RATE_NAME" DataValueField="ID" AppendDataBoundItems="true">
                                                <asp:ListItem Text="" Value="" Selected="True" />
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="rateNameDataSource" runat="server" DataSourceMode="DataReader" 
                                                SelectCommandType="StoredProcedure" SelectCommand="MasterData_GetRateNames" />
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>                             
                            </ul>
                        </div>
                        <div style="width:50%;float:left">
                            <ul class="formFields">
                                <li>
                                    <div>
                                        <div class="formFieldsLeftColumn">
                                            <span class="requiredIndicator">*</span>&nbsp;Effective From:
                                        </div>
                                        <div class="formFieldsRightColumn">
                                            <asp:TextBox ID="textEffectiveFrom" runat="server" />
                                            <img src="/Images/calendar.gif" id="effectiveFromCal" runat="server" alt="Click for Calendar"  class="calendarPopup" />
                                            <ajax:CalendarExtender ID="textEffectiveFrom_calExt" runat="server" 
                                                Enabled="True" TargetControlID="textEffectiveFrom" PopupButtonID="effectiveFromCal">
                                            </ajax:CalendarExtender>
                                        </div>
                                        <div style="clear:both"></div>
                                    </div>
                                </li>                                
                            </ul>
                        </div>
                        <div style="width:100%">
                            <ul class="formFields">
                                <li>
                                    <span class="requiredIndicator">*&nbsp;Required Field</span>
                                </li>
                                <li>
                                    <br />
                                </li>
                                <li>                                    
                                    <div style="background-color:#ededfe;padding:3px 0;">
                                        <b>Rates History</b>
                                    </div>                                    
                                </li>
                                <li>
                                    <div id="rateHistoryTableHeaders" style="display:none">
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" class="buttonHead_Center" width="10%">&nbsp;</td>                                                
                                                <td valign="top" class="buttonHead_Center" width="20%">Effective From</td>
                                                <td valign="top" class="buttonHead_Center" width="50%">Rate Name</td>
                                                <td valign="top" class="buttonHead_Center" width="20%">Rate %</td>                                                
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="rateHistoryTable" class="historyTable"></div>
                                    <input type="button" ID="modalPlaceHolder" runat="server" style="display:none" />
                                    <ajax:ModalPopupExtender ID="ratePopupExtender" runat="server" CancelControlID="closeDetails" BehaviorID="ratePopupExtenderBehavior" 
                                        TargetControlID="modalPlaceHolder" PopupControlID="ratePanel" BackgroundCssClass="popupbg" DropShadow="true" />
                                    <div ID="ratePanel" runat="server" style="width:560px;border:1px solid #333333;background-color:White;padding:10px;display:none;">
                                        <div>
                                            <div style="float:left">
                                                <img src="/Images/titan_help_logo.gif" alt="Titan" />
                                            </div>
                                            <div style="clear:both">
                                            </div>        
                                            <div>        
                                                <span class="breadcrumb_current" style="width:100%;text-align:center">
                                                    <asp:Label runat="server" ID="labelRatePopupTitle" />
                                                </span>
                                                <br />
                                                <div id="detailsArea" runat="server"></div>        
                                                <br />
                                            </div>
                                        </div>                                    
                                        <div style="text-align:right">
                                            <input type="button" id="closeDetails" value="Close" style="font-size:10px" runat="server" onclick="CloseDetailsPopup()" />
                                        </div>
                                    </div>
                                    <br /><br />
                                </li>
                            </ul>
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

