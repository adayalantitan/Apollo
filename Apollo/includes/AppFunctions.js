//From: http://www.codeproject.com/KB/scripting/jsnamespaces.aspx
var Namespace = {
    Register: function (_Name) {
        var chk = false;
        var cob = "";
        var spc = _Name.split(".");
        for (var i = 0; i < spc.length; i++) {
            if (cob != "") { cob += "."; }
            cob += spc[i];
            chk = this.Exists(cob);
            if (!chk) { this.Create(cob); }
        }
        if (chk) { throw "Namespace: " + _Name + " is already defined."; }
    },
    Create: function (_Src) { eval("window." + _Src + " = new Object();"); },
    Exists: function (_Src) { eval("var NE = false; try{if(" + _Src + "){NE = true;}else{NE = false;}}catch(err){NE=false;}"); return NE; }
}
/* Globals */
Namespace.Register("Apollo.Client");
var $apollo = Apollo.Client;
var debug = false;
var previousColor;
var validationMessageStack = "";
/* End Globals */

/* Error Handling Functions */
function ErrorCallback(e) { alert(e._message); }
function ShowDiv(elId) { document.getElementById(elId).style.display = "block"; }
function ClearErrorDiv(errorDivId) { var errorDiv = document.getElementById(errorDivId); errorDiv.innerHTML = ""; }
function SetErrorDiv(msg, errorDivId) { var errorDiv = document.getElementById(errorDivId); errorDiv.innerHTML = msg.replace(/\n/g, '<br/>'); }
function AddErrorMessage(msg) { validationMessageStack += (validationMessageStack != "") ? ("\n" + msg) : msg; }
/* End Error Handling Functions */

