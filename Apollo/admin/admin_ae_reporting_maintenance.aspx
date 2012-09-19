<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="admin_ae_reporting_maintenance.aspx.cs" Inherits="Apollo.admin_admin_ae_reporting_maintenance" %>

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
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            var lastsel;
            $("#list1").jqGrid({
                url: '../services/IOService.asmx/GetAEReportingGrid'
                , datatype: "xml"                
                , colNames: ['AE ID', 'AE Name', 'Market Code', 'Active?', 'Hide in Commission Reports?', 'Hidden', 'Company', 'Company ID']
                , colModel: [{ name: 'aeId', index: 'ACCOUNT_EXECUTIVE_ID', width: 50, search: true, sort: true }
                    , { name: 'aeName', index: 'ACCOUNT_EXECUTIVE_NAME', width: 125, search: true, sort: true }
                    , { name: 'aeMarket', index: 'ACCOUNT_EXECUTIVE_MARKET_ID', width: 100, search: true, sort: true }
                    , { name: 'isActive', index: 'ACTIVE', width: 100, search: true, sort: true }
                    , { name: 'isHiddenCheckBox', index: 'HIDDEN_CHECK_BOX', search: false, sortable: true, align: 'center', editable: false, width: 75 }
                    , { name: 'isHiddenFromReports', index: 'HIDDEN', hidden: true }
                    , { name: 'companyName', index: 'COMPANY_NAME', width: 95, editable: false, search: true }
                    , { name: 'companyId', index: 'COMPANY_ID', hidden: true}]
                , rowNum: 25
                , height: 500
                , width: 800
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'ACCOUNT_EXECUTIVE_NAME'
                , sortorder: 'ASC'
                , viewrecords: true
                , toolbar: [true, 'top']
                , gridComplete: function() {
                    var ids = $("#list1").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var id = ids[i];
                        var isHidden = ($('#list1').getRowData(id)['isHiddenFromReports'] == -1);
                        var aeId = $('#list1').getRowData(id)['aeId'];
                        var companyId = $('#list1').getRowData(id)['companyId'];
                        var isHiddenCheckBox;                        
                        hiddenCheckBox = "<input style='height:22px;font-size:10px;' type='checkbox' onclick=\"ToggleAEHidden('" + aeId + "',this," + companyId + ");\"" + ((isHidden) ? " checked='checked' " : "") + " />";
                        $("#list1").jqGrid('setRowData', ids[i], { isHiddenCheckBox: hiddenCheckBox });                        
                    }
                }
                , caption: "AE Reporting Maintenance"
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false, search: false });
            $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
        });
        function ToggleAEHidden(aeId, checkBox, companyId) {
            var isHidden = checkBox.checked;
            Apollo.IOService.ToggleHiddenAE(aeId, isHidden, companyId, ToggleAEHiddenCallback);
        }
        function ToggleAEHiddenCallback(response){
            if (response) {
                alert('Changes Saved');
            } else {
                alert('An error occurred while trying to save your Changes.');
            }
        }        
    </script>
    <div style="margin:0 auto;">
        <div style="width:95%;margin:50px;"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
</asp:Content>