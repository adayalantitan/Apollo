<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contractSearch.ascx.cs" Inherits="Apollo.UserControls_contractSearch" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>
<div style="float:left">
    <asp:TextBox ID="textTaggerContractNumber" runat="server" onclick="this.select()" autocomplete="off" AutoCompleteType="Disabled" Width="158px" />
    <ajax:AutoCompleteExtender ID="textTaggerContractNumber_AutoCompleteExtender"
        CompletionListItemCssClass="autoCompleteItem" CompletionListHighlightedItemCssClass="autoCompleteItemSelected" CompletionListCssClass="modalAutoComplete" 
        runat="server" DelimiterCharacters="" Enabled="True" ServicePath="~/services/AutoCompleteService.asmx"
        TargetControlID="textTaggerContractNumber" CompletionInterval="10" ServiceMethod="GetContractsWithCountry" MinimumPrefixLength="2">
    </ajax:AutoCompleteExtender>
</div>
<div style="float:left;margin-left:5px;">    
    <img id="searchClick" runat="server" src="/Images/but_click.gif" alt="Search for Contract" align="middle" style="cursor:pointer;padding-top:1px;" />
    <ajax:ModalPopupExtender ID="contractSearchPopupExtender" runat="server" TargetControlID="dummyClick1"
        PopupControlID="gridPanel" BackgroundCssClass="popupbg" DropShadow="true" CancelControlID="closeModal" />               
</div>
<asp:Button ID="dummyClick1" runat="server" style="display:none" />
<div style="clear:both"></div> 
<div ID="gridPanel" runat="server" style="border:1px solid #333333;background-color:White;padding:10px;display:none;z-index:600 !important;">
    <asp:Button ID="dummyClick" runat="server" style="display:none" />
    <div style="float:right"><asp:ImageButton ID="closeModal" runat="server" ImageUrl="/Images/dl/icon_tooltip_close.png" style="cursor:pointer;margin:5px 3px;" /></div>
    <br />
    <div>
        <div id="contractSearchParameters" style="width:100%">
            <div style="float:left;width:48%;">
                <ul class="searchFilters">
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Country:</div>
                            <div style="float:left;width:65%;text-align:left;">                            
                                <asp:DropDownList ID="dropDownCompany" runat="server" Width="185px" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>   
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Market:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <asp:DropDownList ID="dropDownMarket" runat="server" Width="185px" />                            
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Sub Market:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <asp:DropDownList ID="dropDownSubMarket" runat="server" Width="185px" />                            
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Profit Center:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <asp:DropDownList ID="dropDownProfitCenter" runat="server" Width="185px" />                            
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">AE:</div>
                            <div style="float:left;width:65%;text-align:left;">                                
                                <uc:searchPopup ID="aeSearch" runat="server" ServiceMethod="GetAEsNonFiltered"
                                    GridContext="ae" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                    SearchImageAlt="Search AEs" SearchImageTitle="Search AEs" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                                                              
                </ul>
            </div>            
            <div style="float:left;width:4%"></div>
            <div style="float:left;width:48%;text-align:right">
                <ul class="searchFilters"> 
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Advertiser:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <uc:searchPopup ID="advertiserSearch" runat="server" ServiceMethod="GetAdvertisersDLNonFiltered"
                                    GridContext="consolidatedAdvertiser" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                    SearchImageAlt="Search Advertisers" SearchImageTitle="Search Advertisers" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>  
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Agency:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <uc:searchPopup ID="agencySearch" runat="server" ServiceMethod="GetAgenciesDLNonFiltered"
                                    GridContext="consolidatedAgency" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                    SearchImageAlt="Search Agencies" SearchImageTitle="Search Agencies" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                    
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Media Type:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <asp:DropDownList ID="dropDownMediaType" runat="server" Width="185px" />                            
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>       
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Media Form:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <uc:searchPopup ID="mediaFormSearch" runat="server" ServiceMethod="GetMediaFormsAutoComplete"
                                    GridContext="mediaForm" AutoCompleteIdIndex="0" AutoCompleteNameIndex="2"
                                    SearchImageAlt="Search Media Forms" SearchImageTitle="Search Media Forms" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                                              
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Program:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <asp:TextBox ID="textProgram" runat="server" Width="181px" />                                
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                      
                </ul>
            </div>
            <div style="clear:both"></div>
            <div style="width:100%;text-align:center">
                <img src="/Images/but_search.gif" id="executeSearch" runat="server" style="cursor:pointer" alt="search" />             
            </div>        
        </div>      
        <asp:HiddenField ID="contractLoadCallbackFunctionName" runat="server" />           
        <div style="margin:0 auto;">
            <div style="width:95%;margin:20px;">
                <table id="contractSearchGrid" runat="server" class="contractSearchGridTable"></table>
            </div>
            <div id="contractSearchGridPager" runat="server"></div>
        </div>                     
    </div>
</div>