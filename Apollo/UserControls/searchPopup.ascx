<%@ Control Language="C#" AutoEventWireup="true" CodeFile="searchPopup.ascx.cs" Inherits="Apollo.UserControls_searchPopup" %>
<div>
    <div style="float:left">        
        <asp:TextBox ID="textName" onclick="this.select()" runat="server" width="158px" AutoCompleteType="Disabled" autocomplete="off" />                
        <ajax:AutoCompleteExtender ID="textName_AutoCompleteExtender" runat="server"
            CompletionListItemCssClass="autoCompleteItem" CompletionListHighlightedItemCssClass="autoCompleteItemSelected" CompletionListCssClass="modalAutoComplete" 
            DelimiterCharacters="" Enabled="True" ServicePath="~/services/AutoCompleteService.asmx" UseContextKey="true"                   
            TargetControlID="textName" CompletionInterval="10" MinimumPrefixLength="2" EnableCaching="false">
        </ajax:AutoCompleteExtender>
        <asp:TextBox ID="textId" runat="server" style="display:none" />
        <asp:HiddenField ID="dependencyValue" runat="server" Value="" />        
        <asp:HiddenField ID="dependencyId" runat="server" Value="" />        
        <asp:HiddenField ID="companyId" runat="server" Value="1" />
        <asp:HiddenField ID="behaviorID" runat="server" />
        <asp:HiddenField ID="autoCompleteIdIndex" runat="server" />
        <asp:HiddenField ID="autoCompleteNameIndex" runat="server" />
        <asp:HiddenField ID="gridContext" runat="server" />
    </div>
    <div style="float:left;margin-left:5px;">        
        <img id="searchClick" runat="server" src="/Images/but_click.gif" title="" alt="" style="cursor:pointer;padding-top:1px" />        
    </div>
    <div style="clear:both"></div>    
</div>