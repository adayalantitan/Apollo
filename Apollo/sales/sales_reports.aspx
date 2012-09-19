<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchVertSplit.master" AutoEventWireup="true" CodeFile="sales_reports.aspx.cs" Inherits="Apollo.sales_sales_reports" Title="Commissions Reports | Titan 360" %>
<%@ MasterType VirtualPath="~/App_MasterPages/ApolloSearchVertSplit.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="searchCriteriaPanel" Runat="Server">

    <script type="text/javascript" language="javascript">
        var errorStack;
        function ClearErrorDiv() {
            var errorDiv = document.getElementById('<%=errorDiv.ClientID%>');
            errorDiv.innerHTML = "";
            errorStack = "";
        }
        function SetErrorDiv() {
            var errorDiv = document.getElementById('<%=errorDiv.ClientID%>');
            errorDiv.innerHTML = errorStack.replace(/\n/g, '<br/>');
        }
        function AddErrorMessage(msg) {
            errorStack += (errorStack != "") ? ("\n" + msg) : msg;
        }
        function ShowCriteriaArea(areaId) {
            if ($get('<%=isProcessing.ClientID %>').value==1){return;}            
            Done();
            var hash = {};            
            hash["area1"] = '<%=accountingCriteria.ClientID %>';
            hash["area2"] = '<%=accountingByAeCriteria.ClientID %>';
            hash["area3"] = '<%=commissionFlashCriteria.ClientID %>';
            hash["area4"] = '<%=commissionAnalysisCriteria.ClientID %>';
            hash["area5"] = '<%=aeCommissionByMarket.ClientID %>';
            hash["area6"] = '<%=processCommissions.ClientID %>';
            ClearErrorDiv();
            $get('<%=commissionProcessInfo.ClientID %>').style.display = "block";
            for (var key in hash) {
                $get(hash[key]).style.display = (hash[key] == areaId) ? "block" : "none";                
            }
        }
        function Validate(startDateValue, endDateValue) {
            var bValid = true;
            if (!IsValidDate(startDateValue)) {
                AddErrorMessage('Start date is not in a valid format');
                bValid = false;
            }
            if (!IsValidDate(endDateValue)) {
                AddErrorMessage('End date is not in a valid format');
                bValid = false;
            }
            if (!IsValidDateRange(startDateValue, endDateValue)) {
                AddErrorMessage('End date must be greater than or equal to Start Date');
                bValid = false;
            }
            return bValid;
        }
        function ValidateAccounting() {
            startDate = $get('<%=textACRLineItemStartDate.ClientID %>').value;
            endDate = $get('<%=textACRLineItemEndDate.ClientID %>').value;
            ClearErrorDiv();
            if (!Validate(startDate, endDate)) {
                SetErrorDiv();
                return false;
            }            
            return true;            
        }
        function ValidateAccountingByAE() {
            startDate = $get('<%=textACRAELineItemStartDate.ClientID %>').value;
            endDate = $get('<%=textACRAELineItemEndDate.ClientID %>').value;
            ae = $get('<%=listACRAE.ClientID %>').value;
            ClearErrorDiv();
            if (!Validate(startDate, endDate) || !ValidateAE(ae)) {
                SetErrorDiv();
                return false;
            }            
            return true;  
        }
        function ValidateFlash() {
            startDate = $get('<%=textFlashFromDate.ClientID %>').value;
            endDate = $get('<%=textFlashThruDate.ClientID %>').value;
            ae = $get('<%=listFlashAE.ClientID %>').value;            
            ClearErrorDiv();
            if (!Validate(startDate, endDate) || !ValidateAE(ae)) {
                SetErrorDiv();
                return false;
            }            
            return true;
        }
        function ValidateAnalysis() {
            asOfDate = $get('<%=textAsOfDate.ClientID %>').value;
            ClearErrorDiv();
            if (!IsValidDate(asOfDate)){
                AddErrorMessage('As Of date is not in a valid format');
                SetErrorDiv();
                return false;
            }
            return true;
        }
        function ValidateCommissionByMarket() {
            asOfDate = $get('<%=textCommissionByMarketAsOFDate.ClientID %>').value;
            ClearErrorDiv();
            if (!IsValidDate(asOfDate)) {
                AddErrorMessage('As of date is not in a valid format');
                SetErrorDiv();
                return false;
            }
            return true;
        }
        function ValidateAE(ae) {
            if (ae == "") {
                AddErrorMessage('Please select AE(s) from the List');
                return false;
            }
            return true;
        }
        function IsValidDateRange(startDateValue, endDateValue) {
            return (Date(startDateValue) <= Date(endDateValue));
        }
        function Done() {
            $get('<%=labelMessage.ClientID %>').innerHTML = '';
            $get('<%=reportDone.ClientID %>').style.display = 'none';
        }
        function ValidateProcessCommissions() {
            ClearErrorDiv();
            if ($get('<%=dropDownProcessYear %>').value == '') {
                AddErrorMessage('Commission Process Year must be selected.');
                SetErrorDiv();
                return false;
            }
            return true;
        }
        function onFlashTypeChange() {
            var useInvoiceCreationDate = $get("<%=radioFlashByInvoice.ClientID %>").checked;
            var today = new Date();
            //$get("<%=textFlashFromDate.ClientID %>").value = (useInvoiceCreationDate ? ("1/1/" + today.getFullYear()) : "1/1/2002");            
            $get("<%=textFlashFromDate.ClientID %>").value = "1/1/2002";
            $get("<%=textFlashThruDate.ClientID %>").value = ((today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear());
        }
        function onCompanyDdlChange(ddl) {
            var companyId = ddl.value;
            var ddlId = ddl.id;
            switch (ddlId) {
                case $get("<%=dropDownACByAERptCompany.ClientID %>").id:
                    if (companyId == "1") {
                        $get("<%=listACRAE.ClientID %>").style.display = "block";
                        $get("<%=listACRAECan.ClientID %>").style.display = "none";
                    } else {
                        $get("<%=listACRAE.ClientID %>").style.display = "none";
                        $get("<%=listACRAECan.ClientID %>").style.display = "block";
                    }
                    break;
                case $get("<%=dropDownFlashRptCompany.ClientID %>").id:
                    if (companyId == "1") {
                        $get("<%=listFlashAE.ClientID %>").style.display = "block";
                        $get("<%=listFlashAECan.ClientID %>").style.display = "none";
                    } else {
                        $get("<%=listFlashAE.ClientID %>").style.display = "none";
                        $get("<%=listFlashAECan.ClientID %>").style.display = "block";
                    }
                    break;
                default: break;
            }
        }
    </script>
    <div class="inventory_search">REPORTS</div>
    <div id="searchCriteriaArea">
        <ul class="searchFilters">
            <li>
                <div class="spanColumnNoColor"><a href="#" onclick="ShowCriteriaArea('<%=accountingCriteria.ClientID %>');">Accounting</a></div>                
                <div style="clear:both"></div>
            </li>
            <li>
                <div class="spanColumnNoColor"><a href="#" onclick="ShowCriteriaArea('<%=accountingByAeCriteria.ClientID %>');">Accounting By AE</a></div>                
                <div style="clear:both"></div>
            </li>
            <li>
                <div class="spanColumnNoColor"><a href="#" onclick="ShowCriteriaArea('<%=commissionFlashCriteria.ClientID %>');">Commission Flash</a></div>                
                <div style="clear:both"></div>
            </li>                        
            <li>
                <div class="spanColumnNoColor"><a href="#" onclick="ShowCriteriaArea('<%=commissionAnalysisCriteria.ClientID %>');">Commission Analysis</a></div>                
                <div style="clear:both"></div>
            </li>                
            <li>
                <div class="spanColumnNoColor"><a href="#" onclick="ShowCriteriaArea('<%=aeCommissionByMarket.ClientID %>');">AE Commission By Market</a></div>                
                <div style="clear:both"></div>
            </li>                
            <li>
                <div class="spanColumnNoColor"><a href="#" onclick="ShowCriteriaArea('<%=processCommissions.ClientID %>');">Process Commissions</a></div>                
                <div style="clear:both"></div>
            </li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchResultsPanel" Runat="Server">
    <div id="errorDiv" class="errorDisplay" style="margin:3px 0;" runat="server"></div>
    <div style="height:100%;width:100%;text-align:center" id="reportSelectionArea">        
        <asp:UpdateProgress ID="reportSelectionUpdPnlProgress" AssociatedUpdatePanelID="reportSelectionUpdPnl" runat="server">
            <ProgressTemplate>
                <div style="margin:5px;"><img src="../Images/pleasewait.gif" alt="Please Wait" /></div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="reportSelectionUpdPnl" runat="server" UpdateMode="Conditional">            
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="runACRReport" />
                <asp:AsyncPostBackTrigger ControlID="runARCAEReport" />
                <asp:AsyncPostBackTrigger ControlID="runFlashReport" />
                <asp:AsyncPostBackTrigger ControlID="runAnalysisReport" />
                <asp:AsyncPostBackTrigger ControlID="runCommissionByMarketReport" />
                <asp:AsyncPostBackTrigger ControlID="runProcessCommissions" />
            </Triggers>
            <ContentTemplate>     
                <asp:HiddenField ID="isProcessing" runat="server" Value="0" />           
                <asp:Label ID="labelMessage" runat="server" />                
                <center><img id="reportDone" alt="Done" style="margin:10px 0;display:none;cursor:pointer;" src="../Images/but_done.gif" runat="server" onclick="Done()" /></center>                                
                <div id="accountingCriteria" style="display:none" runat="server">
                    <span class="search_filter_title" style="font-style:italic;">Commission Selection - Accounting Commission Report</span>                     
                    <br /><br />
                    <ul class="searchFilters">
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Company:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left;">
                                <asp:DropDownList ID="dropDownACRptCompany" runat="server">
                                    <asp:ListItem Selected="True" Value="1" Text="USA - Titan Outdoor (US)" />
                                    <asp:ListItem Value="2" Text="CAN - Titan Outdoor (CAN)" />
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Line Item Start Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textACRLineItemStartDate" runat="server" />
                                <img src="../Images/calendar.gif" id="calImage1" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="textEnteredFrom_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="textACRLineItemStartDate" PopupButtonID="calImage1">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Line Item End Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textACRLineItemEndDate" runat="server" />
                                <img src="../Images/calendar.gif" id="calImage2" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" 
                                    Enabled="True" TargetControlID="textACRLineItemEndDate" PopupButtonID="calImage2">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>     
                        <li>
                            <asp:RadioButton ID="radioACRWantAllAEs" runat="server" Text="All AEs" GroupName="radioACRReportType" Checked="true" style="margin-right:10px;" />
                            <asp:RadioButton ID="radioACRWantAllActiveAEs" runat="server" Text="All Active AEs" GroupName="radioACRReportType" style="margin-right:10px;" />
                            <asp:RadioButton ID="radioACRWantAllActiveAEsWOHidden" runat="server" Text="All Active AEs w/o Hidden" GroupName="radioACRReportType" />
                        </li>                                           
                    </ul>
                    <span style="text-align:center">
                        <asp:ImageButton ID="runACRReport" runat="server" style="cursor:pointer" 
                            ImageUrl="~/Images/but_go.gif" OnClientClick="return ValidateAccounting()" 
                        onclick="runACRReport_Click" />
                    </span>
                </div>
                <div id="accountingByAeCriteria" style="display:none" runat="server">
                    <span class="search_filter_title" style="font-style:italic;">Commission Selection - Accounting Commission Report - By AE</span>                    
                    <br /><br />
                    <ul class="searchFilters">
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Company:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left;">
                                <asp:DropDownList ID="dropDownACByAERptCompany" runat="server" onchange="onCompanyDdlChange(this);">
                                    <asp:ListItem Selected="True" Value="1" Text="USA - Titan Outdoor (US)" />
                                    <asp:ListItem Value="2" Text="CAN - Titan Outdoor (CAN)" />
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Line Item Start Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textACRAELineItemStartDate" runat="server" />
                                <img src="../Images/calendar.gif" id="calImage3" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="CalendarExtender2" runat="server" 
                                    Enabled="True" TargetControlID="textACRAELineItemStartDate" PopupButtonID="calImage3">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Line Item End Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textACRAELineItemEndDate" runat="server" />
                                <img src="../Images/calendar.gif" id="calImage4" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="CalendarExtender3" runat="server" 
                                    Enabled="True" TargetControlID="textACRAELineItemEndDate" PopupButtonID="calImage4">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>    
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">AE:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:ListBox ID="listACRAE" runat="server" Rows="10"  
                                    AppendDataBoundItems="true" SelectionMode="Multiple"
                                    DataSourceID="aeDataSource" DataTextField="ACCOUNT_EXECUTIVE_NAME"
                                    DataValueField="ACTIVE_AE_ID">
                                    <asp:ListItem Text=" * ALL" Value="*" Selected="True" />
                                    <asp:ListItem Text=" * ALL Active" Value="AA" />
                                    <asp:ListItem Text=" * ALL Active (w/o Hidden)" Value="AAH" />
                                </asp:ListBox>
                                <asp:ListBox ID="listACRAECan" runat="server" Rows="10"
                                    AppendDataBoundItems="true" SelectionMode="Multiple"
                                    DataSourceID="aeDataSourceCan" DataTextField="ACCOUNT_EXECUTIVE_NAME"
                                    DataValueField="ACTIVE_AE_ID" style="display:none">
                                    <asp:ListItem Text=" * ALL" Value="*" Selected="True" />
                                    <asp:ListItem Text=" * ALL Active" Value="AA" />
                                    <asp:ListItem Text=" * ALL Active (w/o Hidden)" Value="AAH" />
                                </asp:ListBox>                                                        
                            </div>
                            <div style="clear:both"></div>
                        </li>
                    </ul>
                    <span style="text-align:center">
                        <asp:ImageButton ID="runARCAEReport" runat="server" style="cursor:pointer" 
                            ImageUrl="~/Images/but_go.gif" OnClientClick="return ValidateAccountingByAE()" 
                        onclick="runARCAEReport_Click" />
                    </span>
                </div>
                <div id="commissionFlashCriteria" style="display:none" runat="server">
                    <span class="search_filter_title" style="font-style:italic;">Commission Selection - Flash Commission Report</span>                    
                    <br /><br />
                    <ul class="searchFilters">
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Company:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left;">
                                <asp:DropDownList ID="dropDownFlashRptCompany" runat="server" onchange="onCompanyDdlChange(this);">
                                    <asp:ListItem Selected="True" Value="1" Text="USA - Titan Outdoor (US)" />
                                    <asp:ListItem Value="2" Text="CAN - Titan Outdoor (CAN)" />
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">From Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textFlashFromDate" runat="server" />
                                <img src="../Images/calendar.gif" id="calImage5" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="CalendarExtender4" runat="server" 
                                    Enabled="True" TargetControlID="textFlashFromDate" PopupButtonID="calImage5">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Thru Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textFlashThruDate" runat="server" />
                                <img src="../Images/calendar.gif" id="calImage6" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="CalendarExtender5" runat="server" 
                                    Enabled="True" TargetControlID="textFlashThruDate" PopupButtonID="calImage6">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <asp:RadioButton ID="radioFlashByInvoice" runat="server" GroupName="radioGroupFlashType" Checked="true" Text="Use Invoice Creation Date" onchange="onFlashTypeChange();" />
                            <asp:RadioButton ID="radioFlashByContract" runat="server" GroupName="radioGroupFlashType" Checked="false" Text="Use Contract Entry Date" onchange="onFlashTypeChange();" />
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">AE:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:ListBox ID="listFlashAE" runat="server" Rows="10" 
                                    AppendDataBoundItems="true" SelectionMode="Multiple" 
                                    DataSourceID="aeDataSource" DataTextField="ACCOUNT_EXECUTIVE_NAME" 
                                    DataValueField="ACTIVE_AE_ID">
                                    <asp:ListItem Text=" * ALL" Value="*" Selected="True" />
                                    <asp:ListItem Text=" * ALL Active" Value="AA" />
                                    <asp:ListItem Text=" * ALL Active (w/o Hidden)" Value="AAH" />
                                </asp:ListBox>
                                <asp:ListBox ID="listFlashAECan" runat="server" Rows="10"
                                    AppendDataBoundItems="true" SelectionMode="Multiple"
                                    DataSourceID="aeDataSourceCan" DataTextField="ACCOUNT_EXECUTIVE_NAME"
                                    DataValueField="ACTIVE_AE_ID" style="display:none">
                                    <asp:ListItem Text=" * ALL" Value="*" Selected="True" />
                                    <asp:ListItem Text=" * ALL Active" Value="AA" />
                                    <asp:ListItem Text=" * ALL Active (w/o Hidden)" Value="AAH" />
                                </asp:ListBox> 
                            </div>
                            <div style="clear:both"></div>
                        </li>
                    </ul>
                    <span style="text-align:center">
                        <asp:ImageButton ID="runFlashReport" runat="server" style="cursor:pointer" 
                            ImageUrl="~/Images/but_go.gif" OnClientClick="return ValidateFlash()" 
                        onclick="runFlashReport_Click" />
                    </span>         
                </div>  
                <div id="commissionAnalysisCriteria" style="display:none" runat="server">
                    <span class="search_filter_title" style="font-style:italic;">Commission Selection - Commission Analysis Report</span>                    
                    <br /><br />
                    <ul class="searchFilters">
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Company:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left;">
                                <asp:DropDownList ID="dropDownCommAnlRptCompany" runat="server">
                                    <asp:ListItem Selected="True" Value="1" Text="USA - Titan Outdoor (US)" />
                                    <asp:ListItem Value="2" Text="CAN - Titan Outdoor (CAN)" />
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Run As-Of Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textAsOfDate" runat="server" />
                                <img src="../Images/calendar.gif" id="Img1" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="textAsOfDate_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="textAsOfDate" PopupButtonID="calImage5">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <asp:RadioButton ID="radioCommAnalysisAllAEs" runat="server" Text="All AEs" GroupName="radioCommAnalysisReportType" Checked="true" style="margin-right:10px;" />
                            <asp:RadioButton ID="radioCommAnalysisAllActiveAEs" runat="server" Text="All Active AEs" GroupName="radioCommAnalysisReportType" style="margin-right:10px;" />
                            <asp:RadioButton ID="radioCommAnalysisAllActiveAEsWOHidden" runat="server" Text="All Active AEs w/o Hidden" GroupName="radioCommAnalysisReportType" />
                        </li>
                    </ul>
                    <span style="text-align:center">
                        <asp:ImageButton ID="runAnalysisReport" runat="server" style="cursor:pointer" 
                            ImageUrl="~/Images/but_go.gif" 
                        OnClientClick="return ValidateAnalysis();" onclick="runAnalysisReport_Click" />
                    </span>         
                </div>  
                <div id="aeCommissionByMarket" style="display:none" runat="server">
                    <span class="search_filter_title" style="font-style:italic;">Commission Selection - AE Commission By Market</span>                    
                    <br /><br />
                    <ul class="searchFilters">
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Company:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left;">
                                <asp:DropDownList ID="dropDownAECommByMktRptCompany" runat="server">
                                    <asp:ListItem Selected="True" Value="1" Text="USA - Titan Outdoor (US)" />
                                    <asp:ListItem Value="2" Text="CAN - Titan Outdoor (CAN)" />
                                </asp:DropDownList>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <div class="leftColumnNoColor" style="text-align:right">Run As-Of Date:&nbsp;&nbsp;</div>
                            <div class="rightColumn" style="text-align:left">
                                <asp:TextBox ID="textCommissionByMarketAsOFDate" runat="server" />
                                <img src="../Images/calendar.gif" id="Img2" runat="server" alt="Click for Calendar" class="calendarPopup" />                                            
                                <ajax:CalendarExtender ID="CalendarExtender6" runat="server" 
                                    Enabled="True" TargetControlID="textCommissionByMarketAsOFDate" PopupButtonID="calImage5">
                                </ajax:CalendarExtender>
                            </div>
                            <div style="clear:both"></div>
                        </li>
                        <li>
                            <asp:RadioButton ID="radioCommByMarketAllAEs" runat="server" Text="All AEs" GroupName="radioCommByMarketReportType" Checked="true" style="margin-right:10px;" />
                            <asp:RadioButton ID="radioCommByMarketAllActiveAEs" runat="server" Text="All Active AEs" GroupName="radioCommByMarketReportType" style="margin-right:10px;" />
                            <asp:RadioButton ID="radioCommByMarketAllActiveAEsWOHidden" runat="server" Text="All Active AEs w/o Hidden" GroupName="radioCommByMarketReportType" />
                        </li>
                    </ul>
                    <span style="text-align:center">
                        <asp:ImageButton ID="runCommissionByMarketReport" runat="server" style="cursor:pointer" 
                            ImageUrl="~/Images/but_go.gif" 
                        OnClientClick="return ValidateCommissionByMarket();" onclick="runCommissionByMarketReport_Click" />
                    </span>         
                </div>  
                <div id="processCommissions" style="display:none" runat="server">
                    <span class="search_filter_title" style="font-style:italic;">Process Commissions</span>                    
                    <br /><br />
                    <span style="text-align:center">
                        <asp:ImageButton ID="runProcessCommissions" runat="server" style="cursor:pointer" 
                            ImageUrl="~/Images/but_go.gif" 
                        OnClientClick="return ValidateProcessCommissions();" 
                        onclick="runProcessCommissions_Click" />
                    </span>       
                    <br />
                    <br />                                        
                    <span style="text-align:center">
                        <asp:CheckBox ID="checkProcessCommissions" runat="server" />
                        &nbsp;&nbsp;Process Commissions For:&nbsp;&nbsp;
                        <asp:DropDownList ID="dropDownProcessYear" runat="server" Width="100px" />                                                                                
                    </span>                                        
                </div>
                <br /><br />
                <div id="commissionProcessInfo" runat="server" style="display:none">
                    Processing Status:&nbsp;&nbsp;&nbsp;<asp:Label ID="labelProcessingStatus" runat="server" /><br />
                    Year Processed:&nbsp;&nbsp;&nbsp;<asp:Label ID="labelProcessYear" runat="server" /><br />                        
                    Last Process Time:&nbsp;&nbsp;&nbsp;<asp:Label ID="labelProcessTime" runat="server" /><br />
                </div>
                <asp:SqlDataSource ID="aeDataSource" runat="server" DataSourceMode="DataSet" SelectCommandType="StoredProcedure" 
                    SelectCommand="GetAEs" FilterExpression="COMPANY_ID = {0}">
                    <FilterParameters>
                        <asp:Parameter Name="COMPANY_ID" DefaultValue="1" DbType="Int32" />
                    </FilterParameters>                    
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="aeDataSourceCan" runat="server" DataSourceMode="DataSet" SelectCommandType="StoredProcedure" 
                    SelectCommand="GetAEs" FilterExpression="COMPANY_ID = {0}">
                    <FilterParameters>
                        <asp:Parameter Name="COMPANY_ID" DefaultValue="2" DbType="Int32" />
                    </FilterParameters>                    
                </asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>        
</asp:Content>

