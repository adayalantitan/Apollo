<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="mtaInvoiceNotes.aspx.cs" Inherits="Apollo.collections_mtaInvoiceNotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
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
    </style>   
    <script type="text/javascript" language="javascript">
        var lastsel, currentRowId;
        $(document).ready(function() {
            //var filterObject = BuildFilterObject($get('< % = textEntryDateFrom.ClientID %>').value, $get('< % = textEntryDateTo.ClientID %>').value, '', '');            
            $("#list1").jqGrid({
                url: '../services/CollectionsService.asmx/GetMTAInvoiceNotesGrid'
                , datatype: "xml"
                , colNames: ['Document #', 'Credited Invoice #', 'Customer #', 'Customer Name', 'Is Multi Market?', 'Contract #'
                    , 'AE 1', 'AE 2', 'AE 3', 'Invoice Date', 'Period From', 'Period To'
                    , 'MTA Net Amount', 'MTA Gross Amount', 'Open Balance', 'Total Titan Amount'
                    , 'Total CBS Amount', 'Passed To Accounts', 'Original GP Invoice Amount', 'GP Pay/Credit Total'
                    , 'Outstanding GP Invoice Balance', 'CBS Remit $$$', 'Document Type Int', 'Document Abbreviation'
                    , 'Payment Number', 'Payment Date', 'Payment Amount', 'Notes', 'User ID', 'Note Date']
                , colModel: [{ name: 'invoiceNumber', index: 'Document Number', width: 80, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center' }
                    , { name: 'creditedInvoiceNumber', index: 'Credited Invoice Number', width: 50, search: true, searchoptions: { sopt: ['eq'] }, sort: true }
                    , { name: 'customerNumber', index: 'Customer Number', width: 80, search: true, searchoptions: { sopt: ['eq'] }, sort: true }
                    , { name: 'customerName', index: 'Customer Name', width: 150, search: true, searchoptions: { sopt: ['eq'] }, sort: true }
                    , { name: 'isMultiMarket', index: 'Is Multi Market', width: 50, search: false, sort: true, searchoptions: { sopt: ['eq'] }, align: 'center' }
                    , { name: 'contractNumber', index: 'Contract Number', width: 80, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center' }
                    , { name: 'ae1', index: 'AE 1', width: 110, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center' }
                    , { name: 'ae2', index: 'AE 2', width: 110, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center', hidden: true }
                    , { name: 'ae3', index: 'AE 3', width: 110, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center', hidden: true }
                    , { name: 'invoiceDate', index: 'Invoice Date', width: 80, search: false, sort: true, align: 'right', formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} }
                    , { name: 'periodFrom', index: 'Period From', width: 80, search: false, sort: true, align: 'right', formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} }
                    , { name: 'periodTo', index: 'Period To', width: 80, search: false, sort: true, align: 'right', formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} }
                    , { name: 'mtaNetAmount', index: 'MTA Net Amount', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'mtaGrossAmount', index: 'MTA Gross Amount', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'openBalance', index: 'Open Balance', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'totalTitanAmount', index: 'Total Titan Amount', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'totalCbsAmount', index: 'Total CBS Amount', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'passedToAccounts', index: 'Passed To Accounts', width: 80, search: false, sort: true, searchoptions: { sopt: ['eq'] }, align: 'right', hidden: true }
                    , { name: 'originalGpInvoiceAmount', index: 'Original GP Invoice Amount', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'gpPayCreditTotal', index: 'GP Pay Credit Total', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'outstandingGpInvoiceBalance', index: 'Outstanding GP Invoice Balance', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'cbsRemitAmount', index: 'CBS Remit Amount', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'documentTypeInt', index: 'Document Type Int', width: 80, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center', hidden: true }
                    , { name: 'documentAbbreviation', index: 'Document Abbreviation', width: 80, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center', hidden: true }
                    , { name: 'paymentNumber', index: 'Payment Number', width: 80, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center' }
                    , { name: 'paymentDate', index: 'Payment Date', width: 80, search: false, sort: true, align: 'right', formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} }
                    , { name: 'paymentAmount', index: 'Payment Amount', width: 80, search: false, sort: true, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'notes', index: 'NOTES', search: true, sort: false, editable: true, edittype: 'textarea', editoptions: { rows: "7", cols: "30"} }
                    , { name: 'userId', index: 'USER_ID', search: false, hidden: true }
                    , { name: 'noteDate', index: 'NOTE_DATE', search: false, hidden: true}]
                , rowNum: 25
                , height: 400
                , width: "100%"
                , rowList: [25, 50, 100, 250, 500, 1000]
                , pager: '#pager1'
                , sortname: 'Document Number'
                , sortorder: 'ASC'
                , caption: "MTA Invoice Notes"
                , viewrecords: true
                , onSelectRow: function(id) {
                    if (id && id !== lastsel) {
                        var extraParams = { invoiceNumber: $('#list1').getRowData(id)['invoiceNumber'] };
                        $('#list1').saveRow(lastsel, null, null, extraParams, afterEdit, null, null);
                        $('#list1').editRow(id, true, null, null, null, extraParams, afterEdit);
                        lastsel = id;
                        currentRowId = id;
                    }
                }
                , editurl: "../services/CollectionsService.asmx/AddMTAInvoiceNote"
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false }).navButtonAdd("#pager1",
                { caption: 'Toggle Columns', onClickButton: function() {
                    $("#list1").setColumns({ caption: 'Show/Hide Columns', bSubmit: 'Update', bCancel: 'Cancel', colnameview: false, drag: false, ShrinkToFit: true }); return false;
                }
                });
        });
        function afterEdit(id,response){
            if (response){
                alert('Entry Saved');
            } else {
                alert('An error occurred while trying to save your Entry.');
            }
        }     
    </script>
    <asp:ImageButton ID="excelExport" runat="server" ImageUrl="/Images/but_excel.gif" AlternateText="Export To Excel" onclick="excelExport_Click" />
    <br />
    <div style="width:95%">
        <div style="width:95%;margin:10px;z-index:-1"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
    <div id="popupFormEntry" style="visibility:hidden;display:none">
        <input type="hidden" id="popupContractNumber" value="" />
        <input type="hidden" id="popupCompanyId" value="" />
    </div>
</asp:Content>

