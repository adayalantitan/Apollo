using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class digital_popups_mapPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string iFrameUrl = "http://maps.google.com/maps?q=@{0},{1}&output=embed";
                decimal latitude = -1, longitude = -1;
                if (!decimal.TryParse(Request.QueryString["lat"],out latitude))
                {
                    latitude = -1;
                }
                if (!decimal.TryParse(Request.QueryString["long"], out longitude))
                {
                    longitude = -1;
                }
                if (latitude != -1 && longitude != -1)
                {
                    mapIFrame.Attributes["src"] = string.Format(iFrameUrl, latitude, longitude);
                }
            }
        }
    }
}