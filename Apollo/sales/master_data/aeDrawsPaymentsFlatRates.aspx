<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Apollo.master" AutoEventWireup="true" CodeFile="aeDrawsPaymentsFlatRates.aspx.cs" Inherits="Apollo.sales_master_data_aeDrawsPaymentsFlatRates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .datepicker, .button {}
        label {background-color:#EDEDFE !important;padding:3px;}
    </style>
    <script type="text/javascript" src="aeDrawPaymentsFlatRate.js?v=1.06"></script>
    <script type="text/javascript">
        var lastsel;
        var selectedData = {};
        var maximumFlatRatePercentage = "<%=MaximumFlatRatePercentage() %>";
        $(document).ready(function () {
            $(".datepicker").datepicker();
            $("#aeDrawPaymentFlatRateDialog").dialog({ autoOpen: false, modal: true, width: 625, height: 550, position: ["center", 200] });
            $(".flatRateMaxPercentage").html(maximumFlatRatePercentage)
            $("#aeDrawsAndPayments").jqGrid({
                url: "../../services/IOService.asmx/GetAEDrawPaymentGrid"
                , datatype: "xml"
                , colNames: ["AE Id", "AE Effective Date", "AE Name", "Market", "AE Type", "Start Date", "Status", "Company ID", "Market ID", "Company Name", "Flat Rate ID"]
                , colModel: [{ name: "aeId", index: "ACCOUNT_EXECUTIVE_ID", width: 50, align: "center", search: true, sort: true }
                    , { name: "aeEffectiveDate", index: "ACCOUNT_EXECUTIVE_EFFECTIVE_DATE", hidden: true }
                    , { name: "aeName", index: "ACCOUNT_EXECUTIVE_NAME", width: 125, search: true, sort: true }
                    , { name: "marketDescription", index: "MARKET_DESCRIPTION", width: 125, search: true }
                    , { name: "aeType", index: "ACCOUNT_EXECUTIVE_TYPE", width: 125, search: true, hidden: true }
                    , { name: "aeStartDate", index: "EMPLOYMENT_START_DATE", width: 125, search: false, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} }
                    , { name: "active", index: "ACTIVE", width: 110, editable: false, search: true }
                    , { name: "companyId", index: "COMPANY_ID", hidden: true }
                    , { name: "aeMarketId", index: "ACCOUNT_EXECUTIVE_MARKET_ID", hidden: true }
                    , { name: "companyName", index: "COMPANY_NAME", width: 125, search: true }
                    , { name: "flatRateId", index: "FLAT_RATE_ID", hidden: true}]
                , rowNum: 25
                , height: 500
                , width: 800
                , rowList: [25, 50, 100]
                , pager: "#aeDrawsAndPaymentsPager"
                , sortname: "ACCOUNT_EXECUTIVE_NAME"
                , sortorder: "ASC"
                , viewrecords: true
                , toolbar: [true, "top"]
                , caption: "AE Draw-Payment Maintenance"
                , onSelectRow: function (id) {
                    if (id) {
                        var aeId = $("#aeDrawsAndPayments").getRowData(id)["aeId"];
                        var aeName = $("#aeDrawsAndPayments").getRowData(id)["aeName"];
                        var companyId = $("#aeDrawsAndPayments").getRowData(id)["companyId"];
                        var companyName = $("#aeDrawsAndPayments").getRowData(id)["companyName"];
                        var flatRateId = $("#aeDrawsAndPayments").getRowData(id)["flatRateId"];
                        var startDate = $("#aeDrawsAndPayments").getRowData(id)["aeStartDate"];
                        selectedData = { aeId: aeId, aeName: aeName, companyId: companyId, companyName: companyName, flatRateId: flatRateId, startDate: startDate };
                        PopupModalWindow(selectedData);
                    }
                }
            });
            $("#aeDrawsAndPayments").jqGrid("navGrid", "#pager1", { edit: false, add: false, del: false });
            $("#aeDrawsAndPayments").jqGrid("filterToolbar", { searchOnEnter: true });
        });        
        function PopupModalWindow(selectedData) {
            $("#labelAeId").html(selectedData.aeId);
            $("#labelAeName").html(selectedData.aeName);
            $("#labelAeCompany").html(selectedData.companyName);
            $("#labelAeStartDate").html(selectedData.startDate);
            TogglePaymentSelectionFields(false);
            Apollo.MasterDataService.GetAEDrawPayments(selectedData.aeId, selectedData.companyId, GetAEDrawPaymentsCallback, ErrorCallback);
            Apollo.MasterDataService.GetAEFlatRates(selectedData.aeId, selectedData.companyId, GetAEFlatRatesCallback, ErrorCallback);
            Apollo.MasterDataService.GetAECommissionAmounts(selectedData.aeId, selectedData.companyId, GetCommissionAmountsCallback, ErrorCallback);

            //Clear the fields
            $("#textFlatRateEffectiveDate, #textAeFlatRateNew, #textAeFlatRateRenew, #textCommissionAmountYear, #textCommissionAmount, #dropDownType, #dropDownMonth, #textAeDrawPaymentYear, #textAeDrawPaymentAmount, #dropDownPaymentStatus, #textPaymentDate").val("");
            $("#aeDrawPaymentFlatRateDialog").dialog({ "title": selectedData.aeName }).dialog("open");
        }
    </script>
    <div style="margin:0 auto;">
        <div style="width:95%;margin:50px;"><table id="aeDrawsAndPayments"></table></div>
        <div id="aeDrawsAndPaymentsPager"></div>
    </div>
    <div id="aeDrawPaymentFlatRateDialog">
        <div id="errorDiv" class="errorDisplay" style="margin:4px"></div>
        <div style="width:100%;border:1px solid #06347a;padding:2px;">
            <div class="spanColumn">
                <div>
                    <div style="width:50%;float:left;">AE Information</div>
                    <div style="width:50%;float:left;">
                        <img id="addFlatRate" alt="Add Flat Rate" src="/Images/but_addrate.gif" style="cursor:pointer;margin:0 3px;display:inline" onclick="AddFlatRateRecord();" />
                        <img id="updateFlatRate" alt="Update Flat Rate" src="/Images/but_update.gif" style="cursor:pointer;margin:0 3px;display:none" onclick="UpdateFlatRateRecord();" />
                        <img id="cancelFlatRateChanges" alt="Cancel Changes" src="/Images/but_cancel.gif" style="cursor:pointer;margin:0 3px;" onclick="CancelFlatRateChanges();" />
                    </div>
                    <div style="clear:both"></div>
                </div>
            </div>
            <div style="width:50%;float:left">
                <ul class="formFields">                                
                    <li>
                        <div>
                            <div class="formFieldsLeftColumn">AE Company:</div>
                            <div class="formFieldsRightColumn"><span id="labelAeCompany" tabindex="1"></span></div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="formFieldsLeftColumn">AE Name:</div>
                            <div class="formFieldsRightColumn"><span id="labelAeName"></span></div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                </ul>
            </div>                                                
            <div style="width:50%;float:left">
                <ul class="formFields">
                    <li>
                        <div>
                            <div class="formFieldsLeftColumn">AE ID:</div>
                            <div class="formFieldsRightColumn"><span id="labelAeId"></span></div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="formFieldsLeftColumn">AE Start Date:</div>
                            <div class="formFieldsRightColumn"><span id="labelAeStartDate"></span></div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                    
                </ul>
            </div>
            <div style="clear:both"></div>
            <div style="margin:5px 0;">
                <div style="float:left;padding:2px 5px;">
                    <br />
                    <label for="textFlatRateEffectiveDate"><span class="requiredIndicator">*</span>&nbsp;Flat Rate Effective Date:</label>
                    <input type="text" id="textFlatRateEffectiveDate" style="width:60px !important;text-align:right;" class="datepicker" />
                    <br />
                    <span class="requiredIndicator">*&nbsp;Required Field</span>
                </div>
                <div style="float:left;margin-left:20px;">
                    <div style="text-align:center;display:block;">
                        Flat Rates cannot be more than <span class="flatRateMaxPercentage"></span>%
                    </div>
                    <div style="float:left;margin-left:10px;">
                        <label for="textAeFlatRateNew"><span class="requiredIndicator">*</span>&nbsp;Flat Rate New:</label>
                        <input type="text" id="textAeFlatRateNew" class="rateInputBox" style="width:35px !important;text-align:right;" />
                    </div>
                    <div style="float:left;margin-left:10px;">
                        <label for="textAeFlatRateRenew"><span class="requiredIndicator">*</span>&nbsp;Flat Rate Renew:</label>
                        <input type="text" id="textAeFlatRateRenew" class="rateInputBox" style="width:35px !important;text-align:right;" />
                    </div>
                    <div style="clear:both"></div>
                </div>
                <div style="clear:both"></div>                
            </div>
            <div style="width:100%">
                <ul class="formFields">
                    <li>                                    
                        <div style="background-color:#ededfe;padding:3px 0;"><b>Flat Rate History</b></div>                                    
                    </li>
                    <li>
                        <table id="flatRateHistoryTable" style="display:none;width:100%;">
                            <thead>
                                <tr>
                                    <th class="buttonHead_Center" style="width:5%">&nbsp;</th>
                                    <th class="buttonHead_Center" style="width:5%">&nbsp;</th>
                                    <th class="buttonHead_Right" style="width:30%;">Flat Rate New</th>
                                    <th class="buttonHead_Right" style="width:30%;">Flat Rate Renew</th>
                                    <th class="buttonHead_Right" style="width:30%;">Effective Date</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>                        
                    </li>
                </ul>
            </div>
            <div style="clear:both"></div>
            <div class="spanColumn">
                <div>
                    <div style="width:50%;float:left;">AE Commission Amount Entry</div>
                    <div style="width:50%;float:right;">
                        <img id="addCommissionAmount" alt="Add Amount" src="/Images/but_save.gif" 
                            style="cursor:pointer;margin:0 3px;display:inline" onclick="AddCommissionAmountRecord()" />
                        <img id="updateCommissionAmount" alt="Update Amount" src="/Images/but_update.gif" 
                            style="cursor:pointer;margin:0 3px;display:none" onclick="UpdateCommissionAmountRecord()" />                                
                        <img id="cancelCommissionAmountChanges" alt="Cancel Changes" src="/Images/but_cancel.gif" 
                            style="cursor:pointer;margin:0 3px;" onclick="CancelCommissionAmountChanges();" />
                    </div>
                    <div style="clear:both;"></div>
                </div>
            </div>
            <div>
                <div class="formFieldsLeftColumn" style="width:100px !important;"><span class="requiredIndicator">*</span>&nbsp;Commission Year:</div>
                <div class="formFieldsRightColumn" style="width:125px !important;">
                    <input type="text" id="textCommissionAmountYear" style="width:50px !important;text-align:right;" />
                </div>
                <div class="formFieldsLeftColumn" style="width:125px !important;"><span class="requiredIndicator">*</span>&nbsp;Commission Amount:</div>
                <div class="formFieldsRightColumn" style="width:150px !important;">
                    <input type="text" id="textCommissionAmount" style="width:100px !important;text-align:right;" />                    
                </div>
                <div style="clear:both"></div>
            </div>
            <div style="width:100%">
                <ul class="formFields">
                    <li>                                    
                        <div style="background-color:#ededfe;padding:3px 0;"><b>Commission Amount History</b></div>                                    
                    </li>
                    <li>
                        <table id="commissionAmountTable" style="display:none;width:100%;">
                            <thead>
                                <tr>
                                    <th class="buttonHead_Center" style="width:5%">&nbsp;</th>
                                    <th class="buttonHead_Center" style="width:5%">&nbsp;</th>
                                    <th class="buttonHead_Right" style="width:30%;">Year</th>
                                    <th class="buttonHead_Right" style="width:30%;">Commission Amount</th>
                                    <th class="buttonHead_Right" style="width:30%;">Last Update</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>                        
                    </li>
                </ul>
            </div>
            <div style="clear:both"></div>
            <div class="spanColumn">
                <div>
                    <div style="width:50%;float:left;">AE Draw and Payment Entry</div>
                    <div style="width:50%;float:right;">
                        <img id="addPayment" alt="Add Payment" src="/Images/but_add_payment.gif" 
                            style="cursor:pointer;margin:0 3px;display:inline" onclick="AddDrawPaymentRecord()" />
                        <img id="updatePayment" alt="Update Payment" src="/Images/but_update.gif" 
                            style="cursor:pointer;margin:0 3px;display:none" onclick="UpdateDrawPaymentRecord()" />                                
                        <img id="cancelPaymentChanges" alt="Cancel Changes" src="/Images/but_cancel.gif" 
                            style="cursor:pointer;margin:0 3px;" onclick="CancelPaymentChanges();" />
                    </div>
                    <div style="clear:both;"></div>
                </div>
            </div>
            <div style="width:50%;float:left">
                <ul class="formFields">
                    <li>
                        <div>
                            <div class="formFieldsLeftColumn"><span class="requiredIndicator">*</span>&nbsp;Type:</div>
                            <div class="formFieldsRightColumn">
                                <select id="dropDownType" onchange="PaymentTypeChangeHandler(this);">
                                    <option selected="selected" value=""></option>
                                    <option value="D">Draw</option>
                                    <option value="P">Payment</option>
                                </select>                              
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li id="paymentMonthSelection">
                        <div>
                            <div class="formFieldsLeftColumn"><span class="requiredIndicator">*</span>&nbsp;Payment Month:</div>
                            <div class="formFieldsRightColumn">
                                <select id="dropDownMonth">
                                    <option value="" selected="selected">Select Month</option>
                                    <option value="1">January</option>
                                    <option value="2">February</option>
                                    <option value="3">March</option>
                                    <option value="4">April</option>
                                    <option value="5">May</option>
                                    <option value="6">June</option>
                                    <option value="7">July</option>
                                    <option value="8">August</option>
                                    <option value="9">September</option>
                                    <option value="10">October</option>
                                    <option value="11">November</option>
                                    <option value="12">December</option>
                                </select>                                       
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li>
                        <div>
                            <div class="formFieldsLeftColumn"><span class="requiredIndicator">*</span>&nbsp;Year:</div>
                            <div class="formFieldsRightColumn"><input type="text" id="textAeDrawPaymentYear" class="rateInputBox" /></div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                                
                </ul>
            </div>
            <div style="width:50%;float:left">
                <ul class="formFields">
                    <li>
                        <div>
                            <div class="formFieldsLeftColumn"><span class="requiredIndicator">*</span>&nbsp;Amount:</div>
                            <div class="formFieldsRightColumn"><input type="text" id="textAeDrawPaymentAmount" class="rateInputBox" /></div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li id="paymentStatusSelection">
                        <div>
                            <div class="formFieldsLeftColumn">Payment Status:</div>
                            <div class="formFieldsRightColumn">
                                <select id="dropDownPaymentStatus">
                                    <option value="N" selected="selected">Not Paid</option>
                                    <option value="P">Paid</option>
                                </select>                                 
                            </div>
                            <div style="clear:both"></div>
                        </div>
                    </li>
                    <li id="paymentDateSelection">
                        <div>
                            <div class="formFieldsLeftColumn">Payment Date:</div>
                            <div class="formFieldsRightColumn"><input type="text" id="textPaymentDate" class="datepicker" /></div>
                            <div style="clear:both"></div>
                        </div>
                    </li>                                
                </ul>
            </div>                                                
            <div style="clear:both"></div>
            <div style="width:100%">
                <ul class="formFields">
                    <li>
                        <span class="requiredIndicator">*&nbsp;Required Field</span>
                    </li>
                    <li>
                        <br />
                    </li>
                    <li>                                    
                        <div style="background-color:#ededfe;padding:3px 0;">
                            <b>Draw and Payment History</b>
                        </div>                                    
                    </li>
                    <li>
                        <table id="drawPaymentHistoryTable" style="display:none;width:100%;">
                            <thead>
                                <tr>
                                    <th class="buttonHead_Center" style="width:5%">&nbsp;</th>
                                    <th class="buttonHead_Center" style="width:5%">&nbsp;</th>
                                    <th class="buttonHead_Center" style="width:10%">Type</th>
                                    <th class="buttonHead_Center" style="width:10%">Year</th>
                                    <th class="buttonHead_Center" style="width:18%">Payment Month</th>
                                    <th class="buttonHead_Center" style="width:19%">Payment Status</th>
                                    <th class="buttonHead_Center" style="width:18%">Entered By</th>
                                    <th class="buttonHead_Center" style="width:15%">Amount</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>