using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class photogallery_photoGalleryImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string name = Request.QueryString["i"] ?? "";
                string path = Request.QueryString["p"] ?? "";
                photoName.Value = name;
                photoPath.Value = path;                
            }
        }
    }
}