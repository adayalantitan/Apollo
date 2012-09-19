#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Text;
using AjaxControlToolkit;
using Titan.Email;
using Titan.DataIO;
using System.Configuration;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for TitanADService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class TitanADService : System.Web.Services.WebService
    {

        #region ADProperties enumeration
        public enum ADProperties
        {
            Company,
            Market,
            UserFullName,
            UserEmail,
        }
        #endregion

        #region Default constructor
        /// <summary>TBD</summary>
        public TitanADService()
        {
        }
        #endregion

        public static bool UseSqlServerAuthentication
        {
            get
            {
                return (Convert.ToInt32(ConfigurationManager.AppSettings["useSqlServerADAuthentication"]) != 0);
            }
        }

        #region CheckRoleForUser method
        /// <summary>TBD</summary>
        /// <param name="userId">TBD</param>
        /// <param name="roleName">TBD</param>
        /// <returns>TBD</returns>
        public static bool CheckRoleForUser(string userId, string roleName)
        {
            if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(roleName))
            {
                return false;
            }
            if (UseSqlServerAuthentication)
            {
                return CheckRoleForUserSqlServer(userId, roleName);
            }
            PrincipalContext adContext = new PrincipalContext(ContextType.Domain, "TITANOUTDOOR");
            UserPrincipal user = UserPrincipal.FindByIdentity(adContext, userId);
            foreach (Principal group in user.GetGroups()) //user.GetAuthorizationGroups())
            {
                if (String.Compare(group.Name, roleName, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        public static bool CheckRoleForUserSqlServer(string userId, string groupName)
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return (0 != Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromSql("SELECT COUNT(*) FROM _ActiveDirectoryGroups WHERE UserId='{0}' AND GroupName='{1}'", userId, groupName))));
            }
        }

        #region GetADProperty method
        /// <summary>TBD</summary>
        /// <param name="userId">TBD</param>
        /// <param name="property">TBD</param>
        /// <returns>TBD</returns>
        public static string GetADProperty(string userId, ADProperties property)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return string.Empty;
            }
            DirectoryEntry entry = new DirectoryEntry("LDAP://SV-NYC-DC03");
            DirectorySearcher adSearcher = new DirectorySearcher(entry);
            adSearcher.Filter = "(& (samaccountname=" + userId.ToLower() + ")(objectClass=user))";
            adSearcher.PropertiesToLoad.Add("displayName");
            adSearcher.PropertiesToLoad.Add("mail");
            adSearcher.PropertiesToLoad.Add("co");
            adSearcher.PropertiesToLoad.Add("physicalDeliveryOfficeName");
            adSearcher.PropertyNamesOnly = true;
            SearchResult user = adSearcher.FindOne();
            if (user == null)
            {
                return string.Empty;
            }
            switch (property)
            {
                case ADProperties.Company:
                    return (string)user.GetDirectoryEntry().Properties["co"].Value;
                case ADProperties.Market:
                    return (string)user.GetDirectoryEntry().Properties["physicalDeliveryOfficeName"].Value;
                case ADProperties.UserEmail:
                    return (string)user.GetDirectoryEntry().Properties["mail"].Value;
                case ADProperties.UserFullName:
                    return (string)user.GetDirectoryEntry().Properties["displayName"].Value;
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region GetFullUserNameFromId method
        /// <summary>TBD</summary>
        /// <param name="userId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string GetFullUserNameFromId(string userId)
        {
            return GetADProperty(userId, ADProperties.UserFullName);
        }
        #endregion

        #region GetGroupMembers method
        /// <summary>TBD</summary>
        /// <param name="groupName">TBD</param>
        /// <!--<param name="userId">TBD</param>-->
        /// <!--<param name="roleName">TBD</param>-->
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetGroupMembers(string groupName)
        {
            //if (String.IsNullOrEmpty(groupName))
            //{
            //    return string.Empty;
            //}
            ArrayList users = new ArrayList();
            PrincipalContext adContext = new PrincipalContext(ContextType.Domain, "TITANOUTDOOR");
            GroupPrincipal group = GroupPrincipal.FindByIdentity(adContext, groupName);
            foreach (Principal user in group.GetMembers())
            {
                users.Add(user.DisplayName);
            }
            return (string[])users.ToArray(typeof(string));
        }
        #endregion

        #region GetUserEmailFromId method
        /// <summary>TBD</summary>
        /// <param name="userId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string GetUserEmailFromId(string userId)
        {
            return GetADProperty(userId, ADProperties.UserEmail);
        }
        #endregion

    }

}
