<%@ Page Title="Contract Posting Summary | Titan 360" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="postingSummaryByContract.aspx.cs" Inherits="Apollo.quattro_postingSummaryByContract" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .button{}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">        
    <script type="text/javascript" language="javascript">
        function SubmitContractNumber() {
            if ($("#textLookupText").val() == "") { alert("Please enter a search value."); return; }            
            $("#resultArea").css({ "display": "block" });
            $get("<%=searchText.ClientID %>").value = $("#textLookupText").val();
        
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
      
            <div style="float:left;margin:3px 10px 0 0;">
                <label for="textLookupText" style="margin-right:5px;">Contract Number:</label>
                <input type="text" id="textLookupText" style="width:150px;" />
            </div>
          
   
  
            <div style="float:left;">                
                <asp:Button ID="executeReport" runat="server" Text="Get Report" 
                    CssClass="button" onclick="executeReport_Click" />
            </div> 
             
              <div style="clear:both;"></div>       
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
</asp:Content>

