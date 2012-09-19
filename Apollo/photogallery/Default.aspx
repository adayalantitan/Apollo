<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchHorizSplit.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Apollo.photogallery_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" href="../includes/jqueryFileTree/jqueryFileTree.css" />
    <style type="text/css">
        .demo {
				width: 350px;
				height: 425px;
				border-top: solid 1px #BBB;
				border-left: solid 1px #BBB;
				border-bottom: solid 1px #FFF;
				border-right: solid 1px #FFF;
				background: #FFF;
				overflow: scroll;
				padding: 5px;
				margin-left:25px;
			}
		.button {margin-bottom:10px;cursor:pointer;}
		li {text-align:left !important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="reportCriteriaPanel" Runat="Server">    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="reportListPanel" Runat="Server">
    <script type="text/javascript" src="../includes/jqueryFileTree/jqueryFileTree.js"></script>
    <script type="text/javascript">
        var imageListResults;
        var photosPerPage = 50;
        var currentPage = 0;
        $(document).ready(function () {
            var rootFolder = $get('<%=rootFolder.ClientID %>').value;
            $('#fileTree').fileTree({ root: unescape(rootFolder), script: 'PhotoGalleryFileTreeHandler.ashx', usingUnc: true }, onFileClick);
        });
        function onFileClick(file) {
            var fileName = file.substring(file.lastIndexOf("\\") + 1);
            var path = file.substring(0, file.lastIndexOf("\\"));
            PopupFile(fileName, path);
        }
        function ShowFolderContents(folderPath, folderType) {
            //alert(folderPath);
            if (folderType == "containerFolder") {
                Apollo.DigitalLibraryService.GetPhotoGalleryImageList(folderPath, ShowFolderContentsCallback);
            } else {
                $("#photoGalleryImages").remove();
                $(".pagerDiv").remove();  
            }
        }
        function ShowFolderContentsCallback(imageList) {
            imageListResults = imageList;
            if (imageList.length == 0) {
                $("#photoGalleryImages").append("<h4>No Images Found</h4>");
            } else {
                PaginateFolderContents(1);
            }
        }
        function PaginateFolderContents(pageNumber) {
            currentPage = pageNumber;
            $("#photoGalleryImages").remove();
            $(".pagerDiv").remove();                       
            var table = "<table id='photoGalleryImages' style='width:100%;'><tr>";
            var count = 1;
            var startRecord = Math.max((pageNumber * photosPerPage) - photosPerPage, 0);
            var endRecord = Math.min((photosPerPage * pageNumber), imageListResults.length) - ((pageNumber != 1) ? 1 : 0);
            var path, fileName;
            for (var i = 0; i < imageListResults.length; i++) {
                if (i >= startRecord && i <= endRecord) {
                    fileName = imageListResults[i].fileName;
                    path = imageListResults[i].path;
                    table += "<td style='width:25%;text-align:center;border:2px solid #00B0D8;padding:25px;'><img style='cursor:pointer;' onclick='PopupFile(\"" + fileName + "\",\"" + path + "\");' src='PhotoGalleryImageHandler.ashx?i=" + fileName + "&p=" + path + "&t=1' alt='" + fileName + "' /></td>";                    
                    if (count++ % 4 == 0) {
                        table += "</tr><tr>";
                    }
                }
            }
            table += "</tr></table>";
            var paginator = "<div class='pagerDiv' style='float:right;'>" + imageListResults.length + " Images " + GeneratePaginator() + "</div><div class='pagerDiv' style='clear:both'></div>";
            //var paginator = GeneratePaginator();
            $("#imageListDisplay").append(paginator);
            $("#imageListDisplay").append(table);
            $("#imageListDisplay").append(paginator);
        }
        function GeneratePaginator() {
            var pager = "";
            var totalPages = (imageListResults.length + (photosPerPage - 1)) / photosPerPage;
            if (totalPages <= 1 || imageListResults.length <= photosPerPage) {
                return pager;
            }
            if (currentPage - 4 > 1) {
                //First
                pager += "<span class='pagerLink' onclick='PaginateFolderContents(1);'><&nbsp;&nbsp;First</span>";
                //Prev
                pager += "<span class='sep'>|</span><span class='pagerLink' onclick='PaginateFolderContents(" + (currentPage - 1) + ");'>Prev</span><span class='sep'>|</span>";
            }
            for (var i = Math.max((currentPage - 4), 1); i <= Math.min(totalPages, (currentPage + 5)); i++) {
                if (i <= totalPages) {
                    if (i != Math.max((currentPage - 4), 1)) { pager += "<span class='sep'>|</span>"; }
                    if (i != currentPage) {
                        pager += "<span class='pagerLink' onclick='PaginateFolderContents(" + i + ");'>" + i + "</span>";
                    } else {
                        pager += "<b>" + i + "</b>";
                    }
                }
            }
            if ((currentPage + 5) < totalPages) {
                //Next
                pager += "<span class='sep'>|</span><span class='pagerLink' onclick='PaginateFolderContents(" + (currentPage + 1) + ");'>Next</span><span class='sep'>|</span>";
                //Last
                pager += "<span class='pagerLink' onclick='PaginateFolderContents(" + totalPages + ");'>Last&nbsp;&nbsp;></span>";
            }
            return pager;
        }
        function PopupFile(fileName, path) {            
            var url = "photoGalleryImage.aspx?i=" + fileName + "&p=" + path;
            var w = window.open(url, "PhotoGalleryView", "width=660,height=500,scrollbars=yes,resizable=yes");            
            w.focus();

            //alert("File Name:\t" + fileName + "\nPath:\t" + path);
        }
        function PerPageChange(ddl) {
            photosPerPage = ddl.value;
            if (imageListResults !== undefined) {
                PaginateFolderContents(1);
            }
        }
    </script>
    <div style="float:left;width:400px;margin-right:10px;">
        <div id="fileTree" class="demo"></div>
        <div style="margin-top:10px;">
            <label for="dropDownPerPage" style="margin-right:10px;">Photos Per Page:</label>
            <select id="dropDownPerPage" onchange="PerPageChange(this);">
                <option value="25">25</option>
                <option value="50" selected="selected">50</option>
                <option value="100">100</option>
                <option value="10000">All</option>
            </select>
        </div>
    </div>
    <div style="float:left;margin-left:20px;">
        <div id="imageListDisplay"></div>
    </div>
    <asp:HiddenField ID="rootFolder" runat="server" />    
</asp:Content>

