<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchVertSplit.master" AutoEventWireup="true" CodeFile="new_digital_library.aspx.cs" Inherits="digital_new_digital_library" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        body {overflow-y:scroll;}
        ul.searchFilters li {padding:1px 8px !important;margin:0;}        
    </style>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchCriteriaPanel" Runat="Server">
    <script type="text/javascript">
        var imageList = ["http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262537&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262536&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262535&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262534&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262533&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262532&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262531&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262530&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262529&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262528&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262527&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262526&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262525&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262524&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262523&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262522&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262521&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262520&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262519&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262518&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262517&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262516&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262515&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262514&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262513&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262512&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262511&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262510&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262509&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262508&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262507&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262506&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262505&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262504&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262503&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262502&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262501&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262500&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262499&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262498&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262497&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262496&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262495&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262494&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262493&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262492&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262491&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262490&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262489&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262488&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262487&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262486&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262485&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262484&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262483&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262482&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262481&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262480&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262479&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262478&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262477&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262476&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262475&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262474&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262473&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262472&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262471&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262470&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262469&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262468&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262467&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262466&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262465&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262464&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262463&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262462&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262461&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262460&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262459&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262458&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262457&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262456&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262455&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262454&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262453&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262452&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262451&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262450&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262449&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262448&t=1&x=jpg"
            , "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262447&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262446&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262445&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262444&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262443&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262442&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262441&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262440&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262439&t=1&x=jpg", "http://apollo.titan360.com/digital/DigitalLibraryImageHandler.ashx?i=262438&t=1&x=jpg"
        ];
        var selectedImages = [];
        var searchTimeoutId;
        var enterPressedTwice = false;
        var pageSettings = { widthWidth: 1024, windowHeight: 768, columnsPerPage: 4, rowsPerPage: 25 };
        var imageSettings = { width: 220, height: 220 };
        $(document).ready(function () {
            pageSettings.windowWidth = $(window).width();
            pageSettings.windowHeight = $(window).height();
            pageSettings.columnsPerPage = Math.floor(pageSettings.windowWidth / imageSettings.width);
            //How many rows? Enough to fit 100 cells
            pageSettings.rowsPerPage = Math.ceil(100 / pageSettings.columnsPerPage);
            //Generate the table
            var tr = "";
            var td = "";
            var imageCount = 0;
            for (var i = 0; i < pageSettings.rowsPerPage; i++) {
                tr = "<tr style='width:100%;'>";
                td = "";
                for (var j = 0; j < pageSettings.columnsPerPage; j++) {
                    td += "<td id='row_" + imageCount + "' style='width:100%;height:100%;padding:2px;border:1px solid #ffffff;' onmouseover='foo(this);' onmouseout='fooOut(this);' onclick='fooClick(this);'><table><tr><td><img src='" + imageList[imageCount] + "' alt='Image' /></td></tr><tr><td style='text-align:center;'><input type='checkbox' id='" + imageCount + "' /></td></tr><tr><td style='text-align:center;'>Advertiser</td></tr><tr><td style='text-align:center;'>Media Type - Media Form</td></tr><tr><td style='text-align:center;'>Market</td></tr><tr><td style='text-align:center;'>AE</td></tr></table></td>";
                    imageCount++;
                }
                tr += td + "</tr>";
                $("#imageTable").append(tr);
            }
        });
        function foo(cell) {
            cell.style.border = "1px solid #00CCFF";
        }
        function fooOut(cell) {
            cell.style.border = "1px solid #ffffff";
        }
        function fooClick(cell) {
            cell.style.border = "1px solid #ec008c";
            var checkBoxId = cell.id.split('_')[1];
            $("#" + checkBoxId).attr("checked", "checked");
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="searchResultsPanel" Runat="Server">
    <div style="width:100%;display:block;margin:0 auto;">
        <table id="imageTable" style="width:100%;">
        </table>
    </div>
</asp:Content>

