var editingDrawPaymentId;
var editingFlatRateId;
var editingCommissionAmountId;

/************* Draw/Payment Operations *************/
function GetAEDrawPaymentsCallback(aeDrawPayments) {
    var data = "";
    var prevYear = 0;
    $(".drawPaymentHistoryRow").remove();
    if (aeDrawPayments.length == 0) {
        $("#drawPaymentHistoryTable").append("<tr class='drawPaymentHistoryRow'><td colspan='8'>No Draw/Payment data has been entered.</td></tr>");
        $("#drawPaymentHistoryTable").show();
        return;
    }
    for (var i = 0; i < aeDrawPayments.length; i++) {
        if (prevYear != aeDrawPayments[i].DrawPaymentYear) {
            data += "<tr class='drawPaymentHistoryRow'><td class='dashboard_column_head_current' colspan='8'>" + aeDrawPayments[i].DrawPaymentYear + "</td></tr>";
        }
        data += "<tr class='drawPaymentHistoryRow' style='background-color:#ffffcc;'>";
        data += "<td style='width:5%;text-align:center;'><img alt='Edit' style='cursor:pointer;' src='/Images/icon_edit.gif' onclick='EditDrawPaymentRecord(" + aeDrawPayments[i].AEDrawPaymentId + ");' /></td>";
        data += "<td style='width:5%;text-align:center;'><img alt='Delete' style='cursor:pointer;' src='/Images/icon_delete.gif' onclick='DeleteDrawPaymentRecord(" + aeDrawPayments[i].AEDrawPaymentId + ");' /></td>";
        data += "<td style='width:10%;text-align:center'>" + aeDrawPayments[i].DrawPaymentTypeDisplay + "</td>";
        data += "<td style='width:10%;text-align:center'>" + aeDrawPayments[i].DrawPaymentYear + "</td>";
        data += "<td style='width:18%;text-align:center'>" + aeDrawPayments[i].PaymentMonthDisplay + "</td>";
        data += "<td style='width:19%;text-align:center'>" + aeDrawPayments[i].PaymentStatusDisplay + "</td>";
        data += "<td style='width:18%;text-align:center'>" + aeDrawPayments[i].EnteredBy + "</td>";
        data += "<td style='width:15%;text-align:right'>" + aeDrawPayments[i].DrawPaymentAmountDisplay + "</td>";
        data += "</tr>";
        prevYear = aeDrawPayments[i].DrawPaymentYear;
    }
    $("#drawPaymentHistoryTable").append(data);
    $("#drawPaymentHistoryTable").show();
}
function CancelPaymentChanges() { editingDrawPaymentId = null; ClearPaymentFields(); }
function ClearPaymentFields() {
    $("#dropDownType, #textAeDrawPaymentAmount, #textAeDrawPaymentYear, #dropDownMonth, #textPaymentDate, #dropDownPaymentStatus").val("");
    $("#addPayment").css({ "display": "inline" });
    $("#updatePayment").hide();
    TogglePaymentSelectionFields(false);
}
function TogglePaymentSelectionFields(bShow) {
    $("#paymentMonthSelection, #paymentStatusSelection, #paymentDateSelection").css({ "display": (bShow ? "block" : "none") });
}
function BuildDrawPaymentObject() {
    var aeDrawPayment = {};
    aeDrawPayment.AEId = $("#labelAeId").html();
    aeDrawPayment.DrawPaymentType = $("#dropDownType").val();
    aeDrawPayment.DrawPaymentAmount = ($("#textAeDrawPaymentAmount").val() == "" ? 0 : parseFloat($("#textAeDrawPaymentAmount").val()));
    aeDrawPayment.DrawPaymentYear = ($("#textAeDrawPaymentYear").val() == "" ? 0 : parseInt($("#textAeDrawPaymentYear").val(), 10));
    aeDrawPayment.PaymentMonth = ($("#dropDownMonth").val() == "" ? 0 : parseInt($("#dropDownMonth").val(), 10));
    aeDrawPayment.PaymentDate = ($("#textPaymentDate").val() == "" ? new Date() : $("#textPaymentDate").val());
    aeDrawPayment.PaymentStatus = $("#dropDownPaymentStatus").val();
    aeDrawPayment.CompanyId = parseInt(selectedData.companyId, 10);
    return aeDrawPayment;
}
function PaymentTypeChangeHandler(drawPaymentTypeBox) {
    var type = drawPaymentTypeBox.value;
    TogglePaymentSelectionFields((type == "P"));
}
/* Draw Payment CRUD */
function ValidateDrawEntry() {
    var isValid = true;
    var msg = "";
    //Validate Required Fields:
    if ($("#dropDownType").val() == "") {
        msg += "Type is a required field.\n";
        isValid = false;
    }
    if ($("#textAeDrawPaymentAmount").val() == "") {
        msg += "Amount is a required field.\n";
        isValid = false;
    }
    if ($("#textAeDrawPaymentYear").val() == "") {
        msg += "Year is a required field.\n";
        isValid = false;
    }
    if (!IsValidDecimal($("#textAeDrawPaymentAmount").val())) {
        msg += "Invalid Amount. Please enter a currency value.\n";
        isValid = false;
    }
    if (($("#dropDownType").val() == "P") && ($("#dropDownMonth").val() == "")) {
        msg += "Payment month is a required field.\n";
        isValid = false;
    }
    if (!isValid) { alert(msg); }
    return isValid;
}
//Create      
function AddDrawPaymentRecord() {
    if (!ValidateDrawEntry()) { return; }
    var aeDrawPayment = BuildDrawPaymentObject();
    aeDrawPayment.AEDrawPaymentId = -1;
    Apollo.MasterDataService.AddAEDrawPaymentRecord(aeDrawPayment, AddDrawPaymentRecordCallback, ErrorCallback);
}
function AddDrawPaymentRecordCallback() {
    ClearPaymentFields();
    alert("Record Added.");
    Apollo.MasterDataService.GetAEDrawPayments(selectedData.aeId, selectedData.companyId, GetAEDrawPaymentsCallback, ErrorCallback);
}
//Retrieve
function EditDrawPaymentRecord(drawPaymentRecordId) {
    editingDrawPaymentId = drawPaymentRecordId
    Apollo.MasterDataService.GetAEDrawPaymentRecord(drawPaymentRecordId, EditDrawPaymentRecordCallback, ErrorCallback);
}
function EditDrawPaymentRecordCallback(aeDrawPaymentRecord) {
    $("#dropDownType").val(aeDrawPaymentRecord.DrawPaymentType);
    $("#dropDownMonth").val(aeDrawPaymentRecord.PaymentMonth);
    $("#textAeDrawPaymentYear").val(aeDrawPaymentRecord.DrawPaymentYear);
    $("#textAeDrawPaymentAmount").val(aeDrawPaymentRecord.DrawPaymentAmount);
    $("#dropDownPaymentStatus").val(aeDrawPaymentRecord.PaymentStatus);
    $("#textPaymentDate").val(GetDateAsString(aeDrawPaymentRecord.PaymentDate));
    $("#addPayment").hide();
    $("#updatePayment").css({ "display": "inline" });
    $("#paymentMonthSelection, #paymentStatusSelection, #paymentDateSelection").css({ "display": (aeDrawPaymentRecord.DrawPaymentType == "P" ? "block" : "none") });
}
//Update
function UpdateDrawPaymentRecord() {
    if (!ValidateDrawEntry()) { return; }
    var aeDrawPayment = BuildDrawPaymentObject();
    aeDrawPayment.AEDrawPaymentId = editingDrawPaymentId;
    Apollo.MasterDataService.UpdateAEDrawPaymentRecord(aeDrawPayment, UpdateDrawPaymentRecordCallback, ErrorCallback);
}
function UpdateDrawPaymentRecordCallback() {
    ClearPaymentFields();
    alert("Record Updated.");
    Apollo.MasterDataService.GetAEDrawPayments(selectedData.aeId, selectedData.companyId, GetAEDrawPaymentsCallback, ErrorCallback);
}
//Delete
function DeleteDrawPaymentRecord(drawPaymentRecordId) {
    var canContinue = confirm('Are you sure you want to delete this record?');
    if (!canContinue) { return false; }
    Apollo.MasterDataService.DeleteAEDrawPaymentRecord(drawPaymentRecordId, DeleteDrawPaymentRecordCallback, ErrorCallback);
}
function DeleteDrawPaymentRecordCallback() {
    alert("Record Deleted.");
    Apollo.MasterDataService.GetAEDrawPayments(selectedData.aeId, selectedData.companyId, GetAEDrawPaymentsCallback, ErrorCallback);
}
/**************************/


