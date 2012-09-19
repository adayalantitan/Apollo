#region Using Statements
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for Security
    /// </summary>
    public abstract class Security
    {

        #region GetCurrentUserId property
        /// <summary>property for Current Logged in User</summary>
        /// <value>TBD</value>
        public static string GetCurrentUserId
        {
            get
            {
                //return "rcoggins";
                string[] user = HttpContext.Current.User.Identity.Name.Trim().ToUpper(System.Globalization.CultureInfo.CurrentCulture).Split('\\');                
                return user[1];
            }
        }
        #endregion

        #region GetFullUserNameFromId method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <returns>TBD</returns>
        public static string GetFullUserNameFromId(string id)
        {
            return TitanADService.GetADProperty(id, TitanADService.ADProperties.UserFullName);
        }
        #endregion

        #region GetUserEmailFromId method
        /// <summary>TBD</summary>
        /// <param name="id">TBD</param>
        /// <returns>TBD</returns>
        public static string GetUserEmailFromId(string id)
        {
            return TitanADService.GetADProperty(id, TitanADService.ADProperties.UserEmail);
        }
        #endregion

        #region IsAdminUser method
        /// <summary>Determines if the current user is in the Admin user group.</summary>
        /// <returns>True if the user is an Admin/False otherwise</returns>
        public static bool IsAdminUser()
        {
            return ApolloApp.Instance.IsAdminUser;
            if (IsSupportUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["adminUserGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsAgedRevenueFlashUser method
        /// <summary>Determines if the current user is in the Corporate user group.</summary>
        /// <returns>True if the user is an Admin/False otherwise</returns>
        public static bool IsAgedRevenueFlashUser()
        {
            return ApolloApp.Instance.IsAgedRevenueFlashUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["agedRevFlashUserGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsCollectionsUser method
        /// <summary>Determines if the current user is in the Corporate user group.</summary>
        /// <returns>True if the user is an Admin/False otherwise</returns>
        public static bool IsCollectionsUser()
        {
            return ApolloApp.Instance.IsCollectionsUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["collectionsUserGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsCorporateUser method
        /// <summary>Determines if the current user is in the Corporate user group.</summary>
        /// <returns>True if the user is an Admin/False otherwise</returns>
        public static bool IsCorporateUser()
        {
            return ApolloApp.Instance.IsCorporateUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["corporateUserGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsDigitalAvailsAdminUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsDigitalAvailsAdminUser()
        {
            return ApolloApp.Instance.IsDigitalAvailsAdminUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalAvailsAdminGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsDigitalAvailsAtlantaUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsDigitalAvailsAtlantaUser()
        {
            return ApolloApp.Instance.IsDigitalAvailsAtlantaUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalAvailsAtlantaGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsDigitalAvailsChicagoUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsDigitalAvailsChicagoUser()
        {
            return ApolloApp.Instance.IsDigitalAvailsChicagoUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalAvailsChicagoGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsDigitalAvailsNewJerseyUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsDigitalAvailsNewJerseyUser()
        {
            return ApolloApp.Instance.IsDigitalAvailsNewJerseyUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalAvailsNewJerseyGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsDigitalAvailsPhiladelphiaUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsDigitalAvailsPhiladelphiaUser()
        {
            return ApolloApp.Instance.IsDigitalAvailsPhiladelphiaUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalAvailsPhiladelphiaGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsDigitalAvailsNewYorkUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsDigitalAvailsNewYorkUser()
        {
            return ApolloApp.Instance.IsDigitalAvailsNewYorkUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalAvailsNewYorkGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        public static bool IsDigitalAvailsTorontoUser()
        {
            return ApolloApp.Instance.IsDigitalAvailsTorontoUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalAvailsTorontoGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }

        #region IsDigitalUser method
        /// <summary>Determines if the current user is in the Digital user group.</summary>
        /// <returns>True if the user is an Admin/False otherwise</returns>
        public static bool IsDigitalUser()
        {
            return ApolloApp.Instance.IsDigitalUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["digitalUserGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsMarketingUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsMarketingUser()
        {
            return ApolloApp.Instance.IsMarketingUser;
            if (IsSupportUser() || IsAdminUser() || IsDigitalUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["marketingGroup"];
            return TitanADService.CheckRoleForUser("TNSMITH", groupName);//WebCommon.GetCurrentUserId, groupName);
        }
        #endregion

        #region IsSalesCoordinator method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsSalesCoordinator()
        {
            return ApolloApp.Instance.IsSalesCoordinator;
            //Twiggy, Elaine, Stephanie Schneider
            if (IsSupportUser() || IsAdminUser()) { return true; }
            if (String.Compare(GetCurrentUserId, "ttovar", false) == 0) { return true; }
            if (String.Compare(GetCurrentUserId, "edyjak", false) == 0) { return true; }
            if (String.Compare(GetCurrentUserId, "sschneider", false) == 0) { return true; }
            string groupName = ConfigurationManager.AppSettings["salesCoordinatorsGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        public static bool IsContractAdminTeam()
        {
            return ApolloApp.Instance.IsContractAdminTeam;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["contractAdminGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }

        #region IsSalesFlashFullAccessUser method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static bool IsSalesFlashFullAccessUser()
        {
            return ApolloApp.Instance.IsSalesFlashFullAccessUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["salesFlashFullUserGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsSalesUser method
        /// <summary>Determines if the current user is in the Sales user group.</summary>
        /// <returns>True if the user is an Admin/False otherwise</returns>
        public static bool IsSalesUser()
        {
            return ApolloApp.Instance.IsSalesUser;
            if (IsSupportUser() || IsAdminUser()) { return true; }
            string groupName = ConfigurationManager.AppSettings["salesUserGroup"];
            return TitanADService.CheckRoleForUser(GetCurrentUserId, groupName);
        }
        #endregion

        #region IsSupportUser method
        /// <summary>Determines if the current user is in the web.config as a support user.</summary>
        /// <returns>True if the user is an Admin/False otherwise</returns>
        public static bool IsSupportUser()
        {
            return ApolloApp.Instance.IsSupportUser;
            string[] supportUserIds = ConfigurationManager.AppSettings["devSupportUsers"].Split(';');
            foreach (string userId in supportUserIds)
            {
                if (String.Compare(userId, GetCurrentUserId, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region UserAEId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public static string UserAEId
        {
            get
            {
                string aeId = (string)WebCommon.GetSessionValue("currentUserAEId") ?? "";
                if (!String.IsNullOrEmpty(aeId))
                {
                    return aeId;
                }
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    object result = io.ExecuteScalarQuery(IO.CreateCommandFromSql("SELECT dbo.Apollo_GetAEIdFromNTID('{0}');", GetCurrentUserId));
                    aeId = (result == DBNull.Value) ? string.Empty : Convert.ToString(result);
                    WebCommon.SetSessionState("currentUserAEId", aeId);
                }
                return aeId;
            }
        }
        #endregion

        #region UserCompanyID property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public static string UserCompanyID
        {
            get
            {
                if (string.Compare(UserCompanyName, "United States", true) == 0)
                {
                    return "1";
                }
                return "2";
            }
        }
        #endregion

        #region UserCompanyName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public static string UserCompanyName
        {
            get
            {
                string company = (string)WebCommon.GetSessionValue("currentUserCompany") ?? "";
                if (String.IsNullOrEmpty(company))
                {
                    company = TitanADService.GetADProperty(GetCurrentUserId, Apollo.TitanADService.ADProperties.Company) ?? "";
                    WebCommon.SetSessionState("currentUserCompany", company);
                }
                return company;
            }
        }
        #endregion

        #region UserMarket property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public static string UserMarket
        {
            get
            {
                string market = (string)WebCommon.GetSessionValue("currentUserMarket") ?? "";
                if (String.IsNullOrEmpty(market))
                {
                    market = TitanADService.GetADProperty(GetCurrentUserId, Apollo.TitanADService.ADProperties.Market) ?? "";
                    WebCommon.SetSessionState("currentUserMarket", market);
                }
                return market;
            }
        }
        #endregion

        #region UserMarketCode property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public static string UserMarketCode
        {
            get
            {
                if (String.Compare(UserCompanyID, "2") == 0)
                {
                    return "TOR";
                }
                switch (UserMarket.ToLower())
                {
                    case "boston":
                        return "BOS";
                    case "chicago":
                        return "CHI";
                    case "dallas":
                        return "DAL";
                    case "orange county":
                    case "los angeles":
                    case "los angeles/oc":
                    case "gardena":
                        return "LA";
                    case "minnesota":
                    case "minneapolis":
                        return "MIN";
                    case "fairfield":
                        return "NJ";
                    case "new york":
                    case "long island city":
                        return "NYO";
                    case "philadelphia":
                        return "PHI";
                    case "seattle":
                        return "SEA";
                    case "san francisco":
                        return "SF";
                    default:
                        return UserMarket;
                }
            }
        }
        #endregion

        #region UserMarketID property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public static string UserMarketID
        {
            get
            {
                string filter = "MARKET_DESCRIPTION LIKE '%{0}%'";
                string market = string.Empty;
                DataSet markets = App.GetCachedDataSet(App.DataSetType.MarketDataSetType);
                try
                {
                    if (String.Compare(UserCompanyID, "2") == 0)
                    {
                        market = "Toronto";
                    }
                    switch (UserMarket.ToLower())
                    {
                        case "fairfield":
                            market = "New Jersey";
                            break;
                        case "orange county":
                        case "los angeles":
                        case "los angeles/oc":
                        case "gardena":
                            market = "Los Angeles";
                            break;
                        case "minnesota":
                            market = "minne";
                            break;
                        default:
                            market = UserMarket;
                            break;
                    }
                    return Convert.ToString(markets.Tables[0].Select(string.Format(filter, market))[0][0]);
                }
                catch
                {
                    return "";
                }
            }
        }
        #endregion

    }

}
