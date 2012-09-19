<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchVertSplit.master" AutoEventWireup="true" CodeFile="station_images.aspx.cs" Inherits="Apollo.digital_station_images" EnableEventValidation="false" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="stationSearchPopup" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalImageDetail.ascx" TagName="digitalImageDetail" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="searchCriteriaPanel" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            RegisterSearchFilterKeyPressHandler();    
            $('.dlImagePopup').cluetip({ width: 305
                , cluetipClass: 'rounded'                
                , dropShadow: false, sticky: false, ajaxCache: false, arrows: true, positionBy: 'bottomTop'
                , ajaxSettings: { dataType: 'html' }
                , hoverIntent: { sensitivity: 1, interval: 100, timeout: 0 }
            });
            ddlCompanyDefaultVal = $get('<%=dropDownCompanyDefault.ClientID %>').value;
            ddlMarketDefaultVal = $get('<%=dropDownMarketDefault.ClientID %>').value;                        
            try { $('#aspnetForm').submit(function(event) { if (event.keyCode == 13) { return false; } }); } catch (e) { }
        });
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
                setTimeout(function() { enterPressedTwice = false; }, 500);
            }
        }
        function RegisterSearchFilterKeyPressHandler() { $addHandler($get('<%=stationSearch.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress); }    
        function BindDropDowns() {
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');
            ddlMarket = $get('<%=dropDownMarket.ClientID %>');                        

            ddlCompanyDefaultVal = $get('<%=dropDownCompanyDefault.ClientID %>').value;
            ddlMarketDefaultVal = $get('<%=dropDownMarketDefault.ClientID %>').value;            

            Apollo.AutoCompleteService.GetCompanies(ddlCompanyDefaultVal, AddToList, null, '<%=dropDownCompany.ClientID %>');
            Apollo.AutoCompleteService.GetMarkets(ddlCompanyDefaultVal, ddlMarketDefaultVal, AddToList, null, '<%=dropDownMarket.ClientID %>');                        

            $addHandler(ddlCompany, "change", onCompanyChange);
            $addHandler(ddlMarket, "change", onMarketChange);
        }
        function onCompanyChange(sender, e) {
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');
            //Reset the Market
            Apollo.AutoCompleteService.GetMarkets(ddlCompany.value, '', AddToList, null, '<%=dropDownMarket.ClientID %>');            
        }
        function onMarketChange(sender, e) {
            ddlCompany = $get('<%=dropDownCompany.ClientID %>');
            ddlMarket = $get('<%=dropDownMarket.ClientID %>');
            try {
                $find($get('<%=stationSearch.BehaviorID.ClientID %>').value)._contextKey = 'marketId:' + ddlMediaTypes.value;
            } catch (e) { }
        }
        function ExecuteSearch() {
            //Check for Tag Selected or Download Selected            
            if ($get('<%=dlDiv.ClientID %>').style.display != "block") {
                $get('<%=downloadDiv.ClientID %>').style.display = "none";                
                $get('<%=dlDiv.ClientID %>').style.display = "block";
            }
            $get('<%=isPaging.ClientID %>').value = "0";
            ToggleLoadingImage(true);
            SaveSearchFilters();
        }   
        function ClearSearchFilters() {
            $get('<%=isPaging.ClientID %>').value = "0";
            ToggleLoadingImage(true);            
            $get('<%=dropDownCompany.ClientID %>').value = 1;
            $get('<%=dropDownMarket.ClientID %>').value = "";
            $get('<%=stationSearch.Id.ClientID %>').value = "";
            $get('<%=stationSearch.Name.ClientID %>').value = "";                        
            $get('<%=dropDownPerPageCount.ClientID %>').value = "20";
            $get('<%=pageNumber.ClientID %>').value = "1";
            SaveSearchFilters();
        }
        function BuildSearchParamsHash() {
            var hash = new Object();            
            hash["dropDownCompany"] = $get('<%=dropDownCompany.ClientID %>').value;
            hash["dropDownMarket"] = $get('<%=dropDownMarket.ClientID %>').value;
            hash["stationSearch.Id"] = $get('<%=stationSearch.Id.ClientID %>').value;
            hash["stationSearch.Name"] = $get('<%=stationSearch.Name.ClientID %>').value;
            hash["dropDownPerPageCount"] = $get('<%=dropDownPerPageCount.ClientID %>').value;
            if ($get('<%=isPaging.ClientID %>').value == "0") {
                $get('<%=pageNumber.ClientID %>').value = "1";
            }
            hash["pageNumber"] = $get('<%=pageNumber.ClientID %>').value;
            return hash;
        }
        function ToggleLoadingImage(show) {
            try {
                $get('<%=loadingImage.ClientID %>').style.display = (show) ? "block" : "none";
            } catch (e) { }
        }
        function SaveSearchFilters() {
            checkACField($get('<%=stationSearch.Name.ClientID %>'), $get('<%=stationSearch.Id.ClientID %>'));            
            var hash = BuildSearchParamsHash();            
            PageMethods.SaveSearchParams(hash);
            hash["selectedImages"] = $get('<%=selectedImages.ClientID %>').value;
            hash["previousPageNumber"] = $get('<%=previousPageNumber.ClientID %>').value;            
            Apollo.DigitalLibraryService.ExecuteStationImageSearch(($get('<%=isPaging.ClientID %>').value == "1"), hash, SearchCallback, SearchError);
        }
        function SearchCallback(results) {
            //Make sure the 'loading' image is hidden            
            ToggleLoadingImage(false);
            //Repaint the grid:                        
            $get('<%=resultsDiv.ClientID %>').innerHTML = results;
            try {
                $('.dlImagePopup').cluetip({ width: 305
                    , cluetipClass: 'rounded'                    
                    , dropShadow: false, sticky: false, ajaxCache: false, arrows: true, positionBy: 'bottomTop'
                    , ajaxSettings: { dataType: 'html' }
                    , hoverIntent: { sensitivity: 1, interval: 100, timeout: 0 }
                });
            } catch (e) { }
            try {
                // This will create a ShiftClick set of all the checkboxes on a page.
                $(function() {
                    $('.dlImageCell').shiftClickTD();
                });
            } catch (e) { }
            try { setScrollLeft(10); } catch (e) { }
            try { setScrollTop(10); } catch (e) { }
        }
        function SearchError(e) {
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
        function RefreshPage(reload) {
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
        }
        function AddIdToSelectedList(id, details) {
            var previousValue = $get('<%=selectedImages.ClientID %>').value;
            var value = id + ":" + details;
            if (previousValue.indexOf(id) == -1) {
                $get('<%=selectedImages.ClientID %>').value += ((previousValue == "") ? value : (";" + value));
            }            
        }
        function ClearSelected() {
            $get('<%=selectedImages.ClientID %>').value = "";
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
            //      station name - file type - file extension - is tagged - is deleted            
            var selectedIds = selectedIdList.split(';');
            var selectedKV, id, contractNumber, ext;
            var outputTable = '';
            for (var i = 0; i < selectedIds.length; i++) {
                selectedKV = selectedIds[i].split(':');
                id = selectedKV[0];
                contractNumber = selectedKV[1].split('-')[0];
                ext = selectedKV[1].split('-')[2];
                outputTable += "<img class='thumbSelected' style='margin:5px;' onclick='HandleImageClick(this,\"" + selectedKV[1] + "\")' src='DigitalLibraryImageHandler.ashx?i=" + id + "&t=1&x=" + ext + "' alt='" + selectedIds[i] + "' id='" + id + "' />";
            }
            $get('<%=downloadTable.ClientID %>').innerHTML = outputTable;
            $get('<%=dlDiv.ClientID %>').style.display = "none";
            $get('<%=downloadDiv.ClientID %>').style.display = "block";
        }
        function DownloadBack() {
            $get('<%=dlDiv.ClientID %>').style.display = "block";
            $get('<%=downloadDiv.ClientID %>').style.display = "none";
        }
        function PopupImage(imageId, context) {
            var details = $get('hdnDetails' + imageId).value;
            PopupImageDetail(imageId, details, context);
        }
    </script>
    <asp:HiddenField ID="pageNumber" runat="server" />
    <asp:HiddenField ID="previousPageNumber" runat="server" />
    <asp:HiddenField ID="selectedImages" runat="server" Value="" />
    <asp:HiddenField ID="dropDownCompanyDefault" runat="server" />    
    <asp:HiddenField ID="dropDownMarketDefault" runat="server" />        
    <asp:HiddenField ID="isPhoto" runat="server" />
    <div class="inventory_search">SEARCH FILTERS</div>
    <div id="searchCriteriaArea">                
        <div id="contractSearchFilters" style="display:block">
            <ul class="searchFilters">
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
               <li>
                    <div>
                        <div class="searchFilterLeftColumn">Station:</div>
                        <div class="searchFilterRightColumn">
                            <uc:stationSearchPopup ID="stationSearch" runat="server" ServiceMethod="GetStations" GridContext="station" AutoCompleteIdIndex="0" AutoCompleteNameIndex="2" SearchImageAlt="Search Stations" SearchImageTitle="Search Stations" />                            
                        </div>
                        <div style="clear:both"></div>
                    </div>                    
                </li>
            </ul>
        </div>        
        <br />
        <ul class="searchFilters">
            <li>
                <div>
                    <div class="searchFilterLeftColumn">Images per Page:</div>
                    <div class="searchFilterRightColumn">
                        <asp:DropDownList ID="dropDownPerPageCount" runat="server" Width="50px">
                            <asp:ListItem Text="20" Value="20" Selected="True" /><asp:ListItem Text="40" Value="40" />
                            <asp:ListItem Text="60" Value="60" /><asp:ListItem Text="80" Value="80" />
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
    <script type="text/javascript" language="javascript">
        BindDropDowns();        
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchResultsPanel" Runat="Server">
    <div id="dlDiv" runat="server">
        <div id="errorDiv" class="errorDisplay" style="margin:3px 0;" runat="server"></div>
        <span class="search_filter_title" id="dlPanelTitle">Digital Library Search</span>
        <br />        
        <div style="height:100%;width:100%;text-align:left" id="digitalLibraryResultsArea">                                    
            <img src="../Images/dl/but_download_selected.png" alt="Download Selected Images" style="display:inline;cursor:pointer;" id="digitalDownload" runat="server" onclick="DownloadSelected();" />            
            <img src="../Images/dl/but_clear_selection.gif" alt="Clear Selected Images" style="display:inline;cursor:pointer;" id="digitalClearSelected" runat="server" onclick="ClearSelected();" />
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
            onclick="downloadImages_Click" />        
        <br />
        Image Size: <asp:RadioButton ID="radioOriginal" runat="server" Text=" Original" Checked="true" GroupName="downloadType" />&nbsp;&nbsp;<asp:RadioButton ID="radioEmail" runat="server" Text="  Email" GroupName="downloadType" />
        <br />
        <div id="downloadTable" runat="server" style="width:99%;background-color:#cccccc;"></div>             
    </div>
    <uc:digitalImageDetail id="digitalImageDetail" runat="server" />
</asp:Content>

