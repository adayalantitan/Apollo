<%@ Page Title="Digital Library | Titan 360" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchVertSplit.master" AutoEventWireup="true" CodeFile="digital_library.aspx.cs" Inherits="Apollo.digital_digital_library" MaintainScrollPositionOnPostback="false" EnableEventValidation="false" EnableViewState="false" %>
<%@ Register Src="~/UserControls/digitalUploader.ascx" TagName="digitalUploader" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalImageDetailNew.ascx" TagName="digitalImageDetail" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalTagger.ascx" TagName="digitalTagger" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {overflow-y:scroll;}
        ul.searchFilters li {padding:1px 8px !important;margin:0;}        
    </style>        
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="searchCriteriaPanel" Runat="Server">        
    <script type="text/javascript" src="DigitalLibrary.js?v=1.0"></script>
    <script type="text/javascript">
        var searchTimeoutId;
        var enterPressedTwice = false;
        //$("#appliedSearchFilterList").live("mouseover", BuildAppliedFiltersDisplay);
        $(document).ready(function () {
            selectedImages = {};
            RegisterSearchFilterKeyPressHandler();
            $('.dlImagePopup').cluetip({ width: 305
                , cluetipClass: 'rounded'
                //, closeText: "<img src='/Images/dl/icon_tooltip_close.png' alt='Close' title='Close' style='margin-top:2px !important;position:absolute !important'/>"
                , dropShadow: false, sticky: false, ajaxCache: false, arrows: true, positionBy: 'bottomTop'
                , ajaxSettings: { dataType: 'html' }
                , hoverIntent: { sensitivity: 1, interval: 100, timeout: 0 }
            });
            $('.mapIconLink').cluetip({ width: 225, height: 225
			    , showTitle: false
			    , dropShadow: false, sticky: false, ajaxCache: false
			    , ajaxSettings: { dataType: 'html' }
			    , hoverIntent: { sensitivity: 1, interval: 100, timeout: 0 }
            });
            ddlCompanyDefaultVal = $get('<%=dropDownCompanyDefault.ClientID %>').value;
            ddlMarketDefaultVal = $get('<%=dropDownMarketDefault.ClientID %>').value;
            ddlProfitCenterDefaultVal = $get('<%=dropDownProfitCenterDefault.ClientID %>').value;
            ddlProdClassDefaultVal = $get('<%=dropDownProductClassDefault.ClientID %>').value;
            ddlMediaTypeDefaultVal = $get('<%=dropDownMediaTypeDefault.ClientID %>').value;
            ddlEthnicityDefaultVal = $get('<%=dropDownEthnicityDefault.ClientID%>').value;
            try { $('#aspnetForm').submit(function (event) { if (event.keyCode == 13) { return false; } }); } catch (e) { }
            try {
                if ($get('<%=dropDownMarketDefault.ClientID %>').value.toLowerCase() == "nyo") {
                    //$get('subMarket').style.display = "block";
                }
            }
            catch (e) { }
            LoadAdvertiserSearch();
        });
        function onSearchFilterKeyPress(evt) {
            $find('globalGridPopupExtBehavior').hide();
            if (evt.keyCode == 13) {
                try { evt.preventDefault(); } catch (e) { }
                searchTimeoutId = setTimeout(ExecuteSearch, 500);
            }
        }
        function onAutoCompleteSearchFilterKeyPress(evt) {
            $find('globalGridPopupExtBehavior').hide();
            //If the user hits the Enter key to select an option
            //  from the autocomplete, we do not want to the form to submit
            //  Add a flag that determines if the Enter key is pressed a second time
            //      (the flag will reset after .5 seconds)          
            if (evt.keyCode == 13 && enterPressedTwice) {
                enterPressedTwice = false;
                searchTimeoutId = setTimeout(ExecuteSearch, 500);
            } else {
                enterPressedTwice = true;
                setTimeout(function () { enterPressedTwice = false; }, 500);
            }
        }
        function RegisterSearchFilterKeyPressHandler(){
            $addHandler($get('<%=textSearch.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textContractNumber.ClientID %>'),"keyup",onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=dropDownCompany.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=dropDownMarket.ClientID %>'),"keyup",onSearchFilterKeyPress);            
            $addHandler($get('<%=dropDownProfitCenter.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=dropDownProductClass.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=dropDownMediaType.ClientID %>'), "keyup", onSearchFilterKeyPress);
            $addHandler($get('<%=dropDownEthnicity.ClientID %>'), "keyup", onSearchFilterKeyPress);
            $addHandler($get('<%=advertiser.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=conAdvertiser.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=agency.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=conAgency.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);            
            $addHandler($get('<%=aeSearch.Name.ClientID %>'),"keyup",onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=mediaFormSearch.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=productClassSearch.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=stationSearch.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);
            $addHandler($get('<%=textProgram.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textStartDateFrom.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textStartDateTo.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textEndDateFrom.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textEndDateTo.ClientID %>'),"keyup",onSearchFilterKeyPress);            
            $addHandler($get('<%=dropDownUploadedBy.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textOriginalName.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=dropDownDocType.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textNotes.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textUploadedFrom.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=textUploadedTo.ClientID %>'),"keyup",onSearchFilterKeyPress);
            $addHandler($get('<%=dropDownPerPageCount.ClientID %>'), "keyup", onSearchFilterKeyPress);            
        }        
        function BindDropDowns(){
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');
            ddlMarket = $get('<%=dropDownMarket.ClientID %>');
            ddlPC = $get('<%=dropDownProfitCenter.ClientID %>');            
            ddlMediaTypes = $get('<%=dropDownMediaType.ClientID %>');
            ddlSubMarket = $get('<%=dropDownSubMarket.ClientID %>');
            ddlProductClasses = $get('<%=dropDownProductClass.ClientID %>');
            ddlEthnicity = $get('<%=dropDownEthnicity.ClientID %>');            
            
            ddlCompanyDefaultVal = $get('<%=dropDownCompanyDefault.ClientID %>').value;
            ddlMarketDefaultVal = $get('<%=dropDownMarketDefault.ClientID %>').value;            
            ddlProfitCenterDefaultVal = $get('<%=dropDownProfitCenterDefault.ClientID %>').value;
            ddlProdClassDefaultVal = $get('<%=dropDownProductClassDefault.ClientID %>').value;
            ddlMediaTypeDefaultVal = $get('<%=dropDownMediaTypeDefault.ClientID %>').value;
            ddlSubMarketDefaultVal = $get('<%=dropDownSubMarketDefault.ClientID %>').value;
            ddlEthnicityDefaultVal = $get('<%=dropDownEthnicityDefault.ClientID %>').value;            
                        
            Apollo.AutoCompleteService.GetCompanies(ddlCompanyDefaultVal,AddToList,null, '<%=dropDownCompany.ClientID %>');
            Apollo.AutoCompleteService.GetMarkets(ddlCompanyDefaultVal, ddlMarketDefaultVal, AddToList,null,'<%=dropDownMarket.ClientID %>');
            Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompanyDefaultVal, ddlMarketDefaultVal, ddlProfitCenterDefaultVal, AddToList,null,'<%=dropDownProfitCenter.ClientID %>');
            Apollo.AutoCompleteService.GetParentProductClasses(ddlCompanyDefaultVal, ddlProdClassDefaultVal, AddToList, null, '<%=dropDownProductClass.ClientID %>');
            Apollo.AutoCompleteService.GetNewMediaTypes(ddlCompanyDefaultVal, ddlMediaTypeDefaultVal, AddToList, null, '<%=dropDownMediaType.ClientID %>');
            Apollo.AutoCompleteService.GetSalesMarket(ddlSubMarketDefaultVal, AddToList, null, '<%=dropDownSubMarket.ClientID %>');
            Apollo.AutoCompleteService.GetEthnicities(ddlEthnicityDefaultVal, AddToList, null, '<%=dropDownEthnicity.ClientID %>');
            
            //Setup default values for popups
            //Filter AEs
            $get('<%=aeSearch.DependencyId.ClientID %>').value = ddlCompanyDefaultVal;

            $get('<%=mediaFormSearch.CompanyId.ClientID %>').valueOf = ddlCompanyDefaultVal;
            $get('<%=productClassSearch.CompanyId.ClientID %>').valueOf = ddlCompanyDefaultVal;
            
            $addHandler(ddlCompany, "change", onCompanyChange);
            $addHandler(ddlMarket, "change", onMarketChange);
            $addHandler(ddlMediaTypes, "change", onMediaTypeChange);
            $addHandler(ddlProductClasses, "change", onProductClassChange);                     
        }
        
        function onCompanyChange(sender, e) {            
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');                            
            //Reset the Market
            Apollo.AutoCompleteService.GetMarkets(ddlCompany.value, '', AddToList, null, '<%=dropDownMarket.ClientID %>');
            //Reset the Profit Center, but ignore the Market
            Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompany.value, '', '', AddToList, null, '<%=dropDownProfitCenter.ClientID %>');
            //Reset the Product Classes
            Apollo.AutoCompleteService.GetParentProductClasses(ddlCompany.value, '', AddToList, null, '<%=dropDownProductClass.ClientID %>');
            Apollo.AutoCompleteService.GetNewMediaTypes(ddlCompany.value, '', AddToList, null, '<%=dropDownMediaType.ClientID %>');
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
            $get('<%=aeSearch.DependencyId.ClientID %>').value = ddlCompany.value;
            $get('<%=aeSearch.DependencyValue.ClientID %>').value = ddlCompany[ddlCompany.selectedIndex].text;
            $get('<%=mediaFormSearch.CompanyId.ClientID %>').value = ddlCompany.value;
            $get('<%=productClassSearch.CompanyId.ClientID %>').valueOf = ddlCompany.value;
            try {
                $find($get('<%=advertiser.BehaviorID.ClientID %>').value)._contextKey = 'companyId:' + ddlCompany.value;
                $find($get('<%=conAdvertiser.BehaviorID.ClientID %>').value)._contextKey = 'companyId:' + ddlCompany.value;
                $find($get('<%=agency.BehaviorID.ClientID %>').value)._contextKey = 'companyId:' + ddlCompany.value;
                $find($get('<%=conAgency.BehaviorID.ClientID %>').value)._contextKey = 'companyId:' + ddlCompany.value;                

                $find($get('<%=aeSearch.BehaviorID.ClientID %>').value)._contextKey =  'companyId:'+ddlCompany.value;            
            } catch(e) {}
        }
        function onMarketChange(sender, e) {
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');
            ddlMarket = $get('<%=dropDownMarket.ClientID %>');            
            Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompany.value, ddlMarket.value,'', AddToList,null,'<%=dropDownProfitCenter.ClientID %>');
            //$get('subMarket').style.display = (ddlMarket.value == 'NYO') ? "block" : "none";
            try {
                $find($get('<%=stationSearch.BehaviorID.ClientID %>').value)._contextKey = 'marketId:' + ddlMediaTypes.value;
            } catch (e) { }
        }               
        function onMediaTypeChange(sender,e){
            ddlMediaTypes = $get('<%=dropDownMediaType.ClientID %>');
            $get('<%=mediaFormSearch.DependencyId.ClientID %>').value = ddlMediaTypes.value;
            try {
                $find($get('<%=mediaFormSearch.BehaviorID.ClientID %>').value)._contextKey = 'mediaFormId:'+ddlMediaTypes.value;            
            } catch(e) {}
        }
        function onProductClassChange(sender, e) {
            ddlProductClasses = $get('<%=dropDownProductClass.ClientID %>');
            $get('<%=productClassSearch.DependencyId.ClientID %>').value = ddlProductClasses.value;
            try {
                $find($get('<%=productClassSearch.BehaviorID.ClientID %>').value)._contextKey = 'parentProductClassId:' + ddlProductClasses.value;
            } catch (e) { }
        }        
        function ExecuteSearch() {
            //Check for Tag Selected or Download Selected            
            if ($get('<%=dlDiv.ClientID %>').style.display!="block"){
                $get('<%=downloadDiv.ClientID %>').style.display = "none";
                $get('<%=tagDiv.ClientID %>').style.display = "none";
                $get('<%=dlDiv.ClientID %>').style.display = "block";
            }                
            $get('<%=isPaging.ClientID %>').value = "0";
            ToggleLoadingImage(true);            
            SaveSearchFilters();
        }        
        function ClearSearchFilters() {
            $get('<%=isPaging.ClientID %>').value = "0";
            ToggleLoadingImage(true);
            $get('<%=textSearch.ClientID %>').value = "";
            $get('<%=textContractNumber.ClientID %>').value = "";
            $get('<%=dropDownCompany.ClientID %>').value = 1;
            $get('<%=dropDownMarket.ClientID %>').value = "";
            $get('<%=dropDownProfitCenter.ClientID %>').value = "";            
            $get('<%=dropDownProductClass.ClientID %>').value = "";
            $get('<%=dropDownMediaType.ClientID %>').value = "";
            $get('<%=dropDownEthnicity.ClientID %>').value = "";
            $get('<%=mediaFormSearch.Id.ClientID %>').value = "";
            $get('<%=mediaFormSearch.Name.ClientID %>').value = "";

            $get('<%=advertiser.Id.ClientID %>').value = "";
            $get('<%=advertiser.Name.ClientID %>').value = "";
            $get('<%=conAdvertiser.Id.ClientID %>').value = "";
            $get('<%=conAdvertiser.Name.ClientID %>').value = "";
            $get('<%=agency.Id.ClientID %>').value = "";
            $get('<%=agency.Name.ClientID %>').value = "";
            $get('<%=conAgency.Id.ClientID %>').value = "";
            $get('<%=conAgency.Name.ClientID %>').value = "";

            $get('<%=aeSearch.Id.ClientID %>').value = "";
            $get('<%=aeSearch.Name.ClientID %>').value = "";
            $get('<%=productClassSearch.Id.ClientID %>').value = "";
            $get('<%=productClassSearch.Name.ClientID %>').value = "";
            $get('<%=stationSearch.Id.ClientID %>').value = "";
            $get('<%=stationSearch.Name.ClientID %>').value = "";                        
            $get('<%=textProgram.ClientID %>').value = "";
            $get('<%=textStartDateFrom.ClientID %>').value = "";
            $get('<%=textStartDateTo.ClientID %>').value = "";
            $get('<%=textEndDateFrom.ClientID %>').value = "";
            $get('<%=textEndDateTo.ClientID %>').value = "";
            $get('<%=dropDownSubMarket.ClientID %>').value = "";
            $get('<%=radioUntagged.ClientID %>').checked = false;
            $get('<%=radioTagged.ClientID %>').checked = true;
            //$get(' < %=radioWebImages.ClientID %>').checked = false;
            $get('<%=checkBestOfPhotos.ClientID %>').checked = false;
            $get('<%=checkPhoto.ClientID %>').checked = false;
            $get('<%=checkHero.ClientID %>').checked = false;
            $get('<%=checkExcludeMTA.ClientID %>').checked = false;
            $get('<%=checkExcludeTwinAmerica.ClientID %>').checked = false;
            $get('<%=dropDownUploadedBy.ClientID %>').value = "";
            $get('<%=textOriginalName.ClientID %>').value = "";
            $get('<%=dropDownDocType.ClientID %>').value = "";
            $get('<%=textNotes.ClientID %>').value = "";
            $get('<%=textUploadedFrom.ClientID %>').value = "";
            $get('<%=textUploadedTo.ClientID %>').value = "";
            $get('<%=dropDownPerPageCount.ClientID %>').value = "20";
            $get('<%=pageNumber.ClientID %>').value = "1";
            SaveSearchFilters();
        }
        function BuildSearchParamsHash(){            
            var hash = new Object();
            hash["textSearch"] = $get('<%=textSearch.ClientID %>').value;
            hash["textContractNumber"] = $get('<%=textContractNumber.ClientID %>').value;
            hash["dropDownCompany"] = $get('<%=dropDownCompany.ClientID %>').value;
            hash["dropDownMarket"] = $get('<%=dropDownMarket.ClientID %>').value;
            //if (hash["dropDownMarket"]=="NYO"){
                hash["dropDownSubMarket"] = $get('<%=dropDownSubMarket.ClientID %>').value;
            //} else {
              //  hash["dropDownSubMarket"] = '';
            //}
            hash["dropDownProfitCenter"] = $get('<%=dropDownProfitCenter.ClientID %>').value;
            hash["dropDownProductClass"] = $get('<%=dropDownProductClass.ClientID %>').value;
            hash["dropDownEthnicity"] = $get('<%=dropDownEthnicity.ClientID %>').value;
            hash["dropDownMediaType"] = $get('<%=dropDownMediaType.ClientID %>').value;
            hash["mediaFormSearch.MediaFormId"] = $get('<%=mediaFormSearch.Id.ClientID %>').value;
            hash["mediaFormSearch.MediaFormName"] = $get('<%=mediaFormSearch.Name.ClientID %>').value;

            hash["radioConsolidated"] = $get("<%=radioConsolidated.ClientID %>").checked;
            hash["radioNonConsolidated"] = $get("<%=radioNonConsolidated.ClientID %>").checked;
            hash["advertiser.AdvertiserId"] = $get('<%=advertiser.Id.ClientID %>').value;
            hash["advertiser.AdvertiserName"] = $get('<%=advertiser.Name.ClientID %>').value;
            hash["conAdvertiser.AdvertiserId"] = $get('<%=conAdvertiser.Id.ClientID %>').value;
            hash["conAdvertiser.AdvertiserName"] = $get('<%=conAdvertiser.Name.ClientID %>').value;
            hash["agency.AgencyId"] = $get('<%=agency.Id.ClientID %>').value;
            hash["agency.AgencyName"] = $get('<%=agency.Name.ClientID %>').value;
            hash["conAgency.AdvertiserId"] = $get('<%=conAgency.Id.ClientID %>').value;
            hash["conAgency.AdvertiserName"] = $get('<%=conAgency.Name.ClientID %>').value;

            hash["aeSearch.AEId"] = $get('<%=aeSearch.Id.ClientID %>').value;
            hash["aeSearch.AEName"] = $get('<%=aeSearch.Name.ClientID %>').value;
            hash["productClassSearch.ProductClassId"] = $get('<%=productClassSearch.Id.ClientID %>').value;
            hash["productClassSearch.ProductClassName"] = $get('<%=productClassSearch.Name.ClientID %>').value;
            hash["stationSearch.StationId"] = $get('<%=stationSearch.Id.ClientID %>').value;
            hash["stationSearch.StationName"] = $get('<%=stationSearch.Name.ClientID %>').value;
            hash["textProgram"] = $get('<%=textProgram.ClientID %>').value;
            hash["textStartDateFrom"] = $get('<%=textStartDateFrom.ClientID %>').value;
            hash["textStartDateTo"] = $get('<%=textStartDateTo.ClientID %>').value;
            hash["textEndDateFrom"] = $get('<%=textEndDateFrom.ClientID %>').value;
            hash["textEndDateTo"] = $get('<%=textEndDateTo.ClientID %>').value;            
            hash["radioUntagged"] = $get('<%=radioUntagged.ClientID %>').checked;
            hash["radioTagged"] = $get('<%=radioTagged.ClientID %>').checked;
            //hash["radioWebImages"] = $get('< % =radioWebImages.ClientID %>').checked;
            hash["checkBestOfPhotos"] = $get('<%=checkBestOfPhotos.ClientID %>').checked;
            hash["checkPhoto"] = $get('<%=checkPhoto.ClientID %>').checked;
            hash["checkHero"] = $get('<%=checkHero.ClientID %>').checked;
            hash["checkExcludeMTA"] = $get('<%=checkExcludeMTA.ClientID %>').checked;
            hash["checkExcludeTwinAmerica"] = $get('<%=checkExcludeTwinAmerica.ClientID %>').checked;
            hash["dropDownUploadedBy"] = $get('<%=dropDownUploadedBy.ClientID %>').value;
            hash["textOriginalName"] = $get('<%=textOriginalName.ClientID %>').value;
            hash["dropDownDocType"] = $get('<%=dropDownDocType.ClientID %>').value;
            hash["textNotes"] = $get('<%=textNotes.ClientID %>').value;
            hash["textUploadedFrom"] = $get('<%=textUploadedFrom.ClientID %>').value;
            hash["textUploadedTo"] = $get('<%=textUploadedTo.ClientID %>').value;     
            hash["dropDownPerPageCount"] = $get('<%=dropDownPerPageCount.ClientID %>').value;                        
            if ($get('<%=isPaging.ClientID %>').value=="0"){                
                $get('<%=pageNumber.ClientID %>').value = "1";
            }
            hash["pageNumber"] = $get('<%=pageNumber.ClientID %>').value;
            return hash;                
        }
        function ToggleLoadingImage(show){
            try {
                $get('<%=loadingImage.ClientID %>').style.display = (show) ? "block" : "none";
            } catch(e) {}
        }
        function SaveSearchFilters() {
            CustomerPreSubmitCheck();
            //Make sure any tooltips are hidden
            checkACField($get('<%=advertiser.Name.ClientID %>'), $get('<%=advertiser.Id.ClientID %>'));
            checkACField($get('<%=conAdvertiser.Name.ClientID %>'), $get('<%=conAdvertiser.Id.ClientID %>'));
            checkACField($get('<%=agency.Name.ClientID %>'), $get('<%=agency.Id.ClientID %>'));
            checkACField($get('<%=conAgency.Name.ClientID %>'), $get('<%=conAgency.Id.ClientID %>'));

            checkACField($get('<%=aeSearch.Name.ClientID %>'),$get('<%=aeSearch.Id.ClientID %>'));
            checkACField($get('<%=mediaFormSearch.Name.ClientID %>'), $get('<%=mediaFormSearch.Id.ClientID %>'));
            checkACField($get('<%=productClassSearch.Name.ClientID %>'), $get('<%=productClassSearch.Id.ClientID %>'));
            checkACField($get('<%=stationSearch.Name.ClientID %>'), $get('<%=stationSearch.Id.ClientID %>'));            
            var hash = BuildSearchParamsHash();                  
            PageMethods.SaveSearchParams(hash);             
            hash["selectedImages"] = $get('<%=selectedImages.ClientID %>').value;    
            hash["previousPageNumber"] = $get('<%=previousPageNumber.ClientID %>').value;
            Apollo.DigitalLibraryService.ExecuteDigitalLibrarySearch(($get('<%=isPaging.ClientID %>').value == "1"), hash, SearchCallback, SearchError);
        }
        function SearchCallback(results){            
            //Make sure the 'loading' image is hidden            
            ToggleLoadingImage(false);            
            //Repaint the grid:                        
            $get('<%=resultsDiv.ClientID %>').innerHTML = results;
            try {
                $('.dlImagePopup').cluetip({ width: 305
                    , cluetipClass: 'rounded'
                    //, closeText: "<img src='/Images/dl/icon_tooltip_close.png' alt='Close' title='Close' style='margin-top:2px !important;position:absolute !important'/>"
                    , dropShadow: false, sticky: false, ajaxCache: false, arrows: true, positionBy: 'bottomTop'
                    , ajaxSettings: { dataType: 'html' }
                    , hoverIntent: {sensitivity:1,interval:100,timeout:0}
                });
                $('.mapIconLink').cluetip({ width: 225, height: 225
				    , showTitle: false
				    , dropShadow: false, sticky: false, ajaxCache: false
				    , ajaxSettings: { dataType: 'html' }
				    , hoverIntent: { sensitivity: 1, interval: 100, timeout: 0 }
                });
            } catch (e) { }
            try {
                // This will create a ShiftClick set of all the checkboxes on a page.
		        $(function() {			        
			        $('.dlImageCell').shiftClickTD();
		        });
            } catch (e){}
            try { setScrollLeft(10); } catch(e) {}
            try { setScrollTop(10); } catch(e) {}
        }
        function SearchError(e){
            //Make sure the 'loading' image is hidden            
            ToggleLoadingImage(false);            
            alert('An error occurred while trying to execute your search:\n' + e._message);
        }        
        function PageBump(pageNumber) {
            $get('<%=isPaging.ClientID %>').value = "1";
            $get('<%=previousPageNumber.ClientID %>').value = $get('<%=pageNumber.ClientID %>').value;
            $get('<%=pageNumber.ClientID %>').value = pageNumber;            
            ToggleLoadingImage(true);            
            SaveSearchFilters();
        }
        function RefreshPage(reload){
            if (reload) {
                $get('<%=isPaging.ClientID %>').value = "0";
            } else {
                $get('<%=isPaging.ClientID %>').value = "1";
            }
            $get('<%=pageNumber.ClientID %>').value = $get('<%=pageNumber.ClientID %>').value;
            $get('<%=previousPageNumber.ClientID %>').value = $get('<%=pageNumber.ClientID %>').value;
            ToggleLoadingImage(true);
            SaveSearchFilters();            
        }  
        function ToggleGroupButtons(){
            var show = ($get('<%=selectedImages.ClientID %>').value != '');
            var isDig = ($get('<%=isPhoto.ClientID %>').value == '1');
            $get('<%=digitalDownload.ClientID %>').style.display = (show) ? 'inline' : 'none';
            $get('<%=digitalTag.ClientID %>').style.display = (show&&isDig) ? 'inline' : 'none';
            $get('<%=digitalDeactivate.ClientID %>').style.display = (show&&isDig) ? 'inline' : 'none';
            $get('<%=digitalClearSelected.ClientID %>').style.display = (show) ? 'inline' : 'none';
            
        }      
        function IsSelected(id) {            
            var selectedImages = $get('<%=selectedImages.ClientID %>').value;
            //if both the id and the contract number are present, return true
            //  This may return false positives            
            return (selectedImages.indexOf(id) != -1);
        }
        function RemoveIdFromSelectedList(id, details) {
            var previousValue = $get('<%=selectedImages.ClientID %>').value;
            var value = id + ":" + details;
            if (previousValue.indexOf(value) != -1) {
                var allValues = previousValue.split(';');
                var newValue = "";
                for (var i = 0; i < allValues.length; i++) {
                    newValue += (allValues[i] != value) ? ((newValue != "") ? ';' : '') + allValues[i] : '';
                }
            }
            $get('<%=selectedImages.ClientID %>').value = newValue;                
            $get('<%=tagSelected.ImageIds.ClientID %>').value = newValue;
        }
        function AddIdToSelectedList(id, details) {            
            var previousValue = $get('<%=selectedImages.ClientID %>').value;
            var value = id + ":" + details;
            if (previousValue.indexOf(id) == -1) {
                $get('<%=selectedImages.ClientID %>').value += ((previousValue == "") ? value : (";" + value));
            }
            $get('<%=tagSelected.ImageIds.ClientID %>').value = $get('<%=selectedImages.ClientID %>').value;
        }
        function ClearSelected() {
            $get('<%=selectedImages.ClientID %>').value = "";
            selectedImages = {};
            try {
                $("input[id^=check]").attr("checked", false);
                $("td[id^=td]").css({ 'border': '1px solid #00B0D8' });
            } catch (e) { }
            return;            
        }
        function DownloadSelected() {            
            var selectedIdList = $get('<%=selectedImages.ClientID %>').value;
            if (selectedIdList == "") { alert('No images have been selected.'); return; }
            //format of Selected ID:
            //  Image ID : Details;
            //  format of details:
            //      contract number - file type - file extension - is tagged - is deleted            
            var selectedIds = selectedIdList.split(';');            
            var selectedKV, id, contractNumber, ext;
            var outputTable = '';
            for (var i = 0; i < selectedIds.length; i++) {
                selectedKV = selectedIds[i].split(':');
                id = selectedKV[0];
                contractNumber = selectedKV[1].split('-')[0];
                ext = selectedKV[1].split('-')[2];                
                outputTable += "<img class='thumbSelected' style='margin:5px;' onclick='HandleImageClick(this,\"" + selectedKV[1] + "\")' src='http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=" + id + "&c=" + contractNumber + "&t=1&x=" + ext + "' alt='" + selectedIds[i] + "' id='" + id + "' />";                
            }            
            $get('<%=downloadTable.ClientID %>').innerHTML = outputTable;
            $get('<%=dlDiv.ClientID %>').style.display = "none";
            $get('<%=downloadDiv.ClientID %>').style.display = "block";
        }
        function DownloadSelectedNew() {
            if (selectedImages === undefined || selectedImages == null || HashLength(selectedImages) == 0) {
                alert("No images have been selected.");
                return;
            }
            var outputTable = "";
            for (var image in selectedImages) {
                var id = selectedImages[image].detailsJson.imageId;
                var contractNumber = selectedImages[image].detailsJson.contractNumber;
                var ext = selectedImages[image].detailsJson.fileExtension;
                outputTable += "<img class='thumbSelected' style='margin:5px;' onclick='HandleImageClickNew(this," + id + ")' src='http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=" + id + "&c=" + contractNumber + "&t=1&x=" + ext + "' alt='" + selectedImages[image].details + "' id='" + id + "' />";
            }
            $get('<%=downloadTable.ClientID %>').innerHTML = outputTable;
            $get('<%=dlDiv.ClientID %>').style.display = "none";
            $get('<%=downloadDiv.ClientID %>').style.display = "block";
        }
        function PrepForDownload() {
            var fileString = "";
            for (var image in selectedImages) {
                if (!selectedImages[image].deselected) {
                    if (fileString != "") { fileString += ";"; }
                    fileString += image + ":" + selectedImages[image].details;
                }
            }
            if (fileString == "") {
                alert("There are no files to download.");
                return false;
            }
            $get('<%=selectedImages.ClientID %>').value = fileString;
            //alert(fileString);
            //return false;
            return true;
        }
        function DownloadBack() {
            $get('<%=dlDiv.ClientID %>').style.display = "block";
            $get('<%=downloadDiv.ClientID %>').style.display = "none";
        }               
        function ContractSearchAutoCompleteSelectHandler(source, eventArgs) {
            var selectedName = eventArgs.get_value();
            var selectedNameSplit = selectedName.split('-');
            var contractNumber = trimValue(selectedNameSplit[0]);
            $get('<%=textContractNumber.ClientID %>').value = contractNumber;
        }
        function StartUpload() {
            //window.location = "digitalLibraryUpload.aspx";
            Apollo.DigitalLibraryService.GetNewBatch(StartUploadCallback);                                       
        }
        function StartUploadCallback(results) {
            $get('<%=dlDiv.ClientID %>').style.display = "none";
            $get('<%=uploadDiv.ClientID %>').style.display = "block";
            try { LoadSWFUploader(results["BATCHID"],results["USERID"]); } catch (e) { }            
        }
        function UploadBack(imagesUploaded, imageIdList) {
            $get('<%=dlDiv.ClientID %>').style.display = "block";
            $get('<%=uploadDiv.ClientID %>').style.display = "none";
            try {
                //Clear out the Thumbnails/File Progress
                $get('divFileProgressContainer').innerHTML = "";
                if ($get('thumbnails').hasChildNodes()) {
                    while ($get('thumbnails').childNodes.length != 0) {
                        $get('thumbnails').removeChild($get('thumbnails').firstChild);
                    }
                }
            } catch (e) { }
            try { swfu = null; } catch (e) { }
            if (imagesUploaded && imageIdList != '') {
                TagSelected(imagesUploaded, imageIdList);
            }
        }
        function PopupImage(imageId, context){
            var details = $get('hdnDetails' + imageId).value;
            PopupImageDetail(imageId, details, context);
        }
        function TagSelectedSaved(taggedIds) {
            var ids = taggedIds.split(';');
            var id, details;
            for (var i = 0; i < ids.length; i++) {
                id = ids[i].split(':')[0];
                details = ids[i].split(':')[1];
                RemoveIdFromSelectedList(id, details);
                $('img.thumbSelected').remove();
            }
        }
        function TagSelected(uploading, selectedIds) {            
            var selectedIdList = selectedIds;
            try {
                if (selectedIdList == '') {
                    selectedIdList = $get('<%=selectedImages.ClientID %>').value;
                } else {
                    //Set the hidden field, so the DeleteSelected function will work
                    //  for uploaded images
                    $get('<%=selectedImages.ClientID %>').value = selectedIds;
                }
                if (selectedIdList == "") { alert('No images have been selected.'); return; }
            } catch(e) {
                alert('Error trying to capture selected Images.\nPlease clear your selection and try again.');//\nDetails:\n\tError Number: '+e.number+'\n\tError Desc: '+e.description);
            }
            try {
                var selectedIds = selectedIdList.split(';');
                var selectedKV, id, contractNumber, ext;
                var idList = '';            
                var outputTable = '';
                for (var i = 0; i < selectedIds.length; i++) {
                    selectedKV = selectedIds[i].split(':');
                    id = selectedKV[0];                
                    if (id!=undefined){
                        ext = (uploading) ? selectedKV[1] : selectedKV[1].split('-')[2];                
                        outputTable += "<img class='thumbSelected' style='margin:5px;' onclick='HandleImageClick(this,\"" + selectedKV[1] + "\")' src='DigitalLibraryImageHandler.ashx?i=" + id + "&t=1&x=" + ext + "' alt='" + selectedIds[i] + "' id='" + id + "' />";                
                        idList += ((idList == '') ? '' : ';') + id;
                    }
                }
            } catch(e) {
                alert('Error trying to build list of selected Images.\nPlease clear your selection and try again.');//\nDetails:\n\tError Number: '+e.number+'\n\tError Desc: '+e.description);
            }
            try {            
                $get('<%=tagSelected.ImageIds.ClientID %>').value = idList;
                $get('<%=tagSelected.TaggerLoad.ClientID %>').click();
                $get('<%=tagTable.ClientID %>').innerHTML = outputTable;
                $get('<%=dlDiv.ClientID %>').style.display = "none";
                $get('<%=tagDiv.ClientID %>').style.display = "block";            
            } catch(e) { 
                alert('Error trying to display Tagger.\nPlease clear your selection and try again.');//\nDetails:\n\tError Number: '+e.number+'\n\tError Desc: '+e.description);
            }
        }
        function TaggerClearSelected(){
            $get('<%=tagSelected.ImageIds.ClientID %>').value = "";
            $get('<%=selectedImages.ClientID %>').value = "";
            selectedImages = {};
            try {
                $("input[id^=check]").attr("checked", false);
                $("td[id^=td]").css({ 'border': '1px solid #00B0D8' });
                $("img[class=thumbSelected]").attr("class", "thumbDeSelected");
            } catch (e) { }
            return;
        }
        function TagBack(hasChanges) {
            $get('<%=dlDiv.ClientID %>').style.display = "block";
            $get('<%=tagDiv.ClientID %>').style.display = "none";
            if (hasChanges) { RefreshPage(true); }
        }
        function DeactivateSelected(){
            if (confirm('Are you sure you want to deactivate the selected images?')){
                Apollo.DigitalLibraryService.DeactivateSelected($get('<%=selectedImages.ClientID %>').value,DeactivateSelectedCallback);
            }
        }
        function DeleteSelected(){
            if (confirm('Are you sure you want to delete the selected images?')){
                var selectedIdList = $get('<%=selectedImages.ClientID %>').value;
                if (selectedIdList == '') {return;}
                var selectedIds = selectedIdList.split(';');
                var id,ext;
                for (var i=0;i<selectedIds.length;i++){
                    id=selectedIds[i].split(':')[0];
                    if (ext=selectedIds[i].split(':')[1].split('-')[2]==undefined){
                        ext=selectedIds[i].split(':')[1];
                    }
                    Apollo.DigitalLibraryService.RemoveUploadedFile(id,ext);
                }
                $get('<%=selectedImages.ClientID %>').value = "";
                try {
                    $("input[id^=check]").attr("checked", false);
                    $("td[id^=td]").css({ 'border': '1px solid #00B0D8' });
                    $('img.thumbSelected').remove();
                } catch (e) { }
                return;
                //ClearSelected();  
                //TagBack(true);   
                RefreshPage(true);      
            }
        }
        function DeactivateSelectedCallback(){ alert('The selected images have been deactivated.'); ClearSelected(); TagBack(false); }
        function ToggleFilters(){ $('#searchCriteriaArea').toggle(); }       
        function MarkAsWebImage(){
            try {
                var imageIds = $get('<%=selectedImages.ClientID %>').value;
                InnerMarkAsWebImage(imageIds);
                //Apollo.DigitalLibraryService.ToggleWebImage(imageIds,MarkAsWebImageCallback,MarkAsWebImageCallbackError);
            } catch(e) { alert('An error occurred while trying to mark your selection(s) as Web Images'); }
        }
        function InnerMarkAsWebImage(imageIds) {
            try {
                $("#unmarkWebImage").show();
                $("#markWebImage").hide();
            } catch (e) { }
            Apollo.DigitalLibraryService.ToggleWebImage(imageIds, MarkAsWebImageCallback, MarkAsWebImageCallbackError);
        }
        function MarkAsWebImageCallback(){ alert('Photo(s) have been marked as Web Images'); }
        function MarkAsWebImageCallbackError(e) { alert('An error occurred while trying to mark your Photo(s) as Web Images:\n' + e._message); }
        function onRadioChange(sender, e) {
            if (sender.target.id.toLowerCase().indexOf('nonconsolidated') == -1) {
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

        $(document).ready(function () {
            $("#jumpToInput").live("keypress", function (e) {
                if (e.keyCode == '13') {
                    var total = parseInt($(this).attr("totalpages"), 10);
                    var val = parseInt(this.value, 10);

                    e.stopPropagation();
                    e.preventDefault();

                    if (isNaN(val)) {
                        alert("Jump to is not a number");
                        return;
                    }
                    if (val > total) {
                        alert("There are only " + total + " pages");
                        return;
                    }
                    if (val == 0) {
                        PageBump(1);
                    } else {
                        PageBump(this.value);
                    }
                }
            });
        });
        
    </script>    
    <asp:HiddenField ID="pageNumber" runat="server" />
    <asp:HiddenField ID="previousPageNumber" runat="server" />
    <asp:HiddenField ID="selectedImages" runat="server" Value="" />
    <asp:HiddenField ID="dropDownCompanyDefault" runat="server" />    
    <asp:HiddenField ID="dropDownMarketDefault" runat="server" />
    <asp:HiddenField ID="dropDownProfitCenterDefault" runat="server" />    
    <asp:HiddenField ID="dropDownProductClassDefault" runat="server" />
    <asp:HiddenField ID="dropDownMediaTypeDefault" runat="server" />    
    <asp:HiddenField ID="dropDownSubMarketDefault" runat="server" />
    <asp:HiddenField ID="dropDownEthnicityDefault" runat="server" />
    <asp:HiddenField ID="isPhoto" runat="server" />
    <div class="inventory_search">SEARCH FILTERS</div>
    <div id="searchCriteriaArea">
        <ul class="searchFilters">
            <li>
                <div>
                    <div class="searchFilterLeftColumn">Search:</div>
                    <div class="searchFilterRightColumn"><asp:TextBox ID="textSearch" runat="server" Width="181px" /></div>
                    <div style="clear:both"></div>
                </div>
            </li>
        </ul>
        <div style="border:1px solid #666666;margin-right:5px;margin-top:2px;">
            <div style="background-color:#ababab;padding-left:2px;cursor:pointer" onclick="TogglePanel('contractSearchFilters')">   
                <img src="../Images/icon_expand_block.gif" alt="Expand" id="contractSearchFiltersPlus" onclick="TogglePanel('contractSearchFilters')" style="display:inline;padding-top:5px;cursor:pointer;" />
                <img src="../Images/icon_expand_none.gif" alt="Collapse" id="contractSearchFiltersMinus"  onclick="TogglePanel('contractSearchFilters')" style="display:none;padding-top:5px;cursor:pointer;" />
                <div style="display:inline;padding-left:5px;padding-bottom:2px">CONTRACT FILTERS</div>
            </div>
            <div id="contractSearchFilters" style="display:block">
                <ul class="searchFilters">
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Contract Number:</div>
                            <div class="searchFilterRightColumn">
                                <asp:TextBox ID="textContractNumber" runat="server" Width="181px" onclick="this.select()" AutoCompleteType="Disabled" autocomplete="off" />
                                <ajax:AutoCompleteExtender ID="textContractNumber_AutoCompleteExtender" OnClientItemSelected="ContractSearchAutoCompleteSelectHandler"
                                    CompletionListItemCssClass="autoCompleteItem" CompletionListHighlightedItemCssClass="autoCompleteItemSelected" CompletionListCssClass="modalAutoComplete" 
                                    runat="server" DelimiterCharacters="" Enabled="True" ServicePath="~/services/AutoCompleteService.asmx" UseContextKey="false" 
                                    TargetControlID="textContractNumber" CompletionInterval="10" ServiceMethod="GetContracts" MinimumPrefixLength="2">
                                </ajax:AutoCompleteExtender>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>                
                        <div>
                            <div class="searchFilterLeftColumn">Company:</div>
                            <div class="searchFilterRightColumn"><asp:DropDownList ID="dropDownCompany" runat="server" Width="185px" /></div>
                            <div style="clear:both"></div>
                        </div>                    
                    </li>
                    <li>                  
                        <div>             
                            <div class="searchFilterLeftColumn">Market:</div>
                            <div class="searchFilterRightColumn">                                
                                <asp:DropDownList ID="dropDownMarket" runat="server" Width="185px" />                                    
                            </div>
                            <div style="clear:both"></div>
                        </div>                   
                    </li>
                    <li id="subMarket">
                        <div>
                            <div class="searchFilterLeftColumn">Sub-Market:</div>
                            <div class="searchFilterRightColumn">                                
                                <asp:DropDownList ID="dropDownSubMarket" runat="server" Width="185px" />                                    
                            </div>
                            <div style="clear:both"></div>
                        </div>                   
                    </li>
                    <li>                  
                        <div>             
                            <div class="searchFilterLeftColumn">Demographic Profile:</div>
                            <div class="searchFilterRightColumn">                                
                                <asp:DropDownList ID="dropDownEthnicity" runat="server" Width="185px" />                                    
                            </div>
                            <div style="clear:both"></div>
                        </div>                   
                    </li>
                    <li>                
                        <div>
                            <div class="searchFilterLeftColumn">Profit Center:</div>
                            <div class="searchFilterRightColumn">                                
                                <asp:DropDownList ID="dropDownProfitCenter" runat="server" Width="185px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>                    
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Station:</div>
                            <div class="searchFilterRightColumn">    
                                <uc:searchPopup ID="stationSearch" runat="server" ServiceMethod="GetStations" 
                                    GridContext="station" AutoCompleteIdIndex="0" AutoCompleteNameIndex="2" 
                                    SearchImageAlt="Search Stations" SearchImageTitle="Search Stations" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>                    
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Parent Prod. Class:</div>
                            <div class="searchFilterRightColumn">                                                        
                                <asp:DropDownList ID="dropDownProductClass" runat="server" Width="185px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>                    
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Product Class:</div>
                            <div class="searchFilterRightColumn">
                                <uc:searchPopup ID="productClassSearch" runat="server" ServiceMethod="GetProductClasses"
                                    GridContext="productClass" AutoCompleteIdIndex="0" AutoCompleteNameIndex="2"
                                    SearchImageAlt="Search Product Classes" SearchImageTitle="Search Product Classes" />                               
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Media Type:</div>
                            <div class="searchFilterRightColumn"> 
                                <asp:DropDownList ID="dropDownMediaType" runat="server" Width="185px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Media Form:</div>
                            <div class="searchFilterRightColumn">           
                                <uc:searchPopup ID="mediaFormSearch" runat="server" ServiceMethod="GetMediaFormsAutoComplete"
                                    GridContext="mediaForm" AutoCompleteIdIndex="0" AutoCompleteNameIndex="2"
                                    SearchImageAlt="Search Media Forms" SearchImageTitle="Search Media Forms" />                                                                                   
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Customer Type:</div>
                            <div class="searchFilterRightColumn">
                                <asp:RadioButton ID="radioConsolidated" runat="server" Checked="true" GroupName="radioCustomerTypeGroup" Text="&nbsp;Consolidated" style="margin-right:3px;" />
                                <asp:RadioButton ID="radioNonConsolidated" runat="server" GroupName="radioCustomerTypeGroup" Text="&nbsp;Non-Consolidated" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Advertiser:</div>
                            <div class="searchFilterRightColumn">  
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
                            <div class="searchFilterLeftColumn">Agency:</div>
                            <div class="searchFilterRightColumn">                                
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
                            <div class="searchFilterLeftColumn">AE:</div>
                            <div class="searchFilterRightColumn">     
                                <uc:searchPopup ID="aeSearch" runat="server" ServiceMethod="GetAEsNonFiltered"
                                    GridContext="ae" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                    SearchImageAlt="Search AEs" SearchImageTitle="Search AEs" />                                                               
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                     
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Program:</div>
                            <div class="searchFilterRightColumn">
                                <asp:TextBox ID="textProgram" runat="server" Width="181px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li> 
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Notes:</div>
                            <div class="searchFilterRightColumn">          
                                <asp:TextBox ID="textNotes" runat="server" Width="181px" />              
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li> 
                     <li>
                        <div>
                            <div class="searchFilterLeftColumn">Exclude MTA:</div>
                            <div class="searchFilterRightColumn">       
                                <asp:CheckBox ID="checkExcludeMTA" runat="server" />                                   
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>   
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Exclude Twin America:</div>
                            <div class="searchFilterRightColumn">       
                                <asp:CheckBox ID="checkExcludeTwinAmerica" runat="server" />                                   
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>   
                </ul>
            </div>
        </div>        
        <br />
        <div style="border:1px solid #666666;margin-right:5px;margin-top:2px;">
            <div style="background-color:#ababab;padding-left:2px;cursor:pointer" onclick="TogglePanel('contractDateFilters')">            
                <img src="../Images/icon_expand_block.gif" alt="Expand" id="contractDateFiltersPlus" onclick="TogglePanel('contractDateFilters')" style="display:inline;padding-top:5px;cursor:pointer;" />
                <img src="../Images/icon_expand_none.gif" alt="Collapse" id="contractDateFiltersMinus"  onclick="TogglePanel('contractDateFilters')" style="display:none;padding-top:5px;cursor:pointer;" />
                <div style="display:inline;padding-left:5px;padding-bottom:2px">CONTRACT DATE FILTERS</div>
            </div>
            <div id="contractDateFilters" style="display:block">
                <ul class="searchFilters">
                    <li>
                        <div>
                            <div class="spanColumnNoColor" style="font-weight:bold">Contract Start Date</div>
                        </div>
                    </li>
                    <li>   
                        <div>
                            <div style="float:left;">
                                From: <asp:TextBox ID="textStartDateFrom" runat="server" Width="65px" />
                                <img src="../Images/calendar.gif" id="calImage1" runat="server" alt="Click for Calendar" class="calendarPopup" />
                                <ajax:CalendarExtender ID="textStartDateTo_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="textStartDateFrom" PopupButtonID="calImage1" />                                
                            </div>
                            <div style="float:left;padding-left:10px">
                                To: <asp:TextBox ID="textStartDateTo" runat="server" Width="65px" />
                                <img src="../Images/calendar.gif" id="calImage2" runat="server" alt="Click for Calendar" class="calendarPopup" />
                                <ajax:CalendarExtender ID="textStartDateFrom_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="textStartDateTo" PopupButtonID="calImage2" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div> 
                    </li>
                    <li>
                        <div>
                            <div class="spanColumnNoColor" style="font-weight:bold">Contract End Date</div>
                        </div>
                    </li>
                    <li>   
                        <div>
                            <div style="float:left;">
                                From: <asp:TextBox ID="textEndDateFrom" runat="server" Width="65px" />
                                <img src="../Images/calendar.gif" id="calImage3" runat="server" alt="Click for Calendar" class="calendarPopup" />
                                <ajax:CalendarExtender ID="textEndDateFrom_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="textEndDateFrom" PopupButtonID="calImage3" />                                
                            </div>
                            <div style="float:left;padding-left:10px">
                                To: <asp:TextBox ID="textEndDateTo" runat="server" Width="65px" />
                                <img src="../Images/calendar.gif" id="calImage4" runat="server" alt="Click for Calendar" class="calendarPopup" />
                                <ajax:CalendarExtender ID="textEndDateTo_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="textEndDateTo" PopupButtonID="calImage4" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div> 
                    </li>
                </ul>
            </div>
        </div>
        <br />
        <div style="border:1px solid #666666;margin-right:5px;margin-top:2px;">
            <div style="background-color:#ababab;padding-left:2px;cursor:pointer" onclick="TogglePanel('contractDocumentFilters')">
                <img src="../Images/icon_expand_block.gif" alt="Expand" id="contractDocumentFiltersPlus" onclick="TogglePanel('contractDocumentFilters')" style="display:inline;padding-top:5px;cursor:pointer;" />
                <img src="../Images/icon_expand_none.gif" alt="Collapse" id="contractDocumentFiltersMinus"  onclick="TogglePanel('contractDocumentFilters')" style="display:none;padding-top:5px;cursor:pointer;" />
                <div style="display:inline;padding-left:5px;padding-bottom:2px">CONTRACT DOCUMENT FILTERS</div>
            </div>
            <div id="contractDocumentFilters" style="display:block">
                <ul class="searchFilters">                     
                    <li>
                        <div style="margin-left:-2px">
                            <div class="spanColumnNoColor">
                                <div style="margin-bottom:5px;">
                                    <div style="float:left;margin-left:2px;"><asp:RadioButton ID="radioUntagged" runat="server" GroupName="Tagged" Text="   Show Untagged" /></div>
                                    <div style="float:left;margin-left:10px;"><asp:RadioButton ID="radioTagged" runat="server" GroupName="Tagged" Text="   Tagged" /></div>
                                    <!--div style="float:left;margin-left:10px;"><asp:RadioButton ID="radioWebImages" runat="server" GroupName="Tagged" Text="   Best of Photos" /></div-->
                                    <div style="clear:both"></div>
                                </div>
                                <div>
                                    <div style="float:left;margin-left:2px;">Show Best Of Photos:</div>
                                    <div style="float:left;margin-left:5px;"><asp:CheckBox ID="checkBestOfPhotos" runat="server" /></div>
                                    <div style="clear:both"></div>
                                </div>
                                <div style="display:none;">
                                    <div style="float:left;margin-left:2px;">Marketing Photo:</div>
                                    <div style="float:left;margin-left:5px;"><asp:CheckBox ID="checkPhoto" runat="server" /></div>
                                    <div style="float:left;margin-left:10px;">Hero:</div>
                                    <div style="float:left;margin-left:5px;"><asp:CheckBox ID="checkHero" runat="server" /></div>
                                    <div style="clear:both"></div>
                                </div>                                
                            </div>
                        </div>
                    </li>                                      
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Uploaded By:</div>
                            <div class="searchFilterRightColumn">          
                                <asp:DropDownList ID="dropDownUploadedBy" runat="server" Width="175px">
                                    <asp:ListItem Selected="True" Value="" Text=" * ALL" />
                                </asp:DropDownList>                                                                                        
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li> 
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Original Name:</div>
                            <div class="searchFilterRightColumn">          
                                <asp:TextBox ID="textOriginalName" runat="server" Width="171px" />              
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li> 
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">Document Type:</div>
                            <div class="searchFilterRightColumn">                                          
                                <asp:DropDownList ID="dropDownDocType" runat="server" Width="175px">
                                    <asp:ListItem Selected="True" Value="" Text=" * ALL" />
                                    <asp:ListItem Value="R" Text="Completion Report" />
                                    <asp:ListItem Value="C" Text="Contract" />
                                    <asp:ListItem Value="I" Text="Photo" />
                                    <asp:ListItem Value="P" Text="Copy Receipt" />
                                    <asp:ListItem Value="M" Text="Manual Invoice" />
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                     
                    <li>
                        <div>
                            <div class="spanColumnNoColor" style="font-weight:bold">Uploaded Date</div>
                        </div>
                    </li>
                    <li>   
                        <div>
                            <div class="searchFilterLeftColumn">From:</div>
                            <div class="searchFilterRightColumn">
                                <asp:TextBox ID="textUploadedFrom" runat="server" Width="125px" />                        
                                <ajax:MaskedEditExtender ID="textUploadedFromMaskedEditExtender" runat="server" TargetControlID="textUploadedFrom"
                                    AcceptAMPM="true" MaskType="DateTime" Mask="99/99/9999 99:99:99" InputDirection="LeftToRight"
                                    AutoComplete="true" />
                            </div>
                            <div style="clear:both;"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="searchFilterLeftColumn">To:</div>                        
                            <div class="searchFilterRightColumn">
                                <asp:TextBox ID="textUploadedTo" runat="server" Width="125px" />
                                <ajax:MaskedEditExtender ID="textUploadedToMaskedEditExtender" runat="server" TargetControlID="textUploadedTo"
                                    AcceptAMPM="true" MaskType="DateTime" Mask="99/99/9999 99:99:99" InputDirection="LeftToRight"
                                    AutoComplete="true" />
                            </div>
                            <div style="clear:both"></div>
                        </div> 
                    </li>
                </ul>
            </div>
        </div>
        <br />
        <ul class="searchFilters">
            <li>
                <div>
                    <div class="searchFilterLeftColumn">Images per Page:</div>
                    <div class="searchFilterRightColumn">
                        <asp:DropDownList ID="dropDownPerPageCount" runat="server" Width="50px">
                            <asp:ListItem Text="20" Value="20" Selected="True" />
                            <asp:ListItem Text="40" Value="40" />
                            <asp:ListItem Text="60" Value="60" />
                            <asp:ListItem Text="80" Value="80" />
                            <asp:ListItem Text="100" Value="100" />
                        </asp:DropDownList>
                    </div>
                    <div style="clear:both"></div>
                </div>
            </li>                                                         
            <li>     
                <div style="text-align:right;width:100%">                    
                    <img id="clear" runat="server" style="cursor:pointer" src="~/Images/but_clear.gif" onclick="ClearSearchFilters();" alt="Clear Search Filters" />
                    <img id="search" runat="server" style="cursor:pointer" src="~/Images/but_search.gif" onclick="ExecuteSearch();" alt="Search" />                                                                                
                    <asp:UpdatePanel ID="foobar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="hiddenSearchButton" runat="server" UseSubmitBehavior="true" CausesValidation="false" style="display:none" OnClientClick="ExecuteSearch();" />
                        </ContentTemplate>
                    </asp:UpdatePanel>                    
                </div>
            </li>
        </ul>        
    </div>
    <br />    
    <script type="text/javascript">
        (function () {
            BindDropDowns();
            radioConsolidated = $get('<%=radioConsolidated.ClientID %>');
            radioNonConsolidated = $get('<%=radioNonConsolidated.ClientID %>');
            $addHandler(radioConsolidated, "click", onRadioChange);
            $addHandler(radioNonConsolidated, "click", onRadioChange);
        })();
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchResultsPanel" Runat="Server">
    <div id="dlDiv" runat="server">
        <div id="errorDiv" class="errorDisplay" style="margin:3px 0;" runat="server"></div>
        <span class="search_filter_title" id="dlPanelTitle">Digital Library Search</span>
        <br />        
        <div style="height:100%;width:100%;text-align:left" id="digitalLibraryResultsArea">                        
            <img src="../Images/dl/but_upload.png" alt="Upload" style="display:inline;margin-right:3px;cursor:pointer;" id="digitalUpload" runat="server" onclick="StartUpload();" />
            <img src="../Images/dl/but_download_selected.png" alt="Download Selected Images" style="display:inline;cursor:pointer;" id="digitalDownload" runat="server" onclick="DownloadSelectedNew();" />
            <img src="../Images/dl/but_tagselected.png" alt="Tag Selected Images" style="display:inline;cursor:pointer;" id="digitalTag" runat="server" onclick="TagSelected(false,'')" />
            <img src="../Images/dl/but_deactivate_selected.png" alt="Deactivate Image" style="display:none;cursor:pointer;" id="digitalDeactivate" runat="server" onclick="DeactivateSelected();" />
            <img src="../Images/dl/but_clear_selection.gif" alt="Clear Selected Images" style="display:inline;cursor:pointer;" id="digitalClearSelected" runat="server" onclick="ClearSelected();" />
            <img src="../Images/dl/but_web_image.png" alt="Mark as Web Image" style="display:none;cursor:pointer;margin-left:15px;" id="webImage" runat="server" onclick="MarkAsWebImage();" />
        </div>
        <br />
        <div style="margin:5px 5px 5px 0;display:none;" id="loadingImage" runat="server"><img src="../Images/pleasewait.gif" alt="Please Wait" /></div>            
        <asp:HiddenField ID="isPaging" runat="server" />
        <div id="resultsDiv" runat="server" style="width:100%;display:block;"></div>        
    </div>    
    <div id="downloadDiv" runat="server" style="display:none;margin-top:4px;">
        <span class="search_filter_title">Download Documents</span><br />
        <img src="../Images/but_back.gif" alt="Back" style="display:inline;cursor:pointer;" id="downloadBack" onclick="DownloadBack()" />        
        <asp:ImageButton ID="downloadImages" runat="server" 
            ImageUrl="~/Images/dl/but_download.png" style="display:inline;cursor:pointer" 
            onclick="downloadImages_Click" OnClientClick="return PrepForDownload();" />        
        <br />
        Image Size: <asp:RadioButton ID="radioOriginal" runat="server" Text=" Original" Checked="true" GroupName="downloadType" />&nbsp;&nbsp;<asp:RadioButton ID="radioEmail" runat="server" Text="  Email" GroupName="downloadType" />
        <br />
        <div id="downloadTable" runat="server" style="width:99%;background-color:#cccccc;"></div>             
    </div>
    <div id="uploadDiv" runat="server" style="display:none;margin-top:4px;">
        <uc:digitalUploader ID="digitalUploader" runat="server" />        
    </div>
    <uc:digitalImageDetail id="digitalImageDetail" runat="server" />
    <div id="tagDiv" runat="server" style="display:none;margin-top:4px;">
        <span class="search_filter_title">Tag Document</span><br />        
        <uc:digitalTagger ID="tagSelected" TaggerContext="TagSelected" runat="server" />            
        <br />
        <div id="tagTable" runat="server" style="width:99%;background-color:#cccccc;"></div>
    </div>
</asp:Content>

