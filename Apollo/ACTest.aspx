<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="ACTest.aspx.cs" Inherits="ACTest" %>
<%@ Register Src="~/UserControls/autoCompleteControl.ascx" TagName="autoComplete" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function CreateAutoComplete(options) {
            //Options Expected:
            //  elId: ID of Text Box  *Required
            //  url: URL of Autocomplete webservice *Required
            //  dependencies: JSON of Dependent Parameters/Element IDs to be sent to webservice ~Optional
            //      should be in format [ParameterName] = ElementId
            //  callback: callback function to be called when a value is selected ~Optional
            //  defaultText: Placeholder text in Text Box ~Optional

            if (options.defaultText !== undefined) {
                $("#" + options.elId).val(options.defaultText);
            }
            $("#" + options.elId).autocomplete({
                minLength: 2,
                source: function (request, response) {
                    if (response === undefined) { return; }
                    var postData = { term: request.term };
                    if (options.dependencies !== undefined) {
                        for (var dependency in options.dependencies) {
                            if (options.dependencies.hasOwnProperty(dependency)) {
                                try { eval("postData." + dependency + " = '" + $("#" + options.dependencies[dependency]).val() + "';"); } catch (e) { }
                            }
                        }
                    }
                    $.ajax({
                        url: options.url,
                        data: JSON.stringify(postData),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.Text,
                                    value: item.Value
                                }
                            }));
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
                    });
                },
                select: function (event, ui) {
                    try { event.preventDefault(); } catch (e) { }
                    if (ui.item == null) { return; }
                    $(this).val(ui.item.label).select();
                    if (ui.item.value !== undefined && ui.item.value != null && ui.item.value != -1) {
                        if (options.callback !== undefined) { options.callback.call(this, ui.item.value); }
                    }
                },
                change: function (event, ui) {
                    try { event.preventDefault(); } catch (e) { }
                    if (ui.item == null) { return; }
                    $(this).val(ui.item.label);
                },
                focus: function (event, ui) {
                    try { event.preventDefault(); } catch (e) { }
                    if (ui.item == null) { return; }
                    $(this).val(ui.item.label);
                }
            });
        }
        function onDialogACSelectCallback(acValue) {
            $(this).attr("acvalue", acValue);
            alert(acValue);
            //RefreshGrid();
        }
    </script>
    <div style="margin:10px;">
        <input type="hidden" id="foo" value="1" />
        Test: <uc:autoComplete id="test" runat="server" DefaultText="Hello" Callback="onDialogACSelectCallback" />
    </div>
</asp:Content>

