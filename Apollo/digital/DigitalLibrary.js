$apollo.DigitalLibrary = {};

var selectedImages = {};
function SelectAllLines(parentCheck) { $(".lineItemTag").attr('checked', parentCheck.checked) }
function TogglePanel(panelName) { var div = document.getElementById(panelName); var plus = document.getElementById(panelName + 'Plus'); var minus = document.getElementById(panelName + 'Minus'); div.style.display = (div.style.display == "block") ? "none" : "block"; plus.style.display = (plus.style.display == "inline") ? "none" : "inline"; minus.style.display = (minus.style.display == "inline") ? "none" : "inline" }
function ToggleCellSelection(id, selected, isHovering) { if (!(isHovering && IsSelected(id.substring(2)))) { $get(id).style.border = (selected) ? ((isHovering) ? "1px solid #ec008c" : "2px solid #ec008c") : "1px solid #00B0D8" } }
function ClearAllSelectedImages() { $() }
function HandleImageClick(img, details) {
    if (img.getAttribute('class') == 'thumbSelected') {
        try { img.setAttribute('class', 'thumbDeSelected'); } catch (e) { }
        try { img.setAttribute('className', 'thumbDeSelected'); } catch (e) { }
        try { $get('check' + img.id).checked = false; } catch (e) { }
        try { ToggleCellSelection('td' + img.id, false, false); } catch (e) { }
        try { RemoveIdFromSelectedList(img.id, details); } catch (e) { }
    } else {
        try { img.setAttribute('class', 'thumbSelected'); } catch (e) { }
        try { img.setAttribute('className', 'thumbSelected'); } catch (e) { }
        try { $get('check' + img.id).checked = true; } catch (e) { }
        try { ToggleCellSelection('td' + img.id, true, false); } catch (e) { }
        try { AddIdToSelectedList(img.id, details); } catch (e) { }
    }
}
function HandleImageClickNew(img, id) {
    selectedImages[id].deselected = (!(img.getAttribute('class') == 'thumbSelected'));    
    HandleImageClick(img, selectedImages[id].details);
}
function CellCheckSelectionHandler(cellId) {
    var id = cellId.substring(2);
    var checkBoxId = "check" + id;
    var checkBox = $get(checkBoxId);
    checkBox.checked = !checkBox.checked;
    ImageCheckSelectionHandler(checkBox);
}
function ImageCheckSelectionHandler(checkBox) {
    var id = checkBox.id.substring(5);
    var details = $get('hdnDetails' + checkBox.id.substring(5)).value;
    var detailsJson = eval("(" + $("#hdnJSONDetails" + id).val() + ")");
    //alert(detailsJson.contractNumber);
    if (checkBox.checked) {
        AddIdToSelectedList(id, details);
        selectedImages[id] = { detailsJson: detailsJson, details: details, deselected: false };
        ToggleCellSelection('td' + id, true, false);
    } else {
        RemoveIdFromSelectedList(id, details);
        selectedImages = RemoveHashKey(selectedImages, id);
        ToggleCellSelection('td' + id, false, false);
    }
}
function ClickCell(checkBox) { checkBox.checked = !checkBox.checked; }