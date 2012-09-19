<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="uninvoiced_segment_breakdown.aspx.cs" Inherits="Apollo.quattro_uninvoiced_segment_breakdown" %>

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
        function ShowAttachmentPopup(contractNumber, companyId) {
            jQuery.get('QuattroAttachmentHandler.ashx?contractNumber=' + contractNumber + '&companyId=' + companyId, null, ShowAttachmentPopupCallback, 'html');
        }
        function ShowAttachmentPopupCallback(data) {
            var dialog = $("<div style='font-size:10px !important;'></div>");
            dialog.addClass("dialog")
                .attr({ "id": "attachmentDialog" })
                .appendTo("body")
                .dialog({ title: "Attachments"
                    , close: function () { $(this).remove() }
                    , modal: true, autoOpen: false
                    , width: 600, height: 250
                }).html(data);
            dialog.dialog("open");
        }
        function ShowAttachmentPopupCallbackOld(data) {
            //innerShowAttachmentPopupCallback(data);
            jQuery('#attachmentPopupDetail').html(data);
            $find('attachmentPopupExtBehavior').show();
        }
        $(document).ready(function () {
            $("#list1").jqGrid({
                url: '../services/QuattroService.asmx/GetUninvoicedSegmentGrid'
                , datatype: "xml"
                , colNames: ['Company Name', 'Entry Date', 'Most Recent Attachment Date', 'Entered By', 'Contract #', 'Contract Status', 'Campaign Type'
                    , 'Customer', 'Segment Status', 'Segment Sales Market', 'Has Segment Profit Center Split', 'Media Type'
                    , 'Media Product', 'Segment Space Reserved', 'Segment Start Date', 'Segment End Date', 'View Attachments']
                , colModel: [{ name: 'companyName', index:'Company Name', width:75, align:'center', stype: 'select', editoptions: { value: ":ALL;1:Titan US;2:Titan Canada"} }
                    , { name: 'entryDate', index: 'Entry Date', width: 80, search: false, sort: true, align: 'right' }
                    , { name: 'mostRecentAttDate', index: 'Most Recent Attachment Date', width: 125, search: false, sort: true, align: 'right' }
                    , { name: 'enteredBy', index: 'Entered By', width: 110, search: true, sort: true, align: 'center' }
                    , { name: 'contractNumber', index: 'Contract #', width: 80, search: true, sort: true, align: 'center' }
                    , { name: 'contractStatus', index: 'Contract Status', width: 80, search: true, sort: true, align: 'center' }
                    , { name: 'campaignType', index: 'Campaign Type', width: 90, search: true, sort: true, align: 'center' }
                    , { name: 'customer', index: 'Customer', width: 150, search: true, sort: true }
                    , { name: 'segmentStatus', index: 'Segment Status', width: 80, search: true, sort: true, align: 'center' }
                    , { name: 'segmentMarket', index: 'Segment Sales Market', width: 100, search: true, sort: true, align: 'center' }
                    , { name: 'segmentProfCtrSplit', index: 'Has Segment Profit Center Split', width: 50, search: true, sort: true, align: 'center' }
                    , { name: 'mediaType', index: 'Media Type', width: 50, search: true, sort: true, align: 'center' }
                    , { name: 'mediaProduct', index: 'Media Product', width: 120, search: true, sort: true, align: 'center' }
                    , { name: 'segmentSpaceRes', index: 'Segment Space Reserved', width: 50, search: true, sort: true, align: 'right' }
                    , { name: 'segmentStartDate', index: 'Segment Start Date', width: 80, search: false, sort: true, align: 'right' }
                    , { name: 'segmentEndDate', index: 'Segment End Date', width: 80, search: false, sort: true, align: 'right' }
                    , { name: 'viewAttachments', index: 'View Attachments', width: 100, search: false, hidden: true }                    
                ]
                , rowNum: 25
                , height: 400
                , width: "100%"
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'Entry Date'
                , sortorder: 'ASC'
                , caption: "Segments not setup for Invoicing"
                , viewrecords: true
                , toolbar: [true, 'top']
                , onSelectRow: function (id) {
                    if (id) {
                        var contractNumber = $('#list1').getRowData(id)['contractNumber'];
                        var companyId = $("#list1").getRowData(id)["companyId"];
                        ShowAttachmentPopup(contractNumber, companyId);
                    }
                }
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false, search: false }).navButtonAdd("#pager1",
                { caption: 'Toggle Columns', onClickButton: function () {
                    $("#list1").setColumns({ caption: 'Show/Hide Columns', bSubmit: 'Update', bCancel: 'Cancel', colnameview: false, drag: false, ShrinkToFit: true }); return false;
                }
                });
            $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
        });
        function onExportClick() {
            $get('<%=sortIndex.ClientID %>').value = $('#list1').getGridParam('sortname');
            $get('<%=sortOrder.ClientID %>').value = $('#list1').getGridParam('sortorder');
            var postData = $('#list1').getGridParam('postData');
            if (postData["Entered By"]) {
                $get('<%=enteredBy.ClientID %>').value = postData["Entered By"];
            } else { $get('<%=enteredBy.ClientID %>').value = "" }
            if (postData["Contract #"]) {
                $get('<%=contractNumber.ClientID %>').value = postData["Contract #"];
            } else { $get('<%=contractNumber.ClientID %>').value = "" }
            if (postData["Contract Status"]) {
                $get('<%=contractStatus.ClientID %>').value = postData["Contract Status"];
            } else { $get('<%=contractStatus.ClientID %>').value = "" }
            if (postData["Campaign Type"]) {
                $get('<%=campaignType.ClientID %>').value = postData["Campaign Type"];
            } else { $get('<%=campaignType.ClientID %>').value = "" }
            if (postData["Customer"]) {
                $get('<%=customer.ClientID %>').value = postData["Customer"];
            } else { $get('<%=customer.ClientID %>').value = "" }
            if (postData["Segment Status"]) {
                $get('<%=segmentStatus.ClientID %>').value = postData["Segment Status"];
            } else { $get('<%=segmentStatus.ClientID %>').value = "" }
            if (postData["Segment Sales Market"]) {
                $get('<%=segmentSalesMarket.ClientID %>').value = postData["Segment Sales Market"];
            } else { $get('<%=segmentSalesMarket.ClientID %>').value = "" }
            if (postData["Has Segment Profit Center Split"]) {
                $get('<%=segmentProfCtrSplit.ClientID %>').value = postData["Has Segment Profit Center Split"];
            } else { $get('<%=segmentProfCtrSplit.ClientID %>').value = "" }
            if (postData["Media Type"]) {
                $get('<%=mediaType.ClientID %>').value = postData["Media Type"];
            } else { $get('<%=mediaType.ClientID %>').value = "" }
            if (postData["Media Product"]) {
                $get('<%=mediaProduct.ClientID %>').value = postData["Media Product"];
            } else { $get('<%=mediaProduct.ClientID %>').value = "" }
            if (postData["Segment Space Reserved"]) {
                $get('<%=segmentSpaceRes.ClientID %>').value = postData["Segment Space Reserved"];
            } else { $get('<%=segmentSpaceRes.ClientID %>').value = "" }
            if (postData["Company Name"]) {
                $get('<%=companyId.ClientID %>').value = postData["Company Name"];
            } else { $get('<%=companyId.ClientID %>').value = ""; }
        }
        function CloseAttachmentPopup() {
            $get('<%=attachmentPopup.ClientID %>').style.display = "none";
        } 
    </script>
    <asp:HiddenField ID="sortIndex" runat="server" />
    <asp:HiddenField ID="sortOrder" runat="server" />
    <asp:HiddenField ID="enteredBy" runat="server" />
    <asp:HiddenField ID="contractNumber" runat="server" />
    <asp:HiddenField ID="contractStatus" runat="server" />
    <asp:HiddenField ID="campaignType" runat="server" />
    <asp:HiddenField ID="customer" runat="server" />
    <asp:HiddenField ID="segmentStatus" runat="server" />
    <asp:HiddenField ID="segmentSalesMarket" runat="server" />
    <asp:HiddenField ID="segmentProfCtrSplit" runat="server" />
    <asp:HiddenField ID="mediaType" runat="server" />
    <asp:HiddenField ID="mediaProduct" runat="server" />
    <asp:HiddenField ID="segmentSpaceRes" runat="server" />    
    <asp:HiddenField ID="companyId" runat="server" />
    <asp:ImageButton ID="excelExport" runat="server" 
        ImageUrl="/Images/but_excel.gif" AlternateText="Export To Excel" 
        style="margin-left:10px;" onclick="excelExport_Click" OnClientClick="onExportClick();" />
    <br />
    <div style="margin:5px 0 0 10px;"><h5>Click on a row to view the Contract's attachments.</h5></div>
    <br />
    <div style="width:95%">
        <div style="width:95%;margin:10px;z-index:-1"><table id="list1"></table></div>
        <div id="pager1"></div>
    </div>
    <asp:Button ID="showAttachmentPopup" runat="server" style="display:none" />
    <ajax:ModalPopupExtender ID="attachmentPopupExt" runat="server"
        CancelControlID="closeAttachmentPopup" BehaviorID="attachmentPopupExtBehavior" PopupControlID="attachmentPopup" 
        TargetControlID="showAttachmentPopup" BackgroundCssClass="popupbg" DropShadow="true" />
    <div id="attachmentPopup" runat="server" style="height:175px;border:1px solid #333333;background-color:White;padding:10px;display:none">                    
        <div style="float:right"><asp:ImageButton ID="closeAttachmentPopup" runat="server" ImageUrl="/Images/dl/icon_tooltip_close.png" style="cursor:pointer;margin:5px 3px;" OnClientClick="CloseAttachmentPopup();" /></div>
        <div style="clear:both"></div>
        <div id="attachmentPopupDetail"></div>
    </div>
</asp:Content>

