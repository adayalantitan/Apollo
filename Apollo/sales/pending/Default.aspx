<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Apollo.sales_pending_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            alert("As of 11/28/2011, Pending entries will no longer be made in Apollo.\n\nPlease use the new Pending screens in the CRM system.\n\nYou will be redirected after clicking OK.");
            window.location = "http://crm.titan360.com/Pending";
            return;
        });
    </script>
    <asp:HiddenField ID="isProd" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">    
</asp:Content>

