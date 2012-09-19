<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contractDetailPopup.aspx.cs" Inherits="Apollo.popups_contractDetailPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contract Detail</title>
    <link rel="Stylesheet" href="../Styles/StyleSheet.css" />
    <link rel="Stylesheet" href="../includes/jqueryUI/css/custom-theme/jquery-ui-1.8.custom.css" />   
    <link rel="Stylesheet" href="../includes/jqGrid/ui.jqgrid.css" />        
    <link rel="shortcut icon" href="/Images/favicon.ico" />     
    <script type="text/javascript" language="javascript">
    <!--
        function ShowContractDetail(contractNumber, companyId) {
            var hash = new Object();
            hash["CONTRACTNUMBER"] = contractNumber;
            hash["COMPANYID"] = companyId;
            jQuery('#attachmentPopup').dialog({ autoOpen: false, stack: true });
            PageMethods.GetContractDetail(hash, ShowContractDetailCallback);
        }
        function ShowContractDetailCallback(contractDetailData) {
            //show data
            $get('<%=labelDetailContractNumber.ClientID %>').innerHTML = contractDetailData["CONTRACT_NUMBER"];
            $get('<%=labelDetailContractDate.ClientID %>').innerHTML = contractDetailData["CONTRACT_START_DATE"];
            $get('<%=labelDetailEntryDate.ClientID %>').innerHTML = contractDetailData["CONTRACT_ENTRY_DATE"];
            $get('<%=labelDetailAgency.ClientID %>').innerHTML = contractDetailData["AGENCY"];
            $get('<%=labelDetailAgencyFee.ClientID %>').innerHTML = contractDetailData["AGENCY_FEE"];
            $get('<%=labelDetailContact.ClientID %>').innerHTML = contractDetailData["CONTACT_NAME"];
            $get('<%=labelDetailAdvertiser.ClientID %>').innerHTML = contractDetailData["ADVERTISER"];
            $get('<%=labelDetailProgram.ClientID %>').innerHTML = contractDetailData["PROGRAM"];
            $get('<%=labelDetailPONumber.ClientID %>').innerHTML = contractDetailData["AGENCY_PO_NUMBER"];
            $get('<%=labelDetailAe1.ClientID %>').innerHTML = contractDetailData["AE1_NAME"];
            $get('<%=labelDetailAe2.ClientID %>').innerHTML = contractDetailData["AE2_NAME"];
            $get('<%=labelDetailAe3.ClientID %>').innerHTML = contractDetailData["AE3_NAME"];
            $get('<%=labelDetailProductClass.ClientID %>').innerHTML = contractDetailData["PRODUCT_CLASS_DESCRIPTION"];
            $get('<%=labelDetailLocality.ClientID %>').innerHTML = contractDetailData["LOCAL_OR_NATIONAL"];
            $get('detailLineItemsTable').innerHTML = contractDetailData["LINE_ITEM_TABLE"];
            $get('detailTransactionsTable').innerHTML = contractDetailData["TRANSACTIONS_TABLE"];
            if (contractDetailData["HAS_ATTACHMENTS"]!=0){                
                $get('<%=attachmentLink.ClientID %>').innerHTML = "<span class='pagerLink' onclick='ShowAttachmentPopup(" +  contractDetailData["CONTRACT_NUMBER"] + ",1);'>View Attachments</span>";                
            }            
        }    
        function ShowAttachmentPopup(contractNumber,companyId){                        
            jQuery.get('/quattro/QuattroAttachmentHandler.ashx?contractNumber='+contractNumber+'&companyId='+companyId,null,ShowAttachmentPopupCallback,'html');
        }    
        function ShowAttachmentPopupCallback(data){
            //innerShowAttachmentPopupCallback(data);
            jQuery('#attachmentPopup').html(data);
            jQuery('#attachmentPopup').dialog('open');
        }
        function openPrint(){
            var details = $get('<%=contractDetail.ClientID %>').innerHTML;
            innerOpenPrint(details);
        }       
    -->
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <ajax:ToolkitScriptManager ID="popupScriptManager" runat="server" EnablePartialRendering="true" EnablePageMethods="true" CombineScripts="true" LoadScriptsBeforeUI="false" ScriptMode="Release">
        <CompositeScript ScriptMode="Release">                
            <Scripts>                                          
                <asp:ScriptReference Path="~/includes/jquery.min.js" ScriptMode="Release" />                
                <asp:ScriptReference Path="~/includes/jqueryUI/jquery-ui-1.8.20.custom.min.js" />
                <asp:ScriptReference Path="~/includes/jqGrid/grid.locale-en.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/includes/jqGrid/jquery.jqGrid.min.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/includes/hoverIntent-r5.min.js" ScriptMode="Release" />                    
                <asp:ScriptReference Path="~/includes/AppFunctions.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/includes/jqGridHelper.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/includes/shiftClick.js" ScriptMode="Release" />
                <asp:ScriptReference Path="~/includes/cluetip/jquery.cluetip.js" ScriptMode="Release" />                      
                <asp:ScriptReference name="MicrosoftAjax.js" ScriptMode="Release" />
                <asp:ScriptReference name="MicrosoftAjaxWebForms.js" ScriptMode="Release" />	                
            </Scripts>
        </CompositeScript>            
        <Services>
            <asp:ServiceReference Path="~/services/DigitalLibraryService.asmx" />
            <asp:ServiceReference Path="~/services/IOService.asmx" />
            <asp:ServiceReference Path="~/services/AutoCompleteService.asmx" />                                
        </Services>
    </ajax:ToolkitScriptManager>
    <div>
        <div id="contractDetail" runat="server" style="width:675px;height:575px;background-color:White;padding:10px;display:block">
            <div style="float:left"><img src="/Images/but_print.gif" style="cursor:pointer;" onclick="openPrint();" alt="Print Details" /></div>        
            <div style="clear:both"></div>
            <br />
            <div style="width:100%;border:1px solid #06347a;padding:2px;font-size:8pt;">
                <div class="spanColumn">
                    <div>
                        <div style="width:50%;float:left;">Contract Information</div>
                        <div style="width:50%;float:left;"></div>
                        <div style="clear:both"></div>
                    </div>
                </div>
                <div style="width:50%;float:left">
                    <ul class="formFields">                                
                        <li>
                            <div>
                                <div class="leftColumn">Number:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailContractNumber" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn">Contract Date:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailContractDate" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div style="width:50%;float:left">
                    <ul class="formFields">                                
                        <li>
                            <div>
                                <div class="leftColumn">Entry Date:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailEntryDate" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn"><br /></div>
                                <div class="rightColumn"><div id="attachmentLink" runat="server"></div></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div style="clear:both"></div>
                <div class="spanColumn">
                    <div>
                        <div style="width:50%;float:left;">Customer &amp; Agency Information</div>
                        <div style="width:50%;float:left;"></div>
                        <div style="clear:both"></div>
                    </div>
                </div>
                <div style="width:50%;float:left">
                    <ul class="formFields">                                
                        <li>
                            <div>
                                <div class="leftColumn">Agency #1:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailAgency" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn">Agency #1 %:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailAgencyFee" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn">Contact:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailContact" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div style="width:50%;float:left">
                    <ul class="formFields">                                
                        <li>
                            <div>
                                <div class="leftColumn">Advertiser:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailAdvertiser" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn">Program:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailProgram" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn">Customer PO Number:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailPONumber" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div style="clear:both"></div>
                <div class="spanColumn">
                    <div>
                        <div style="width:50%;float:left;">Sales Rep Information</div>
                        <div style="width:50%;float:left;"></div>
                        <div style="clear:both"></div>
                    </div>
                </div>
                <div style="width:50%;float:left">
                    <ul class="formFields">                                
                        <li>
                            <div>
                                <div class="leftColumn">Account Executive 1:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailAe1" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn">Account Executive 2:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailAe2" runat="server" />                                        
                                </div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                        <li>
                            <div>
                                <div class="leftColumn">Account Executive 3:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailAe3" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div style="width:50%;float:left"></div>
                <div style="clear:both"></div>
                <div class="spanColumn">
                    <div>
                        <div style="width:50%;float:left;">Advertisement Information</div>
                        <div style="width:50%;float:left;"></div>
                        <div style="clear:both"></div>
                    </div>
                </div>
                <div style="width:50%;float:left">
                    <ul class="formFields">                                
                        <li>
                            <div>
                                <div class="leftColumn">Product Class:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailProductClass" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>                                
                    </ul>
                </div>
                <div style="width:50%;float:left">
                    <ul class="formFields">                                
                        <li>
                            <div>
                                <div class="leftColumn">National or Local:</div>
                                <div class="rightColumn"><asp:Label ID="labelDetailLocality" runat="server" /></div>
                                <div style="clear:both"></div>
                            </div>
                        </li>                                
                    </ul>
                </div>                        
                <div style="clear:both"></div>
                <div id="detailTables" class="historyTable">                        
                    <br /><div id="detailLineItemsTable" style="width:100%"></div>
                    <br /><div id="detailTransactionsTable" style="width:100%"></div>
                </div>
            </div>      
        </div>    
    </div>
    <div id="attachmentPopup" title="Contract Attachments">
    </div>
    </form>
</body>
</html>
    