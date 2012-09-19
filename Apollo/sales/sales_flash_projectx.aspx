﻿<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="sales_flash_projectx.aspx.cs" Inherits="Apollo.sales_sales_flash_projectx" Title="Flash | Titan 360" EnableEventValidation="false" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/contractDetailPopup.ascx" TagName="contractDetailPopup" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
    <!--[if !IE]><!-->
    <style type="text/css">
        body {overflow-y:scroll;}        
    </style> 
    <!--<![endif]-->
    <style type="text/css">
        input[type=text] {
            background-clip: padding-box;
            background-color: #FFFFFF;
            background-image: -moz-linear-gradient(center top , #FFFFFF 20%, #F6F6F6 50%, #EEEEEE 52%, #F4F4F4 100%);
            border: 1px solid #AAAAAA;
            border-radius: 5px 5px 5px 5px;
            box-shadow: 0 0 3px #FFFFFF inset, 0 1px 1px rgba(0, 0, 0, 0.1);
            color: #444444;
            height: 20px;
            line-height: 21px;
            overflow: hidden;
            padding: 0 0 0 5px;
            position: relative;
            text-decoration: none;
            white-space: nowrap;
        }
    </style>
    <script type="text/javascript">        
        function openexcel(tableNumber) {
            $get('<%=excelTable.ClientID %>').value = tableNumber;
            $get("<%=dropDownFlashYearSelectedValue.ClientID %>").value = $get("<%=dropDownFlashYear.ClientID %>").value;
            $get('<%=exportToExcel.ClientID %>').click();
        }
        function openexcelalternate(tableNumber) {
            $get('<%=excelTable.ClientID %>').value = tableNumber;
            $get("<%=dropDownFlashYearSelectedValue.ClientID %>").value = $get("<%=dropDownFlashYear.ClientID %>").value;
            $get('<%=exportToExcelAlternate.ClientID %>').click();
        }
        function showOrhide(divId) {
            var div = document.getElementById(divId);
            if (div.style.display == 'none') {
                div.style.display = 'inline';
                $get('<%=flashReportExpanded.ClientID %>').value = 1;
                document.getElementById(divId + "_img").src = "/Images/flash/arrow_down_blue.gif";                
            } else {                
                div.style.display = 'none';
                $get('<%=flashReportExpanded.ClientID %>').value = 0;
                document.getElementById(divId + "_img").src = "/Images/flash/arrow_right_blue.gif";                
            }
        }
        function expandAll(salesType) {            
            $get('<%=flashReportExpanded.ClientID %>').value = 1;            
            $('.' + salesType).show();
            $("img[id$=_img]").attr('src','/Images/flash/arrow_down_blue.gif');
        }
        function collapseAll(salesType) {            
            $get('<%=flashReportExpanded.ClientID %>').value = 0;            
            $('.' + salesType).hide();
            $("img[id$=_img]").attr('src','/Images/flash/arrow_right_blue.gif');
        }
        function toggleMoreOption() {            
            if (document.getElementById('personalSearchOptions').style.display == 'none') {
                //Effect.Appear('personalSearchOptions');
                document.getElementById('personalSearchOptions').style.display = "block";
            } else {
                //Effect.Fade('personalSearchOptions');
                document.getElementById('personalSearchOptions').style.display = "none";
            }
        }
        function SaveSearchOptions() {
            var hash = new Object();
            $get('<%=textContractNumber.ClientID %>').value = stripNonNumericCharacters($get('<%=textContractNumber.ClientID %>').value);
            hash["checkProfitCenter"] = $get('<%=checkProfitCenter.ClientID %>').checked;
            hash["checkRevenueType"] = $get('<%=checkRevenueType.ClientID %>').checked;
            hash["checkMediaType"] = $get('<%=checkMediaType.ClientID %>').checked;
            hash["checkMediaForm"] = $get('<%=checkMediaForm.ClientID %>').checked;
            hash["checkAe"] = $get('<%=checkAe.ClientID %>').checked;
            hash["checkParentProductClass"] = $get('<%=checkParentProductClass.ClientID %>').checked;
            hash["checkProductClass"] = $get('<%=checkProductClass.ClientID %>').checked;
            hash["textProgram"] = $get('<%=textProgram.ClientID %>').value;
            hash["dropDownCustomerType"] = $get('<%=dropDownCustomerType.ClientID %>').value;
            hash["textEnteredFrom"] = $get('<%=textEnteredFrom.ClientID %>').value;
            hash["textEnteredThru"] = $get('<%=textEnteredThru.ClientID %>').value;
            hash["textStartingFrom"] = $get('<%=textStartingFrom.ClientID %>').value;
            hash["textStartingTo"] = $get('<%=textStartingTo.ClientID %>').value;
            hash["textEndingFrom"] = $get('<%=textEndingFrom.ClientID %>').value;
            hash["textEndingTo"] = $get('<%=textEndingTo.ClientID %>').value;
            hash["textInvoiceCreationDateFrom"] = $get('<%=textInvoiceCreationDateFrom.ClientID %>').value;
            hash["textInvoiceCreationDateTo"] = $get('<%=textInvoiceCreationDateTo.ClientID %>').value;
            hash["textBillingFrom"] = $get('<%=textBillingFrom.ClientID %>').value;
            hash["textBillingTo"] = $get('<%=textBillingTo.ClientID %>').value;
            hash["radioSortByAdvertiser"] = $get('<%=radioSortByAdvertiser.ClientID %>').checked;
            hash["radioSortByContractNumber"] = $get('<%=radioSortByContractNumber.ClientID %>').checked;
            hash["textPanelType"] = $get('<%=textPanelType.ClientID %>').value;
            hash["dropDownRevenue"] = $get('<%=dropDownRevenue.ClientID %>').value;
            hash["dropDownNatOrLocal"] = $get('<%=dropDownNatOrLocal.ClientID %>').value;
            hash["dropDownCompany"] = $get('<%=dropDownCompany.ClientID %>').value;            
            hash["dropDownMarket"] = $get('<%=dropDownMarket.ClientID %>').value;
            hash["dropDownStartDateMonth"] = $get('<%=dropDownStartDateMonth.ClientID %>').value;
            hash["dropDownStartDateYear"] = $get('<%=dropDownStartDateYear.ClientID %>').value;
            hash["textContractNumber"] = $get('<%=textContractNumber.ClientID %>').value;
            hash["checkExcludeMTA"] = $get('<%=checkExcludeMTA.ClientID %>').checked;
            hash["checkMediaTransit"] = $get('<%=checkMediaTransit.ClientID %>').checked;
            hash["checkMediaNonTransit"] = $get('<%=checkMediaNonTransit.ClientID %>').checked;
            hash["checkShipIn"] = $get('<%=checkShipIn.ClientID %>').checked;
            hash["checkShipOut"] = $get('<%=checkShipOut.ClientID %>').checked;
            hash["checkLocal"] = $get('<%=checkLocal.ClientID %>').checked;
            hash["radioAllSalesType"] = $get('<%=radioAllSalesType.ClientID %>').checked;
            hash["radioShipIn"] = $get('<%=radioShipIn.ClientID %>').checked;
            hash["radioShipOut"] = $get('<%=radioShipOut.ClientID %>').checked;
            hash["radioLocal"] = $get('<%=radioLocal.ClientID %>').checked;
            hash["radioConsolidated"] = $get('<%=radioConsolidated.ClientID %>').checked;
            hash["radioNonConsolidated"] = $get('<%=radioNonConsolidated.ClientID %>').checked;
            hash["advertiser.AdvertiserId"] = $get('<%=advertiser.Id.ClientID %>').value;
            hash["advertiser.AdvertiserName"] = $get('<%=advertiser.Name.ClientID %>').value;
            hash["conAdvertiser.AdvertiserId"] = $get('<%=conAdvertiser.Id.ClientID %>').value;
            hash["conAdvertiser.AdvertiserName"] = $get('<%=conAdvertiser.Name.ClientID %>').value;
            hash["agency.AgencyId"] = $get('<%=agency.Id.ClientID %>').value;
            hash["agency.AgencyName"] = $get('<%=agency.Name.ClientID %>').value;            
            hash["conAgency.AgencyId"] = $get('<%=conAgency.Id.ClientID %>').value;
            hash["conAgency.AgencyName"] = $get('<%=conAgency.Name.ClientID %>').value;
            hash["productClassSearch.ProductClassId"] = $get('<%=productClassSearch.Id.ClientID %>').value;
            hash["productClassSearch.ProductClassName"] = $get('<%=productClassSearch.Name.ClientID %>').value;

            hash["dropDownParentProductClassHTML"] = GetMultiSelectValue("dropDownParentProductClassHTML", true);
            hash["dropDownProductClassHTML"] = GetMultiSelectValue("dropDownProductClassHTML", true);
            hash["dropDownProfitCenterHTML"] = GetMultiSelectValue("dropDownProfitCenterHTML", true);
            hash["dropDownAEHTML"] = GetMultiSelectValue("dropDownAEHTML", true);
            hash["dropDownMediaTypeHTML"] = GetMultiSelectValue("dropDownMediaTypeHTML", true);
            hash["dropDownMediaFormHTML"] = GetMultiSelectValue("dropDownMediaFormHTML", true);


            $("#<%=dropDownParentProductClassSelection.ClientID %>").val(GetMultiSelectValue("dropDownParentProductClassHTML", true));
            $("#<%=dropDownProductClassSelection.ClientID %>").val(GetMultiSelectValue("dropDownProductClassHTML", true));
            $("#<%=dropDownProfitCenterSelection.ClientID %>").val(GetMultiSelectValue("dropDownProfitCenterHTML", true));
            $("#<%=dropDownAESelection.ClientID %>").val(GetMultiSelectValue("dropDownAEHTML", true));
            $("#<%=dropDownMediaTypeSelection.ClientID %>").val(GetMultiSelectValue("dropDownMediaTypeHTML", true));
            $("#<%=dropDownMediaFormSelection.ClientID %>").val(GetMultiSelectValue("dropDownMediaFormHTML", true));
            document.getElementById('personalSearchOptions').style.display = "none";    
            PageMethods.SaveSearchParams(hash);          
        }            
        function BindDropDowns(){
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');
            ddlMarket = $get('<%=dropDownMarket.ClientID %>');
            radioConsolidated = $get('<%=radioConsolidated.ClientID %>');
            radioNonConsolidated = $get('<%=radioNonConsolidated.ClientID %>');            
            
            ddlCompanyDefaultVal = $get('<%=dropDownCompanyDefault.ClientID %>').value;
            ddlMarketDefaultVal = $get('<%=dropDownMarketDefault.ClientID %>').value;
            ddlProfitCenterDefaultVal = $get('<%=dropDownProfitCenterDefault.ClientID %>').value;
            ddlProdClassDefaultVal = $get('<%=dropDownProductClassDefault.ClientID %>').value;
            ddlMediaTypeDefaultVal = $get('<%=dropDownMediaTypeDefault.ClientID %>').value;            
                        
            Apollo.AutoCompleteService.GetCompanies(ddlCompanyDefaultVal,AddToList,null, '<%=dropDownCompany.ClientID %>');
            Apollo.AutoCompleteService.GetMarkets(ddlCompanyDefaultVal, ddlMarketDefaultVal, AddToList, null, '<%=dropDownMarket.ClientID %>');

            Apollo.AutoCompleteService.GetParentProductClasses(ddlCompanyDefaultVal, ddlProdClassDefaultVal, AddToList, null, "dropDownParentProductClassHTML");
            Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompanyDefaultVal, ddlMarketDefaultVal, ddlProfitCenterDefaultVal, AddToList, null, "dropDownProfitCenterHTML");
            Apollo.AutoCompleteService.GetAEDD(ddlCompanyDefaultVal, "", AddToList, null, "dropDownAEHTML");
            Apollo.AutoCompleteService.GetNewMediaTypes(ddlCompanyDefaultVal, ddlMediaTypeDefaultVal, AddToList, null, "dropDownMediaTypeHTML");
            Apollo.AutoCompleteService.GetMediaForms("", "", AddToList, null, "dropDownMediaFormHTML");
            
            //Setup default values for popups
            //Filter Advertisers
            $get('<%=advertiser.DependencyId.ClientID %>').value = ddlCompanyDefaultVal;
            $get('<%=conAdvertiser.DependencyId.ClientID %>').value = ddlCompanyDefaultVal;
            //$get(' < % = advertiserSearch.CompanyValue.ClientID %>').value = ddlCompany[ddlCompany.selectedIndex].text;
            //Filter Agencies
            $get('<%=agency.DependencyId.ClientID %>').value = ddlCompanyDefaultVal;
            $get('<%=conAgency.DependencyId.ClientID %>').value = ddlCompanyDefaultVal;            
            //$get(' < % = agencySearch.CompanyValue.ClientID %>').value = ddlCompany[ddlCompany.selectedIndex].text;            

            $get('<%=productClassSearch.CompanyId.ClientID %>').value = ddlCompanyDefaultVal;

            $addHandler(ddlCompany, "change", onCompanyChange);
            $("#<%=dropDownCompany.ClientID %>").chosen().change(onCompanyChange);
            $("#<%=dropDownMarket.ClientID %>").chosen().change(onMarketChange);

            //$("#dropDownMediaTypeHTML").chosen().change(onMediaTypeChange);

            $addHandler(radioConsolidated, "click", onRadioChange);
            $addHandler(radioNonConsolidated, "click", onRadioChange);

            LoadAdvertiserSearch();
        }
        function LoadAdvertiserSearch() {
            //Make sure the correct Advertiser Auto Complete is loaded
            if ($get('<%=radioConsolidated.ClientID %>').checked) {
                //Display the Consolidated Adv/Ag Search
                $get('conAdvSearch').style.display = "block";
                $get('conAgSearch').style.display = "block";
                //Hide the NonConsolidated and Clear out the values
                $get('nonConAdvSearch').style.display = "none";
                $get('nonConAgSearch').style.display = "none";
                $get('<%=advertiser.Id.ClientID %>').value = '';
                $get('<%=advertiser.Name.ClientID %>').value = '';
                $get('<%=agency.Id.ClientID %>').value = '';
                $get('<%=agency.Name.ClientID %>').value = '';
            } else {
                //Display the NonConsolidated Search                
                $get('nonConAdvSearch').style.display = "block";
                $get('nonConAgSearch').style.display = "block";
                //Hide the Consolidated and Clear out the values
                $get('conAdvSearch').style.display = "none";
                $get('conAgSearch').style.display = "none";
                $get('<%=conAdvertiser.Id.ClientID %>').value = '';
                $get('<%=conAdvertiser.Name.ClientID %>').value = '';
                $get('<%=conAgency.Id.ClientID %>').value = '';
                $get('<%=conAgency.Name.ClientID %>').value = '';
            }
        }
        function onRadioChange(sender, e){            
            if (sender.target.id.toLowerCase().indexOf('nonconsolidated')==-1){
                //Display the Consolidated Adv/Ag Search
                $get('conAdvSearch').style.display = "block";
                $get('conAgSearch').style.display = "block";
                //Hide the NonConsolidated and Clear out the values
                $get('nonConAdvSearch').style.display = "none";    
                $get('nonConAgSearch').style.display = "none";                    
                $get('<%=advertiser.Id.ClientID %>').value = '';
                $get('<%=advertiser.Name.ClientID %>').value = '';
                $get('<%=agency.Id.ClientID %>').value = '';
                $get('<%=agency.Name.ClientID %>').value = '';
            } else {                
                //Display the NonConsolidated Search                
                $get('nonConAdvSearch').style.display = "block";    
                $get('nonConAgSearch').style.display = "block";                               
                //Hide the Consolidated and Clear out the values
                $get('conAdvSearch').style.display = "none";
                $get('conAgSearch').style.display = "none";
                $get('<%=conAdvertiser.Id.ClientID %>').value = '';
                $get('<%=conAdvertiser.Name.ClientID %>').value = '';
                $get('<%=conAgency.Id.ClientID %>').value = '';
                $get('<%=conAgency.Name.ClientID %>').value = '';
            }
        }        
        function onCompanyChange(sender, e) {
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');                
            //Reset the Market
            Apollo.AutoCompleteService.GetMarkets(ddlCompany.value, '', AddToList, null, '<%=dropDownMarket.ClientID %>');

            Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompany.value, '', '', AddToList, null, "dropDownProfitCenterHTML");
            //Reset the Product Classes
            Apollo.AutoCompleteService.GetParentProductClasses(ddlCompany.value, '', AddToList, null, "dropDownParentProductClassHTML");
            //Reset the Media Types
            Apollo.AutoCompleteService.GetNewMediaTypes(ddlCompany.value, '', AddToList, null, "dropDownMediaTypeHTML");
            //Reset the AEs
            Apollo.AutoCompleteService.GetAEDD(ddlCompany.value, "", AddToList, null, "dropDownAEHTML");

            $("#<%=dropDownMarket.ClientID %>, #dropDownParentProductClassHTML, #dropDownMediaTypeHTML, #dropDownProfitCenterHTML, #dropDownAEHTML").trigger("liszt:updated");

            //Filter Advertisers
            $get('<%=advertiser.DependencyId.ClientID %>').value = ddlCompany.value;
            $get('<%=advertiser.DependencyValue.ClientID %>').value = ddlCompany[ddlCompany.selectedIndex].text;
            $get('<%=conAdvertiser.DependencyId.ClientID %>').value = ddlCompany.value;
            $get('<%=conAdvertiser.DependencyValue.ClientID %>').value = ddlCompany[ddlCompany.selectedIndex].text;
            //Filter Agencies
            $get('<%=agency.DependencyId.ClientID %>').value = ddlCompany.value;
            $get('<%=agency.DependencyValue.ClientID %>').value = ddlCompany[ddlCompany.selectedIndex].text;
            $get('<%=conAgency.DependencyId.ClientID %>').value = ddlCompany.value;
            $get('<%=conAgency.DependencyValue.ClientID %>').value = ddlCompany[ddlCompany.selectedIndex].text;            
            //Filter AEs
            $get('<%=marketSalesType.ClientID %>').style.display = ($("#<%=dropDownMarket.ClientID %>").val()!='') ? "block" : "none";
            $get('<%=allMarketSalesType.ClientID %>').style.display = ($("#<%=dropDownMarket.ClientID %>").val() != '') ? "none" : "block";
            $get('<%=productClassSearch.CompanyId.ClientID %>').value = ddlCompany.value;            
        }
        function GetMultiSelectValue(multiSelectId, wantArray){
            if ($("#" + multiSelectId).val() == null){return "";}
            if (wantArray){
                var val = $("#" + multiSelectId).val();
                if (val == "") { return ""; }
                return val.join();                
            } else {
                return $("#" + multiSelectId).val();
            }
        }
        function onMarketChange(sender, e) {
            var marketVal = $("#<%=dropDownMarket.ClientID %>").val();
            $get('<%=marketSalesType.ClientID %>').style.display = (marketVal != '') ? "block" : "none";
            $get('<%=allMarketSalesType.ClientID %>').style.display = (marketVal != '') ? "none" : "block";
        }                     
        function Validate() {            
            return ValidateDateRanges() && ValidateRequiredFields();
        }
        function ValidateRequiredFields() {
            if (($get('<%=textEnteredFrom.ClientID %>').value == "") || ($get('<%=textEnteredThru.ClientID %>').value == "")) {
                alert('Please enter Contract Entry From and Thru dates');
                return false;
            }           
            if (!IsValidDate($get('<%=textEnteredFrom.ClientID %>').value)) {
                alert('Contract Entry From Date is not in a valid format.'); 
                return false
            }
            if (!IsValidDate($get('<%=textEnteredThru.ClientID %>').value)) { 
                alert('Contract Entry Thru Date is not in a valid format.'); 
                return false;
            }
            var entryDateStart = new Date($get('<%=textEnteredFrom.ClientID %>').value);
            var entryDateThru = new Date($get('<%=textEnteredThru.ClientID %>').value);
            if (entryDateThru < entryDateStart) {
                alert ('Contract Entry Thru Date can not be earlier than Contract Entry From Date.');
                return false;
            }            
            if ((!$get('<%=checkMediaTransit.ClientID %>').checked) && (!$get('<%=checkMediaNonTransit.ClientID %>').checked)) {
                alert('Please choose a Media of Transit, Non-Transit, or both');
                return false;
            }      
            if ($("#<%=dropDownMarket.ClientID %>").val() == ""){
                if (!$get('<%=radioAllSalesType.ClientID %>').checked
                    && !$get('<%=radioShipIn.ClientID %>').checked
                    && !$get('<%=radioShipOut.ClientID %>').checked
                    && !$get('<%=radioLocal.ClientID %>').checked) {
                        alert('Please specify a Sales Type.');
                        return false;            
                    }
            }
            if ($("#<%=dropDownMarket.ClientID %>").val() != "") {
                if (!$get('<%=checkShipIn.ClientID %>').checked
                    && !$get('<%=checkShipOut.ClientID %>').checked
                    && !$get('<%=checkLocal.ClientID %>').checked) {
                    alert('Please specify a Sales Type when choosing a Market.');
                    return false;
                }                
            }
            return true;
        }
        function ValidateDateRanges() {                        
            var contractStartingFrom = $get('<%=textStartingFrom.ClientID %>').value;
            var contractStartingTo = $get('<%=textStartingTo.ClientID %>').value;
            if (contractStartingFrom != "") {                
                if (!IsValidDate(contractStartingFrom)) {
                    alert('Contract Starting From Date is not in a valid format.'); 
                    return false;
                }
            }
            if (contractStartingTo != "") {
                if (!IsValidDate(contractStartingTo)) {
                    alert('Contract Starting To Date is not in a valid format.'); 
                    return false;
                }
            }
            if (Date(contractStartingTo) < Date(contractStartingFrom)){
                alert ('Contract Starting To Date can not be earlier than Contract Starting From Date.');
                return false;
            }

            var contractEndingFrom = $get('<%=textEndingFrom.ClientID %>').value;
            var contractEndingTo = $get('<%=textEndingTo.ClientID %>').value;
            if (contractEndingFrom != "") {
                if (!IsValidDate(contractEndingFrom)) {
                    alert('Contract End From Date is not in a valid format.');
                    return false;
                }
            }
            if (contractEndingTo != "") {
                if (!IsValidDate(contractEndingTo)) {
                    alert('Contract End To Date is not in a valid format.');
                    return false;
                }
            }
            if (Date(contractEndingTo) < Date(contractEndingFrom)) {
                alert('Contract End To Date can not be earlier than Contract End From Date.');
                return false;
            }               
 
            var contractBillingFrom = $get('<%=textBillingFrom.ClientID %>').value;
            var contractBillingTo = $get('<%=textBillingTo.ClientID %>').value;
            if (contractBillingFrom != "") {
                if (!IsValidDate(contractBillingFrom)) {
                    alert('Contract Billing From Date is not in a valid format.'); 
                    return false;
                }
            }
            if (contractBillingTo != "") {
                if (!IsValidDate(contractBillingTo)) {
                    alert('Contract Billing To Date is not in a valid format.'); 
                    return false;
                }
            }
            if (Date(contractBillingTo) < Date(contractBillingFrom)){
                alert ('Contract Billing To Date can not be earlier than Contract Billing From Date.');
                return false;
            }

            var invoiceCreationDateFrom = $get('<%=textBillingFrom.ClientID %>').value;
            var invoiceCreationDateTo = $get('<%=textBillingTo.ClientID %>').value;
            if (invoiceCreationDateFrom != "") {
                if (!IsValidDate(invoiceCreationDateFrom)) {
                    alert('Invoice Creation From Date is not in a valid format.');
                    return false;
                }
            }
            if (invoiceCreationDateTo != "") {
                if (!IsValidDate(invoiceCreationDateTo)) {
                    alert('Invoice Creation To Date is not in a valid format.');
                    return false;
                }
            }
            if (Date(invoiceCreationDateTo) < Date(invoiceCreationDateFrom)) {
                alert('Invoice Creation To Date can not be earlier than Invoice Creation From Date.');
                return false;
            }   
            return true;
        }
        function CustomerPreSubmitCheck() {
            //Sanity check...
            //  Make sure entries in the Advertiser/Agency fields don't get 'stuck'
            if ($get('<%=radioConsolidated.ClientID %>').checked) {                
                $get('<%=advertiser.Id.ClientID %>').value = '';
                $get('<%=advertiser.Name.ClientID %>').value = '';
                $get('<%=agency.Id.ClientID %>').value = '';
                $get('<%=agency.Name.ClientID %>').value = '';
            }
            if ($get('<%=radioNonConsolidated.ClientID %>').checked) {
                $get('<%=conAdvertiser.Id.ClientID %>').value = '';
                $get('<%=conAdvertiser.Name.ClientID %>').value = '';
                $get('<%=conAgency.Id.ClientID %>').value = '';
                $get('<%=conAgency.Name.ClientID %>').value = '';
            }
        }
        function ExecuteSearch() {
            CustomerPreSubmitCheck();
            if (Validate()) {
                $get('<%=flashGridUpdPnlProgress.ClientID %>').style.display = 'block';
                SaveSearchOptions();
                var ddlMarket = $get('<%=dropDownMarket.ClientID %>');
                if (ddlMarket.value!=''){
                    $get('<%=dropDownMarketSelectedText.ClientID %>').value = ddlMarket[ddlMarket.selectedIndex].text;
                }
                return true;
            }
            return false;
        }
        function Search() {
            return ExecuteSearch();            
        }
        function Save() {
            return ExecuteSearch();
        }
        function DropDownFlashYearChange(ddl) {
            var year = ddl.value;
            $get('<%=dropDownStartDateYear.ClientID %>').value = year;
            $get('<%=search.ClientID %>').click();
            //Search();
        }
        function handleContractKeyPress(e, buttonId) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13 && evt.shiftKey) {
                var contractNumber = $get('<%=textContractNumber.ClientID %>').value;
                var companyId = $get('<%=dropDownCompany.ClientID %>').value;
                ShowContractDetail(contractNumber, companyId);
                return true;
            } else {
                return setDefaultButton(e, buttonId);
            }
        }           
        function ContractSearchAutoCompleteSelectHandler(source, eventArgs) {
            var selectedName = eventArgs.get_value();
            var selectedNameSplit = selectedName.split('-');
            var contractNumber = trimValue(selectedNameSplit[0]);
            $get('<%=textContractNumber.ClientID %>').value = contractNumber;
        }
        $(document).ready(function () {
            $("#dropDownParentProductClassHTML, #<%=dropDownCustomerType.ClientID %>, #<%=dropDownRevenue.ClientID %>, #<%=dropDownNatOrLocal.ClientID %>, #<%=dropDownCompany.ClientID %>").chosen();
            $("#<%=dropDownMarket.ClientID %>, #<%=dropDownStartDateMonth.ClientID %>, #<%=dropDownStartDateYear.ClientID %>, #dropDownMediaTypeHTML, #dropDownMediaFormHTML").chosen();
            $("#dropDownProfitCenterHTML, #dropDownAEHTML").chosen();
            SetDateRangePicker("<%=textEnteredFrom.ClientID %>", "<%=textEnteredThru.ClientID %>");
            SetDateRangePicker("<%=textStartingFrom.ClientID %>", "<%=textStartingTo.ClientID %>");
            SetDateRangePicker("<%=textEndingFrom.ClientID %>", "<%=textEndingTo.ClientID %>");
            SetDateRangePicker("<%=textInvoiceCreationDateFrom.ClientID %>", "<%=textInvoiceCreationDateTo.ClientID %>");
            SetDateRangePicker("<%=textBillingFrom.ClientID %>", "<%=textBillingTo.ClientID %>");
        });
        function ClearSearchOptions() {
            var currentDate = new Date();
            $get('<%=textContractNumber.ClientID %>').value = "";
            $get('<%=checkProfitCenter.ClientID %>').checked = false;
            $get('<%=checkRevenueType.ClientID %>').checked = false;
            $get('<%=checkMediaType.ClientID %>').checked = false;
            $get('<%=checkMediaForm.ClientID %>').checked = false;
            $get('<%=checkAe.ClientID %>').checked = false;
            $get('<%=checkParentProductClass.ClientID %>').checked = false;
            $get('<%=checkProductClass.ClientID %>').checked = false;
            $get('<%=textProgram.ClientID %>').value = "";
            $get('<%=dropDownCustomerType.ClientID %>').value = "";
            $get('<%=textEnteredFrom.ClientID %>').value = "1/1/2002";
            $get('<%=textEnteredThru.ClientID %>').value = GetDateAsString(currentDate);
            $get('<%=textStartingFrom.ClientID %>').value = "";
            $get('<%=textStartingTo.ClientID %>').value = "";
            $get('<%=textEndingFrom.ClientID %>').value = "";
            $get('<%=textEndingTo.ClientID %>').value = "";
            $get('<%=textInvoiceCreationDateFrom.ClientID %>').value = "";
            $get('<%=textInvoiceCreationDateTo.ClientID %>').value = "";
            $get('<%=textBillingFrom.ClientID %>').value = "";
            $get('<%=textBillingTo.ClientID %>').value = "";
            $get('<%=radioSortByAdvertiser.ClientID %>').checked = true;
            $get('<%=radioSortByContractNumber.ClientID %>').checked = false;
            $get('<%=textPanelType.ClientID %>').value = "";
            $get('<%=dropDownRevenue.ClientID %>').value = "M";
            $get('<%=dropDownNatOrLocal.ClientID %>').value = "";
            $get('<%=dropDownCompany.ClientID %>').value = "1";
            $get('<%=dropDownMarket.ClientID %>').value = "";
            $get('<%=dropDownStartDateMonth.ClientID %>').value = 1;
            $get('<%=dropDownStartDateYear.ClientID %>').value = currentDate.getFullYear();
            $get('<%=textContractNumber.ClientID %>').value = "";
            $get('<%=checkExcludeMTA.ClientID %>').checked = true;
            $get('<%=checkMediaTransit.ClientID %>').checked = true;
            $get('<%=checkMediaNonTransit.ClientID %>').checked = true;
            $get('<%=marketSalesType.ClientID %>').style.display = "none";
            $get('<%=allMarketSalesType.ClientID %>').style.display = "block";
            $get('<%=checkShipIn.ClientID %>').checked = true;
            $get('<%=checkShipOut.ClientID %>').checked = true;
            $get('<%=checkLocal.ClientID %>').checked = true;
            $get('<%=radioAllSalesType.ClientID %>').checked = true;
            $get('<%=radioShipIn.ClientID %>').checked = false;
            $get('<%=radioShipOut.ClientID %>').checked = false;
            $get('<%=radioLocal.ClientID %>').checked = false;
            $get('<%=radioConsolidated.ClientID %>').checked = true;
            $get('<%=radioNonConsolidated.ClientID %>').checked = false;
            $get('<%=advertiser.Id.ClientID %>').value = "";
            $get('<%=advertiser.Name.ClientID %>').value = "";
            $get('conAdvSearch').style.display = "block";
            $get('conAgSearch').style.display = "block";
            $get('nonConAdvSearch').style.display = "none";
            $get('nonConAgSearch').style.display = "none"; 
            $get('<%=conAdvertiser.Id.ClientID %>').value = "";
            $get('<%=conAdvertiser.Name.ClientID %>').value = "";
            $get('<%=agency.Id.ClientID %>').value = "";
            $get('<%=agency.Name.ClientID %>').value = "";
            $get('<%=conAgency.Id.ClientID %>').value = "";
            $get('<%=conAgency.Name.ClientID %>').value = "";
            $get('<%=productClassSearch.Id.ClientID %>').value = "";
            $get('<%=productClassSearch.Name.ClientID %>').value = "";

            $("#dropDownParentProductClassHTML, #dropDownProductClassHTML, #dropDownProfitCenterHTML, #dropDownAEHTML, #dropDownMediaTypeHTML, #dropDownMediaFormHTML").val("");
            $("#<%=dropDownParentProductClassSelection %>, #<%=dropDownProductClassSelection %>, #<%=dropDownProfitCenterSelection %>").value = "";
            $("#<%=dropDownAESelection %>, #<%=dropDownMediaTypeSelection %>, #<%=dropDownMediaFormSelection %>").value = "";
            return ExecuteSearch();
        }
    </script>
    <div style="margin-bottom:10px">   
        <asp:HiddenField ID="dropDownCompanyDefault" runat="server" />    
        <asp:HiddenField ID="dropDownMarketDefault" runat="server" />
        <asp:HiddenField ID="dropDownMarketSelectedText" runat="server" />
        <asp:HiddenField ID="dropDownProfitCenterDefault" runat="server" />        
        <asp:HiddenField ID="dropDownProductClassDefault" runat="server" />
        <asp:HiddenField ID="dropDownMediaTypeDefault" runat="server" />
        <asp:HiddenField ID="dropDownFlashYearSelectedValue" runat="server" />        

        <asp:HiddenField ID="dropDownParentProductClassSelection" runat="server" />
        <asp:HiddenField ID="dropDownProductClassSelection" runat="server" />
        <asp:HiddenField ID="dropDownMarketSelection" runat="server" />
        <asp:HiddenField ID="dropDownProfitCenterSelection" runat="server" />
        <asp:HiddenField ID="dropDownAESelection" runat="server" />
        <asp:HiddenField ID="dropDownMediaTypeSelection" runat="server" />
        <asp:HiddenField ID="dropDownMediaFormSelection" runat="server" />        
        <asp:HiddenField ID="userId" runat="server" />
        <div style="float:left;width:20%;text-align:left;">
            <span class="inventory_search">SEARCH FILTERS</span>
        </div>
        <div style="float:left;width:60%;text-align:center;">
            <span class="search_filter_title">Flash</span>
        </div>
        <div style="float:left;width:19%;text-align:right;">
            <span class="search_filter_options" style="cursor:pointer;">
                <a href="#" onclick="toggleMoreOption();">options</a>
            </span>
        </div>
        <div style="clear:both;"></div>
    </div>
    <div id="searchOptionsErrors" style="margin-top:5px;display:none;padding:5px 10px">
    </div>
    <div id="personalSearchOptions" style="margin-top:5px;display:none">
        <div class="horizontalSearchCriteriaArea">
            <div style="float:left;width:30%;text-align:left;">                
                <ul class="horizontalSearchFilters" style="width:100% !important;">
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Sub Total:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:CheckBox ID="checkProfitCenter" runat="server" style="padding-right:5px" />Profit Center                                
                                <asp:CheckBox ID="checkRevenueType" runat="server" style="padding-right:5px" />Revenue Type
                                <asp:CheckBox ID="checkMediaType" runat="server" style="padding-right:5px;" />Media Type
                                <asp:CheckBox ID="checkMediaForm" runat="server" style="padding:0 5px;" />Media Form
                                <br />
                                <asp:CheckBox ID="checkParentProductClass" runat="server" style="padding-right:5px;" />Parent Product Class
                                <asp:CheckBox ID="checkProductClass" runat="server" style="padding-right:5px;" />Product Class
                                <asp:CheckBox ID="checkAe" runat="server" style="padding:0 5px" />AE
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Program:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:TextBox ID="textProgram" runat="server" Width="181px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Parent Prod. Class:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <select id="dropDownParentProductClassHTML" multiple="multiple" style="width:185px;"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Product Class:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <select id="dropDownProductClassHTML" multiple="multiple" style="width:185px;display:none;"></select>
                                <uc:searchPopup ID="productClassSearch" runat="server" ServiceMethod="GetProductClasses"
                                    GridContext="productClass" AutoCompleteIdIndex="0" AutoCompleteNameIndex="2"
                                    SearchImageAlt="Search Product Classes" SearchImageTitle="Search Product Classes" />                               
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                    
                </ul>
            </div>
            <div style="float:left;width:30%;text-align:left;">                
                <ul class="horizontalSearchFilters" style="width:100% !important;">
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Contract Entry Date:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <div style="float:left;margin-right:5px;">
                                    From:&nbsp;
                                    <asp:TextBox ID="textEnteredFrom" runat="server" Width="60px" />
                                </div>
                                <div style="float:left;">
                                    To:&nbsp;
                                    <asp:TextBox ID="textEnteredThru" runat="server" Width="60px" />
                                </div>
                                <div style="clear:both;"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Contract Start Date:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <div style="float:left;margin-right:5px;">
                                    From:&nbsp;
                                    <asp:TextBox ID="textStartingFrom" runat="server" Width="60px" />
                                </div>
                                <div style="float:left;">
                                    To:&nbsp;
                                    <asp:TextBox ID="textStartingTo" runat="server" Width="60px" />
                                </div>
                                <div style="clear:both;"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Contract End Date:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <div style="float:left;margin-right:5px;">
                                    From:&nbsp;
                                    <asp:TextBox ID="textEndingFrom" runat="server" Width="60px" />
                                </div>
                                <div style="float:left;">
                                    To:&nbsp;
                                    <asp:TextBox ID="textEndingTo" runat="server" Width="60px" />
                                </div>
                                <div style="clear:both;"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Invoice Creation Date:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <div style="float:left;margin-right:5px;">
                                    From:&nbsp;
                                    <asp:TextBox ID="textInvoiceCreationDateFrom" runat="server" Width="60px" />
                                </div>
                                <div style="float:left;">
                                    To:&nbsp;
                                    <asp:TextBox ID="textInvoiceCreationDateTo" runat="server" Width="60px" />
                                </div>
                                <div style="clear:both"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Billing Date:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <div style="float:left;margin-right:5px;">
                                    From:&nbsp;
                                    <asp:TextBox ID="textBillingFrom" runat="server" Width="60px" />
                                </div>
                                <div style="float:left;">
                                    To:&nbsp;
                                    <asp:TextBox ID="textBillingTo" runat="server" Width="60px" />
                                </div>
                                <div style="float:left;"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                </ul>
            </div>
            <div style="float:left;width:30%;text-align:left;">                
                <ul class="horizontalSearchFilters" style="width:100% !important;">
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Sort By:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:RadioButton ID="radioSortByAdvertiser" runat="server" GroupName="SortBy" Text="&nbsp;Advertiser" Checked="true" />
                                <asp:RadioButton ID="radioSortByContractNumber" runat="server" GroupName="SortBy" Text="&nbsp;Contract Number" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Exclude MTA?:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:CheckBox ID="checkExcludeMTA" runat="server" />
                            </div>
                        </div>
                    </li>
                    <li style="display:none">
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Panel Type:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:TextBox ID="textPanelType" Width="181px" runat="server" TextMode="MultiLine" Rows="3" ToolTip="Enter a comma-separated list of Panel Types" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                    
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">National/Local:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:DropDownList ID="dropDownNatOrLocal" runat="server" Width="185px">
                                    <asp:ListItem Selected="True" Text=" * ALL" Value="" />
                                    <asp:ListItem Text="National" Value="N" />
                                    <asp:ListItem Text="Local" Value="L" /> 
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Customer Type:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:DropDownList ID="dropDownCustomerType" runat="server" Width="185px">
                                    <asp:ListItem Selected="True" Text=" * ALL" Value="" />
                                    <asp:ListItem Text="Agency" Value="AG" />
                                    <asp:ListItem Text="Large Agency" Value="LA" />
                                    <asp:ListItem Text="Direct Client" Value="DC" />
                                </asp:DropDownList>                            
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                </ul>
            </div>
            <div style="float:left;width:9%;text-align:left;padding-top:10px;">
                <asp:UpdatePanel ID="saveUpdPnl" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton ID="save" runat="server" ImageUrl="~/Images/but_save.gif" 
                            OnClientClick="return Save()" AlternateText="Save" onclick="save_Click" style="cursor:pointer" />
                    </ContentTemplate>
                </asp:UpdatePanel>                
            </div>
            <div style="clear:both;"></div>            
        </div>
    </div>        
    <div style="margin-top:5px;display:block;">
        <div class="horizontalSearchCriteriaArea">
            <div style="float:left;width:30%;text-align:left;">                
                <ul class="horizontalSearchFilters">
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Company:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:DropDownList ID="dropDownCompany" runat="server" Width="185px" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Market:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:DropDownList ID="dropDownMarket" runat="server" Width="185px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Start Date:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:DropDownList ID="dropDownStartDateMonth" runat="server" Width="108px" />                                
                                <asp:DropDownList ID="dropDownStartDateYear" runat="server" Width="69px" style="margin-left:5px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Contract #:</div>
                            <div class="horizontalSearchFilterRightColumn"> 
                                <asp:TextBox ID="textContractNumber" runat="server" Width="181px" onclick="this.select()" AutoCompleteType="Disabled" autocomplete="off" />
                                <ajax:AutoCompleteExtender ID="textContractNumber_AutoCompleteExtender" OnClientItemSelected="ContractSearchAutoCompleteSelectHandler"
                                    CompletionListItemCssClass="autoCompleteItem" CompletionListHighlightedItemCssClass="autoCompleteItemSelected" CompletionListCssClass="modalAutoComplete" 
                                    runat="server" DelimiterCharacters="" Enabled="True" ServicePath="~/services/AutoCompleteService.asmx" UseContextKey="false" 
                                    TargetControlID="textContractNumber" CompletionInterval="10" ServiceMethod="GetRevContracts" MinimumPrefixLength="2">
                                </ajax:AutoCompleteExtender>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                    
                </ul>
            </div>
            <div style="float:left;width:30%;text-align:left;">                
                <ul class="horizontalSearchFilters">
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Profit Center:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <select id="dropDownProfitCenterHTML" multiple="multiple" style="width:185px;"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">AE:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <select id="dropDownAEHTML" multiple="multiple" style="width:185px;"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Media:</div>
                            <div class="horizontalSearchFilterRightColumn">    
                                <asp:CheckBox ID="checkMediaTransit" runat="server" style="padding-right:5px" />Transit
                                <asp:CheckBox ID="checkMediaNonTransit" runat="server" style="padding:0 5px" />Non-Transit            
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Revenue:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:DropDownList ID="dropDownRevenue" runat="server" Width="185px">
                                    <asp:ListItem Selected="True" Text="All" Value="" />
                                    <asp:ListItem Text="Media" Value="M" />
                                    <asp:ListItem Text="Production" Value="P" />
                                    <asp:ListItem Text="Installation" Value="I" />
                                    <asp:ListItem Text="Media + Installation" Value="MI" />
                                    <asp:ListItem Text="Media + Production" Value="MP" />
                                    <asp:ListItem Text="Media + Production + Installation" Value="MPI" />
                                    <asp:ListItem Text="Production + Installation" Value="PI" />
                                    <asp:ListItem Text="Barter" Value="B" />
                                    <asp:ListItem Text="Digital" Value="D" Enabled="false" />
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Sales Type:</div>
                            <div class="horizontalSearchFilterRightColumn">    
                                <div id="marketSalesType" runat="server">                                
                                    <asp:CheckBox ID="checkShipIn" runat="server" style="padding-right:5px" />Ship In
                                    <asp:CheckBox ID="checkShipOut" runat="server" style="padding:0 5px" />Ship Out
                                    <asp:CheckBox ID="checkLocal" runat="server" style="padding:0 5px" />Local            
                                </div>
                                <div id="allMarketSalesType" runat="server">
                                    <asp:RadioButton ID="radioAllSalesType" GroupName="allMarketSales" runat="server" Text="&nbsp;&nbsp;All" style="padding-right:5px;" />
                                    <asp:RadioButton ID="radioShipIn" GroupName="allMarketSales" runat="server" Text="&nbsp;&nbsp;Ship In" style="padding-right:5px;" />
                                    <asp:RadioButton ID="radioShipOut" GroupName="allMarketSales" runat="server" Text="&nbsp;&nbsp;Ship Out" style="padding:0 5px;" />
                                    <asp:RadioButton ID="radioLocal" GroupName="allMarketSales" runat="server" Text="&nbsp;&nbsp;Local" style="padding:0 5px;" />
                                </div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                </ul>
            </div>
            <div style="float:left;width:30%;text-align:left;">                
                <ul class="horizontalSearchFilters">
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Customer Type:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <asp:RadioButton ID="radioConsolidated" runat="server" Checked="true" GroupName="radioCustomerTypeGroup" Text="&nbsp;&nbsp;Consolidated" style="margin-right:10px;" />
                                <asp:RadioButton ID="radioNonConsolidated" runat="server" GroupName="radioCustomerTypeGroup" Text="&nbsp;&nbsp;Non-Consolidated" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Advertiser:</div>
                            <div class="horizontalSearchFilterRightColumn">  
                                <div id="nonConAdvSearch" style="display:none">
                                    <uc:searchPopup ID="advertiser" runat="server" ServiceMethod="GetAdvertisers"
                                        GridContext="advertiser" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                        SearchImageAlt="Search Advertisers" SearchImageTitle="Search Advertisers" />                                    
                                </div>
                                <div id="conAdvSearch" style="display:block">
                                    <uc:searchPopup ID="conAdvertiser" runat="server" ServiceMethod="GetAdvertisersDL"
                                        GridContext="consolidatedAdvertiser" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                        SearchImageAlt="Search Advertisers" SearchImageTitle="Search Advertisers" />                                    
                                </div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Agency:</div>
                            <div class="horizontalSearchFilterRightColumn">                                
                                <div id="nonConAgSearch" style="display:none">
                                    <uc:searchPopup ID="agency" runat="server" ServiceMethod="GetAgencies"
                                        GridContext="agency" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                        SearchImageAlt="Search Agencies" SearchImageTitle="Search Agencies" />                                    
                                </div>
                                <div id="conAgSearch" style="display:block">
                                    <uc:searchPopup ID="conAgency" runat="server" ServiceMethod="GetAgenciesDL"
                                        GridContext="consolidatedAgency" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                        SearchImageAlt="Search Agencies" SearchImageTitle="Search Agencies" />                                    
                                </div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Media Type:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <select id="dropDownMediaTypeHTML" multiple="multiple" style="width:185px;"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="horizontalSearchFilterLeftColumn">Media Form:</div>
                            <div class="horizontalSearchFilterRightColumn">
                                <select id="dropDownMediaFormHTML" multiple="multiple" style="width:185px;"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                </ul>
            </div>
            <div style="float:left;width:9%;text-align:left;padding-top:10px;">
                <asp:UpdatePanel ID="searchUpdPnl" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton ID="search" runat="server" ImageUrl="~/Images/but_search.gif" 
                            style="cursor:pointer" onclick="search_Click" OnClientClick="return Search()" />
                        <asp:ImageButton ID="clear" runat="server" ImageUrl="~/Images/but_clear.gif"
                            style="cursor:pointer;margin-left:5px;" onclick="clear_Click" OnClientClick="return ClearSearchOptions();" />
                    </ContentTemplate>
                </asp:UpdatePanel>                
            </div>
            <div style="clear:both;"></div> 
        </div>
    </div>
    <script type="text/javascript">
        BindDropDowns();        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportListPanel" Runat="Server">
    <div align="center" class="flashTable">
        <asp:UpdateProgress ID="flashGridUpdPnlProgress" AssociatedUpdatePanelID="flashGridUpdPnl" runat="server">
            <ProgressTemplate>
                <div style="margin:5px;"><img src="../Images/pleasewait.gif" alt="Please Wait" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="flashGridUpdPnl" runat="server" UpdateMode="Conditional" EnableViewState="false">                    
            <Triggers>
                <asp:PostBackTrigger ControlID="exportToExcel" />
                <asp:PostBackTrigger ControlID="exportToExcelAlternate" />
            </Triggers>
            <ContentTemplate>   
                <asp:HiddenField ID="flashReportExpanded" runat="server" Value="0" />
                <asp:HiddenField ID="excelTable" runat="server" Value="0" />
                <asp:Button ID="exportToExcel" runat="server" style="display:none" 
                    onclick="exportToExcel_Click" />
                <asp:Button ID="exportToExcelAlternate" runat="server" style="display:none" 
                    onclick="exportToExcelAlternate_Click" />
                <div id="searchResults" runat="server" style="width:100%;display:none;" enableviewstate="false">
                    <table width="100%" cellpadding="0" cellspacing="1" border="0" align="center" class="inventory_views_main_table" bgcolor="#ffffff">
                        <tr>
                            <td style="padding-top:4px;padding-bottom:5px;">
                                <table cellspacing="0" border="0" cellpadding="0" width="100%" style="border-collapse: collapse;">
                                    <tr>
                                        <td width="22%"><img src="../Images/flash/spacer.gif" width="192" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="61" height="0" alt="spacer"></td>
                                        <td width="6%" align="right"><img src="../Images/flash/spacer.gif" width="67" height="0" alt="spacer"></td>
                                    </tr>
                                    <tr bgcolor="#ffffff">
                                        <td colspan="15"><asp:Label ID="labelFlashReportMessage" runat="server" Visible="false" /></td>
                                    </tr>
						            <tr bgcolor="#ffffff">
                                        <td colspan="15">
                                            <asp:Label ID="labelDropDownFlashYear" runat="server" AssociatedControlID="dropDownFlashYear" Text="Years with Billing: " Visible="false" />
                                            <asp:DropDownList ID="dropDownFlashYear" runat="server" Visible="false" onchange="DropDownFlashYearChange(this);" />
                                        </td>
                                    </tr>
                                    <asp:Label ID="labelFlashReport" runat="server" />
						        </table>
						    </td>
						</tr>
					</table>
                </div> 
                <uc:contractDetailPopup id="contractDetailPopup" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>            
    </div>    
</asp:Content>