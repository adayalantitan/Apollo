<%@ Control Language="C#" AutoEventWireup="true" CodeFile="contractDetailPopup.ascx.cs" Inherits="Apollo.UserControls_contractDetailPopup" %>
<script type="text/javascript">
    function ShowContractDetail(contractNumber, companyId) {
        Apollo.IOService.GetContractDetail(("" + contractNumber), ("" + companyId), ShowContractDetailCallback, null, companyId);
    }
    function ShowContractDetailCallback(contractDetailData, companyId) {
        $get('<%=labelDetailContractNumber.ClientID %>').innerHTML = contractDetailData.contractNumber;
        $get('<%=labelDetailContractDate.ClientID %>').innerHTML = contractDetailData.contractStartDate;
        $get('<%=labelDetailEntryDate.ClientID %>').innerHTML = contractDetailData.contractEntryDate;
        $get('<%=labelDetailAgency.ClientID %>').innerHTML = contractDetailData.agency;
        $get('<%=labelDetailAgencyFee.ClientID %>').innerHTML = contractDetailData.agencyFee;
        $get('<%=labelDetailContact.ClientID %>').innerHTML = contractDetailData.contactName;
        $get('<%=labelDetailAdvertiser.ClientID %>').innerHTML = contractDetailData.advertiser;
        $get('<%=labelDetailProgram.ClientID %>').innerHTML = contractDetailData.program;
        $get('<%=labelDetailPONumber.ClientID %>').innerHTML = contractDetailData.agencyPoNumber;
        $get('<%=labelDetailAe1.ClientID %>').innerHTML = contractDetailData.ae1Name;
        $get('<%=labelDetailAe2.ClientID %>').innerHTML = contractDetailData.ae2Name;
        $get('<%=labelDetailAe3.ClientID %>').innerHTML = contractDetailData.ae3Name;
        $get('<%=labelDetailProductClass.ClientID %>').innerHTML = contractDetailData.productClassDescription;
        $get('<%=labelDetailLocality.ClientID %>').innerHTML = contractDetailData.localOrNational;
        $get('detailLineItemsTable').innerHTML = contractDetailData.lineItemTable;
        $get('detailTransactionsTable').innerHTML = contractDetailData.transactionsTable;
        if (contractDetailData.hasAttachments != 0) {
            $get('<%=attachmentLink.ClientID %>').innerHTML = "<span class='pagerLink' onclick='ShowAttachmentPopup(" + contractDetailData.contractNumber + ",\"" + companyId + "\");'>View Attachments</span>";
        }
        //$find('contractDetailPopupExtBehavior').show();
        var dialog = $("<div style='font-size:10px !important;'></div>");
        dialog.addClass("dialog")
            .attr({ "id": "contractDetailDialog" })
            .appendTo("body")
            .dialog({ title: "Contract Details"
                , close: function () { $(this).remove() }
                , modal: true, autoOpen: false
                , width: 675, height: 575
            })
            .html($get("<%=contractDetail.ClientID %>").innerHTML);
        dialog.dialog("open");
    }
    function openPrint() {
        var details = $get('<%=contractDetail.ClientID %>').innerHTML;
        innerOpenPrint(details);
    }
</script>
<div id="contractDetail" runat="server" style="width:675px;height:575px;border:1px solid #333333;background-color:White;padding:10px;display:none">
    <div style="float:left"><img src="/Images/but_print.gif" style="cursor:pointer;" onclick="openPrint();" alt="Print Details" /></div>
    <div style="clear:both"></div>
    <div style="width:100%;border:1px solid #06347a;padding:2px;font-size:8pt;page-break-inside:avoid !important;">
        <div class="spanColumn">
            <div>
                <div style="width:50%;float:left;">
                    Contract Information
                </div>
                <div style="width:50%;float:left;">                                    
                </div>
                <div style="clear:both">
                </div>
            </div>
        </div>
        <div style="width:50%;float:left">
            <ul class="formFields">                                
                <li>
                    <div>
                        <div class="leftColumn">
                            Number: 
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailContractNumber" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            Contract Date:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailContractDate" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="width:50%;float:left">
            <ul class="formFields">                                
                <li>
                    <div>
                        <div class="leftColumn">
                            Entry Date: 
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailEntryDate" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            <br />
                        </div>
                        <div class="rightColumn">
                            <div id="attachmentLink" runat="server"></div>
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="clear:both"></div>
        <div class="spanColumn">
            <div>
                <div style="width:50%;float:left;">
                    Customer &amp; Agency Information
                </div>
                <div style="width:50%;float:left;">                                    
                </div>
                <div style="clear:both">
                </div>
            </div>
        </div>
        <div style="width:50%;float:left">
            <ul class="formFields">                                
                <li>
                    <div>
                        <div class="leftColumn">
                            Agency #1: 
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailAgency" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            Agency #1 %:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailAgencyFee" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            Contact:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailContact" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="width:50%;float:left">
            <ul class="formFields">                                
                <li>
                    <div>
                        <div class="leftColumn">
                            Advertiser: 
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailAdvertiser" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            Program:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailProgram" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            Customer PO Number:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailPONumber" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="clear:both"></div>
        <div class="spanColumn">
            <div>
                <div style="width:50%;float:left;">
                    Sales Rep Information
                </div>
                <div style="width:50%;float:left;">                                    
                </div>
                <div style="clear:both">
                </div>
            </div>
        </div>
        <div style="width:50%;float:left">
            <ul class="formFields">                                
                <li>
                    <div>
                        <div class="leftColumn">
                            Account Executive 1: 
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailAe1" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            Account Executive 2:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailAe2" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
                <li>
                    <div>
                        <div class="leftColumn">
                            Account Executive 3:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailAe3" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="width:50%;float:left"></div>
        <div style="clear:both"></div>
        <div class="spanColumn">
            <div>
                <div style="width:50%;float:left;">
                    Advertisement Information
                </div>
                <div style="width:50%;float:left;">                                    
                </div>
                <div style="clear:both">
                </div>
            </div>
        </div>
        <div style="width:50%;float:left">
            <ul class="formFields">                                
                <li>
                    <div>
                        <div class="leftColumn">
                            Product Class: 
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailProductClass" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>                                
            </ul>
        </div>
        <div style="width:50%;float:left">
            <ul class="formFields">                                
                <li>
                    <div>
                        <div class="leftColumn">
                            National or Local:
                        </div>
                        <div class="rightColumn">
                            <asp:Label ID="labelDetailLocality" runat="server" />                                        
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </li>                                
            </ul>
        </div>                        
        <div style="clear:both;"></div>
        <div id="detailTables" class="historyTable" style="page-break-inside:avoid !important;">
            <div style="clear:both"></div>
            <div id="detailLineItemsTable" style="width:100%;"></div>
            <div style="clear:both"></div>
            <div id="detailTransactionsTable" style="width:100%"></div>
        </div>
    </div>      
</div>