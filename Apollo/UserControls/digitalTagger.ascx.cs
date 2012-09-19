#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class UserControls_digitalTagger : System.Web.UI.UserControl
    {

        #region ContextEnum enumeration
        public enum ContextEnum
        {
            Detail,
            TagSelected
        }
        #endregion

        #region Member variables
        /// <summary>TBD</summary>
        string activateCallbackFunctionName;
        /// <summary>TBD</summary>
        string activateFunctionName;
        /// <summary>TBD</summary>
        string activateSelectedCallbackFunctionName;
        /// <summary>TBD</summary>
        string activateSelectedFunctionName;
        /// <summary>TBD</summary>
        string autoCompleteHandlerFunctionName;
        /// <summary>TBD</summary>
        string clearContractDataFunctionName;
        /// <summary>The client-side name of the Clear function used by this control</summary>
        string clearFunctionName;
        string clearAfterTaggingFunctionName;
        /// <summary>TBD</summary>
        string deactivateCallbackFunctionName;
        /// <summary>TBD</summary>
        string deactivateFunctionName;
        /// <summary>TBD</summary>
        string deactivateSelectedCallbackFunctionName;
        /// <summary>TBD</summary>
        string deactivateSelectedFunctionName;
        /// <summary>TBD</summary>
        string deleteCallbackFunctionName;
        /// <summary>TBD</summary>
        string deleteFunctionName;
        /// <summary>TBD</summary>
        string handleContractSelectionFunctionName;
        /// <summary>TBD</summary>
        string loadContractDataCallbackFunctionName;
        /// <summary>TBD</summary>
        string loadContractDataFunctionName;
        /// <summary>TBD</summary>
        string loadDetailCallbackFunctionName;
        /// <summary>TBD</summary>
        string loadDetailFunctionName;
        /// <summary>TBD</summary>
        string saveCallbackFunctionName;
        /// <summary>TBD</summary>
        string saveFunctionName;
        /// <summary>TBD</summary>
        private ContextEnum taggerContext;
        /// <summary>TBD</summary>
        string untagCallbackFunctionName;
        /// <summary>TBD</summary>
        string untagFunctionName;
        string bindDropDownsFunctionName;
        string saveErrorFunctionName;
        string updateFunctionName;        
        #endregion

        #region BuildDropDownScripts method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDropDownScripts()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format("function {0}(){{", bindDropDownsFunctionName));
            script.Append(string.Format("    var ddlStationMarket = $get('{0}');", dropDownStationMarket.ClientID));
            script.Append(string.Format("    var ddlStations = $get('{0}');", dropDownStation.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetMarkets('1','',AddToList,null,'{0}');", dropDownStationMarket.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetStationList('','',AddToList,null,'{0}');", dropDownStation.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetEthnicities('', AddToList, null, '{0}');", dropDownEthnicity.ClientID));
            script.Append(string.Format("    $addHandler(ddlStationMarket,'change',{0}onMarketChange);", this.ClientID));            
            script.AppendLine("}");            
            script.Append(string.Format("function {0}onMarketChange(sender,e){{", this.ClientID));
            script.Append(string.Format("    var ddlStationMarket = $get('{0}');", dropDownStationMarket.ClientID));
            script.Append(string.Format("    Apollo.AutoCompleteService.GetStationList(ddlStationMarket.value,'',AddToList,null,'{0}');", dropDownStation.ClientID));
            script.AppendLine("}");            
            return script.ToString();
        }
        #endregion

        #region BindButtonClicks method
        /// <summary>Dynamically binds buttons on this control with JavaScript functions created on the server-side</summary>
        private void BindButtonClicks()
        {
            //Data Load
            taggerLoad.Attributes["onclick"] = string.Format("{0}();", loadDetailFunctionName);
            nonRevContract.CallbackFunctionName.Value = loadContractDataFunctionName;
            nonRevLine.CallbackFunctionName.Value = loadContractDataFunctionName;
            //Cleanup
            taggerUnload.Attributes["onclick"] = string.Format("{0}();", clearFunctionName);
            //Untag
            unTagLink.Attributes["onclick"] = string.Format("{0}();", untagFunctionName);
            //Save
            taggerSave.Attributes["onclick"] = string.Format("{0}();", saveFunctionName);
            //UI functions
            //textTaggerContractNumber_AutoCompleteExtender.OnClientItemSelected = autoCompleteHandlerFunctionName;
            contractSearch.ContractLoadCallbackFunctionName.Value = loadContractDataFunctionName;
            taggerClearSelected.Style["display"] = "none";
            //contractSearch.ContractSelectionCallback.Value = handleContractSelectionFunctionName;
            //Activate/Deactive
            if (taggerContext == ContextEnum.Detail)
            {
                taggerActivate.Attributes["onclick"] = string.Format("{0}();", activateFunctionName);
                taggerDeactivate.Attributes["onclick"] = string.Format("{0}();", deactivateFunctionName);
                taggerDelete.Attributes["onclick"] = string.Format("{0}();", deleteFunctionName);
            }
            else
            {
                taggerActivate.Attributes["onclick"] = string.Format("{0}();", activateSelectedFunctionName);                
                taggerDeactivate.Attributes["onclick"] = string.Format("{0}();", deactivateSelectedFunctionName);
                taggerDelete.Attributes["onclick"] = "DeleteSelected();";
                taggerDeactivate.Src = "/Images/dl/but_deactivate_selected.png";
                taggerDelete.Src = "/Images/dl/but_delete_selected.png";
            }            
        }
        #endregion

        #region BuildActivateCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildActivateCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", activateCallbackFunctionName));
            script.AppendLine(@"    alert('File has been Activated');");
            script.AppendLine(string.Format(@"    {0}();", loadDetailFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildActivateScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildActivateScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", activateFunctionName));
            script.AppendLine(string.Format(@"    var id=$get('{0}').value;", textImageId.ClientID));
            script.AppendLine(string.Format(@"    Apollo.DigitalLibraryService.ActivateDigitalLibraryFile(id,{0});", activateCallbackFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildActivateSelectedCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildActivateSelectedCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", activateSelectedCallbackFunctionName));
            script.AppendLine(@"    alert('Files have been Activated');");
            script.AppendLine(string.Format(@"    {0}();", loadDetailFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildActivateSelectedScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildActivateSelectedScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", activateSelectedFunctionName));
            script.AppendLine(string.Format(@"    var ids=$get('{0}').value;", textImageIds.ClientID));
            script.AppendLine(string.Format(@"    Apollo.DigitalLibraryService.ActivateSelected(ids,{0});", activateSelectedCallbackFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildClearContractDataScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildClearContractDataScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}() {{", clearContractDataFunctionName));
            script.AppendLine(string.Format(@"    $get('{0}').value = '';", contractSearch.TaggerContractNumber.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').value='';", dropDownSubMarketOverride.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", selectedContractNumber.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').style.display = 'none';", selectedContractNumber.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').style.display = 'inline';", contractLookupSection.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').style.display = 'inline';", contractSearch.SearchImage.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').style.display = 'none';", unTagLink.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').style.display = 'inline';", addNonRevLink.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').value = 'I';", dropDownDocumentType.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", country.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", ae1Value.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", agency.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", advertiser.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", ae2Label.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", ae2Value.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", ae3Label.ClientID));
            script.AppendLine(string.Format(@"    $get('{0}').innerHTML = '';", ae3Value.ClientID));            
            script.AppendLine(string.Format(@"    $get('{0}').style.display = 'none';", tagDetailsTable.ClientID));
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
            script.AppendLine(string.Format(@"function {0}() {{", clearFunctionName));
            script.AppendLine(string.Format(@"  $get('{0}').checked = false;", checkHeroQuality.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').checked = false;", checkMarketingQuality.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').checked = true;", radioPhotographer.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').checked = false;", radioInstaller.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = '';", textNotes.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = '';", contractSearch.TaggerContractNumber.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'inline';", contractSearch.SearchImage.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'none';", unTagLink.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'inline';", addNonRevLink.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = 'I';", dropDownDocumentType.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownSubMarketOverride.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownStationMarket.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownStation.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownEthnicity.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", country.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", ae1Value.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", agency.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", advertiser.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", ae2Label.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", ae2Value.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", ae3Label.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = '';", ae3Value.ClientID));            
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'none';", tagDetailsTable.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildClearAfterTaggingScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildClearAfterTaggingScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}() {{", clearAfterTaggingFunctionName));
            script.AppendLine(string.Format(@"  $get('{0}').value = '';", textNotes.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownSubMarketOverride.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownStationMarket.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownStation.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value='';", dropDownEthnicity.ClientID));
            script.AppendLine("    $('.lineItemTag').attr('checked',false);");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildDeactivateCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDeactivateCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", deactivateCallbackFunctionName));
            script.AppendLine(@"    alert('File has been Deactivated');");
            script.AppendLine(string.Format(@"    {0}();", loadDetailFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildDeactivateScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDeactivateScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", deactivateFunctionName));
            script.AppendLine(@"    if (confirm('Are you sure you wish to deactivate this file?')){");
            script.AppendLine(string.Format(@"        var id=$get('{0}').value;", textImageId.ClientID));
            script.AppendLine(string.Format(@"        Apollo.DigitalLibraryService.DeactivateDigitalLibraryFile(id,{0});", deactivateCallbackFunctionName));
            script.AppendLine(@"    }");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildDeactivateSelectedCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDeactivateSelectedCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", deactivateSelectedCallbackFunctionName));
            script.AppendLine(@"    alert('Files have been Deactivated');");
            script.AppendLine(string.Format(@"    {0}();", loadDetailFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildDeactivateSelectedScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDeactivateSelectedScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", deactivateSelectedFunctionName));
            script.AppendLine(@"    if (confirm('Are you sure you wish to deactivate these files?')){");
            script.AppendLine(string.Format(@"        var ids=$get('{0}').value;", textImageIds.ClientID));
            script.AppendLine(string.Format(@"        Apollo.DigitalLibraryService.DeactivateSelected(ids,{0});", deactivateSelectedCallbackFunctionName));
            script.AppendLine(@"    }");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildDeleteCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDeleteCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(){{", deleteCallbackFunctionName));
            script.Append(@"    RefreshPage(true);");
            script.Append(string.Format(@"    {0}();", clearFunctionName));
            script.Append(@"    alert('File deleted.');");            
            script.Append(@"    HideImageDetail(false);");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildDeleteScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildDeleteScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", deleteFunctionName));
            script.AppendLine(string.Format(@"    var id=$get('{0}').value;", textImageId.ClientID));
            script.AppendLine(string.Format(@"    var ext=$get('{0}').value;", textExtension.ClientID));
            script.AppendLine(@"    if (confirm('Are you sure you wish to delete this file?')){");
            script.AppendLine(string.Format(@"        Apollo.DigitalLibraryService.RemoveUploadedFile(id,ext,{0});", deleteCallbackFunctionName));
            script.AppendLine(@"    }");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildLoadContractDataCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string BuildLoadContractDataCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(values){{", loadContractDataCallbackFunctionName));
            script.AppendLine(@"    if (values != null) {");
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""CONTRACT_NUMBER""];", contractSearch.TaggerContractNumber.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""CONTRACT_NUMBER""];", nonRevLine.ContractNumber.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""COMPANY_ID""];", nonRevLine.CompanyId.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""COMPANY_ID""];", nonRevContract.CompanyId.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""COMPANY_ID""];", companyId.ClientID));            
            script.AppendLine(string.Format(@"      $get('{0}').value = $get('{1}').value;", nonRevLine.FileId.ClientID, textImageId.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = $get('{1}').value;", nonRevContract.FileId.ClientID, textImageId.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').style.display = 'inline';", contractSearch.SearchImage.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').style.display = 'none';", unTagLink.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').style.display = 'block';", addNonRevLink.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = values[""COMPANY""];", country.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = values[""AE_1_NAME""];", ae1Value.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = values[""AGENCY""];", agency.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = values[""ADVERTISER""];", advertiser.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = (values[""AE_2_NAME""] == '') ? '' : 'AE 2';", ae2Label.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = values[""AE_2_NAME""];", ae2Value.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = (values[""AE_3_NAME""] == '') ? '' : 'AE 3';", ae3Label.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = values[""AE_3_NAME""];", ae3Value.ClientID));            
            script.AppendLine(string.Format(@"      $get('{0}').innerHTML = values[""LINE_ITEMS""];", taggerContractLines.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').style.display = 'block';", tagDetailsTable.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').style.display = 'block';", addNonRevLines.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""SALES_MARKET_OVERRIDE""];", dropDownSubMarketOverride.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""STATION_MARKET_ID""];", dropDownStationMarket.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = values[""STATION_ID""];", dropDownStation.ClientID));
            script.AppendLine(string.Format(@"      $get('{0}').value = 1;", performedTagging.ClientID));
            script.AppendLine(@"    } else {");
            script.AppendLine(string.Format(@"      {0}();", clearContractDataFunctionName));
            script.AppendLine(@"    }");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildLoadContractDataScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private string BuildLoadContractDataScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(contractNumber,companyId){{", loadContractDataFunctionName));
            script.AppendLine(string.Format(@"    {0}();", clearContractDataFunctionName));
            script.AppendLine(string.Format(@"    $get('{0}').click()", contractSearch.CloseModal.ClientID));
            script.AppendLine(string.Format(@"    Apollo.DigitalLibraryService.LoadContractData(contractNumber,companyId,{0});", loadContractDataCallbackFunctionName));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildLoadDetailCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildLoadDetailCallbackScript()
        {
            bool isDigitalLibraryUser = (Security.IsDigitalUser() || Security.IsAdminUser() || Security.IsCorporateUser());
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(values){{", loadDetailCallbackFunctionName));
            script.AppendLine(@"    if (values == null) {");
            script.AppendLine(string.Format(@"      {0}();", clearFunctionName));
            script.AppendLine(@"        return;");
            script.AppendLine(@"    }");
            if (isDigitalLibraryUser)
            {
                //script.AppendLine(string.Format(@"  $get('{0}').style.display = (values[""IS_DELETED""] == 'N') ? 'inline' : 'none';", taggerDeactivate.ClientID));
                //script.AppendLine(string.Format(@"  $get('{0}').style.display = (values[""IS_DELETED""] == 'N') ? 'none' : 'inline';", taggerActivate.ClientID));
            }
            script.AppendLine(string.Format(@"  $get('{0}').checked = (values[""IS_HERO_QUALITY""].toLowerCase() == 'y');", checkHeroQuality.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').checked = (values[""IS_MARKETING_QUALITY""].toLowerCase() == 'y');", checkMarketingQuality.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').checked = (values[""TAKEN_BY""].toLowerCase() == 'photographer');", radioPhotographer.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').checked = (values[""TAKEN_BY""].toLowerCase() == 'installer');", radioInstaller.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""NOTES""];", textNotes.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""FILE_TYPE""].toUpperCase();", dropDownDocumentType.ClientID));
            script.AppendLine(@"    if (values[""CONTRACT_NUMBER""]=='' || values[""IS_TAGGED""].toUpperCase()=='N'){");
            script.AppendLine(string.Format(@"      {0}();", clearContractDataFunctionName));
            script.AppendLine(@"        return;");
            script.AppendLine(@"    }");
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""CONTRACT_NUMBER""];", selectedContractNumber.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""CONTRACT_NUMBER""];", contractSearch.TaggerContractNumber.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'inline';", selectedContractNumber.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'none';", contractLookupSection.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""CONTRACT_NUMBER""];", nonRevLine.ContractNumber.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""COMPANY_ID""];", nonRevLine.CompanyId.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""COMPANY_ID""];", nonRevContract.CompanyId.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""COMPANY_ID""];", companyId.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = $get('{1}').value;", nonRevLine.FileId.ClientID, textImageId.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = $get('{1}').value;", nonRevContract.FileId.ClientID, textImageId.ClientID));
            if (isDigitalLibraryUser)
            {                
                script.AppendLine(string.Format(@"  $get('{0}').style.display = (values[""IS_TAGGED""].toUpperCase() == 'Y') ? 'inline' : 'none';", unTagLink.ClientID));                
            }
            script.AppendLine(string.Format(@"  $get('{0}').value = (values[""IS_TAGGED""].toUpperCase() == 'Y') ? '-1' : '0';", isTagged.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'none';", contractSearch.SearchImage.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'none';", addNonRevLink.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""COMPANY""];", country.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""AE_1_NAME""];", ae1Value.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""AGENCY""];", agency.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""ADVERTISER""];", advertiser.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = (values[""AE_2_NAME""] == '') ? '' : 'AE 2';", ae2Label.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""AE_2_NAME""];", ae2Value.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = (values[""AE_3_NAME""] == '') ? '' : 'AE 3';", ae3Label.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""AE_3_NAME""];", ae3Value.ClientID));            
            script.AppendLine(string.Format(@"  $get('{0}').innerHTML = values[""LINE_ITEMS""];", taggerContractLines.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'none';", addNonRevLines.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').style.display = 'block';", tagDetailsTable.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""SALES_MARKET_OVERRIDE""];", dropDownSubMarketOverride.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""STATION_MARKET_ID""];", dropDownStationMarket.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""STATION_ID""];", dropDownStation.ClientID));
            script.AppendLine(string.Format(@"  $get('{0}').value = values[""ETHNICITY_ID""];", dropDownEthnicity.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildLoadDetailScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildLoadDetailScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", loadDetailFunctionName));
            script.AppendLine(string.Format(@"    {0}();", bindDropDownsFunctionName));
            script.AppendLine(string.Format(@"    {0}();", clearFunctionName));
            if (taggerContext != ContextEnum.TagSelected)
            {
                script.AppendLine(string.Format(@"    Apollo.DigitalLibraryService.LoadTaggingData($get('{0}').value,$get('{1}').value,{2});", textImageId.ClientID, textContractNumber.ClientID, loadDetailCallbackFunctionName));
            }
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildSaveCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildSaveCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", saveCallbackFunctionName));
            if (taggerContext != ContextEnum.TagSelected)
            {
                //script.AppendLine(string.Format(@"    $get('{0}').click();", taggerLoad.ClientID));
                script.AppendLine(@"    alert('Tagging Details have been Saved.');");
                script.AppendLine(string.Format("try {{ {0}(); }} catch(e) {{ alert('Error Number: '+e.number+'\\nError Desc: '+e.description); }}", loadDetailFunctionName));                
            }
            else
            {
                script.AppendLine(@"try{");
                script.AppendLine(string.Format(@"    TagSelectedSaved($get('{0}').value);", textImageIds.ClientID));
                script.AppendLine(string.Format(@"    {0}();", clearAfterTaggingFunctionName));
                script.AppendLine(@"} catch(e) { alert('Error Number: '+e.number+'\\nError Desc: '+e.description); }");
            }            
            script.AppendLine(@"    RefreshPage(true);");            
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        public string BuildSaveErrorScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(e){{", saveErrorFunctionName));
            script.AppendLine(@"alert('An error occurred while trying to tag your Image(s): ' + e._message);}");            
            return script.ToString();
        }

        public string BuildUpdateScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(){{", updateFunctionName));
            script.AppendLine(@"    try {");
            script.AppendLine(@"        var hash = {};");
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').value;", "ID", textImageIds.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').checked;", "IS_HERO_QUALITY", checkHeroQuality.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').checked;", "IS_MARKETING_QUALITY", checkMarketingQuality.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = ($get('{1}').checked) ? 'photographer' : (($get('{2}').checked) ? 'installer' : '');", "TAKEN_BY", radioPhotographer.ClientID, radioInstaller.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').value;", "NOTES", textNotes.ClientID));
            script.AppendLine(string.Format(@"        hash[""SALES_MARKET_OVERRIDE""] = $get('{0}').value;", dropDownSubMarketOverride.ClientID));
            script.AppendLine(string.Format(@"        hash[""STATION_MARKET_ID""] = $get('{0}').value;", dropDownStationMarket.ClientID));
            script.AppendLine(string.Format(@"        hash[""STATION_ID""] = $get('{0}').value;", dropDownStation.ClientID));
            script.AppendLine(string.Format(@"        hash[""ETHNICITY_ID""] = $get('{0}').value;", dropDownEthnicity.ClientID));
            script.AppendLine(string.Format(@"        Apollo.DigitalLibraryService.UpdateSelectedTag(hash,{0},{1});", saveCallbackFunctionName, saveErrorFunctionName));
            script.AppendLine(@"    } catch (e) { alert('Error Number: '+e.number+'\\nError Desc: '+e.description); }");
            script.AppendLine("}");
            return script.ToString();
        }

        #region BuildSaveScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildSaveScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", saveFunctionName));            
            script.AppendLine(@"    try {");
            script.AppendLine(string.Format(@"        if ($get('{0}').value=='-1'){{ {1}();return; }}", isTagged.ClientID, updateFunctionName));
            script.AppendLine(@"        var hash = new Object();");
            script.AppendLine(string.Format(@"        var perfomedTagging = ($get('{0}').value==1);", performedTagging.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').value;", "ID", textImageIds.ClientID));            
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').checked;", "IS_HERO_QUALITY", checkHeroQuality.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').checked;", "IS_MARKETING_QUALITY", checkMarketingQuality.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = ($get('{1}').checked) ? 'photographer' : (($get('{2}').checked) ? 'installer' : '');", "TAKEN_BY", radioPhotographer.ClientID, radioInstaller.ClientID));
            script.AppendLine(string.Format(@"        hash[""{0}""] = $get('{1}').value;", "NOTES", textNotes.ClientID));
            script.AppendLine(string.Format(@"        if ($get('{0}').value==1){{", performedTagging.ClientID));            
            script.AppendLine(string.Format(@"            var checkBoxes = $('#{0} .lineItemTag');", taggerContractLines.ClientID));
            script.AppendLine(@"            var lineItemNumbers='';");
            script.AppendLine(@"            for (var i=0;i<checkBoxes.length;i++){");
            script.AppendLine(@"                if (checkBoxes[i].checked){");
            script.AppendLine(@"                    lineItemNumbers += ((lineItemNumbers != '') ? ';' : '') + checkBoxes[i].id.split('_')[1];");
            script.AppendLine(@"                }");
            script.AppendLine(@"            }");            
            script.AppendLine(string.Format(@"            try {{ if ($get('{0}').value=='') {{ alert('An error occurred while trying to save. Please reload the Contract information and try again.'); }} }} catch (e) {{ alert('An error occurred while trying to save. Please reload the Contract information and try again.'); }}", contractSearch.TaggerContractNumber.ClientID));
            script.AppendLine(string.Format(@"            hash[""{0}""] = ($get('{1}').value=='') ? -1 : $get('{1}').value;", "CONTRACT_NUMBER", contractSearch.TaggerContractNumber.ClientID));
            script.AppendLine(string.Format(@"            hash[""{0}""] = lineItemNumbers;", "LINE_ITEM_NUMBER"));            
            script.AppendLine(string.Format(@"            hash[""{0}""] = $get('{1}').value;", "COMPANYID", companyId.ClientID));
            script.AppendLine(string.Format(@"            hash[""SALES_MARKET_OVERRIDE""] = $get('{0}').value;", dropDownSubMarketOverride.ClientID));
            script.AppendLine(string.Format(@"            hash[""STATION_MARKET_ID""] = $get('{0}').value;", dropDownStationMarket.ClientID));
            script.AppendLine(string.Format(@"            hash[""STATION_ID""] = $get('{0}').value;", dropDownStation.ClientID));
            script.AppendLine(string.Format(@"            hash[""ETHNICITY_ID""] = $get('{0}').value;", dropDownEthnicity.ClientID));
            script.AppendLine(@"        }");
            script.AppendLine(@"        if (!perfomedTagging) {alert('Please choose a Contract and Contract Line to tag the images to.');return;}");
            script.AppendLine(@"        if (hash[""ID""]=='') {alert('Please choose an image to tag.');return;}");
            script.AppendLine(@"        if (lineItemNumbers=='') {alert('Please choose a Contract Line to tag the images to.');return;}");                        
            script.AppendLine(string.Format(@"        Apollo.DigitalLibraryService.TagSelected(hash,{0},{1});", saveCallbackFunctionName, saveErrorFunctionName));
            script.AppendLine(@"    } catch (e) { alert('Error Number: '+e.number+'\\nError Desc: '+e.description); }");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildUntagCallbackScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildUntagCallbackScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", untagCallbackFunctionName));
            script.AppendLine(string.Format(@"    $get('{0}').click()", taggerLoad.ClientID));
            script.AppendLine(@"    alert('Tagging details removed.');");
            //script.AppendLine(string.Format(@"  {0}();", closeFunctionName));
            script.AppendLine(@"    RefreshPage(true);");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region BuildUntagScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildUntagScript()
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine(string.Format(@"function {0}(){{", untagFunctionName));
            script.AppendLine(string.Format(@"  var id=$get('{0}').value;", textImageId.ClientID));
            script.AppendLine(@"    if (confirm('Are you sure you wish to un-tag this file?')){");
            script.AppendLine(string.Format(@"      Apollo.DigitalLibraryService.UntagDigitalLibraryFile(id,{0});", untagCallbackFunctionName));
            script.AppendLine(@"    }");
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region Extension property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public TextBox Extension
        {
            get { return textExtension; }
        }
        #endregion

        #region ImageId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public TextBox ImageId
        {
            get { return textImageId; }
        }
        #endregion

        #region ContractNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public TextBox ContractNumber
        {
            get { return textContractNumber; }
        }
        #endregion

        #region ImageIds property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public TextBox ImageIds
        {
            get { return textImageIds; }
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
                taggerBack.Style["display"] = (taggerContext == ContextEnum.TagSelected) ? "inline" : "none";
                taggerClearSelected.Style["display"] = (taggerContext == ContextEnum.TagSelected) ? "inline" : "none";
                SecureIt();
            }
        }
        #endregion

        #region PopulateScriptNames method
        /// <summary>Create the JavaScript function names for this instance of the UserControl</summary>
        private void PopulateScriptNames()
        {
            //Data load functions
            loadDetailFunctionName = string.Format("{0}LoadDetail", this.ClientID);
            loadDetailCallbackFunctionName = string.Format("{0}LoadDetailCallback", this.ClientID);
            loadContractDataFunctionName = string.Format("{0}LoadContractData", this.ClientID);
            loadContractDataCallbackFunctionName = string.Format("{0}LoadContractDataCallback", this.ClientID);
            bindDropDownsFunctionName = string.Format("{0}BindDropDowns", this.ClientID);
            saveErrorFunctionName = string.Format("{0}SaveError", this.ClientID);
            //Close/Cleanup functions
            //closeFunctionName = string.Format("{0}Close", this.ClientID);
            clearFunctionName = string.Format("{0}Clear", this.ClientID);
            clearAfterTaggingFunctionName = string.Format("{0}ClearAfterTagging", this.ClientID);
            clearContractDataFunctionName = string.Format("{0}ClearContractData", this.ClientID);
            //Untagging
            untagFunctionName = string.Format("{0}Untag", this.ClientID);
            untagCallbackFunctionName = string.Format("{0}UntagCallback", this.ClientID);
            //Saving
            updateFunctionName = string.Format("{0}Update", this.ClientID);
            saveFunctionName = string.Format("{0}Save", this.ClientID);
            saveCallbackFunctionName = string.Format("{0}SaveCallback", this.ClientID);
            //UI functions
            handleContractSelectionFunctionName = string.Format("{0}HandleContractSelection", this.ClientID);
            autoCompleteHandlerFunctionName = string.Format("{0}AutoCompleteSelectHandler", this.ClientID);
            //Activate functions
            activateFunctionName = string.Format("{0}Activate", this.ClientID);
            activateCallbackFunctionName = string.Format("{0}ActivateCallback", this.ClientID);
            activateSelectedFunctionName = string.Format("{0}ActivateSelected", this.ClientID);
            activateSelectedCallbackFunctionName = string.Format("{0}ActivateSelectedCallback", this.ClientID);
            //Deactive functions
            deactivateFunctionName = string.Format("{0}Deactivate", this.ClientID);
            deactivateCallbackFunctionName = string.Format("{0}DeactivateCallback", this.ClientID);
            deactivateSelectedFunctionName = string.Format("{0}DeactivateSelected", this.ClientID);
            deactivateSelectedCallbackFunctionName = string.Format("{0}DeactivateSelectedCallback", this.ClientID);
            //Delete functions
            deleteFunctionName = string.Format("{0}Delete", this.ClientID);
            deleteCallbackFunctionName = string.Format("{0}DeleteCallback", this.ClientID);
        }
        #endregion

        #region RegisterScriptBlocks method
        /// <summary>Register the JavaScript functions for this instance of the UserControl</summary>
        private void RegisterScriptBlocks()
        {
            //Data Load Functions
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), loadDetailFunctionName, BuildLoadDetailScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), loadDetailCallbackFunctionName, BuildLoadDetailCallbackScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), loadContractDataFunctionName, BuildLoadContractDataScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), loadContractDataCallbackFunctionName, BuildLoadContractDataCallbackScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), bindDropDownsFunctionName, BuildDropDownScripts(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), saveErrorFunctionName, BuildSaveErrorScript() , true);            
            //Close/Cleanup functions
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), clearFunctionName, BuildClearScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), clearAfterTaggingFunctionName, BuildClearAfterTaggingScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), clearContractDataFunctionName, BuildClearContractDataScript(), true);
            //Untag Functions
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), untagFunctionName, BuildUntagScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), untagCallbackFunctionName, BuildUntagCallbackScript(), true);
            //Save Functions
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), updateFunctionName, BuildUpdateScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), saveFunctionName, BuildSaveScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), saveCallbackFunctionName, BuildSaveCallbackScript(), true);
            //UI functions
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), handleContractSelectionFunctionName, BuildContractSelectionScript(), true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), autoCompleteHandlerFunctionName, BuildAutoCompleteHandlerScript(), true);
            //Activate functions
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), activateFunctionName, BuildActivateScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), activateCallbackFunctionName, BuildActivateCallbackScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), activateSelectedFunctionName, BuildActivateSelectedScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), activateSelectedCallbackFunctionName, BuildActivateSelectedCallbackScript(), true);
            //Deactivate functions
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), deactivateFunctionName, BuildDeactivateScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), deactivateCallbackFunctionName, BuildDeactivateCallbackScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), deactivateSelectedFunctionName, BuildDeactivateSelectedScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), deactivateSelectedCallbackFunctionName, BuildDeactivateSelectedCallbackScript(), true);
            //Delete functions            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), deleteFunctionName, BuildDeleteScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), deleteCallbackFunctionName, BuildDeleteCallbackScript(), true);
        }
        #endregion

        #region TaggerContext property
        /// <summary>Expose a property that can be set at design-time to specify which context this control will be used in</summary>
        /// <value>
        ///     TaggerContext will corresponed to one of the ContextEnum values:
        ///         Detail, - Used for viewing Tagging Detail for a single File/Image
        ///         TagSelected - Used for Tagging multiple Files/Images at once
        /// </value>
        [Bindable(true), Browsable(true)]
        [Category("Context"), Description("In which context should this control be used")]
        public ContextEnum TaggerContext
        {
            get { return taggerContext; }
            set { taggerContext = value; }
        }
        #endregion

        #region TaggerLoad property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public System.Web.UI.HtmlControls.HtmlInputButton TaggerLoad
        {
            get { return taggerLoad; }
        }
        #endregion

        #region TaggerUnload property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public System.Web.UI.HtmlControls.HtmlInputButton TaggerUnload
        {
            get { return taggerUnload; }
        }
        #endregion

        #region SecureIt method
        /// <summary>TBD</summary>
        private void SecureIt()
        {
            bool isDigitalLibraryUser = (Security.IsDigitalUser() || Security.IsAdminUser() || Security.IsCorporateUser());            
            taggerDelete.Style["display"] = (isDigitalLibraryUser) ? "inline" : "none";            
            taggerSave.Style["display"] = (isDigitalLibraryUser) ? "inline" : "none";
            addNonRevLink.Style["display"] = (isDigitalLibraryUser) ? "inline" : "none";            
            checkHeroQuality.Enabled = isDigitalLibraryUser;
            checkMarketingQuality.Enabled = isDigitalLibraryUser;
            radioInstaller.Enabled = isDigitalLibraryUser;
            radioPhotographer.Enabled = isDigitalLibraryUser;
            dropDownDocumentType.Enabled = isDigitalLibraryUser;
            dropDownStation.Enabled = isDigitalLibraryUser;
            dropDownStationMarket.Enabled = isDigitalLibraryUser;
            textNotes.ReadOnly = !isDigitalLibraryUser;
        }
        #endregion

    }

}
