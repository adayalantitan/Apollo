<%@ Control Language="C#" AutoEventWireup="true" CodeFile="digitalTagger.ascx.cs" Inherits="Apollo.UserControls_digitalTagger" %>
<%@ Register Src="~/UserControls/contractSearch.ascx" TagName="contractSearch" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalTaggerNonRevContract.ascx" TagName="nonRevContract" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalTaggerNonRevLine.ascx" TagName="nonRevLine" TagPrefix="uc" %>
<asp:UpdateProgress ID="taggerUpdPnlProgress" AssociatedUpdatePanelID="taggerUpdPnl" runat="server">
    <ProgressTemplate>
        <div style="margin:5px;"><img src="/Images/pleasewait.gif" alt="Please Wait" /></div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="taggerUpdPnl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <input type="button" id="taggerLoad" runat="server" style="display:none" />        
        <input type="button" id="taggerUnload" runat="server" style="display:none" />        
        <img src="/Images/but_delete.gif" alt="Delete" title="Delete" style="display:inline;cursor:pointer;" id="taggerDelete" runat="server" />        
        <img src="/Images/dl/but_deactivate.png" alt="Deactivate" style="display:none;cursor:pointer;" id="taggerDeactivate" runat="server" />        
        <img src="/Images/dl/but_activate.png" alt="Activate Image" style="display:none;cursor:pointer;" id="taggerActivate" runat="server" />        
        <img src="/Images/but_back.gif" alt="Back" style="display:inline;cursor:pointer;" id="taggerBack" onclick="TagBack(false)" runat="server" />        
        <img src="../Images/dl/but_clear_selection.gif" alt="Clear Selected Images" style="display:inline;cursor:pointer;" id="taggerClearSelected" runat="server" onclick="TaggerClearSelected();" />
        <img src="/Images/but_save.gif" alt="Save" title="Save" style="display:inline;cursor:pointer;margin-left:15px;" id="taggerSave" runat="server" />                
        <br />
        <asp:TextBox ID="textImageIds" runat="server" style="display:none" />
        <asp:TextBox ID="textImageId" runat="server" style="display:none" />
        <asp:TextBox ID="textContractNumber" runat="server" style="display:none" />
        <asp:TextBox ID="textExtension" runat="server" style="display:none" />
        <asp:HiddenField ID="taggerDetails" runat="server" />
        <asp:HiddenField ID="lineItemNumbers" runat="server" />  
        <asp:HiddenField ID="companyId" runat="server" />   
        <asp:HiddenField ID="performedTagging" runat="server" />
        <asp:HiddenField ID="isTagged" runat="server" />
        <table cellspacing="1" cellpadding="1" width="100%">
            <tr>
                <td colspan="5" class="infoBoxContentDark" style="padding:2px 10px;"><b>Attributes</b></td>
            </tr>
            <tr style="display:none;">
                <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Photo Quality:</td>
                <td nowrap="nowrap"  class="infoBoxContent" width="30%">
                    <asp:CheckBox ID="checkMarketingQuality" runat="server" Text="&nbsp;Marketing" Checked="false" />&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="checkHeroQuality" runat="server" Text="&nbsp;Hero" Checked="false" />
                </td>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Photo Taken By:</td>
                <td nowrap="nowrap"  class="infoBoxContent" width="30%">
                    <asp:RadioButton ID="radioPhotographer" runat="server" GroupName="RadioTakenBy" Text="&nbsp;Photographer" Checked="true" />&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="radioInstaller" runat="server" GroupName="RadioTakenBy" Text="&nbsp;Installer" />
                </td>                
                <td colspan="3">&nbsp;</td>
            </tr>            
            <tr>
                <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Notes:</td>
                <td nowrap="nowrap"  class="infoBoxContent" colspan="4">
                    <asp:TextBox ID="textNotes" runat="server" TextMode="MultiLine" Rows="4" Width="100%" />
                </td>                
            </tr>      
            <tr>
                <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Document Type:</td>
                <td nowrap="nowrap"  class="infoBoxContent">
                    <asp:DropDownList ID="dropDownDocumentType" runat="server">                        
                        <asp:ListItem Text="Completion Report" Value="R" />
                        <asp:ListItem Text="Contract" Value="C" />
                        <asp:ListItem Selected="True" Text="Photo" Value="I" />
                        <asp:ListItem Text="Copy Receipt" Value="P" />
                    </asp:DropDownList>
                </td>
                <td colspan="3">&nbsp;</td>
            </tr>      
            <tr>
                <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Sales Market Override:</td>
                <td nowrap="nowrap"  class="infoBoxContent" width="30%">
                    <asp:DropDownList ID="dropDownSubMarketOverride" runat="server">
                        <asp:ListItem Selected="True" Value="" Text="" />
                        <asp:ListItem Value="29" Text="Bronx - Bus" Enabled="false" />
                        <asp:ListItem Value="30" Text="Brooklyn - Bus" Enabled="false" />
                        <asp:ListItem Value="58" Text="LI - Bus" Enabled="false" />
                        <asp:ListItem Value="31" Text="Manhattan - Bus" Enabled="false" />
                        <asp:ListItem Value="32" Text="Queens - Bus" Enabled="false" />
                        <asp:ListItem Value="33" Text="Staten Island - Bus" Enabled="false" />
                        <asp:ListItem Value="8888" Text="Double Banners" />
                        <asp:ListItem Value="4444" Text="Illuminated Bus Shelter" />
                        <asp:ListItem Value="9999" Text="Illuminated Phone Kiosk" />
                        <asp:ListItem Value="7777" Text="Interactive Media" />
                        <asp:ListItem Value="5555" Text="Lucy Bus" />
                        <asp:ListItem Value="3333" Text="NFC" />
                        <asp:ListItem Value="6666" Text="Night Photos" />
                        <asp:ListItem Value="2222" Text="Ticker Kiosk" />                        
                    </asp:DropDownList>
                </td>                
                <td nowrap="nowrap" class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Demographic Profiles:</td>
                <td nowrap="nowrap" class="infoBoxContent"><asp:DropDownList ID="dropDownEthnicity" runat="server" /></td>                
                <td>&nbsp;</td>
            </tr>  
            <tr>
                <td nowrap="nowrap" class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Station Override:</td>
                <td nowrap="nowrap" class="infoBoxContent" style="width:95% !important" colspan="4">
                    <div>
                        <div style="float:left;margin-right:10px">
                            <asp:DropDownList ID="dropDownStationMarket" runat="server" Width="100px" />
                        </div>
                        <div style="float:left;">
                            <asp:DropDownList ID="dropDownStation" runat="server" Width="175px" />
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="5" class="infoBoxContentDark" style="padding:2px 10px;"><b>Tag Information</b></td>
            </tr>
        </table>
        <table cellspacing="1" cellpadding="1" width="100%">        
            <tr>
                <td nowrap="nowrap" class="infoBoxContentDark" style="padding:2px 20px;width:10%;vertical-align:top">Contract Number:</td>
                <td nowrap="nowrap" class="infoBoxContent" valign="top" style="width:30%">
                    <div id="selectedContractNumber" runat="server"></div>
                    <div id="contractLookupSection" runat="server" style="vertical-align:middle;line-height:inherit;">                        
                        <uc:contractSearch ID="contractSearch" runat="server" />                                
                    </div>                    
                    <span class="pagerLink" style="padding-left:10px" id="unTagLink" runat="server">Un-tag</span>
                </td>
                <td nowrap="nowrap"  class="infoBoxContent" style="padding:2px 20px;width:15%;">
                    <span id="addNonRevLink" runat="server"><uc:nonRevContract ID="nonRevContract" runat="server" /></span>
                </td>
                <td nowrap="nowrap"  class="infoBoxContent" colspan="2">&nbsp;</td>               
            </tr>
        </table> 
        <div id="tagDetailsTable" runat="server">        
            <table cellspacing="1" cellpadding="1" width="100%">
                <tr>
                    <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;">Country:</td>
                    <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="country" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContentDark" style="padding:2px 20px;width:15%;" width="15%">AE 1</td>
                    <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="ae1Value" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContent">&nbsp;</td>                
                </tr>      
                <tr>
                    <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;">Agency:</td>
                    <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="agency" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContentDark" style="padding:2px 20px;width:15%;" width="15%"><div id="ae2Label" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="ae2Value" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContent">&nbsp;</td>               
                </tr>      
                <tr>
                    <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;">Advertiser:</td>
                    <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="advertiser" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContentDark" style="padding:2px 20px;width:15%;" width="15%"><div id="ae3Label" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="ae3Value" runat="server"></div></td>
                    <td nowrap="nowrap" class="infoBoxContent">&nbsp;</td>                
                </tr>            
                <tr>
                    <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;vertical-align:middle;">Contract<br />Lines:</td>
                    <td nowrap="nowrap" class="infoBoxContent" colspan="4">
                        <div id="taggerContractLines" runat="server" style="display:block" class="taggerContractLinesTable"></div>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap"  class="infoBoxContentDark" style="padding:2px 20px;width:10%;">
                        <div id="addNonRevLines" runat="server"><uc:nonRevLine id="nonRevLine" runat="server" /></div>
                    </td>
                    <td nowrap="nowrap"  class="infoBoxContent" colspan="4">&nbsp;</td>
                </tr>            
            </table>   
        </div>
    </ContentTemplate>
</asp:UpdatePanel>