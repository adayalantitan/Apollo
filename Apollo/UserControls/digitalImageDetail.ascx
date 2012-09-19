<%@ Control Language="C#" AutoEventWireup="true" CodeFile="digitalImageDetail.ascx.cs" Inherits="Apollo.UserControls_digitalImageDetail" %>
<%@ Register Src="~/UserControls/digitalTagger.ascx" TagName="digitalTagger" TagPrefix="uc" %>
<script type="text/javascript" language="javascript">
    function PopupImageDetail(imageId, details) {
        var contractNumber, fileType, fileExtension, isWebImage;
        if (details != '') {
            contractNumber = details.split('-')[0].replace(/\s/g, '_');
            fileType = (details.split('-')[1] == 'S') ? 'I' : details.split('-')[1];
            fileExtension = details.split('-')[2];
            isWebImage = (details.split('-')[5] != 0);
        } else {
            contractNumber = '000';
            fileType = '';
            fileExtension = '';
        }

        var isAllowedWebImage = ($get("<%=isAllowedWebImage.ClientID %>").value != 0);

        //Set hidden values
        $get('<%=imageDetailImageId.ClientID %>').value = imageId;
        $get('<%=imageDetailContractNumber.ClientID %>').value = contractNumber;
        $get('<%=imageDetailFileType.ClientID %>').value = fileType;
        $get('<%=imageDetailFileExtension.ClientID %>').value = fileExtension;
        $get('<%=popupTagger.ImageId.ClientID %>').value = imageId;
        $get('<%=popupTagger.ContractNumber.ClientID %>').value = (contractNumber == '000') ? '' : contractNumber;
        $get('<%=popupTagger.ImageIds.ClientID %>').value = imageId;
        $get('<%=popupTagger.Extension.ClientID %>').value = fileExtension;

        //Cleanup any left-over values
        $get('imageDetailDisplay').src = "";
        $get('imageDetailDocumentThumbnail').src = "";
        $get('imageDetailViewer').style.display = "block";
        $get('imageDetailImageProperties').style.display = "none";
        $get('imageDetailImageTagging').style.display = "none";

        if (fileType.toUpperCase() == "I") {
            $get('imageDetailDisplay').src = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=d";
            //ImageDetailDesc(800, 533, ("/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=e&s=1&c=" + contractNumber), true);
            ImageDetailDesc(640, 480, ("/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=d&s=1&c=" + contractNumber), true);
            $get('imageDetailOriginalLink').href = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=o&s=1&c=" + contractNumber;
            //imageDetailiPadLink
            //$get('imageDetailiPadLink').href = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=i&s=1&c=" + contractNumber;
            //$get('imageDetail144Link').href = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&d=144&s=1&c=" + contractNumber;
            //$get('imageDetail2400Link').href = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&w=2400&h=1600&s=1&c=" + contractNumber;
            $get('imageDetailPopLink').href = "/digital/DigitalLibraryPopHandler.ashx?i=" + imageId;
            $get('imageDetailImageDisplayAndDownload').style.display = "block";
            $get('imageDetailPopLink').style.display = "block";
            $get('imageDetailDocumentDisplayAndDownload').style.display = "none";
        } else {
            ImageDetailDesc(0, 0, "", false);
            $get('imageDetailDisplay').src = "";
            $get('imageDetailDisplay').style.display = "none";
            $get('imageDetailDocumentThumbnail').src = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&c=" + contractNumber + "&t=1&ft=" + fileType + "&x=" + fileExtension;
            $get('imageDetailDocumentDisplayLink').href = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&c=" + contractNumber + "&ft=" + fileType + "&x=" + fileExtension;
            $get('imageDetailDocumentDownloadLink').href = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&c=" + contractNumber + "&s=1&ft=" + fileType + "&x=" + fileExtension;
            $get('imageDetailImageDisplayAndDownload').style.display = "none";
            $get('imageDetailPopLink').style.display = "none";
            $get('imageDetailDocumentDisplayAndDownload').style.display = "block";
        }
        if (!isAllowedWebImage) {
            $("#unmarkWebImage").hide();
            $("#markWebImage").hide();
        } else {
            if (isWebImage) {
                $("#unmarkWebImage").show();
                $("#markWebImage").hide();
            } else {
                $("#unmarkWebImage").hide();
                $("#markWebImage").show();
            }
        }
        $find('imageDetailPopupExtBehavior').show();
    }
    function ToggleImage(quality) {
        var imageId = $get('<%=imageDetailImageId.ClientID %>').value;
        var contractNumber = $get('<%=imageDetailContractNumber.ClientID %>').value;
        var height, width;
        switch (quality) {
            case "p": width = 960; height = 640; break;
            case "h": width = 1200; height = 800; break;
            case "d": width = 640; height = 480; break;
            default: width = 800; height = 533; break;
        }
        $get('imageDetailDisplay').src = "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=" + quality;
        $get('imageDetailImageProperties').style.display = "none";
        $get('imageDetailViewer').style.display = "block";
        $get('imageDetailImageTagging').style.display = "none";
        ImageDetailDesc(width, height, ("/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=" + quality + "&s=1&c=" + contractNumber), true);
    }
    function ToggleBorder() {
        var border = $get('imageDetailDisplay').style.border;
        $get('imageDetailDisplay').style.border = ((border == "") ? "1px solid #333333" : "");
        $get('imageDetailImageProperties').style.display = "none";
        $get('imageDetailViewer').style.display = "block";
        $get('imageDetailImageTagging').style.display = "none";
    }
    function HideImageDetail(isLocal) {
        if (isLocal) {
            $get('<%=imageDetail.ClientID %>').style.display = "none";
            $get('<%=popupTagger.TaggerUnload.ClientID %>').click();
        } else {
            $get('<%=back.ClientID %>').click();
        }
    }
    function ImageDetailDesc(width, height, link, show) {
        var inner = "";
        if (show) {
            inner = "<span class='imageDetailDesc'>Size: " + width + " x " + height + ", </span>";
            inner += "<span class='pagerLink'><a href='" + link + "'>Download</a></span>";
            $get('imageDetailDesc').style.display = "inline";
            $get('imageDetailDesc').innerHTML = inner;
        } else {
            $get('imageDetailDesc').style.display = "none";
        }
    }
    function GetImageProperties() {
        var imageId = $get('<%=imageDetailImageId.ClientID %>').value;
        var fileType = $get('<%=imageDetailFileType.ClientID %>').value;
        var hash = new Object();
        hash["ID"] = imageId;
        hash["FILE_TYPE"] = fileType;
        Apollo.DigitalLibraryService.GetImageProperties(hash, GetImagePropertiesCallback);
    }
    function GetImagePropertiesCallback(propertyTable) {
        $get('imageDetailImageProperties').innerHTML = propertyTable;
        $get('imageDetailImageProperties').style.display = "block";
        $get('imageDetailViewer').style.display = "none";
        $get('imageDetailImageTagging').style.display = "none";
    }
    function ShowTagger() {
        $get('imageDetailImageTagging').style.display = "block";
        $get('imageDetailImageProperties').style.display = "none";
        $get('imageDetailViewer').style.display = "none";
        $get('<%=popupTagger.TaggerLoad.ClientID %>').click();
    }
    function MarkSingleAsWebImage() {
        var imageId = $get('<%=imageDetailImageId.ClientID %>').value;
        InnerMarkAsWebImage(imageId);
    }
    function UnmarkWebImage() {
        var imageId = $get('<%=imageDetailImageId.ClientID %>').value;
        Apollo.DigitalLibraryService.UnmarkWebImage(imageId, UnmarkWebImageCallback, MarkAsWebImageCallbackError);
    }
    function UnmarkWebImageCallback(results) {
        alert("Image has been umarked.");
        $("#unmarkWebImage").hide();
        $("#markWebImage").show();
    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        var divId = '<%=imageDetail.ClientID %>';
        $("#" + divId).resizable({ aspectRatio: true, minHeight: 555, minWidth: 860 });
    });
