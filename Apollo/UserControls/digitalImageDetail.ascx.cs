using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class UserControls_digitalImageDetail : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool isTiffany = (String.Compare(Security.GetCurrentUserId, "TNSMITH", true) == 0);
                isAllowedWebImage.Value = (isTiffany || Security.IsAdminUser()) ? "-1" : "0";
            }
        }
    }
}