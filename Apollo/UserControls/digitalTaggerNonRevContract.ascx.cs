#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class UserControls_digitalTaggerNonRevContract : System.Web.UI.UserControl
    {

        #region Member variables
        /// <summary>TBD</summary>
        string addContractCallbackFunctionName;
        /// <summary>TBD</summary>
        string addContractFunctionName;
        /// <summary>TBD</summary>
        string clearFunctionName;
        /// <summary>TBD</summary>
        string closeFunctionName;
        /// <summary>TBD</summary>
        private string validationFunctionName;
        #endregion

        #region BindButtonClicks method
        /// <summary>TBD</summary>
        public void BindButtonClicks()
        {
            addNonRevCont.Attributes["onclick"] = string.Format("{0}BindDropDowns();", this.ClientID);
            addContract.Attributes["onclick"] = string.Format("{0}();", addContractFunctionName);
            clearContract.Attributes["onclick"] = string.Format("{0}();", clearFunctionName);
            back.OnClientClick = string.Format("{0}(this);", closeFunctionName);
        }
        #endregion

        #region BuildAddContractCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildAddContractCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(contractNumber){{", addContractCallbackFunctionName));
            script.Append(string.Format(@"  var companyId = $get('{0}').value;", dropDownCompany.ClientID));
            script.Append(string.Format(@"  {0}(contractNumber,parseInt(companyId,10));", callbackFunctionName.Value));
            script.Append(string.Format(@"  {0}();", clearFunctionName));
            script.Append(string.Format(@"  $get('{0}').click();", back.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildAddContractScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildAddContractScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}() {{", addContractFunctionName));
            script.Append(@"    var valuesHash = new Object();");
            script.Append(string.Format(@"    valuesHash[""{0}""] = $get('{1}').value;", "PRODUCTCLASSID", productClassSearch.Id.ClientID));
            script.Append(string.Format(@"    valuesHash[""{0}""] = $get('{1}').value;", "AGENCYID", agencySearch.Id.ClientID));
            script.Append(string.Format(@"    valuesHash[""{0}""] = $get('{1}').value;", "ADVERTISERID", advertiserSearch.Id.ClientID));
            script.Append(string.Format(@"    valuesHash[""{0}""] = $get('{1}').value;", "AEID", aeSearch.Id.ClientID));
            script.Append(string.Format(@"    valuesHash[""{0}""] = $get('{1}').value;", "PROGRAM", textProgram.ClientID));
            script.Append(string.Format(@"    valuesHash[""{0}""] = $get('{1}').value;", "MEDIAFORMID", mediaFormSearch.Id.ClientID));
            script.Append(string.Format(@"    valuesHash[""{0}""] = $get('{1}').value;", "PROFITCENTERID", dropDownProfitCenter.ClientID));  
            script.Append(string.Format(@"    valuesHash[""{0}""] = ($get('{1}').value=='') ? 0 : parseInt($get('{1}').value,10);", "QUANTITY", textQuantity.ClientID));
            script.Append(string.Format(@"    valuesHash[""{0}""] = parseInt($get('{1}').value,10);", "COMPANYID", dropDownCompany.ClientID));
            script.Append(string.Format(@"    if ({0}()){{Apollo.DigitalLibraryService.AddNonRevContract(valuesHash,{1},Error);}}", validationFunctionName, addContractCallbackFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildClearScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildClearScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(){{", clearFunctionName));
            script.Append(@"    validationMessageStack='';");
            script.Append(string.Format(@"  ClearErrorDiv('{0}');", errorDiv.ClientID));            
            script.Append(string.Format(@"  $get('{0}').value = '';", productClassSearch.Id.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", productClassSearch.Name.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", agencySearch.Id.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", agencySearch.Name.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", advertiserSearch.Id.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", advertiserSearch.Name.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", aeSearch.Id.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", aeSearch.Name.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", textProgram.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", dropDownMarket.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", dropDownProfitCenter.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", mediaFormSearch.Name.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", mediaFormSearch.Id.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", dropDownMediaType.ClientID));            
            script.Append(string.Format(@"  $get('{0}').value = '';", textQuantity.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '1';", dropDownCompany.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildCloseScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildCloseScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(btn){{", closeFunctionName));
            script.Append(string.Format(@"  {0}();", clearFunctionName));
            script.Append(@"    HandleClose(btn);");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildDropDownScripts method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDropDownScripts()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format("function {0}BindDropDowns(){{", this.ClientID));
            script.Append(string.Format("    var ddlCompany = $get('{0}');", dropDownCompany.ClientID));
            script.Append(string.Format("    var ddlMarket = $get('{0}');", dropDownMarket.ClientID));
            script.Append(string.Format("    var ddlProfCenter = $get('{0}');", dropDownProfitCenter.ClientID));
            script.Append(string.Format("    var ddlMediaTypes = $get('{0}');", dropDownMediaType.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetCompanies('1',AddToList,null,'{0}');", dropDownCompany.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetMarkets('1','',AddToList,null,'{0}');", dropDownMarket.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetProfitCentersDD('1','','',AddToList,null,'{0}');", dropDownProfitCenter.ClientID));            
            script.Append(string.Format("    Apollo.AutoCompleteService.GetNewMediaTypes('1','',AddToList,null,'{0}');", dropDownMediaType.ClientID));            
            script.Append(string.Format("    $get('{0}').value = 1;", advertiserSearch.DependencyId.ClientID));
            script.Append(string.Format("    $get('{0}').value = 1;", agencySearch.DependencyId.ClientID));
            script.Append(string.Format("    $get('{0}').value = 1;", aeSearch.DependencyId.ClientID));
            script.Append(string.Format("    $get('{0}').value = 1;", mediaFormSearch.CompanyId.ClientID));
            script.Append(string.Format("    $addHandler(ddlCompany,'change',{0}onCompanyChange);", this.ClientID));
            script.Append(string.Format("    $addHandler(ddlMarket,'change',{0}onMarketChange);", this.ClientID));            
            script.Append(string.Format("    $addHandler(ddlMediaTypes,'change',{0}onMediaTypeChange);", this.ClientID));            
            script.AppendLine("}");
            script.Append(string.Format("function {0}onCompanyChange(sender,e){{", this.ClientID));
            script.Append(string.Format("    var ddlCompany = $get('{0}');", dropDownCompany.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetMarkets(ddlCompany.value,'',AddToList,null,'{0}');", dropDownMarket.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompany.value,'','',AddToList,null,'{0}');", dropDownProfitCenter.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetNewMediaTypes(ddlCompany.value,'',AddToList,null,'{0}');", dropDownMediaType.ClientID));            
            script.Append(string.Format("    $get('{0}').value = ddlCompany.value;", advertiserSearch.DependencyId.ClientID));
            script.Append(string.Format("    $get('{0}').value = ddlCompany.value;", agencySearch.DependencyId.ClientID));
            script.Append(string.Format("    $get('{0}').value = ddlCompany.value;", aeSearch.DependencyId.ClientID));
            script.Append(string.Format("    $get('{0}').value = ddlCompany.value;", mediaFormSearch.CompanyId.ClientID));            
            script.AppendLine("}");
            script.Append(string.Format("function {0}onMarketChange(sender,e){{", this.ClientID));
            script.Append(string.Format("    var ddlCompany = $get('{0}');", dropDownCompany.ClientID));
            script.Append(string.Format("    var ddlMarket = $get('{0}');", dropDownMarket.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompany.value,ddlMarket.value,'',AddToList,null,'{0}');", dropDownProfitCenter.ClientID));            
            script.AppendLine("}");
            script.Append(string.Format("function {0}onMediaTypeChange(sender,e){{", this.ClientID));
            script.Append(string.Format("    var ddlMediaTypes = $get('{0}');", dropDownMediaType.ClientID));
            script.Append(string.Format("    $get('{0}').value = ddlMediaTypes.value;", mediaFormSearch.DependencyId.ClientID));
            script.Append(string.Format("    $find($get('{0}').value)._contextKey = 'mediaFormId:'+ddlMediaTypes.value;", mediaFormSearch.BehaviorID.ClientID));
            script.AppendLine("}");            
            return script.ToString();
        }
        #endregion

        #region BuildValidationScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildValidationScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(){{", validationFunctionName));
            script.Append(@"    var isValid=true;");
            script.Append(@"    validationMessageStack='';");
            script.Append(string.Format(@"    ClearErrorDiv('{0}');", errorDiv.ClientID));
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", productClassSearch.Id.ClientID));
            script.Append(@"        AddErrorMessage('Product Class is Required.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", advertiserSearch.Id.ClientID));
            script.Append(@"        AddErrorMessage('Advertiser is Required.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", agencySearch.Id.ClientID));
            script.Append(@"        AddErrorMessage('Agency is Required.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", aeSearch.Id.ClientID));
            script.Append(@"        AddErrorMessage('AE is Required.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", dropDownProfitCenter.ClientID));
            script.Append(@"        AddErrorMessage('Profit Center is Required.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", mediaFormSearch.Id.ClientID));
            script.Append(@"        AddErrorMessage('Media Form is Required.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");                                
            script.Append(string.Format(@"    if ($get('{0}').value!='' && !IsValidNumber($get('{0}').value)){{", textQuantity.ClientID));
            script.Append(@"        AddErrorMessage('Quantity must be a valid number.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(@"    if (validationMessageStack!='' && !isValid){");
            script.Append(string.Format(@"        SetErrorDiv(validationMessageStack,'{0}');", errorDiv.ClientID));
            script.Append(@"        alert(validationMessageStack);");
            script.Append(@"    }");
            script.Append(@"    return isValid;");
            script.AppendLine("}");
            return script.ToString();
        }
        #endregion

        #region CallbackFunctionName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField CallbackFunctionName
        {
            get { return callbackFunctionName; }
        }
        #endregion

        #region CompanyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField CompanyId
        {
            get { return companyId; }
        }
        #endregion

        #region FileId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField FileId
        {
            get { return fileId; }
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
                PopulateScriptNames();
                RegisterScriptBlocks();
                BindButtonClicks();
            }
        }
        #endregion

        #region PopulateScriptNames method
        /// <summary>TBD</summary>
        public void PopulateScriptNames()
        {
            addContractFunctionName = string.Format("{0}AddContract", this.ClientID);
            addContractCallbackFunctionName = string.Format("{0}AddContractCallback", this.ClientID);
            closeFunctionName = string.Format("{0}Close", this.ClientID);
            clearFunctionName = string.Format("{0}Clear", this.ClientID);
            validationFunctionName = string.Format("{0}ValidateForm", this.ClientID);
        }
        #endregion

        #region RegisterScriptBlocks method
        /// <summary>TBD</summary>
        public void RegisterScriptBlocks()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), addContractFunctionName, BuildAddContractScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), addContractCallbackFunctionName, BuildAddContractCallbackScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), validationFunctionName, BuildValidationScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), clearFunctionName, BuildClearScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), closeFunctionName, BuildCloseScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Format("{0}DropDownScripts", this.ClientID), BuildDropDownScripts(), true);
        }
        #endregion

    }

}
