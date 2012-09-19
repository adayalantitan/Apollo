<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="aeImageTest.aspx.cs" Inherits="Apollo.aeImageTest" %>
<%@ Register Src="~/UserControls/searchPopup.ascx" TagName="searchPopup" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .nav-links {position: absolute;top: 30px;left: 20px; width: 200px; list-style: none; margin: 0; padding: 0;}
        .nav-links li {line-height: 1.6em;	font-size: 1.2em;}
        #content {margin-left: 240px;margin-top: 30px;margin-right: 50px;}
        .twitthis {position: absolute;top: 250px;left: 20px;}
        pre {background: #e8e8e8;border-left: 10px solid #777;font-size: 0.85em;padding: 1em;color: black !important;overflow-x: auto;}
        .important {border: 1px solid #666;background: #ddd;padding: 0 1em;color: #C30;}
        /* h3 {font-size: 1.45em;line-height: 1.05em;border-bottom: 1px solid #333;} */
        .galleryview {font-size: 12px;font-family: Arial, Helvetica, sans-serif;}
        dt {font-weight: bold;}
        dd {margin-bottom: 0.5em;}
        .code_wrapper {border: 1px solid #888;background: #f0f0f0;padding: 10px;}
        code, .code {}
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
        $(document).ready(function() {
            $('#photos').galleryView({ panel_width: 600, panel_height: 400, frame_width: 220, frame_height: 220 });
        });
        function ExecuteSearch() {
            checkACField($get('<%=aeSearch.Name.ClientID %>'), $get('<%=aeSearch.Id.ClientID %>'));
            try {
                Apollo.DigitalLibraryService.ExecuteAEDashboardSearchJS($get('<%=aeSearch.Id.ClientID %>').value,SearchCallback,SearchError);                
            } catch (e) { alert('An error occurred while executing your search.'); }
        }
        function SearchCallback(results) {
            if (results == '') { alert('No results for that AE were found'); return; }
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
    </script>
    <div style="margin:25px 0 25px 50px;">        
        <div style="float:left">
            <h4>Welcome to the new Digital Library!</h4><br />
            All photos from the old Digital Library have been imported into the new system. If you have any issues using the new online system, please send an email to <a href="mailto:apps@titan360.com">apps@titan360.com</a>.
        </div>
        <div style="clear:both"></div>
    </div>    
    <div style="margin-left:50px;margin-bottom:25px;" id="aeLookup" runat="server">
        <uc:searchPopup ID="aeSearch" runat="server" ServiceMethod="GetAEsNonFiltered"
            GridContext="ae" AutoCompleteIdIndex="0" AutoCompleteNameIndex="1"
            SearchImageAlt="Search AEs" SearchImageTitle="Search AEs" />
        <br />
        <img id="search" runat="server" style="cursor:pointer" src="~/Images/but_search.gif" onclick="ExecuteSearch();" alt="Search" />
    </div>
    <div style="margin-left:50px" id="aePhotoGallery" runat="server"></div>                 
</asp:Content>