function debugMsg(msg) { if (debug) { alert(msg); } }
function trimValue(value) { return value.replace(/^\s*/, "").replace(/\s*$/, ""); }
function IsValidDecimal(testValue) { var re = /^[-|0-9]+(\.[0-9][0-9]?)?$/; return (re.test(testValue)); }
function IsValidNumber(testValue) { var re = /^[0-9]+$/; return (re.test(testValue)); }
function IsValidDate(testValue) { var re = /^\d{1,2}[\-\/\.]\d{1,2}[\-\/\.]\d{4}$/; return (re.test(testValue)); }
function IsValidEmail(testValue) { var re = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/; return (re.test(testValue)); }
function Over(gridId, cellID, objectType) { try { var cell = igtbl_getCellById(cellID); setRowBackColor(cell.Row, "#ffebce"); } catch (e) { } }
function Out(gridId, cellID, objectType) { try { var cell = igtbl_getCellById(cellID); setRowBackColor(cell.Row, previousColor); } catch (e) { } }
function setRowBackColor(row, color) { try { var cells = row.getCellElements(); previousColor = cells[0].style.backgroundColor; for (var i = 0; i < cells.length; i++) { if (cells[i].style.backgroundColor != color) { cells[i].style.backgroundColor = color } } } catch (e) { } }
function setRowBold(row, show) { try { var cells = row.getCellElements(); for (var i = 0; i < cells.length; i++) { cells[i].style.fontWeight = (show) ? "bold" : "normal" } } catch (e) { } }
function HashLength(hashTable) { var size = 0; for (var key in hashTable) { if (hashTable.hasOwnProperty(key)) { size++; } } return size; }
function RemoveHashKey(hashTable, keyToRemove) { var newHashTable = {}; for (var key in hashTable) { if (key != keyToRemove) { newHashTable[key] = hashTable[key]; } } return newHashTable; }
function GetDateAsString(date) { if (date === undefined || date == "") { return ""; } return ((date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear()); }
function roundNumber(num, dec) { return Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec); }
function SetDateRangePicker(from, to) {
    var dates = $("#" + from + ", #" + to).datepicker({
        defaultDate: "+1w",
        dateFormat: "mm/dd/yy",
        changeMonth: true,
        changeYear: true,
        onSelect: function (selectedDate) {
            var option = this.id == from ? "minDate" : "maxDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });
}
/* User Control Functions */
//Dynamically build the id of a nested control
//      that is (could be) contained within a repeated User Control
//  refId = the id of another control within the User Control
//  targetControlId = the id of a control where a reference is needed
//  splitChar = the character which splits the nested controls:
//      i.e: ctl$00_ctl$01_ctl$02 '_' would be the splitChar
function BuildIdFromRefId(refId, targetControlId, splitChar) { var part = refId.split(splitChar); var id = ""; for (var i = 0; i < part.length - 1; i++) { id += ((id == "") ? "" : "_") + part[i] } id += targetControlId; return id }
function HandleClose(closeButton) { var gridPanelId = BuildIdFromRefId(closeButton.id, '_gridPanel', '_'); $get(gridPanelId).style.display = "none"; try { $get('gridPanel').style.display = 'none' } catch (e) { } }
function checkACField(textField, idField) { if (textField.value == '') { idField.value = ''; } }
/*
function GlobalPageMethodErrorHandler(e){
    //Send a friendly message to the user
    alert('An error occurred during the operation.');
    //Log the exception
    Page Methods.LogException(e);
}
*/
 /* 
From:
    http://blogs.iis.net/webtopics/archive/2009/06/18/fix-updatepanel-async-postbacks-slow-in-internet-explorer.aspx
Purpose:
    Try to speed up Asynchronous postbacks in IE originating from within ASP.NET Update Panels
*/
function disposeTree(sender, args) { elements = args.get_panelsUpdating(); for (var i = elements.length - 1; i >= 0; i--) { var nodes = elements[i].getElementsByTagName('*'); for (var j = 0; j < nodes.length; j++) { var node = nodes[j]; if (node.nodeType === 1) { if (node.dispose && typeof (node.dispose) === "function") { node.dispose() } else if (node.control && typeof (node.control.dispose) === "function") { node.control.dispose() } var behaviors = Sys.UI.Behavior.getBehaviors(node); for (var k = behaviors.length - 1; k >= 0; k--) { behaviors[k].dispose() } } } elements[i].innerHTML = "" } }
/* Cascading DropDown Helper methods */
function AddToList(kv, dropDownListId) { var dropDownList = $get(dropDownListId); if (dropDownList == null) { alert('object not found'); throw ('Object not found'); } ClearList(dropDownList); for (var i = 0; i < kv.length; i++) { dropDownList[dropDownList.length] = new Option(kv[i].name, kv[i].value, kv[i].isDefaultValue, kv[i].isDefaultValue); }try { $("#" + dropDownListId).trigger("liszt:updated"); } catch (e) { }  }
function ClearList(dropDownList) { for (var i = dropDownList.length; i >= 0; i--) { dropDownList[i] = null } }
/* Digital Library Methods */
function cancelBubble(){
    try {        
        window.clearTimeout(searchTimeoutId);        
        if (!e) {var e = window.event;}
        e.cancelBubble = true;
        if (e.stopPropagation) { e.stopPropagation(); }
    } catch(e){}
}
function updateAutoCompleteContext(currentContextKey, keyToUpdate, newValue) {
    if (currentContextKey.indexOf(keyToUpdate) == -1) {
        currentContextKey += keyToUpdate + ':' + newValue;
    } else {
        allKeys = currentContextKey.split(';');
        var newContextKeyValue = '';
        for (var i = 0; i < allKeys.length; i++) {
            if (allKeys[i].split(':')[0] != keyToUpdate) {
                newContextKeyValue += ((newContextKeyValue != '') ? ';' : '') + allKeys[i];
            } else {
                newContextKeyValue += ((newContextKeyValue != '') ? ';' : '') + keyToUpdate + ':' + newValue;
            }
        }
        currentContextKey = newContextKeyValue;
    }
    return currentContextKey;
}
function mouseincolor(obj){ obj.style.backgroundColor='#339933'; obj.style.color='#ffffff'; }
function mouseoutcolor(obj){ obj.style.backgroundColor='#ffffff'; obj.style.color='#2f4070'; }
function dlContractRowClick(checkId){ var isChecked = $('#'+checkId).attr("checked"); $('#'+checkId).attr("checked",!isChecked); }
function setDefaultButton(e,buttonId){        
    var evt = e ? e : window.event;
    try { if(evt.shiftKey){return true;} } catch(e) {}
    var bt = document.getElementById(buttonId);
    if (bt){
        if (evt.keyCode == 13){
            bt.click();
            evt.cancelBubble=true;
            evt.stopPropagation();
            return false;
        }
    }
}
function setDefaultParmlessFunction(e,func){        
    var evt = e ? e : window.event;
    try { if(evt.shiftKey){return true;} } catch(e) {}    
    if (evt.keyCode == 13){
        try { 
            evt.cancelBubble=true;
            evt.stopPropagation();
            func.apply(null,null); 
            return false;
        } catch(e) {}
    }
}
function innerPopupContractDetail(contractNumber,companyId){
    window.open('/popups/contractDetailPopup.aspx?contractNumber='+contractNumber+'&companyId='+companyId+'','Contract Detail','width=700,height=575,scrollbars=yes,resizable=yes');
}
function innerShowAttachmentPopupCallback(data){
    var w=window.open('','printdetails','width=400,height=300,scrollbars=yes,resizable=yes');
    w.document.write('<html><head>');
    w.document.write('  <title>Titan 360</title>');
    w.document.write('  <link rel="Stylesheet" href="/Styles/StyleSheet.css?v=07" />');
    w.document.write('  <link rel="Stylesheet" href="includes/jqueryUI/css/custom-theme/jquery-ui-1.8.20.custom.css" />');
    w.document.write('  <link rel="Stylesheet" href="includes/jqGrid/ui.jqgrid.css" />');
    w.document.write('  <link rel="stylesheet" href="includes/cluetip/jquery.cluetip.custom.css" type="text/css" />');                        
    w.document.write('</head>');
    w.document.write('<body><div style="margin:10px">');
    w.document.write(data);
    w.document.write('</div>');
    w.document.write('</body>');
    w.document.write('</html>');            
    w.document.close();	        
    w.focus();
}  
function innerOpenPrint(details){
    var w = window.open('', 'printdetails', 'width=750,height=650,scrollbars=yes,resizable=yes,menubar=yes');
    try {
        w.document.write('<html><head>');
        w.document.write('  <title>Titan 360</title>');
        w.document.write('  <link rel="Stylesheet" href="/Styles/StyleSheet.css?v=04" />');
        w.document.write('  <link rel="Stylesheet" href="includes/jqueryUI/css/custom-theme/jquery-ui-1.8.20.custom.css" />');
        w.document.write('  <link rel="Stylesheet" href="includes/jqGrid/ui.jqgrid.css" />');
        w.document.write('  <link rel="stylesheet" href="includes/cluetip/jquery.cluetip.custom.css" type="text/css" />');
        w.document.write('<style>');
        w.document.write('  img,input {display:none !important;}');
        w.document.write('  .historyTable {height:100% !important; overflow:visible !important;}');
        w.document.write('</style>');
        w.document.write('</head>');
        w.document.write('<body><div style="margin:10px">');
        w.document.write(details);
        w.document.write('</div>');
        w.document.write('</body>');
        w.document.write('</html>');
        w.document.close();
        w.focus();
    } catch (e) {
        alert(e.description);
    }
}
function ShowAttachmentPopup(contractNumber, companyId) {
    var url = "/quattro/QuattroAttachmentHandler.ashx?contractNumber=" + contractNumber + "&companyId=" + companyId;
    jQuery.get(url, null, ShowAttachmentPopupCallback, "html");
}
function ShowAttachmentPopupCallback(data) {
    var dialog = $("<div style='font-size:10px !important;z-index:11000 !important;'></div>");
    dialog.addClass("dialog")
        .attr({ "id": "attachmentDialog" })
        .appendTo("body")
        .dialog({ title: "Attachments"
            , close: function () { $(this).remove() }
            , modal: true, autoOpen: false
            , width: 640, height: 250
        })
        .html(data);
    dialog.dialog("open");
}
function setScrollLeft(left) { try { window.pageXOffset = left } catch (e) { } try { document.documentElement.scrollLeft = left } catch (e) { } try { document.body.scrollLeft = left } catch (e) { } } 
function setScrollTop(top) { try { window.pageYOffset = top } catch (e) { } try { document.documentElement.scrollTop = top } catch (e) { } try { document.body.scrollTop = top } catch (e) { } }
function stripNonNumericCharacters(input){
    var output = input.replace(/[^0-9]/g, '');
    return output;
}    
/*From: http://www.softcomplex.com/docs/get_window_size_and_scrollbar_position.html*/
function f_clientWidth() {
	return f_filterResults (
		window.innerWidth ? window.innerWidth : 0,
		document.documentElement ? document.documentElement.clientWidth : 0,
		document.body ? document.body.clientWidth : 0
	);
}
function f_clientHeight() {
	return f_filterResults (
		window.innerHeight ? window.innerHeight : 0,
		document.documentElement ? document.documentElement.clientHeight : 0,
		document.body ? document.body.clientHeight : 0
	);
}
function f_scrollLeft() {
	return f_filterResults (
		window.pageXOffset ? window.pageXOffset : 0,
		document.documentElement ? document.documentElement.scrollLeft : 0,
		document.body ? document.body.scrollLeft : 0
	);
}
function f_scrollTop() {
	return f_filterResults (
		window.pageYOffset ? window.pageYOffset : 0,
		document.documentElement ? document.documentElement.scrollTop : 0,
		document.body ? document.body.scrollTop : 0
	);
}
function f_filterResults(n_win, n_docel, n_body) {
	var n_result = n_win ? n_win : 0;
	if (n_docel && (!n_result || (n_result > n_docel)))
		n_result = n_docel;
	return n_body && (!n_result || (n_result > n_body)) ? n_body : n_result;
}
if (!document.getBoxObjectFor) {
    document.getBoxObjectFor = function(el) {
        if (!(el instanceof HTMLElement)) {
            return;
        } //else:
        var b = el.getBoundingClientRect(), p = el, x = sx = b.left - el.offsetLeft, y = sy = b.top - el.offsetTop, w = window;
        while (!(p instanceof HTMLHtmlElement)) {
            sx += p.scrollLeft;
            sy += p.scrollTop;
            p = p.parentNode;
        }
        return { x: sx, y: sy, width: Math.round(b.width), height: Math.round(b.height),
            element: el, firstChild: el, lastChild: el, previousSibling: null, nextSibling: null, parentBox: el.parentNode,
            screenX: x + w.screenX + (w.outerWidth - w.innerWidth) / 2, screenY: y + w.screenY + (w.outerHeight - w.innerHeight) / 2
        };
    };
}