<%@ Control Language="C#" AutoEventWireup="true" CodeFile="digitalUploader.ascx.cs" Inherits="Apollo.UserControls_digitalUploader" %>
<link rel="Stylesheet" type="text/css" href="../Styles/Upload.css" />
<script type="text/javascript">
    var imagesUploaded;
</script>
<script type="text/javascript" src="/includes/swfUpload/swfupload.js"></script>
<script type="text/javascript" src="/includes/swfUpload/swfUploadHandlers.js"></script>
<script type="text/javascript">
	var swfu;
	function LoadSWFUploader(batchId,userId) {
	    document.getElementById("uploadImageIdList").value = "";
	    try { document.getElementById('spanButtonPlaceholder') } catch (e) {
	        //if (document.getElementById('spanButtonPlaceholder').id == '') {
	        //When the SWFUpload.destroy method is invoked 
	        //  (and it will be when the 'Back' button is clicked)
	        //it removes the button placeholder element from the DOM
	        //In order to recreate the SWFUpload object
	        //we need to re-create the placeholder
	        var span = document.createElement("span");
	        span.id = "spanButtonPlaceholder";
	        span.style.border = "none";
	        document.getElementById('buttonPlaceHolder').appendChild(span);	    	    
	    }
	    //reset the imagesUploaded flag
	    imagesUploaded = false;
	//window.onload = function () {
		swfu = new SWFUpload({
			// Backend Settings
			upload_url: "/digital/DigitalLibraryUploadHandler.ashx",
            post_params : {
                "ASPSESSID" : "<%=Session.SessionID %>",
                "BATCHID" : batchId,
                "USERID" : userId
            },

			// File Upload Settings			
            file_types: "*.doc; *.flv; *.htm; *.html; *.jpg; *.pdf; *.swf; *.txt; *.xls",
			file_types_description : "Allowed Files",
			file_upload_limit : "0",    // Zero means unlimited

			// Event Handler Settings - these functions as defined in Handlers.js
			//  The handlers are not part of SWFUpload but are part of my website and control how
			//  my website reacts to the SWFUpload events.
			file_queue_error_handler : fileQueueError,
			file_dialog_complete_handler : fileDialogComplete,
			upload_progress_handler : uploadProgress,
			upload_error_handler : uploadError,
			upload_success_handler : uploadSuccess,
			upload_complete_handler : uploadComplete,

			// Button settings
			//button_image_url : "/Images/swfUpload/XPButtonNoText_160x22.png",			
			button_image_url: "/Images/dl/but_add_image.png",
			button_placeholder_id : "spanButtonPlaceholder",
			button_width: 69,
			button_height: 16,
			//button_text : '<span class="button" style="z-index:-1">Upload</span>',
			//button_text : '<span class="button" style="text-align:center">Upload</span>',
			//button_text_style : '.button { font-family: Helvetica, Arial, sans-serif; font-size: 14pt; } .buttonSmall { font-size: 10pt; }',
			//button_text_top_padding: 1,
			//button_text_left_padding: 5,
			button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
			button_cursor: SWFUpload.CURSOR.HAND,
			prevent_swf_caching : true,
			// Flash Settings
			flash_url: "/includes/swfUpload/swfupload.swf", // Relative to this file

			custom_settings : {
				upload_target : "divFileProgressContainer"
			},

			// Debug Settings
			debug: false
        });
    }
    function RemoveWithoutCallback(id,fileExtension){
        $get('imageToRemove').value = id;
        RemoveIdFromUploadList(id);
        RemoveCallback();
        Apollo.DigitalLibraryService.RemoveUploadedFile(id,fileExtension);            
    }
    function Remove(id,fileExtension){
        if (confirm('Are you sure you want to remove this file?')){            
            $get('imageToRemove').value = id;
            RemoveIdFromUploadList(id);
            Apollo.DigitalLibraryService.RemoveUploadedFile(id,fileExtension,RemoveCallback);            
        }
    }
    function RemoveCallback(){
        var id = $get('imageToRemove').value;
        var table = $get('thumbnails');
        var row = $get('trUp' + id);
        table.removeChild(row);
        $get('imageToRemove').value = "";
        //alert('File deleted');
    }
    function RemoveIdFromUploadList(id) {
        var previousValue = $get('uploadImageIdList').value;
        var value = id;
        if (previousValue.indexOf(value) != -1) {
            var allValues = previousValue.split(';');
            var newValue = "";
            for (var i = 0; i < allValues.length; i++) {
                newValue += (allValues[i].split(':')[0] != value) ? ((newValue != "") ? ';' : '') + allValues[i] : '';
            }
        }
        $get('uploadImageIdList').value = newValue;                
    }
    function UploadBackStart(){
        var imageIdList = '';
        if (imagesUploaded){
            imageIdList = $get('uploadImageIdList').value;
        }
        $get('uploadDone').style.display = "none";
        $get('removeAllLink').style.display = "none";
        UploadBack(imagesUploaded,imageIdList);
    }
    function RemoveAll(){
        if (imagesUploaded) {
            if (confirm('Are you sure you want to remove all uploaded files?')){
                var imageList = $get('uploadImageIdList').value.split(';');
                if (imageList==''){return;}
                for (var i=0;i<imageList.length;i++){
                    RemoveWithoutCallback(imageList[i].split(':')[0],imageList[i].split(':')[1]);
                }            
            }
            imagesUploaded=false;
        }
        $get('uploadDone').style.display = "none";
        $get('removeAllLink').style.display = "none";
    }
</script>
<span class="search_filter_title">Digital Library Upload</span>
<br />
<div>
    <div style="float:left;margin-right:10px">
        <img id="uploadCancel" src="../Images/but_cancel.gif" alt="Cancel Uploads" title="Cancel Uploads" onclick="RemoveAll();UploadBackStart();" style="cursor:pointer;margin-right:5px;display:inline;" />        
        <img id="uploadDone" src="../Images/dl/but_tag.png" alt="Complete Uploads" title="Complete Uploads" onclick="UploadBackStart();" style="cursor:pointer;margin:0 5px;display:none;" />
    </div>    
    <div style="float:left" id="buttonPlaceHolder">
        <span id="spanButtonPlaceholder"></span>
    </div>
    <div style="clear:both"></div>
    <div id="removeAllLink" style="display:none;margin:25px;">
        <span class="pagerLink" style="display:inline !important;" onclick="RemoveAll();">Remove All</span>
    </div>    
</div>
<input type="hidden" id="uploadImageIdList" value="" />
<input type="hidden" id="imageToRemove" value="" />
<div id="divFileProgressContainer"></div>        
<table id="thumbnails" class="thumb"></table>
