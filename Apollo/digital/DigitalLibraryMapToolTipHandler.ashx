<%@ WebHandler Language="C#" Class="Apollo.DigitalLibraryMapToolTipHandler" %>

using System;
using System.Web;
namespace Apollo {
public class DigitalLibraryMapToolTipHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        context.Response.ContentType = "text/html";        
        //string iFrameUrl = "http://maps.google.com/maps?q=@{0},{1}&output=embed";
        string iFrameUrl = @"<img src=""http://maps.google.com/maps/api/staticmap?zoom=16&size=200x200&maptype=roadmap&markers=color:blue|label:X|{0},{1}&sensor=false"" title=""Image Location"" alt=""Image Location"" />";        
        //string outputHtml = @"<div><iframe width=""300"" height=""300"" frameborder=""0"" scrolling=""no"" marginheight=""0"" marginwidth=""0"" src=""{0}""></iframe></div>";
        string outputHtml = @"<div>{0}</div>";
        decimal latitude = -1, longitude = -1;
        try
        {
            if (!decimal.TryParse(context.Request.QueryString["lat"], out latitude))
            {
                latitude = -1;
            }
            if (!decimal.TryParse(context.Request.QueryString["long"], out longitude))
            {
                longitude = -1;
            }
            if (latitude != -1 && longitude != -1)
            {
                context.Response.Write(string.Format(outputHtml, string.Format(iFrameUrl, latitude, longitude)));
            }
        }
        catch (Exception ex)
        {
            WebCommon.LogExceptionInfo(ex);
            context.Response.Write("Map info could not be retrieved");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
}