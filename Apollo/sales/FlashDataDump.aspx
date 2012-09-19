<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="FlashDataDump.aspx.cs" Inherits="Apollo.sales_FlashDataDump" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="margin-left:50px;">
        <div>
            <div style="float:left;">
                Start Year:
                <asp:DropDownList ID="dropDownStartYear" runat="server">
                    <asp:ListItem Value="2002" Text="2002" />
                    <asp:ListItem Value="2003" Text="2003" />
                    <asp:ListItem Value="2004" Text="2004" />
                    <asp:ListItem Value="2005" Text="2005" />
                    <asp:ListItem Value="2006" Text="2006" />
                    <asp:ListItem Value="2007" Text="2007" />
                    <asp:ListItem Value="2008" Text="2008" />
                    <asp:ListItem Value="2009" Text="2009" />
                    <asp:ListItem Value="2010" Text="2010" />
                    <asp:ListItem Value="2011" Text="2011" />
                    <asp:ListItem Value="2012" Text="2012" />
                    <asp:ListItem Value="2013" Text="2013" />
                    <asp:ListItem Value="2014" Text="2014" />
                    <asp:ListItem Value="2015" Text="2015" />
                    <asp:ListItem Value="2016" Text="2016" />
                    <asp:ListItem Value="2017" Text="2017" />
                </asp:DropDownList>
            </div>
            <div style="float:left;margin-left:10px;">
                <asp:DropDownList ID="dropDownCompany" runat="server">
                    <asp:ListItem Value="1" Text="Titan US" Selected="True" />
                    <asp:ListItem Value="2" Text="Titan Canada" />
                </asp:DropDownList>
            </div>
            <div style="clear:both;"></div>
        </div>
        <div style="margin-top:20px;">
            <div style="float:left;">
                <asp:Button ID="exportBillingDataDump" Text="Export Data Dump (Billings)" 
                    runat="server" onclick="exportBillingDataDump_Click" />
            </div>
            <div style="float:left;margin-left:20px;">
                <asp:Button ID="exportRevenueDataDump" Text="Export Data Dump (Revenue)" 
                    runat="server" onclick="exportRevenueDataDump_Click" />
            </div>
        </div>
    </div>
</asp:Content>

