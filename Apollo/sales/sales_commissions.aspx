<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="sales_commissions.aspx.cs" Inherits="Apollo.sales_sales_commissions" Title="Contracts Split | Titan 360" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/contractInfo.ascx" TagName="contractInfo" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="reportListPanel" Runat="Server">
    <div style="height:100%;width:100%">
        <script type="text/javascript" language="javascript">
            var lastsel;
            function Back() {
                $get('<%=contractDetail.ClientID %>').style.display = "none";
                $get('commissionsGrid').style.display = "block";
                ClearContractData();
                lastSel = -1;
            }
            function PopupModalWindow(contractNumber, companyId) {
                ShowFirstTab();
                ShowContractDetail();
                LoadContractDetail(contractNumber, companyId, -1, false);
            }            
            function ShowContractDetail() {
                $get('commissionsGrid').style.display = "none";
                $get('<%=contractDetail.ClientID %>').style.display = "block";
            }
            $(document).ready(function () {
                $("#list1").jqGrid({
                    url: '../services/IOService.asmx/GetCommSplitsGrid'
                    , datatype: "xml"
                    , colNames: ['Contract Number', 'Agency', 'Advertiser', 'Program', 'AE 1', 'AE 2', 'AE 3', 'Company ID', 'Company']
                    , colModel: [{ name: 'contractNumber', index: 'ContractNumber', width: 125, search: true, sort: true }
                        , { name: 'agency', index: 'Agency', width: 125, search: true, sort: true }
                        , { name: 'advertiser', index: 'Advertiser', width: 125, search: true }
                        , { name: 'program', index: 'PROGRAM', width: 125, search: true }
                        , { name: 'ae1Name', index: 'AE1Name', width: 125, search: true }
                        , { name: 'ae2Name', index: 'AE2Name', width: 125, search: false }
                        , { name: 'ae3Name', index: 'AE3Name', width: 125, search: false }
                        , { name: 'companyId', index: 'CompanyId', hidden: true }
                        , { name: 'company', index: 'Company', width: 100, search: true, sort: true}]
                    , rowNum: 25
                    , height: 500
                    , width: 800
                    , rowList: [25, 50, 100]
                    , pager: '#pager1'
                    , sortname: 'ContractNumber'
                    , sortorder: 'DESC'
                    , viewrecords: true
                    , toolbar: [true, 'top']
                    , caption: "Commission Splits"
                    , onSelectRow: function (id) {
                        if (id) {
                            var contractNumber = $('#list1').getRowData(id)['contractNumber'];
                            var companyId = $('#list1').getRowData(id)['companyId'];
                            //$('#list1').restoreRow(lastsel);
                            //lastsel = id;
                            PopupModalWindow(contractNumber, companyId);
                        }
                    }
                });
                $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false });
                $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
            });
        </script>
        <div style="margin:0 auto;" id="commissionsGrid">
            <div style="width:95%;margin:50px;"><table id="list1"></table></div>
            <div id="pager1"></div>
        </div>
        <div id="contractDetail" runat="server" style="display:none;width:95%;margin:50px;">
            <uc:contractInfo ID="contractDetailInfo" runat="server" />                    
        </div>                
    </div>
</asp:Content>