</script>
<asp:Button ID="showModalPopup" runat="server" style="display:none" />
<asp:HiddenField ID="isAllowedWebImage" runat="server" />
<ajax:ModalPopupExtender ID="imageDetailPopup" runat="server" PopupDragHandleControlID="logoDrag"
    CancelControlID="back" BehaviorID="imageDetailPopupExtBehavior" PopupControlID="imageDetail" RepositionMode="RepositionOnWindowResizeAndScroll" 
    TargetControlID="showModalPopup" BackgroundCssClass="popupbg" DropShadow="false" />
    <div id="imageDetail" runat="server" style="width:785px;height:550px;border:1px solid #333333;background-color:White;padding:10px;display:none">
        <div style="float:right"><asp:ImageButton ID="back" runat="server" ImageUrl="/Images/dl/icon_tooltip_close.png" style="cursor:pointer;margin:5px 3px;" OnClientClick="HideImageDetail(true)" /></div>
        <asp:HiddenField ID="imageDetailImageId" runat="server" />                    
        <asp:HiddenField ID="imageDetailContractNumber" runat="server" />
        <asp:HiddenField ID="imageDetailFileType" runat="server" />
        <asp:HiddenField ID="imageDetailFileExtension" runat="server" />
        <asp:UpdatePanel ID="imageDetailUpdPnl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>        
                <div>
                    <div style="float:left;width:15%;">
                        <img src="/Images/titan_help_logo.gif" alt="" runat="server" id="logoDrag" /><br /><br />
                        <span class="search_filter_title" style="font-size:16px !important">Document Detail</span><br /><br />
                        <span class="search_filter_title" style="font-size:12px !important">Tagging & Properties</span><br />
                        <ul class="imageDetail">
                            <li><span class="pagerLink" onclick="ShowTagger();">Tagging</span></li>
                            <li><span class="pagerLink" onclick="GetImageProperties();">Properties</span></li>
                        </ul>                                              
                        <br />          
                        <span class="search_filter_title" style="font-size:12px !important">Display & Download</span><br />
                        <div id="imageDetailImageDisplayAndDownload">
                            <ul class="imageDetail">
                                <li><span class="pagerLink" onclick="ToggleImage('d');">For E-Mail</span></li>
                                <!-- li><span class="pagerLink" onclick="ToggleImage('e');">800 x 533 (e-mail)</span></li -->
                                <li><span class="pagerLink" onclick="ToggleImage('p');">For PowerPoint</span></li>
                                <!--li><span class="pagerLink" onclick="ToggleImage('h');">For Hi-Res</span></li-->
                                <!-- li><span class="pagerLink" onclick="ToggleBorder();">Toggle Border</span></li -->
                                <li><span class="pagerLinkRed"><a href="#" id="imageDetailOriginalLink" style="color:Red !important">For Orig/Spec/Hi-Res</a></span></li>
                                <!-- li><span class="pagerLink"><a href="#" id="imageDetailiPadLink">iPad - 1024 x 768 @ 132 dpi (download)</a></span></li>
                                <li><span class="pagerLink"><a href="#" id="imageDetail144Link">144 dpi (download)</a></span></li>
                                <li><span class="pagerLink"><a href="#" id="imageDetail2400Link">2400 x 1600 (download)</a></span></li -->
                                <!--li><span class="pagerLink"><a href="#" id="imageDetailPopLink" target="_blank">For PoP (PDF)</a></span></li-->
                            </ul>
                        </div>
                        <div id="markWebImage"><img src="/Images/dl/but_web_image.png" alt="Mark as Web Image" style="cursor:pointer;margin:10px 0;" onclick="MarkSingleAsWebImage();" /></div>
                        <div id="unmarkWebImage"><img src="/Images/dl/but_unmark.png" alt="Unmark Web Image" style="cursor:pointer;margin:10px 0;" onclick="UnmarkWebImage();" /></div>                        
                        <span style="text-align: left; display: block;">Right click text above to email/send photo link (internal only).</span>
                        <div id="imageDetailDocumentDisplayAndDownload" style="display:none">
                            <ul class="imageDetail">
                                <li><span class="pagerLink"><a href="#" target="_blank" id="imageDetailDocumentDisplayLink">Display</a></span></li>
                                <li><span class="pagerLink"><a href="#" id="imageDetailDocumentDownloadLink">Download</a></span></li>
                                <li style="margin-top:5px;"><img id="imageDetailDocumentThumbnail" src="" alt="" /></li>
                            </ul>
                        </div>
                    </div>
                    <div style="float:left;width:83%;margin:20px 0 0 10px;">
                        <div id="imageDetailViewer">
                            <div id="imageDetailDesc" style="display:none"></div>
                            <br />
                            <div style="width:715px;height:500px;overflow:auto;display:block">
                                <img src="" id="imageDetailDisplay" alt="" style="display:inline" />
                            </div>
                        </div>
                        <div id="imageDetailImageProperties" style="width:715px;height:500px;overflow:auto;">
                        </div>
                        <div id="imageDetailImageTagging"><uc:digitalTagger ID="popupTagger" runat="server" TaggerContext="Detail" /></div>
                    </div>
                    <div style="clear:both"></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>            
    </div>
