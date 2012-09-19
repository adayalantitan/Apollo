<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contractInfo.ascx.cs" Inherits="Apollo.UserControls_contractInfo" EnableViewState="true" %>

<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>
    <style type="text/css">
        .button {cursor:pointer;}
    </style>
<script type="text/javascript" language="javascript">
    var errorStack;    
    function ValidateEntry() {
        errorStack = "";
        ClearErrorDiv('<%=errorDiv.ClientID%>');
        var isRevValid = ValidateRevSplits();
        var isComValid = ValidateComSplits();
        var isFixedValid = ValidateFixedPoints();
        var isApprovedByValid = ValidateApprovedByFields();
        var isEffectiveDateValid = ValidateEffectiveDate();        
        if (!(isRevValid && isComValid && isFixedValid && isApprovedByValid && isEffectiveDateValid)) {
            alert(errorStack);
            SetErrorDiv(errorStack,'<%=errorDiv.ClientID%>');
            return;
        }        
        if ($get('<%=isUpdating.ClientID %>').value==0){
            CheckExistingEffectiveDate();
        } else {
            SaveContractDetail();
        }
    }
    function CheckExistingEffectiveDate() {
        //CheckExistingEffectiveDate
        var hash = new Object();
        hash["CONTRACTNUMBER"] = $get('<%=labelContractNumber.ClientID %>').innerHTML;
        hash["COMPANYID"] = $get('<%=companyId.ClientID %>').value;
        hash["EFFECTIVEDATE"] = $get('<%=textSplitEffFrom.ClientID %>').value;
        PageMethods.CheckExistingEffectiveDate(hash, CheckExistingEffectiveDateCallback, ErrorCallback);
    }
    function CheckExistingEffectiveDateCallback(isExistingDate) {
        if (!isExistingDate) {
            SaveContractDetail();
        } else {
            errorStack += "Effective date already exists for this contract.";
            alert(errorStack);
            SetErrorDiv(errorStack,'<%=errorDiv.ClientID%>');
            return;
        }
    }
    function ValidateRevSplits() {
        var ae1Id = $get('<%=ae1.Id.ClientID%>');
        var ae2Id = $get('<%=ae2.Id.ClientID%>');
        var ae3Id = $get('<%=ae3.Id.ClientID%>');
        var ae1Rev = $get('<%=textAe1RevSplit.ClientID%>').value;
        var ae2Rev = $get('<%=textAe2RevSplit.ClientID%>').value;
        var ae3Rev = $get('<%=textAe3RevSplit.ClientID%>').value;
        var numberOfAes = 1 + ((ae2Id != '') ? 1 : 0) + ((ae3Id != '') ? 1 : 0);
        var maxPercentage = Math.round(parseFloat((100/numberOfAes)*3));
        ae1Rev = (ae1Rev == "") ? 0 : parseFloat(ae1Rev);
        ae2Rev = (ae2Rev == "") ? 0 : parseFloat(ae2Rev);
        ae3Rev = (ae3Rev == "") ? 0 : parseFloat(ae3Rev);
        var isValid = true;

        if ((ae1Rev < 0) || (ae2Rev < 0) || (ae3Rev < 0)) {
            errorStack += '\nError: Revenue Split Percentages cannot be negative.';
            isValid = false;
        }

        if (Math.round((ae1Rev + ae2Rev + ae3Rev)) > maxPercentage) {
            errorStack += '\nError: Total Revenue Split Percentage exceeds limit.';
            isValid = false;
        }
        
        if (Math.round((ae1Rev + ae2Rev + ae3Rev)) < maxPercentage) {
            errorStack += '\nError: All Revenue must be Allocated to the selected AE(s).';
            isValid = false;
        }

        if ((ae1Id.value == "") && (ae1Rev != 0)) {
            errorStack += '\nError: Please select Sales Rep #1 or remove split percentage for Sales Rep #1';
            isValid = false;
        }

        if ((ae2Id.value == "") && (ae2Rev != 0)) {
            errorStack += '\nError: Please select Sales Rep #2 or remove split percentage for Sales Rep #2';
            isValid = false;
        }

        if ((ae3Id.value == "") && (ae3Rev != 0)) {
            errorStack += '\nError: Please select Sales Rep #3 or remove split percentage for Sales Rep #3';
            isValid = false;
        }

        return isValid;
    }
    function ValidateComSplits() {
        var ae1Id = $get('<%=ae1.Id.ClientID%>');
        var ae2Id = $get('<%=ae2.Id.ClientID%>');
        var ae3Id = $get('<%=ae3.Id.ClientID%>');
        var ae1Com = $get('<%=textAe1ComSplit.ClientID%>').value;
        var ae2Com = $get('<%=textAe2ComSplit.ClientID%>').value;
        var ae3Com = $get('<%=textAe3ComSplit.ClientID%>').value;
        var numberOfAes = 1 + ((ae2Id != '') ? 1 : 0) + ((ae3Id != '') ? 1 : 0);
        var maxPercentage = Math.round(parseFloat((100/numberOfAes)*3));
        ae1Com = (ae1Com == "") ? 0 : parseFloat(ae1Com);
        ae2Com = (ae2Com == "") ? 0 : parseFloat(ae2Com);
        ae3Com = (ae3Com == "") ? 0 : parseFloat(ae3Com);
        var isValid = true;

        if ((ae1Com < 0) || (ae2Com < 0) || (ae3Com < 0)) {
            errorStack += '\nError: Commission Split Percentages cannot be negative.';
            isValid = false;
        }

        if (Math.round((ae1Com + ae2Com + ae3Com)) > maxPercentage) {
            errorStack += '\nError: Total Commission Split Percentage exceeds limit.';
            isValid = false;
        }

        if ((ae1Id.value == "") && (ae1Com != 0)) {
            errorStack += '\nError: Please select Sales Rep #1 or remove split percentage for Sales Rep #1';
            isValid = false;
        }

        if ((ae2Id.value == "") && (ae2Com != 0)) {
            errorStack += '\nError: Please select Sales Rep #2 or remove split percentage for Sales Rep #2';
            isValid = false;
        }

        if ((ae3Id.value == "") && (ae3Com != 0)) {
            errorStack += '\nError: Please select Sales Rep #3 or remove split percentage for Sales Rep #3';
            isValid = false;
        }

        return isValid;
    }
    function ValidateFixedPoints() {
        var ae1Id = $get('<%=ae1.Id.ClientID%>');
        var ae2Id = $get('<%=ae2.Id.ClientID%>');
        var ae3Id = $get('<%=ae3.Id.ClientID%>');
        var ae1Fixed = $get('<%=textAe1FixedSplit.ClientID%>').value;
        var ae2Fixed = $get('<%=textAe2FixedSplit.ClientID%>').value;
        var ae3Fixed = $get('<%=textAe3FixedSplit.ClientID%>').value;
        ae1Fixed = (ae1Fixed == "") ? 0 : parseFloat(ae1Fixed);
        ae2Fixed = (ae2Fixed == "") ? 0 : parseFloat(ae2Fixed);
        ae3Fixed = (ae3Fixed == "") ? 0 : parseFloat(ae3Fixed);
        var fixedPointMax = '<%=GetFixedPointsMax()%>';
        var usePoints = $get('<%=radioFixedPointsYes.ClientID%>');
        var isValid = true;

        if (!usePoints.checked) { return true; }

        if ((ae1Fixed < 0) || (ae2Fixed < 0) || (ae3Fixed < 0)) {
            errorStack += '\nError: Fixed Points cannot be negative.';
            isValid = false;
        }

        if ((ae1Fixed + ae2Fixed + ae3Fixed) > parseInt(fixedPointMax,10)) {
            errorStack += '\nError: Total Fixed Points cannot be greater than ' + fixedPointMax + '.';
            isValid = false;
        }

        if ((ae1Id.value == "") && (ae1Fixed != 0)) {
            errorStack += '\nError: Please select Sales Rep #1 or remove fixed point for Sales Rep #1';
            isValid = false;
        }

        if ((ae2Id.value == "") && (ae2Fixed != 0)) {
            errorStack += '\nError: Please select Sales Rep #2 or remove fixed point for Sales Rep #2';
            isValid = false;
        }

        if ((ae3Id.value == "") && (ae3Fixed != 0)) {
            errorStack += '\nError: Please select Sales Rep #3 or remove fixed point for Sales Rep #3';
            isValid = false;
        }
        return isValid;
    }
    function ValidateApprovedByFields() {        
        var approvedBy = $get('<%=dropDownApprovedBy.ClientID%>');
        var approvedByDate = $get('<%=textApprovedDate.ClientID%>');        
        var contractEntryDate = new Date($get('<%=contractEntryDate.ClientID%>').value);
        var minEffectiveDate = new Date('1/1/2002');
        var today = new Date();
        var isValid = true;

        if ((approvedBy.value == 0) || (approvedBy.value == "")) {
            errorStack += '\nError: Please enter an Approved By User.';
            isValid = false;
        }
        if (approvedByDate.value == "") {
            errorStack += '\nError: Please enter an Approved By Date.';
            isValid = false;
            return false;            
        }        
        if (!IsValidDate(approvedByDate.value)) {
            errorStack += '\nError: Approved By Date format needs to be mm/dd/yyyy.';
            isValid = false;
            return false;
        }
        var approvedByDateAsDate = new Date(approvedByDate.value);
        if (approvedByDateAsDate < minEffectiveDate) {
            errorStack += '\nError: Approved By Date can not be less than Contract Entry Date.';
            isValid = false;
        }
        if (approvedByDateAsDate > today) {
            errorStack += '\nError: Approved By Date can not be greater than today.';
            isValid = false;
        }
        return isValid;
    }
    function ValidateEffectiveDate() {
        var effectiveDate = document.getElementById('<%=textSplitEffFrom.ClientID%>');
        var minEffectiveDate = new Date('1/1/2002');
        var isValid = true;
        
        if (effectiveDate.value == "") {
            errorStack += '\nError: Please enter an Effective Date.';
            isValid = false;
            return false;
        }
        if (!IsValidDate(effectiveDate.value)) {
            errorStack += '\nError: Effective Date format needs to be mm/dd/yyyy.';
            isValid = false;
            return false;
        }
        var effectiveDateAsDate = new Date(effectiveDate.value);
        if (effectiveDateAsDate <= minEffectiveDate) {
            errorStack += '\nError: Effective Date must be greater than 1/1/2002.';
            isValid = false;
        }
        return isValid;
    }
    function ToggleFixedPoints(disabled) {
        var fixedPtsEl1 = $get('<%=textAe1FixedSplit.ClientID%>').disabled = disabled;
        var fixedPtsEl2 = $get('<%=textAe2FixedSplit.ClientID%>').disabled = disabled;
        var fixedPtsEl3 = $get('<%=textAe3FixedSplit.ClientID%>').disabled = disabled;
        
        var revSplitEl1 = $get('<%=textAe1RevSplit.ClientID%>').disabled = !disabled;
        var revSplitEl2 = $get('<%=textAe2RevSplit.ClientID%>').disabled = !disabled;
        var revSplitEl3 = $get('<%=textAe3RevSplit.ClientID%>').disabled = !disabled;
        
        var comSplitEl1 = $get('<%=textAe1ComSplit.ClientID%>').disabled = !disabled;
        var comSplitEl2 = $get('<%=textAe2ComSplit.ClientID%>').disabled = !disabled;
        var comSplitEl3 = $get('<%=textAe3ComSplit.ClientID%>').disabled = !disabled;
    }
    function ValidateForm() {
        return (ValidateFixedPoints() && ValidateRevenueSplits());        
    }
    function ConfirmDelete() {
        return confirm('Are you sure you wish to delete this item?');
    }
    function EditContractDetailHistoryItem(contractNumber, companyId, commissionId, rowCount, isOriginal) {
        //if the original history record is selected, we do not want the user to be able to change the 
        //source data.
        $('tr[id^=historyRow]').css({'background-color':'#ffffff'});
        $get('<%=isUpdating.ClientID %>').value = (isOriginal!="0") ? "0" : "1";
        if (isOriginal=="0") {                       
            $('#historyRow' + rowCount).css({'background-color':'#ffffcc'});
        }
        LoadContractDetail(contractNumber, companyId, commissionId, true);
    }
    function ShowContractDetailHistoryItem(contractNumber, companyId, commissionId, source, effectiveFrom) {
        var hash = new Object();
        hash["COMPANYID"] = companyId;
        hash["COMMISSIONID"] = commissionId;
        hash["SOURCE"] = source;
        $get('<%=labelDetailsContractNumber.ClientID %>').innerHTML = contractNumber;
        $get('<%=labelDetailsEffectiveDate.ClientID %>').innerHTML = effectiveFrom;
        PageMethods.LoadContractDetailHistoryItem(hash, ShowContractDetailHistoryItemCallback, ErrorCallback);
    }
    function ShowContractDetailHistoryItemCallback(historyItem) {
        $get('<%=detailsArea.ClientID %>').innerHTML = historyItem;
        $get('<%=detailsPanel.ClientID %>').style.display = "block";
        $get('<%=modalPlaceHolder.ClientID %>').click();
    }
    function CloseDetailsPopup() {
        $get('<%=detailsPanel.ClientID %>').style.display = "none";
    }
    function DeleteContractDetailHistoryItem(contractNumber, companyId, commissionId, source) {
        if (confirm('Are you sure you wish to delete this item?')) {
            var hash = new Object();            
            hash["COMMISSIONID"] = parseInt(commissionId,10);
            PageMethods.DeleteHistoryItem(hash, DeleteContractDetailHistoryItemCallback, ErrorCallback);            
        }
    }
    function DeleteContractDetailHistoryItemCallback(wasDeleteSuccessful) {
        if (wasDeleteSuccessful) {
            alert('Contract Detail History Item was deleted.');
            var contractNumber = $get('<%=labelContractNumber.ClientID %>').innerHTML;
            var companyId = $get('<%=companyId.ClientID %>').value;
            LoadContractDetailHistory(contractNumber, companyId);
        }
    }
    function ShowFirstTab() {
        $('#contractInfoTabs').tabs("select", 0);
    }
    function SaveContractDetail() {
        var hash = new Object();
        hash["CONTRACTNUMBER"] = $get('<%=labelContractNumber.ClientID %>').innerHTML;
        hash["COMPANYID"] = $get('<%=companyId.ClientID %>').value;
        hash["PACKAGE"] = $get('<%=dropDownPackage.ClientID %>').value;
        hash["AE1ID"] = $get('<%=ae1.Id.ClientID %>').value;
        hash["AE2ID"] = $get('<%=ae2.Id.ClientID %>').value;
        hash["AE3ID"] = $get('<%=ae3.Id.ClientID %>').value;
        hash["AE1REV"] = $get('<%=textAe1RevSplit.ClientID %>').value;
        hash["AE2REV"] = $get('<%=textAe2RevSplit.ClientID %>').value;
        hash["AE3REV"] = $get('<%=textAe3RevSplit.ClientID %>').value;
        hash["AE1COM"] = $get('<%=textAe1ComSplit.ClientID %>').value;
        hash["AE2COM"] = $get('<%=textAe2ComSplit.ClientID %>').value;
        hash["AE3COM"] = $get('<%=textAe3ComSplit.ClientID %>').value;
        hash["AE1FIXED"] = $get('<%=textAe1FixedSplit.ClientID %>').value;
        hash["AE2FIXED"] = $get('<%=textAe2FixedSplit.ClientID %>').value;
        hash["AE3FIXED"] = $get('<%=textAe3FixedSplit.ClientID %>').value;
        hash["EFFECTIVEDATE"] = $get('<%=textSplitEffFrom.ClientID %>').value;
        hash["USEPOINTS"] = ($get('<%=radioFixedPointsYes.ClientID %>').checked) ? "Y" : "N";
        hash["APPROVEDBY"] = $get('<%=dropDownApprovedBy.ClientID %>').value;
        hash["APPROVEDBYDATE"] = $get('<%=textApprovedDate.ClientID %>').value;
        hash["COMMISSIONID"] = $get('<%=commissionId.ClientID %>').value;
        hash["ISUPDATING"] = $get('<%=isUpdating.ClientID %>').value;
        PageMethods.SaveContractData(hash, SaveContractDetailCallback, ErrorCallback);
    }
    function SaveContractDetailCallback(wasInsertSuccessful) {
        if (wasInsertSuccessful) {
            alert('Contract Details Saved.');
            var contractNumber = $get('<%=labelContractNumber.ClientID %>').innerHTML;
            var companyId = $get('<%=companyId.ClientID %>').value;
            $('tr[id^=historyRow]').css({'background-color':'#ffffff'});
            $get('<%=isUpdating.ClientID %>').value=0;
            LoadContractDetailHistory(contractNumber, companyId, ErrorCallback);                        
        } else {
            alert('Your changes could not be Saved. Please try again.\nIf the problem persits please contact the Administrator.');        
        }
    }
    function LoadContractDetail(contractNumber, companyId, commissionId, isUpdating) {
        var hash = new Object();
        hash["CONTRACTNUMBER"] = contractNumber;
        hash["COMPANYID"] = companyId;
        hash["COMMISSIONID"] = commissionId;
        if (!isUpdating) { $get('<%=isUpdating.ClientID %>').value = 0; }
        //Apollo.AutoCompleteService.GetCommissionSplitApprovers(companyId, "", AddToList, { dropDownId: "<%=dropDownApprovedBy.ClientID %>", hashTable: hash });
        Apollo.AutoCompleteService.GetCommissionSplitApprovers(companyId, "", LoadListCallback, ErrorCallback, { dropDownId: "<%=dropDownApprovedBy.ClientID %>", hashTable: hash });
        //PageMethods.LoadContractDetail(hash, LoadContractDetailCallback, ErrorCallback);
    }
    function LoadListCallback(kv, values) {
        AddToList(kv, values.dropDownId);
        PageMethods.LoadContractDetail(values.hashTable, LoadContractDetailCallback, ErrorCallback);
    }
    function LoadContractDetailCallback(values) {
        $get('<%=labelContractNumber.ClientID %>').innerHTML = values["CONTRACT_NUMBER"];
        $get('<%=contractEntryDate.ClientID %>').value = values["ENTRYDATE"];
        $get('<%=companyId.ClientID %>').value = values["COMPANYID"];
        $get('<%=contractNumber.ClientID %>').value = values["CONTRACT_NUMBER"];
        $get('<%=commissionId.ClientID %>').value = values["COMMISSION_ID"];
        $get('<%=labelTotalPercentage.ClientID %>').innerHTML = values["TOTALPERCENTAGE"];
        $get('<%=labelAgency.ClientID %>').innerHTML = values["AGENCY"];
        $get('<%=labelAdvertiser.ClientID %>').innerHTML = values["ADVERTISER"];
        $get('<%=labelProgram.ClientID %>').innerHTML = values["PROGRAM"];
        $get('<%=labelContractTotal.ClientID %>').innerHTML = values["TOTALCONTRACT"];
        $get('<%=dropDownPackage.ClientID %>').value = values["PACKAGE_ID"];

        $get('<%=ae1.Id.ClientID %>').value = values["AE_1_ID"];
        $get('<%=ae1.DependencyId.ClientID %>').value = values["COMPANYID"];
        $get('<%=ae1.Name.ClientID %>').value = values["AE_1_NAME"];
        $get('<%=textAe1RevSplit.ClientID %>').value = values["AE_1_REVENUE_PERCENTAGE"];        
        $get('<%=textAe1ComSplit.ClientID %>').value = values["AE_1_COMMISSION_PERCENTAGE"];       
        $get('<%=textAe1FixedSplit.ClientID %>').value = values["AE_1_FIXED_POINTS_PERCENTAGE"];

        $get('<%=ae2.Id.ClientID %>').value = values["AE_2_ID"];
        $get('<%=ae2.DependencyId.ClientID %>').value = values["COMPANYID"];
        $get('<%=ae2.Name.ClientID %>').value = values["AE_2_NAME"];
        $get('<%=textAe2RevSplit.ClientID %>').value = values["AE_2_REVENUE_PERCENTAGE"];
        $get('<%=textAe2ComSplit.ClientID %>').value = values["AE_2_COMMISSION_PERCENTAGE"];       
        $get('<%=textAe2FixedSplit.ClientID %>').value = values["AE_2_FIXED_POINTS_PERCENTAGE"];

        $get('<%=ae3.Id.ClientID %>').value = values["AE_3_ID"];
        $get('<%=ae3.DependencyId.ClientID %>').value = values["COMPANYID"];
        $get('<%=ae3.Name.ClientID %>').value = values["AE_3_NAME"];
        $get('<%=textAe3RevSplit.ClientID %>').value = values["AE_3_REVENUE_PERCENTAGE"];
        $get('<%=textAe3ComSplit.ClientID %>').value = values["AE_3_COMMISSION_PERCENTAGE"];       
        $get('<%=textAe3FixedSplit.ClientID %>').value = values["AE_3_FIXED_POINTS_PERCENTAGE"];

        $get('<%=textSplitEffFrom.ClientID %>').value = values["AE_SPLIT_EFFECTIVE_FROM_DATE"];
        $get('<%=radioFixedPointsYes.ClientID %>').checked = (values["USE_POINTS"] == "Y");
        $get('<%=radioFixedPointsNo.ClientID %>').checked = (values["USE_POINTS"] == "N");
        ToggleFixedPoints((values["USE_POINTS"] == "N"));
        
        $get('<%=dropDownApprovedBy.ClientID %>').value = values["APPROVED_BY_ID"];
        $get('<%=textApprovedDate.ClientID %>').value = values["APPROVED_BY_DATE"];
        if ($get('<%=isUpdating.ClientID %>').value!=1){
            LoadContractDetailHistory(values["CONTRACT_NUMBER"], values["COMPANYID"]);
        }
    }
    function LoadContractDetailHistory(contractNumber, companyId) {
        var hash = new Object();
        hash["CONTRACTNUMBER"] = contractNumber;
        hash["COMPANYID"] = companyId;
        PageMethods.LoadContractDetailHistory(hash, LoadContractDetailHistoryCallback, ErrorCallback);
    }
    function LoadContractDetailHistoryCallback(historyTable) {
        $get('<%=contractDetailHistoryTable.ClientID %>').innerHTML = historyTable;        
        LoadNoteData();
    }
    function HandleDetailHistoryClick(commissionId, source, company, effectiveDate, contractNumber) {
        var hash = new Object();
        hash["COMMISSIONID"] = commissionId;
        hash["SOURCE"] = source;
        hash["COMPANYID"] = company;
        $get('<%=labelDetailsContractNumber.ClientID %>').innerHTML = contractNumber;
        $get('<%=labelDetailsEffectiveDate.ClientID %>').innerHTML = effectiveDate;
        PageMethods.LoadContractDetailHistoryItem(hash, HandleDetailHistoryClickCallback, ErrorCallback);        
    }
    function HandleDetailHistoryClickCallback(historyDetailsTable) {
        $get('<%=detailsArea.ClientID %>').innerHTML = historyDetailsTable;                
        $find('detailsPopupExtenderBehavior').show();
    }
    function ValidateNote() {
        if ($get('<%=textNote.ClientID %>').value == "") {
            errorStack += 'Please enter a note before saving.';
            alert(errorStack);
            SetErrorDiv(errorStack,'<%=errorDiv.ClientID%>');
            return false;
        }
        return true;
    }
    function AddNote() {        
        errorStack = "";
        ClearErrorDiv('<%=errorDiv.ClientID%>');       
        if (ValidateNote()) {            
            var hash = new Object();
            hash["CONTRACTNUMBER"] = $get('<%=contractNumber.ClientID %>').value;
            hash["NOTETEXT"] = $get('<%=textNote.ClientID %>').value;
            hash["COMPANYID"] = $get('<%=companyId.ClientID %>').value;
            PageMethods.AddNotes(hash, AddNoteCallback, ErrorCallback);
        }
    }
    function AddNoteCallback(wasInsertSuccessful) {
        if (wasInsertSuccessful) {
            alert('Note Added.');
            $get('<%=textNote.ClientID %>').value = "";
            LoadNoteData();
        }
    }
    function AddNoteFailureCallback() {
        alert('Note could not be added.');
    }
    function LoadNoteData() {
        var hash = new Object();
        hash["CONTRACTNUMBER"] = $get('<%=contractNumber.ClientID %>').value;
        hash["COMPANYID"] = $get('<%=companyId.ClientID %>').value;
        PageMethods.LoadNoteData(hash, LoadNoteDataCallback, ErrorCallback);
    }
    function LoadNoteDataCallback(noteTable) {
        $get('<%=notesTable.ClientID %>').innerHTML = noteTable;
    }
    function ClearContractData() {
        $get('<%=labelContractNumber.ClientID %>').innerHTML = "";
        $get('<%=contractEntryDate.ClientID %>').value = "";
        $get('<%=companyId.ClientID %>').value = "";
        $get('<%=contractNumber.ClientID %>').value = "";
        $get('<%=commissionId.ClientID %>').value = "";
        $get('<%=labelTotalPercentage.ClientID %>').innerHTML = "";
        $get('<%=labelAgency.ClientID %>').innerHTML = "";
        $get('<%=labelAdvertiser.ClientID %>').innerHTML = "";
        $get('<%=labelProgram.ClientID %>').innerHTML = "";
        $get('<%=labelContractTotal.ClientID %>').innerHTML = "";
        $get('<%=dropDownPackage.ClientID %>').value = "";

        $get('<%=ae1.Id.ClientID %>').value = "";
        $get('<%=ae1.Name.ClientID %>').value = "";
        $get('<%=textAe1RevSplit.ClientID %>').value = "";
        $get('<%=textAe1ComSplit.ClientID %>').value = "";
        $get('<%=textAe1FixedSplit.ClientID %>').value = "";

        $get('<%=ae2.Id.ClientID %>').value = "";
        $get('<%=ae2.Name.ClientID %>').value = "";
        $get('<%=textAe2RevSplit.ClientID %>').value = "";        
        $get('<%=textAe2ComSplit.ClientID %>').value = "";
        $get('<%=textAe2FixedSplit.ClientID %>').value = "";

        $get('<%=ae3.Id.ClientID %>').value = "";
        $get('<%=ae3.Name.ClientID %>').value = "";
        $get('<%=textAe3RevSplit.ClientID %>').value = "";
        $get('<%=textAe3ComSplit.ClientID %>').value = "";
        $get('<%=textAe3FixedSplit.ClientID %>').value = "";

        $get('<%=textSplitEffFrom.ClientID %>').value = "";
        $get('<%=radioFixedPointsYes.ClientID %>').checked = false;
        $get('<%=radioFixedPointsNo.ClientID %>').checked = false;
        ToggleFixedPoints(true);        
        
        $get('<%=dropDownApprovedBy.ClientID %>').value = "";
        $get('<%=textApprovedDate.ClientID %>').value = "";
        $get('<%=contractDetailHistoryTable.ClientID %>').innerHTML = "";
        $get('<%=detailsArea.ClientID %>').innerHTML = "";
        $get('<%=textNote.ClientID %>').value = "";
        $get('<%=notesTable.ClientID %>').innerHTML = "";
    }
    $(document).ready(function() {
        $('#contractInfoTabs').tabs();
    });
