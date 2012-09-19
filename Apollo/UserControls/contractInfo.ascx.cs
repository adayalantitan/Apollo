#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
    public partial class UserControls_contractInfo : System.Web.UI.UserControl
    {

        #region approvedByDataSource_Selecting method
        /// <summary>TBD</summary>
        /// <param name="sender">TBD</param>
        /// <param name="e">TBD</param>
        protected void approvedByDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@companyId"].Value = CompanyId;
        }
        #endregion

        #region CommissionId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CommissionId
        {
            get
            {
                return (ViewState["commissionId"] == null) ? 1 : Convert.ToInt32(ViewState["commissionId"]);
            }
            set { ViewState["commissionId"] = value; }
        }
        #endregion

        #region CompanyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CompanyId
        {
            get
            {
                return (ViewState["companyId"] == null) ? 1 : Convert.ToInt32(ViewState["companyId"]);
            }
            set { ViewState["companyId"] = value; }
        }
        #endregion

        #region ContractNumberHdn property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public HiddenField ContractNumberHdn
        {
            get { return this.contractNumber; }
        }
        #endregion

        #region GetFixedPointsMax method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public int GetFixedPointsMax()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["maxFixedPointsPercentage"]);
        }
        #endregion

        #region GetMinEffectiveDate method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public string GetMinEffectiveDate()
        {
            return Convert.ToDateTime("1/1/2002").ToShortDateString();
        }
        #endregion

        #region GetNullableField method
        /// <summary>TBD</summary>
        /// <param name="fieldValue">TBD</param>
        /// <returns>TBD</returns>
        private object GetNullableField(object fieldValue)
        {
            return (!String.IsNullOrEmpty(Convert.ToString(fieldValue))) ? fieldValue : DBNull.Value;
        }
        #endregion

        #region Page_Init method
        /// <summary>TBD</summary>
        protected void Page_Init()
        {
            //approvedByDataSource.ConnectionString = WebCommon.ConnectionString;
            packageDataSource.ConnectionString = WebCommon.ConnectionString;
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
