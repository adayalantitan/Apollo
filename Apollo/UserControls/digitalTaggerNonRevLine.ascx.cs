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
    public partial class UserControls_digitalTaggerNonRevLine : System.Web.UI.UserControl
    {

        #region Member variables
        /// <summary>TBD</summary>
        string addLineCallbackFunctionName;
        /// <summary>TBD</summary>
        string addLineFunctionName;
        /// <summary>TBD</summary>
        string clearFunctionName;
        /// <summary>TBD</summary>
        string validationFunctionName;
        #endregion

        public string BuildDropDownScripts()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format("function {0}BindDropDowns(){{", this.ClientID));            
            script.Append(string.Format("    var ddlMediaTypes = $get('{0}');", dropDownMediaType.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetNewMediaTypes('1','',AddToList,null,'{0}');", dropDownMediaType.ClientID));            
            script.Append(string.Format("    $addHandler(ddlMediaTypes,'change',{0}onMediaTypeChange);", this.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetProfitCentersDD(1,'','', AddToList,null,'{0}');", dropDownProfitCenter.ClientID));
            script.Append(string.Format(@"   $get('{0}').value = 1;", mediaFormSearch.CompanyId.ClientID));
            script.AppendLine("}");            
            script.Append(string.Format("function {0}onMediaTypeChange(sender,e){{", this.ClientID));
            script.Append(string.Format("    var ddlMediaTypes = $get('{0}');", dropDownMediaType.ClientID));
            script.Append(string.Format("    $get('{0}').value = ddlMediaTypes.value;", mediaFormSearch.DependencyId.ClientID));            
            script.Append(string.Format("    $find($get('{0}').value)._contextKey = 'mediaFormId:'+ddlMediaTypes.value;", mediaFormSearch.BehaviorID.ClientID));
            script.AppendLine("}");            
            return script.ToString();
        }

        #region BindButtonClicks method
        /// <summary>TBD</summary>
        public void BindButtonClicks()
        {
            saveLine.Attributes["onclick"] = string.Format("{0}();", addLineFunctionName);
            addLine.Attributes["onclick"] = string.Format("{0}BindDropDowns();", this.ClientID);
        }
        #endregion

        #region BuildAddLineCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildAddLineCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(){{", addLineCallbackFunctionName));
            script.Append(string.Format(@"  var contractNumber = $get('{0}').value;", contractNumber.ClientID));
            script.Append(string.Format(@"  var companyId = $get('{0}').value;", companyId.ClientID));
            script.Append(string.Format(@"  {0}(contractNumber,companyId);", callbackFunctionName.Value));
            script.Append(string.Format(@"  {0}();", clearFunctionName));
            script.Append(string.Format(@"  $get('{0}').click();", back.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildAddLineScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildAddLineScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(){{", addLineFunctionName));
            script.Append(@"    var hash = new Object();");            
            script.Append(string.Format(@"  hash[""CONTRACTNUMBER""] = $get('{0}').value;", contractNumber.ClientID));
            script.Append(string.Format(@"  hash[""QUANTITY""] = ($get('{0}').value=='') ? 0 : $get('{0}').value;", textQuantity.ClientID));
            script.Append(string.Format(@"  hash[""ISBONUS""] = $get('{0}').value;", dropDownReason.ClientID));
            script.Append(string.Format(@"  hash[""COMPANYID""] = $get('{0}').value;", companyId.ClientID));
            script.Append(string.Format(@"  hash[""PROFITCENTERID""] = $get('{0}').value;", dropDownProfitCenter.ClientID));            
            script.Append(string.Format(@"  hash[""MEDIAFORMID""] = $get('{0}').value;", mediaFormSearch.Id.ClientID));
            script.Append(string.Format(@"  if ({0}()){{Apollo.DigitalLibraryService.AddNonRevContractLine(hash,{1});}}", validationFunctionName, addLineCallbackFunctionName));
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
            script.Append(string.Format(@"  $get('{0}').value = '';", dropDownMediaType.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", dropDownProfitCenter.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", mediaFormSearch.DependencyId.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", mediaFormSearch.Id.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = '';", mediaFormSearch.Name.ClientID));                        
            script.Append(string.Format(@"  $get('{0}').value = 0;", textQuantity.ClientID));
            script.Append(string.Format(@"  $get('{0}').value = 'B';", dropDownReason.ClientID));
            script.AppendLine(@"}");
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
            script.Append(string.Format(@"    if ($get('{0}').value!='' && !IsValidNumber($get('{0}').value)){{", textQuantity.ClientID));
            script.Append(@"        AddErrorMessage('Quantity must be a valid number.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", dropDownMediaType.ClientID));
            script.Append(@"        AddErrorMessage('A Media Type must be selected.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", mediaFormSearch.Id.ClientID));
            script.Append(@"        AddErrorMessage('A Media Form must be selected.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(string.Format(@"    if ($get('{0}').value==''){{", dropDownProfitCenter.ClientID));
            script.Append(@"        AddErrorMessage('A Profit Center must be selected.');");
            script.Append(@"        isValid = false;");
            script.Append(@"    }");
            script.Append(@"    if (validationMessageStack!='' && !isValid){");
            script.Append(string.Format(@"        SetErrorDiv(validationMessageStack,'{0}');", errorDiv.ClientID));
            script.Append(@"        alert(validationMessageStack);");
            script.Append(@"    }");
            script.Append(@"    return isValid;");
            script.AppendLine(@"}");
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

        #region ContractNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField ContractNumber
        {
            get { return contractNumber; }
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
            addLineFunctionName = string.Format("{0}AddNonRevLine", this.ClientID);
            addLineCallbackFunctionName = string.Format("{0}AddNonRevLineCallback", this.ClientID);
            clearFunctionName = string.Format("{0}ClearNonRevLine", this.ClientID);
            validationFunctionName = string.Format("{0}ValidateNonRevLine", this.ClientID);
        }
        #endregion

        #region RegisterScriptBlocks method
        /// <summary>TBD</summary>
        public void RegisterScriptBlocks()
        {            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), addLineFunctionName, BuildAddLineScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), addLineCallbackFunctionName, BuildAddLineCallbackScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), clearFunctionName, BuildClearScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), validationFunctionName, BuildValidationScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Format("{0}DropDownScripts", this.ClientID), BuildDropDownScripts(), true);
        }
        #endregion

    }

}
