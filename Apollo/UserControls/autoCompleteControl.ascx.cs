using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class UserControls_autoCompleteControl : System.Web.UI.UserControl
    {
        private string url { get; set; }
        private string callback { get; set; }
        private string defaultText { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string script = string.Format(@"jQuery(document).ready(function(){{"
                + @"CreateAutoComplete({{ elId: '{0}', url: 'services/AutoCompleteService.asmx/ContractAutoComplete', dependencies: {{'companyId':'foo'}}, callback: {1}, defaultText: '{2}'}});"
                + @"}});", this.autoCompleteContainer.ClientID, this.callback, this.defaultText);
            //CreateAutoComplete({ elId: "textContractSearchAutocomplete", url: "../services/AutoCompleteService.asmx/ContractAutoComplete", dependencies: { "companyId": "dropDownContractSearchCompany" }, callback: onACSelectCallback, defaultText: " - Search For Contract - " });
            //this.Page.ClientScript.RegisterClientScriptInclude("autoCompleteFunctions", "~/UserControls/autoComplete.js");
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "acStartupScript", script, true);
        }

        [Bindable(true), Browsable(true)]
        [DefaultValue("")]
        [Category("URL"), Description("Which ServiceMethod should the Autocomplete control invoke")]
        public string URL
        {
            get { return this.url; }
            set { this.url = value; }
        }

        [Bindable(true), Browsable(true)]
        [DefaultValue("")]
        [Category("Callback"), Description("Which Callback method should be invoked")]
        public string Callback
        {
            get { return this.callback; }
            set { this.callback = value; }
        }

        [Bindable(true), Browsable(true)]
        [DefaultValue("")]
        [Category("DefaultText"), Description("The DefaultText to display in the Autocomplete text box")]
        public string DefaultText
        {
            get { return this.defaultText; }
            set { this.defaultText = value; }
        }
    }
}