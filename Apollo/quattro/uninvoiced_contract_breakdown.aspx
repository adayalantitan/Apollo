<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="uninvoiced_contract_breakdown.aspx.cs" Inherits="Apollo.quattro_uninvoiced_contract_breakdown" %>

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
            jQuery.get('QuattroAttachmentHandler.ashx?contractNumber=' + contractNumber + '&companyId=' + companyId, null, ShowAttachmentPopupCallbackNew, 'html');
        }
        function ShowAttachmentPopupCallbackNew(data) {
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
        function ShowAttachmentPopupCallback(data) {
            //innerShowAttachmentPopupCallback(data);
            jQuery('#attachmentPopupDetail').html(data);
            $find('attachmentPopupExtBehavior').show();
        }
        $(document).ready(function () {
            var lastsel;
            $("#list1").jqGrid({
                url: '../services/QuattroService.asmx/GetUninvoicedContractGrid'
                , editurl: "../services/QuattroService.asmx/AddUpdateUninvoicedContractNote"
                , datatype: "xml"
                , colNames: ['Approved for Invoicing?', 'Approval ID', 'View Attachments', 'UID Cmpgn', 'Company Id', 'Company', 'Entry Date', 'Most Recent Attachment Date', 'Entered By'
                    , 'Contract #', 'Campaign Type', 'Customer', '# of Campaign Segments', '# of Barter Segments', 'Market', 'Earliest Flight Start Date'
                    , 'Latest Flight End Date', 'Est. Contract Value', 'Is Missing Segment Profit Center Split', 'Is Banner', 'Notes', 'Credit Check']
                , colModel: [{ name: 'approvedForInvoicing', index: "APPROVED_FOR_INVOICING", hidden: true, search: false }
                    , { name: 'approvalId', index: 'APPROVAL_ID', hidden: true, search: false }
                    , { name: 'viewAttachmentButton', index: 'VIEW_ATTACHMENT_BUTTON', search: false, width: 100 }
                    , { name: 'uidCmpgn', index: 'uid_cmpgn', hidden: true }
                    , { name: 'companyId', index: 'COMPANY_ID', hidden: true }
                    , { name: 'companyName', index: 'COMPANY_NAME', width: 85, align: 'center', stype: 'select', editoptions: { value: ":ALL;1:Titan US;2:Titan Canada"} }
                    , { name: 'entryDate', index: 'Entry Date', width: 75, search: false, sort: true, align: 'right' }
                    , { name: 'mostRecentAttDate', index: 'Most Recent Attachment Date', width: 125, search: false, sort: true, align: 'right' }
                    , { name: 'enteredBy', index: 'Entered By', width: 110, search: true, searchoptions: { sopt: ['eq'] }, sort: true, align: 'center' }
                    , { name: 'contractNumber', index: 'Contract #', width: 75, search: true, sort: true, align: 'center' }
                    , { name: 'campaignType', index: 'Campaign Type', width: 90, search: true, sort: true, align: 'center' }
                    , { name: 'customer', index: 'Customer', width: 150, search: true, sort: true }
                    , { name: 'numCmpgnSegments', index: 'Number of Campaign Segments', width: 50, search: true, sort: true, align: 'right' }
                    , { name: 'numBarterSegments', index: 'Number of Barter Segments', hidden: true }
                    , { name: 'market', index: 'Market', hidden: true }
                    , { name: 'earliestFlightStartDate', index: 'Earliest Flight Start Date', width: 75, search: false, sort: true, align: 'right' }
                    , { name: 'latestFlightEndDate', index: 'Latest Flight End Date', width: 75, search: false, sort: true, align: 'right' }
                    , { name: 'estContractValue', index: 'Est. Contract Value', width: 80, search: false, sort: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'isMissingSplit', index: 'Is Missing Segment Profit Center Split', width: 50, search: true, sort: true, align: 'center' }
                    , { name: 'isBanner', index: 'Is Banner', hidden: true }
                    , { name: 'notes', index: 'NOTE_TEXT', width: 175, search: false, sort: true, align: 'center', editable: true, editoptions: { size: "30", maxlength: "4000"} }
               	    , { name: 'creditChecked', index: 'CREDIT_CHECKED', sort: true, search: false }

		 ]
                , rowNum: 25
                , height: 400
                //, width: "100%"
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'Entry Date'
                , sortorder: 'ASC'
                , caption: "Contracts not setup for Invoicing"
                , viewrecords: true
                , toolbar: [true, 'top']
                , onSelectRow: function (id) {
                    if (id && id != lastsel) {
                        //var contractNumber = $('#list1').getRowData(id)['contractNumber'];
                        //ShowAttachmentPopup(contractNumber, 1);
                        $('#list1').restoreRow(lastsel);
                        lastsel = id;
                    }
                    var extraParams = { uidCmpgn: $('#list1').getRowData(id)['uidCmpgn'], companyId: $('#list1').getRowData(id)['companyId'] };
                    //$('#list1').editRow(id, true, null, null, null, extraParams, afterEdit);
                    $('#list1').editRow(id, true, onRowEdit, null, null, extraParams, afterEdit);
                }
                , gridComplete: function () {
                    var ids = $("#list1").jqGrid('getDataIDs');
                    var isApprover = ($get('<%=isApprover.ClientID %>').value != '0');
                    var input;
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var approvalId = $('#list1').getRowData(cl)['approvalId'];
                        var uidCmpgn = $('#list1').getRowData(cl)['uidCmpgn'];
                        var contractNumber = $("#list1").getRowData(cl)["contractNumber"];
                        var companyId = $("#list1").getRowData(cl)["companyId"];
                        var viewAttachmentButton = "<input style='font-size:10px' type='button' value='View Attachment' onclick='ShowAttachmentPopup(\"" + contractNumber + "\",\"" + companyId + "\");' />";
                        if (approvalId == -1) {
                            input = (isApprover) ? "<input style='height:22px;font-size:10px;' type='checkbox' onclick='ToggleUninvoicedApproval(" + approvalId + "," + uidCmpgn + ",\"" + companyId + "\");' />" : "No";
                        } else {
                            input = (isApprover) ? "<input style='height:22px;font-size:10px;' type='checkbox' onclick='ToggleUninvoicedApproval(" + approvalId + "," + uidCmpgn + ",\"" + companyId + "\");' checked='checked' />" : "Yes";
                        }
                        //approvedForInvoicing
                        $("#list1").jqGrid('setRowData', ids[i], { viewAttachmentButton: viewAttachmentButton });
                        $("#list1").jqGrid('setRowData', ids[i], { approvedForInvoicing: input });
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
        function onRowEdit(id) {
            if ($("#" + id + "_notes").val() == "") {
                $("#" + id + "_notes").val(GetCurrentDateAsString());
            }
        }
        function GetCurrentDateAsString() {
            return ("" + (new Date().getMonth() + 1) + "/" + new Date().getDate() + "/" + new Date().getFullYear());
        }
        function afterEdit(id, response) {
            if (response) {
                alert('Entry Saved');
            } else {
                alert('An error occurred while trying to save your Entry.');
            }
        }  
        function ToggleUninvoicedApproval(approvalId, uidCmpgn, companyId) {
            Apollo.QuattroService.ToggleUninvoicedApproval(approvalId, uidCmpgn, companyId, ToggleUninvoicedApprovalCallback);
        }
        function ToggleUninvoicedApprovalCallback(response) {
            if (response) {
                alert('Changes have been saved');
                $('#list1').trigger('reloadGrid');
            } else {
                alert('An error occurred');
            }
        }
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
            if (postData["Campaign Type"]) {
                $get('<%=campaignType.ClientID %>').value = postData["Campaign Type"];
            } else { $get('<%=campaignType.ClientID %>').value = "" }
            if (postData["Customer"]) {
                $get('<%=customer.ClientID %>').value = postData["Customer"];
            } else { $get('<%=customer.ClientID %>').value = "" }
            if (postData["Number of Campaign Segments"]) {
                $get('<%=numCmpgnSegments.ClientID %>').value = postData["Number of Campaign Segments"];
            } else { $get('<%=numCmpgnSegments.ClientID %>').value = "" }
            if (postData["Number of Barter Segments"]) {
                $get('<%=numBarterSegments.ClientID %>').value = postData["Number of Barter Segments"];
            } else { $get('<%=numBarterSegments.ClientID %>').value = "" }
            if (postData["Market"]) {
                $get('<%=market.ClientID %>').value = postData["Market"];
            } else { $get('<%=market.ClientID %>').value = "" }
            if (postData["Is Missing Segment Profit Center Split"]) {
                $get('<%=isMissingSplit.ClientID %>').value = postData["Is Missing Segment Profit Center Split"];
            } else { $get('<%=isMissingSplit.ClientID %>').value = "" }
            if (postData["Is Banner"]) {
                $get('<%=isBanner.ClientID %>').value = postData["Is Banner"];
            } else { $get('<%=isBanner.ClientID %>').value = "" }
            if (postData["COMPANY_ID"]) {
                $get('<%=companyId.ClientID %>').value = postData["COMPANY_NAME"];
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
    <asp:HiddenField ID="campaignType" runat="server" />
    <asp:HiddenField ID="customer" runat="server" />
    <asp:HiddenField ID="numCmpgnSegments" runat="server" />
    <asp:HiddenField ID="numBarterSegments" runat="server" />
    <asp:HiddenField ID="market" runat="server" />
    <asp:HiddenField ID="isMissingSplit" runat="server" />
    <asp:HiddenField ID="isBanner" runat="server" />
    <asp:HiddenField ID="isApprover" runat="server" />
    <asp:HiddenField ID="companyId" runat="server" />
    <asp:ImageButton ID="excelExport" runat="server" 
        ImageUrl="/Images/but_excel.gif" AlternateText="Export To Excel" 
        onclick="excelExport_Click" style="margin-left:10px;" OnClientClick="onExportClick();" />
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

