#region Using Statements
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class UserControls_contractSearch : System.Web.UI.UserControl
    {

        #region Member variables
        /// <summary>TBD</summary>
        string autoCompleteHandlerFunctionName;                
        string gridOpenFunctionName;
        string contractSelectionCallbackFunctionName;
        string searchFunctionName;
        #endregion

        #region BindButtonClicks method
        /// <summary>TBD</summary>
        public void BindButtonClicks()
        {           
            textTaggerContractNumber_AutoCompleteExtender.OnClientItemSelected = autoCompleteHandlerFunctionName;
            executeSearch.Attributes["onclick"] = string.Format(@"{0}();", searchFunctionName);
            searchClick.Attributes["onclick"] = string.Format(@"{0}();", gridOpenFunctionName);
            closeModal.Attributes["onclick"] = string.Format(@"{0}UnloadGrid(this);", this.ClientID);            
        }
        #endregion

        #region BuildAutoCompleteHandlerScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildAutoCompleteHandlerScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(source,eventArgs){{", autoCompleteHandlerFunctionName));
            script.AppendLine(@"    var selectedName = eventArgs.get_value();");
            script.AppendLine(@"    var selectedNameSplit = selectedName.split('-');");
            script.AppendLine(@"    var contractNumber = trimValue(selectedNameSplit[0]);");
            //script.AppendLine(@"    var company = 1;");
            script.AppendLine(@"    var company = (trimValue(selectedNameSplit[selectedNameSplit.length - 1]).toUpperCase() == 'USA') ? 1 : 2;");
            script.AppendLine(string.Format(@"    {0}(contractNumber,company);", contractLoadCallbackFunctionName.Value));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildGridOpenScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildGridOpenScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(){{", gridOpenFunctionName));
            script.Append(string.Format(@"    $get('{0}').value='1';", dropDownCompany.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", dropDownMarket.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", dropDownSubMarket.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", dropDownProfitCenter.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", dropDownMediaType.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", mediaFormSearch.DependencyId.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", mediaFormSearch.Id.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", mediaFormSearch.Name.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", advertiserSearch.Id.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", advertiserSearch.Name.ClientID));
            script.Append(string.Format(@"    $get('{0}').value=1;", advertiserSearch.DependencyId.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", agencySearch.Id.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", agencySearch.Name.ClientID));
            script.Append(string.Format(@"    $get('{0}').value=1;", agencySearch.DependencyId.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", aeSearch.Id.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", aeSearch.Name.ClientID));
            script.Append(string.Format(@"    $get('{0}').value=1;", aeSearch.DependencyId.ClientID));
            script.Append(string.Format(@"    $get('{0}').value='';", textProgram.ClientID));
            script.Append(string.Format(@"    $find($get('{0}').value)._contextKey = '';", mediaFormSearch.BehaviorID.ClientID));
            script.Append(string.Format(@"    var filterObject = BuildContractFilterObject($get('{0}').value,$get('{1}').value,$get('{2}').value,$get('{3}').value,$get('{4}').value,'',$get('{5}').value,$get('{6}').value,$get('{7}').value,$get('{8}').value,$get('{9}').value);"
                    , dropDownCompany.ClientID                    
                    , dropDownMarket.ClientID                    
                    , dropDownProfitCenter.ClientID
                    , dropDownMediaType.ClientID
                    , mediaFormSearch.Id.ClientID 
                    , textProgram.ClientID
                    , advertiserSearch.Id.ClientID
                    , agencySearch.Id.ClientID
                    , aeSearch.Id.ClientID
                    , dropDownSubMarket.ClientID
                )
            );
            //companyId,advertiserId,agencyId,aeId,dateFrom,dateTo,program,marketId,profitCenterId,mediaTypeId,mediaFormId,panelSubId,panelCode,contractNumber
            script.Append(string.Format(@"    DisplayStaticGrid('dlContracts','{0}','{1}',filterObject,{2});", contractSearchGrid.ClientID, contractSearchGridPager.ClientID, contractSelectionCallbackFunctionName));
            script.Append(string.Format(@"    $find('{0}').show();", contractSearchPopupExtender.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        public string BuildUnloadGridScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}UnloadGrid(btn){{", this.ClientID));
            script.AppendLine(string.Format(@"    $('#{0}').GridUnload('{0}');", contractSearchGrid.ClientID));
            script.AppendLine(@"    HandleClose(btn);");
            script.AppendLine(@"}");
            return script.ToString();
        }

        private string BuildContractSelectionCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(id){{", contractSelectionCallbackFunctionName));
            //var rowData = $('#globalGrid').getRowData(id);
            script.AppendLine(string.Format(@"    var rowData=$('#{0}').getRowData(id);", contractSearchGrid.ClientID));
            script.AppendLine(@"    var selectedContractNumber = rowData['contractNumber']");
            //script.AppendLine(@"    var selectedCompanyId = rowData['coId']");
            script.AppendLine(@"    var selectedCompanyId = 1");
            script.AppendLine(string.Format(@"    {0}(selectedContractNumber,selectedCompanyId);", contractLoadCallbackFunctionName.Value));
            script.AppendLine(@"}");
            return script.ToString();
        }

        private string BuildContractGridSearchScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", searchFunctionName));            
            script.Append(string.Format(@"checkACField($get('{0}'),$get('{1}'));", advertiserSearch.Name.ClientID, advertiserSearch.Id.ClientID));
            script.Append(string.Format(@"checkACField($get('{0}'),$get('{1}'));", agencySearch.Name.ClientID, agencySearch.Id.ClientID));
            script.Append(string.Format(@"checkACField($get('{0}'),$get('{1}'));", aeSearch.Name.ClientID, aeSearch.Id.ClientID));
            script.Append(string.Format(@"checkACField($get('{0}'),$get('{1}'));", mediaFormSearch.Name.ClientID, mediaFormSearch.Id.ClientID));
            script.Append(string.Format(@"    var filterObject = BuildContractFilterObject($get('{0}').value,$get('{1}').value,$get('{2}').value,$get('{3}').value,$get('{4}').value,'',$get('{5}').value,$get('{6}').value,$get('{7}').value,$get('{8}').value,$get('{9}').value);"
                    , dropDownCompany.ClientID                    
                    , dropDownMarket.ClientID                    
                    , dropDownProfitCenter.ClientID
                    , dropDownMediaType.ClientID                    
                    , mediaFormSearch.Id.ClientID
                    , textProgram.ClientID
                    , advertiserSearch.Id.ClientID
                    , agencySearch.Id.ClientID
                    , aeSearch.Id.ClientID
                    , dropDownSubMarket.ClientID 
                )
            );
            script.Append(@"    var postParams = getPostParams('dlContracts',filterObject);");
            script.Append(string.Format(@"    $('#{0}').setGridParam({{postData:postParams}}).trigger('reloadGrid');", contractSearchGrid.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }

        private string BuildOnKeyPressScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(@"function onContractSearchKeyPress(evt){ if (evt.keyCode==13){ try{ evt.preventDefault(); } catch(e) {} return; } }");
            //$addHandler($get('<%=textSearch.ClientID %>'),"keyup",onSearchFilterKeyPress);
            script.Append(@"function BindKeyPressEventHandlers(){");
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", dropDownCompany.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", dropDownMarket.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", dropDownSubMarket.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", dropDownProfitCenter.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", dropDownMediaType.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", aeSearch.Name.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", advertiserSearch.Name.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", agencySearch.Name.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", aeSearch.Name.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", mediaFormSearch.Name.ClientID));
            script.AppendLine(string.Format(@"    $addHandler($get('{0}'),""keyup"",onContractSearchKeyPress);", textProgram.ClientID));
            script.AppendLine("}");
            return script.ToString();
        }

        #region BuildDropDownScripts method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDropDownScripts()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format("function {0}BindDropDowns(){{", this.ClientID));
            script.AppendLine(string.Format("    var ddlCompany = $get('{0}');", dropDownCompany.ClientID));
            script.AppendLine(string.Format("    var ddlMarket = $get('{0}');", dropDownMarket.ClientID));
            script.AppendLine(string.Format("    var ddlProfCenter = $get('{0}');", dropDownProfitCenter.ClientID));
            script.AppendLine(string.Format("    var ddlMediaTypes = $get('{0}');", dropDownMediaType.ClientID));
            script.AppendLine(string.Format("    var ddlSubMarket = $get('{0}');", dropDownSubMarket.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetCompanies('1',AddToList,null,'{0}');", dropDownCompany.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetMarkets('1','',AddToList,null,'{0}');", dropDownMarket.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetProfitCentersDD('1','','',AddToList,null,'{0}');", dropDownProfitCenter.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetNewMediaTypes('1','',AddToList,null,'{0}');", dropDownMediaType.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetSalesMarket('',AddToList,null,'{0}');", dropDownSubMarket.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').value = 1;", advertiserSearch.DependencyId.ClientID));            
            script.AppendLine(string.Format(@"    $get('{0}').value = 1;", agencySearch.DependencyId.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').value = 1;", aeSearch.DependencyId.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').value = 1;", mediaFormSearch.CompanyId.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').disabled = (ddlMarket.value=='NYO') ? false : true;", dropDownSubMarket.ClientID));
            //Set defaults            
            script.AppendLine(string.Format("    $addHandler(ddlCompany,'change',{0}onCompanyChange);", this.ClientID));
            script.AppendLine(string.Format("    $addHandler(ddlMarket,'change',{0}onMarketChange);", this.ClientID));
            script.AppendLine(string.Format("    $addHandler(ddlProfCenter,'change',{0}onProfCenterChange);", this.ClientID));
            script.AppendLine(string.Format("    $addHandler(ddlMediaTypes,'change',{0}onMediaTypeChange);", this.ClientID));            
            script.AppendLine("}");
            script.AppendLine(string.Format("function {0}onCompanyChange(sender,e){{", this.ClientID));
            script.AppendLine(string.Format("    var ddlCompany = $get('{0}');", dropDownCompany.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetMarkets(ddlCompany.value,'',AddToList,null,'{0}');", dropDownMarket.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetNewMediaTypes(ddlCompany.value,'',AddToList,null,'{0}');", dropDownMediaType.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompany.value,'','',AddToList,null,'{0}');", dropDownProfitCenter.ClientID));
            script.AppendLine(string.Format("    $get('{0}').value = ddlCompany.value;", mediaFormSearch.CompanyId.ClientID));
            script.AppendLine(string.Format("    $get('{0}').value = ddlCompany.value;", advertiserSearch.DependencyId.ClientID));
            script.AppendLine(string.Format("    $get('{0}').value = ddlCompany.value;", agencySearch.DependencyId.ClientID));
            script.AppendLine(string.Format("    $get('{0}').value = ddlCompany.value;", aeSearch.DependencyId.ClientID));
            script.AppendLine("}");
            script.AppendLine(string.Format("function {0}onMarketChange(sender,e){{", this.ClientID));
            script.AppendLine(string.Format("    var ddlCompany = $get('{0}');", dropDownCompany.ClientID));
            script.AppendLine(string.Format("    var ddlMarket = $get('{0}');", dropDownMarket.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').disabled = (ddlMarket.value=='NYO') ? false : true;", dropDownSubMarket.ClientID));
            script.AppendLine(string.Format("    Apollo.AutoCompleteService.GetProfitCentersDD(ddlCompany.value,ddlMarket.value,'',AddToList,null,'{0}');", dropDownProfitCenter.ClientID));            
            script.AppendLine("}");
            script.AppendLine(string.Format("function {0}onProfCenterChange(sender,e){{", this.ClientID));
            script.AppendLine(string.Format("    var ddlProfitCenter = $get('{0}');", dropDownProfitCenter.ClientID));            
            script.AppendLine("}");
            script.AppendLine(string.Format("function {0}onMediaTypeChange(sender,e){{", this.ClientID));
            script.Append(string.Format("    var ddlMediaTypes = $get('{0}');", dropDownMediaType.ClientID));
            script.Append(string.Format("    $get('{0}').value = ddlMediaTypes.value;", mediaFormSearch.DependencyId.ClientID));
            script.Append(string.Format("    $find($get('{0}').value)._contextKey = 'mediaFormId:'+ddlMediaTypes.value;", mediaFormSearch.BehaviorID.ClientID));
            script.AppendLine("}");            
            return script.ToString();
        }
        #endregion

        #region ContractLoadCallbackFunctionName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField ContractLoadCallbackFunctionName
        {
            get { return contractLoadCallbackFunctionName; }
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
                dropDownCompany.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                dropDownMarket.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                dropDownMediaType.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                dropDownProfitCenter.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                dropDownSubMarket.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                textProgram.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                aeSearch.Name.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                advertiserSearch.Name.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                agencySearch.Name.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
                mediaFormSearch.Name.Attributes.Add("onkeypress", string.Format("return setDefaultParmlessFunction(event,{0});", searchFunctionName));
            }
        }
        #endregion

        #region PopulateScriptNames method
        /// <summary>TBD</summary>
        public void PopulateScriptNames()
        {            
            autoCompleteHandlerFunctionName = string.Format("{0}AutoCompleteSelectHandler", this.ClientID);                        
            contractSelectionCallbackFunctionName = string.Format("{0}ContractSelected", this.ClientID);
            gridOpenFunctionName = string.Format("{0}GridOpen", this.ClientID);
            searchFunctionName = string.Format("{0}SearchContracts", this.ClientID);
        }
        #endregion

        #region RegisterScriptBlocks method
        /// <summary>TBD</summary>
        public void RegisterScriptBlocks()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), autoCompleteHandlerFunctionName, BuildAutoCompleteHandlerScript(), true);                        
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Format("{0}DropDownScripts", this.ClientID), BuildDropDownScripts(), true);            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), contractSelectionCallbackFunctionName, BuildContractSelectionCallbackScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), gridOpenFunctionName, BuildGridOpenScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), searchFunctionName, BuildContractGridSearchScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), string.Format("{0}UnloadGrid", this.ClientID), BuildUnloadGridScript(), true);            
            ScriptManager.RegisterStartupScript(this, this.GetType(), string.Format("{0}DropDownScriptCall", this.ClientID), string.Format("{0}BindDropDowns();", this.ClientID), true);
        }
        #endregion

        public TextBox TaggerContractNumber
        {
            get { return textTaggerContractNumber; }            
        }

        public System.Web.UI.HtmlControls.HtmlImage SearchImage
        {
            get { return searchClick; }
        }

        public ImageButton CloseModal
        {
            get { return closeModal; }
        }
    }
}
