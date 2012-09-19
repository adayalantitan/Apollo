<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="target_audience_maintenance.aspx.cs" Inherits="Apollo.digital_target_audience_maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            //$(".button").button();
            var lastsel;
            $("#list1").jqGrid({
                url: '/services/IOService.asmx/GetEthnicityGrid'
                , datatype: "xml"
                , colNames: ['Target Audience ID', 'Target Audience Desc']
                , colModel: [{ name: 'ethnicityId', index: 'ETHNICITY_ID', width: 50, search: false, align: 'center' }
                    , { name: 'ethnicityDesc', index: 'ETHNICITY_DESC', width: 175, search: false, align: 'center', editable: true, edittype: 'text'}]
                , rowNum: 25
                , height: 300
                , width: 400
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'ETHNICITY_ID'
                , sortorder: 'ASC'
                , viewrecords: true
                , onSelectRow: function(id) {
                    if (id && id !== lastsel) {
                        var extraParams = { ethnicityId: $('#list1').getRowData(id)['ethnicityId'] };
                        $('#list1').restoreRow(lastsel);
                        $('#list1').editRow(id, true, null, null, "/services/IOService.asmx/UpsertEthnicity", extraParams, afterEdit);
                        lastsel = id;
                    }
                }
                , editurl: "/services/IOService.asmx/AddEthnicity"
                , caption: "Target Audience Maintenance"
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false, search: false });
        });
        function afterEdit(id,response){
            if (response){
                //alert('Entry Saved');
            } else {
                alert('An error occurred while trying to save your Entry.');
            }
        }
        function afterEthnicityEdit(wasSuccessful) { if (wasSuccessful) { alert('Target Audience has been added.'); return true; } else { alert('An error occurred while trying to add the Target Audience Record.'); return false; } }
        function AddEthnicity() {
            $('#list1').setGridParam({ editurl: "/services/IOService.asmx/AddEthnicity" }).trigger('reloadGrid');
            $("#list1").jqGrid('editGridRow', "new", { height: 180, reloadAfterSubmit: true, bSubmit: 'Add Target Audience', afterSubmit: afterEthnicityEdit });
        }    
    </script>
    <div style="margin:0 auto;">
        <div style="width:95%;margin:50px;"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
    <div style="margin:15px 0 0 50px;"><input type="button" value="Add Target Audience Record" onclick="AddEthnicity();" /></div>
</asp:Content>

