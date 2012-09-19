<%@ Control Language="C#" AutoEventWireup="true" CodeFile="digitalImageDetailNew.ascx.cs" Inherits="Apollo.UserControls_digitalImageDetailNew" %>
<%@ Register Src="~/UserControls/digitalTagger.ascx" TagName="digitalTagger" TagPrefix="uc" %>
<style type="text/css">
    .ui-widget-header 
    {
        background: #00ccff;
        color: #ffffff;
        font-weight:bold;
        border:none !important;
    }
</style>
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
        $("#imageDetailImageId").val(imageId);
        $("#imageDetailContractNumber").val(contractNumber);
        $("#imageDetailFileType").val(fileType);
        $("#imageDetailFileExtension").val(fileExtension);
        $get("<%=popupTagger.ImageId.ClientID %>").value = imageId;
        $get("<%=popupTagger.ContractNumber.ClientID %>").value = (contractNumber == "000") ? "" : contractNumber;
        $get("<%=popupTagger.ImageIds.ClientID %>").value = imageId;
        $get("<%=popupTagger.Extension.ClientID %>").value = fileExtension;        

        //Cleanup any left-over values
        $("#imageDetailDisplay").attr("src", "");
        $("#imageDetailDocumentThumbnail").attr("src", "");
        $("#imageDetailViewer").css({ "display": "block" });
        $("#imageDetailImageProperties").css({ "display": "none" });
        $("#imageDetailImageTagging").css({ "display": "none" });

        if (fileType.toUpperCase() == "I") {
            $("#imageDetailDisplay").attr("src", "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=d");
            ImageDetailDesc(640, 480, ("/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=d&s=1&c=" + contractNumber), true);
            $("#imageDetailOriginalLink").attr("href", "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=o&s=1&c=" + contractNumber);
            $("#imageDetailPopLink").attr("href", "/digital/DigitalLibraryPopHandler.ashx?i=" + imageId);
            $("#imageDetailImageDisplayAndDownload").css({ "display": "block" });
            $("#imageDetailPopLink").css({ "display": "block" });
            $("#imageDetailDocumentDisplayAndDownload").css({ "display": "none" });
        } else {
            ImageDetailDesc(0, 0, "", false);
            $("#imageDetailDisplay").attr("src", "");
            $("#imageDetailDisplay").css({ "display": "none" });
            $("#imageDetailDocumentThumbnail").attr("src", "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&c=" + contractNumber + "&t=1&ft=" + fileType + "&x=" + fileExtension);
            $("#imageDetailDocumentDisplayLink").attr("href", "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&c=" + contractNumber + "&ft=" + fileType + "&x=" + fileExtension);
            $("#imageDetailDocumentDownloadLink").attr("href", "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&c=" + contractNumber + "&s=1&ft=" + fileType + "&x=" + fileExtension);
            $("#imageDetailImageDisplayAndDownload").css({ "display": "none" });
            $("#imageDetailPopLink").css({ "display": "none" });
            $("#imageDetailDocumentDisplayAndDownload").css({ "display": "block" });
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
        //$find('imageDetailPopupExtBehavior').show();
        $("#imageDetail").dialog("open");
    }
    function ToggleImage(quality) {
        var imageId = $("#imageDetailImageId").val();
        var contractNumber = $("#imageDetailContractNumber").val();
        var height, width;
        switch (quality) {
            case "p": width = 960; height = 640; break;
            case "h": width = 1200; height = 800; break;
            case "d": width = 640; height = 480; break;
            default: width = 800; height = 533; break;
        }
        $("#imageDetailDisplay").attr("src", "/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=" + quality);
        $("#imageDetailImageProperties").css({ "display": "none" });
        $("#imageDetailViewer").css({ "display": "block" });
        $("#imageDetailImageTagging").css({ "display": "none" });
        ImageDetailDesc(width, height, ("/digital/DigitalLibraryImageHandler.ashx?i=" + imageId + "&q=" + quality + "&s=1&c=" + contractNumber), true);
    }
    function HideImageDetail(isLocal) {
        if (isLocal) {
            $get('<%=popupTagger.TaggerUnload.ClientID %>').click();
        }
        $("#imageDetail").dialog("close");
    }
    function ImageDetailDesc(width, height, link, show) {
        var inner = "";
        if (show) {
            inner = "<span class='imageDetailDesc'>Size: " + width + " x " + height + ", </span>";
            inner += "<span class='pagerLink'><a href='" + link + "'>Download</a></span>";
            $("#imageDetailDesc").css({ "display": "inline" });
            $("#imageDetailDesc").html(inner);
        } else {
            $("#imageDetailDesc").css({ "display": "none" });
        }
    }
    function GetImageProperties() {
        var imageId = $("#imageDetailImageId").val();
        var fileType = $("#imageDetailFileType").val();
        var hash = new Object();
        hash["ID"] = imageId;
        hash["FILE_TYPE"] = fileType;
        Apollo.DigitalLibraryService.GetImageProperties(hash, GetImagePropertiesCallback);
    }
    function GetImagePropertiesCallback(propertyTable) {
        $("#imageDetailImageProperties").html(propertyTable);
        $("#imageDetailImageProperties").css({ "display": "block" });
        $("#imageDetailViewer").css({ "display": "none" });
        $("#imageDetailImageTagging").css({ "display": "none" });
    }
    function ShowTagger() {
        $("#imageDetailImageTagging").css({ "display": "block" });
        $("#imageDetailImageProperties").css({ "display": "none" });
        $("#imageDetailViewer").css({ "display": "none" });
        $get('<%=popupTagger.TaggerLoad.ClientID %>').click();
    }
    function MarkSingleAsWebImage() {
        var imageId = $("#imageDetailImageId").val();
        InnerMarkAsWebImage(imageId);
    }
    function UnmarkWebImage() {
        var imageId = $("#imageDetailImageId").val();
        Apollo.DigitalLibraryService.UnmarkWebImage(imageId, UnmarkWebImageCallback, MarkAsWebImageCallbackError);
    }
    function UnmarkWebImageCallback(results) {
        alert("Image has been umarked.");
        $("#unmarkWebImage").hide();
        $("#markWebImage").show();
    }
