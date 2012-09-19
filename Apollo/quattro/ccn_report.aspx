<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="ccn_report.aspx.cs" Inherits="Apollo.quattro_ccn_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
        .notificationButton {font-size:10px !important;background-color:#0088EE;color:#ffffff;font-weight:bold;border-radius:5px;}
        .completionButton {font-size:10px !important;background-color:#00AA33;color:#ffffff;font-weight:bold;border-radius:5px;}
    </style>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">
        var selectedNoteRow;
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
            jQuery("#attachmentPopupDialog").dialog("open");
            //$find('attachmentPopupExtBehavior').show();
        }
        function ExecuteSearch() {
            if (!ValidateDates()) { return; }
            var attachmentFrom = $get('<%=textAttachmentDateFrom.ClientID %>').value;
            var attachmentTo = $get('<%=textAttachmentDateTo.ClientID %>').value;
            var filterObject = BuildFilterObject(attachmentFrom, attachmentTo);
            var postParams = BuildPostParams(filterObject);
            $('#list1').setGridParam({ postData: postParams }).trigger('reloadGrid');
        }
        function ClearSearch() {
            var attachmentFromDate = new Date();
            attachmentFromDate.setDate(attachmentFromDate.getDate() - 15);
            var attachmentToDate = new Date();
            $get("<%=textAttachmentDateFrom.ClientID %>").value = GetDateAsString(attachmentFromDate);
            $get("<%=textAttachmentDateTo.ClientID %>").value = GetDateAsString(attachmentToDate);
            var attachmentFrom = $get('<%=textAttachmentDateFrom.ClientID %>').value;
            var attachmentTo = $get('<%=textAttachmentDateTo.ClientID %>').value;
            var filterObject = BuildFilterObject(attachmentFrom, attachmentTo);
            var postParams = BuildPostParams(filterObject);
            $('#list1').setGridParam({ postData: postParams }).trigger('reloadGrid');
        }
        function BuildFilterObject(fromDate, toDate) {
            var filterObject = { 'filters': [
                { name: 'fromDate', value: fromDate },
                { name: 'toDate', value: toDate }                
            ]
            };
            return filterObject;
        }
        function BuildPostParams(filterObject) {
            var postParams = { fromDate: filterObject.filters[0].value,
                toDate: filterObject.filters[1].value                
            };
            return postParams;
        }
        function ValidateDates() {
            var attachmentFrom = $get('<%=textAttachmentDateFrom.ClientID %>').value;
            var attachmentTo = $get('<%=textAttachmentDateTo.ClientID %>').value;
            if (attachmentFrom != "") {
                if (!IsValidDate(attachmentFrom)) {
                    alert('Attachment From Date is not in a valid format.');
                    return false;
                }
            }
            if (attachmentTo != "") {
                if (!IsValidDate(attachmentTo)) {
                    alert('Attachment To Date is not in a valid format.');
                    return false;
                }
            }
            if (Date(attachmentTo) < Date(attachmentFrom)) {
                alert('Attachment To Date can not be earlier than Attachment From Date.');
                return false;
            }
            return true;
        }
        $(document).ready(function () {
            var canEdit = ($get("<%=canEdit.ClientID %>").value == "-1");
            var attachmentFromDate = new Date(2002, 0, 1);
            var lastsel;
            var attachmentToDate = new Date();
            $get("<%=textAttachmentDateFrom.ClientID %>").value = GetDateAsString(attachmentFromDate);
            $get("<%=textAttachmentDateTo.ClientID %>").value = GetDateAsString(attachmentToDate);
            var filterObject = BuildFilterObject($get("<%=textAttachmentDateFrom.ClientID %>").value, $get("<%=textAttachmentDateTo.ClientID %>").value);
            var postParams = BuildPostParams(filterObject);
            $("#attachmentPopupDialog").dialog({ autoOpen: false, modal: true, width: 600, height: 400, title: "Contract CCN Report" });
            $("#notesDialog").dialog({ autoOpen: false, modal: true, title: "Contract Notes", width: 325, height: 250 });
            $("#notesDialog").dialog("option", "buttons",
                {
                    "Cancel": function () {
                        $(this).dialog("close");
                    }, "Save": SaveNotes
                }
            );
            $("#list1").jqGrid({
                url: '../services/QuattroService.asmx/GetContractCCNReport'
                , editurl: "../services/QuattroService.asmx/AddUpdateCCNNote"
                , datatype: "xml"
                , postData: postParams
                , colNames: ['View Attachments', 'Uid Cmpgn', 'Company Name', 'Email', 'Attachment Date', 'Attachment ID', 'Attachment ID Int', 'Contract #', 'Attachment Ext.', 'Attachment Name', 'Attachment Title', 'Attachment Desc.', 'Attachment Type', 'Attached By User', 'Notes', 'Notification Sent?', 'Completion Sent?']
                , colModel: [{ name: 'viewAttachmentButton', index: 'VIEW_ATTACHMENT_BUTTON', search: false, width: 100 }
                    , { name: 'uidCmpgn', index: 'uid_cmpgn', hidden: true }
                    , { name: 'companyName', index: 'COMPANY_NAME', width: 85, align: 'center', stype: 'select', editoptions: { value: ":ALL;1:Titan US;2:Titan Canada"} }
                    , { name: "sendEmailButton", index: "SEND_EMAIL_BUTTON", search: false, width: 128, hidden: !canEdit }
                    , { name: 'attachmentDate', index: 'ATTACHMENT_DATE', width: 125, align: 'right', search: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y H:i:s'} }
                    , { name: 'attachmentId', index: 'ATTACHMENT_ID', hidden: true }
                    , { name: 'attachmentIdInt', index: 'ATTACHMENT_ID_INT', hidden: true }
                    , { name: 'contractNumber', index: 'CONTRACT_NUMBER', width: 80, align: 'center', search: true }
                    , { name: 'attachmentExtension', index: 'ATTACHMENT_EXT', width: 50, align: 'center', search: false }
                    , { name: 'attachmentName', index: 'ATTACHMENT_NAME', width: 175, search: true }
                    , { name: 'attachmentTitle', index: 'ATTACHMENT_TITLE', width: 175, search: true }
                    , { name: 'attachmentDesc', index: 'ATTACHMENT_DESC', width: 200, search: true }
                    , { name: 'attachmentType', index: 'ATTACHMENT_TYPE', hidden: true, search: false }
                    , { name: "attachedByUserId", index: "ATTACHED_BY_USER_ID", hidden: !canEdit }
                    , { name: 'notes', index: 'NOTE_TEXT', width: 175, search: true, sort: true, align: 'center', editable: false }
                    , { name: "isNotificationEmailSent", index: "IS_NOTIFICATION_EMAIL_SENT", hidden: true }
                    , { name: "isCompletionEmailSent", index: "IS_NOTIFICATION_EMAIL_SENT", hidden: true }
                ]
                , rowNum: 100
                , height: 600
                , width: "100%"
                , rowList: [25, 50, 100, 250]
                , pager: '#pager1'
                , sortname: 'ATTACHMENT_DATE'
                , sortorder: 'DESC'
                , caption: "CCN Attachments"
                , viewrecords: true
                , toolbar: [true, 'top']
                , onSelectRow: function (id) {
                    if (!canEdit) { return; }
                    selectedNoteRow = { id: id
                        , uidCmpgn: $('#list1').getRowData(id)['uidCmpgn']
                        , companyId: ($("#list1").getRowData(id)["companyName"].toLowerCase() == "titan us" ? "1" : "2")
                        , attachmentId: $("#list1").getRowData(id)["attachmentIdInt"]
                    };
                    $("#notesDialog").dialog({ title: ($("#list1").getRowData(id)["companyName"] + " - " + $("#list1").getRowData(id)["contractNumber"]) + " Notes" });
                    $("#notesDialog").dialog("open");
                    if ($("#list1").getRowData(id)["notes"] == "") {
                        $("#textNotes").val(GetCurrentDateAsString());
                    } else {
                        $("#textNotes").val($("#list1").getRowData(id)["notes"])
                    }
                }
                , gridComplete: function () {
                    var ids = $("#list1").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var uidCmpgn = $("#list1").getRowData(cl)["uidCmpgn"];
                        var contractNumber = $("#list1").getRowData(cl)["contractNumber"];
                        var companyId = ($("#list1").getRowData(cl)["companyName"].toLowerCase() == "titan us" ? "1" : "2");
                        var attachedByUserId = $("#list1").getRowData(cl)["attachedByUserId"];
                        var attachmentId = $("#list1").getRowData(cl)["attachmentIdInt"];
                        var isNotificationEmailSent = ($("#list1").getRowData(cl)["isNotificationEmailSent"] != "0");
                        var isCompletionEmailSent = ($("#list1").getRowData(cl)["isCompletionEmailSent"] != "0");
                        var viewAttachmentButton = "<input style='font-size:10px' type='button' value='View Attachment' onclick='ShowAttachmentPopup(\"" + contractNumber + "\",\"" + companyId + "\");' />";
                        var sendEmailButton = "";
                        if (!isNotificationEmailSent) {
                            sendEmailButton = "<input class='notificationButton' type='button' value='Send Notification Email' onclick='SendNotificationEmail(" + uidCmpgn + ",\"" + contractNumber + "\"," + companyId + ",\"" + attachedByUserId + "\"," + attachmentId + ");' />";
                        }
                        if (isNotificationEmailSent && !isCompletionEmailSent) {
                            sendEmailButton = "<input class='completionButton' type='button' value='Send Completion Email' onclick='SendCompletionEmail(" + uidCmpgn + ",\"" + contractNumber + "\"," + companyId + ",\"" + attachedByUserId + "\"," + attachmentId + ");' />";
                        }
                        if (isNotificationEmailSent && isCompletionEmailSent) {
                            sendEmailButton = "<span>All Emails have been Sent</span>";
                        }
                        $("#list1").jqGrid('setRowData', ids[i], { viewAttachmentButton: viewAttachmentButton });
                        $("#list1").jqGrid('setRowData', ids[i], { sendEmailButton: sendEmailButton });
                    }
                }
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false, search: false });
            $("#list1").jqGrid('filterToolbar', { searchOnEnter: true });
        });
        function SendNotificationEmail(uidCmpgn, contractNumber, companyId, attachmentUserId, attachmentId) {
            //alert("uid_cmpgn:\t" + uidCmpgn + "\nContract #:\t" + contractNumber + "\nCompany Id:\t" + companyId + "\nAttachment User Id:\t" + attachmentUserId);
            Apollo.QuattroService.SendCCNNotificationEmail(uidCmpgn, companyId, contractNumber, attachmentUserId, attachmentId, SendEmailCallback, EmailErrorCallback);
        }
        function SendCompletionEmail(uidCmpgn, contractNumber, companyId, attachmentUserId, attachmentId) {
            //alert("uid_cmpgn:\t" + uidCmpgn + "\nContract #:\t" + contractNumber + "\nCompany Id:\t" + companyId + "\nAttachment User Id:\t" + attachmentUserId);
            Apollo.QuattroService.SendCCNCompletionEmail(uidCmpgn, companyId, contractNumber, attachmentUserId, attachmentId, SendEmailCallback, EmailErrorCallback);
        }
        function SendEmailCallback(result) {
            $("#list1").trigger("reloadGrid");
            if (result) {
                alert("Email has been sent.");
            } else {
                alert("Email was already sent by another user.");
            }
        }
        function SaveNotes() {
            if ($("#textNotes").val() == "") { alert("Please enter a note."); return; }
            Apollo.QuattroService.AddUpdateCCNNote(selectedNoteRow.id, selectedNoteRow.uidCmpgn, selectedNoteRow.attachmentId, selectedNoteRow.companyId, $("#textNotes").val(), SaveNotesCallback, ErrorCallback);
        }
        function SaveNotesCallback(response){
            if (response) {
                alert('Entry Saved');
                $("#list1").trigger("reloadGrid");
                $("#notesDialog").dialog("close");
            } else {
                alert('An error occurred while trying to save your Entry.');
            }            
        }
        function GetCurrentDateAsString() {
            return ("" + (new Date().getMonth() + 1) + "/" + new Date().getDate() + "/" + new Date().getFullYear());
        }
        function onExportClick() {
        }
        function EmailErrorCallback(e) {
            alert(e._message);
        }
    </script>
    <div style="margin:15px;">
        <div>
            <div style="float:left">
                CCN Attachment Date:
            </div>
            <div style="float:left;padding-left:10px">
                From: <asp:TextBox ID="textAttachmentDateFrom" runat="server" Width="65px" />
                <img src="../Images/calendar.gif" id="calImage1" runat="server" alt="Click for Calendar" class="calendarPopup" />
                <ajax:CalendarExtender ID="textAttachmentDateFrom_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="textAttachmentDateFrom" PopupButtonID="calImage1" />                                
            </div>
            <div style="float:left;padding-left:10px">
                To: <asp:TextBox ID="textAttachmentDateTo" runat="server" Width="65px" />
                <img src="../Images/calendar.gif" id="calImage2" runat="server" alt="Click for Calendar" class="calendarPopup" />
                <ajax:CalendarExtender ID="textAttachmentDateTo_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="textAttachmentDateTo" PopupButtonID="calImage2" />                                
            </div>
            <div style="clear:both"></div>
        </div>
        <div>
            <div style="float:left;width:10%;text-align:left;padding-top:10px;">                                         
                <img id="clear" runat="server" style="cursor:pointer" src="~/Images/but_clear.gif" onclick="ClearSearchFilters();" alt="Clear Search Filters" />
                <img id="search" runat="server" style="cursor:pointer" src="~/Images/but_search.gif" onclick="ExecuteSearch();" alt="Search" />                                                                                                                                
                <asp:ImageButton ID="excelExport" runat="server" 
                    ImageUrl="/Images/but_excel.gif" AlternateText="Export To Excel" 
                    onclick="excelExport_Click" style="margin-left:10px;" OnClientClick="onExportClick();" />
            </div>          
            <div style="clear:both"></div>
        </div>
    </div>
    <div style="margin:15px 0 0 15px;">
        <div style="width:95%">
            <div style="width:95%;margin:10px;z-index:-1"><table id="list1"></table></div>
            <div id="pager1"></div>
        </div>
    </div>
    <asp:HiddenField ID="canEdit" runat="server" />
    <asp:Button ID="showAttachmentPopup" runat="server" style="display:none" />
    <div id="attachmentPopupDialog">
        <div id="attachmentPopupDetail"></div>
    </div>
    <div id="notesDialog">
        <textarea id="textNotes" rows="10" cols="50"></textarea>
    </div>
</asp:Content>

