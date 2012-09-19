using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class digital_DragNDropTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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