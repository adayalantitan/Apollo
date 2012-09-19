<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchVertSplit.master" AutoEventWireup="true" CodeFile="dataCheck.aspx.cs" Inherits="Apollo.admin_dataCheck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="searchCriteriaPanel" Runat="Server">
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
    <script type="text/javascript">
        $(document).ready(function () {
            $('#reportSelectionArea').accordion({ fillSpace: true });
            LoadProfitCenterGrid();
            LoadDiscrepancyGrid();
            LoadLNGrid();
        });
        function LoadProfitCenterGrid() {
            $("#profitCenterList").jqGrid({
                url: '../services/IOService.asmx/GetProfitCenterCheckGrid'
                , datatype: "xml"
                , colNames: ['Profit Center ID', 'Profit Center']
                , colModel: [{ name: 'profitCenterId', index: 'GL_PROFIT_CENTER_ID', align: 'center' }
                    , { name: 'profitCenter', index: 'PROFIT_CENTER', align: 'center' }
                ]
                , rowNum: 50
                , height: 250
                , width: 700
                , rowList: [25, 50, 100]
                , pager: '#profitCenterPager'
                , sortname: 'PROFIT_CENTER'
                , sortorder: 'ASC'
                , caption: "Profit Center Rate Check"
                , viewrecords: true
            });
            $("#profitCenterList").jqGrid('navGrid', "#profitCenterPager", { edit: false, add: false, del: false, search: false });
        }        
        function LoadDiscrepancyGrid() {
            $("#discrepancyList").jqGrid({
                url: '../services/IOService.asmx/GetDiscrepancyCheckGrid'
                , datatype: "xml"
                , colNames: ['Contract Number', 'Invoice Number', 'GL Code', 'Profit Center ID', 'Line Message', 'Invoice Date']
                , colModel: [{ name: 'contractNumber', index: 'CONTRACT_NUMBER', align: 'center', width: 100 }
                    , { name: 'invoiceNumber', index: 'INVOICE_NUMBER', align: 'center', width: 50 }
                    , { name: 'glCode', index: 'GL_CODE' }
                    , { name: 'profitCenterId', index: 'PROFIT_CENTER_ID' }
                    , { name: 'lineMessage', index: 'LINE_MESSAGE' }
                    , { name: 'invoiceDate', index: 'INVOICE_DATE', align: 'right', width: 80, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} }
                ]
                , rowNum: 50
                , height: 250
                , width: 700
                , rowList: [25, 50, 100]
                , pager: '#discrepancyPager'
                , sortname: 'CONTRACT_NUMBER'
                , sortorder: 'ASC'
                , caption: "GL Discrepancy Check"
                , viewrecords: true
            });
            $("#discrepancyList").jqGrid('navGrid', "#discrepancyPager", { edit: false, add: false, del: false, search: false });
        }
        function LoadLNGrid() {
            $("#lnList").jqGrid({
                url: '../services/IOService.asmx/GetLocalNationalCheckGrid'
                , datatype: "xml"
                , colNames: ['Contract Number', 'Local/National', 'Company']
                , colModel: [{ name: 'contractNumber', index: 'CONTRACT_NUMBER' }
                    , { name: 'localNational', index: 'LOCAL_OR_NATIONAL' }
                    , { name: 'company', index: 'COMPANY' }
                ]
                , rowNum: 50
                , height: 250
                , width: 700
                , rowList: [25, 50, 100]
                , pager: '#lnPager'
                , sortname: 'CONTRACT_NUMBER'
                , sortorder: 'ASC'
                , caption: "Incorrect Local/National Indicator"
                , viewrecords: true
            });
            $("#lnList").jqGrid('navGrid', "#lnPager", { edit: false, add: false, del: false, search: false });
        }
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchResultsPanel" Runat="Server">    
    <div style="height:100%;width:100%;text-align:center" id="reportSelectionArea">                                
        <h3><a href="#">Profit Center Rate Check</a></h3>
        <div id="profitCenterRateArea" style="height:350px !important;">            
            <div id="profitCenterGrid" style="margin:10px 25px;width:90%;">
                <div style="width:95%;z-index:-1"><table id="profitCenterList"></table></div>
                <div id="profitCenterPager"></div>
            </div>
        </div>
        <h3><a href="#">GL Discrepancy Check</a></h3>
        <div id="glDiscrepArea" style="height:350px !important;">            
            <div id="discrepancyGrid" style="margin:10px 25px;width:90%;">
                <div style="width:95%;z-index:-1"><table id="discrepancyList"></table></div>
                <div id="discrepancyPager"></div>
            </div>
        </div>        
        <h3><a href="#">Contracts with incorrect Local/National Indicator</a></h3>
        <div id="lnIndicatorArea" style="height:350px !important;">            
            <div id="lnGrid" style="margin:10px 25px;width:90%;">
                <div style="width:95%;z-index:-1"><table id="lnList"></table></div>
                <div id="lnPager"></div>
            </div>
        </div>
    </div>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
</asp:Content>

