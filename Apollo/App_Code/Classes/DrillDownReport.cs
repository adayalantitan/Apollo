#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public class CustomerSalesReport : DrillDownReport
    {

        #region Member variables
        /// <summary>TBD</summary>
        private int compareToYear;
        /// <summary>TBD</summary>
        private int reportYear;
        #endregion

        #region Parameterized constructor (reportData, sumFields, market, companyId, reportYear, compareToYear)
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <param name="sumFields">TBD</param>
        /// <param name="market">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="reportYear">TBD</param>
        /// <param name="compareToYear">TBD</param>
        public CustomerSalesReport(DataSet reportData, string[] sumFields, string market, string companyId, int reportYear, int compareToYear)
            : base(reportData, sumFields, market, companyId)
        {
            this.reportYear = reportYear;
            this.compareToYear = compareToYear;
        }
        #endregion

        #region BuildExcelReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildExcelReportTableHeaders(bool showContracts)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.AppendLine(@"<table cellspacing=""1"" cellpadding=""0"" border=""1"" align=""center"">");
            recordDisplay.AppendFormat(@"     <tr><td colspan=""7"" height=""55"" align=""center""><img src=""http://{0}/Images/flash/titan_report_logo.gif"" alt=""Titan 360"" /></td></tr>", HttpContext.Current.Request.Url.Host);
            recordDisplay.AppendLine(@"     <tr><td colspan=""7"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>Advertiser Comparison Report</b></td></tr>");
            recordDisplay.AppendLine(@"     <tr nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_);"">");
            if (showContracts)
            {
                recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Advertiser</b></td>");
                recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>AE</b></td>");
                recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Contract Entry Date</b></td>");
                recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Contract</b></td>");
            }
            else
            {
                recordDisplay.AppendLine(@"         <td colspan=""4"">&nbsp;</td>");
            }
            recordDisplay.AppendFormat(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0} Total</b></td>", Convert.ToString(this.reportYear));
            recordDisplay.AppendFormat(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0} Total</b></td>", Convert.ToString(this.compareToYear));
            recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Variance</b></td>");
            recordDisplay.AppendLine(@"     </tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="tableCount">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildReportTableHeaders(int tableCount)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.Append("");
            recordDisplay.Append(@"<tr>");
            recordDisplay.Append(@"<td width=""43%""><img src=""/Images/flash/spacer.gif"" width=""192"" height=""0""></td>");
            recordDisplay.Append(@"<td width=""18%"" align=""right""><img src=""../Images/flash/spacer.gif"" width=""61"" height=""0""></td>");
            recordDisplay.Append(@"<td width=""18%"" align=""right""><img src=""../Images/flash/spacer.gif"" width=""61"" height=""0""></td>");
            recordDisplay.Append(@"<td width=""21%"" align=""right""><img src=""../Images/flash/spacer.gif"" width=""67"" height=""0""></td>");
            recordDisplay.Append(@"</tr><tr bgcolor=""#ffffff""><td colspan=""6"">&nbsp;</td></tr>");
            recordDisplay.Append(@"<tr bgcolor=""#ffffff""><td></td>");
            recordDisplay.Append(@"     <td colspan='4' style=""text-align:center;"">");
            recordDisplay.Append(@"         <table width=""100%"" cellpadding='0' cellspacing='0' border='0' bgcolor=""#ffffff"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_left_corner.gif""></td>");
            recordDisplay.AppendFormat(@"                 <td class=""report_heading"" nowrap background=""/Images/flash/blue_thick_bar.gif"">&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", (String.IsNullOrEmpty(this.marketName) ? string.Format("All Markets {0}", salesType) : this.marketName));
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td></td>");
            recordDisplay.Append(@"     <td colspan='4' style=""text-align:center;"">");
            recordDisplay.Append(@"         <table width='100%' cellpadding='0' cellspacing='0' border='0'>");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_corner.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr>");
            recordDisplay.Append(@"     <td bgcolor=""#ffffff"" style=""color:#666699;font-weight:bold;text-align:left;"">");
            recordDisplay.AppendFormat(@"         <img src=""/Images/but_excel.gif"" border=0 style=""cursor:pointer;"" onclick=""openexcel({0});"">", tableCount);
            recordDisplay.Append(@"         <br/>");
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""expandAll('{0}');"">Expand All</a>", salesTypeId);
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""collapseAll('{0}');"">Collapse All</a>", salesTypeId);
            recordDisplay.Append(@"    </td>");
            recordDisplay.AppendFormat(@"    <td class=""dashboard_column_head"">{0} Total</td>", Convert.ToString(this.reportYear));
            recordDisplay.AppendFormat(@"    <td class=""dashboard_column_head"">{0} Total</td>", Convert.ToString(this.compareToYear));
            recordDisplay.AppendFormat(@"    <td class=""dashboard_column_head"">Variance</td>", "");
            recordDisplay.AppendLine(@"</tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildSubTableRow method
        /// <summary>TBD</summary>
        /// <param name="subTable">TBD</param>
        /// <param name="divId">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildSubTableRow(StringBuilder subTable, string divId)
        {
            StringBuilder subTableRow = new StringBuilder();
            subTableRow.AppendFormat(@"<div class=""{0}"" id=""{1}"" style=""display:none"">", salesTypeId, divId);
            subTableRow.Append(@"  <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" bgcolor=""#ededed"" style=""border-collapse: collapse;border-spacing: 0px;border-width:0px;"">");
            subTableRow.Append(@"      <tr style=""height:0px;background-color:#ffffff;"">");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" width=""43%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""192""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""18%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""18%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""21%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"      </tr>");
            subTableRow.Append(subTable.ToString());
            subTableRow.Append(@"  </table>");
            subTableRow.AppendLine(@"</div>");
            return subTableRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (forExcel)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            string previousContractNumber = "";
            string currentContractNumber = "";
            string filterExpression = base.GetContractSumFilterExpression(sumFields, currentSumOnValues);
            foreach (DataRow row in this.reportDataSorted.Select(filterExpression))
            {
                currentContractNumber = (string)row["CONTRACT_NUMBER"];
                if (previousContractNumber != currentContractNumber)
                {
                    contractRow.AppendLine(GenerateContractRow(row, forExcel));
                }
                previousContractNumber = currentContractNumber;
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (row, forExcel)
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(DataRow row, bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            if (!forExcel)
            {
                contractRow.AppendFormat(@"<tr class=""contract_item_row"" align=""right"" style=""cursor:pointer;background-color:#ffffff;color:#2f4070"" ondblclick=""ShowContractDetail({0},{1});"" onclick=""ShowContractDetail({0},{1});"" onmouseover=""mouseincolor(this);"" onmouseout=""mouseoutcolor(this);"">", Convert.ToString(row["CONTRACT_NUMBER"]), this.companyId);
                contractRow.AppendFormat(@"     <td class=""contract_item_row_text"">{0} - {1}&nbsp;</td>", ((DateTime)row["CONTRACT_ENTRY_DATE"]).ToShortDateString(), Convert.ToString(row["CONTRACT_NUMBER"]));
                contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Report Year Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Compare-To Year Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Variance"]), false, false));
                contractRow.AppendLine(@"</tr>");
            }
            else
            {
                contractRow.Append(@"      <tr valign=""top"">");
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (((DateTime)row["CONTRACT_ENTRY_DATE"]).ToShortDateString() as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row["CONTRACT_NUMBER"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Report Year Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Compare-To Year Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Variance"]), false, false));
                contractRow.AppendLine("       </tr>");
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateGrandTotalRow method
        /// <summary>TBD</summary>
        /// <param name="forExcel">Flag for determining if this report is being Exported to Excel</param>
        /// <returns>TBD</returns>
        public override string GenerateGrandTotalRow(bool forExcel)
        {
            StringBuilder grandTotal = new StringBuilder();
            if (!forExcel)
            {
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td class=""grand_total_text"">Grand Total&nbsp;</td>");
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Report Year Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Compare-To Year Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Variance])", ""))));
                grandTotal.AppendLine(@"</tr>");
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td>&nbsp;</td>");
                grandTotal.Append(@"    <td colspan=""5"" valign=""top"" style=""border-top:1px solid #ffffff;"">");
                grandTotal.Append(@"        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
                grandTotal.Append(@"            <tr>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_left_flip.gif""></td>");
                grandTotal.Append(@"                <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_right_corner_flip.gif""></td>");
                grandTotal.Append(@"            </tr>");
                grandTotal.Append(@"        </table>");
                grandTotal.Append(@"    </td>");
                grandTotal.AppendLine(@"</tr>");
            }
            else
            {
                grandTotal.Append(@"    <tr valign=""top"">");
                grandTotal.Append(@"        <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""right"" colspan=""2""><b>Grand Total&nbsp;</b></td>");
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Report Year Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Compare-To Year Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Variance])", ""))));
                grandTotal.Append(@"    </tr>");
                grandTotal.AppendLine(@"</table>");
            }
            return grandTotal.ToString();
        }
        #endregion

        #region GenerateLineItemRow method
        /// <summary>Generates a row in the Grouping report</summary>
        /// <param name="level">Determines which level in the Grouping this row will fall in</param>
        /// <param name="count">Used for formatting the style of the row</param>
        /// <param name="forExcel">Flag for determining if this will be exported to Excel</param>
        /// <param name="showContracts">Flag for determining if the Excel export will display contracts</param>
        /// <returns>A formatted HTML Table Row containing Contract info</returns>
        public override string GenerateLineItemRow(int level, int count, bool forExcel, bool showContracts)
        {
            StringBuilder lineItem = new StringBuilder();
            string uniqueDivId = GenerateUniqueDivId();
            string displayText = this.previousSumOnValues[level];
            string displayField = this.sumFields[level].Replace("_", " ");
            StringBuilder rowToAppend = reportBuilder[level + 1];
            string filterExpression = GetSumFilterExpression(sumFields, previousSumOnValues, level);
            if (!forExcel)
            {
                lineItem.AppendFormat(@"<tr class=""forecast_report_{0}_total{1}"">", level, ((count % 2 == 0) ? "0" : "1"));
                lineItem.AppendFormat(@"    <td class=""report_market_{0}"" onclick=""showOrhide('{1}');"" style=""border-left:0px;cursor:pointer;text-align:left;"">", level, uniqueDivId);
                lineItem.AppendFormat(@"        <img id=""{0}_img"" src=""/Images/flash/arrow_right_blue.gif"" />&nbsp;{1}", uniqueDivId, displayText);
                lineItem.Append(@"    </td>");
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Report Year Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Compare-To Year Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Variance])", filterExpression))));
                lineItem.AppendLine(@"</tr>");
                lineItem.AppendFormat(@"<tr><td colspan=""5"">{0}</td></tr>", BuildSubTableRow(rowToAppend, uniqueDivId));
            }
            else
            {
                if (level == 0)
                {
                    lineItem.Append(@"     <tr valign=""top"">");
                    lineItem.Append(@"         <td colspan=""5"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>" + displayText + "</b></td>");
                    lineItem.AppendLine(@"     </tr>");
                }
                lineItem.Append(rowToAppend.ToString());
                lineItem.Append(@"     <tr valign=""top"">");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""right"" colspan=""4""><b>" + ((showContracts) ? "TOTAL " : "") + displayField + ": " + displayText + "</b></td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Report Year Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Compare-To Year Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Variance])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.AppendLine(@"     </tr>");
            }
            return lineItem.ToString();
        }
        #endregion

    }

    /// <summary>
    /// Summary description for DrillDownReport
    /// </summary>
    public abstract class DrillDownReport
    {

        #region Member variables
        /// <summary>TBD</summary>
        protected string companyId;
        /// <summary>TBD</summary>
        protected string[] currentSumOnValues;
        /// <summary>TBD</summary>
        protected string marketName;
        /// <summary>TBD</summary>
        protected string[] previousSumOnValues;
        /// <summary>TBD</summary>
        protected StringBuilder[] reportBuilder;
        /// <summary>TBD</summary>
        protected DataSet reportData;
        /// <summary>TBD</summary>
        protected DataTable reportDataSorted;
        /// <summary>TBD</summary>
        protected StringBuilder reportTable;
        /// <summary>TBD</summary>
        protected string salesType;
        /// <summary>TBD</summary>
        protected string salesTypeId;
        /// <summary>TBD</summary>
        protected string[] sumFields;
        #endregion

        #region Parameterized constructor (reportData, sumFields, market, companyId)
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <param name="sumFields">TBD</param>
        /// <param name="market">TBD</param>
        /// <param name="companyId">TBD</param>
        public DrillDownReport(DataSet reportData, string[] sumFields, string market, string companyId)
        {
            this.reportData = reportData;
            this.sumFields = sumFields;
            this.marketName = market;
            this.companyId = companyId;
        }
        #endregion

        #region BuildDetailLineItemTable method
        /// <summary>TBD</summary>
        /// <param name="lineItemData">TBD</param>
        /// <returns>TBD</returns>
        public static string BuildDetailLineItemTable(DataSet lineItemData)
        {
            //Sanity Check
            if (lineItemData.Tables[0].Rows.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder lineItemTable = new StringBuilder();
            lineItemTable.AppendLine(@"<table cellspacing=""0"" cellpadding=""0"" width=""100%"" border=""0"">");
            lineItemTable.AppendLine(@"     <tr><td nowrap class=""infoBoxContentDark"" colspan=""10""><b>Media Line Items</b></td></tr>");
            lineItemTable.AppendLine(@"     <tr><td nowrap class=""buttonHead"" width=""20%"">Market</td>");
            lineItemTable.AppendLine(@"         <td nowrap class=""buttonHead_Center"" width=""20%"">Profit Center</td>");
            lineItemTable.AppendLine(@"         <td nowrap class=""buttonHead_Center"" width=""20%"">Media Product</td>");
            lineItemTable.AppendLine(@"         <td nowrap class=""buttonHead_Right"" width=""11%"">Start Date</td>");
            lineItemTable.AppendLine(@"         <td nowrap class=""buttonHead_Right"" width=""11%"">End Date</td>");
            lineItemTable.AppendLine(@"         <td nowrap class=""buttonHead_Right"" width=""7%"">Quantity</td>");
            lineItemTable.AppendLine(@"         <td nowrap class=""buttonHead_Right"" width=""11%"">Amount</td></tr>");
            string previousPanelType = "";
            int count = 0;
            foreach (DataRow row in lineItemData.Tables[0].Rows)
            {
                //Generate Line Item table
                if (previousPanelType != Convert.ToString(IO.GetDataRowValue(row, "MEDIA_PRODUCT", "")))
                {
                    lineItemTable.AppendFormat(@"<tr class=""report_total_numbers"" bgcolor=""#cccccc""><td colspan=""6"" align=""left"">{0}</td><td align=""right"">&nbsp;</td></tr>", IO.GetDataRowValue(row, "MEDIA_PRODUCT", "&nbsp;"));
                }
                lineItemTable.AppendFormat(@"<tr class=""infoBoxSearch""{0}"">", ((count++ % 2 == 0) ? "0" : "1"));
                lineItemTable.AppendFormat(@"     <td nowrap width=""20%"" align=""left"">{0}</td>", IO.GetDataRowValue(row, "MARKET", "&nbsp;"));
                lineItemTable.AppendFormat(@"     <td nowrap width=""20%"" align=""center"">{0}</td>", IO.GetDataRowValue(row, "PROFIT_CENTER", "&nbsp;"));
                lineItemTable.AppendFormat(@"     <td nowrap width=""20%"" align=""center"">{0}</td>", IO.GetDataRowValue(row, "MEDIA_PRODUCT", "&nbsp;"));
                lineItemTable.AppendFormat(@"     <td nowrap width=""11%"" align=""right"">{0}</td>", (row["LINE_ITEM_START_DATE"] as string ?? "&nbsp;"));
                lineItemTable.AppendFormat(@"     <td nowrap width=""11%"" align=""right"">{0}</td>", (row["LINE_ITEM_END_DATE"] as string ?? "&nbsp;"));
                lineItemTable.AppendFormat(@"     <td nowrap width=""7%"" align=""right"">{0}</td>", Convert.ToInt32(IO.GetDataRowValue(row, "QUANTITY", "&nbsp;")));
                lineItemTable.AppendFormat(@"     <td nowrap width=""11%"" align=""right"">{0}</td>", DoubleDecimalDisplay(Convert.ToDouble(IO.GetDataRowValue(row, "AMOUNT", "&nbsp;"))));
                lineItemTable.AppendLine(@"</tr>");
                previousPanelType = Convert.ToString(IO.GetDataRowValue(row, "MEDIA_PRODUCT", ""));
            }
            lineItemTable.AppendLine(@"<tr class=""buttonHead""><td colspan=""6"" align=""right"">Total Amount&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>");
            lineItemTable.AppendFormat(@"<td nowrap align=""right"">{0}</td></tr>", DoubleDecimalDisplay(Convert.ToDouble(lineItemData.Tables[0].Compute("Sum([AMOUNT])", ""))));
            return lineItemTable.ToString();
        }
        #endregion

        #region BuildDetailTransactionsTable method
        /// <summary>TBD</summary>
        /// <param name="transactionData">TBD</param>
        /// <returns>TBD</returns>
        public static string BuildDetailTransactionsTable(DataSet transactionData)
        {
            if (transactionData.Tables[1].Rows.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder transactionsTable = new StringBuilder();
            transactionsTable.AppendLine(@"<table cellspacing=""0"" cellpadding=""0"" width=""100%"" border=""0"">");
            transactionsTable.AppendLine(@"     <tr><td nowrap class=""infoBoxContentDark"" colspan=""10""><b>Contract Transactions</b></td></tr>");
            transactionsTable.AppendLine(@"     <tr><td nowrap class=""buttonHead"" width=""20%"">Market</td>");
            transactionsTable.AppendLine(@"         <td nowrap class=""buttonHead"" width=""20%"">Profit Center</td>");
            transactionsTable.AppendLine(@"         <td nowrap class=""buttonHead"" width=""20%"">Media Product</td>");
            transactionsTable.AppendLine(@"         <td nowrap class=""buttonHead"" width=""10%"">Type</td>");
            transactionsTable.AppendLine(@"         <td nowrap class=""buttonHead_Center"" width=""10%"">Document Number</td>");
            transactionsTable.AppendLine(@"         <td nowrap class=""buttonHead_Right"" width=""10%"">Date</td>");
            transactionsTable.AppendLine(@"         <td nowrap class=""buttonHead_Right"" width=""10%"">Amount</td></tr>");
            string previousPanelType = "";
            int count = 0;
            foreach (DataRow row in transactionData.Tables[1].Rows)
            {
                //Generate Line Item table
                if (previousPanelType != Convert.ToString(IO.GetDataRowValue(row, "MEDIA_PRODUCT", "")))
                {
                    transactionsTable.AppendFormat(@"<tr class=""report_total_numbers"" bgcolor=""#cccccc""><td colspan=""6"" align=""left"">{0}</td><td align=""right""></td></tr>", IO.GetDataRowValue(row, "MEDIA_PRODUCT", "&nbsp;"));
                }
                transactionsTable.AppendFormat(@"<tr class=""infoBoxSearch""{0}"">", ((count++ % 2 == 0) ? "0" : "1"));
                transactionsTable.AppendFormat(@"     <td nowrap width=""20%"" align=""left"">{0}</td>", IO.GetDataRowValue(row, "MARKET", "&nbsp;"));
                transactionsTable.AppendFormat(@"     <td nowrap width=""20%"" align=""left"">{0}</td>", IO.GetDataRowValue(row, "PROFIT_CENTER", "&nbsp;"));
                transactionsTable.AppendFormat(@"     <td nowrap width=""20%"" align=""left"">{0}</td>", IO.GetDataRowValue(row, "MEDIA_PRODUCT", "&nbsp;"));
                transactionsTable.AppendFormat(@"     <td nowrap width=""10%"" align=""left"">{0}</td>", (row["TYPE"] as string ?? "&nbsp;"));
                transactionsTable.AppendFormat(@"     <td nowrap width=""10%"" align=""center"">{0}</td>", IO.GetDataRowValue(row, "INVOICE_NUMBER", "&nbsp;"));
                transactionsTable.AppendFormat(@"     <td nowrap width=""10%"" align=""right"">{0}</td>", (row["DATE"] as string ?? "&nbsp;"));
                transactionsTable.AppendFormat(@"     <td nowrap width=""10%"" align=""right"">{0}</td>", DoubleDecimalDisplay(Convert.ToDouble(IO.GetDataRowValue(row, "AMOUNT", "&nbsp;"))));
                transactionsTable.AppendLine(@"</tr>");
                previousPanelType = Convert.ToString(IO.GetDataRowValue(row, "MEDIA_PRODUCT", ""));
            }
            transactionsTable.AppendLine(@"<tr class=""buttonHead""><td colspan=""6"" align=""right"">Total Amount&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>");
            transactionsTable.AppendFormat(@"<td nowrap align=""right"">{0}</td></tr>", DoubleDecimalDisplay(Convert.ToDouble(transactionData.Tables[1].Compute("Sum([AMOUNT])", ""))));
            return transactionsTable.ToString();
        }
        #endregion

        #region BuildExcelReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public virtual string BuildExcelReportTableHeaders(bool showContracts)
        {
            throw new NotImplementedException("BuildExcelReportTableHeaders(bool showContracts) not implemented by Subclass");
        }
        #endregion

        #region BuildReportTable method (forExcel, showContracts)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public string BuildReportTable(bool forExcel, bool showContracts)
        {
            return BuildReportTable(forExcel, showContracts, null);
        }
        #endregion

        #region BuildReportTable method (forExcel, showContracts, tableNumber)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <param name="showContracts">TBD</param>
        /// <param name="tableNumber">TBD</param>
        /// <returns>TBD</returns>
        private string BuildReportTable(bool forExcel, bool showContracts, int? tableNumber)
        {
            int count = 0;
            int tableCount = 0;
            bool isLastRow;
            previousSumOnValues = new string[sumFields.Length];
            currentSumOnValues = new string[sumFields.Length];
            //Add an extra string builder for the contracts
            reportBuilder = new StringBuilder[sumFields.Length + 1];
            //Initialize the StringBuilder that will hold the entire report
            reportTable = new StringBuilder();
            //Initialized the Table that will hold the Flash Data
            if (forExcel && tableNumber != null && tableNumber != 0)
            {
                reportDataSorted = reportData.Tables[(int)tableNumber];
            }
            else
            {
                reportDataSorted = reportData.Tables[0];
            }
            //Add the Table headers to the report
            tableCount++;
            salesType = (reportData.Tables[0].Columns.Contains("SALES_TYPE")) ? ((string)reportData.Tables[0].Rows[0]["SALES_TYPE"] ?? string.Empty) : "Flash";
            salesTypeId = salesType.Replace(" ", "_");
            reportTable.AppendLine((!forExcel) ? BuildReportTableHeaders(tableCount) : BuildExcelReportTableHeaders(showContracts));            
            //Initialize the report generation arrays
            for (int i = 0; i < sumFields.Length; i++)
            {
                previousSumOnValues[i] =
                currentSumOnValues[i] = Convert.ToString(IO.GetDataRowValue(reportDataSorted.Rows[0], sumFields[i], ""));
                //(string)reportDataSorted.Rows[0][sumFields[i]];
                reportBuilder[i] = new StringBuilder();
            }
            //Initialize the contract table stringbuilder
            reportBuilder[sumFields.Length] = new StringBuilder();
            //If only the contracts were requested, do not group the report
            if (sumFields.Length == 0)
            {
                if (!forExcel)
                {
                    foreach (DataRow row in reportDataSorted.Rows)
                    {
                        reportBuilder[0].AppendLine(GenerateContractRow(row, forExcel));
                    }
                }
                else
                {
                    reportBuilder[0].AppendLine(GenerateContractRow(true));
                }
            }
            else
            {
                foreach (DataRow row in reportDataSorted.Rows)
                {
                    for (int i = 0; i < sumFields.Length; i++)
                    {
                        currentSumOnValues[i] = Convert.ToString(IO.GetDataRowValue(row, sumFields[i], ""));
                    }
                    isLastRow = (count + 1 == reportDataSorted.Rows.Count);
                    for (int i = sumFields.Length - 1; i >= 0; i--)
                    {
                        if (previousSumOnValues[i] != currentSumOnValues[i] || ((previousSumOnValues[i] == currentSumOnValues[i]) && HasChangeAtAnyParentLevel(i)))
                        {
                            reportBuilder[i].AppendLine(GenerateLineItemRow(i, count, forExcel, showContracts));
                            reportBuilder[i + 1] = new StringBuilder();
                        }
                        previousSumOnValues[i] = currentSumOnValues[i];
                    }
                    //if this is the last row, go through the arrays one more time
                    if (isLastRow)
                    {
                        if (showContracts)
                        {
                            reportBuilder[sumFields.Length].AppendLine(GenerateContractRow(row, forExcel));
                        }
                        for (int i = sumFields.Length - 1; i >= 0; i--)
                        {
                            reportBuilder[i].AppendLine(GenerateLineItemRow(i, count, forExcel, showContracts));
                            reportBuilder[i + 1] = new StringBuilder();
                        }
                    }
                    if (!forExcel || (forExcel && showContracts))
                    {
                        reportBuilder[sumFields.Length].AppendLine(GenerateContractRow(row, forExcel));
                    }
                    count++;
                }
            }
            //Calculate the Grand Totals
            reportTable.AppendLine(reportBuilder[0].ToString());
            reportTable.AppendLine(GenerateGrandTotalRow(forExcel));
            reportTable.AppendLine("<br/>");
            return reportTable.ToString();
        }
        #endregion

        #region BuildReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="tableCount">TBD</param>
        /// <returns>TBD</returns>
        public virtual string BuildReportTableHeaders(int tableCount)
        {
            throw new NotImplementedException("BuildReportTableHeaders(int tableCount)");
        }
        #endregion

        #region BuildSubTableRow method
        /// <summary>TBD</summary>
        /// <param name="subTable">TBD</param>
        /// <param name="divId">TBD</param>
        /// <returns>TBD</returns>
        public virtual string BuildSubTableRow(StringBuilder subTable, string divId)
        {
            throw new NotImplementedException("BuildSubTableRow(StringBuilder subTable, string divId)");
        }
        #endregion

        #region DoubleDecimalDisplay method
        /// <summary>TBD</summary>
        /// <param name="value">TBD</param>
        /// <returns>TBD</returns>
        public static string DoubleDecimalDisplay(double value)
        {
            return DoubleDisplay(value, true, true);
        }
        #endregion

        #region DoubleDisplay method (value)
        /// <summary>TBD</summary>
        /// <param name="value">TBD</param>
        /// <returns>TBD</returns>
        public static string DoubleDisplay(double value)
        {
            return DoubleDisplay(value, true, false);
        }
        #endregion

        #region DoubleDisplay method (value, wantZero, wantDecimal)
        /// <summary>TBD</summary>
        /// <param name="value">TBD</param>
        /// <param name="wantZero">TBD</param>
        /// <param name="wantDecimal">TBD</param>
        /// <returns>TBD</returns>
        public static string DoubleDisplay(double value, bool wantZero, bool wantDecimal)
        {
            string mask = (wantDecimal) ? "{0:#,###.00}" : "{0:#,###}";
            string negativeMask = (wantDecimal) ? "{0:#,###.00;(#,###.00)}" : "{0:#,###;(#,###)}";
            if (value == 0)
            {
                return ((wantZero) ? "0" : "&nbsp;");
            }
            if (value < 0)
            {
                return string.Format(@"<span style=""color:red"">{0}</span>", String.Format(negativeMask, value));
            }
            return String.Format(mask, value);
        }
        #endregion

        #region GenerateContractRow method (forExcel)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public virtual string GenerateContractRow(bool forExcel)
        {
            throw new NotImplementedException("GenerateContractRow(bool forExcel)");
        }
        #endregion

        #region GenerateContractRow method (row, forExcel)
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public virtual string GenerateContractRow(DataRow row, bool forExcel)
        {
            throw new NotImplementedException("GenerateContractRow(DataRow row, bool forExcel)");
        }
        #endregion

        #region GenerateGrandTotalRow method
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public virtual string GenerateGrandTotalRow(bool forExcel)
        {
            throw new NotImplementedException("GenerateGrandTotalRow(bool forExcel)");
        }
        #endregion

        #region GenerateLineItemRow method
        /// <summary>TBD</summary>
        /// <param name="level">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public virtual string GenerateLineItemRow(int level, int count, bool forExcel, bool showContracts)
        {
            throw new NotImplementedException("GenerateLineItemRow(int level, int count, bool forExcel, bool showContracts)");
        }
        #endregion

        #region GenerateReport method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string GenerateReport()
        {
            return BuildReportTable(false, true);
        }
        #endregion

        #region GenerateReportExcelExport method (showContracts)
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateReportExcelExport(bool showContracts)
        {
            return BuildReportTable(true, showContracts);
        }
        #endregion

        #region GenerateReportExcelExport method (showContracts, table)
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <param name="table">TBD</param>
        /// <returns>TBD</returns>
        public string GenerateReportExcelExport(bool showContracts, int table)
        {
            return BuildReportTable(true, showContracts, table);
        }
        #endregion

        #region GenerateUniqueDivId method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        protected string GenerateUniqueDivId()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        #region GetContractSumFilterExpression method
        /// <summary>TBD</summary>
        /// <param name="sumFields">TBD</param>
        /// <param name="currentValues">TBD</param>
        /// <returns>TBD</returns>
        protected string GetContractSumFilterExpression(string[] sumFields, string[] currentValues)
        {
            return GetSumFilterExpression(sumFields, currentValues, sumFields.Length - 1);
        }
        #endregion

        #region GetErrorOccurredDisplay method
        /// <summary>Generates a default Error message row</summary>
        /// <returns>An HTML Table Row containing a friendly error message</returns>
        public static string GetErrorOccurredDisplay()
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td colspan=""15"" align=""right"">");
            recordDisplay.Append(@"         <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_corner.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr><td colspan=""14"" style=""text-align:left;"">An error occurred while trying to execute your search. Please try again.</td></tr>");
            recordDisplay.Append(@"<tr>");
            recordDisplay.Append(@"     <td colspan=""15"" valign=""top"">");
            recordDisplay.Append(@"         <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_flip.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner_flip.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.AppendLine(@"</tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region GetNoContractFoundDisplay method
        /// <summary>Generates a default No Contract Found row</summary>
        /// <returns>An HTML Table Row containing a friendly message</returns>
        public static string GetNoContractFoundDisplay()
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.AppendLine(@"<tr><td colspan=""18"" style=""text-align:left;"">No Contract Found</td></tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region GetNoRecordFoundDisplay method
        /// <summary>Generates a default No Record Found row</summary>
        /// <returns>An HTML Table Row containing a friendly message</returns>
        public static string GetNoRecordFoundDisplay()
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td colspan=""19"" align=""right"">");
            recordDisplay.Append(@"         <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_corner.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr><td colspan=""18"" style=""text-align:left;"">No Record Found</td></tr>");
            recordDisplay.Append(@"<tr>");
            recordDisplay.Append(@"     <td colspan=""19"" valign=""top"">");
            recordDisplay.Append(@"         <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_flip.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner_flip.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.AppendLine(@"</tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region GetSumFilterExpression method
        /// <summary>TBD</summary>
        /// <param name="sumFields">TBD</param>
        /// <param name="currentValues">TBD</param>
        /// <param name="stop">TBD</param>
        /// <returns>TBD</returns>
        protected string GetSumFilterExpression(string[] sumFields, string[] currentValues, int stop)
        {
            string filterExpression = "";
            for (int i = 0; i <= stop; i++)
            {
                filterExpression += (String.IsNullOrEmpty(filterExpression) ? "" : " AND ") + "[" + sumFields[i] + "] = '" + currentValues[i].Replace("'", "''") + "' ";
            }
            return filterExpression;
        }
        #endregion

        #region HasChangeAtAnyParentLevel method
        /// <summary>TBD</summary>
        /// <param name="index">TBD</param>
        /// <returns>TBD</returns>
        private bool HasChangeAtAnyParentLevel(int index)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (previousSumOnValues[i] != currentSumOnValues[i])
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class OnlineFlashReport : DrillDownReport
    {

        #region Member variables
        /// <summary>TBD</summary>
        private DateTime startDate;
        #endregion

        #region Parameterized constructor (reportData, sumFields, startDate, market, companyId)
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <param name="sumFields">TBD</param>
        /// <param name="startDate">TBD</param>
        /// <param name="market">TBD</param>
        /// <param name="companyId">TBD</param>
        public OnlineFlashReport(DataSet reportData, string[] sumFields, DateTime startDate, string market, string companyId)
            : base(reportData, sumFields, market, companyId)
        {
            this.startDate = startDate;
        }
        #endregion

        #region BuildColumnHeader method
        /// <summary>TBD</summary>
        /// <param name="offSet">TBD</param>
        /// <param name="current">TBD</param>
        /// <returns>TBD</returns>
        public string BuildColumnHeader(int offSet, out bool current)
        {
            current = (startDate.AddMonths(offSet).Month == DateTime.Now.Month && startDate.AddMonths(offSet).Year == DateTime.Now.Year);
            string showMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.AddMonths(offSet).Month).Substring(0, 3);
            string showYear = Convert.ToString(startDate.AddMonths(offSet).Year).Substring(2, 2);
            return string.Format("{0} {1}", showMonth, showYear);
        }
        #endregion

        #region BuildExcelReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildExcelReportTableHeaders(bool showContracts)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.AppendLine(@"<table cellspacing=""1"" cellpadding=""0"" border=""1"" align=""center"">");
            recordDisplay.AppendFormat(@"     <tr><td colspan=""19"" height=""55"" align=""center""><img src=""http://{0}/Images/flash/titan_report_logo.gif"" alt=""Titan 360"" /></td></tr>", HttpContext.Current.Request.Url.Host);
            recordDisplay.AppendLine(@"     <tr><td colspan=""19"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>Flash Report</b></td></tr>");
            recordDisplay.AppendLine(@"     <tr nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_);"">");
            if (showContracts || sumFields.Length == 0)
            {
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Contract</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>AE</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Agency</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Advertiser</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Customer Type</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Program</b></td>");
            }
            else
            {
                recordDisplay.AppendLine(@"         <td colspan=""6"">&nbsp;</td>");
            }
            bool isCurrent;
            string headerText;
            for (int i = 0; i < 12; i++)
            {
                headerText = BuildColumnHeader(i, out isCurrent);
                recordDisplay.AppendFormat(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", headerText);
            }
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Total</b></td>");
            recordDisplay.AppendLine(@"     </tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="tableCount">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildReportTableHeaders(int tableCount)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.Append(@"");
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td></td>");
            recordDisplay.Append(@"     <td colspan='13' style=""text-align:center;"">");
            recordDisplay.Append(@"         <table width=""100%"" cellpadding='0' cellspacing='0' border='0' bgcolor=""#ffffff"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_left_corner.gif""></td>");
            recordDisplay.AppendFormat(@"                 <td class=""report_heading"" nowrap background=""/Images/flash/blue_thick_bar.gif"">&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", (String.IsNullOrEmpty(this.marketName) ? string.Format("All Markets {0}", salesType) : this.marketName));
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td></td>");
            recordDisplay.Append(@"     <td colspan='13' align=""right"">");
            recordDisplay.Append(@"         <table width='100%' cellpadding='0' cellspacing='0' border='0'>");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_corner.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr>");
            recordDisplay.Append(@"     <td bgcolor=""#ffffff"" style=""color:#666699;font-weight:bold;text-align:left;"">");
            recordDisplay.AppendFormat(@"<div style=""float:left;margin-right:3px""><img src=""/Images/but_excel.png"" border=0 style=""cursor:pointer;"" onclick=""openexcel({0});""></div><div style=""float:left;""><img src=""/Images/but_excel_alt.png"" border=0 style=""cursor:pointer;"" onclick=""openexcelalternate({0});""></div><div style=""clear:both;""></div>", tableCount);
            recordDisplay.Append(@"         <br/>");
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""expandAll('{0}');"">Expand All</a>", salesTypeId);
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""collapseAll('{0}');"">Collapse All</a>", salesTypeId);
            recordDisplay.Append(@"    </td>");
            bool isCurrent;
            string headerText;
            for (int i = 0; i < 12; i++)
            {
                headerText = BuildColumnHeader(i, out isCurrent);
                recordDisplay.AppendFormat(@"     <td class=""dashboard_column_head{0}"">{1}</td>", ((isCurrent) ? "_current" : ""), headerText);
            }
            recordDisplay.Append(@"     <td width=""6%"" class=""dashboard_column_head"">Total</td>");
            recordDisplay.AppendLine(@"</tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildSubTableRow method
        /// <summary>TBD</summary>
        /// <param name="subTable">TBD</param>
        /// <param name="divId">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildSubTableRow(StringBuilder subTable, string divId)
        {
            StringBuilder subTableRow = new StringBuilder();
            subTableRow.AppendFormat(@"<div class=""{0}"" id=""{1}"" style=""display:none"">", salesTypeId, divId);
            subTableRow.Append(@"  <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" bgcolor=""#ededed"" style=""border-collapse: collapse;border-spacing: 0px;border-width:0px;"">");
            subTableRow.Append(@"      <tr style=""height:0px;background-color:#ffffff;"">");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" width=""22%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""192""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""67""></td>");
            subTableRow.Append(@"      </tr>");
            subTableRow.Append(subTable.ToString());
            subTableRow.Append(@"  </table>");
            subTableRow.AppendLine(@"</div>");
            return subTableRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (forExcel)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            string previousContractNumber = "";
            string currentContractNumber = "";
            string filterExpression = GetContractSumFilterExpression(sumFields, currentSumOnValues);
            foreach (DataRow row in this.reportDataSorted.Select(filterExpression))
            {
                currentContractNumber = (string)row["CONTRACT_NUMBER"];
                if (previousContractNumber != currentContractNumber)
                {
                    if (!forExcel)
                    {
                        contractRow.AppendFormat(@"<tr class=""contract_item_row"" align=""right"" ondblclick=""ShowContractDetail({0},{1});"" onclick=""ShowContractDetail({0},{1});"" onmouseover=""mouseincolor(this);"" onmouseout=""mouseoutcolor(this);"">", currentContractNumber, this.companyId);
                        contractRow.AppendFormat(@"     <td class=""contract_item_row_text"">{0} - {1}&nbsp;</td>", (string)row["CONTRACT_NUMBER"], (string)row["ADVERTISER"]);
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.AppendLine("</tr>");
                    }
                    else
                    {
                        if (sumFields.Length == 0)
                        {
                            //filterExpression = string.Format("PROFIT_CENTER = '{0}' AND AE = '{1}'", (string)row["PROFIT_CENTER"], (string)row["AE"]);
                            //filterExpression = string.Format("AE = '{0}'", (string)row["AE"]);
                            filterExpression = string.Format("(1=1)");
                        }
                        contractRow.Append(@"      <tr valign=""top"">");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">" + (row["CONTRACT_NUMBER"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["AE"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["AGENCY"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["ADVERTISER"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["CUSTOMER_TYPE"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["PROGRAM"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.AppendLine("       </tr>");
                    }
                }
                previousContractNumber = currentContractNumber;
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (row, forExcel)
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(DataRow row, bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            if (!forExcel)
            {
                contractRow.AppendFormat(@"<tr class=""contract_item_row"" align=""right"" style=""cursor:pointer;background-color:#ffffff;color:#2f4070"" ondblclick=""ShowContractDetail({0},{1});"" onclick=""ShowContractDetail({0},{1});"" onmouseover=""mouseincolor(this);"" onmouseout=""mouseoutcolor(this);"">", (string)row["CONTRACT_NUMBER"], this.companyId);
                contractRow.AppendFormat(@"     <td class=""contract_item_row_text"">{0} - {1}&nbsp;</td>", (string)row["CONTRACT_NUMBER"], Convert.ToString(IO.GetDataRowValue(row, "ADVERTISER", "N/A")));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["1"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["2"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["3"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["4"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["5"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["6"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["7"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["8"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["9"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["10"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["11"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["12"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Total"]), false, false));
                contractRow.AppendLine(@"</tr>");
            }
            else
            {
                contractRow.Append(@"      <tr valign=""top"">");
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row["CONTRACT_NUMBER"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["AE"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["AGENCY"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["ADVERTISER"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["CUSTOMER_TYPE"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["PROGRAM"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["1"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["2"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["3"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["4"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["5"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["6"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["7"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["8"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["9"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["10"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["11"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["12"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Total"]), false, false));
                contractRow.AppendLine("       </tr>");
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateGrandTotalRow method
        /// <summary>TBD</summary>
        /// <param name="forExcel">Flag for determining if this report is being Exported to Excel</param>
        /// <returns>TBD</returns>
        public override string GenerateGrandTotalRow(bool forExcel)
        {
            StringBuilder grandTotal = new StringBuilder();
            if (!forExcel)
            {
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td class=""grand_total_text"">Grand Total&nbsp;</td>");
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum(Total)", ""))));
                grandTotal.AppendLine(@"</tr>");
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td>&nbsp;</td>");
                grandTotal.Append(@"    <td colspan=""14"" valign=""top"" style=""border-top:1px solid #ffffff;"">");
                grandTotal.Append(@"        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
                grandTotal.Append(@"            <tr>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_left_flip.gif""></td>");
                grandTotal.Append(@"                <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_right_corner_flip.gif""></td>");
                grandTotal.Append(@"            </tr>");
                grandTotal.Append(@"        </table>");
                grandTotal.Append(@"    </td>");
                grandTotal.AppendLine(@"</tr>");
            }
            else
            {
                grandTotal.Append(@"    <tr valign=""top"">");
                grandTotal.Append(@"        <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""right"" colspan=""6""><b>Grand Total&nbsp;</b></td>");
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum(Total)", ""))));
                grandTotal.Append(@"    </tr>");
                grandTotal.AppendLine(@"</table>");
            }
            return grandTotal.ToString();
        }
        #endregion

        #region GenerateLineItemRow method
        /// <summary>Generates a row in the Grouping report</summary>
        /// <param name="level">Determines which level in the Grouping this row will fall in</param>
        /// <param name="count">Used for formatting the style of the row</param>
        /// <param name="forExcel">Flag for determining if this will be exported to Excel</param>
        /// <param name="showContracts">Flag for determining if the Excel export will display contracts</param>
        /// <returns>A formatted HTML Table Row containing Contract info</returns>
        public override string GenerateLineItemRow(int level, int count, bool forExcel, bool showContracts)
        {
            StringBuilder lineItem = new StringBuilder();
            string uniqueDivId = GenerateUniqueDivId();
            string displayText = this.previousSumOnValues[level];
            string displayField = this.sumFields[level].Replace("_", " ");
            StringBuilder rowToAppend = reportBuilder[level + 1];
            string filterExpression = GetSumFilterExpression(sumFields, previousSumOnValues, level);
            if (!forExcel)
            {
                lineItem.AppendFormat(@"<tr class=""forecast_report_{0}_total{1}"">", level, ((count % 2 == 0) ? "0" : "1"));
                lineItem.AppendFormat(@"    <td class=""report_market_{0}"" onclick=""showOrhide('{1}');"" style=""border-left:0px;cursor:pointer;text-align:left;"">", level, uniqueDivId);
                lineItem.AppendFormat(@"        <img id=""{0}_img"" src=""/Images/flash/arrow_right_blue.gif"" />&nbsp;{1}", uniqueDivId, displayText);
                lineItem.Append(@"    </td>");
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"" style=""border-right:0px;"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum(Total)", filterExpression))));
                lineItem.AppendLine(@"</tr>");
                lineItem.AppendFormat(@"<tr><td colspan=""14"">{0}</td></tr>", BuildSubTableRow(rowToAppend, uniqueDivId));
            }
            else
            {
                if (level == 0 && this.sumFields.Length > 1)
                {
                    lineItem.Append(@"     <tr valign=""top"">");
                    lineItem.Append(@"         <td colspan=""19"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_);" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @"""><b>" + displayText + "</b></td>");
                    lineItem.AppendLine(@"     </tr>");
                }
                lineItem.Append(rowToAppend.ToString());
                lineItem.Append(@"     <tr valign=""top"">");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"" colspan=""6""><b>" + ((showContracts) ? "TOTAL " : "") + displayField + ": " + displayText + "</b></td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum(Total)", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.AppendLine(@"     </tr>");
            }
            return lineItem.ToString();
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class OnlineFlashWithoutSubTotalReport : OnlineFlashReport
    {

        #region Member variables
        /// <summary>TBD</summary>
        private DateTime startDate;
        /// <summary>TBD</summary>
        private Hashtable sumFieldFriendlyNames = new Hashtable();
        #endregion

        #region Parameterized constructor (reportData, sumFields, startDate, market, companyId)
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <param name="sumFields">TBD</param>
        /// <param name="startDate">TBD</param>
        /// <param name="market">TBD</param>
        /// <param name="companyId">TBD</param>
        public OnlineFlashWithoutSubTotalReport(DataSet reportData, string[] sumFields, DateTime startDate, string market, string companyId)
            : base(reportData, sumFields, startDate, market, companyId)
        {
            this.startDate = startDate;
            this.BuildSumFieldFriendlyNameHash();
        }
        #endregion

        #region BuildExcelReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildExcelReportTableHeaders(bool showContracts)
        {
            if (!showContracts)
            {
                return base.BuildExcelReportTableHeaders(false);
            }
            StringBuilder recordDisplay = new StringBuilder();
            int span = 21 + sumFields.Length;
            recordDisplay.AppendLine(@"<table cellspacing=""1"" cellpadding=""0"" border=""1"" align=""center"">");
            recordDisplay.AppendFormat(@"     <tr><td colspan=""{1}"" height=""55"" align=""center""><img src=""http://{0}/Images/flash/titan_report_logo.gif"" alt=""Titan 360"" /></td></tr>", HttpContext.Current.Request.Url.Host, span);
            recordDisplay.AppendFormat(@"     <tr><td colspan=""{0}"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>Flash Report</b></td></tr>", span);
            recordDisplay.AppendLine(@"     <tr nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_);"">");
            if (sumFields.Length > 0)
            {
                foreach (string sumField in sumFields)
                {
                    if (string.Compare(sumField, "AE", true) == 0)
                    {
                        continue;
                    }
                    recordDisplay.AppendFormat(@"<td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>{0}</b></td>", this.sumFieldFriendlyNames[sumField]);
                }
            }
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Market</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Contract</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>AE</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>AE Market</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Agency</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Advertiser</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Customer Type</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Program</b></td>");
            bool isCurrent;
            string headerText;
            for (int i = 0; i < 12; i++)
            {
                headerText = base.BuildColumnHeader(i, out isCurrent);
                recordDisplay.AppendFormat(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", headerText);
            }
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Total</b></td>");
            recordDisplay.AppendLine(@"     </tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildSumFieldFriendlyNameHash method
        /// <summary>TBD</summary>
        private void BuildSumFieldFriendlyNameHash()
        {
            this.sumFieldFriendlyNames.Add("MARKET", "Market");
            this.sumFieldFriendlyNames.Add("PROFIT_CENTER", "Profit Center");
            this.sumFieldFriendlyNames.Add("REVENUE_TYPE", "Revenue Type");
            this.sumFieldFriendlyNames.Add("MEDIA_TYPE", "Media Type");
            this.sumFieldFriendlyNames.Add("MEDIA_FORM", "Media Form");
            this.sumFieldFriendlyNames.Add("PARENT_PROD_CLASS", "Parent Product Class");
            this.sumFieldFriendlyNames.Add("PRODUCT_CLASS", "Product Class");
            this.sumFieldFriendlyNames.Add("AE", "AE");
        }
        #endregion

        #region GenerateContractRow method (forExcel)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(bool forExcel)
        {
            if (!forExcel)
            {
                return base.GenerateContractRow(false);
            }
            StringBuilder contractRow = new StringBuilder();
            string previousContractNumber = "";
            string currentContractNumber = "";
            string filterExpression = GetContractSumFilterExpression(sumFields, currentSumOnValues);
            foreach (DataRow row in this.reportDataSorted.Select(filterExpression))
            {
                currentContractNumber = (string)row["CONTRACT_NUMBER"];
                if (previousContractNumber != currentContractNumber)
                {
                    contractRow.Append(@"      <tr valign=""top"">");
                    if (sumFields.Length == 0)
                    {
                        filterExpression = string.Format("(1=1)");
                    }
                    else
                    {
                        foreach (string sumField in sumFields)
                        {
                            if (string.Compare(sumField, "AE", true) == 0)
                            {
                                continue;
                            }
                            contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row[sumField] as string ?? "&nbsp;") + "</td>");
                        }
                    }
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">" + (row["MARKET"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">" + (row["CONTRACT_NUMBER"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["AE"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["AEMARKET"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["AGENCY"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["ADVERTISER"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["CUSTOMER_TYPE"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["PROGRAM"] as string ?? "&nbsp;") + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "CONTRACT_NUMBER = '" + currentContractNumber + "'")), false, false) + "</td>");
                    contractRow.AppendLine("       </tr>");
                }
                previousContractNumber = currentContractNumber;
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (row, forExcel)
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(DataRow row, bool forExcel)
        {
            if (!forExcel)
            {
                return base.GenerateContractRow(row, false);
            }
            StringBuilder contractRow = new StringBuilder();
            contractRow.Append(@"      <tr valign=""top"">");
            if (sumFields.Length > 0)
            {
                foreach (string sumField in sumFields)
                {
                    if (string.Compare(sumField, "AE", true) == 0)
                    {
                        continue;
                    }
                    contractRow.AppendFormat(@"<td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row[sumField] as string ?? "&nbsp;"));
                }
            }
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row["MARKET"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row["CONTRACT_NUMBER"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["AE"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["AEMARKET"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["AGENCY"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["ADVERTISER"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["CUSTOMER_TYPE"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["PROGRAM"] as string ?? "&nbsp;"));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["1"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["2"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["3"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["4"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["5"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["6"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["7"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["8"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["9"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["10"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["11"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["12"]), false, false));
            contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Total"]), false, false));
            contractRow.AppendLine("       </tr>");
            return contractRow.ToString();
        }
        #endregion

        #region GenerateGrandTotalRow method
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateGrandTotalRow(bool forExcel)
        {
            if (!forExcel)
            {
                return base.GenerateGrandTotalRow(false);
            }
            StringBuilder grandTotal = new StringBuilder();
            int grandTotalColumnSpan = 8 + sumFields.Length;
            grandTotal.Append(@"    <tr valign=""top"">");
            grandTotal.AppendFormat(@"        <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""right"" colspan=""{0}""><b>Grand Total&nbsp;</b></td>", grandTotalColumnSpan);
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([1])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([2])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([3])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([4])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([5])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([6])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([7])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([8])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([9])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([10])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([11])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([12])", ""))));
            grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum(Total)", ""))));
            grandTotal.Append(@"    </tr>");
            grandTotal.AppendLine(@"</table>");
            return grandTotal.ToString();
        }
        #endregion

        #region GenerateLineItemRow method
        /// <summary>TBD</summary>
        /// <param name="level">TBD</param>
        /// <param name="count">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateLineItemRow(int level, int count, bool forExcel, bool showContracts)
        {
            if (!forExcel)
            {
                return base.GenerateLineItemRow(level, count, false, showContracts);
            }
            return reportBuilder[level + 1].ToString();
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class QuaterlyBillingReport : DrillDownReport
    {

        #region Member variables
        /// <summary>TBD</summary>
        private int quarter;
        #endregion

        #region Parameterized constructor (reportData, sumFields, quarter, market, companyId)
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <param name="sumFields">TBD</param>
        /// <param name="quarter">TBD</param>
        /// <param name="market">TBD</param>
        /// <param name="companyId">TBD</param>
        public QuaterlyBillingReport(DataSet reportData, string[] sumFields, int quarter, string market, string companyId)
            : base(reportData, sumFields, market, companyId)
        {
            this.quarter = quarter;
        }
        #endregion

        #region BuildExcelReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildExcelReportTableHeaders(bool showContracts)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.AppendLine(@"<table cellspacing=""1"" cellpadding=""0"" border=""1"" align=""center"">");
            switch (this.quarter)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    recordDisplay.AppendFormat(@"     <tr><td colspan=""8"" height=""55"" align=""center""><img src=""http://{0}/Images/flash/titan_report_logo.gif"" alt=""Titan 360"" /></td></tr>", HttpContext.Current.Request.Url.Host);
                    recordDisplay.AppendLine(@"     <tr><td colspan=""8"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>Quarterly Report</b></td></tr>");
                    break;
                default:
                    recordDisplay.AppendFormat(@"     <tr><td colspan=""21"" height=""55"" align=""center""><img src=""http://{0}/Images/flash/titan_report_logo.gif"" alt=""Titan 360"" /></td></tr>", HttpContext.Current.Request.Url.Host);
                    recordDisplay.AppendLine(@"     <tr><td colspan=""21"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>Quarterly Report</b></td></tr>");
                    break;
            }
            recordDisplay.AppendLine(@"     <tr nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_);"">");
            if (showContracts)
            {
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Contract Entry Date</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Contract</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>AE</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Advertiser</b></td>");
            }
            else
            {
                recordDisplay.AppendLine(@"         <td colspan=""4"">&nbsp;</td>");
            }
            switch (this.quarter)
            {
                case 1:
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>January Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>February Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>March Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q1 Billing</b></td>");
                    break;
                case 2:
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>April Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>May Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>June Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q2 Billing</b></td>");
                    break;
                case 3:
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>July Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>August Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>September Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q3 Billing</b></td>");
                    break;
                case 4:
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>October Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>November Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>December Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q4 Billing</b></td>");
                    break;
                default:
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>January Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>February Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>March Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q1 Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>April Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>May Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>June Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q2 Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>July Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>August Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>September Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q3 Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>October Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>November Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>December Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Q4 Billing</b></td>");
                    recordDisplay.Append(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>Total Billing</b></td>");
                    break;
            }
            recordDisplay.AppendLine(@"     </tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="tableCount">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildReportTableHeaders(int tableCount)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.Append("");
            recordDisplay.Append(@"<tr>");
            switch (this.quarter)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    recordDisplay.Append(@"<td width=""41%""><img src=""/Images/flash/spacer.gif"" width=""192"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""14%"" align=""right""><img src=""../Images/flash/spacer.gif"" width=""61"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""14%"" align=""right""><img src=""../Images/flash/spacer.gif"" width=""61"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""14%"" align=""right""><img src=""../Images/flash/spacer.gif"" width=""61"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""17%"" align=""right""><img src=""../Images/flash/spacer.gif"" width=""67"" height=""0""></td>");
                    recordDisplay.Append(@"</tr><tr bgcolor=""#ffffff""><td colspan=""6"">&nbsp;</td></tr>");
                    break;
                default:
                    recordDisplay.Append(@"<td width=""21%""><img src=""/Images/flash/spacer.gif"" width=""192"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""6%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""6%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""6%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""4%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""6%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"<td width=""7%"" align=""right""><img src=""../Images/flash/spacer.gif"" height=""0""></td>");
                    recordDisplay.Append(@"</tr><tr bgcolor=""#ffffff""><td colspan=""19"">&nbsp;</td></tr>");
                    break;
            }
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td></td>");
            switch (this.quarter)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    recordDisplay.Append(@"     <td colspan='4' style=""text-align:center;"">");
                    break;
                default:
                    recordDisplay.Append(@"     <td colspan='17' style=""text-align:center;"">");
                    break;
            }
            recordDisplay.Append(@"         <table width=""100%"" cellpadding='0' cellspacing='0' border='0' bgcolor=""#ffffff"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_left_corner.gif""></td>");
            recordDisplay.AppendFormat(@"                 <td class=""report_heading"" nowrap background=""/Images/flash/blue_thick_bar.gif"">&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", (String.IsNullOrEmpty(this.marketName) ? string.Format("All Markets {0}", salesType) : this.marketName));
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td></td>");
            switch (this.quarter)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    recordDisplay.Append(@"     <td colspan='4' style=""text-align:center;"">");
                    break;
                default:
                    recordDisplay.Append(@"     <td colspan='17' style=""text-align:center;"">");
                    break;
            }
            recordDisplay.Append(@"         <table width='100%' cellpadding='0' cellspacing='0' border='0'>");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_corner.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr>");
            recordDisplay.Append(@"     <td bgcolor=""#ffffff"" style=""color:#666699;font-weight:bold;text-align:left;"">");
            recordDisplay.AppendFormat(@"         <img src=""/Images/but_excel.gif"" border=0 style=""cursor:pointer;"" onclick=""openexcel({0});"">", tableCount);
            recordDisplay.Append(@"         <br/>");
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""expandAll('{0}');"">Expand All</a>", salesTypeId);
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""collapseAll('{0}');"">Collapse All</a>", salesTypeId);
            recordDisplay.Append(@"    </td>");
            switch (this.quarter)
            {
                case 1:
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">January<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Feburary<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">March<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q1 Billing</td>");
                    break;
                case 2:
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">April<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">May<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">June<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q2 Billing</td>");
                    break;
                case 3:
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">July<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">August<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">September<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q3 Billing</td>");
                    break;
                case 4:
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">October<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">November<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">December<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q4 Billing</td>");
                    break;
                default:
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">January<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Feburary<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">March<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q1 Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">April<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">May<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">June<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q2 Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">July<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">August<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">September<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q3 Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">October<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">November<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">December<br/>Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Q4 Billing</td>");
                    recordDisplay.Append(@"    <td class=""dashboard_column_head"">Total Billing</td>");
                    break;
            }
            recordDisplay.AppendLine(@"</tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildSubTableRow method
        /// <summary>TBD</summary>
        /// <param name="subTable">TBD</param>
        /// <param name="divId">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildSubTableRow(StringBuilder subTable, string divId)
        {
            StringBuilder subTableRow = new StringBuilder();
            subTableRow.AppendFormat(@"<div class=""{0}"" id=""{1}"" style=""display:none"">", salesTypeId, divId);
            subTableRow.Append(@"  <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" bgcolor=""#ededed"" style=""border-collapse: collapse;border-spacing: 0px;border-width:0px;"">");
            subTableRow.Append(@"      <tr style=""height:0px;background-color:#ffffff;"">");
            switch (this.quarter)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" width=""41%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""192""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""14%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""14%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""14%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""17%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    break;
                default:
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" width=""21%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""192""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""4%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""6%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
                    subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""7%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""67""></td>");
                    break;
            }
            subTableRow.Append(@"      </tr>");
            subTableRow.Append(subTable.ToString());
            subTableRow.Append(@"  </table>");
            subTableRow.AppendLine(@"</div>");
            return subTableRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (forExcel)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            string previousContractNumber = "";
            string currentContractNumber = "";
            string filterExpression = base.GetContractSumFilterExpression(sumFields, currentSumOnValues);
            foreach (DataRow row in this.reportDataSorted.Select(filterExpression))
            {
                currentContractNumber = (string)row["CONTRACT_NUMBER"];
                if (previousContractNumber != currentContractNumber)
                {
                    contractRow.AppendLine(GenerateContractRow(row, forExcel));
                }
                previousContractNumber = currentContractNumber;
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (row, forExcel)
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(DataRow row, bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            if (!forExcel)
            {
                contractRow.AppendFormat(@"<tr class=""contract_item_row"" align=""right"" style=""cursor:pointer;background-color:#ffffff;color:#2f4070"" ondblclick=""ShowContractDetail({0},{1});"" onclick=""ShowContractDetail({0},{1});"" onmouseover=""mouseincolor(this);"" onmouseout=""mouseoutcolor(this);"">", (string)row["CONTRACT_NUMBER"], this.companyId);
                contractRow.AppendFormat(@"     <td class=""contract_item_row_text"">{0} - {1} - {2}&nbsp;</td>", (string)row["CONTRACT_ENTRY_DATE"], (string)row["CONTRACT_NUMBER"], (string)row["ADVERTISER"]);
                switch (this.quarter)
                {
                    case 1:
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JANUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["FEBRUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MARCH BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q1 BILLING"]), false, false));
                        break;
                    case 2:
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["APRIL BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MAY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JUNE BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q2 BILLING"]), false, false));
                        break;
                    case 3:
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JULY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["AUGUST BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["SEPTEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q3 BILLING"]), false, false));
                        break;
                    case 4:
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["OCTOBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["NOVEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["DECEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q4 BILLING"]), false, false));
                        break;
                    default:
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JANUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["FEBRUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MARCH BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q1 BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["APRIL BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MAY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JUNE BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q2 BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JULY BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["AUGUST BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["SEPTEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q3 BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["OCTOBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["NOVEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["DECEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q4 BILLING"]), false, false));
                        contractRow.AppendFormat(@"     <td>{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["TOTAL BILLING"]), false, false));
                        break;
                }
                contractRow.AppendLine(@"</tr>");
            }
            else
            {
                contractRow.Append(@"      <tr valign=""top"">");
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row["CONTRACT_ENTRY_DATE"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row["CONTRACT_NUMBER"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["AE"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["ADVERTISER"] as string ?? "&nbsp;"));
                switch (this.quarter)
                {
                    case 1:
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JANUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["FEBRUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MARCH BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q1 BILLING"]), false, false));
                        break;
                    case 2:
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["APRIL BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MAY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JUNE BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q2 BILLING"]), false, false));
                        break;
                    case 3:
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JULY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["AUGUST BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["SEPTEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q3 BILLING"]), false, false));
                        break;
                    case 4:
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["OCTOBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["NOVEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["DECEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q4 BILLING"]), false, false));
                        break;
                    default:
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JANUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["FEBRUARY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MARCH BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q1 BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["APRIL BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["MAY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JUNE BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q2 BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["JULY BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["AUGUST BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["SEPTEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q3 BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["OCTOBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["NOVEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["DECEMBER BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["Q4 BILLING"]), false, false));
                        contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(row["TOTAL BILLING"]), false, false));
                        break;
                }
                contractRow.AppendLine("       </tr>");
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateGrandTotalRow method
        /// <summary>TBD</summary>
        /// <param name="forExcel">Flag for determining if this report is being Exported to Excel</param>
        /// <returns>TBD</returns>
        public override string GenerateGrandTotalRow(bool forExcel)
        {
            StringBuilder grandTotal = new StringBuilder();
            if (!forExcel)
            {
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td class=""grand_total_text"">Grand Total&nbsp;</td>");
                switch (this.quarter)
                {
                    case 1:
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", ""))));
                        break;
                    case 2:
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", ""))));
                        break;
                    case 3:
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", ""))));
                        break;
                    case 4:
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", ""))));
                        break;
                    default:
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([TOTAL BILLING])", ""))));
                        break;
                }
                grandTotal.AppendLine(@"</tr>");
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td>&nbsp;</td>");
                switch (this.quarter)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        grandTotal.Append(@"    <td colspan=""6"" valign=""top"" style=""border-top:1px solid #ffffff;"">");
                        break;
                    default:
                        grandTotal.Append(@"    <td colspan=""19"" valign=""top"" style=""border-top:1px solid #ffffff;"">");
                        break;
                }
                grandTotal.Append(@"        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
                grandTotal.Append(@"            <tr>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_left_flip.gif""></td>");
                grandTotal.Append(@"                <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_right_corner_flip.gif""></td>");
                grandTotal.Append(@"            </tr>");
                grandTotal.Append(@"        </table>");
                grandTotal.Append(@"    </td>");
                grandTotal.AppendLine(@"</tr>");
            }
            else
            {
                grandTotal.Append(@"    <tr valign=""top"">");
                grandTotal.Append(@"        <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""right"" colspan=""4""><b>Grand Total&nbsp;</b></td>");
                switch (this.quarter)
                {
                    case 1:
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", ""))));
                        break;
                    case 2:
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", ""))));
                        break;
                    case 3:
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", ""))));
                        break;
                    case 4:
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", ""))));
                        break;
                    default:
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", ""))));
                        grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([TOTAL BILLING])", ""))));
                        break;
                }
                grandTotal.Append(@"    </tr>");
                grandTotal.AppendLine(@"</table>");
            }
            return grandTotal.ToString();
        }
        #endregion

        #region GenerateLineItemRow method
        /// <summary>Generates a row in the Grouping report</summary>
        /// <param name="level">Determines which level in the Grouping this row will fall in</param>
        /// <param name="count">Used for formatting the style of the row</param>
        /// <param name="forExcel">Flag for determining if this will be exported to Excel</param>
        /// <param name="showContracts">Flag for determining if the Excel export will display contracts</param>
        /// <returns>A formatted HTML Table Row containing Contract info</returns>
        public override string GenerateLineItemRow(int level, int count, bool forExcel, bool showContracts)
        {
            StringBuilder lineItem = new StringBuilder();
            string uniqueDivId = GenerateUniqueDivId();
            string displayText = this.previousSumOnValues[level];
            string displayField = this.sumFields[level].Replace("_", " ");
            StringBuilder rowToAppend = reportBuilder[level + 1];
            string filterExpression = GetSumFilterExpression(sumFields, previousSumOnValues, level);
            if (!forExcel)
            {
                lineItem.AppendFormat(@"<tr class=""forecast_report_{0}_total{1}"">", level, ((count % 2 == 0) ? "0" : "1"));
                lineItem.AppendFormat(@"    <td class=""report_market_{0}"" onclick=""showOrhide('{1}');"" style=""border-left:0px;cursor:pointer;text-align:left;"">", level, uniqueDivId);
                lineItem.AppendFormat(@"        <img id=""{0}_img"" src=""/Images/flash/arrow_right_blue.gif"" />&nbsp;{1}", uniqueDivId, displayText);
                lineItem.Append(@"    </td>");
                switch (this.quarter)
                {
                    case 1:
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", filterExpression))));
                        break;
                    case 2:
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", filterExpression))));
                        break;
                    case 3:
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", filterExpression))));
                        break;
                    case 4:
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", filterExpression))));
                        break;
                    default:
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", filterExpression))));
                        lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"" style=""border-right:0px;"">{0}</td>", DrillDownReport.DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([TOTAL BILLING])", filterExpression))));
                        break;
                }
                lineItem.AppendLine(@"</tr>");
                switch (this.quarter)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        lineItem.AppendFormat(@"<tr><td colspan=""6"">{0}</td></tr>", BuildSubTableRow(rowToAppend, uniqueDivId));
                        break;
                    default:
                        lineItem.AppendFormat(@"<tr><td colspan=""19"">{0}</td></tr>", BuildSubTableRow(rowToAppend, uniqueDivId));
                        break;
                }
            }
            else
            {
                if (level == 0)
                {
                    lineItem.Append(@"     <tr valign=""top"">");
                    switch (this.quarter)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            lineItem.Append(@"         <td colspan=""8"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>" + displayText + "</b></td>");
                            break;
                        default:
                            lineItem.Append(@"         <td colspan=""21"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>" + displayText + "</b></td>");
                            break;
                    }
                    lineItem.AppendLine(@"     </tr>");
                }
                lineItem.Append(rowToAppend.ToString());
                lineItem.Append(@"     <tr valign=""top"">");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""right"" colspan=""4""><b>" + ((showContracts) ? "TOTAL " : "") + displayField + ": " + displayText + "</b></td>");
                switch (this.quarter)
                {
                    case 1:
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        break;
                    case 2:
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        break;
                    case 3:
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        break;
                    case 4:
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        break;
                    default:
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JANUARY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([FEBRUARY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MARCH BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q1 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([APRIL BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([MAY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JUNE BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q2 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([JULY BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([AUGUST BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SEPTEMBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q3 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([OCTOBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([NOVEMBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([DECEMBER BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Q4 BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;"" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([TOTAL BILLING])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                        break;
                }
                lineItem.AppendLine(@"     </tr>");
            }
            return lineItem.ToString();
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class RevenueFlashReport : DrillDownReport
    {

        #region Member variables
        /// <summary>TBD</summary>
        private DateTime startDate;
        #endregion

        #region Parameterized constructor (reportData, sumFields, startDate, market, companyId)
        /// <summary>TBD</summary>
        /// <param name="reportData">TBD</param>
        /// <param name="sumFields">TBD</param>
        /// <param name="startDate">TBD</param>
        /// <param name="market">TBD</param>
        /// <param name="companyId">TBD</param>
        public RevenueFlashReport(DataSet reportData, string[] sumFields, DateTime startDate, string market, string companyId)
            : base(reportData, sumFields, market, companyId)
        {
            this.startDate = startDate;
        }
        #endregion

        #region BuildColumnHeader method
        /// <summary>TBD</summary>
        /// <param name="offSet">TBD</param>
        /// <param name="isSdly">TBD</param>
        /// <param name="current">TBD</param>
        /// <returns>TBD</returns>
        private string BuildColumnHeader(int offSet, bool isSdly, out bool current)
        {
            current = (!isSdly) && (startDate.AddMonths(offSet).Month == DateTime.Now.Month && startDate.AddMonths(offSet).Year == DateTime.Now.Year);
            string showMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.AddMonths(offSet).Month).Substring(0, 3);
            string showYear = Convert.ToString(startDate.AddMonths(offSet).Year - ((isSdly) ? 1 : 0)).Substring(2, 2);
            return string.Format("{0} {1}", showMonth, showYear);
        }
        #endregion

        #region BuildExcelReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="showContracts">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildExcelReportTableHeaders(bool showContracts)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.AppendLine(@"<table cellspacing=""1"" cellpadding=""0"" border=""1"" align=""center"">");
            recordDisplay.AppendFormat(@"     <tr><td colspan=""29"" height=""55"" align=""center""><img src=""http://{0}/Images/flash/titan_report_logo.gif"" alt=""Titan 360"" /></td></tr>", HttpContext.Current.Request.Url.Host);
            recordDisplay.AppendLine(@"     <tr><td colspan=""29"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_)""><b>Revenue Flash Report</b></td></tr>");
            recordDisplay.AppendLine(@"     <tr nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_);"">");
            if (showContracts)
            {
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Contract</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>AE</b></td>");
                recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""left""><b>Advertiser</b></td>");
            }
            else
            {
                recordDisplay.AppendLine(@"         <td colspan=""5"">&nbsp;</td>");
            }
            bool isCurrent;
            string headerText;
            for (int i = 0; i < 12; i++)
            {
                headerText = BuildColumnHeader(i, false, out isCurrent);
                recordDisplay.AppendFormat(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", headerText);
                headerText = BuildColumnHeader(i, true, out isCurrent);
                recordDisplay.AppendFormat(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", headerText);
            }
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>CDTY Total</b></td>");
            recordDisplay.AppendLine(@"         <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>SDLY Total</b></td>");
            recordDisplay.AppendLine(@"     </tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildReportTableHeaders method
        /// <summary>TBD</summary>
        /// <param name="tableCount">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildReportTableHeaders(int tableCount)
        {
            StringBuilder recordDisplay = new StringBuilder();
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td></td>");
            recordDisplay.Append(@"     <td colspan='27' style=""text-align:center;"">");
            recordDisplay.Append(@"         <table width=""100%"" cellpadding='0' cellspacing='0' border='0' bgcolor=""#ffffff"">");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_left_corner.gif""></td>");
            recordDisplay.AppendFormat(@"                 <td class=""report_heading"" nowrap background=""/Images/flash/blue_thick_bar.gif"">&nbsp;&nbsp;{0}&nbsp;&nbsp;</td>", (String.IsNullOrEmpty(this.marketName) ? string.Format("All Markets {0}", salesType) : this.marketName));
            recordDisplay.Append(@"                 <td class=""report_heading"" nowrap><img src=""/Images/flash/blue_thick_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"                 <td class=""report_heading"" width=""50%"">&nbsp;</td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr bgcolor=""#ffffff"">");
            recordDisplay.Append(@"     <td></td>");
            recordDisplay.Append(@"     <td colspan='27' align=""right"">");
            recordDisplay.Append(@"         <table width='100%' cellpadding='0' cellspacing='0' border='0'>");
            recordDisplay.Append(@"             <tr>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_left_corner.gif""></td>");
            recordDisplay.Append(@"                 <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
            recordDisplay.Append(@"                 <td><img src=""/Images/flash/blue_bar_right_corner.gif""></td>");
            recordDisplay.Append(@"             </tr>");
            recordDisplay.Append(@"         </table>");
            recordDisplay.Append(@"     </td>");
            recordDisplay.Append(@"</tr>");
            recordDisplay.Append(@"<tr>");
            recordDisplay.Append(@"     <td bgcolor=""#ffffff"" style=""color:#666699;font-weight:bold;text-align:left;"">");
            recordDisplay.AppendFormat(@"         <img src=""/Images/but_excel.gif"" border=0 style=""cursor:pointer;"" onclick=""openexcel({0});"">", 1);
            recordDisplay.Append(@"         <br/>");
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""expandAll('{0}');"">Expand All</a>", salesTypeId);
            recordDisplay.AppendFormat(@"         <a href=""#"" onclick=""collapseAll('{0}');"">Collapse All</a>", salesTypeId);
            recordDisplay.Append(@"    </td>");
            recordDisplay.Append(@"    <td>");
            bool isCurrent;
            string headerText;
            for (int i = 0; i < 12; i++)
            {
                headerText = BuildColumnHeader(i, false, out isCurrent);
                recordDisplay.AppendFormat(@"     <td class=""dashboard_column_head{0}"">{1}</td>", ((isCurrent) ? "_current" : ""), headerText);
                headerText = BuildColumnHeader(i, true, out isCurrent);
                recordDisplay.AppendFormat(@"     <td class=""dashboard_column_head{0}"">{1}</td>", ((isCurrent) ? "_current" : ""), headerText);
            }
            recordDisplay.Append(@"     <td width=""3%"" class=""dashboard_column_head"">CDTY Total</td>");
            recordDisplay.Append(@"     <td width=""3%"" class=""dashboard_column_head"">SDLY Total</td>");
            recordDisplay.AppendLine(@"</tr>");
            return recordDisplay.ToString();
        }
        #endregion

        #region BuildSubTableRow method
        /// <summary>TBD</summary>
        /// <param name="subTable">TBD</param>
        /// <param name="divId">TBD</param>
        /// <returns>TBD</returns>
        public override string BuildSubTableRow(StringBuilder subTable, string divId)
        {
            StringBuilder subTableRow = new StringBuilder();
            subTableRow.AppendFormat(@"<div class=""{0}"" id=""{1}"" style=""display:none"">", salesTypeId, divId);
            subTableRow.Append(@"  <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" bgcolor=""#ededed"" style=""border-collapse: collapse;border-spacing: 0px;border-width:0px;"">");
            subTableRow.Append(@"      <tr style=""height:0px;background-color:#ffffff;"">");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" width=""15%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""192""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""61""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""67""></td>");
            subTableRow.Append(@"          <td style=""background-color:#ffffff;"" align=""right"" width=""3%""><img src=""/Images/flash/spacer.gif"" height=""0"" width=""67""></td>");
            subTableRow.Append(@"      </tr>");
            subTableRow.Append(subTable.ToString());
            subTableRow.Append(@"  </table>");
            subTableRow.AppendLine(@"</div>");
            return subTableRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (forExcel)
        /// <summary>TBD</summary>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            string previousContractNumber = "";
            string currentContractNumber = "";
            string filterExpression = GetContractSumFilterExpression(sumFields, currentSumOnValues);
            foreach (DataRow row in this.reportDataSorted.Select(filterExpression))
            {
                currentContractNumber = (string)row["Contract Number"];
                if (previousContractNumber != currentContractNumber)
                {
                    if (!forExcel)
                    {
                        contractRow.AppendFormat(@"<tr class=""contract_item_row"" align=""right"" ondblclick=""ShowContractDetail({0},{1});"" onclick=""ShowContractDetail({0},{1});"" onmouseover=""mouseincolor(this);"" onmouseout=""mouseoutcolor(this);"">", currentContractNumber, this.companyId);
                        contractRow.AppendFormat(@"     <td class=""contract_item_row_text"">{0} - {1}&nbsp;</td>", (string)row["Contract Number"], (string)row["ADVERTISER"]);
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"     <td>" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.AppendLine("</tr>");
                    }
                    else
                    {
                        if (sumFields.Length == 0)
                        {
                            //filterExpression = string.Format("PROFIT_CENTER = '{0}' AND AE = '{1}'", (string)row["PROFIT_CENTER"], (string)row["AE"]);
                            filterExpression = string.Format("AE = '{0}'", (string)row["AE"]);
                        }
                        contractRow.Append(@"      <tr valign=""top"">");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">" + (row["Contract Number"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["AE"] as string ?? "&nbsp;") + "</td>");
                        //contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["AGENCY"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["ADVERTISER"] as string ?? "&nbsp;") + "</td>");
                        //contractRow.Append(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">" + (row["PROGRAM"] as string ?? "&nbsp;") + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([CDTY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.Append(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">" + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SDLY Total])", (String.IsNullOrEmpty(filterExpression) ? "" : filterExpression + " AND ") + "[Contract Number] = '" + currentContractNumber + "'")), false, false) + "</td>");
                        contractRow.AppendLine("       </tr>");
                    }
                }
                previousContractNumber = currentContractNumber;
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateContractRow method (row, forExcel)
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <param name="forExcel">TBD</param>
        /// <returns>TBD</returns>
        public override string GenerateContractRow(DataRow row, bool forExcel)
        {
            StringBuilder contractRow = new StringBuilder();
            if (!forExcel)
            {
                contractRow.AppendFormat(@"<tr class=""contract_item_row"" align=""right"" style=""cursor:pointer;background-color:#ffffff;color:#2f4070"" ondblclick=""ShowContractDetail({0},{1});"" onclick=""ShowContractDetail({0},{1});"" onmouseover=""mouseincolor(this);"" onmouseout=""mouseoutcolor(this);"">", Convert.ToString(row["Contract Number"]), this.companyId);
                contractRow.AppendFormat(@"     <td class=""contract_item_row_text"">{0} - {1}&nbsp;</td>", Convert.ToString(row["Contract Number"]), (string)row["ADVERTISER"]);
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jan CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jan SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Feb CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Feb SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Mar CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Mar SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Apr CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Apr SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["May CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["May SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jun CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jun SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jul CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jul SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Aug CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Aug SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Sep CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Sep SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Oct CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Oct SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Nov CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Nov SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Dec CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["Dec SDLY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["CDTY Total"]), false, false));
                contractRow.AppendFormat(@"     <td>{0}</td>", DoubleDisplay(Convert.ToDouble(row["SDLY Total"]), false, false));
                contractRow.AppendLine(@"</tr>");
            }
            else
            {
                contractRow.Append(@"      <tr valign=""top"">");
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:@"" align=""left"">{0}</td>", (row["Contract Number"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["AE"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_)"" align=""left"">{0}</td>", (row["ADVERTISER"] as string ?? "&nbsp;"));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jan CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jan SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Feb CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Feb SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Mar CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Mar SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Apr CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Apr SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["May CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["May SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jun CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jun SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jul CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Jul SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Aug CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Aug SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Sep CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Sep SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Oct CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Oct SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Nov CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Nov SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Dec CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["Dec SDLY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["CDTY Total"]), false, false));
                contractRow.AppendFormat(@"          <td nowrap valign=middle align=""right"" style=""font-size:8pt;"">{0}</td>", DoubleDisplay(Convert.ToDouble(row["SDLY Total"]), false, false));
                contractRow.AppendLine("       </tr>");
            }
            return contractRow.ToString();
        }
        #endregion

        #region GenerateGrandTotalRow method
        /// <summary>TBD</summary>
        /// <param name="forExcel">Flag for determining if this report is being Exported to Excel</param>
        /// <returns>TBD</returns>
        public override string GenerateGrandTotalRow(bool forExcel)
        {
            StringBuilder grandTotal = new StringBuilder();
            if (!forExcel)
            {
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td class=""grand_total_text"">Grand Total&nbsp;</td>");
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td class=""grand_total_column"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SDLY Total])", ""))));
                grandTotal.AppendLine(@"</tr>");
                grandTotal.Append(@"<tr>");
                grandTotal.Append(@"    <td>&nbsp;</td>");
                grandTotal.Append(@"    <td colspan=""27"" valign=""top"" style=""border-top:1px solid #ffffff;"">");
                grandTotal.Append(@"        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">");
                grandTotal.Append(@"            <tr>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_left_flip.gif""></td>");
                grandTotal.Append(@"                <td width=""100%"" background=""/Images/flash/blue_bar.gif""></td>");
                grandTotal.Append(@"                <td><img src=""/Images/flash/blue_bar_right_corner_flip.gif""></td>");
                grandTotal.Append(@"            </tr>");
                grandTotal.Append(@"        </table>");
                grandTotal.Append(@"    </td>");
                grandTotal.AppendLine(@"</tr>");
            }
            else
            {
                grandTotal.Append(@"    <tr valign=""top"">");
                grandTotal.Append(@"        <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);color:#ffffff;background-color:#00CCFF"" align=""right"" colspan=""5""><b>Grand Total&nbsp;</b></td>");
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec SDLY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([CDTY Total])", ""))));
                grandTotal.AppendFormat(@"    <td nowrap valign=middle style=""font-size:8pt;color:#ffffff;background-color:#00CCFF"" align=""right""><b>{0}</b></td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SDLY Total])", ""))));
                grandTotal.Append(@"    </tr>");
                grandTotal.AppendLine(@"</table>");
            }
            return grandTotal.ToString();
        }
        #endregion

        #region GenerateLineItemRow method
        /// <summary>Generates a row in the Grouping report</summary>
        /// <param name="level">Determines which level in the Grouping this row will fall in</param>
        /// <param name="count">Used for formatting the style of the row</param>
        /// <param name="forExcel">Flag for determining if this will be exported to Excel</param>
        /// <param name="showContracts">Flag for determining if the Excel export will display contracts</param>
        /// <returns>A formatted HTML Table Row containing Contract info</returns>
        public override string GenerateLineItemRow(int level, int count, bool forExcel, bool showContracts)
        {
            StringBuilder lineItem = new StringBuilder();
            string uniqueDivId = GenerateUniqueDivId();
            string displayText = this.previousSumOnValues[level];
            string displayField = this.sumFields[level].Replace("_", " ");
            StringBuilder rowToAppend = reportBuilder[level + 1];
            string filterExpression = GetSumFilterExpression(sumFields, previousSumOnValues, level);
            if (!forExcel)
            {
                lineItem.AppendFormat(@"<tr class=""forecast_report_{0}_total{1}"">", level, ((count % 2 == 0) ? "0" : "1"));
                lineItem.AppendFormat(@"    <td class=""report_market_{0}"" onclick=""showOrhide('{1}');"" style=""border-left:0px;cursor:pointer;text-align:left;"">", level, uniqueDivId);
                lineItem.AppendFormat(@"        <img id=""{0}_img"" src=""/Images/flash/arrow_right_blue.gif"" />&nbsp;{1}", uniqueDivId, displayText);
                lineItem.Append(@"    </td>");
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec SDLY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"" style=""border-right:0px;"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([CDTY Total])", filterExpression))));
                lineItem.AppendFormat(@"    <td class=""report_cell"" align=""right"" style=""border-right:0px;"">{0}</td>", DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SDLY Total])", filterExpression))));
                lineItem.AppendLine(@"</tr>");
                lineItem.AppendFormat(@"<tr><td colspan=""27"">{0}</td></tr>", BuildSubTableRow(rowToAppend, uniqueDivId));
            }
            else
            {
                if (level == 0 && this.sumFields.Length > 1)
                {
                    lineItem.Append(@"     <tr valign=""top"">");
                    lineItem.Append(@"         <td colspan=""27"" nowrap valign=middle style=""vnd.ms-excel.numberformat:_(@_);" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @"""><b>" + displayText + "</b></td>");
                    lineItem.AppendLine(@"     </tr>");
                }
                lineItem.Append(rowToAppend.ToString());
                lineItem.Append(@"     <tr valign=""top"">");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;vnd.ms-excel.numberformat:_(@_);" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"" colspan=""5""><b>" + ((showContracts) ? "TOTAL " : "") + displayField + ": " + displayText + "</b></td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jan SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Feb SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Mar SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Apr SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([May SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jun SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Jul SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Aug SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Sep SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Oct SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Nov SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([Dec SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([CDTY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.Append(@"         <td nowrap valign=middle style=""font-size:8pt;" + ((showContracts) ? "color:#ffffff;background-color:#00CCFF" : "") + @""" align=""right"">" + ((showContracts) ? "<b>" : "") + DoubleDisplay(Convert.ToDouble(this.reportDataSorted.Compute("Sum([SDLY Total])", filterExpression))) + ((showContracts) ? "</b>" : "") + @"</td>");
                lineItem.AppendLine(@"     </tr>");
            }
            return lineItem.ToString();
        }
        #endregion

    }
}
