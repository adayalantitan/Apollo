function JSValidator() {
    this.debugMode = false;
    this.errorMessageStack = "";    
    this.hasAnyErrors = false;
    this.dateRegExp = /^(0?[1-9]|1[012])[\-\/\.](0?[1-9]|[12][0-9]|3[01])[\-\/\.](19|20)\d\d$/;
    this.decimalRegExp = /^\d+(\.\d+)?$/;
    this.numericRegExp = /[^0-9]+$/;
}
JSValidator.prototype.PushErrorMessage = function(msg) {
    this.errorMessageStack += '\n' + msg;
}
JSValidator.prototype.GetErrorMessageHTML = function() {
    return this.errorMessageStack.replace(/\n/g,'<br/>');
}
JSValidator.prototype.ClearErrorDiv = function(divId) {
    var errorDiv = this.Get(divId);
    errorDiv.innerHTML = "";
    this.errorMessageStack = "";
    this.hasAnyErrors = false;
    if ((errorDiv.style.display == 'block') || (errorDiv.style.display == 'inline')) {
        errorDiv.style.display = 'none';
    }
}
JSValidator.prototype.SetErrorMessageDiv = function(divId) {
    var errorDiv = this.Get(divId);
    if (this.hasAnyErrors) {
        errorDiv.innerHTML = this.GetErrorMessageHTML();
        if (errorDiv.style.display == 'none') {
            errorDiv.style.display = 'inline';
        }
    }
}
JSValidator.prototype.ValidateControl = function(controlId, validationMethod, msg) {
    var control = this.Get(controlId);
    if (typeof (validationMethod) === 'function') {
        if (!validationMethod(control)) {
            this.PushErrorMessage(msg);
            this.Dirty();
        }
    }
}
JSValidator.prototype.ValidateDependentControls = function(control1Id, control2Id, validationMethod, msg) {
    var control1 = this.Get(control1Id);
    var control2 = this.Get(control2Id);
    if (typeof (validationMethod) === 'function') {
        if (!validationMethod(control1, control2)) {
            this.PushErrorMessage(msg);
            this.Dirty();
        }
    }
}
JSValidator.prototype.ValidateDate = function(control) {
    return this.IsValidDate(control.value);
}
JSValidator.prototype.ValidateDecimal = function(control) {
    return this.IsValidDecimal(control.value);
}
JSValidator.prototype.ValidateNumber = function(control) {
    return this.IsValidNumber(control.value);
}
JSValidator.prototype.ValidateDateRange = function(startDateControlId, endDateControlId, msg) {
    var startDateControl = this.Get(startDateControlId);
    var endDateControl = this.Get(endDateControlId);
    if (!this.ValidateDate(startDateControl)) { return false; }
    if (!this.ValidateDate(endDateControl)) { return false; }
    date1 = new Date(startDateControl.value);
    date2 = new Date(endDateControl.value);
    return (date1 <= date2);
}
JSValidator.prototype.IsValidDate = function(testValue) {
    return this.IsValid(this.dateRegExp, testValue);
}
JSValidator.prototype.IsValidDecimal = function(testValue) {
    return this.IsValid(this.decimalRegExp, testValue);
}
JSValidator.prototype.IsValidNumber = function(testValue) {
    return this.IsValid(this.numericRegExp, testValue);
}
JSValidator.prototype.IsValid = function(regExp, testValue) {
    return regExp.test(testValue);
}
JSValidator.prototype.Get = function(controlId) {
    return document.getElementById(controlId);
}
JSValidator.prototype.Dirty = function() {
    this.hasAnyErrors = true;
}