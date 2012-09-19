using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
namespace Apollo
{
    public partial class sales_master_data_aeDrawsPaymentsFlatRates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int MaximumFlatRatePercentage()
        {
            return WebCommon.MaximumFlatRatePercentage;
        }

        #region Page_Init method
        /// <summary>TBD</summary>
        protected void Page_Init()
        {
            if (!Security.IsAdminUser() && !Security.IsCorporateUser())
            {
                WebCommon.ShowAlert("You are not authorized to use this portion of the application.");
                Server.Transfer("/Default.aspx");
            }
        }
        #endregion
    }
}