using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class HR_newEmployeeForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                textRequestorName.Text = Security.GetFullUserNameFromId(Security.GetCurrentUserId);
                textRequestorNTID.Value = Security.GetCurrentUserId;
                textRequestDate.Text = DateTime.Now.ToShortDateString();
                textRequestorEmail.Text = Security.GetUserEmailFromId(Security.GetCurrentUserId);
            }
        }
    }
}