<%@ Page Title="Billings, Credits, &amp; Payments by Account | Titan 360" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="billingCreditsPaymentsByAccount.aspx.cs" Inherits="Apollo.collections_billingCreditsPaymentsByAccount" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .button{}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">        
    <script type="text/javascript" language="javascript">
        function CustomerSearch() {
            if ($("#textLookupText").val() == "") { alert("Please enter a search value."); return; }            
            Apollo.AutoCompleteService.GetCustomerList($("#dropDownCompany").val(), $("#dropDownLookupType").val(), $("#textLookupText").val(), "", AddToList, null, "<%=dropDownSearchResults.ClientID %>");
            $("#resultArea").css({ "display": "block" });
            $get("<%=lookupType.ClientID %>").value = $("#dropDownLookupType").val();
            $get("<%=searchText.ClientID %>").value = $("#textLookupText").val();
            $get("<%=company.ClientID %>").value = $("#dropDownCompany").val();
        }
        function onKeyPress(event) {            
            if (event.keyCode == 13) {
                event.cancelBubble = true;
                event.preventDefault();
                CustomerSearch();
                return;
            }
        }        
        $(document).ready(function() {
            $(".button").button();
            $("#resultArea").css({ "display": "none" });
            $("#textLookupText").bind("keyup keydown keypress", onKeyPress);
        });
    </script>
    <asp:HiddenField ID="lookupType" runat="server" />
    <asp:HiddenField ID="searchText" runat="server" />
    <asp:HiddenField ID="company" runat="server" />
    <div style="margin-left:25px;">
        <div>
            <div style="float:left;margin:3px 10px 0 0;">
                <label for="dropDownCompany">Company:</label>
                <select id="dropDownCompany">
                    <option value="1" selected="selected">Titan US</option>
                    <option value="2">Titan Canada</option>
                </select>
            </div>
            <div style="float:left;margin:3px 10px 0 0;">
                <label for="dropDownLookupType" style="margin-right:5px;">Lookup Customer By:</label>
                <select id="dropDownLookupType">
                    <option value="name" selected="selected">Name</option>
                    <option value="id">ID</option>
                    <option value="contractnumber">Contract #</option>
                    <option value="invoice">Invoice #</option>
                </select>
            </div>
            <div style="float:left;margin:3px 10px 0 0;">
                <label for="textLookupText" style="margin-right:5px;">Search Text:</label>
                <input type="text" id="textLookupText" style="width:150px;" />
            </div>
            <div style="float:left;">
                <input type="button" class="button" value="Find Customers" id="findCustomers" onclick="CustomerSearch();" />
            </div>
            <div style="clear:both;"></div>        
        </div>
        <div style="margin-top:10px;display:none;" id="resultArea">
            <div style="float:left;margin:3px 10px 0 0;">
                <asp:DropDownList ID="dropDownSearchResults" runat="server" />                
            </div>  
            <div style="float:left;">                
                <asp:Button ID="executeReport" runat="server" Text="Get Report" 
                    CssClass="button" onclick="executeReport_Click" />
            </div>      
        </div>        
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
</asp:Content>

