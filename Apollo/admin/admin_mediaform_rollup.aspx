<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="admin_mediaform_rollup.aspx.cs" Inherits="Apollo.admin_admin_mediaform_rollup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            var lastsel;
            $("#list1").jqGrid({
                url: '../services/IOService.asmx/GetMediaFormRollupGrid'
                , datatype: "xml"
                , colNames: ['Media Form Id', 'Media Type Id', 'Media Type Description', 'Media Form Description', 'Rollup Name', 'Should be Changed To']
                , colModel: [{ name: 'mediaFormId', index: 'MEDIA_FORM_ID', hidden: true }
                    , { name: 'mediaTypeId', index: 'MEDIA_TYPE_ID', hidden: true }
                    , { name: 'mediaTypeDescription', index: 'MEDIA_TYPE_DESCRIPTION', width: 125, search: true, sort: true }
                    , { name: 'mediaFormDescription', index: 'MEDIA_FORM_DESCRIPTION', width: 125, search: true, sort: true }
                    , { name: 'rollupName', index: 'ROLLUP_NAME', width: 125, search: true, sort: true, edittype: 'text', editable: true }
                    , { name: 'shouldBe', index: 'SHOULD_BE', width: 125, search: true, sort: true, edittype: 'text', editable: true }
                ]
                , rowNum: 25
                , height: 500
                , width: 800
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'MEDIA_TYPE_DESCRIPTION'
                , sortorder: 'ASC'
                , viewrecords: true
                , toolbar: [true, 'top']
                , onSelectRow: function(id) {
                    if (id && id !== lastsel) {
                        var extraParams = { mediaFormId: $('#list1').getRowData(id)['mediaFormId'] };
                        $('#list1').restoreRow(lastsel);
                        $('#list1').editRow(id, true, null, null, null, extraParams, afterEdit);
                        lastsel = id;
                    }
                }
                , editurl: "../services/IOService.asmx/UpdateMediaFormRollup"
                , caption: "Media Form Rollup Maintenance"
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
    </script>
    <div style="margin:0 auto;">
        <div style="width:95%;margin:50px;"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
</asp:Content>

