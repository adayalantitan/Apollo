<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="rates.aspx.cs" Inherits="Apollo.sales_master_data_rates" Title="Rates | Titan 360" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">
        var errorDivId = '<%=errorDiv.ClientID %>';        
        var maximumRatePercentage = parseInt("<%=MaximumRatePercentage() %>",10);        
        function TogglePanelSelection() {
            //var show = $get('< % = checkIsPackage.ClientID %>').checked && !IsAdding();            
            //$get('< % = modifyPanelTypes.ClientID %>').style.visibility = (show) ? "visible" : "hidden";
            
        }        
        function ValidateRateDetailEntry() {
            validationMessageStack = "";
            ClearErrorDiv(errorDivId);
            if (!ValidateRequiredFields()) {
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);
                return false;
            }
            if (!ValidateRates()) {
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);
                return false;
            }
            if (IsAdding()) {
                //Verify that this rate name is unique and perform an Add
                //Check for existing Rate Detail and Effective Date                
                IsExistingRateDetailName();                
                return;
            } else {//Updating
                //Modifying an existing Rate, make sure the date and name are unique                
                CheckBoth();
                return;
            }
        }
        function IsAdding() {
            return ($get('<%=isAdding.ClientID %>').value == "true");
        }
        function HasRateNameChanged() {
            var oldRateName = $get('<%=rateNameOriginal.ClientID%>').value.toLowerCase();
            var newRateName = $get('<%=textRateName.ClientID %>').value.toLowerCase();
            return (oldRateName != newRateName);
        }
        function HasEffectiveDateChanged() {
            var oldDate = new Date($get('<%=rateDateOriginal.ClientID%>').value);
            var newDate = new Date($get('<%=textEffectiveFrom.ClientID%>').value);
            return (oldDate.toString() != newDate.toString());                
        }
        function ValidateRequiredFields() {
            var enteredRateName = $get('<%=textRateName.ClientID %>').value;
            var enteredEffectiveFrom = $get('<%=textEffectiveFrom.ClientID %>').value;
            var enteredLocalRenew = $get('<%=textLocalRenew.ClientID %>').value;
            var enteredLocalNew = $get('<%=textLocalNew.ClientID %>').value;
            var enteredNationalRenew = $get('<%=textNationalRenew.ClientID %>').value;
            var enteredNationalNew = $get('<%=textNationalNew.ClientID %>').value;
            var isValid = true;

            if (enteredRateName == "") {
                AddErrorMessage("Rate Name is a required field.");
                isValid = false;
            }
            if (enteredEffectiveFrom == "") {
                AddErrorMessage("Effective From is a required field.");
                isValid = false;
            }
            if (enteredLocalRenew == "") {
                AddErrorMessage("Local Renew % is a required field.");
                isValid = false;
            }
            if (enteredLocalNew == "") {
                AddErrorMessage("Local New % is a required field.");
                isValid = false;
            }
            if (enteredNationalRenew == "") {
                AddErrorMessage("National Renew % is a required field.");
                isValid = false;
            }
            if (enteredNationalNew == "") {
                AddErrorMessage("National New % is a required field.");
                isValid = false;
            }
            return isValid;
        }
        function ValidateRates() {
            var enteredLocalRenew = $get('<%=textLocalRenew.ClientID %>').value;
            var enteredLocalNew = $get('<%=textLocalNew.ClientID %>').value;
            var enteredNationalRenew = $get('<%=textNationalRenew.ClientID %>').value;
            var enteredNationalNew = $get('<%=textNationalNew.ClientID %>').value;
            var isValid = true;

            if (!IsValidDecimal(enteredLocalRenew)) {
                AddErrorMessage("Local Renew % must be a valid percentage.");
                isValid = false;
            }
            if (enteredLocalRenew > maximumRatePercentage) {
                AddErrorMessage("Local Renew % exceeds allowed maximum.");
                isValid = false;
            }
            if (!IsValidDecimal(enteredLocalNew)) {
                AddErrorMessage("Local New % must be a valid percentage.");
                isValid = false;
            }
            if (enteredLocalNew > maximumRatePercentage) {
                AddErrorMessage("Local New % exceeds allowed maximum.");
                isValid = false;
            }
            if (!IsValidDecimal(enteredNationalRenew)) {
                AddErrorMessage("National Renew % must be a valid percentage.");
                isValid = false;
            }
            if (enteredNationalRenew > maximumRatePercentage) {
                AddErrorMessage("National Renew % exceeds allowed maximum.");
                isValid = false;
            }
            if (!IsValidDecimal(enteredNationalNew)) {
                AddErrorMessage("National New % must be a valid percentage.");
                isValid = false;
            }
            if (enteredNationalNew > maximumRatePercentage) {
                AddErrorMessage("National New % exceeds allowed maximum.");
                isValid = false;
            }
            return isValid;
        }
        function CheckBoth() {
            if (HasRateNameChanged()) {
                var enteredRateName = document.getElementById('<%=textRateName.ClientID %>').value;
                PageMethods.CheckExistingRateDetailName(enteredRateName, CheckBothCallbackFirstStep);
            } else {
                CheckBothCallbackFirstStep(false);
            }
        }
        function CheckBothCallbackFirstStep(isExistingRate) {
            if (isExistingRate) {
                AddErrorMessage("A Rate with that Name already exists.");
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);
                return;
            }
            if (HasEffectiveDateChanged()) {
                var rateHash = new Object();
                rateHash["RATEID"] = $get('<%=rateId.ClientID %>').value;
                rateHash["RATEEFFECTIVEDATE"] = $get('<%=textEffectiveFrom.ClientID %>').value;           
                PageMethods.CheckExistingRateDetailEffectiveDate(rateHash, CheckBothCallbackSecondStep);
            } else {
                UpdateRateDetail();
            }
        }
        function CheckBothCallbackSecondStep(isExistingDate) {
            if (isExistingDate) {
                AddErrorMessage("A Rate with that Effective Date already exists.");
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);          
                return;
            } else {
                ClearErrorDiv(errorDivId);
                AddRateDetail();
            } 
        }
        function IsExistingRateDetailName() {
            var enteredRateName = document.getElementById('<%=textRateName.ClientID %>').value;
            PageMethods.CheckExistingRateDetailName(enteredRateName, IsExistingRateDetailNameCallback);
        }
        function IsExistingRateDetailNameCallback(isExistingRate) {
            if (isExistingRate) {
                AddErrorMessage("A Rate with that Name already exists.");
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);
                return;
            } else {
                ClearErrorDiv(errorDivId);
                AddRate();
            }           
        }
        function IsExistingEffectiveDate() {
            PageMethods.CheckExistingRateDetailEffectiveDate(BuildRateHash(), IsExistingEffectiveDateCallback);
        }
        function IsExistingEffectiveDateCallback(isExistingEffectiveDate) {
            if (isExistingEffectiveDate) {
                AddErrorMessage("A Rate with that Effective Date already exists.");
                alert(validationMessageStack);
                SetErrorDiv(validationMessageStack, errorDivId);
                return;
            } else {
                ClearErrorDiv(errorDivId);
                UpdateRateDetail();
            }
        }
        function AddRate() {
            var rateHash = new Object();
            rateHash["RATENAME"] = $get('<%=textRateName.ClientID%>').value;
            rateHash["RATELOCALRENEW"] = $get('<%=textLocalRenew.ClientID %>').value;
            rateHash["RATELOCALNEW"] = $get('<%=textLocalNew.ClientID %>').value;
            rateHash["RATENATIONALRENEW"] = $get('<%=textNationalRenew.ClientID %>').value;
            rateHash["RATENATIONALNEW"] = $get('<%=textNationalNew.ClientID %>').value;
            rateHash["RATEEFFECTIVEDATE"] = $get('<%=textEffectiveFrom.ClientID %>').value;            
            //rateHash["ISPACKAGE"] = ($get('< % = checkIsPackage.ClientID %>').checked) ? 1 : 0;
            rateHash["ACTIVE"] = $get('<%=dropDownStatus.ClientID %>').value;
            PageMethods.AddRate(rateHash, AddRateDetailCallback);
        }
        function AddRateDetail() {
            var rateHash = new Object();
            rateHash["RATEID"] = $get('<%=rateId.ClientID %>').value;            
            rateHash["RATELOCALRENEW"] = $get('<%=textLocalRenew.ClientID %>').value;
            rateHash["RATELOCALNEW"] = $get('<%=textLocalNew.ClientID %>').value;
            rateHash["RATENATIONALRENEW"] = $get('<%=textNationalRenew.ClientID %>').value;
            rateHash["RATENATIONALNEW"] = $get('<%=textNationalNew.ClientID %>').value;
            rateHash["RATEEFFECTIVEDATE"] = $get('<%=textEffectiveFrom.ClientID %>').value;
            //rateHash["ISPACKAGE"] = ($get('< % = checkIsPackage.ClientID %>').checked) ? 1 : 0;
            rateHash["ACTIVE"] = $get('<%=dropDownStatus.ClientID %>').value;
            PageMethods.AddRateDetail(rateHash, AddRateDetailCallback);
        }
        function AddRateDetailCallback(wasInsertSuccessful) {
            if (wasInsertSuccessful) {
                $('#list1').trigger('reloadGrid');   
                setTimeout("alert('Rate added.');HideRatePopup();", 1500);                                                 
            }            
        }
        function UpdateRateDetail() {
            var rateHash = new Object();
            rateHash["RATEID"] = $get('<%=rateId.ClientID %>').value;
            rateHash["RATENAME"] = $get('<%=textRateName.ClientID%>').value;
            rateHash["RATEDETAILID"] = $get('<%=rateDetailId.ClientID%>').value;
            rateHash["RATELOCALRENEW"] = $get('<%=textLocalRenew.ClientID %>').value;
            rateHash["RATELOCALNEW"] = $get('<%=textLocalNew.ClientID %>').value;
            rateHash["RATENATIONALRENEW"] = $get('<%=textNationalRenew.ClientID %>').value;
            rateHash["RATENATIONALNEW"] = $get('<%=textNationalNew.ClientID %>').value;            
            //rateHash["ISPACKAGE"] = ($get('< % = checkIsPackage.ClientID %>').checked) ? 1 : 0;
            rateHash["ACTIVE"] = $get('<%=dropDownStatus.ClientID %>').value;
            PageMethods.UpdateRate(rateHash, UpdateRateDetailCallback);
        }
        function UpdateRateDetailCallback(wasUpdateSuccessful) {
            if (wasUpdateSuccessful) {
                $('#list1').trigger('reloadGrid');   
                setTimeout("alert('Rate updated.');HideRatePopup();", 1500);                
            }
        }
        function PopupModalWindow(rateDetailId, rateId) {
            $get('<%=rateDetailId.ClientID%>').value = rateDetailId;
            $get('<%=rateId.ClientID%>').value = rateId;
            PageMethods.LoadDetailRecord(rateDetailId, LoadPopupTitle);
        }
        function ClearRateDetailFields() {
            $get('<%=textRateName.ClientID %>').value = "";
            $get('<%=rateNameOriginal.ClientID %>').value = "";
            $get('<%=textEffectiveFrom.ClientID %>').value = "";
            $get('<%=rateDateOriginal.ClientID %>').value = "";
            $get('<%=textLocalRenew.ClientID %>').value = "";
            $get('<%=textLocalNew.ClientID %>').value = "";
            $get('<%=textNationalRenew.ClientID %>').value = "";
            $get('<%=textNationalNew.ClientID %>').value = "";
            //$get('< % = checkIsPackage.ClientID %>').checked = false;
            TogglePanelSelection();
        }
        function LoadPopupTitle(rateHashTable) {            
            $get('<%=textRateName.ClientID %>').value = rateHashTable["RATE_DETAIL_NAME"];
            $get('<%=rateNameOriginal.ClientID %>').value = rateHashTable["RATE_DETAIL_NAME"];
            $get('<%=textEffectiveFrom.ClientID %>').value = rateHashTable["RATE_EFFECTIVE_DATE"];
            $get('<%=rateDateOriginal.ClientID %>').value = rateHashTable["RATE_EFFECTIVE_DATE"];
            $get('<%=textLocalRenew.ClientID %>').value = rateHashTable["RATE_LOCAL_RENEW"];
            $get('<%=textLocalNew.ClientID %>').value = rateHashTable["RATE_LOCAL_NEW"];
            $get('<%=textNationalRenew.ClientID %>').value = rateHashTable["RATE_NATIONAL_RENEW"];
            $get('<%=textNationalNew.ClientID %>').value = rateHashTable["RATE_NATIONAL_NEW"];
            $get('<%=dropDownStatus.ClientID %>').value = rateHashTable["ACTIVE"];
            //$get('< % = checkIsPackage.ClientID %>').checked = (rateHashTable["IS_PACKAGE"] == 1);
            TogglePanelSelection();
            SetPopupTitle(false);
        }
        function SetPopupTitle(isAdding) {
            var title = document.getElementById('rateAddEditPopupTitle');
            title.innerHTML = (isAdding) ? "Master Data - Add Rate" : "Master Data - Edit Rate";
            if (isAdding) {
                $get('<%=isAdding.ClientID %>').value = "true";
                ClearRateDetailFields();
            } else {
                $get('<%=isAdding.ClientID %>').value = "false";
                var modalPopup = $find('rateAddEditModalPopupExtBehavior');
                modalPopup.show();                
            }
        }
        function HideRatePopup() {
            var modalPopup = $find('rateAddEditModalPopupExtBehavior');
            modalPopup.hide();
            $get('<%=rateAddEdit.ClientID %>').style.display = "none";
        }
        $(document).ready(function() {
            $("#list1").jqGrid({
                url: '../../services/IOService.asmx/GetRateGrid'
                , datatype: "xml"
                , colNames: ['ID', 'Rate ID', 'Rate Name', 'Effective Date', 'Local Renew', 'Local New', 'National Renew', 'National New', 'Active', 'Is Package']
                , colModel: [{ name: 'id', index: 'ID', hidden: true }
                    , { name: 'rateId', index: 'RATE_ID', hidden: true }
                    , { name: 'rateDetailName', index: 'RATE_DETAIL_NAME', width: 125, search: true }
                    , { name: 'rateEffectiveDate', index: 'RATE_EFFECTIVE_DATE', width: 50, search: true, align:'right' }
                    , { name: 'rateLocalRenew', index: 'RATE_LOCAL_RENEW', width: 50, search: false, align: 'right' }
                    , { name: 'rateLocalNew', index: 'RATE_LOCAL_NEW', width: 50, search: false, align: 'right' }
                    , { name: 'rateNationalRenew', index: 'RATE_NATIONAL_RENEW', width: 50, search: false, align: 'right' }
                    , { name: 'rateNationalNew', index: 'RATE_NATIONAL_NEW', width: 50, search: false, align: 'right' }
                    , { name: 'active', index: 'ACTIVE', width: 50, search: true }
                    , { name: 'isPackage', index: 'IS_PACKAGE', hidden: true}]
                , rowNum: 25
                , height: 500
                , width: 800
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'RATE_DETAIL_NAME'
                , sortorder: 'ASC'
                , viewrecords: true
                , toolbar: [true, 'top']
                , caption: "Rate Maintenance"
                , onSelectRow: function(id) {
                    if (id) {                        
                        var rateDetailId = $('#list1').getRowData(id)['id'];
                        var rateId = $('#list1').getRowData(id)['rateId'];
                        PopupModalWindow(rateDetailId, rateId);
                    }
                }
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false });
            $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
        });
    </script>
    <asp:ImageButton ID="addNewRate" runat="server" 
        ImageUrl="~/Images/but_addrate.gif" style="cursor:pointer;margin-left:50px;" 
        OnClientClick="SetPopupTitle(true);" />
    <br />
    <div style="margin:0 auto;">
        <div style="width:95%;margin:50px;"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>    
    <asp:Button ID="cancelDummy1" runat="server" style="display:none" />
    <ajax:ModalPopupExtender ID="rateAddEditModalPopupExt" runat="server" PopupDragHandleControlID="rateAddEditPopupTitle" CancelControlID="cancelDummy1" BehaviorID="rateAddEditModalPopupExtBehavior" 
        PopupControlID="rateAddEdit" TargetControlID="addNewRate" BackgroundCssClass="popupbg" DropShadow="true" />    
    <div id="rateAddEdit" runat="server" style="width:575px;border:1px solid #333333;background-color:White;padding:10px;display:none">
        <asp:UpdatePanel ID="rateAddEditUpdPnl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>        
                <div>                              
                    <asp:HiddenField ID="rateDateOriginal" runat="server" /> 
                    <asp:HiddenField ID="rateNameOriginal" runat="server" />
                    <asp:HiddenField ID="rateDetailId" runat="server" />
                    <asp:HiddenField ID="rateId" runat="server" />
                    <asp:HiddenField ID="isAdding" runat="server" />
                    <div class="breadcrumb_current" id="rateAddEditPopupTitle">Master Data - Edit Rate</div>                    
                    <img ID="saveRate" runat="server" alt="Save changes" src="~/Images/but_save.gif" 
                        style="cursor:pointer;margin:5px 3px;" onclick="ValidateRateDetailEntry()" />
                    <img ID="cancel" runat="server" alt="Cancel" onclick="HideRatePopup();" src="~/Images/but_cancel.gif" style="cursor:pointer;margin:5px 3px;" />
                    <div id="errorDiv" runat="server" class="errorDisplay" style="margin:4px"></div>
                    <div style="width:100%;border:1px solid #06347a;">
                        <div>
                            <div style="width:50%;float:left">
                                <ul class="formFields">
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn">
                                                <span class="requiredIndicator">*</span>&nbsp;Rate Name:
                                            </div>
                                            <div class="formFieldsRightColumn">
                                                <asp:TextBox ID="textRateName" runat="server" Width="170px" />
                                            </div>
                                            <div style="clear:both;margin:0;"></div>
                                        </div>
                                    </li>
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
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn">
                                                &nbsp;&nbsp;Status:
                                            </div>
                                            <div class="formFieldsRightColumn">
                                                <asp:DropDownList ID="dropDownStatus" runat="server" Width="100px">                                            
                                                    <asp:ListItem Text="Active" Value="A" Selected="True" />
                                                    <asp:ListItem Text="Inactive" Value="I" />
                                                </asp:DropDownList>
                                            </div>
                                            <div style="clear:both"></div>
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn">
                                                <span class="requiredIndicator">*</span>&nbsp;Local Renew:
                                            </div>
                                            <div class="formFieldsRightColumn">
                                                <asp:TextBox ID="textLocalRenew" runat="server" CssClass="rateInputBox" />
                                            </div>
                                            <div style="clear:both"></div>
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn">
                                                <span class="requiredIndicator">*</span>&nbsp;Local New:
                                            </div>
                                            <div class="formFieldsRightColumn">
                                                <asp:TextBox ID="textLocalNew" runat="server" CssClass="rateInputBox" />
                                            </div>
                                            <div style="clear:both"></div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div>
                            <div style="width:49%;float:left;">
                                <ul class="formFields">
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn" style="background-color:#ffffff !important;">
                                                &nbsp;                                                
                                            </div>
                                            <div class="formFieldsRightColumn" style="padding-top:3px">
                                                &nbsp;                                                
                                            </div>
                                            <div style="clear:both"></div>
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn" style="background-color:#ffffff !important;">
                                                &nbsp;                                                
                                            </div>
                                            <div class="formFieldsRightColumn" style="padding-top:3px">
                                                &nbsp;                                                
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
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn">
                                                <span class="requiredIndicator">*</span>&nbsp;National Renew:
                                            </div>
                                            <div class="formFieldsRightColumn">
                                                <asp:TextBox ID="textNationalRenew" runat="server" CssClass="rateInputBox" />
                                            </div>
                                            <div style="clear:both"></div>
                                        </div>
                                    </li>
                                    <li>
                                        <div>
                                            <div class="formFieldsLeftColumn">
                                                <span class="requiredIndicator">*</span>&nbsp;National New:
                                            </div>
                                            <div class="formFieldsRightColumn">
                                                <asp:TextBox ID="textNationalNew" runat="server" CssClass="rateInputBox" />
                                            </div>
                                            <div style="clear:both"></div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div>
                            <div style="clear:both"></div>
                        </div>
                        <div style="width:100%">
                            <ul class="formFields">
                                <li>
                                    <span class="requiredIndicator">*&nbsp;Required Field</span>
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

