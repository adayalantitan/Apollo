<%@ Control Language="C#" AutoEventWireup="true" CodeFile="autoCompleteControl.ascx.cs" Inherits="Apollo.UserControls_autoCompleteControl" %>
<div id="autoCompleteContainer" runat="server">
    <input type="text" onclick="this.select();" />
    <input type="hidden" value="" />
</div>