<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="newEmployeeForm.aspx.cs" Inherits="Apollo.HR_newEmployeeForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        fieldset {border:1px solid #00CCFF;padding:5px;margin-bottom:10px;width:700px;}
        legend {font-weight:bold;font-size:12px;padding:2px;margin-bottom:5px;background-color:#00CCFF;color:#ffffff}
        .fieldRow {margin-bottom:10px;}
        .fieldLeftColumn {float:left;margin:0 25px 0 5px;width:245px;text-align:left;}
        .fieldRightColumn {float:left;text-align:left;}
        .dateField {text-align:right;width:65px;}
        .deskPhoneSetup, .mobilePhoneSetup, .existingWorkstationInfo, .newWorkstationInfo {display:none;}
        .watermark {}
        .requiredFlag {color:Red;font-size:10px;font-style:italic;padding-left:3px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
    <script type="text/javascript" language="javascript">
        var userRequest;
        function getWatermark(id) { if ($("#" + id).attr("title") === undefined) { return "" } return $("#" + id).attr("title"); }
        function setWatermark() { $(".watermark").each(function () { $(this).val($(this).attr("title")); $(this).css({ "color": "#888888" }); }); }
        function watermarkBlur() { if ($(this).val() == "") { $(this).val($(this).attr("title")); $(this).css({ "color": "#888888" }); } }
        function watermarkFocus() { if ($(this).val() == $(this).attr("title")) { $(this).val(""); $(this).css({ "color": "#333333" }); } }
        function watermarkPopulate() { $(this).css({ "color": ($(this).val() == $(this).attr("title") ? "#888888" : "#333333") }); }
        $(document).ready(function () {
            $("input:checkbox").attr("checked", false);
            $("input:radio").attr("checked", false);
            $("select").val("");
            //$("#radioEmployeeReplace, #radioEmployeeNew, #radioWorkstationTypeNew, #radioWorkstationTypeExisting, #checkDesktopPhone, #checkMobilePhone, #radioExistingPhoneYes, #radioExistingPhoneNo").attr("checked", false);
            $(".watermark").bind("blur", watermarkBlur);
            $(".watermark").bind("focus", watermarkFocus);
            setWatermark();
            $("#textEmployeeStartDate").val("");
            $(".dateField").datepicker();
            Apollo.TitanADService.GetOffices("", AddToList, null, "dropDownEmployeeOffice");
            Apollo.TitanADService.GetDepartments("", AddToList, null, "dropDownEmployeeDept");
            if (window.location.search != "") {
                var queryParams = window.location.search.substring(1).split("&");
                for (var i = 0; i < queryParams.length; i++) {
                    if (queryParams[i].split("=")[0].toLowerCase() == "requestid") {
                        setTimeout("Apollo.TitanADService.GetNewUserRequest(" + queryParams[i].split("=")[1] + ",LoadNewUserRequestCallback,ErrorCallback);", 500);
                    }
                }
            }
        });
        function onNewEmployeeClick(rdo) { $("#replacedUser").css({ "display": (rdo.checked ? "none" : "block") }); }
        function onReplacingEmployeeClick(rdo) { $("#replacedUser").css({ "display": (rdo.checked ? "block" : "none") }); }
        function onNewComputerClick(rdo) { $(".existingWorkstationInfo").css({ "display": (rdo.checked ? "none" : "block") }); $(".newWorkstationInfo").css({ "display": (rdo.checked ? "block" : "none") }); }
        function onExistingWorkstationClick(rdo) { $(".existingWorkstationInfo").css({ "display": (rdo.checked ? "block" : "none") }); $(".newWorkstationInfo").css({ "display": (rdo.checked ? "none" : "block") }); }
        function onDeskPhoneClick(chk) {
            $(".deskPhoneSetup").css({ "display": (chk.checked ? "block" : "none") });
            $("#existingPhoneNumber").css({ "display": "none" });
            $("#newPhoneInfo").css({ "display": "none" });
        }
        function onMobilePhoneClick(chk) { $(".mobilePhoneSetup").css({ "display": (chk.checked ? "block" : "none") }); }
        function onExistingPhoneClick(rdo) { $("#existingPhoneNumber").css({ "display": (rdo.checked ? "block" : "none") }); $("#newPhoneInfo").css({ "display": (rdo.checked ? "none" : "block") }); }
        function onNoExistingPhoneClick(rdo) { $("#existingPhoneNumber").css({ "display": (rdo.checked ? "none" : "block") }); $("#newPhoneInfo").css({ "display": (rdo.checked ? "block" : "none") }); }
        function ValidateEntry() {
            if (!ValidateRequestorFields()) { return false; }
            if (!ValidateNewEmployeeFields()) { return false; }
            if (!ValidatePCRequirementFields()) { return false; }
            if (!ValidatePhoneSetupInfo()) { return false; }
            if (!ValidateEmailInfo()) { return false; }
            var newUserRequest = BuildUserRequest();
            Apollo.TitanADService.ProcessNewUserRequest(newUserRequest, ProcessNewUserRequestCallback, ErrorCallback);
            return true;
        }
        function ProcessNewUserRequestCallback(newRequestId) {
            alert("New User Request created.\n\tRequest Id: " + newRequestId);
        }
        function LoadNewUserRequest(requestId) {
            var someDate = new Date();
            someDate.toDateString();
        }
        function LoadNewUserRequestCallback(newUserRequest) {
            userRequest = newUserRequest;
            $get("<%=textRequestorName.ClientID %>").value = newUserRequest.requestorName;
            $get("<%=textRequestorNTID.ClientID %>").value = newUserRequest.requestorId;
            $get("<%=textRequestDate.ClientID %>").value = ((newUserRequest.requestDate.getMonth() + 1) + "/" + newUserRequest.requestDate.getDate() + "/" + newUserRequest.requestDate.getFullYear());
            $get("<%=textRequestorEmail.ClientID %>").value = newUserRequest.requestorEmail;
            $("#textEmployeeName").val(newUserRequest.newEmployeeName);
            $("#textEmployeeStartDate").val(((newUserRequest.newEmployeeStartDate.getMonth() + 1) + "/" + newUserRequest.newEmployeeStartDate.getDate() + "/" + newUserRequest.newEmployeeStartDate.getFullYear()));
            $("#textEmployeeTitle").val(newUserRequest.newEmployeeTitle);
            //Since these drop downs are being populated asynchronously, delay the data population
            setTimeout("$(\"#dropDownEmployeeOffice\").val(\"" + newUserRequest.newEmployeeOffice + "\");", 3000);
            setTimeout("$(\"#dropDownEmployeeDept\").val(\"" + newUserRequest.newEmployeeDept + "\");", 3000);
            //$("#dropDownEmployeeDept").val(newUserRequest.newEmployeeDept);
            $("#radioEmployeeReplace").attr("checked", newUserRequest.isReplacingPreviousEmployee);
            $("#radioEmployeeNew").attr("checked", !newUserRequest.isReplacingPreviousEmployee);
            if (newUserRequest.isReplacingPreviousEmployee) {
                if (newUserRequest.previousEmployeeName != "") { $("#textEmployeeReplacingUser").val(newUserRequest.previousEmployeeName); }
                $("#replacedUser").css({ "display": (newUserRequest.isReplacingPreviousEmployee ? "block" : "none") });
            }
            $("#radioWorkstationTypeNew").attr("checked", newUserRequest.requiresNewWorkstation);
            $("#radioWorkstationTypeExisting").attr("checked", !newUserRequest.requiresNewWorkstation);
            $(".existingWorkstationInfo").css({ "display": (newUserRequest.requiresNewWorkstation ? "none" : "block") });
            $(".newWorkstationInfo").css({ "display": (newUserRequest.requiresNewWorkstation ? "block" : "none") });
            if (newUserRequest.requiresNewWorkstation) {
                if (newUserRequest.newWorkstationType != "") { $("#dropDownWorkstationType").val(newUserRequest.newWorkstationType); }
                if (newUserRequest.newWorkstationAdditionalRequest != "") { $("#textAdditionalRequests").val(newUserRequest.newWorkstationAdditionalRequest); }
                if (newUserRequest.newWorkstationPeripherals != "") { $("#textPeripherals").val(newUserRequest.newWorkstationPeripherals); }                
            } else {
                if (newUserRequest.existingWorkstationLocation != "") { $("#textExistingWorkstationLocation").val(newUserRequest.existingWorkstationLocation); }
                if (newUserRequest.existingWorkstationIp != "") { $("#textExistingWorkstationIp").val(newUserRequest.existingWorkstationIp); }
            }
            $("#checkDesktopPhone").attr("checked", newUserRequest.requiresDesktopPhone);
            $(".deskPhoneSetup").css({ "display": (newUserRequest.requiresMobilePhone ? "block" : "none") });
            $("#existingPhoneNumber").css({ "display": "none" });
            $("#newPhoneInfo").css({ "display": "none" });

            $("#radioExistingPhoneYes").attr("checked", newUserRequest.isExistingDesktopPhone);
            $("#radioExistingPhoneNo").attr("checked", !newUserRequest.isExistingDesktopPhone);
            $("#existingPhoneNumber").css({ "display": (newUserRequest.isExistingDesktopPhone ? "block" : "none") });
            $("#newPhoneInfo").css({ "display": (newUserRequest.isExistingDesktopPhone ? "none" : "block") });
            $("#textExistingPhoneNumber").val(newUserRequest.existingDesktopPhoneNumber);

            $("#textNewPhoneInfo").val(newUserRequest.newPhoneInfo);
            $("#checkMobilePhone").attr("checked", newUserRequest.requiresMobilePhone);
            $(".mobilePhoneSetup").css({ "display": (newUserRequest.requiresMobilePhone ? "block" : "none") });

            
            $("#dropDownMobilePhoneType").val(newUserRequest.mobilePhoneType);
            if (newUserRequest.mobilePhoneAdditionalRequests != "") { $("#textAdditionalMobilePhoneRequests").val(newUserRequest.mobilePhoneAdditionalRequests); }
            if (newUserRequest.newEmployeeEmail != "") { $("#textPrimaryEmail").val(newUserRequest.newEmployeeEmail); }
            if (newUserRequest.additionalEmailAliases != "") { $("#textAdditionalEmail").val(newUserRequest.additionalEmailAliases); }
            if (newUserRequest.distributionLists != "") { $("#textDistributionLists").val(newUserRequest.distributionLists); }
            if (newUserRequest.departmentShares != "") { $("#textDeptShares").val(newUserRequest.departmentShares); }
            if (newUserRequest.remoteAccess != "") { $("#textRemoteAccess").val(newUserRequest.remoteAccess); }
            if (newUserRequest.nonStandardApps != "") { $("#textNonStandardApps").val(newUserRequest.nonStandardApps); }
            if (newUserRequest.printerAccess != "") { $("#textPrinterAccess").val(newUserRequest.printerAccess); }
            $(".watermark").each(watermarkPopulate);
        }
        function BuildUserRequest() {
            var userRequest = { requestId: -1
                , requestorName: $get("<%=textRequestorName.ClientID %>").value
                , requestorId: $get("<%=textRequestorNTID.ClientID %>").value
                , requestDate: $get("<%=textRequestDate.ClientID %>").value
                , requestorEmail: $get("<%=textRequestorEmail.ClientID %>").value
                , newEmployeeName: $("#textEmployeeName").val()
                , newEmployeeStartDate: $("#textEmployeeStartDate").val()
                , newEmployeeTitle: $("#textEmployeeTitle").val()
                , newEmployeeOffice: $("#dropDownEmployeeOffice").val()
                , newEmployeeDept: $("#dropDownEmployeeDept").val()
                , isReplacingPreviousEmployee: $("#radioEmployeeReplace").is(":checked")
                , previousEmployeeName: $("#radioEmployeeReplace").attr("checked") ? $("#textEmployeeReplacingUser").val() : ""
                , requiresNewWorkstation: $("#radioWorkstationTypeNew").is(":checked")
                , newWorkstationType: $("#radioWorkstationTypeNew").attr("checked") ? $("#dropDownWorkstationType").val() : ""
                , newWorkstationAdditionalRequest: ($("#textAdditionalRequests").val() != getWatermark("textAdditionalRequests")) ? $("#textAdditionalRequests").val() : ""
                , newWorkstationPeripherals: ($("#textPeripherals").val() != getWatermark("textPeripherals")) ? $("#textPeripherals").val() : ""
                , existingWorkstationLocation: $("#radioWorkstationTypeExisting").attr("checked") ? $("#textExistingWorkstationLocation").val() : ""
                , existingWorkstationIp: $("#radioWorkstationTypeExisting").attr("checked") ? $("#textExistingWorkstationIp").val() : ""
                , requiresDesktopPhone: $("#checkDesktopPhone").is(":checked")
                , isExistingDesktopPhone: $("#radioExistingPhoneYes").is(":checked")
                , existingDesktopPhoneNumber: ($("#textExistingPhoneNumber").val() != getWatermark("textExistingPhoneNumber")) ? $("#textExistingPhoneNumber").val() : ""
                , newPhoneInfo: ($("#textNewPhoneInfo").val() != getWatermark("textNewPhoneInfo")) ? $("#textNewPhoneInfo").val() : ""
                , requiresMobilePhone: $("#checkMobilePhone").is(":checked")
                , mobilePhoneType: $("#dropDownMobilePhoneType").val()
                , mobilePhoneAdditionalRequests: ($("#textAdditionalMobilePhoneRequests").val() != getWatermark("textAdditionalMobilePhoneRequests")) ? $("#textAdditionalMobilePhoneRequests").val() : ""
                , newEmployeeEmail: $("#textPrimaryEmail").val()
                , additionalEmailAliases: ($("#textAdditionalEmail").val() != getWatermark("textAdditionalEmail")) ? $("#textAdditionalEmail").val() : ""
                , distributionLists: ($("#textDistributionLists").val() != getWatermark("textDistributionLists")) ? $("#textDistributionLists").val() : ""
                , departmentShares: ($("#textDeptShares").val() != getWatermark("textDeptShares")) ? $("#textDeptShares").val() : ""
                , remoteAccess: ($("#textRemoteAccess").val() != getWatermark("textRemoteAccess")) ? $("#textRemoteAccess").val() : ""
                , nonStandardApps: ($("#textNonStandardApps").val() != getWatermark("textNonStandardApps")) ? $("#textNonStandardApps").val() : ""
                , printerAccess: ($("#textPrinterAccess").val() != getWatermark("textPrinterAccess")) ? $("#textPrinterAccess").val() : ""
            };
            return userRequest;
        }
        function ValidateRequestorFields() {
            var isValid = true;
            var msg = "The requestor fields must be completed:";
            if ($get("<%=textRequestorName.ClientID %>").value == "") { isValid = false; msg += "\n\tRequestor Name is a required field."; }
            if ($get("<%=textRequestDate.ClientID %>").value == "") { isValid = false; msg += "\n\tRequest Date is a required field."; }
            if ($get("<%=textRequestorEmail.ClientID %>").value == "") { isValid = false; msg += "\n\tRequestor E-Mail is a required field."; }
            if (!isValid) { alert(msg); }
            return isValid;
        }
        function ValidateNewEmployeeFields() {
            var isValid = true;
            var msg = "Please correct the following errors in the\nNew Employee Information Section:";
            if ($("#textEmployeeName").val() == "" || $("#textEmployeeName").val() == getWatermark("textEmployeeName")) { isValid = false; msg +="\n\tName cannot be blank."; }
            if ($("#textEmployeeStartDate").val() == "" || $("#textEmployeeStartDate").val() == getWatermark("textEmployeeStartDate")) {
                isValid = false;
                msg += "\n\tStart Date cannot be blank.";
            } else {
                if (!IsValidDate($("#textEmployeeStartDate").val())) { isValid = false; msg += "\n\tStart Date must be in a valid Date format."; }
            }
            if ($("#textEmployeeTitle").val() == "" || $("#textEmployeeTitle").val() == getWatermark("textEmployeeTitle")) { isValid = false; msg += "\n\tTitle cannot be blank."; }
            if ($("#dropDownEmployeeOffice").val() == "") { isValid = false; msg +="\n\tOffice cannot be blank."; }
            if ($("#dropDownEmployeeDept").val() == "") { isValid = false; msg +="\n\tDepartment cannot be blank."; }
            if (!$("#radioEmployeeReplace").attr("checked") && !$("#radioEmployeeNew").attr("checked")) { isValid = false; msg +="\n\tPlease indicate if this Employee is replacing a previous Employee."; }
            if ($("#radioEmployeeReplace").attr("checked") && ($("#textEmployeeReplacingUser").val() == "" || $("#textEmployeeReplacingUser").val() == getWatermark("textEmployeeReplacingUser"))) { isValid = false; msg += "\n\tPlease specify the name of the Employee being replaced."; }
            if (!isValid) { alert(msg); }
            return isValid;
        }
        function ValidatePCRequirementFields() {
            var isValid = true;
            var msg = "Please correct the following errors in the PC Requirements Section:";
            if (!$("#radioWorkstationTypeNew").attr("checked") && !$("#radioWorkstationTypeExisting").attr("checked")) { isValid = false; msg +="\n\tPlease indicate which type of Workstation the New Employee requires."; }
            if ($("#radioWorkstationTypeNew").attr("checked") && $("#dropDownWorkstationType").val() == "") { isValid = false; msg +="\n\tPlease specify which type of Workstation Type is required."; }
            if ($("#radioWorkstationTypeExisting").attr("checked") && $("#textExistingWorkstationLocation").val() == "") { isValid = false; msg +="\n\tPlease specify the location of the existing Workstation."; }
            if ($("#radioWorkstationTypeExisting").attr("checked") && $("#textExistingWorkstationIp").val() == "") { isValid = false; msg +="\n\tPlease enter the Hostname/IP Address of the existing Workstation."; }
            if (!isValid) { alert(msg); }
            return isValid;
        }
        function ValidatePhoneSetupInfo() {
            var isValid = true;
            var msg = "Please correct the following errors in the Phone Setup Section:";
            if (!$("#checkDesktopPhone").attr("checked") && !$("#checkMobilePhone").attr("checked")) { isValid = false; msg +="\n\tPlease indicate which type(s) of Phone will be required."; }
            if ($("#checkDesktopPhone").attr("checked") && !$("#radioExistingPhoneYes").attr("checked") && !$("#radioExistingPhoneNo").attr("checked")) { isValid = false; msg +="\n\tPlease indicate if there is an existing phone."; }
            if ($("#checkDesktopPhone").attr("checked") && $("#radioExistingPhoneYes").attr("checked") && $("#textExistingPhoneNumber").val() == "") { isValid = false; msg +="\n\tPlease specify the existing phone number."; }
            if ($("#checkDesktopPhone").attr("checked") && $("#radioExistingPhoneNo").attr("checked") && $("#textNewPhoneInfo").val() == "") { isValid = false; msg +="\n\tPlease specify the location of the new phone."; }
            if ($("#checkMobilePhone").attr("checked") && $("#dropDownMobilePhoneType").val() == "") { isValid = false; msg +="\n\tPlease specify the type of Mobile Phone required."; }
            if (!isValid) { alert(msg); }
            return isValid;
        }
        function ValidateEmailInfo() {
            var isValid = true;
            var msg = "Please correct the following errors in the E-mail Setup Section:";
            if ($("#textPrimaryEmail").val() == "") {
                isValid = false;
                msg += "\n\tPlease specify the New Employee's E-mail address.";
            } else {
                if (!IsValidEmail($("#textPrimaryEmail").val())) { isValid = false; msg += "\n\tNew Employee's E-mail address must be in the proper format."; }
            }
            if (!isValid) { alert(msg); }
            return isValid;
        }
    </script>
    <div id="" style="margin:10px;">
        <fieldset id="requestorInfo">
            <legend>Requestor Information</legend>
            <div class="fieldRow">
                <div class="fieldLeftColumn">Requested By:</div>
                <div class="fieldRightColumn">
                    <asp:TextBox runat="server" ID="textRequestorName" Width="200" ReadOnly="true" />
                    <asp:HiddenField runat="server" ID="textRequestorNTID" />
                </div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow">
                <div class="fieldLeftColumn"><label for="textRequestDate">Request Date:</label></div>
                <div class="fieldRightColumn"><asp:TextBox ID="textRequestDate" runat="server" ReadOnly="true" CssClass="dateField" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow">
                <div class="fieldLeftColumn">Requestor E-mail:</div>
                <div class="fieldRightColumn"><asp:TextBox runat="server" ID="textRequestorEmail" Width="200" ReadOnly="true" /></div>
                <div style="clear:both;"></div>
            </div>
        </fieldset>
        <fieldset id="newEmployeeInfo">
            <legend>New Employee Information</legend>
            <div class="fieldRow">
                <div class="fieldLeftColumn"><label for="textEmployeeName">Name:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textEmployeeName" class="watermark" title="Enter New Employee Name" style="width:200px;" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow">
                <div class="fieldLeftColumn"><label for="textEmployeeStartDate">Start Date:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textEmployeeStartDate" class="dateField" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow">
                <div class="fieldLeftColumn"><label for="textEmployeeTitle">Title:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textEmployeeTitle" class="watermark" style="width:200px;" title="Enter New Employee Title" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow">
                <div class="fieldLeftColumn"><label for="dropDownEmployeeOffice">Office:</label></div>
                <div class="fieldRightColumn"><select id="dropDownEmployeeOffice"></select></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow">
                <div class="fieldLeftColumn"><label for="dropDownEmployeeDept">Department:</label></div>
                <div class="fieldRightColumn"><select id="dropDownEmployeeDept"></select></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow">
                <div class="fieldLeftColumn">Is this user replacing a previous employee or is this a new role?</div>
                <div class="fieldRightColumn" style="vertical-align:middle;">
                    <input type="radio" name="radioNewEmployeeType" id="radioEmployeeReplace" onclick="onReplacingEmployeeClick(this);" />
                    <label for="radioEmployeeReplace" style="margin-left:3px;">Replacing</label>
                    <input type="radio" name="radioNewEmployeeType" id="radioEmployeeNew" style="margin-left:10px;" onclick="onNewEmployeeClick(this);" />
                    <label for="radioEmployeeNew" style="margin-left:3px;">New</label>
                </div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow" style="display:none;" id="replacedUser">
                <div class="fieldLeftColumn"><label for="textEmployeeReplacingUser">If replacing previous Employee, who is New Employee replacing?:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textEmployeeReplacingUser" class="watermark" style="width:200px;" title="Enter Name of Employee being replaced." /></div>
                <div style="clear:both;"></div>
            </div>
        </fieldset>
        <fieldset id="pcRequirements">
            <legend>PC Requirements</legend>
            <div class="fieldRow">
                <div class="fieldLeftColumn">Does this user require a new computer or will they be using an existing workstation?:</div>
                <div class="fieldRightColumn" style="vertical-align:middle;">
                    <input type="radio" name="radioWorkstationType" id="radioWorkstationTypeNew" onclick="onNewComputerClick(this);" />
                    <label for="radioWorkstationTypeNew" style="margin-left:3px;">New Computer</label>
                    <input type="radio" name="radioWorkstationType" id="radioWorkstationTypeExisting" style="margin-left:10px;" onclick="onExistingWorkstationClick(this);" />
                    <label for="radioWorkstationTypeExisting" style="margin-left:3px;">Existing Workstation</label>
                </div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow existingWorkstationInfo" id="existingWorkstationLocation" style="display:none;">
                <div class="fieldLeftColumn"><label for="textExistingWorkstationLocation">Existing Workstation Location:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textExistingWorkstationLocation" class="watermark" style="width:200px;" title="Please Enter Workstation Location" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow existingWorkstationInfo" id="existingWorkstationIp" style="display:none;">
                <div class="fieldLeftColumn"><label for="textExistingWorkstationIp">Existing Workstation Hostname/IP Address:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textExistingWorkstationIp" class="watermark" style="width:200px;" title="Please Enter Workstation Hostname/IP Address" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow newWorkstationInfo" id="newWorkstationType" style="display:none;">
                <div class="fieldLeftColumn"><label for="dropDownWorkstationType">Select Workstation Type:</label></div>
                <div class="fieldRightColumn">
                    <select id="dropDownWorkstationType">
                        <option value="" selected="selected"> - Choose Workstation Type - </option>
                        <option value="Windows Desktop">Windows Desktop</option>
                        <option value="Windows Laptop">Windows Laptop</option>
                        <option value="Mac Desktop">Mac Desktop</option>
                        <option value="Mac Laptop">Mac Laptop</option>
                    </select>
                </div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow newWorkstationInfo" id="newWorkstationAdditionalRequests" style="display:none;">
                <div class="fieldLeftColumn"><label for="textAdditionalRequests">Additional Requests:</label></div>
                <div class="fieldRightColumn"><textarea id="textAdditionalRequests" title="Click here to enter additional requests, such as: laptop docking station, dual monitor setup, etc. All non-standard configurations will require manager approval." rows="2" cols="50" class="watermark"></textarea></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow newWorkstationInfo" id="newWorkstationAdditionalPeripherals" style="display:none;">
                <div class="fieldLeftColumn"><label for="textPeripherals">Please list any additional peripherals needed (scanners, speakers, etc.):</label></div>
                <div class="fieldRightColumn"><textarea id="textPeripherals" rows="2" cols="50" class="watermark"></textarea></div>
                <div style="clear:both;"></div>
            </div>
        </fieldset>
        <fieldset id="phoneSetupInfo">
            <legend>Phone Setup</legend>
            <div class="fieldRow">
                <div class="fieldLeftColumn">What type of Phone does this user require (select all that apply)?:</div>
                <div class="fieldRightColumn" style="vertical-align:middle;">
                    <input type="checkbox" id="checkDesktopPhone" onclick="onDeskPhoneClick(this);" />
                    <label for="checkDesktopPhone" style="margin-left:3px;">Desktop Phone</label>
                    <input type="checkbox" id="checkMobilePhone" onclick="onMobilePhoneClick(this);" style="margin-left:10px;" />
                    <label for="checkMobilePhone" style="margin-left:3px;">Mobile Phone</label>
                </div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow deskPhoneSetup" style="text-align:left;">
                <h4>Desk Phone Setup</h4>
            </div>
            <div class="fieldRow deskPhoneSetup">
                <div class="fieldLeftColumn">Is there an existing phone?:</div>
                <div class="fieldRightColumn" style="vertical-align:middle;">
                    <input type="radio" name="radioExistingPhone" id="radioExistingPhoneYes" onclick="onExistingPhoneClick(this);" />
                    <label for="radioExistingPhoneYes" style="margin-left:3px;">Yes</label>
                    <input type="radio" name="radioExistingPhone" id="radioExistingPhoneNo" style="margin-left:10px;" onclick="onNoExistingPhoneClick(this);" />
                    <label for="radioExistingPhoneNo" style="margin-left:3px;">No</label>
                </div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow deskPhoneSetup" id="existingPhoneNumber" style="display:none">
                <div class="fieldLeftColumn"><label for="textExistingPhoneNumber">Existing Phone Number:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textExistingPhoneNumber" class="watermark" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow deskPhoneSetup" id="newPhoneInfo" style="display:none">
                <div class="fieldLeftColumn"><label for="textNewPhoneInfo">If a new phone is needed, where is the location? Is there an available data jack?:</label></div>
                <div class="fieldRightColumn"><textarea id="textNewPhoneInfo" rows="2" cols="50" class="watermark"></textarea></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow mobilePhoneSetup" style="text-align:left;">
                <h4>Mobile Phone Setup</h4>
            </div>
            <div class="fieldRow mobilePhoneSetup">
                <div class="fieldLeftColumn"><label for="dropDownMobilePhoneType">Type of Mobile Phone required:</label></div>
                <div class="fieldRightColumn">
                    <select id="dropDownMobilePhoneType">
                        <option value="" selected="selected"> - Choose Mobile Phone Type - </option>
                        <option value="BlackBerry">BlackBerry</option>
                        <option value="Basic Mobile Phone">Basic Mobile Phone</option>
                        <option value="Nextel Phone">Nextel Phone</option>
                    </select>
                </div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow mobilePhoneSetup">
                <div class="fieldLeftColumn"><label for="textAdditionalMobilePhoneRequests">Additional requests:</label></div>
                <div class="fieldRightColumn"><textarea id="textAdditionalMobilePhoneRequests" rows="2" cols="50" class="watermark" title="Please enter any additional needs, such as Headsets."></textarea></div>
                <div style="clear:both;"></div>
            </div>
        </fieldset>
        <fieldset id="emailSetupInfo">
            <legend>E-mail Setup</legend>
            <div class="fieldRow emailSetup">
                <div class="fieldLeftColumn"><label for="textPrimaryEmail">Primary E-mail Address for new user:</label></div>
                <div class="fieldRightColumn"><input type="text" id="textPrimaryEmail" class="watermark" style="width:200px;" /></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow emailSetup">
                <div class="fieldLeftColumn"><label for="textAdditionalEmail">Additional E-mail alias(es), if required:</label></div>
                <div class="fieldRightColumn"><textarea id="textAdditionalEmail" rows="2" cols="50" class="watermark" title="Please enter any additional E-mail addresses needed."></textarea></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow emailSetup">
                <div class="fieldLeftColumn"><label for="textDistributionLists">E-mail distribution list(s) for new user:</label></div>
                <div class="fieldRightColumn"><textarea id="textDistributionLists" rows="2" cols="50" class="watermark" title="Please enter any distribution lists needed."></textarea></div>
                <div style="clear:both;"></div>
            </div>
        </fieldset>
        <fieldset id="otherAccessInfo">
            <legend>Other Access Requirements</legend>
            <div class="fieldRow otherAccess">
                <div class="fieldLeftColumn"><label for="textDeptShares">Department Shares:</label></div>
                <div class="fieldRightColumn"><textarea id="textDeptShares" rows="3" cols="50" class="watermark" title="Please list any department share access needed."></textarea></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow otherAccess">
                <div class="fieldLeftColumn"><label for="textRemoteAccess">Remote Access Capabilities:</label></div>
                <div class="fieldRightColumn"><textarea id="textRemoteAccess" rows="2" cols="50" class="watermark" title="Does user require VPN or remote Terminal Services access? (This does not include Webmail access)"></textarea></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow otherAccess">
                <div class="fieldLeftColumn"><label for="textNonStandardApps">Non-standard applications needed:</label></div>
                <div class="fieldRightColumn"><textarea id="textNonStandardApps" rows="3" cols="50" class="watermark" title="Please list any non-standard applications, such as: Quattro, Great Plains, Acrobat Standard, etc. Standard Applications such as MS Office, Acrobat Reader, Winzip, etc. do not need to be listed."></textarea></div>
                <div style="clear:both;"></div>
            </div>
            <div class="fieldRow otherAccess">
                <div class="fieldLeftColumn"><label for="textPrinterAccess">Printer Access required:</label></div>
                <div class="fieldRightColumn"><textarea id="textPrinterAccess" rows="2" cols="50" class="watermark" title="Please list any specific printers the new employee will print to. Include whether the user requires a specialized printer, etc."></textarea></div>
                <div style="clear:both;"></div>
            </div>            
        </fieldset>        
        <fieldset style="border:none;">
            <div class="fieldRow">
                <div style="float:right;margin-right:5px;"><input type="button" id="submit" value="Submit" onclick="return ValidateEntry();" /></div>
                <div style="clear:both;"></div>
            </div>
        </fieldset>
    </div>
</asp:Content>

