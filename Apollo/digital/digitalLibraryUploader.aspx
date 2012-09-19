<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="digitalLibraryUploader.aspx.cs" Inherits="Apollo.digital_digitalLibraryUploader" %>
<%@ Register Src="~/UserControls/digitalTaggerNonRevContract.ascx" TagName="nonRevContract" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/digitalTaggerNonRevLine.ascx" TagName="nonRevLine" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/contractDetailPopup.ascx" TagName="contractDetailPopup" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../Styles/FileAPIStyle.css?v=1.1" />
    <style type="text/css">
        #dropZone{border-radius:5px;border:2px solid #ccc;background-color:#eee;width:250px;padding:50px 0;text-align:center;font-size:18px;color:#555;margin:50px auto;}
        #dropZone.hover{border-color:#aaa;background-color:#ddd;} 
        #dropZone.error{border-color:#f00;background-color:#faa;}
        .photoAttributes, .tagAttributes {border:1px solid #444444 !important;float:left;padding:7px;}
        legend {font-weight:bold;font-size:1.2em;}
        .sectionHeader {font-weight:bold;font-size:1.2em;margin-bottom:5px;border-bottom:1px solid #555555;width:90%;margin-bottom:10px;}
        .sectionLeftCol {float:left;width:125px;}            
        .sectionRightCol {float:left;margin-left:10px;}
        .tagLeftCol {float:left;width:80px;}
        .tagRightCol {float:left;margin-left:10px;width:150px;}
        .clear {clear:both;margin-bottom:5px;}
        .tagClear {clear:both;margin-bottom:7px;border:1px solid #999999}
        .contractSearchDialog {top:50px !important;width:65% !important;}
        .searchDialog {top:100px !important;width:50% !important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="margin:15px auto;width:100%;text-align:center;">
        <h2 style="font-size:12pt;font-weight:bold;">Digital Library - Drag 'N Drop Tagging</h2>
    </div>
    <div id="instructions" style="margin:25px auto;width:100%;">                
        <div style="margin-left:50px;float:left;">
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
        <div style="margin:25px 0 0 50px;float:left;">
            <div style="float:left;margin-right:10px;">
                <div style="margin-bottom:10px;">
                    <div style="float:left;width:60px;">Company:</div>
                    <div style="float:left;margin-left:10px;">
                        <select id="dropDownContractSearchCompany">
                            <option value="1" selected="selected">Titan US</option>
                            <option value="2">Titan Canada</option>
                        </select>
                    </div>
                    <div style="clear:both;margin-bottom:5px;"></div>
                    <div style="float:left;width:60px;">Contract #:</div>
                    <div style="float:left;margin-left:10px;">
                        <input type="text" id="textContractSearchAutocomplete" onclick="this.select();" />
                    </div>
                    <div style="float:left;margin-left:3px;">
                        <img src="../Images/but_click.gif" alt="Search for Contract" style="cursor:pointer;padding-bottom:7px;" onclick="OpenContractSearchDialog();" />
                    </div>
                    <div style="clear:both;"></div>
                </div>
                <uc:nonRevContract ID="nonRevContract" runat="server" />                
            </div>
        </div>
        <div style="float:left;margin:10px 0 0 25px;">
            <div style="height:150px;width:250px;overflow:scroll;border:1px solid #333333;display:none" id="dragNDropStatus"></div>
        </div>
        <div style="clear:both;"></div>
    </div>
    <div id="taggingSection" style="margin:10px 0 10px 50px;">
        <fieldset id="photoAttributes" class="photoAttributes" style="margin:0 10px 10px 0;">
            <legend>Attributes</legend>
            <div style="display:none">
                <div class="sectionLeftCol">Photo Quality:</div>
                <div class="sectionRightCol">
                    <input type="checkbox" id="checkMarketingQuality" /><label for="checkMarketingQuality" style="padding-left:5px;">Marketing</label>
                    <input type="checkbox" id="checkHeroQuality" /><label for="checkHeroQuality" style="padding-left:5px;">Hero</label>
                </div>
                <div class="clear"></div>
                <div class="sectionLeftCol">Photo Taken By:</div>
                <div class="sectionRightCol">
                    <input type="radio" id="radioPhotographer" name="radioPhotoTakenBy" /><label for="radioPhotographer" style="padding-left:5px;">Photographer</label>
                    <input type="radio" id="radioInstaller" name="radioPhotoTakenBy" /><label for="radioInstaller" style="padding-left:5px;">Installer</label>
                </div>
                <div style="clear:both;margin-bottom:5px;"></div>                    
            </div>
            <div style="float:left;width:125px;">Notes:</div>
            <div style="float:left;margin-left:10px;">
                <textarea id="textNotes" rows="5" cols="35"></textarea>
            </div>
            <div style="clear:both;margin-bottom:5px;"></div>
            <input type="hidden" id="dropDownDocumentType" value="I" />
            <div style="float:left;width:125px;">Sales Market Override:</div>
            <div style="float:left;margin-left:10px;">
                <select id="dropDownSalesMarketOverride">
                    <option value="" selected="selected"></option>
                    <option value="8888">Double Banners</option>
                    <option value="4444">Illuminated Bus Shelter</option>
                    <option value="9999">Illuminated Phone Kiosk</option>
                    <option value="7777">Interactive Media</option>
                    <option value="5555">Lucy Bus</option>
                    <option value="6666">Night Photos</option>
                    <option value="2222">Ticker Kiosk</option>
                </select>
            </div>
            <div style="clear:both;margin-bottom:5px;"></div>
            <div style="float:left;width:125px;">Target Audience:</div>
            <div style="float:left;margin-left:10px;"><select id="dropDownEthnicity"></select></div>
            <div style="clear:both;margin-bottom:5px;"></div>
            <div style="float:left;width:125px;">Station Override:</div>
            <div style="float:left;margin-left:10px;">
                <div style="float:left;margin-right:10px;">
                    <select id="dropDownStationMarket"></select>
                    <br /><br />
                    <select id="dropDownStation"></select>
                </div>
                <div style="clear:both"></div>
            </div>
            <div style="clear:both;"></div>
        </fieldset>
        <fieldset id="tagAttributes" class="tagAttributes">
            <legend>Tag Information</legend>                   
            <div class="tagLeftCol">Contract #:</div>
            <div class="tagRightCol"><span id="labelContractNumber">&nbsp;</span></div>
            <div class="tagLeftCol">Program:</div>
            <div class="tagRightCol" style="width:250px;"><span id="labelProgram">&nbsp;</span></div>
            <div class="tagClear"></div>
            <div class="tagLeftCol">Company:</div>
            <div class="tagRightCol"><span id="labelCompany">&nbsp;</span></div>
            <div class="tagLeftCol">AE 1:</div>
            <div class="tagRightCol"><span id="labelAE1">&nbsp;</span></div>
            <div class="tagClear"></div>
            <div class="tagLeftCol">Agency:</div>
            <div class="tagRightCol"><span id="labelAgency">&nbsp;</span></div>
            <div class="tagLeftCol">AE 2:</div>
            <div class="tagRightCol"><span id="labelAE2">&nbsp;</span></div>
            <div class="tagClear"></div>
            <div class="tagLeftCol">Advertiser:</div>
            <div class="tagRightCol"><span id="labelAdvertiser">&nbsp;</span></div>
            <div class="tagLeftCol">AE 3:</div>
            <div class="tagRightCol"><span id="labelAE3">&nbsp;</span></div>
            <div class="tagClear"></div>
            <div class="tagLeftCol">Contract Lines:</div>
            <div class="sectionRightCol">
                <div id="taggerContractLines" class="taggerContractLinesTable">
                    <table id="taggerContractLineTable" class="pg-paging">
                        <thead>
                            <tr>
                                <th><input type="checkbox" onclick="SelectAllLines(this);" /></th>
                                <th style="width:12%">Market</th>
                                <!--th style="width:15%">Sub-Market</th-->
                                <th style="width:20%">Line Message</th>
                                <th style="width:13%">Profit Center</th>
                                <th style="width:10%">Media Type</th>
                                <th style="width:10%">Media Form</th>
                                <th style="width:10%">Start Date</th>
                                <th style="width:10%">End Date</th>
                                <th style="width:10%">Quantity</th>
                                <th style="width:10%">&nbsp;</th>                                        
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>                            
                </div>
            </div>
            <div class="clear"></div>
            <div class="sectionLeftCol">
                <div id="addNonRevLines" runat="server"><uc:nonRevLine id="nonRevLine" runat="server" /></div>   
            </div>                    
        </fieldset>
        <div style="clear:both;"></div>
    </div>
    <div id="dropZone" class="clearfix" style="width:95%;height:30%;margin:0 auto;display:none;">
		<ul id="dropListing"></ul>
	</div>      
    <div id="contractSearchDialog" style="display:none;">
        <div id="contractSearchParameters" style="width:100%">
            <div style="float:left;width:48%;">
                <ul class="searchFilters">
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Company:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <select id="dropDownContractSearchDialogCompany" style="width:185px"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>   
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Market:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <select id="dropDownContractSearchDialogMarket" style="width:185px"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Sub Market:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <select id="dropDownContractSearchDialogSubMarket" style="width:185px"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Profit Center:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <select id="dropDownContractSearchDialogProfitCenter" style="width:185px"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">AE:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <div style="float:left;">
                                    <input type="text" id="textContractSearchDialogAE" onclick="this.select();" style="width:160px" />                                
                                </div>
                                <div style="float:left;margin-left:5px">
                                    <img id="textContractSearchDialogAE_popup" src="../Images/but_click.gif" alt="Search..." style="cursor:pointer;" onclick="DialogSearchGridOpen(this, 'ae');" />
                                </div>
                                <div style="clear:both;"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                                                              
                </ul>
            </div>            
            <div style="float:left;width:4%"></div>
            <div style="float:left;width:48%;text-align:right">
                <ul class="searchFilters"> 
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Advertiser:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <div style="float:left;">
                                    <input type="text" id="textContractSearchDialogAdvertiser" onclick="this.select();" style="width:160px" />
                                </div>
                                <div style="float:left;margin-left:5px">
                                    <img id="textContractSearchDialogAdvertiser_popup" src="../Images/but_click.gif" alt="Search..." style="cursor:pointer;" onclick="DialogSearchGridOpen(this, 'advertiser');" />
                                </div>
                                <div style="clear:both"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>  
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Agency:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <div style="float:left;">
                                    <input type="text" id="textContractSearchDialogAgency" onclick="this.select();" style="width:160px" />
                                </div>
                                <div style="float:left;margin-left:5px">
                                    <img id="textContractSearchDialogAgency_popup" src="../Images/but_click.gif" alt="Search..." style="cursor:pointer;" onclick="DialogSearchGridOpen(this, 'agency');" />
                                </div>
                                <div style="clear:both"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                    
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Media Type:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <select id="dropDownContractSearchDialogMediaType" style="width:185px"></select>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>       
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Media Form:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <div style="float:left;">
                                    <input type="text" id="textContractSearchDialogMediaForm" onclick="this.select();" style="width:160px" />
                                </div>
                                <div style="float:left;margin-left:5px">
                                    <img id="textContractSearchDialogMediaForm_popup" src="../Images/but_click.gif" alt="Search..." style="cursor:pointer;" onclick="DialogSearchGridOpen(this, 'mediaForm');" />
                                </div>
                                <div style="clear:both"></div>
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div style="float:left;width:30%;text-align:right;margin-right:5px">Program:</div>
                            <div style="float:left;width:65%;text-align:left;">
                                <input type="text" id="textContractSearchDialogProgram" style="width:181px;" />
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                      
                </ul>
            </div>
            <div style="clear:both"></div>
            <div style="width:100%;text-align:center">
                <img src="../Images/but_search.gif" id="executeSearch" style="cursor:pointer" alt="search" onclick="RefreshGrid();" />             
            </div>        
        </div>
        <div style="margin:0 auto;">
            <div style="width:95%;margin:20px;">
                <table id="contractSearchGrid" class="contractSearchGridTable"></table>
            </div>
            <div id="contractSearchGridPager"></div>
        </div>
        <uc:contractDetailPopup id="contractDetailPopup" runat="server" />
    </div>
    <script type="text/javascript" src="DigitalLibrary.js?v=1.0"></script>
	<script type="text/javascript">
	    var dropZone;
	    var taggingData = {};
	    var droppedFiles = {};
	    var selectedContractInfo = {};
	    $(document).ready(function () {
	        SetupDropZone();
	        BindDropDowns();
	        InitAutocompletes();
	        $("#photoAttributes, #tagAttributes").hide();
	    });
	    function SetupDropZone() {
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
	                TagImage(xhr.responseText, fileName);
	            }
	        }
	    }
	    function clearLog() {
	        $("#dragNDropStatus").html("");
	    }
	    function logMessage(msg) {
	        var log = true;
	        if (log) {
	            if ($("#dragNDropStatus").css("display") == "none") {
	                $("#dragNDropStatus").show();
	            }
	            var oldHtml = $("#dragNDropStatus").html();
	            $("#dragNDropStatus").html(msg + "<br/><br/>" + oldHtml);
	        }
	    }
	    function TagImage(imageId, fileName) {
	        try {
	            var hash = {};	            
	            hash["ID"] = imageId;
	            hash["IS_HERO_QUALITY"] = taggingData.isHeroQuality;
	            hash["IS_MARKETING_QUALITY"] = taggingData.isMarketingQuality;
	            hash["TAKEN_BY"] = taggingData.takenBy;
	            hash["NOTES"] = taggingData.notes;
	            hash["CONTRACT_NUMBER"] = taggingData.contractNumber;
	            hash["COMPANY_ID"] = taggingData.companyId;	            
	            hash["SALES_MARKET_OVERRIDE"] = taggingData.salesMarketOverride;
	            hash["STATION_MARKET_ID"] = taggingData.stationMarketId;
	            hash["STATION_ID"] = taggingData.stationId;
	            hash["ETHNICITY_ID"] = taggingData.ethnicityId;
	            var lineItemNumbers = "";
	            for (var i = 0; i < taggingData.contractLines.length; i++) {
	                lineItemNumbers += ((lineItemNumbers != '') ? ';' : '') + taggingData.contractLines[i];
	            }
	            hash["LINE_ITEM_NUMBER"] = lineItemNumbers;
	            Apollo.DigitalLibraryService.TagSelectedSingle(hash, onTagSuccess, onTagError, { imageId: imageId, fileName: fileName });
	        } catch (e) { alert("An error occurred while trying to tag."); }
	    }
	    function onTagError(e, data) { alert('An error occurred while trying to tag your Image(s): ' + e._message); }
	    function onTagSuccess(results, data) {
	        logMessage("Image: " + data.fileName + " tagged. DL Id: " + data.imageId);
	    }
	    function SelectContractFromSearch(id) {
	        var rowData = $("#contractSearchGrid").getRowData(id);
	        var selectedCompanyId = rowData["companyId"];
	        var selectedContractNumber = rowData["contractNumber"];
	        LoadContractDetail(selectedContractNumber, selectedCompanyId);
	        $("#contractSearchDialog").dialog("close");
	    }
	    function LoadContractDetail(contractNumber, companyId) {
	        Apollo.DigitalLibraryService.GetContractData(contractNumber, companyId, LoadContractDetailCallback, ErrorCallback);
	    }
	    function LoadContractDetailCallback(contractDetail) {
	        selectedContractInfo.ContractNumber = contractDetail.ContractNumber;
	        selectedContractInfo.CompanyId = contractDetail.CompanyId;
	        if (contractDetail === undefined || contractDetail == null) { return; }

	        //Fill in Contract header info
	        $("#labelContractNumber").html((contractDetail.ContractNumber == "" ? "&nbsp;" : "<a href='#' onclick='ShowContractDetail(" + contractDetail.ContractNumber + "," + contractDetail.CompanyId + ");'>" + contractDetail.ContractNumber + "</a>"));
	        $("#labelProgram").html((contractDetail.Program == "" ? "&nbsp;" : contractDetail.Program));
	        $("#labelCompany").html((contractDetail.Company == "" ? "&nbsp;" : contractDetail.Company));
	        $("#labelAgency").html((contractDetail.Agency == "" ? "&nbsp;" : contractDetail.Agency));
	        $("#labelAdvertiser").html((contractDetail.Advertiser == "" ? "&nbsp;" : contractDetail.Advertiser));
	        $("#labelAE1").html((contractDetail.AE1 == "" ? "&nbsp;" : contractDetail.AE1));
	        $("#labelAE2").html((contractDetail.AE2 == "" ? "&nbsp;" : contractDetail.AE2));
	        $("#labelAE3").html((contractDetail.AE3 == "" ? "&nbsp;" : contractDetail.AE3));

	        $get('<%=nonRevLine.ContractNumber.ClientID %>').value = contractDetail.ContractNumber;
	        $get('<%=nonRevLine.CompanyId.ClientID %>').value = contractDetail.CompanyId;

	        //Populate Contract Lines
	        $(".taggerLineItemRow").remove();
	        if (contractDetail.ContractDetailLines == null || contractDetail.ContractDetailLines.length == 0) {
	            $("#taggerContractLineTable").append("<tr class='taggerLineItemRow'><td colspan='10'>No taggable Line Items exist</td></tr>");
	            return;
	        }
	        var data = "";
	        var contractLine;
	        for (var i = 0; i < contractDetail.ContractDetailLines.length; i++) {
	            contractLine = contractDetail.ContractDetailLines[i];
	            data += "<tr class='taggerLineItemRow' onmouseover='mouseincolor(this);' onmouseout='mouseoutcolor(this);' onclick='dlContractRowClick(\"check_" + contractLine.LineItemNumber + "\");' >";
	            data += "<td><input class='lineItemTag' type='checkbox' id='check_" + contractLine.LineItemNumber + "' onclick='dlContractRowClick(\"check_" + contractLine.LineItemNumber + "\");' /></td>";
	            data += "<td>" + contractLine.Market + "</td>";
	            data += "<td>" + contractLine.LineMessage + "</td>";
	            data += "<td>" + contractLine.ProfitCenter + "</td>";
	            data += "<td>" + contractLine.MediaType + "</td>";
	            data += "<td>" + contractLine.MediaForm + "</td>";
	            data += "<td>" + contractLine.StartDateDisplay + "</td>";
	            data += "<td>" + contractLine.EndDateDisplay + "</td>";
	            data += "<td>" + contractLine.Quantity + "</td>";
	            data += "<td>" + contractLine.Reason + "</td>";
	            data += "</tr>";
	        }
	        $("#taggerContractLineTable").append(data);
	        $("#photoAttributes, #tagAttributes").show();
	        $("#dropZone").show();
	    }
	    function Clear() {
	        $("#checkHeroQuality").attr("checked", false);
	        $("#checkMarketingQuality").attr("checked", false);
	        $("#radioPhotographer").attr("checked", true);
	        $("#radioInstaller").attr("checked", false);
	        $("#textNotes").val("");
	        $("#dropDownDocumentType").value = 'I';
	        $("#dropDownSalesMarketOverride").val("");
	        $("#dropDownStationMarket").val("");
	        $("#dropDownStation").val("");
	        $("#dropDownEthnicity").val("");
	        //Clear selected lines
	        $("input:checkbox").attr('checked', false);
	        clearLog();
	    }
	    function PrepForUpload() {
	        taggingData = {};
	        var contractNumber = $("#labelContractNumber a").html();
	        var lineItemNumbers = [];
	        $(".lineItemTag").each(function () {
	            if ($(this).is(":checked")) {
	                lineItemNumbers.push($(this).attr("id").split('_')[1]);
	            }
	        });
	        if (contractNumber == "") { alert("Please choose a Contract before attempting to Upload Images."); return false; }
	        if (lineItemNumbers.length == 0) { alert("Please choose a Contract Line before attempting to Upload Images."); return false; }

	        taggingData.isHeroQuality = $("#checkHeroQuality").is(":checked");
	        taggingData.isMarketingQuality = $("#checkMarketingQuality").is(":checked");
	        taggingData.takenBy = ($("#radioPhotographer").is(":checked") ? "photographer" : ($("#radioInstaller").is(":checked") ? "installer" : ""));
	        taggingData.notes = $("#textNotes").val();
	        taggingData.companyId = parseInt(selectedContractInfo.CompanyId, 10);
	        taggingData.salesMarketOverride = $("#dropDownSalesMarketOverride").val();
	        taggingData.stationMarketId = $("#dropDownStationMarket").val();
	        taggingData.stationId = $("#dropDownStation").val();
	        taggingData.ethnicityId = $("#dropDownEthnicity").val();
	        taggingData.contractNumber = contractNumber;
	        taggingData.contractLines = lineItemNumbers;
	        return true;
	    }
        function onACSelectCallback(contractNumber){
            Apollo.DigitalLibraryService.GetContractData(parseInt(contractNumber, 10), parseInt($("#dropDownContractSearchCompany").val(), 10), LoadContractDetailCallback, ErrorCallback);
        }
        function ClearDialogSearchFilters() {
            $("#dropDownContractSearchDialogCompany").val("1");
            $("#dropDownContractSearchDialogMarket, #dropDownContractSearchDialogSubMarket, #dropDownContractSearchDialogProfitCenter, #textContractSearchDialogProgram").val("");
            $("#textContractSearchDialogAE, #textContractSearchDialogAdvertiser, #textContractSearchDialogAgency, #textContractSearchDialogMediaForm, #dropDownContractSearchDialogMediaType").val("");
            $("#textContractSearchDialogAE, #textContractSearchDialogAdvertiser, #textContractSearchDialogAgency, #textContractSearchDialogMediaForm, #dropDownContractSearchDialogMediaType").removeAttr("acvalue");
        }
        function DialogSearchGridOpen(img, context) {            
            var targetId = img.id.split('_')[0];
            var dialog = $("<div></div>");
            dialog.attr("id", "popupSearchDialog").appendTo("body");
            dialog.dialog({ title: "Search", close: function () { $(this).remove(); }, modal: true, autoOpen: false, dialogClass: "searchDialog" });
            dialog.html("<div style='width:95%;margin:20px;'><table id='popupSearchGrid'></table></div><div id='popupSearchGridPager'></div>");
            dialog.dialog("open");
            var filterObject = BuildFilterObjectByContext(context, "", "", ($("#dropDownContractSearchDialogCompany").val() == "1" ? "Titan Outdoor US" : "Titan Outdoor Canada"), $("#dropDownContractSearchDialogCompany").val());
            DisplayStaticGrid(context, "popupSearchGrid", "popupSearchGridPager", filterObject, function (id) {
                var rowData = $("#popupSearchGrid").getRowData(parseInt(id, 10));
                var value = getReturnValue(rowData, getReturnValColName(context));
                var text = getReturnText(rowData, getReturnTextColNames(context), '-');
                $("#" + targetId).attr("acvalue", value);
                $("#" + targetId).val(value + " - " + text);
                $("#popupSearchDialog").dialog("close");
                $("#popupSearchGrid").GridUnload("popupSearchGrid");
                $("#popupSearchDialog").remove();
                RefreshGrid();
            });
        }
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
	                    url: options.url, data: JSON.stringify(postData),
	                    dataType: "json", type: "POST", contentType: "application/json; charset=utf-8",
	                    dataFilter: function (data) { return data; },
	                    success: function (data) { response($.map(data.d, function (item) { return { label: item.Text, value: item.Value} })); },
	                    error: function (XMLHttpRequest, textStatus, errorThrown) { alert(textStatus); }
	                });
	            },
	            select: function (event, ui) {
	                try { event.preventDefault(); } catch (e) { }
	                if (ui.item == null) { return; }
	                $(this).val(ui.item.label).select();
	                if (ui.item.value !== undefined && ui.item.value != null && ui.item.value != -1 && options.callback !== undefined) { options.callback.call(this, ui.item.value); }
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
	    function BindDropDowns() {
	        var ddlStationMarket = $("#dropDownStationMarket");
	        var ddlStations = $("#dropDownStation");
	        Apollo.AutoCompleteService.GetMarkets('1', '', AddToList, null, "dropDownStationMarket");
	        Apollo.AutoCompleteService.GetStationList('', '', AddToList, null, "dropDownStation");
	        Apollo.AutoCompleteService.GetEthnicities('', AddToList, null, "dropDownEthnicity");
	        $("#dropDownStationMarket").bind("change", function () {
	            Apollo.AutoCompleteService.GetStationList($("#dropDownStationMarket").val(), '', AddToList, null, "dropDownStation");
	        });
	        $("#tagDetailsTable").hide();

	        //Populate Contract Search Dialog dropdowns:
	        Apollo.AutoCompleteService.GetCompanies("1", AddToList, ErrorCallback, "dropDownContractSearchDialogCompany");
	        Apollo.AutoCompleteService.GetMarkets("1", "", AddToList, ErrorCallback, "dropDownContractSearchDialogMarket");
	        Apollo.AutoCompleteService.GetProfitCentersDD("1", "", "", AddToList, ErrorCallback, "dropDownContractSearchDialogProfitCenter");
	        Apollo.AutoCompleteService.GetNewMediaTypes("1", "", AddToList, ErrorCallback, "dropDownContractSearchDialogMediaType");
	        Apollo.AutoCompleteService.GetSalesMarket("", AddToList, ErrorCallback, "dropDownContractSearchDialogSubMarket");

	        $("#dropDownContractSearchDialogCompany").bind("change", onContractSearchDialogCompanyChange);
	        $("#dropDownContractSearchDialogMarket").bind("change", onContractSearchDialogMarketChange);
	        $("#dropDownContractSearchDialogProfitCenter").bind("change", onContractSearchDialogProfitCenterChange);
	        $("#dropDownContractSearchDialogMediaType").bind("change", onContractSearchDialogMediaTypeChange);
	        $("#dropDownContractSearchDialogSubMarket").attr("disabled", "disabled");
	    }
	    function onContractSearchDialogCompanyChange(sender, e) {
	        if ($("#dropDownContractSearchDialogCompany").val() == "") { return; }
	        var companyId = $("#dropDownContractSearchDialogCompany").val();
	        Apollo.AutoCompleteService.GetMarkets(companyId, "", AddToList, ErrorCallback, "dropDownContractSearchDialogMarket");
	        Apollo.AutoCompleteService.GetProfitCentersDD(companyId, "", "", AddToList, ErrorCallback, "dropDownContractSearchDialogProfitCenter");
	        Apollo.AutoCompleteService.GetNewMediaTypes(companyId, "", AddToList, ErrorCallback, "dropDownContractSearchDialogMediaType");
	        $("#textContractSearchDialogAE").val("").removeAttr("acvalue");
	        RefreshGrid();
	    }
	    function onContractSearchDialogMarketChange(sender, e) {
	        var companyId = $("#dropDownContractSearchDialogCompany").val();
	        var marketId = $("#dropDownContractSearchDialogMarket").val();
	        Apollo.AutoCompleteService.GetProfitCentersDD(companyId, marketId, "", AddToList, ErrorCallback, "dropDownContractSearchDialogProfitCenter");
	        if (marketId == "NYO") {
	            $("#dropDownContractSearchDialogSubMarket").removeAttr("disabled");
	        } else {
	            $("#dropDownContractSearchDialogSubMarket").attr("disabled", "disabled");
	        }
	    }
	    function onContractSearchDialogProfitCenterChange(sender, e) {
	    }
	    function onContractSearchDialogMediaTypeChange(sender, e) {
	        $("#textContractSearchDialogMediaForm").val();
	        $("#textContractSearchDialogMediaForm").removeAttr("acvalue", "");
	    }
	    function InitAutocompletes() {
	        CreateAutoComplete({ elId: "textContractSearchAutocomplete", url: "../services/AutoCompleteService.asmx/ContractAutoComplete", dependencies: { "companyId": "dropDownContractSearchCompany" }, callback: onACSelectCallback, defaultText: " - Search For Contract - " });
	        CreateAutoComplete({ elId: "textContractSearchDialogAE", url: "../services/AutoCompleteService.asmx/AEAutoComplete", dependencies: { "companyId": "dropDownContractSearchDialogCompany" }, callback: onDialogACSelectCallback });
	        CreateAutoComplete({ elId: "textContractSearchDialogAdvertiser", url: "../services/AutoCompleteService.asmx/AdvertiserAutoComplete", dependencies: { "companyId": "dropDownContractSearchDialogCompany" }, callback: onDialogACSelectCallback });
	        CreateAutoComplete({ elId: "textContractSearchDialogAgency", url: "../services/AutoCompleteService.asmx/AgencyAutoComplete", dependencies: { "companyId": "dropDownContractSearchDialogCompany" }, callback: onDialogACSelectCallback });
            CreateAutoComplete({ elId: "textContractSearchDialogMediaForm", url: "../services/AutoCompleteService.asmx/MediaFormAutoComplete", dependencies: { "companyId": "dropDownContractSearchDialogCompany", "mediaTypeId" : "dropDownContractSearchDialogMediaType" }, callback: onDialogACSelectCallback });
	    }
	    function onDialogACSelectCallback(acValue) {
	        $(this).attr("acvalue", acValue);
	        RefreshGrid();
	    }
	    function OpenContractSearchDialog() {
	        ClearDialogSearchFilters();
	        $("#contractSearchDialog").dialog("destroy");
	        $("#contractSearchDialog").dialog({ title: "Search for Contract", modal: true, autoOpen: false, height: "auto", dialogClass: "contractSearchDialog" });
	        $("#contractSearchDialog").dialog("option", "buttons", { "Cancel": function () { $(this).dialog("close"); } });
	        $("#contractSearchDialog").dialog("open");
	        $("#dropDownContractSearchDialogCompany").removeAttr("disabled");
	        $("#dropDownContractSearchDialogCompany").val($("#dropDownContractSearchCompany").val());
	        $("#textContractSearchDialogAE, #textContractSearchDialogMediaForm, #textContractSearchDialogAdvertiser, #textContractSearchDialogAgency, #textContractSearchDialogMediaForm").attr("acvalue", "");
	        $("#textContractSearchDialogProgram, #textContractSearchDialogAE, #textContractSearchDialogAdvertiser, #textContractSearchDialogAgency, #textContractSearchDialogMediaForm, #dropDownContractSearchDialogMediaType").keyup(function (event) {
	            if (event.keyCode == 13) {
	                RefreshGrid();
	            }
	        });
	        DisplayStaticGrid("dlContracts", "contractSearchGrid", "contractSearchGridPager", getFilterObject(), SelectContractFromSearch);
	        RefreshGrid();
	    }

	    function RefreshGrid() {
	        $("#contractSearchGrid").setGridParam({ postData: getPostParams("dlContracts", getFilterObject()) }).trigger("reloadGrid");
	    }

	    function syncACField(fieldId) {
	        if ($("#" + fieldId).val() == "") {
	            $("#" + fieldId).attr("acvalue", "");
	        }
	    }

	    function getFilterObject() {
	        syncACField("textContractSearchDialogMediaForm");
	        syncACField("textContractSearchDialogAdvertiser");
	        syncACField("textContractSearchDialogAgency");
	        syncACField("textContractSearchDialogAE");
	        syncACField("textContractSearchDialogMediaForm");
	        var filterObject = BuildContractFilterObject($("#dropDownContractSearchDialogCompany").val(),
                $("#dropDownContractSearchDialogMarket").val(),
                $("#dropDownContractSearchDialogProfitCenter").val(),
                $("#dropDownContractSearchDialogMediaType").val(),
                $("#textContractSearchDialogMediaForm").attr("acvalue"), "",
                $("#textContractSearchDialogProgram").val(),
                $("#textContractSearchDialogAdvertiser").attr("acvalue"),
                $("#textContractSearchDialogAgency").attr("acvalue"),
                $("#textContractSearchDialogAE").attr("acvalue"),
                $("#dropDownContractSearchDialogSubMarket").val()
            );
	        return filterObject;
	    }
    </script>
</asp:Content>