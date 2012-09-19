#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class UserControls_searchPopup : System.Web.UI.UserControl
    {

        #region Member variables
        /// <summary>JavaScript function that executes when a selection is made from the AutoComplete list</summary>
        string autoCompleteSelectHandlerFunctionName;
        /// <summary>JavaScript function to clear the TextBox</summary>
        string clearFunctionName;
        /// <summary>JavaScript function to open the search Grid</summary>
        string gridOpenFunctionName;
        #endregion

        #region AutoCompleteIdIndex property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Bindable(true), Browsable(true)]
        [DefaultValue("0")]
        [Description("Autocomplete strings are in the format xxx - xxx. The Id index determines which position (separated by - ) the Id value is in")]
        public string AutoCompleteIdIndex
        {
            get { return autoCompleteIdIndex.Value; }
            set { autoCompleteIdIndex.Value = value; }
        }
        #endregion

        #region AutoCompleteNameIndex property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Bindable(true), Browsable(true)]
        [DefaultValue("1")]
        [Description("Autocomplete strings are in the format xxx - xxx. The Name index determines which position (separated by - ) the Name value is in")]
        public string AutoCompleteNameIndex
        {
            get { return autoCompleteNameIndex.Value; }
            set { autoCompleteNameIndex.Value = value; }
        }
        #endregion

        #region BehaviorID property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField BehaviorID
        {
            get { return behaviorID; }
        }
        #endregion

        #region BuildAutoCompleteSelectHandlerScript method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string BuildAutoCompleteSelectHandlerScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append(string.Format(@"function {0}(source,eventArgs){{", autoCompleteSelectHandlerFunctionName));
            script.Append(@"    var selectedName = eventArgs.get_value();");
            script.Append(@"    var selectedNameSplit = selectedName.split('-');");
            script.Append(string.Format(@"    var id = trimValue(selectedNameSplit[{0}]);", autoCompleteIdIndex.Value));
            script.Append(string.Format(@"    var name = trimValue(selectedNameSplit[{0}]);", autoCompleteNameIndex.Value));
            script.Append(string.Format(@"    $get('{0}').value=id;", textId.ClientID));
            script.Append(string.Format(@"    $get('{0}').value=name;", textName.ClientID));
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
            script.Append(string.Format(@"function {0}(name){{", clearFunctionName));
            script.Append(string.Format(@"    if(name.value==''){{$get('{0}').value='';}}", textId.ClientID));
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
            script.AppendLine(string.Format(@"function {0}(){{", gridOpenFunctionName));
            script.AppendLine(string.Format(@"    var filterObject = BuildFilterObjectByContext('{0}',$get('{1}').value,$get('{2}').value,'{3}','{4}');", gridContext.Value, dependencyValue.ClientID, dependencyId.ClientID, (companyId.Value == "1" ? "Titan Outdoor US" : "Titan Outdoor Canada"), companyId.Value));
            script.AppendLine(string.Format(@"    DisplaySearchGrid('{0}',$get('{1}'),$get('{2}'),filterObject);", gridContext.Value, textId.ClientID, textName.ClientID));
            script.AppendLine(@"}");
            return script.ToString();
        }
        #endregion

        #region DependencyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField DependencyId
        {
            get { return dependencyId; }
        }
        #endregion

        #region DependencyValue property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField DependencyValue
        {
            get { return dependencyValue; }
        }
        #endregion

        public HiddenField CompanyId
        {
            get { return companyId; }
        }

        #region GridContext property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Bindable(true), Browsable(true)]
        [DefaultValue("")]
        [Description("What kind of data will be pulled by this popup.")]
        public string GridContext
        {
            get { return gridContext.Value; }
            set { gridContext.Value = value; }
        }
        #endregion

        #region Id property
        /// <summary>Exposes textId to pages containing this control</summary>
        /// <value>TextBox IdValue</value>
        public TextBox Id
        {
            get { return textId; }
        }
        #endregion

        #region Name property
        /// <summary>Exposes textName to pages containing this control</summary>
        /// <value>TextBox NameValue</value>
        public TextBox Name
        {
            get { return textName; }
        }
        #endregion

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            gridOpenFunctionName = string.Format("{0}OpenGrid", this.ClientID);
            autoCompleteSelectHandlerFunctionName = string.Format("{0}AutoCompleteSelectHandler", this.ClientID);
            clearFunctionName = string.Format("{0}Clear", this.ClientID);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), gridOpenFunctionName, BuildGridOpenScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), autoCompleteSelectHandlerFunctionName, BuildAutoCompleteSelectHandlerScript(), true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), clearFunctionName, BuildClearScript(), true);
            searchClick.Attributes["onclick"] = string.Format("{0}();", gridOpenFunctionName);
            textName_AutoCompleteExtender.BehaviorID = string.Format("{0}autoCompleteBehavior", this.ClientID);
            textName_AutoCompleteExtender.OnClientItemSelected = autoCompleteSelectHandlerFunctionName;
            textName.Attributes["onchange"] = string.Format("{0}(this);", clearFunctionName);
            this.behaviorID.Value = textName_AutoCompleteExtender.BehaviorID;
        }
        #endregion

        #region SearchImageAlt property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Bindable(true), Browsable(true)]
        [DefaultValue("Click to Search")]
        [Description("Set ToolTip of the Search image")]
        public string SearchImageAlt
        {
            get { return searchClick.Attributes["alt"]; }
            set { searchClick.Attributes["alt"] = value; }
        }
        #endregion

        #region SearchImageTitle property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Bindable(true), Browsable(true)]
        [DefaultValue("Click to Search")]
        [Description("Set ToolTip of the Search image")]
        public string SearchImageTitle
        {
            get { return searchClick.Attributes["title"]; }
            set { searchClick.Attributes["title"] = value; }
        }
        #endregion

        #region ServiceMethod property
        /// <summary>Expose a property that can be set at design-time to specify which ServiceMethod the autocomplete will use</summary>
        /// <value>
        ///     String value of ServiceMethod
        /// </value>
        [Bindable(true), Browsable(true)]
        [DefaultValue("")]
        [Category("ServiceMethod"), Description("Which ServiceMethod should the Autocomplete control invoke")]
        public string ServiceMethod
        {
            get { return textName_AutoCompleteExtender.ServiceMethod; }
            set { textName_AutoCompleteExtender.ServiceMethod = value; }
        }
        #endregion

        #region Width property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        [Bindable(true), Browsable(true)]
        [DefaultValue(100)]
        [Category("Width"), Description("Set the width of the Text Box control")]
        public Unit Width
        {
            get { return textName.Width; }
            set { textName.Width = value; }
        }
        #endregion

    }

}
