<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DigitalAvailsPrintPreview.aspx.cs" Inherits="Apollo.digitalAvails_DigitalAvailsPrintPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
w.document.write("<html><head>");
    w.document.write("<title>Digital Avails</title>");
    w.document.write("<link rel='Stylesheet' href='/Styles/StyleSheet.css' />");
    w.document.write("<link rel='Stylesheet' href='/digitalAvails/digitalAvailsPrint.css' />");
    w.document.write("</head>");
    w.document.write("<body>");
    w.document.write("<div style='margin:10px;'>");
    w.document.write($("#availsData").html());
    w.document.write("</div></body>");
    w.document.write("</html>");
    w.document.close();
    w.focus();
-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Digital Avails - Print Preview</title>
    <link rel="Stylesheet" href="../Styles/StyleSheet.css" />
    <link rel="Stylesheet" href="digitalAvailsPrint.css" />
    <link rel="shortcut icon" href="../Images/favicon.ico" />
    <script type="text/javascript" language="javascript" src="../includes/jquery.min.js"></script>
    <script type="text/javascript" language="javascript" src="digitalAvails.js"></script>
    <script type="text/javascript" language="javascript">
        var stationData;
        var stationOptions;
        $(document).ready(function () {
            numberOfMonths = parseInt($("#<%=numberOfMonths.ClientID %>").val(), 10);
            startMonth = parseInt($("#<%=startMonth.ClientID%>").val(), 10);
            startYear = parseInt($("#<%=year.ClientID %>").val(), 10);
            isPrintPreview = true;
            stationOptions = { startMonth: startMonth, startYear: startYear, numberOfMonths: numberOfMonths };
            Apollo.DigitalAvailsService.GetStationByMarket($("#<%=market.ClientID %>").val(), "", GetStationByMarketCallbackPrintPreview, null, "dropDownStation");
        });
        function ErrorCallback(e) { alert(e._message); }
        function GetStationByMarketCallbackPrintPreview(list) {
            for (var i = 1; i < list.length; i++) {
                stationOptions.stationId = list[i].value;
                Apollo.DigitalAvailsService.GetStationInfoWithMatrix(stationOptions, GetStationInfoWithMatrixCallbackPrintPreview, ErrorCallback);
            }
            $("#availsData").hide();
        }        
        function GetStationInfoWithMatrixCallbackPrintPreview(results) {
            stationData = results;
            $("#stationInfo").css({ "display": "block" });
            $("#stationNameDisplay").html(stationData.Name);
            $("#stationDescDisplay").html(stationData.Description);
            $("#njtKey").css({ "display": (stationData.Market == "New Jersey" ? "block" : "none") });
            $("#martaKey").css({ "display": (stationData.Market.indexOf("Atlanta") != -1 ? "block" : "none") });
            GetWeekStartDates((startMonth - 1), startYear, numberOfMonths);
            CreateTableHeaders();
            $("#compilation").append($("#availsData").html()).append("<br/><br/>");
        }
    </script>
</head>
<body>    
    <form id="form1" runat="server">
    <ajax:ToolkitScriptManager ID="masterScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true" CombineScripts="true" LoadScriptsBeforeUI="false" ScriptMode="Release">
        <Services>
            <asp:ServiceReference Path="~/services/DigitalAvailsService.asmx" />
        </Services>
    </ajax:ToolkitScriptManager>
    <asp:HiddenField ID="station" runat="server" />
    <asp:HiddenField ID="market" runat="server" />
    <asp:HiddenField ID="startMonth" runat="server" />
    <asp:HiddenField ID="year" runat="server" />
    <asp:HiddenField ID="numberOfMonths" runat="server" />
    <div style="margin:10px;">
        <div id="availsData">
            <div id="stationInfo">
                <span class="stationName" id="stationNameDisplay"></span>
                <span class="stationDesc" id="stationDescDisplay"></span>
                <table id="displayKey">
                    <tr>
                        <td>Key:</td>
                        <td style="background-color:#FFFF99;">Paid</td>
                        <td style="background-color:#FF99CC;">GB</td>
                        <td style="background-color:#CCFFCC;">SAB</td>
                        <td style="background-color:#DD0806;">Hold</td>
                        <td style="background-color:#FFCC99;" id="njtKey">NJT</td>
                        <td style="background-color:#FFCC99;" id="martaKey">MARTA</td>
                    </tr>
                </table>
            </div>
            <div id="avails"><table id="availsTable" class="featureTable" style="width:95%;margin:0 auto;" cellspacing="5"></table></div>
        </div>
        <div id="compilation"></div>
    </div>
    </form>
</body>
</html>
