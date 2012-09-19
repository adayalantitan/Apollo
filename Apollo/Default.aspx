<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Apollo._Default" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
    <style type="text/css">
        .nav-links {position: absolute;top: 30px;left: 20px; width: 200px; list-style: none; margin: 0; padding: 0;}
        .nav-links li {line-height: 1.6em;	font-size: 1.2em;}
        #content {margin-left: 240px;margin-top: 30px;margin-right: 50px;}                
        .important {border: 1px solid #666;background: #ddd;padding: 0 1em;color: #C30;}
        /* h3 {font-size: 1.45em;line-height: 1.05em;border-bottom: 1px solid #333;} */
        .galleryview {font-size: 12px;font-family: Arial, Helvetica, sans-serif;}
        dt {font-weight: bold;}
        dd {margin-bottom: 0.5em;}                
        .options {border: 1px solid #777;border-right: none;font-size: 0.8em;font-family: Verdana, Geneva, sans-serif;}
        .options th {text-align: left;background: #777;color: white;font-weight: bold;}
        .options th, .options td {padding: 4px 10px;}
        .options td {border-right: 1px solid #777;}
        #parts-img {border: 1px solid black;}
        img.nav {border: 1px solid black;margin-bottom: 5px;}
        #photos.a:link, #photos.a:visited {color: #3671A8;font-weight: bold;text-decoration: none;}
        #photos.a:hover {color: #CC5914;}        
        .panel-overlay h2,.panel-overlay p{margin: .3em 0;}
        .panel-overlay h2 {font-size:14px}
        .panel-overlay p {line-height: 1.2em;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#photos').galleryView({ panel_width: 600, panel_height: 400, frame_width: 220, frame_height: 220 });
            try { RegisterSearchFilterKeyPressHandler(); } catch (e) { }
            $("#fppic").css({ "display": "none" });
            var ntId = $get('<%=ntId.ClientID %>').value.toLowerCase();
        });
        function ExecuteSearch() {
            checkACField($get('<%=aeSearch.Name.ClientID %>'), $get('<%=aeSearch.Id.ClientID %>'));
            if ($get('<%=aeSearch.Id.ClientID %>').value == '' || $get('<%=aeSearch.Name.ClientID %>').value == '') { alert('Please select an AE.');return; }
            try {
                Apollo.DigitalLibraryService.ExecuteAEDashboardSearchJS($get('<%=aeSearch.Id.ClientID %>').value, SearchCallback, SearchError);
            } catch (e) { alert('An error occurred while executing your search.'); }
        }
        function SearchCallback(results) {
            if (results == '') { alert('No images have been uploaded for that AE in the past 7 days.'); return; }
            if (results === undefined) { alert('An error occurred while executing your search.'); return; }
            $get('<%=aePhotoGallery.ClientID %>').style.display = 'none';
            $get('<%=aePhotoGallery.ClientID %>').innerHTML = '';
            $get('<%=aePhotoGallery.ClientID %>').innerHTML = results;
            $get('<%=aePhotoGallery.ClientID %>').style.display = 'block';
            setTimeout(function() { $('#photos').galleryView({ panel_width: 600, panel_height: 400, frame_width: 220, frame_height: 220 }); }, 30);
        }
        function SearchError(e) {
            alert('An error occurred while trying to execute your search:\n' + e._message);
        }
        function onAutoCompleteSearchFilterKeyPress(evt) {            
            //If the user hits the Enter key to select an option
            //  from the autocomplete, we do not want to the form to submit
            //  Add a flag that determines if the Enter key is pressed a second time
            //      (the flag will reset after .5 seconds)          
            if (evt.keyCode == 13 && enterPressedTwice) {
                enterPressedTwice = false;
                searchTimeoutId = setTimeout(ExecuteSearch, 500);
            } else {
                enterPressedTwice = true;
                setTimeout(function() { enterPressedTwice = false; }, 500);
            }
        }    
        function RegisterSearchFilterKeyPressHandler() {
            $addHandler($get('<%=aeSearch.Name.ClientID %>'), "keyup", onAutoCompleteSearchFilterKeyPress);
        }
    </script>
    <div style="margin:25px 0 25px 50px;">        
        <div style="float:left">
            <h4>Welcome to the new Digital Library!</h4><br />
            All photos from the old Digital Library have been imported into the new system. If you have any issues using the new online system, please send an email to <a href="mailto:apps@titan360.com">apps@titan360.com</a>.
            <br /><br />Please check out our youtube page <a href="http://www.youtube.com/TitanWorldwide" target="_blank">http://www.youtube.com/TitanWorldwide</a> for lots of new videos.
        </div>
        <div style="clear:both"></div>
    </div>    
    <div style="margin-left:50px;margin-bottom:25px;" id="aeLookup" runat="server">
        <div>
            <div style="float:left">View an AE's Photos:</div>
            <div style="float:left;margin-left:5px;">
                <uc:searchPopup ID="aeSearch" runat="server" ServiceMethod="GetAEsNonFiltered" GridContext="ae" AutoCompleteIdIndex="0" 
                    AutoCompleteNameIndex="1" SearchImageAlt="Search AEs" SearchImageTitle="Search AEs" />
            </div>
            <div style="float:left;margin-left:10px;"><img id="search" runat="server" style="cursor:pointer" src="~/Images/but_search.gif" onclick="ExecuteSearch();" alt="Search" /></div>
            <div style="clear:both"></div>
        </div>        
    </div>    
    <div style="margin-left:50px" id="aePhotoGallery" runat="server"></div>             
    <div id="specialMessage" runat="server" style="font-size:30px;font-weight:bold;text-align:center"></div>    
    <script type="text/javascript" language="javascript">
        var globalGridCloseButtonId = '<%=closeGrid.ClientID %>';
        function CloseGlobalGrid(){
            $get('<%=gridPanel.ClientID %>').style.display="none";
            $('#globalGrid').GridUnload('globalGrid');
        }
    </script>    
    <asp:HiddenField ID="ntId" runat="server" />
    <asp:Button ID="globalGridDummy" runat="server" style="display:none" UseSubmitBehavior="false" />
    <ajax:ModalPopupExtender ID="globalGridPopupExtender" runat="server" TargetControlID="globalGridDummy" BehaviorID="globalGridPopupExtBehavior"
        PopupControlID="gridPanel" BackgroundCssClass="popupbg" DropShadow="true" CancelControlID="closeGrid" />
    <div id="gridPanel" runat="server" style="width:auto;border:1px solid #333333;background-color:White;padding:10px;display:none;z-index:400 !important;">
        <div style="float:right"><asp:ImageButton ID="closeGrid" runat="server" ImageUrl="/Images/dl/icon_tooltip_close.png" style="cursor:pointer;margin:5px 3px;" OnClientClick="CloseGlobalGrid();" /></div>            
        <br />
        <div style="margin:0 auto;">
            <div style="width:95%;margin:20px;">
                <table id="globalGrid"></table>
            </div>
            <div id="globalGridPager"></div>
        </div>
    </div>
</asp:Content>

