<%@ Page Language="C#" AutoEventWireup="true" CodeFile="photoGalleryImage.aspx.cs" Inherits="Apollo.photogallery_photoGalleryImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Titan Photo Gallery Image</title>
    <link rel="Stylesheet" href="../Styles/StyleSheet.css" />
    <script type="text/javascript" src="../includes/jquery.min.js"></script>
    <script type="text/javascript">
        var photoName = "";
        var photoPath = "";
        $(document).ready(function () {
            photoName = document.getElementById('<%=photoName.ClientID %>').value;
            photoPath = document.getElementById('<%=photoPath.ClientID %>').value;
            //Set Defaults
            document.getElementById("photoGalleryImage").src = "PhotoGalleryImageHandler.ashx?i=" + photoName + "&p=" + photoPath + "&q=d";
            document.getElementById("downloadOriginalLink").href = "PhotoGalleryImageHandler.ashx?i=" + photoName + "&p=" + photoPath + "&q=o&s=1";
        });        
        function ToggleImage(quality) {
            document.getElementById("photoGalleryImage").src = "PhotoGalleryImageHandler.ashx?i=" + photoName + "&p=" + photoPath + "&q=" + quality;
            document.getElementById("downloadLink").href = "PhotoGalleryImageHandler.ashx?i=" + photoName + "&p=" + photoPath + "&q=" + quality + "&s=1";
        }        
    </script>
</head>
<body>    
    <form id="form1" runat="server">
    <div style="margin:10px;">        
        <asp:HiddenField ID="photoName" runat="server" />
        <asp:HiddenField ID="photoPath" runat="server" />
        <div style="margin-bottom:15px;">
            <div style="float:left;margin-right:5px;"><span class="pagerLink"><a href="#" id="downloadLink">Download</a></span></div>
            <div style="float:left;margin-right:5px;"><span id="displayLink" class="pagerLink" onclick="ToggleImage('d');">640 x 480 (Display)</span></div>
            <div style="float:left;margin-right:5px;"><span id="emailLink" class="pagerLink" onclick="ToggleImage('e');">800 x 533 (e-mail)</span></div>
            <div style="float:left;margin-right:5px;"><span id="powerPointLink" class="pagerLink" onclick="ToggleImage('p');">960 x 640 (PowerPoint)</span></div>
            <div style="float:left;margin-right:5px;"><span id="hiResLink" class="pagerLink" onclick="ToggleImage('h');">1200 x 800 (Hi-Res)</span></div>
            <div style="float:left;margin-right:5px;"><span class="pagerLink"><a href="#" id="downloadOriginalLink">Download Original</a></span></div>
            <div style="clear:both"></div>
        </div>
        <img src="" id="photoGalleryImage" alt="Photo Gallery Image" />        
    </div>    
    </form>
</body>
</html>