/************* Flat Rate Operations *************/
function GetAEFlatRatesCallback(aeFlatRates) {
    var data = "";
    $(".flatRateHistoryRow").remove();
    if (aeFlatRates.length == 0) {
        $("#flatRateHistoryTable").append("<tr class='flatRateHistoryRow'><td colspan='5'>No Flat Rate data has been entered.</td></tr>");
        $("#flatRateHistoryTable").show();
        return;
    }
    for (var i = 0; i < aeFlatRates.length; i++) {
        data += "<tr class='flatRateHistoryRow' style='background-color:#ffffcc;'>";
        data += "<td style='width:5%;text-align:center;'><img alt='Edit' style='cursor:pointer;' src='/Images/icon_edit.gif' onclick='EditFlatRateRecord(" + aeFlatRates[i].FlatRateId + ");' /></td>";
        data += "<td style='width:5%;text-align:center;'><img alt='Delete' style='cursor:pointer;' src='/Images/icon_delete.gif' onclick='DeleteFlatRateRecord(" + aeFlatRates[i].FlatRateId + ");' /></td>";
        data += "<td style='width:30%;text-align:right'>" + aeFlatRates[i].FlatRateNew + "</td>";
        data += "<td style='width:30%;text-align:right'>" + aeFlatRates[i].FlatRateRenew + "</td>";
        data += "<td style='width:30%;text-align:right'>" + aeFlatRates[i].FlatRateEffectiveDateDisplay + "</td>";
        data += "</tr>";
    }
    $("#flatRateHistoryTable").append(data);
    $("#flatRateHistoryTable").show();
}

