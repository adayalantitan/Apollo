<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="admin_cust_maintenance.aspx.cs" Inherits="Apollo.admin_admin_cust_maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("#list1").jqGrid({
                url: '/services/IOService.asmx/GetCustomerRollupGrid'
                , datatype: "xml"
                , postData: { customerId: '', customerName: '', contactName: '', newCustomerId: '', newCustomerName: '', companyName: '' }
                , colNames: ['Customer #', 'Customer Name', 'Contact Name', 'New Customer #', 'New Customer Name', 'Report Display #', 'Report Display Name', 'Company', 'Company ID']
                , colModel: [{ name: 'customerId', index: 'CUSTOMER_ID', width: 75, search: true, searchoptions: { sopt: ['eq'] }, sort: true }
                    , { name: 'customerName', index: 'CUSTOMER_NAME', width: 170, search: true, searchoptions: { sopt: ['eq'] }, sort: true }
                    , { name: 'contactName', index: 'CONTACT_NAME', width: 100, search: true, searchoptions: { sopt: ['eq'] }, sort: true }
                    , { name: 'newCustomerId', index: 'NEW_CUSTOMER_ID', width: 75, editable: true, edittype: 'text', search: true, searchoptions: { sopt: ['eq']} }
                    , { name: 'newCustomerName', index: 'NEW_CUSTOMER_NAME', width: 170, editable: true, edittype: 'text', search: true, searchoptions: { sopt: ['eq']} }
                    , { name: 'reportDisplayId', index: 'REPORT_DISPLAY_ID', width: 75, editable: true, edittype: 'text', search: true, searchoptions: { sopt: ['eq']} }
                    , { name: 'reportDisplayName', index: 'REPORT_DISPLAY_NAME', width: 170, editable: true, edittype: 'text', search: true, searchoptions: { sopt: ['eq']} }
                    , { name: 'companyName', index: 'COMPANY_NAME', width: 110, editable: false, search: true, searchoptions: { sopt: ['eq']} }
                    , { name: 'companyId', index: 'COMPANY_ID', hidden: true}]
                , rowNum: 25
                , height: 500
                , width: 950
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'CUSTOMER_ID'
                , sortorder: 'ASC'
                , viewrecords: true
                , toolbar: [true, 'top']
                , onSelectRow: function(id) {
                    if (id) {
                        var extraParams = { customerId: $('#list1').getRowData(id)['customerId'],
                            companyId: $('#list1').getRowData(id)['companyId']
                        };
                        $('#list1').editRow(id, true, null, null, null, extraParams, afterEdit);
                    }
                }
                , editurl: "/services/IOService.asmx/AddCustomerXref"
                , caption: "Customer Maintenance"
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false });
            $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
        });
        function afterEdit(id,response){
            if (response){
                //alert('Entry Saved');
            } else {
                alert('An error occurred while trying to save your Entry.');
            }
        }
        function onExportClick() {
            return true;
        }
    </script>    
    <div style="margin:0 auto;">
        <asp:ImageButton ID="excelExport" runat="server" 
            ImageUrl="/Images/but_excel.gif" AlternateText="Export To Excel" 
            onclick="excelExport_Click" style="margin-left:10px;" OnClientClick="onExportClick();" />
        <br />
        <div style="width:95%;margin:50px;"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
</asp:Content>

