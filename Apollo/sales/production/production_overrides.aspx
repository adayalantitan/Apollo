<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="production_overrides.aspx.cs" Inherits="Apollo.sales_production_production_overrides" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <div style="height:100%;width:100%">
        <script type="text/javascript" language="javascript">
            var lastsel;
            $(document).ready(function() {
                $("#list1").jqGrid({
                    url: '../../services/IOService.asmx/GetProdOverridesGrid'
                    , datatype: "xml"
                    , colNames: ['Override ID', 'Contract Number', 'Contract Entry Date', 'Original Production Amount', 'Override Production %'
                        , 'Override Production Amount', 'Override Entry Date', 'Override Entered By', 'Company ID']
                    , colModel: [{ name: 'overrideId', index: 'OVERRIDE_ID', hidden: true }
                        , { name: 'contractNumber', index: 'CONTRACT_NUMBER', align: 'center', width: 95, search: true, sort: true }
                        , { name: 'contractEntryDate', index: 'CONTRACT_ENTRY_DATE', width: 110, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y H:i:s' }, align: 'right' }
                        , { name: 'originalProdAmount', index: 'ORIGINAL_PRODUCTION_AMOUNT', align: 'right', width: 90, search: false, sort: true, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                        , { name: 'overrideProdPerc', index: 'OVERRIDE_PRODUCTION_PERCENTAGE', width: 50, search: false, sort: true, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '' }, editable: true, editrules: { custom: true, custom_func: validateOverridePercentage} }
                        , { name: 'overrideProdAmount', index: 'OVERRIDE_PRODUCTION_AMOUNT', align: 'right', width: 90, search: false, sort: true, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '' }, editable: true, editrules: { custom: true, custom_func: validateOverrideEntry} }
                        , { name: 'overrideEntryDate', index: 'OVERRIDE_ENTRY_DATE', width: 100, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y H:i:s' }, align: 'right' }
                        , { name: 'overrideEnteredBy', index: 'OVERRIDE_ENTERED_BY', width: 125, search: true, sort: true }
                        , { name: 'companyId', index: 'COMPANY_ID', hidden: true}]
                    , rowNum: 50
                    , height: 500
                    , width: 800
                    , rowList: [50, 100, 150]
                    , pager: '#pager1'
                    , sortname: 'CONTRACT_ENTRY_DATE'
                    , sortorder: 'DESC'
                    , viewrecords: true
                    , toolbar: [true, 'top']
                    , caption: "Production Overrides"
                    , editurl: "../../services/IOService.asmx/AddUpdateProdOverride"
                    , onSelectRow: function(id) {
                        if (id) {
                            var contractNumber = $('#list1').getRowData(id)['contractNumber'];
                            var companyId = $('#list1').getRowData(id)['companyId'];
                            var overrideId = $('#list1').getRowData(id)['overrideId'];
                            var extraParams = { overrideId: overrideId, contractNumber: contractNumber, companyId: companyId };
                            $('#list1').editRow(id, true, null, null, null, extraParams, afterEdit);
                            lastsel = id;
                        }
                    }
                });
                $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false, search: false });
                $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
            });
            function validateOverridePercentage(value, colname) {
                if (parseFloat(value) > 3) {
                    return [false, "\nWarning!\nThe Override Production Percentage is Greater than the Default Percentage (3%)."];
                }
                return [true, ""];
            }
            function validateOverrideEntry(value, colname) {
                //value will contain the value entered for the Override Production Percentage
                //var overrideProductionAmont = $('#list1').getRowData(lastsel)['overrideProdAmount'];
                var originalProductionAmount = $('#list1').getRowData(lastsel)['originalProdAmount'];
                /*
                if (value == '' && overrideProductionAmont == '') {
                    return [false, "Either a Override Production Percentage or Override Production Amount must be specified."];
                }
                if (value != '' && overrideProductionAmont != '') {
                    return [true, "Warning!\nBoth Override Production Percentage and Override Production Amount have been specified.\nThe Override Production Percentage will be used."];
                }
                */
                if (parseFloat(value) > parseFloat(originalProductionAmount)) {
                    return [false, "\nWarning!\nThe Override Production Amount is Greater than the Original Production Amount (" + originalProductionAmount + ")."];
                }
                return [true, ""];
            }
            function afterEdit(response) {
                if (response) {
                    alert('Production Override has been saved.');
                    $("#list1").trigger('reloadGrid');
                    return true;
                } else {
                    alert('An error occurred while trying to save the Production Override.');
                    return false;
                }
            } 
        </script>
        <div style="margin:5px 0 15px 10px;">
            <h5>Click on a row to edit the Override Amount or Percentage.</h5>
            <h5>Note: If both a Percentage and an Amount are specified, the <u>Percentage</u> will be used for Commissions Calculations.</h5>
        </div>
        <div style="margin:0 auto;" id="commissionsGrid">
            <div style="width:95%;margin:50px;"><table id="list1"></table></div>
            <div id="pager1"></div>
        </div>
    </div>
</asp:Content>