function CancelFlatRateChanges() { editingFlatRateId = null; ClearFlatRateFields(); }
function ClearFlatRateFields() {
    $("#textAeFlatRateNew, #textAeFlatRateRenew, #textFlatRateEffectiveDate").val("");
    $("#addFlatRate").css({ "display": "inline" });
    $("#updateFlatRate").hide();
    TogglePaymentSelectionFields(false);
}
function BuildFlatRateObject() {
    var aeFlatRate = {};
    aeFlatRate.AEId = $("#labelAeId").html();
    aeFlatRate.FlatRateNew = ($("#textAeFlatRateNew").val() == "" ? 0 : parseInt($("#textAeFlatRateNew").val(), 10));
    aeFlatRate.FlatRateRenew = ($("#textAeFlatRateRenew").val() == "" ? 0 : parseInt($("#textAeFlatRateRenew").val(), 10));
    aeFlatRate.FlatRateEffectiveDate = $("#textFlatRateEffectiveDate").val();
    aeFlatRate.CompanyId = parseInt(selectedData.companyId, 10);
    return aeFlatRate;
}
/* Flat Rate CRUD */
function ValidateFlatRateFields() {
    var isValid = true;
    var msg = "";
    //Validate Required Fields:
    if ($("#textAeFlatRateNew").val() == "") {
        msg += "Flat Rate New is a required field.\n";
        isValid = false;
    }
    if ($("#textAeFlatRateRenew").val() == "") {
        msg += "Flat Rate Renew is a required field.\n";
        isValid = false;
    }
    if ($("#textFlatRateEffectiveDate").val() == "") {
        msg += "Flat Rate Effective Date is a required field.\n";
        isValid = false;
    }
    if ($("#textAeFlatRateNew").val() < 0) {
        msg += "Flat Rate New cannot be negative.\n";
        isValid = false;
    }
    if (parseInt($("#textAeFlatRateNew").val(), 10) > parseInt(maximumFlatRatePercentage, 10)) {
        msg += "Flat Rate New cannot be greater than " + maximumFlatRatePercentage + ".\n";
        isValid = false;
    }
    if ($("#textAeFlatRateRenew").val() < 0) {
        msg += "Flat Rate Renew cannot be negative.\n";
        isValid = false;
    }
    if (parseInt($("#textAeFlatRateRenew").val(), 10) > parseInt(maximumFlatRatePercentage, 10)) {
        msg += "Flat Rate Renew cannot be greater than " + maximumFlatRatePercentage + ".\n";
        isValid = false;
    }
    if (!isValid) { alert(msg); }
    return isValid;
}
function ValidateFlatRateEffectiveDate(wantUpdate) {
    Apollo.MasterDataService.CheckExistingFlatRateEffectiveDate(selectedData.aeId, selectedData.companyId, $("#textFlatRateEffectiveDate").val(), ValidateFlatRateEffectiveDateCallback, ErrorCallback, wantUpdate);
}
function ValidateFlatRateEffectiveDateCallback(isExisting, wantUpdate) {
    if (isExisting) {
        alert("A Flat Rate for this effective date already exists.");
        return;
    } else {
        var aeFlatRate = BuildFlatRateObject();
        if (wantUpdate) {            
            aeFlatRate.FlatRateId = editingFlatRateId;
            Apollo.MasterDataService.UpdateAEFlatRateRecord(aeFlatRate, UpdateFlatRateRecordCallback, ErrorCallback);
        } else {
            aeFlatRate.FlatRateId = -1;
            Apollo.MasterDataService.AddAEFlatRateRecord(aeFlatRate, AddFlatRateRecordCallback, ErrorCallback);
        }
    }
}
//Create
function AddFlatRateRecord() {
    if (!ValidateFlatRateFields()) { return; }
    ValidateFlatRateEffectiveDate(false);    
}
function AddFlatRateRecordCallback() {
    ClearFlatRateFields();
    alert("Record Added.");
    Apollo.MasterDataService.GetAEFlatRates(selectedData.aeId, selectedData.companyId, GetAEFlatRatesCallback, ErrorCallback);
}
//Retrieve
function EditFlatRateRecord(flatRateId) {
    editingFlatRateId = flatRateId
    Apollo.MasterDataService.GetAEFlatRateRecord(flatRateId, EditFlatRateRecordCallback, ErrorCallback);
}
function EditFlatRateRecordCallback(aeFlatRateRecord) {
    $("#addFlatRate").hide();
    $("#updateFlatRate").css({ "display": "inline" });
    $("#textAeFlatRateNew").val(aeFlatRateRecord.FlatRateNew);
    $("#textAeFlatRateRenew").val(aeFlatRateRecord.FlatRateRenew);
    $("#textFlatRateEffectiveDate").val(GetDateAsString(aeFlatRateRecord.FlatRateEffectiveDate));
}
//Update
function UpdateFlatRateRecord() {
    if (!ValidateFlatRateFields()) { return; }
    ValidateFlatRateEffectiveDate(true);    
}
function UpdateFlatRateRecordCallback() {
    ClearFlatRateFields();
    alert("Record Updated.");
    Apollo.MasterDataService.GetAEFlatRates(selectedData.aeId, selectedData.companyId, GetAEFlatRatesCallback, ErrorCallback);
}
//Delete
function DeleteFlatRateRecord(flatRateId) {
    var canContinue = confirm('Are you sure you want to delete this record?');
    if (!canContinue) { return false; }
    Apollo.MasterDataService.DeleteAEFlatRateRecord(flatRateId, DeleteFlatRateRecordCallback, ErrorCallback);
}
function DeleteFlatRateRecordCallback() {
    alert("Record Deleted.");
    Apollo.MasterDataService.GetAEFlatRates(selectedData.aeId, selectedData.companyId, GetAEFlatRatesCallback, ErrorCallback);
}
/**************************/

