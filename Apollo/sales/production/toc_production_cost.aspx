<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="toc_production_cost.aspx.cs" Inherits="Apollo.sales_production_toc_production_cost" %>

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
            $(document).ready(function () {
                $("#list1").jqGrid({
                    url: '../../services/IOService.asmx/GetProdCostGrid'
                    , datatype: "xml"
                    , colNames: ['Prod Cost ID', 'UID Cmpgn', 'Production Type', 'Contract Number', 'Customer', 'Program', 'Contract Entry Date', 'Original Production Amount'
                        , 'Production Cost Amount', 'Entry Date', 'Entered By', 'Is Comm.?', 'Company ID']
                    , colModel: [{ name: 'prodCostId', index: 'PROD_COST_ID', hidden: true }
                        , { name: 'uidCmpgn', index: 'uid_cmpgn', hidden: true }
                        , { name: 'prodType', index: 'PROD_TYPE', align: 'center', width: 50, search: true, sort: true, stype: 'select', editoptions: { value: ":ALL;Normal:Normal;Special Projects:Special Projects"} }
                        , { name: 'contractNumber', index: 'CONTRACT_NUMBER', align: 'center', width: 80, search: true, sort: true }
                        , { name: 'customerName', index: 'CUSTOMER_NAME', align: 'center', width: 85, search: true, sort: true }
                        , { name: 'program', index: 'PROGRAM', align: 'left', width: 100, search: true, sort: true }
                        , { name: 'contractEntryDate', index: 'CONTRACT_ENTRY_DATE', width: 80, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y H:i:s' }, align: 'right' }
                        , { name: 'originalProdAmount', index: 'ORIGINAL_PRODUCTION_AMOUNT', align: 'right', width: 90, search: false, sort: true, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                        , { name: 'prodCostAmount', index: 'PROD_COST_AMOUNT', align: 'right', width: 90, search: false, sort: true, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '' }, editable: true, editrules: { custom: true, custom_func: validateOverrideEntry} }
                        , { name: 'entryDate', index: 'ENTRY_DATE', width: 75, search: false, sort: true, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y H:i:s' }, align: 'right' }
                        , { name: 'enteredBy', index: 'ENTERED_BY', width: 100, search: true, sort: true, align: 'center' }
                        , { name: 'isCommissionable', index: 'IS_COMMISSIONABLE', width: 55, search: false, sort: true, align: 'center' }
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
                    , caption: "Production Cost"
                    , editurl: "../../services/IOService.asmx/AddUpdateTOCProdCost"
                    , onSelectRow: function (id) {
                        if (id) {
                            var uidCmpgn = $("#list1").getRowData(id)["uidCmpgn"];
                            var prodType = $("#list1").getRowData(id)["prodType"];
                            var contractNumber = $('#list1').getRowData(id)['contractNumber'];
                            var companyId = $('#list1').getRowData(id)['companyId'];
                            var prodCostId = $('#list1').getRowData(id)['prodCostId'];
                            var extraParams = { prodCostId: prodCostId, uidCmpgn: uidCmpgn, prodType: prodType, contractNumber: contractNumber, companyId: companyId };
                            $('#list1').editRow(id, true, null, null, null, extraParams, afterEdit);
                            lastsel = id;
                        }
                    }
                });
                $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false, search: false });
                $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
            });
            function validateOverrideEntry(value, colname) {
                //value will contain the value entered for the Production Cost Amount
                var originalProductionAmount = $('#list1').getRowData(lastsel)['originalProdAmount'];
                if (!IsValidDecimal(value)) {
                    return [false, "\n\nWarning!\nThe Production Cost Amount must be a numeric value."];
                }
                if (parseFloat(value) > parseFloat(originalProductionAmount)) {
                    return [false, "\n\nWarning!\nThe Production Cost Amount is Greater than the Original Production Amount (" + originalProductionAmount + ")."];
                }
                return [true, ""];
            }
            function afterEdit(response) {
                if (response) {
                    alert('Production Cost has been saved.');
                    $("#list1").trigger('reloadGrid');
                    return true;
                } else {
                    alert('An error occurred while trying to save the Production Cost.');
                    return false;
                }
            } 
        </script>
        <div style="margin:5px 0 15px 10px;">
            <h5>Click on a row to edit the Production Cost.</h5>
        </div>
        <div style="margin:0 auto;" id="commissionsGrid">
            <div style="width:95%;margin:50px;"><table id="list1"></table></div>
            <div id="pager1"></div>
        </div>
    </div>
</asp:Content>

