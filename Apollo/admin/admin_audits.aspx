<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchVertSplit.master" AutoEventWireup="true" CodeFile="admin_audits.aspx.cs" Inherits="Apollo.admin_admin_audits" Title="Audits | Titan 360" %>
<asp:Content ID="Content1" ContentPlaceHolderID="searchCriteriaPanel" Runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            var filterObject = BuildAuditFilterObject('','');
            DisplayStaticGrid('audits','audits','auditPager',filterObject,OnItemSelected)
        });
        function SearchGrid(){
            if (!ValidateDates()){return;}
            var auditFrom = $get('<%=textAuditFrom.ClientID %>').value;
            var auditTo = $get('<%=textAuditTo.ClientID %>').value;
            var filterObject = BuildAuditFilterObject(auditFrom,auditTo);            
            var postParams = getPostParams('audits',filterObject);
            $('#audits').setGridParam({postData:postParams}).trigger('reloadGrid');            
        }
        function OnItemSelected(){
            var rowData = $('#audits').getRowData(id);                
        }
        function ValidateDates(){
            var auditFrom = $get('<%=textAuditFrom.ClientID %>').value;
            var auditTo = $get('<%=textAuditTo.ClientID %>').value;
            if (auditFrom != "") {                
                if (!IsValidDate(auditFrom)) {
                    alert('Audit From Date is not in a valid format.'); 
                    return false;
                }
            }
            if (auditTo != "") {
                if (!IsValidDate(auditTo)) {
                    alert('Audit To Date is not in a valid format.'); 
                    return false;
                }
            }
            if (Date(auditTo) < Date(auditFrom)){
                alert ('Audit To Date can not be earlier than Audit From Date.');
                return false;
            }   
            return true;
        }
        function ClearFilters(){
            $get('<%=textAuditFrom.ClientID %>').value = "";
            $get('<%=textAuditTo.ClientID %>').value = "";                        
            var filterObject = BuildAuditFilterObject('','');
            var postParams = getPostParams('audits',filterObject);
            $('#audits').setGridParam({postData:postParams}).trigger('reloadGrid');            
        }
    </script>
    <div class="inventory_search">SEARCH FILTERS</div>
    <div id="searchCriteriaArea">
        <ul class="searchFilters">            
            <li>
                <div>
                    <div class="spanColumnNoColor" style="font-weight:bold">Audit Date</div>
                </div>
            </li>
            <li>   
                <div>
                    <div style="float:left;">
                        From: <asp:TextBox ID="textAuditFrom" runat="server" Width="72px" style="text-align:right" />
                        <img src="../Images/calendar.gif" id="calImage5" runat="server" alt="Click for Calendar" class="calendarPopup" />
                        <ajax:CalendarExtender ID="textAuditFrom_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="textAuditFrom" PopupButtonID="calImage5" />                                
                    </div>
                    <div style="float:left;padding-left:11px">
                        To: <asp:TextBox ID="textAuditTo" runat="server" Width="72px" style="text-align:right" />
                        <img src="../Images/calendar.gif" id="calImage6" runat="server" alt="Click for Calendar" class="calendarPopup" />
                        <ajax:CalendarExtender ID="textAuditTo_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="textAuditTo" PopupButtonID="calImage6" />                                
                    </div>
                    <div style="clear:both"></div>
                </div> 
            </li>            
            <li>     
                <div style="text-align:right;width:100%">                    
                    <img id="clear" runat="server" style="cursor:pointer" src="~/Images/but_clear.gif" alt="Clear Search Filters" onclick="ClearFilters();" />
                    <img id="search" runat="server" style="cursor:pointer" src="~/Images/but_search.gif" alt="Search" onclick="SearchGrid();" />                                                            
                </div>
            </li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchResultsPanel" Runat="Server">
    <div id="errorDiv" class="errorDisplay" style="margin:3px 0;" runat="server"></div>
    <span class="search_filter_title" id="dlPanelTitle">Audits</span>
    <div style="margin:10px auto;">
        <div style="width:95%;margin:20px;"><table id="audits"></table></div>
        <div id="auditPager"></div>
    </div>
</asp:Content>

