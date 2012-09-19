<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="attachmentDownload.aspx.cs" Inherits="Apollo.quattro_attachmentDownload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .ColTable td {text-align:left !important;}
        .ui-jqgrid .ui-jqgrid-htable th div {
            height:auto;
            overflow:hidden;
            padding-right:4px;
            padding-top:2px;
            position:relative;
            vertical-align:text-top;
            white-space:normal !important;
        }
        .ui-jqgrid tr.jqgrow td {
            white-space: normal !important;
            height:auto;
            vertical-align:text-top;
            padding-top:2px;
        }
        .fieldLabel {width:100px !important;margin:0 5px 10px 0;float:left;}
        .numericField {text-align:right;}
        #resultsTable td {padding:3px;border-top:1px solid #333333;}
        #gridFilters {padding-left:10px;margin-bottom:25px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
    <script type="text/javascript" language="javascript">
        var searchResultCount;
        function ErrorCallback(e) { endWait(); alert(e._message); }
        $(document).ready(function () {
            $(".button").button();
            Apollo.QuattroService.GetAttachmentTypes(1, AddToList, ErrorCallback, "dropDownAttachmentType");
            $("#dropDownCompany").bind("change", onCompanyChange);
            $("#checkMergeAll").attr("checked", false);
            $("#checkMergeAll").bind("click", onCheckAll);
            searchResultCount = 0;
            //\\QuattroData\KBObjects$
        });
        function onCheckAll(sender, e) {
            $(".pdfCheckBox").attr("checked", $(this).attr("checked"));
        }
        function onCompanyChange(sender, e) {
            if ($("#dropDownCompany").val() == "") { return; }
            Apollo.QuattroService.GetAttachmentTypes(parseInt($("#dropDownCompany").val(), 10), AddToList, ErrorCallback, "dropDownAttachmentType");
        }
        function ValidateInvoiceNumbers() {
            var invoiceNumbers = GetInvoiceNumberList();
            if (invoiceNumbers = '' || invoiceNumbers.length < 1) {
                alert('Please enter Invoice Number(s) before proceeding.');
                return false;
            }
            return true;
        }
        function GetMultipleList(value) {
            var returnVal = "";
            if (value == "") { return ""; }
            if (value.indexOf(",") != -1) {
                returnVal = value.split(",");
            } else if (value.indexOf(";") != -1) {
                returnVal = value.split(";");
            } else {
                returnVal = new Array();
                returnVal.push(value);
            }
            return returnVal;
        }
        function ExecuteAttachmentSearch() {
            var contractNumbers = GetMultipleList(trimValue($("#textContractNumber").val()));
            var invoiceNumbers = GetMultipleList(trimValue($("#textInvoiceNumber").val()));
            if (contractNumbers == "" && invoiceNumbers == "" && $("#textAdvertiser").val() == "") {
                alert("Please enter Contract #(s), Invoice #(s), or an Advertiser name.");
                return;
            }
            if ($("#textAdvertiser").val() != "" && $("#textAdvertiser").val().length <= 3) {
                alert("Please enter at least three characters for the Advertiser name.");
                return;
            }
            var searchParams = { companyId: $("#dropDownCompany").val() };
            if (contractNumbers != "") { searchParams.contractNumber = contractNumbers; }
            if (invoiceNumbers != "") { searchParams.invoiceNumber = invoiceNumbers; }
            if ($("#textAdvertiser").val() != "") { searchParams.advertiser = $("#textAdvertiser").val(); }
            if ($("#dropDownAttachmentType").val() != "") {
                searchParams.uidObjectType = parseInt($("#dropDownAttachmentType").val(), 10);
            }
            startWait("Retrieving Attachments...");
            Apollo.QuattroService.ExecuteAttachmentSearch(searchParams, SearchCallback, ErrorCallback);
        }
        function startWait(msg) { $.blockUI({ theme: true, draggable: false, title: 'Please Wait', message: '<p>' + msg + '...</p>' }); }
        function endWait() { $.unblockUI(); }
        function SearchCallback(searchResults) {
            $(".attachmentRow").remove();
            $("#checkMergeAll").attr("checked", false);
            endWait();
            searchResultCount = searchResults.length;
            $("#noResultsMessage").remove();
            if (searchResults.length == 0) {
                $("#resultsTable").css({ "display": "none" });
                $("#resultsGrid").append("<span id='noResultsMessage'>No results found.</span>");
                $("#mergePDFs").css({ "display": "none" });
                $("#resultsGrid").css({ "display": "block" });
                return;
            }            
            var searchResultsRows = "";
            var attachmentLink = "";
            for (var i = 0; i < searchResults.length; i++) {
                attachmentLink = "/quattro/quattro_attachments" + (searchResults[i].companyId != 1 ? "_toc" : "") + "/" + searchResults[i].objectId + "." + searchResults[i].extension;
                searchResultsRows += "<tr class='attachmentRow'>"
                    //+ "<td style='text-align:center;'>" + (searchResults[i].extension.toLowerCase() == "pdf" ? "<input type='checkbox' class='pdfCheckBox' id='checkPDF_" + searchResults[i].objectId + "' />" : "&nbsp;") + "</td>"
                    + "<td style='text-align:center;'><input type='checkbox' class='pdfCheckBox' id='checkPDF_" + searchResults[i].objectId + "' /></td>"
                    + "<td><a href='" + attachmentLink + "' alt='Download Attachment' target='_blank'>View Attachment</a></td>"
                    + "<td>" + searchResults[i].contractNumber + "</td>"
                    + "<td>" + searchResults[i].type + "</td>"
                    + "<td>" + searchResults[i].name + "</td>"
                    + "<td>" + searchResults[i].description + "</td>"
                    + "<td class='numericField'>" + GetDateAsString(searchResults[i].attachmentDate) + "</td>"
                    + "</tr>";
            }
            $("#resultsTable").append(searchResultsRows);
            $("#resultsTable").css({ "display": "block" });
            $("#mergePDFs").css({ "display": "block" });
            $("#resultsGrid").css({ "display": "block" });
        }
        function MergePDFs() {
            if (searchResultCount == 0) { alert("Please execute a search prior to downloading."); return; }
            var attachmentIDList = "";
            $(".pdfCheckBox").each(function () {
                if ($(this).attr("checked")) {
                    attachmentIDList += (attachmentIDList != "" ? "," : "") + $(this).attr("id").split("_")[1];
                }
            });
            if (attachmentIDList == "") { alert("Please select Attachments to download."); return; }
            $get("<%=companyId.ClientID %>").value = $("#dropDownCompany").val();
            $get("<%=attachmentIDs.ClientID %>").value = attachmentIDList;
            $get("<%=downloadPDFs.ClientID %>").click();
        }
    </script>
    <asp:Button ID="downloadPDFs" runat="server" onclick="downloadPDFs_Click" style="display:none;" />
    <asp:HiddenField ID="attachmentIDs" runat="server" />
    <asp:HiddenField ID="companyId" runat="server" />
    <div style="margin:15px 0 15px 25px;">
        <div style="margin-bottom:15px;width:375px;border:1px solid #333333;padding:10px;">
            <h3 style="margin-bottom:10px;">Search Filters</h3>
            <div class="fieldLabel"><label for="textContractNumber">Contract #(s):</label></div><textarea id="textContractNumber" rows="5" cols="30"></textarea>
            <div style="clear:both;margin-bottom:10px;"></div><div class="fieldLabel"><label for="textInvoiceNumber">Invoice #(s):</label></div><textarea id="textInvoiceNumber" rows="5" cols="30"></textarea>
            <div style="clear:both;margin-bottom:10px;"></div><div style="display:none;"><div class="fieldLabel"><label for="textAdvertiser">Advertiser:</label></div><input type="text" id="textAdvertiser" style="width:150px;" /></div>
            <div style="clear:both;margin-bottom:10px;"></div><div class="fieldLabel"><label for="dropDownCompany">Company:</label></div><select id="dropDownCompany"><option value="1" selected="selected">Titan US</option><option value="2">Titan Canada</option></select>
            <div style="clear:both;"></div>
            <div style="float:left;"><input type="button" class="button" id="mergePDFs" value="Merge &amp; Download PDF Documents" onclick="MergePDFs();" style="display:none;" /></div>
            <input type="button" class="button" id="executeSearch" value="Search" style="margin-right:10px;float:right" onclick="ExecuteAttachmentSearch();" />
            <div style="clear:both;"></div>
        </div>
        <div id="gridFilters">
            <div class="fieldLabel"><label for="dropDownAttachmentType">Attachment Type:</label></div><select id="dropDownAttachmentType" onchange="ExecuteAttachmentSearch();"></select>            
            <div style="clear:both;"></div>
        </div>
        <div id="resultsGrid" style="margin-top:10px;display:none;">
            <table id="resultsTable">
                <thead><tr><th>Merge PDFs<br /><input type="checkbox" id="checkMergeAll" /></th><th>&nbsp;</th><th>Contract #</th><th>Attachment Type</th><th>Name</th><th>Description</th><th>Date Attached</th></tr></thead>
                <tbody></tbody>
            </table>
        </div>
        <br />
        <asp:Label ID="creds" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
</asp:Content>