<%@ Control Language="C#" AutoEventWireup="true" CodeFile="digitalTaggerNonRevLine.ascx.cs" Inherits="Apollo.UserControls_digitalTaggerNonRevLine" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>
    <span class="pagerLink" runat="server" id="addLine">Add Line Item</span>    
    <ajax:ModalPopupExtender ID="nonRevLinePopupExt" runat="server" TargetControlID="addLine"
        PopupControlID="gridPanel" BackgroundCssClass="popupbg" DropShadow="false" CancelControlID="back" />    
    <div ID="gridPanel" runat="server" style="width:625px;border:1px solid #333333;background-color:White;padding:10px;display:none">
        <div style="float:right"><asp:ImageButton ID="back" runat="server" ImageUrl="/Images/dl/icon_tooltip_close.png" style="cursor:pointer;margin:5px 3px;" OnClientClick="HandleClose(this)" /></div>
        <div style="clear:both"></div>
        <asp:UpdatePanel ID="nonRevLineUpdPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="callbackFunctionName" runat="server" />
                <asp:HiddenField ID="contractNumber" runat="server" />
                <asp:HiddenField ID="companyId" runat="server" />
                <asp:HiddenField ID="fileId" runat="server" />
                <div id="errorDiv" runat="server" class="errorDisplay" style="margin:4px"></div>
                <table style="width:100%;" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>Media Type:</td>
                        <td>
                            <asp:DropDownList ID="dropDownMediaType" runat="server" Width="160px" />                                    
                        </td>
                        <td>&nbsp;</td>
                        <td>Media Form:</td>
                        <td colspan="2">
                            <uc:searchPopup ID="mediaFormSearch" runat="server" ServiceMethod="GetMediaFormsAutoComplete"
                                GridContext="mediaForm" AutoCompleteIdIndex="0" AutoCompleteNameIndex="2"
                                SearchImageAlt="Search Media Forms" SearchImageTitle="Search Media Forms" />                            
                        </td>                                                      
                        <td style="width:5px">&nbsp;</td>                        
                        <td>&nbsp;</td>
                    </tr>
                    <tr>           
                        <td>Profit Center:</td>
                        <td>
                            <asp:DropDownList ID="dropDownProfitCenter" runat="server" Width="160px" />
                        </td>             
                        <td>Quantity:</td>
                        <td><asp:TextBox ID="textQuantity" runat="server" Width="40px" /></td>
                        <td>Reason:</td>
                        <td>
                            <asp:DropDownList ID="dropDownReason" runat="server" Width="100px">
                                <asp:ListItem Enabled="true" Selected="True" Text="Bonus" Value="B" />
                                <asp:ListItem Enabled="true" Text="Additional Line" Value="A" />
                            </asp:DropDownList>
                        </td>
                        <td style="width:5px">&nbsp;</td>
                        <td><img src="/Images/dl/but_add_line_item.gif" alt="Add Line Item" id="saveLine" runat="server" /></td>
                    </tr>
                </table>                
            </ContentTemplate>                
        </asp:UpdatePanel>
    </div>    