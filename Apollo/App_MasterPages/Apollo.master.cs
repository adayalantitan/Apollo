#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class Apollo : System.Web.UI.MasterPage
    {

        #region GenerateMenuHTML method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string GenerateMenuHTML()
        {
            StringBuilder menu = new StringBuilder();
            //Base menu - Available to All Users
            menu.Append(@"<ul class=""sf-menu"" style=""width:100% !important;background-color:#EAEAEA !important;margin:0 auto !important;"">");
            menu.Append(@"<li><a href=""/Default.aspx"">Home</a></li><li><a href=""/digital/digital_library.aspx"">Digital Library</a><ul><li><a href=""/digital/digital_library.aspx"">Digital Library</a>");
            //Digital Library Drag 'N Drop
            if (Security.IsAdminUser() || Security.IsDigitalUser())
            {
                menu.Append(@"<ul><li><a href=""/digital/digitalLibraryUpload.aspx"">Drag 'N Drop Tagging</a></li></ul>");
            }
            menu.Append(@"</li><li><a href=""/digital/station_images.aspx"">Station Photos</a></li></ul></li>");
            //Sales Menu
            menu.Append(@"<li><a href=""/Sales/sales_flash.aspx"">Sales</a><ul>");
            menu.Append(@"<li><a href=""/Sales/sales_flash.aspx"">Flash (by Billings)</a></li>");
            menu.Append(@"<li><a href=""/sales/sales_revenue_flash.aspx"">Revenue Flash</a></li>");
            if (Security.IsAgedRevenueFlashUser())
            {
                menu.Append(@"<li><a href=""/sales/sales_aged_revenue_flash.aspx"">Aged Revenue Flash</a></li>");
            }
            if (Security.IsCorporateUser() || Security.IsAdminUser())
            {//Rest of Sales Menu
                menu.Append(@"<li><a href=""/Sales/sales_commissions.aspx"">Commissions</a><ul><li><a href=""/Sales/sales_commissions.aspx"">Splits</a></li><li><a href=""/Sales/production/production_overrides.aspx"">Production Overrides</a></li><li><a href=""/Sales/sales_reports.aspx"">Reports</a></li></ul></li>");
                menu.Append(@"<li><a href=""/Sales/master_data/rates.aspx"">Master Data</a><ul><li><a href=""/Sales/master_data/aeDrawsPaymentsFlatRates.aspx"">AE Draws / Payments</a></li><li><a href=""/Sales/master_data/profit_center_rates.aspx"">Profit Center Rates</a></li><li><a href=""/Sales/master_data/rates.aspx"">Rates</a></li></ul></li>");
            }                
            menu.Append(@"</ul></li>");
            //Collections Menu
            if (Security.IsCollectionsUser())
            {
                menu.Append(@"<li><a href=""/collections/creditCheckingDashboard.aspx"">Collections</a><ul><li><a href=""/collections/creditCheckingDashboard.aspx"">Credit Checking Dashboard</a></li>");
                menu.Append(@"<li><a href=""/collections/mtaInvoiceNotes.aspx"">MTA Invoice Notes</a></li></ul></li>");
            }
            //Production Menu
            menu.Append(@"<li><a href=""/production/avails.aspx"">Production</a><ul><li><a href=""/production/avails.aspx"">Station Dom. Avails</a></li><li><a href=""/digitalAvails"">Digital Avails</a></li></ul></li>");
            //Quattro Menu
            menu.Append(@"<li><a href=""/quattro/Documents.aspx"">Quattro</a><ul><li><a href=""/quattro/Documents.aspx"">Documentation</a></li>");
            menu.Append(@"<li><a href=""/quattro/attachmentDownload.aspx"">Attachment Download</a></li>");
            menu.Append(@"<li><a href=""/quattro/uninvoiced_contract_breakdown.aspx"">Reports</a><ul><li><a href=""/quattro/uninvoiced_contract_breakdown.aspx"">Uninvoiced Contract Report</a></li>");
            menu.Append(@"<li><a href=""/quattro/uninvoiced_segment_breakdown.aspx"">Segments w/o Space Reservation</a></li>");			
            menu.Append(@"<li><a href=""/quattro/ccn_report.aspx"">Contracts w/CCN Attachment</a></li>");
			menu.Append(@"<li><a href=""/collections/billingCreditsPaymentsByAccount.aspx"">Billings, Credits and Payments</a></li>");
            menu.Append(@"<li><a href=""/quattro/postingSummaryByContract.aspx"">Contract Posting Summary</a></li>");
            menu.Append(@"</ul></li>");            
            menu.Append(@"<li><a href=""/quattro/invoiceReprint.aspx"">Invoice Reprint</a></li>");            
            if (Security.IsAdminUser())
            {
                menu.Append(@"<li><a href=""/quattro/contract_audit_details.aspx"">Contract Audit Details</a></li>");
            }
            menu.Append(@"</ul></li>");
            if (Security.IsAdminUser())
            {
                //Admin Menu
                menu.Append(@"<li><a href=""/admin/Default.aspx"">Admin</a><ul><li><a href=""/admin/admin_audits.aspx"">Audit Log</a></li><li><a href=""/admin/admin_ae_reporting_maintenance.aspx"">AE Reporting Maintenance</a></li><li><a href=""/admin/admin_cust_maintenance.aspx"">Customer Maintenance</a></li></ul></li>");
                //Support Menu
                menu.Append(@"<li><a href=""/admin/ExceptionViewer.aspx"">Support</a><ul><li><a href=""/admin/ExceptionViewer.aspx"">Exception Viewer</a></li><li><a href=""/admin/SupportDashboard.aspx"">Support Dashboard</a></li><li><a href=""/admin/dataCheck.aspx"">Data Consistency Checks</a></li></ul></li>");
            }
            //end menu
            menu.Append("</ul>");
            return menu.ToString();
        }
        #endregion

        #region HideHeader method
        /// <summary>TBD</summary>
        public void HideHeader()
        {
            headerArea.Visible = false;
        }
        #endregion

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                menuBar.InnerHtml = GenerateMenuHTML();
            }
        }
        #endregion

    }

}
