<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="contract_audit_details.aspx.cs" Inherits="Apollo.quattro_contract_audit_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server" />
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <style type="text/css">
        .ColTable td {text-align:left !important;}
        .ui-jqgrid .ui-jqgrid-htable th div {
            height:auto;
            overflow:hidden;
            padding-right:4px;
            padding-top:2px;
            position:relative;
            vertical-align:text-top;
            white-space:normal !important;
        }
        .ui-jqgrid tr.jqgrow td {
            white-space: normal !important;
            height:auto;
            vertical-align:text-top;
            padding-top:2px;
        }
    </style>   
    <script type="text/javascript" language="javascript">
        var gridsLoaded = false;
        $(document).ready(function() {
            $('#reportSelectionArea').accordion({ active: false, collapsible: true });
            $(".button").button();            
            $("#contractNumber").val('');
        });
        function GetAuditDetails() {
            //Get the User-Entered contract number
            var contractNumber = $("#contractNumber").val();
            var companyId = $("#dropDownCompany").val();
            if (contractNumber == '') {
                alert('Please enter a Contract Number');
                return;
            }
            if (!gridsLoaded) {
                //This is the first time a search has been executed on the page
                //  Add the Grids to the DOM
                LoadCampaignAuditGrid(contractNumber, companyId);
                LoadInvoiceAuditGrid(contractNumber, companyId);
                LoadInvoiceLineAuditGrid(contractNumber, companyId);
                LoadInvoiceLineGlSplitAuditGrid(contractNumber, companyId);
                $('#reportSelectionArea').accordion("activate", 0);
                gridsLoaded = true;
            } else {
                //The user is executing another search
                //  Update the grids
                $("#contractAuditList").setGridParam({ postData: { contractNumber: contractNumber, companyId: companyId} }).trigger('reloadGrid');
                $("#invoiceAuditList").setGridParam({ postData: { contractNumber: contractNumber, companyId: companyId} }).trigger('reloadGrid');
                $("#invoiceLineAuditList").setGridParam({ postData: { contractNumber: contractNumber, companyId: companyId} }).trigger('reloadGrid');
                $("#invoiceLineGlSplitAuditList").setGridParam({ postData: { contractNumber: contractNumber, companyId: companyId} }).trigger('reloadGrid');
            }
        }
        function LoadCampaignAuditGrid(contract, companyId) {
            $("#contractAuditList").jqGrid({
                url: '../services/QuattroService.asmx/GetCampaignAuditGrid'
                , datatype: "xml"
                , postData: { contractNumber: contract, companyId: companyId }
                , colNames: ['uid_cmpgn', 'Contract #', 'Brand Code', 'Brand Desc', 'Product Code'
                    , 'Product Name', 'Parent Product Code', 'Parent Product', 'Advertiser ID', 'Agency ID'
                    , 'Buying Service', 'Contact', 'Is Speculative', 'Is Campaign Finished', 'Is Campaign Posted'
                    , 'Is Public Service', 'Campaign Start', 'Campaign Finish', 'Campaign Name', 'Their Ref. #'
                    , 'Is Cancelled', 'Campaign Cancel Reason Code', 'Campaign Cancel Reason', 'Is Credit Approved'
                    , 'Ethnic Group Code', 'Ethnic Group', 'Campaign Type Code', 'Campaign Type', 'AE 1', 'AE 2', 'AE 3'
                    , 'Date Contract Signed', 'Access Group Code', 'Access Group', 'Contract Name', 'Production Contract Name'
                    , 'Agency Commission Changed?', 'Buying Service Commission Changed?', 'Invoice To', 'Is Cancel Allowed'
                    , 'Cancel Notice Months', 'Agency Commission %', 'Buying Service Commission %', 'Use Buying Service as Agency?'
                    , 'Is Production Included on Contract', 'Is Used in Digital', 'Changed By', 'Date Action Taken'
                    , 'Action Taken']
                , colModel: [{ name: 'uidCmpgn', index: 'uid_cmpgn', hidden: true, sortable: false }
                    , { name: 'contractNumber', index: 'Contract Number', width: 75, sortable: false, align: 'center' }
                    , { name: 'brandCode', index: 'Brand Code', width: 75, sortable: false }
                    , { name: 'brandDesc', index: 'Brand Desc', width: 75, sortable: false }
                    , { name: 'productCode', index: 'Product Code', width: 75, sortable: false }
                    , { name: 'productName', index: 'Product Name', width: 75, sortable: false }
                    , { name: 'parentProductCode', index: 'Parent Product Code', width: 75, sortable: false }
                    , { name: 'parentProduct', index: 'Parent Product', width: 75, sortable: false }
                    , { name: 'advertiserId', index: 'Advertiser ID', width: 75, sortable: false }
                    , { name: 'agencyId', index: 'Agency ID', width: 75, sortable: false }
                    , { name: 'buyingService', index: 'Buying Service', width: 75, sortable: false }
                    , { name: 'contactName', index: 'Contact', width: 75, sortable: false }
                    , { name: 'isSpeculative', index: 'Is Speculative', width: 50, sortable: false }
                    , { name: 'isCampaignFinished', index: 'Is Campaign Finished', sortable: false, width: 50, align: 'center' }
                    , { name: 'isCampaignPosted', index: 'Is Campaign Posted', sortable: false, width: 50, align: 'center' }
                    , { name: 'isPublicService', index: 'Is Public Service', sortable: false, width: 50, align: 'center' }
                    , { name: 'campaignStart', index: 'Campaign Start', width: 75, sortable: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: 'campaignFinish', index: 'Campaign Finish', width: 75, sortable: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: 'campaignName', index: 'Campaign Name', width: 75, sortable: false }
                    , { name: 'theirRefNo', index: 'Their Ref. No.', width: 75, sortable: false }
                    , { name: 'isCancelled', index: 'Is Cancelled', sortable: false, width: 50, align: 'center' }
                    , { name: 'campaignCancelReasonCode', index: 'Campaign Cancel Reason Code', width: 75, sortable: false }
                    , { name: 'campaignCancelReason', index: 'Campaign Cancel Reason', width: 75, sortable: false }
                    , { name: 'isCreditApproved', index: 'Is Credit Approved', sortable: false, width: 50, align: 'center' }
                    , { name: 'ethnicGroupCode', index: 'Ethnic Group Code', width: 75, sortable: false }
                    , { name: 'ethnicGroup', index: 'Ethnic Group', width: 75, sortable: false }
                    , { name: 'campaignTypeCode', index: 'Campaign Type Code', width: 75, sortable: false }
                    , { name: 'campaignType', index: 'Campaign Type', width: 75, sortable: false }
                    , { name: 'ae1', index: 'AE 1', width: 75, sortable: false }
                    , { name: 'ae2', index: 'AE 2', width: 75, sortable: false }
                    , { name: 'ae3', index: 'AE 3', width: 75, sortable: false }
                    , { name: 'dateContractSigned', index: 'Date Contract Signed', width: 75, sortable: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: 'accessGroupCode', index: 'Access Group Code', width: 75, sortable: false }
                    , { name: 'accessGroup', index: 'Access Group', width: 75, sortable: false }
                    , { name: 'contractName', index: 'Contract Name', width: 75, sortable: false }
                    , { name: 'productionContractName', index: 'Production Contract Name', width: 75, sortable: false }
                    , { name: 'agencyCommissionChanged', index: 'Agency Commission Changed?', width: 50, sortable: false, align: 'center' }
                    , { name: 'buyingServiceCommission Changed?', index: 'Buying Service Commission Changed?', width: 50, sortable: false, align: 'center' }
                    , { name: 'invoiceTo', index: 'Invoice To', width: 75, sortable: false }
                    , { name: 'isCancelAllowed', index: 'Is Cancel Allowed', sortable: false, width: 50, align: 'center' }
                    , { name: 'cancelNoticeMonths', index: 'Cancel Notice Months', width: 75, sortable: false }
                    , { name: 'agencyCommission', index: 'Agency Commission %', width: 50, sortable: false, align: 'right' }
                    , { name: 'buyingServiceCommission', index: 'Buying Service Commission %', width: 50, sortable: false, align: 'right' }
                    , { name: 'useBuyingServiceAgency', index: 'Use Buying Service as Agency?', sortable: false, width: 50, align: 'center' }
                    , { name: 'isProductionIncludedOnContract', index: 'Is Production Included on Contract', sortable: false, width: 50, align: 'center' }
                    , { name: 'isUsedInDigital', index: 'Is Used in Digital', sortable: false, width: 50, align: 'center' }
                    , { name: 'changedBy', index: 'Changed By', width: 75, sortable: false, width: 100 }
                    , { name: 'dateActionTaken', index: 'Date Action Taken', sortable: false, align: 'right', width: 85 }
                    , { name: 'actionTaken', index: 'Action Taken', sortable: false, width: 50, align: 'center' }
                ]
                , rowNum: 50
                , height: 350
                , width: 900
                , rowList: [25, 50, 100]
                , pager: '#contractAuditPager'
                , caption: "Contract Audit Details"
                , viewrecords: true
                , shrinkToFit: false
            });
            $("#contractAuditList").jqGrid('navGrid', "#contractAuditPager", { edit: false, add: false, del: false, search: false }).navButtonAdd("#contractAuditPager",
                { caption: 'Toggle Columns', onClickButton: function() {
                    $("#contractAuditList").setColumns({ caption: 'Show/Hide Columns'
                        , bSubmit: 'Update', bCancel: 'Cancel', colnameview: false
                        , drag: false, ShrinkToFit: false
                    });
                    return false;
                }
            });
        }
        function LoadInvoiceAuditGrid(contract, companyId) {
            $("#invoiceAuditList").jqGrid({
                url: '../services/QuattroService.asmx/GetInvoiceAuditGrid'
                , datatype: "xml"
                , postData: { contractNumber: contract, companyId: companyId }
                , colNames: ['uid_invoice', 'str_operator_code', 'Invoice To', 'uid_cmpgn', 'Invoice #', 'Invoice To Name'
                    , 'Invoice To Contact', 'Invoice To Address 1', 'Invoice To Address 2', 'Invoice To Address 3', 'Invoice To Address 4'
                    , 'City', 'State', 'Zip Code', 'Country', 'Billing Terms Code', 'Billing Terms', 'Invoice Type Code', 'Invoice Type'
                    , 'Client Ref. #', 'Message', 'Gross $', 'Tax $', 'Commission $', 'Deduction $', 'Net $'
                    , 'Pre Paid $', 'Is Client Copy', 'Is Pre Paid', 'Is On Hold', 'Period From', 'Period To', 'Date Passed to Accounts'
                    , 'Date Printed', 'Invoice Date', 'AE 1', 'AE 2', 'AE 3', 'Is Local Sale', 'Has Segment Change', 'Date Commission Passed to Accounts'
                    , 'Print Net Amount Only?', 'Print Display Panel?', 'Report', 'Date Revenue Closed', 'Is Revenue Split Evenly', 'Reconciled?'
                    , 'GST Exempt On Close?', 'PST Exempt On Close?', 'QST Exempt On Close?', 'Print Location List?', 'Product Code', 'Product'
                    , 'Parent Product Code', 'Parent Product', 'Credit Card?', 'Print Period From', 'Print Period To', 'Pre Paid Comment', 'Changed By'
                    , 'Date Action Taken', 'Action Taken']
                , colModel: [{ name: 'uid_invoice', index: 'uid_invoice', hidden: true, sortable: false }
                    , { name: 'str_operator_code', index: 'str_operator_code', hidden: true, sortable: false }
                    , { name: 'InvoiceTo', index: 'Invoice To', hidden: true, sortable: false }
                    , { name: 'uid_cmpgn', index: 'uid_cmpgn', hidden: true, sortable: false }
                    , { name: 'InvoiceNumber', index: 'Invoice Number', width: 50, sortable: false, align: 'center' }
                    , { name: 'InvoiceToName', index: 'Invoice To Name', sortable: false }
                    , { name: 'InvoiceToContact', index: 'Invoice To Contact', sortable: false }
                    , { name: 'InvoiceToAddress1', index: 'Invoice To Address 1', sortable: false }
                    , { name: 'InvoiceToAddress2', index: 'Invoice To Address 2', sortable: false }
                    , { name: 'InvoiceToAddress3', index: 'Invoice To Address 3', sortable: false }
                    , { name: 'InvoiceToAddress4', index: 'Invoice To Address 4', sortable: false }
                    , { name: 'City', index: 'City', sortable: false }
                    , { name: 'State', index: 'State', sortable: false }
                    , { name: 'ZipCode', index: 'Zip Code', sortable: false }
                    , { name: 'Country', index: 'Country', sortable: false }
                    , { name: 'BillingTermsCode', index: 'Billing Terms Code', sortable: false }
                    , { name: 'BillingTerms', index: 'Billing Terms', sortable: false }
                    , { name: 'InvoiceTypeCode', index: 'Invoice Type Code', sortable: false }
                    , { name: 'InvoiceType', index: 'Invoice Type', sortable: false }
                    , { name: 'ClientRef.No.', index: 'Client Ref. No.', sortable: false }
                    , { name: 'Message', index: 'Message', sortable: false }
                    , { name: 'GrossAmount', index: 'Gross Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'TaxAmount', index: 'Tax Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'CommissionAmount', index: 'Commission Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'DeductionAmount', index: 'Deduction Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'NetAmount', index: 'Net Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'PrePaidAmount', index: 'Pre Paid Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'IsClientCopy', index: 'Is Client Copy', sortable: false, width: 50, align: 'center' }
                    , { name: 'IsPrePaid', index: 'Is Pre Paid', sortable: false, width: 50, align: 'center' }
                    , { name: 'IsOnHold', index: 'Is On Hold', sortable: false, width: 50, align: 'center' }
                    , { name: 'PeriodFrom', index: 'Period From', width: 50, sortable: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: 'PeriodTo', index: 'Period To', width: 50, sortable: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: 'DatePassedtoAccounts', index: 'Date Passed to Accounts', sortable: false, align: 'right' }
                    , { name: 'DatePrinted', index: 'Date Printed', sortable: false, align: 'right' }
                    , { name: 'InvoiceDate', index: 'Invoice Date', width: 50, sortable: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y' }, align: 'right' }
                    , { name: 'AE1', index: 'AE 1', sortable: false }
                    , { name: 'AE2', index: 'AE 2', sortable: false }
                    , { name: 'AE3', index: 'AE 3', sortable: false }
                    , { name: 'IsLocalSale', index: 'Is Local Sale', sortable: false, width: 50, align: 'center' }
                    , { name: 'HasSegmentChange', index: 'Has Segment Change', sortable: false, width: 50, align: 'center' }
                    , { name: 'DateCommissionPassedtoAccounts', index: 'Date Commission Passed to Accounts', sortable: false, align: 'right' }
                    , { name: 'PrintNetAmountOnly?', index: 'Print Net Amount Only?', sortable: false, width: 50, align: 'center' }
                    , { name: 'PrintDisplayPanel?', index: 'Print Display Panel?', sortable: false, width: 50, align: 'center' }
                    , { name: 'Report', index: 'Report', sortable: false }
                    , { name: 'DateRevenueClosed', index: 'Date Revenue Closed', sortable: false, align: 'right' }
                    , { name: 'IsRevenueSplitEvenly', index: 'Is Revenue Split Evenly', sortable: false, width: 50, align: 'center' }
                    , { name: 'Reconciled?', index: 'Reconciled?', sortable: false, width: 50, align: 'center' }
                    , { name: 'GSTExemptOnClose?', index: 'GST Exempt On Close?', sortable: false, width: 50, align: 'center', hidden: true }
                    , { name: 'PSTExemptOnClose?', index: 'PST Exempt On Close?', sortable: false, width: 50, align: 'center', hidden: true }
                    , { name: 'QSTExemptOnClose?', index: 'QST Exempt On Close?', sortable: false, width: 50, align: 'center', hidden: true }
                    , { name: 'PrintLocationList?', index: 'Print Location List?', sortable: false, width: 50, align: 'center' }
                    , { name: 'ProductCode', index: 'Product Code', sortable: false }
                    , { name: 'Product', index: 'Product', sortable: false }
                    , { name: 'ParentProductCode', index: 'Parent Product Code', sortable: false }
                    , { name: 'ParentProduct', index: 'Parent Product', sortable: false }
                    , { name: 'CreditCard?', index: 'Credit Card?', sortable: false, width: 50, align: 'center' }
                    , { name: 'PrintPeriodFrom', index: 'Print Period From', sortable: false, align: 'right' }
                    , { name: 'PrintPeriodTo', index: 'Print Period To', sortable: false, align: 'right' }
                    , { name: 'PrePaidComment', index: 'Pre Paid Comment', sortable: false }
                    , { name: 'ChangedBy', index: 'Changed By', sortable: false, width: 100 }
                    , { name: 'DateActionTaken', index: 'Date Action Taken', sortable: false, align: 'right', width: 85 }
                    , { name: 'ActionTaken', index: 'Action Taken', sortable: false, width: 50, align: 'center' }
                ]
                , rowNum: 50
                , height: 350
                , width: 900
                , rowList: [25, 50, 100]
                , pager: '#invoiceAuditPager'
                , caption: "Invoice Audit Details"
                , viewrecords: true
                , shrinkToFit: false
            });
            $("#invoiceAuditList").jqGrid('navGrid', "#invoiceAuditPager", { edit: false, add: false, del: false, search: false }).navButtonAdd("#invoiceAuditPager",
                { caption: 'Toggle Columns', onClickButton: function() {
                    $("#invoiceAuditList").setColumns({ caption: 'Show/Hide Columns'
                        , bSubmit: 'Update', bCancel: 'Cancel', colnameview: false
                        , drag: false, ShrinkToFit: false
                    });
                    return false;
                }
            });
        }
        function LoadInvoiceLineAuditGrid(contract, companyId) {
            $("#invoiceLineAuditList").jqGrid({
                url: '../services/QuattroService.asmx/GetInvoiceLineAuditGrid'
                , datatype: "xml"
                , postData: { contractNumber: contract, companyId: companyId }
                , colNames: ['uid_invoice_line', 'Invoice #', 'Line #', 'Cost Code', 'Cost Code Desc'
                    , 'Tax Struct.', 'uid_invoice', 'Line Message', 'Quantity', 'Gross $', 'Tax $'
                    , 'Commission $', 'Deduction $', 'Net $', 'Commission #', 'Changed By'
                    , 'Date Action Taken', 'Action Taken']
                , colModel: [{ name: 'uid_invoice_line', index: 'uid_invoice_line', hidden: true, sortable: false }
                    , { name: 'InvoiceNumber', index: 'Invoice Number', sortable: false, width: 50, align: 'center' }
                    , { name: 'LineNumber', index: 'Line Number', sortable: false, width: 25, align: 'center' }
                    , { name: 'CostCode', index: 'Cost Code', sortable: false, align: 'center', width: 110 }
                    , { name: 'CostCodeDesc', index: 'Cost Code Desc', sortable: false, align: 'center' }
                    , { name: 'TaxStruct.', index: 'Tax Struct.', sortable: false }
                    , { name: 'uid_invoice', index: 'uid_invoice', hidden: true, sortable: false }
                    , { name: 'LineMessage', index: 'Line Message', sortable: false }
                    , { name: 'Quantity', index: 'Quantity', sortable: false, align: 'right', width: 50 }
                    , { name: 'GrossAmount', index: 'Gross Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'TaxAmount', index: 'Tax Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'CommissionAmount', index: 'Commission Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'DeductionAmount', index: 'Deduction Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'NetAmount', index: 'Net Amount', width: 100, sortable: false, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "$", suffix: "", defaultValue: '0.00'} }
                    , { name: 'CommissionNumber', index: 'Commission Number', sortable: false }
                    , { name: 'ChangedBy', index: 'Changed By', sortable: false, width: 100 }
                    , { name: 'DateActionTaken', index: 'Date Action Taken', sortable: false, align: 'right', width: 85 }
                    , { name: 'ActionTaken', index: 'Action Taken', sortable: false, width: 50, align: 'center' }
                ]
                , rowNum: 50
                , height: 350
                , width: 900
                , rowList: [25, 50, 100]
                , pager: '#invoiceLineAuditPager'
                , caption: "Invoice Line Audit Details"
                , viewrecords: true
                , shrinkToFit: false
            });
            $("#invoiceLineAuditList").jqGrid('navGrid', "#invoiceLineAuditPager", { edit: false, add: false, del: false, search: false }).navButtonAdd("#invoiceLineAuditPager",
                { caption: 'Toggle Columns', onClickButton: function() {
                    $("#invoiceLineAuditList").setColumns({ caption: 'Show/Hide Columns'
                        , bSubmit: 'Update', bCancel: 'Cancel', colnameview: false
                        , drag: false, ShrinkToFit: false
                    });
                    return false;
                }
            });
        }
        function LoadInvoiceLineGlSplitAuditGrid(contract, companyId) {
            $("#invoiceLineGlSplitAuditList").jqGrid({
                url: '../services/QuattroService.asmx/GetInvoiceLineGlOverrideAuditGrid'
                , datatype: "xml"
                , postData: { contractNumber: contract, companyId: companyId }
                , colNames: ['uid_invoice_line_gl_override', 'uid_invoice_line', 'Invoice #', 'Line #'
                    , 'Cost Code', 'Cost Code Desc', 'Changed By', 'Date Action Taken', 'Action Taken']
                , colModel: [{ name: 'uid_invoice_line_gl_override', index: 'uid_invoice_line_gl_override', sortable: false, hidden: true }
                    , { name: 'uid_invoice_line', index: 'uid_invoice_line', hidden: true, sortable: false, hidden: true }
                    , { name: 'InvoiceNumber', index: 'Invoice Number', sortable: false, width: 50, align: 'center' }
                    , { name: 'LineNumber', index: 'Line Number', sortable: false, width: 25, align: 'center' }
                    , { name: 'CostCode', index: 'Cost Code', sortable: false, align: 'center', width: 100 }
                    , { name: 'CostCodeDesc', index: 'Cost Code Desc', sortable: false, align: 'center' }
                    , { name: 'ChangedBy', index: 'Changed By', sortable: false, width: 100 }
                    , { name: 'DateActionTaken', index: 'Date Action Taken', sortable: false, align: 'right', width: 85 }
                    , { name: 'ActionTaken', index: 'Action Taken', sortable: false, width: 50, align: 'center' }
                ]
                , rowNum: 50
                , height: 350
                , width: 900
                , rowList: [25, 50, 100]
                , pager: '#invoiceLineGlSplitAuditPager'
                , caption: "Invoice Line GL Override Audit Details"
                , viewrecords: true
            });
            $("#invoiceLineGlSplitAuditList").jqGrid('navGrid', "#invoiceLineGlSplitAuditPager", { edit: false, add: false, del: false, search: false }).navButtonAdd("#invoiceLineGlSplitAuditPager",
                { caption: 'Toggle Columns', onClickButton: function() {
                    $("#invoiceLineGlSplitAuditList").setColumns({ caption: 'Show/Hide Columns'
                        , bSubmit: 'Update', bCancel: 'Cancel', colnameview: false
                        , drag: false, ShrinkToFit: false
                    });
                    return false;
                }
            });
        }       
    </script>   
    <div style="margin:15px;">
        <div style="float:left;">Choose Company:</div>
        <div style="float:left;margin-left:30px;">
            <select id="dropDownCompany">
                <option value="1" selected="selected">Titan US</option>
                <option value="2">Titan Canada</option>
            </select>
        </div>
    </div>
    <div style="margin:15px;">
        Retrieve Audit Details for Contract #: <input type="text" id="contractNumber" />
        <a href="#" class="button" id="getAuditDetailsButton" onclick="GetAuditDetails();" style="margin-right:10px;">Get Audit Details</a>
    </div>
    <div style="height:100%;width:100%;text-align:center" id="reportSelectionArea">                                
        <h3><a href="#">Contract Audit Details</a></h3>
        <div id="contractAuditArea" style="height:500px !important;">            
            <div id="contractAuditGrid" style="margin:10px 25px;width:90%;">
                <div style="width:95%;z-index:-1"><table id="contractAuditList"></table></div>
                <div id="contractAuditPager"></div>
            </div>
        </div>
        <h3><a href="#">Invoice Audit Details</a></h3>
        <div id="invoiceAuditArea" style="height:500px !important;">            
            <div id="invoiceAuditGrid" style="margin:10px 25px;width:90%;">
                <div style="width:95%;z-index:-1"><table id="invoiceAuditList"></table></div>
                <div id="invoiceAuditPager"></div>
            </div>
        </div>        
        <h3><a href="#">Invoice Line Audit Details</a></h3>
        <div id="invoiceLineAuditArea" style="height:500px !important;">            
            <div id="invoiceLineAuditGrid" style="margin:10px 25px;width:90%;">
                <div style="width:95%;z-index:-1"><table id="invoiceLineAuditList"></table></div>
                <div id="invoiceLineAuditPager"></div>
            </div>
        </div>    
        <h3><a href="#">Invoice Line GL Override Audit Details</a></h3>
        <div id="invoiceLineGlSplitAuditArea" style="height:500px !important;">            
            <div id="invoiceLineGlSplitAuditGrid" style="margin:10px 25px;width:90%;">
                <div style="width:95%;z-index:-1"><table id="invoiceLineGlSplitAuditList"></table></div>
                <div id="invoiceLineGlSplitAuditPager"></div>
            </div>
        </div>    
    </div>  
</asp:Content>