</script>
<div>
    <span class="section_title"><span class="breadcrumb_current">Commission Allocation</span></span>
</div>
<br />
<div>
<asp:UpdateProgress ID="contractDetailUpdPnlPrg" runat="server">
    <ProgressTemplate>
        <div class="popupbg">
            <img src="/Images/pleasewait.gif" alt="Please Wait..." />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="contractDetailUpdPnl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="errorDiv" class="errorDisplay" style="margin:3px 0;" runat="server"></div>
        <div id="contractInfoTabs">
            <ul>
                <li><a href="#contractInfoTab">Contract Detail</a></li>
                <li><a href="#notesTab">Notes</a></li>
            </ul>
            <div id="contractInfoTab">
                <asp:HiddenField ID="contractEntryDate" runat="server" />
                <asp:HiddenField ID="companyId" runat="server" />
                <asp:HiddenField ID="commissionId" runat="server" />
                <asp:HiddenField ID="contractNumber" runat="server" />
                <asp:HiddenField ID="isUpdating" runat="server" value="0" />
                <div>
                    <div style="text-align:right;width:100%">
                        <img id="save" style="cursor:pointer;margin-right:5px" src="/Images/but_save.gif" alt="Save" onclick="ValidateEntry();" />
                        <img ID="back" runat="server" style="cursor:pointer;" alt="Back"
                            src="~/Images/but_back.gif" onclick="Back()" />
                    </div>
                    <div>
                        <table cellspacing="1" cellpadding="1" width="100%" style="background-color:#ffffff !important">
                            <tr>
                                <td colspan="4" class="infoBoxContentDark"><b>Contract Information</b></td>
                            </tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%">Number:</td>
                                <td nowrap class="infoBoxContent" width="25%" style="background-color:#ffffff !important">
                                    <asp:Label ID="labelContractNumber" runat="server" />                                            
                                </td>
                                <td nowrap class="infoBoxContentDark" width="25%" colspan="2">Total %</td>
                                <td nowrap class="infoBoxContent" width="25%" style="background-color:#ffffff !important">
                                    <asp:Label ID="labelTotalPercentage" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%">Agency</td>
                                <td nowrap class="infoBoxContent" width="25%" style="background-color:#ffffff !important"><asp:Label ID="labelAgency" runat="server" /></td>
                                <td nowrap class="infoBoxContentDark" width="25%" colspan="2">Advertiser</td>
                                <td nowrap class="infoBoxContent" width="25%" style="background-color:#ffffff !important"><asp:Label ID="labelAdvertiser" runat="server" /></td>                                                                          
                            </tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark">Program</td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important"><asp:Label ID="labelProgram" runat="server" /></td>                                    
                                <td nowrap class="infoBoxContentDark" width="25%" colspan="2">Contract Total</td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important"><asp:Label ID="labelContractTotal" runat="server" /></td>                                      
                            </tr>
                            <tr height="5px"><td colspan="5">&nbsp;</td></tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark"><b>Package</b></td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important">
                                    <asp:DropDownList ID="dropDownPackage" runat="server" DataSourceID="packageDataSource" DataTextField="PACKAGE" DataValueField="PACKAGEID" AppendDataBoundItems="true">
                                        <asp:ListItem Text="None" Value="" />
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="packageDataSource" runat="server" SelectCommandType="StoredProcedure" SelectCommand="GetPackages" />
                                </td>
                                <td nowrap class="infoBoxContentDark"><b>Revenue Split %</b></td>                                        
                                <td nowrap class="infoBoxContentDark"><b>Commission Split %</b></td>                                        
                                <td nowrap class="infoBoxContentDark">
                                    <b>Use Fixed Points (Total cannot exceed 10%)</b>&nbsp;&nbsp;&nbsp;
                                    <asp:RadioButton ID="radioFixedPointsYes" runat="server" GroupName="radioFixedPoints" onclick="ToggleFixedPoints(false);" Text="&nbsp;&nbsp;Yes" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:RadioButton ID="radioFixedPointsNo" runat="server" GroupName="radioFixedPoints" onclick="ToggleFixedPoints(true);" Text="&nbsp;&nbsp;No" />
                                </td>                                        
                            </tr>                                
                            <tr>
                                <td nowrap class="infoBoxContentDark"><b>AE #1</b></td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important">
                                    <uc:searchPopup ID="ae1" runat="server" ServiceMethod="GetAEsNonFiltered"
                                        GridContext="ae" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                        SearchImageAlt="Search AEs" SearchImageTitle="Search AEs" />                                            
                                </td>                                    
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe1RevSplit" runat="server" Width="50" />                                        
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe1ComSplit" runat="server" Width="50" />                                        
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe1FixedSplit" runat="server" Width="50" />                                        
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%"><b>AE #2</b></td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important">
                                    <uc:searchPopup ID="ae2" runat="server" ServiceMethod="GetAEsNonFiltered"
                                        GridContext="ae" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                        SearchImageAlt="Search AEs" SearchImageTitle="Search AEs" />
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe2RevSplit" runat="server" Width="50" />                                        
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe2ComSplit" runat="server" Width="50" />                                        
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe2FixedSplit" runat="server" Width="50" />                                        
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%"><b>AE #3</b></td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important">
                                    <uc:searchPopup ID="ae3" runat="server" ServiceMethod="GetAEsNonFiltered"
                                        GridContext="ae" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                        SearchImageAlt="Search AEs" SearchImageTitle="Search AEs" />                                            
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe3RevSplit" runat="server" Width="50" />                                        
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe3ComSplit" runat="server" Width="50" />                                        
                                </td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important" align="center">
                                    <asp:TextBox ID="textAe3FixedSplit" runat="server" Width="50" />                                        
                                </td>
                            </tr>                                                              
                            <tr><td colspan="5">&nbsp;</td></tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%"><b>AE Split Effective From:</b></td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important">
                                    <asp:TextBox ID="textSplitEffFrom" runat="server" />
                                    <img src="../Images/calendar.gif" id="calImage1" runat="server" alt="Click for Calendar" class="calendarPopup" />
                                    <ajax:CalendarExtender ID="textSplitEffFrom_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="textSplitEffFrom" PopupButtonID="calImage1">
                                    </ajax:CalendarExtender>
                                </td>                                    
                            </tr>                                    
                            <tr><td colspan="5">&nbsp;</td></tr>
                            <tr><td colspan="5" nowrap class="infoBoxContentDark"><b>Authorization</b></td></tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%">Approved By</td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important">
                                    <asp:DropDownList ID="dropDownApprovedBy" runat="server" />
                                        
                                </td>
                                <td nowrap class="infoBoxContentDark" colspan="2">Approved Date</td>
                                <td nowrap class="infoBoxContent" style="background-color:#ffffff !important">
                                    <asp:TextBox ID="textApprovedDate" runat="server" />
                                    <img src="../Images/calendar.gif" id="calImage2" runat="server" alt="Click for Calendar" class="calendarPopup" />
                                    <ajax:CalendarExtender ID="textApprovedDate_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="textApprovedDate" PopupButtonID="calImage2">
                                    </ajax:CalendarExtender>
                                </td>
                            </tr>
                            <tr><td colspan="5">&nbsp;</td></tr>
                            <tr><td colspan="5" nowrap class="infoBoxContentDark"><b>History</b></td></tr>
                            <tr>
                                <td colspan="5">
                                    <div>
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" class="buttonHead_Center" width="5%">Delete</td>
                                                <td valign="top" class="buttonHead_Center" width="5%">Edit</td>
                                                <td valign="top" class="buttonHead_Center" width="15%">Effective From</td>
                                                <td valign="top" class="buttonHead_Center" width="15%">AE #1</td>
                                                <td valign="top" class="buttonHead_Center" width="15%">AE #2</td>
                                                <td valign="top" class="buttonHead_Center" width="15%">AE #3</td>
                                                <td valign="top" class="buttonHead_Center" width="15%">Package</td>
                                                <td valign="top" class="buttonHead_Center" width="15%">&nbsp;</td>
                                            </tr>                                                
                                        </table>
                                    </div>
                                    <div id="contractDetailHistoryTable" class="historyTableSmall" runat="server" style="background-color:#ffffff !important"></div>
                                </td>
                            </tr>                                                                  
                        </table>
                        <input type="button" ID="modalPlaceHolder" runat="server" style="display:none" />
                        <ajax:ModalPopupExtender ID="detailsPopupExtender" runat="server" CancelControlID="closeDetails" BehaviorID="detailsPopupExtenderBehavior" 
                            TargetControlID="modalPlaceHolder" PopupControlID="detailsPanel" BackgroundCssClass="popupbg" DropShadow="true" />
                        <div ID="detailsPanel" runat="server" style="width:560px;border:1px solid #333333;background-color:White;padding:10px;display:none;">
                            <div>
                                <div style="float:left">
                                    <img src="/Images/titan_help_logo.gif" alt="Titan" />
                                </div>
                                <div style="clear:both">
                                </div>        
                                <div>        
                                    <span class="breadcrumb_current" style="width:100%;text-align:center">                                                
                                        Commission Allocation for Contract # <asp:Label ID="labelDetailsContractNumber" runat="server" /><br />(Effective <asp:Label ID="labelDetailsEffectiveDate" runat="server" />)
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
                    </div>
                </div>
            </div>
            <div id="notesTab">
                <div>      
                    <div style="text-align:right;width:100%">
                        <img src="/Images/but_save.gif" alt="Save Note" style="cursor:pointer;margin-right:5px;" onclick="AddNote();" />                                                                
                        <img ID="back2" style="cursor:pointer;" alt="Back" src="/Images/but_back.gif" onclick="Back()" />
                    </div>                        
                    <div>  
                        <table cellspacing="1" cellpadding="1" width="100%">                                
                            <tr><td colspan="2" nowrap class="infoBoxContentDark"><b>Note Entry</b></td></tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%"><b>Contract Number:</b></td>
                                <td nowrap class="infoBoxContent"><asp:Label ID="noteContractNumber" runat="server" /></td>
                            </tr>
                            <tr>
                                <td nowrap class="infoBoxContentDark" width="25%"><b>Note:</b></td>
                                <td nowrap class="infoBoxContent">
                                    <asp:TextBox ID="textNote" runat="server" TextMode="MultiLine" Rows="4" Width="90%" />
                                </td>                                    
                            </tr>    
                            <tr><td colspan="2" nowrap class="infoBoxContentDark"><b>Note History</b></td></tr>
                            <tr>
                                <td colspan="4">                                                              
                                    <table width="100%">
                                        <tr>
                                            <td valign="top" class="buttonHead_Center" width="15%">Entered On</td>
                                            <td valign="top" align="left" class="buttonHead" width="70%">Note</td>
                                            <td valign="top" class="buttonHead_Center" width="15%">Entered By</td>
                                        </tr>
                                    </table>
                                    <div id="notesTable" class="historyTable" runat="server"></div>
                                </td>
                            </tr>
                        </table>
                    </div>                                
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<br /><br />
</div>