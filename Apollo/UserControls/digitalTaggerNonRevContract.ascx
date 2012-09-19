<%@ Control Language="C#" AutoEventWireup="true" CodeFile="digitalTaggerNonRevContract.ascx.cs" Inherits="Apollo.UserControls_digitalTaggerNonRevContract" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>
    <span class="pagerLink" runat="server" id="addNonRevCont">Add Non Revenue Contract</span>    
    <ajax:ModalPopupExtender ID="nonRevContPopupExt" runat="server" TargetControlID="addNonRevCont"
        PopupControlID="gridPanel" BackgroundCssClass="popupbg" DropShadow="false" CancelControlID="back" />    
    <div ID="gridPanel" runat="server" style="width:625px;border:1px solid #333333;background-color:White;padding:10px;display:none">
        <div style="float:right"><asp:ImageButton ID="back" runat="server" ImageUrl="/Images/dl/icon_tooltip_close.png" style="cursor:pointer;margin:5px 3px;" /></div>
        <div style="clear:both"></div>
        <asp:UpdatePanel ID="nonRevContUpdPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <asp:HiddenField ID="callbackFunctionName" runat="server" />
                    <asp:HiddenField ID="fileId" runat="server" />
                    <asp:HiddenField ID="companyId" runat="server" />
                    <div id="errorDiv" runat="server" class="errorDisplay" style="margin:4px"></div>
                    <div style="float:left;width:48%;">
                        <ul class="searchFilters">
                            <li>
                                <div>
                                    <div class="searchFilterLeftColumn">Country:</div>
                                    <div class="searchFilterRightColumn">
                                        <asp:DropDownList ID="dropDownCompany" runat="server" Width="185px" />                                        
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
                                    <div class="searchFilterLeftColumn">Agency:</div>
                                    <div class="searchFilterRightColumn">
                                        <uc:searchPopup ID="agencySearch" runat="server" ServiceMethod="GetAgenciesDLNonFiltered"
                                            GridContext="consolidatedAgency" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                            SearchImageAlt="Search Agencies" SearchImageTitle="Search Agencies" />
                                    </div>
                                    <div style="clear:both"></div>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <div class="searchFilterLeftColumn">Advertiser:</div>
                                    <div class="searchFilterRightColumn">
                                        <uc:searchPopup ID="advertiserSearch" runat="server" ServiceMethod="GetAdvertisersDLNonFiltered"
                                            GridContext="consolidatedAdvertiser" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
                                            SearchImageAlt="Search Advertisers" SearchImageTitle="Search Advertisers" />                                        
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
                        </ul>
                    </div>            
                    <div style="float:left;width:4%"></div>
                    <div style="float:left;width:48%;">
                        <ul class="searchFilters">                            
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
                                    <div class="searchFilterLeftColumn">Profit Center:</div>
                                    <div class="searchFilterRightColumn">
                                        <asp:DropDownList ID="dropDownProfitCenter" runat="server" Width="185px" />                                        
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
                                    <div class="searchFilterLeftColumn">Quantity:</div>
                                    <div class="searchFilterRightColumn">
                                        <asp:TextBox ID="textQuantity" runat="server" Width="181px" />
                                    </div>
                                </div>                    
                            </li>             
                        </ul>
                    </div>
                    <div style="clear:both"></div>       
                </div>        
                <div style="float:right">
                    <img src="/Images/but_clear.gif" alt="Clear" style="margin-right:5px;cursor:pointer" id="clearContract" runat="server" />
                    <img src="/Images/dl/but_add_contract.gif" alt="Add Non Revenue Contract" id="addContract" style="cursor:pointer" runat="server" />
                </div>
                <div style="clear:both"></div>        
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>