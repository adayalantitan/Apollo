#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public partial class ApolloSearchHorizSplit : System.Web.UI.MasterPage
    {

        #region HideHeader method
        /// <summary>TBD</summary>
        public void HideHeader()
        {
            Master.HideHeader();
        }
        #endregion

        #region Page_Load method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion
    }

}