</script>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#imageDetail").dialog({ autoOpen: false, modal: true, height: 550, width: 800 });
    });
</script>
<asp:HiddenField ID="isAllowedWebImage" runat="server" />
<div id="imageDetail" style="background-color:White;padding:10px;display:none">
    <!--div style="float:right"><img alt="Close" src="/Images/dl/icon_tooltip_close.png" style="cursor:pointer;margin:5px 3px;" onclick="HideImageDetail(true);") /></div-->
    <input type="hidden" id="imageDetailImageId" />
    <input type="hidden" id="imageDetailContractNumber" />
    <input type="hidden" id="imageDetailFileType" />
    <input type="hidden" id="imageDetailFileExtension" />    
    <div>
        <div style="float:left;width:15%;">
            <img src="/Images/titan_help_logo.gif" alt="" id="logoDrag" style="width:128px;height:29px;margin-left:-5px;" /><br /><br />
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
                    <li><span class="pagerLink" onclick="ToggleImage('p');">For PowerPoint</span></li>
                    <!--li><span class="pagerLink" onclick="ToggleImage('h');">For Hi-Res</span></li-->
                    <li><span class="pagerLink" ><a href="#" id="imageDetailOriginalLink" style="color:Red !important">For Orig/Spec/Hi-Res</a></span></li>
                    <li><span class="pagerLink"><a href="#" id="imageDetailPopLink" target="_blank">For PoP (PDF)</a></span></li>
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
                <div>
                    <img src="" id="imageDetailDisplay" alt="" style="display:inline" />
                </div>
            </div>
            <div id="imageDetailImageProperties">
            </div>
            <div id="imageDetailImageTagging"><uc:digitalTagger ID="popupTagger" runat="server" TaggerContext="Detail" /></div>
        </div>
        <div style="clear:both"></div>
    </div>
</div>
