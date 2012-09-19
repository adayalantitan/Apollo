<%@ Application Language="C#" %>
<%@ Import Namespace="Apollo" %>
<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        Apollo.App.AppStart();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        Apollo.App.AppEnd();                        
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        try
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            Apollo.WebCommon.LogExceptionInfo(ex);
            Server.ClearError();            
            //TODO: transfer to friendly page
            Response.Redirect("/Error.aspx", false);            
        }
        catch (System.Threading.ThreadAbortException)
        {
        }
        catch (Exception)
        {
            //TODO: Handle un-loggable error
            Response.Redirect("/Error.aspx", false);            
        }        
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        try
        {
            WebCommon.SetSessionState("currentUserCompany", TitanADService.GetADProperty(Security.GetCurrentUserId, Apollo.TitanADService.ADProperties.Company));
            WebCommon.SetSessionState("currentUserMarket", TitanADService.GetADProperty(Security.GetCurrentUserId, Apollo.TitanADService.ADProperties.Market));
            WebCommon.SetSessionState("currentUserAEId", Security.UserAEId);
        }
        catch (Exception ex)
        {
            WebCommon.LogExceptionInfo(ex);
        }
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.        
        Session.Clear();
    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        /* Fix for the Flash Player Cookie bug in Non-IE browsers.
         * Since Flash Player always sends the IE cookies even in FireFox
         * we have to bypass the cookies by sending the values as part of the POST or GET
         * and overwrite the cookies with the passed in values.
         * 
         * The theory is that at this point (BeginRequest) the cookies have not been read by
         * the Session and Authentication logic and if we update the cookies here we'll get our
         * Session and Authentication restored correctly
         */

        try
        {
            string session_param_name = "ASPSESSID";
            string session_cookie_name = "ASP.NET_SESSIONID";

            if (HttpContext.Current.Request.Form[session_param_name] != null)
            {
                UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
            }
            else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
            {
                UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
            }
        }
        catch (Exception)
        {
            Response.StatusCode = 500;
            Response.Write("Error Initializing Session");
        }

        try
        {
            string auth_param_name = "AUTHID";
            string auth_cookie_name = FormsAuthentication.FormsCookieName;

            if (HttpContext.Current.Request.Form[auth_param_name] != null)
            {
                UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
            }
            else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
            {
                UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
            }

        }
        catch (Exception)
        {
            Response.StatusCode = 500;
            Response.Write("Error Initializing Forms Authentication");
        }
    }

    void UpdateCookie(string cookie_name, string cookie_value)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
        if (cookie == null)
        {
            cookie = new HttpCookie(cookie_name);
            HttpContext.Current.Request.Cookies.Add(cookie);
        }
        cookie.Value = cookie_value;
        HttpContext.Current.Request.Cookies.Set(cookie);
    }
</script>
