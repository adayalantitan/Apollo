var pager = "<div id='pager' class='pager' style='text-align:center;width:90%;margin:0 auto;'><form><table style='margin-top:5px;width:275px;'>"
    + "<thead><tr><th colspan='2'>&nbsp;</th><th style='white-space:nowrap'>Page x of n</th><th colspan='2'>&nbsp;</th><th>Items per Page</th></tr></thead><tbody><tr>"
    + "<td style='vertical-align:middle;width:5%;'><img src='../includes/tablesorter/pager/first.png' class='first' style='margin-top:3px;' /></td>"
    + "<td style='vertical-align:middle;width:5%;'><img src='../includes/tablesorter/pager/prev.png' class='prev' style='margin-top:3px;' /></td>"
    + "<td style='vertical-align:middle;text-align:center;width:10%;'><input type='text' class='pagedisplay' style='width:55px;text-align:center;' /></td>"
    + "<td style='vertical-align:middle;width:5%;'><img src='../includes/tablesorter/pager/next.png' class='next' style='margin-top:3px;' /></td>"
    + "<td style='vertical-align:middle;width:5%;'><img src='../includes/tablesorter/pager/last.png' class='last' style='margin-top:3px;' /></td>"
    + "<td style='vertical-align:middle;text-align:center;width:50%;'><select class='pagesize' style='margin-left:3px;'><option selected='selected' value='10'>10</option><option value='25'>25</option><option value='50'>50</option><option value='100'>100</option><option value='10000'>All</option></select></td>"
    + "</tr></tbody></table></form></div>";

function newAePopupSearch(companyId) {    
    Apollo.AutoCompleteService.GetAEData(companyId, "", aePopupSearchCallback, null, "");
}
function aeFilterSearch(txt) {
    var companyId = $get('<%=dropDownCompany.ClientID %>').value;
    Apollo.AutoCompleteService.GetAEData(companyId, txt.value, aePopupSearchCallback, null, txt.value);
}
function aePopupSearchCallback(results, searchText) {
    $(".searchResultRow").remove();
    var row = "";
    for (var i = 0; i < results.length; i++) {
        row += "<tr class='searchResultRow'><td style='text-align:center;'><input type='checkbox' onclick='selectAe(\"" + results[i].aeId + "\",\"" + results[i].aeName + "\");' /></td><td>" + results[i].aeName + "</td></tr>";
    }
    $("#globalModalPopup").dialog({ "modal": true });
    var customSearch = "Search: <input type='text' style='width:100px;' onkeyup='aeFilterSearch(this);' value='" + (searchText !== undefined ? searchText : "") + "' /><br/><br/>";
    $("#globalModalPopup").html(customSearch + pager + "<table class='featureTable tablesorter'><thead><tr><th>&nbsp;</th><th>AE Name</th></tr></thead><tbody>" + row + "</tbody></table>");
    $("#globalModalPopup").dialog("option", { "height": "auto", "width": "auto" });
    $(".tablesorter").tablesorter({ headers: { 0: { sorter: false}} });
    $(".tablesorter").tablesorterPager({ container: $("#pager") });
    $("#globalModalPopup").dialog("open");
    //globalModalPopup
}
function selectAe(aeId, aeName) {
    //textAeSelections
    selectedFilters.push({ type: "AE", id: aeId, displayValue: aeName });
    var pre = $("#textAeSelections").text();
    $("#textAeSelections").text("AE ID:\t" + aeId + "\nAE Name:\t" + aeName + "\n" + pre);
    //alert("AE ID:\t" + aeId + "\nAE Name:\t" + aeName);
}