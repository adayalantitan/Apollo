<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="DragNDropTest.aspx.cs" Inherits="Apollo.digital_DragNDropTest" %>
<%@ Register Src="~/UserControls/contractSearch.ascx" TagName="contractSearch" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalTaggerNonRevContract.ascx" TagName="nonRevContract" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalTaggerNonRevLine.ascx" TagName="nonRevLine" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/contractDetailPopup.ascx" TagName="contractDetailPopup" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../Styles/FileAPIStyle.css?v=1.1" />
    <style type="text/css">
        #dropZone{border-radius:5px;border:2px solid #ccc;background-color:#eee;width:250px;padding:50px 0;text-align:center;font-size:18px;color:#555;margin:50px auto;}
        #dropZone.hover{border-color:#aaa;background-color:#ddd;} 
        #dropZone.error{border-color:#f00;background-color:#faa;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HiddenField ID="contractSearchCloseButton" runat="server" />    
    <div style="margin:50px;">        
        <div style="margin:25px auto;width:100%;text-align:center;">
            <h2 style="font-size:12pt;font-weight:bold;">Digital Library - Drag 'N Drop Tagging</h2>
        </div>
        <div style="margin:25px auto;width:75%;">
            <h5>Instructions</h5>
            <ul>
                <li style="font-weight:bold;">For best results, use the latest version of Google Chrome!!</li>
                <li>Choose a Contract to tag to</li>
                <li>Choose Contract Line(s) to tag to</li>
                <li>Fill in any extra Metadata (Notes, Station, Doc. Type, Photo Quality...etc).</li>
                <li>Drag and Drop Image(s) into provided area below.                
                    <ul style="margin-left:20px;">
                        <li>Image(s) can only be dragged 'n dropped once a Contract/Contract Line Item(s) are chosen.</li>
                        <li>Image(s) must be in JPG format</li>
                        <li>Image(s) cannot be larger than 20MB</li>
                        <li>When an Image is dropped it will be Uploaded and Tagged.</li>
                    </ul>
                </li>
                <li>Rinse & Repeat</li>
            </ul>
        </div>
        <div>
            <div style="float:left;margin-right:10px;">
                <uc:contractSearch ID="contractSearch" runat="server" />
                <br /><br />
                <uc:nonRevContract ID="nonRevContract" runat="server" />
                <div style="margin-top:10px;height:300px;width:170px;overflow:scroll;border:1px solid #333333;display:none" id="dragNDropStatus">
                </div>                
            </div>
            <div style="float:left;width:70%;">
                <table cellspacing="1" cellpadding="1" width="100%" id="taggingTable">
                    <tr>
                        <td colspan="5" class="infoBoxContentDark" style="padding:2px 10px;"><b>Attributes</b></td>
                    </tr>
                    <tr style="display:none;">
                        <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Photo Quality:</td>
                        <td nowrap="nowrap"  class="infoBoxContent" width="30%">
                            <asp:CheckBox ID="checkMarketingQuality" runat="server" Text="&nbsp;Marketing" Checked="false" />&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="checkHeroQuality" runat="server" Text="&nbsp;Hero" Checked="false" />
                        </td>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Photo Taken By:</td>
                        <td nowrap="nowrap"  class="infoBoxContent" width="30%">
                            <asp:RadioButton ID="radioPhotographer" runat="server" GroupName="RadioTakenBy" Text="&nbsp;Photographer" Checked="true" />&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="radioInstaller" runat="server" GroupName="RadioTakenBy" Text="&nbsp;Installer" />
                        </td>                
                        <td colspan="3">&nbsp;</td>
                    </tr>            
                    <tr>
                        <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Notes:</td>
                        <td nowrap="nowrap"  class="infoBoxContent" colspan="4">
                            <asp:TextBox ID="textNotes" runat="server" TextMode="MultiLine" Rows="4" Width="100%" />
                        </td>                
                    </tr>      
                    <tr>
                        <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Document Type:</td>
                        <td nowrap="nowrap"  class="infoBoxContent">
                            <asp:DropDownList ID="dropDownDocumentType" runat="server">                        
                                <asp:ListItem Text="Completion Report" Value="R" />
                                <asp:ListItem Text="Contract" Value="C" />
                                <asp:ListItem Selected="True" Text="Photo" Value="I" />
                                <asp:ListItem Text="Copy Receipt" Value="P" />
                            </asp:DropDownList>
                        </td>
                        <td colspan="3">&nbsp;</td>
                    </tr>      
                    <tr>
                        <td nowrap="nowrap"  class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Sales Market Override:</td>
                        <td nowrap="nowrap"  class="infoBoxContent" width="30%">
                            <asp:DropDownList ID="dropDownSubMarketOverride" runat="server">
                                <asp:ListItem Selected="True" Value="" Text="" />
                                <asp:ListItem Value="29" Text="Bronx - Bus" Enabled="false" />
                                <asp:ListItem Value="30" Text="Brooklyn - Bus" Enabled="false" />
                                <asp:ListItem Value="58" Text="LI - Bus" Enabled="false" />
                                <asp:ListItem Value="31" Text="Manhattan - Bus" Enabled="false" />
                                <asp:ListItem Value="32" Text="Queens - Bus" Enabled="false" />
                                <asp:ListItem Value="33" Text="Staten Island - Bus" Enabled="false" />
                                <asp:ListItem Value="8888" Text="Double Banners" />
                                <asp:ListItem Value="4444" Text="Illuminated Bus Shelter" />
                                <asp:ListItem Value="9999" Text="Illuminated Phone Kiosk" />
                                <asp:ListItem Value="7777" Text="Interactive Media" />
                                <asp:ListItem Value="5555" Text="Lucy Bus" />
                                <asp:ListItem Value="3333" Text="NFC" />
                                <asp:ListItem Value="6666" Text="Night Photos" />
                                <asp:ListItem Value="2222" Text="Ticker Kiosk" />
                            </asp:DropDownList>
                        </td>                
                        <td nowrap="nowrap" class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Target Audience:</td>
                        <td nowrap="nowrap" class="infoBoxContent"><asp:DropDownList ID="dropDownEthnicity" runat="server" /></td>                
                        <td>&nbsp;</td>
                    </tr>  
                    <tr>
                        <td nowrap="nowrap" class="infoBoxContentDark" width="15%" style="padding:2px 20px;">Station Override:</td>
                        <td nowrap="nowrap" class="infoBoxContent" style="width:95% !important" colspan="4">
                            <div>
                                <div style="float:left;margin-right:10px">
                                    <asp:DropDownList ID="dropDownStationMarket" runat="server" Width="100px" />
                                </div>
                                <div style="float:left;">
                                    <asp:DropDownList ID="dropDownStation" runat="server" Width="175px" />
                                </div>
                                <div style="clear:both"></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" class="infoBoxContentDark" style="padding:2px 10px;"><b>Tag Information</b></td>
                    </tr>
                </table>
                <div id="tagDetailsTable" runat="server">        
                    <table cellspacing="1" cellpadding="1" width="100%">
                        <tr>
                            <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;">Contract #:</td>
                            <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="contractNumber"></div></td>
                            <td nowrap="nowrap"  class="infoBoxContent" colspan="3">&nbsp;</td>                    
                        </tr>
                        <tr>
                            <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;">Country:</td>
                            <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="country"></div></td>
                            <td nowrap="nowrap" class="infoBoxContentDark" style="padding:2px 20px;width:15%;" width="15%">AE 1</td>
                            <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="ae1Value"></div></td>
                            <td nowrap="nowrap" class="infoBoxContent">&nbsp;</td>                
                        </tr>      
                        <tr>
                            <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;">Agency:</td>
                            <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="agency"></div></td>
                            <td nowrap="nowrap" class="infoBoxContentDark" style="padding:2px 20px;width:15%;" width="15%"><div id="ae2Label"></div></td>
                            <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="ae2Value"></div></td>
                            <td nowrap="nowrap" class="infoBoxContent">&nbsp;</td>               
                        </tr>      
                        <tr>
                            <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;">Advertiser:</td>
                            <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="advertiser"></div></td>
                            <td nowrap="nowrap" class="infoBoxContentDark" style="padding:2px 20px;width:15%;" width="15%"><div id="ae3Label"></div></td>
                            <td nowrap="nowrap" class="infoBoxContent" style="width:30%" width="30%"><div id="ae3Value"></div></td>
                            <td nowrap="nowrap" class="infoBoxContent">&nbsp;</td>                
                        </tr>            
                        <tr>
                            <td class="infoBoxContentDark" style="padding:2px 20px;width:10%;vertical-align:middle;">Contract<br />Lines:</td>
                            <td nowrap="nowrap" class="infoBoxContent" colspan="4">
                                <div id="taggerContractLines" style="display:block" class="taggerContractLinesTable" runat="server"></div>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap"  class="infoBoxContentDark" style="padding:2px 20px;width:10%;">
                                <div id="addNonRevLines" runat="server"><uc:nonRevLine id="nonRevLine" runat="server" /></div>
                            </td>
                            <td nowrap="nowrap" class="infoBoxContent" colspan="4">&nbsp;</td>
                        </tr>            
                    </table>   
                </div>        
            </div>
            <div style="clear:both"></div>
        </div>                        
    </div>    
    <div id="dropZone" class="clearfix" style="width:90%;height:30%;margin:0 auto;display:none;">
		<ul id="dropListing"></ul>
	</div>
    <uc:contractDetailPopup id="contractDetailPopup" runat="server" />
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
    <script type="text/javascript" src="DigitalLibrary.js?v=1.0"></script>
	<script type="text/javascript">
	    var dropZone;
	    var globalGridCloseButtonId = '<%=closeGrid.ClientID %>';
	    var taggingData = {};
	    var droppedFiles = {};
	    function CloseGlobalGrid() {
	        $get('<%=gridPanel.ClientID %>').style.display = "none";
	        $('#globalGrid').GridUnload('globalGrid');
	    }
	    $(document).ready(function () {
	        dropZone = $("#dropZone");
	        dropZone.removeClass("error");
	        // Check if window.FileReader exists to make
	        // sure the browser supports file uploads
	        if (typeof (window.FileReader) == 'undefined') {
	            dropZone.text('Browser Not Supported!');
	            dropZone.addClass('error');
	            return;
	        }
	        // Add a nice drag effect
	        dropZone[0].ondragover = function () {
	            dropZone.addClass('hover');
	            return false;
	        };

	        // Remove the drag effect when stopping our drag
	        dropZone[0].ondragend = function () {
	            dropZone.removeClass('hover');
	            $("#dropListing").empty();
	            return false;
	        };
	        // The drop event handles the file sending
	        dropZone[0].ondrop = function (event) {
	            // Stop the browser from opening the file in the window
	            $("#dropListing").empty();
	            event.stopPropagation();
	            event.preventDefault();
	            if (!PrepForUpload()) { return false; }
	            dropZone.removeClass('hover');
	            $.each(event.dataTransfer.files, function (index, file) {
	                var fileExtension = file.name.substring(file.name.lastIndexOf(".") + 1).toUpperCase();
	                if (fileExtension != "JPG") {
	                    logMessage(file.name + " is not a valid file. Files must be in JPG format.");
	                } else {
	                    var uniqueId = new Date().getTime();
	                    var fileReader = new FileReader();
	                    droppedFiles[uniqueId] = { name: file.name, index: index };
	                    fileReader.onload = (function (file) {
	                        return function (event) {
	                            $("#dropListing").append("<li id='item" + uniqueId + "'><a><img style='width:75px;' src='" + event.target.result + "' /></a></li>");
	                            ProcessXHR(uniqueId, file.name, index, event.target.result);
	                        };
	                    })(file);
	                    fileReader.readAsDataURL(file);
	                }
	            });
	            Clear();
	        }
	    });
	    function logMessage(msg) {
	        var log = true;
	        if (log) {
	            if ($get("dragNDropStatus").style.display == "none") {
	                $get("dragNDropStatus").style.display = "block";
	            }
	            if ($get("dragNDropStatus").innerHTML == "") {
	                $get("dragNDropStatus").innerHTML += msg + "<br/><br/>";
	            } else {
	                var oldHtml = $get("dragNDropStatus").innerHTML;
	                $get("dragNDropStatus").innerHTML = msg + "<br/><br/>" + oldHtml;
	            }
	        }
	    }
	    function ProcessXHR(uniqueId, fileName, index, binary) {
	        var xhr = new XMLHttpRequest();
	        var fileUpload = xhr.upload;
	        var container = $("#item" + uniqueId);

	        $("#item" + uniqueId).append("<div class='progressBar' id='itemProgress" + uniqueId + "'><p>0%</p></div>");
	        fileUpload.log = container;
	        fileUpload.addEventListener("progress", function (event) {
	            if (event.lengthComputable) {
	                var percentage = Math.round((event.loaded * 100) / event.total);
	                if (percentage < 100) {
	                    $("#itemProgress" + uniqueId + " p").css({ "width": (percentage + "px") }).text(percentage + "%");
	                }
	            }
	        }, false);

	        fileUpload.addEventListener("load", function (event) {
	            $("li[id='item" + uniqueId + "']").addClass("loaded");
	            logMessage("xhr upload of " + fileName + " complete");
	        }, false);

	        fileUpload.addEventListener("error", function (error) {
	            logMessage("error: " + error.code);
	        }, false);

	        var params = "file=" + encodeURIComponent(binary) + "&fileName=" + fileName;
	        xhr.open("POST", "DigitalLibraryDragNDropUploadHandler.ashx");
	        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	        xhr.send(params);
	        xhr.onreadystatechange = function () {
	            if (xhr.readyState != 4) { return; }
	            if (xhr.status == 200) {
	                //make sure the image opacity is set 100%
	                //...in case the progress/load events did not fire
	                $("li[id='item" + uniqueId + "']").addClass("loaded");
	                //var container = document.getElementById("item" + currentIndex);
	                //container.className = "loaded";
	                TagImage(xhr.responseText);
	            }
	        }
	    }
	    function TagImage(imageId) {
	        try {
	            var hash = {};
	            var lineItemNumbers = "";
	            for (var i = 0; i < taggingData.contractLines.length; i++) {
	                lineItemNumbers += ((lineItemNumbers != '') ? ';' : '') + taggingData.contractLines[i];
	            }
	            hash["ID"] = imageId;
	            hash["IS_HERO_QUALITY"] = taggingData.isHeroQuality;
	            hash["IS_MARKETING_QUALITY"] = taggingData.isMarketingQuality;
	            hash["TAKEN_BY"] = taggingData.takenBy;
	            hash["NOTES"] = taggingData.notes;
	            hash["CONTRACT_NUMBER"] = taggingData.contractNumber;
	            hash["COMPANY_ID"] = taggingData.companyId;
	            hash["LINE_ITEM_NUMBER"] = lineItemNumbers;
	            hash["SALES_MARKET_OVERRIDE"] = taggingData.salesMarketOverride;
	            hash["STATION_MARKET_ID"] = taggingData.stationMarketId;
	            hash["STATION_ID"] = taggingData.stationId;
	            hash["ETHNICITY_ID"] = taggingData.ethnicityId;
	            Apollo.DigitalLibraryService.TagSelectedSingle(hash, onTagSuccess, onTagError);
	        } catch (e) { alert("An error occurred while trying to tag."); }
	    }
	    function onTagError(e) { alert('An error occurred while trying to tag your Image(s): ' + e._message); }
	    function onTagSuccess() {
	        //alert('Success!'); 
	    }
	    function LoadContractDetail(contractNumber, companyId) {
	        $("#taggerContractLines").html('');
	        $get($get('<%=contractSearchCloseButton.ClientID %>').value).click();
	        Apollo.DigitalLibraryService.LoadContractData(contractNumber, companyId, LoadContractDetailCallback);
	    }
	    function LoadContractDetailCallback(values) {
	        if (values != null) {
	            $("#taggerContractLines").html('');
	            $("#country").html(values["COMPANY"]);
	            $("#agency").html(values["AGENCY"]);
	            $("#advertiser").html(values["ADVERTISER"]);
	            $("#ae1Value").html(values["AE_1_NAME"]);
	            $("#ae2Value").html(values["AE_2_NAME"]);
	            $("#ae2Label").html((values["AE_2_NAME"] == "") ? "" : "AE 2");
	            $("#ae3Value").html(values["AE_3_NAME"]);
	            $("#ae2Label").html((values["AE_3_NAME"] == "") ? "" : "AE 3");
	            $("#" + "<%=taggerContractLines.ClientID %>").html(values["LINE_ITEMS"]);
	            $("#contractNumber").html(values["CONTRACT_NUMBER"]);
	            $get('<%=contractSearch.TaggerContractNumber.ClientID %>').value = "";
	            $get('<%=nonRevLine.ContractNumber.ClientID %>').value = values["CONTRACT_NUMBER"];
	            $get('<%=nonRevLine.CompanyId.ClientID %>').value = values["COMPANY_ID"];
	            //$get('<%=addNonRevLines.ClientID %>').style.display = 'block';
	            $get("taggingTable").style.display = "block";
	            $get("<%=tagDetailsTable.ClientID %>").style.display = "block";
	            //Clear out the tagging fields
	            $("#dropZone").show();
	        } else {
	            $("#" + "<%=taggerContractLines.ClientID %>").html('');
	        }
	    }
	    function Clear() {
	        $get('<%=checkHeroQuality.ClientID %>').checked = false;
	        $get('<%=checkMarketingQuality.ClientID %>').checked = false;
	        $get('<%=radioPhotographer.ClientID %>').checked = true;
	        $get('<%=radioInstaller.ClientID %>').checked = false;
	        $get('<%=textNotes.ClientID %>').value = '';
	        $get('<%=dropDownDocumentType.ClientID %>').value = 'I';
	        $get('<%=dropDownSubMarketOverride.ClientID %>').value = '';
	        $get('<%=dropDownStationMarket.ClientID %>').value = '';
	        $get('<%=dropDownStation.ClientID %>').value = '';
	        $get('<%=dropDownEthnicity.ClientID %>').value = '';
	        //Clear selected lines
	        $('input:checkbox').attr('checked', false);
	    }
	    function PrepForUpload() {
	        taggingData = {};
	        var contractNumber = $("#contractNumber").html();
	        var checkBoxes = $("#" + "<%=taggerContractLines.ClientID %>" + " .lineItemTag");
	        var lineItemNumbers = new Array();
	        for (var i = 0; i < checkBoxes.length; i++) {
	            if (checkBoxes[i].checked) {
	                lineItemNumbers.push(checkBoxes[i].id.split('_')[1]);
	            }
	        }

	        if (contractNumber == "") { alert("Please choose a Contract before attempting to Upload Images."); return false; }
	        if (lineItemNumbers.length == 0) { alert("Please choose a Contract Line before attempting to Upload Images."); return false; }

	        taggingData.isHeroQuality = $get("<%=checkHeroQuality.ClientID %>").checked;
	        taggingData.isMarketingQuality = $get("<%=checkMarketingQuality.ClientID %>").checked;
	        taggingData.takenBy = ($get("<%=radioPhotographer.ClientID %>").checked) ? "photographer" : (($get("<%=radioInstaller.ClientID %>").checked) ? "installer" : "");
	        taggingData.notes = $get("<%=textNotes.ClientID %>").value;
	        taggingData.companyId = 1;
	        taggingData.salesMarketOverride = $get("<%=dropDownSubMarketOverride.ClientID %>").value;
	        taggingData.stationMarketId = $get("<%=dropDownStationMarket.ClientID %>").value;
	        taggingData.stationId = $get("<%=dropDownStation.ClientID %>").value;
	        taggingData.ethnicityId = $get("<%=dropDownEthnicity.ClientID %>").value;
	        taggingData.contractNumber = contractNumber;
	        taggingData.contractLines = lineItemNumbers;
	        return true;
	    }

	    function BindDropDowns() {
	        var ddlStationMarket = $get("<%=dropDownStationMarket.ClientID %>");
	        var ddlStations = $get("<%=dropDownStation.ClientID %>");
	        Apollo.AutoCompleteService.GetMarkets('1', '', AddToList, null, "<%=dropDownStationMarket.ClientID %>");
	        Apollo.AutoCompleteService.GetStationList('', '', AddToList, null, "<%=dropDownStation.ClientID %>");
	        Apollo.AutoCompleteService.GetEthnicities('', AddToList, null, "<%=dropDownEthnicity.ClientID %>");
	        $addHandler(ddlStationMarket, 'change', onMarketChange);
	        $get("taggingTable").style.display = "none";
	        $get("<%=tagDetailsTable.ClientID %>").style.display = "none";
	    }
	    function onMarketChange(sender, e) {
	        var ddlStationMarket = $get("<%=dropDownStationMarket.ClientID %>");
	        Apollo.AutoCompleteService.GetStationList(ddlStationMarket.value, '', AddToList, null, "<%=dropDownStation.ClientID %>");
	    }
	    BindDropDowns();
	</script>
</asp:Content>

