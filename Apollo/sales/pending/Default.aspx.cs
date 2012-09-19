#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class sales_pending_Default : System.Web.UI.Page
    {

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isProd.Value = (WebCommon.InTestingEnvironment() ? "0" : "-1");
            }
        }
        #endregion
    }
}
