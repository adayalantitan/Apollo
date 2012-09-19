using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Apollo
{
    public partial class digital_digitalLibraryUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("DragNDropTest.aspx", false);
            if (!IsPostBack)
            {
                contractSearchCloseButton.Value = contractSearch.CloseModal.ClientID;                
                contractSearch.ContractLoadCallbackFunctionName.Value = "LoadContractDetail";
                nonRevLine.CallbackFunctionName.Value = "LoadContractDetail";
                nonRevContract.CallbackFunctionName.Value = "LoadContractDetail";
            }
        }
    }
}