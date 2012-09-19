using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Apollo
{
    public partial class digitalAvails_DigitalAvailsPrintPreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQueryParams();
            }
        }

        public void LoadQueryParams()
        {
            //set defaults
            market.Value = "Philadelphia";
            numberOfMonths.Value = "3";
            station.Value = "-1";
            startMonth.Value = Convert.ToString(DateTime.Now.Month);
            year.Value = Convert.ToString(DateTime.Now.Year);
            if (Request.QueryString.Count > 0)
            {
                //Market
                market.Value = Request.QueryString["market"];
                //Number of Months
                int numMonths;
                if (!int.TryParse(Request.QueryString["numMonths"], out numMonths))
                {
                    numMonths = 3;
                }
                numberOfMonths.Value = Convert.ToString(numMonths);
                //Station Id
                int stationId;
                if (!String.IsNullOrEmpty(Request.QueryString["stationId"]))
                {
                    if (int.TryParse(Request.QueryString["stationId"], out stationId))
                    {
                        station.Value = Convert.ToString(stationId);
                    }
                }
                //Start Month
                int startingMonth;                
                if (!String.IsNullOrEmpty(Request.QueryString["startMonth"]))
                {
                    if (int.TryParse(Request.QueryString["startMonth"], out startingMonth))
                    {
                        startMonth.Value = Convert.ToString(startingMonth);
                    }
                }
                //Year
                int startYear;                
                if (!String.IsNullOrEmpty(Request.QueryString["year"]))
                {
                    if (int.TryParse(Request.QueryString["year"], out startYear))
                    {
                        year.Value = Convert.ToString(startYear);
                    }
                }                
            }
        }
    }
}