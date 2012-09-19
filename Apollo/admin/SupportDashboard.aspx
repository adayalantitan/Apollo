<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/ApolloSearchVertSplit.master" AutoEventWireup="true" CodeFile="SupportDashboard.aspx.cs" Inherits="Apollo.admin_SupportDashboard" Trace="true" TraceMode="SortByCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="searchCriteriaPanel" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="searchResultsPanel" Runat="Server">
    <script type="text/javascript" language="javascript">
        function confirmDelete() {
            return confirm('Are you sure you wish to delete the contents of the Status Log?');
        }
        function RefreshGrid() { $('#list1').trigger('reloadGrid'); }
        $(document).ready(function() {
            var lastsel;
            $("#list1").jqGrid({
                url: '../services/IOService.asmx/GetStatusGrid'
                , datatype: "xml"
                , colNames: ['Status Timestamp', 'Status Info']
                , colModel: [{ name: 'statusTimestamp', index: 'DEBUG_MESSAGE_TIMESTAMP', width: 125, search: false, sort: true }
                    , { name: 'statusInfo', index: 'DEBUG_MESSAGES', width: 525, search: false, sort: true}]
                , rowNum: 25
                , height: 500
                , width: 800
                , rowList: [25, 50, 100]
                , pager: '#pager1'
                , sortname: 'DEBUG_MESSAGE_TIMESTAMP'
                , sortorder: 'DESC'
                , viewrecords: true
                , caption: "Status Info"
            });
            $("#list1").jqGrid('navGrid', "#pager1", { edit: false, add: false, del: false });
        });
    </script>
    <div style="margin:20px">    
        <asp:UpdatePanel ID="exceptionUpdPnl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>        
                <br />                           
                <div>                    
                    <div style="float:left;margin-left:10px">
                        <input type="button" id="home" style="font-size:10px" value="Home" onclick="javascript:window.location='/Default.aspx'" />
                    </div>
                    <div style="float:left;margin-left:10px">
                        <asp:Button id="flushLog" runat="server" Text="Flush Log Contents"
                            style="font-size:10px" onclick="flushLog_Click" OnClientClick="return confirmDelete();" />
                    </div>
                    <div style="clear:both"></div>
                </div>                
                <br />
                <div>
                    <asp:Label ID="labelStatus" runat="server" />
                </div>     
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="margin:0 auto;" id="commissionsGrid">
            <div style="width:95%;margin-left:10px;"><table id="list1"></table></div>
            <div id="pager1"></div>
        </div>
    </div>
</asp:Content>