/************* Commission Amount Operations *************/
function GetCommissionAmountsCallback(commissionAmounts) {
    var data = "";
    $(".commissionAmountRow").remove();
    if (commissionAmounts.length == 0) {
        $("#commissionAmountTable").append("<tr class='commissionAmountRow'><td colspan='5'>No Commission Amounts have been Entered.</td></tr>");
        $("#commissionAmountTable").show();
        return;
    }
    for (var i = 0; i < commissionAmounts.length; i++) {
        data += "<tr class='commissionAmountRow' style='background-color:#ffffcc;'>";
        data += "<td style='width:5%;text-align:center;'><img alt='Edit' style='cursor:pointer;' src='/Images/icon_edit.gif' onclick='EditCommissionAmountRecord(" + commissionAmounts[i].AECommissionAmountId + ");' /></td>";
        data += "<td style='width:5%;text-align:center;'><img alt='Delete' style='cursor:pointer;' src='/Images/icon_delete.gif' onclick='DeleteCommissionAmountRecord(" + commissionAmounts[i].AECommissionAmountId + ");' /></td>";
        data += "<td style='width:30%;text-align:right'>" + commissionAmounts[i].CommissionYear + "</td>";
        data += "<td style='width:30%;text-align:right'>" + commissionAmounts[i].CommissionAmountDisplay + "</td>";
        data += "<td style='width:30%;text-align:right'>" + commissionAmounts[i].DateLastModifiedDisplay + "</td>";
        data += "</tr>";
    }
    $("#commissionAmountTable").append(data);
    $("#commissionAmountTable").show();
}
function CancelCommissionAmountChanges() { editingCommissionAmountId = null; ClearCommissionAmountFields(); }
function ClearCommissionAmountFields() {
    $("#textCommissionAmountYear, #textCommissionAmount").val("");
    $("#addCommissionAmount").css({ "display": "inline" });
    $("#updateCommissionAmount").hide();
}
function BuildCommissionAmountObject() {
    var commissionAmount = {};
    commissionAmount.AEId = $("#labelAeId").html();
    commissionAmount.CommissionYear = parseInt($("#textCommissionAmountYear").val(), 10);
    commissionAmount.CommissionAmount = parseFloat($("#textCommissionAmount").val());
    commissionAmount.CompanyId = parseInt(selectedData.companyId, 10);
    return commissionAmount;
}
function ValidateCommissionAmountEntry() {
    var isValid = true;
    var msg = "";
    if ($("#textCommissionAmountYear").val() == "") {
        msg += "Year is a required field.\n";
        isValid = false;
    }
    if (!IsValidNumber($("#textCommissionAmountYear").val())) {
        msg += "Year must be an integer value.\n";
        isValid = false;
    }
    if ($("#textCommissionAmount").val() == "") {
        msg += "Amount is a required field.\n";
        isValid = false;
    }
    if (!IsValidDecimal($("#textCommissionAmount").val())) {
        msg += "Invalid Amount. Please enter a currency value.\n";
        isValid = false;
    }
    if (!isValid) { alert(msg); }
    return isValid;
}
function ValidateCommissionYear(wantUpdate) {
    Apollo.MasterDataService.CheckExistingCommissionAmountYear(selectedData.aeId, selectedData.companyId, $("#textCommissionAmountYear").val(), ValidateCommissionYearCallback, ErrorCallback, wantUpdate);
}
function ValidateCommissionYearCallback(isExisting, wantUpdate) {
    if (isExisting) {
        alert("A Commission Amount for this year already exists.");
        return;
    } else {
        var commissionAmount = BuildCommissionAmountObject();
        commissionAmount.AECommissionAmountId = -1;
        Apollo.MasterDataService.AddAECommissionAmountRecord(commissionAmount, AddCommissionAmountRecordCallback, ErrorCallback);
    }
}
//Create
function AddCommissionAmountRecord() {
    if (!ValidateCommissionAmountEntry()) { return; }
    ValidateCommissionYear(false);
}
function AddCommissionAmountRecordCallback() {
    ClearCommissionAmountFields();
    alert("Record Added.");
    Apollo.MasterDataService.GetAECommissionAmounts(selectedData.aeId, selectedData.companyId, GetCommissionAmountsCallback, ErrorCallback);
}
//Retrieve
function EditCommissionAmountRecord(aeCommissionAmountId) {
    editingCommissionAmountId = aeCommissionAmountId;
    Apollo.MasterDataService.GetAECommissionAmountRecord(aeCommissionAmountId, EditCommissionAmountRecordCallback, ErrorCallback);
}
function EditCommissionAmountRecordCallback(commissionAmountRecord) {
    $("#addCommissionAmount").hide();
    $("#updateCommissionAmount").css({ "display": "inline" });
    $("#textCommissionAmountYear").val(commissionAmountRecord.CommissionYear);
    $("#textCommissionAmount").val(commissionAmountRecord.CommissionAmount);
}
//Update
function UpdateCommissionAmountRecord() {
    if (!ValidateCommissionAmountEntry()) { return; }
    var commissionAmount = BuildCommissionAmountObject();
    commissionAmount.AECommissionAmountId = editingCommissionAmountId;
    Apollo.MasterDataService.UpdateAECommissionAmountRecord(commissionAmount, UpdateCommissionAmountRecordCallback, ErrorCallback);
}
function UpdateCommissionAmountRecordCallback() {
    ClearCommissionAmountFields();
    alert("Record Updated.");
    Apollo.MasterDataService.GetAECommissionAmounts(selectedData.aeId, selectedData.companyId, GetCommissionAmountsCallback, ErrorCallback);
}
//Delete
function DeleteCommissionAmountRecord(commissionAmountId) {
    var canContinue = confirm('Are you sure you want to delete this record?');
    if (!canContinue) { return false; }
    Apollo.MasterDataService.DeleteAECommissionAmountRecord(commissionAmountId, DeleteCommissionAmountRecordCallback, ErrorCallback);
}
function DeleteCommissionAmountRecordCallback() {
    alert("Record Deleted.");
    Apollo.MasterDataService.GetAECommissionAmounts(selectedData.aeId, selectedData.companyId, GetCommissionAmountsCallback, ErrorCallback);
}