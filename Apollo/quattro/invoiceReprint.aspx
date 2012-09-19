<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="invoiceReprint.aspx.cs" Inherits="Apollo.quattro_invoiceReprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
        function ValidateInvoiceNumbers() {
            var invoiceNumbers = GetInvoiceNumberList();
            if (invoiceNumbers = '' || invoiceNumbers.length < 1) {
                alert('Please enter Invoice Number(s) before proceeding.');
                return false;
            }
            return true;
        }
        function GetInvoiceNumberList() {
            var invoiceNumbers;
            var textInvoiceNumberVal = trimValue($get('<%=textInvoiceNumbers.ClientID %>').value).toUpperCase().replace("NYO-", "").replace("TOR-", "");
            if (textInvoiceNumberVal == '') { return ''; }
            if (textInvoiceNumberVal.indexOf(',') != -1) {
                invoiceNumbers = textInvoiceNumberVal.split(',');
            } else if (textInvoiceNumberVal.indexOf(';') != -1) {
                invoiceNumbers = textInvoiceNumberVal.split(';');
            } else {
                invoiceNumbers = new Array();
                invoiceNumbers.push(textInvoiceNumberVal);
            }
            return invoiceNumbers;
        }
        function ContractInvoiceLookup() {
            if ($get('<%=textContractInvoiceLookup.ClientID %>').value == '') {
                alert('Please enter a Contract Number.');
                return;
            }
            var contractNumber = $get('<%=textContractInvoiceLookup.ClientID %>').value;
            var companyId = $get('<%=dropDownCompany.ClientID %>').value;
            Apollo.QuattroService.GetInvoicesByContract(contractNumber, companyId, ContractInvoiceLookupCallback);
        }
        function ContractInvoiceLookupCallback(invoiceNumberList) {
            if (invoiceNumberList == '') {
                alert('No Invoices could be found for that Contract.');
                return;
            }
            var currentList = $get('<%=textInvoiceNumbers.ClientID %>').value;
            if (currentList == '') {
                currentList = invoiceNumberList;
            } else {
                currentList += "," + invoiceNumberList;
            }
            $get('<%=textInvoiceNumbers.ClientID %>').value = currentList;
        }
    </script>
    <div id="work" style="margin:50px;">
        <div style="margin-bottom:20px;">
            <h5>Instructions:</h5>
            <ul style="margin:5px 0 5px 30px">
                <li>Choose the Company to reprint Invoice(s) for.</li>
                <li>
                    Enter a single Invoice Number or comma (or semi-colon) delimited list of Invoice Numbers for printing.
                </li>
                <li>
                    Enter a Contract Number and click 'Lookup Invoices for Contract' and<br />
                    a list of all Invoice Numbers will be retrieved for the specified Contract.
                </li>
            </ul>            
        </div>
        <div>
            <div style="float:left;">Choose Company:</div>
            <div style="float:left;margin-left:34px;">
                <asp:DropDownList ID="dropDownCompany" runat="server">
                    <asp:ListItem Selected="True" Value="1">Titan US</asp:ListItem>
                    <asp:ListItem Value="2">Titan Canada</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="clear:both;"></div>
        </div>
        <br />
        <div>            
            <div style="float:left">Enter Contract Number:</div>
            <div style="float:left;margin-left:10px;">
                <asp:TextBox ID="textContractInvoiceLookup" runat="server" Width="100px" />
            </div>
            <div style="float:left;margin-left:10px;">
                <input type="button" id="buttonContractLookup" onclick="ContractInvoiceLookup();" value="Lookup Invoices For Contract" />
            </div>
            <div style="clear:both"></div>
        </div>                                
        <br />
        <div>
            <div style="float:left">Enter Invoice Numbers:</div>
            <div style="float:left;margin-left:10px;">
                <asp:TextBox ID="textInvoiceNumbers" runat="server" TextMode="MultiLine" Rows="5" Columns="40" />
            </div>
            <div style="clear:both"></div>
        </div>                                
        <br />
        <asp:Button ID="buttonServerPrint" runat="server" Text="Print Invoices" onclick="buttonServerPrint_Click" OnClientClick="return ValidateInvoiceNumbers();"  />
    </div>
</asp:Content>

