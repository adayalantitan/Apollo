using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace Apollo
{
    /// <summary>TBD</summary>
    public sealed class ApolloApp
    {

        #region Member variables
        /// <summary>TBD</summary>
        private ICacheManager appCache = CacheFactory.GetCacheManager();
        /// <summary>TBD</summary>
        private static readonly ApolloApp instance = new ApolloApp();
        #endregion

        #region Default constructor
        /// <summary>TBD</summary>
        private ApolloApp() { }
        #endregion

        #region AddLongTermItemToCache method
        /// <summary>TBD</summary>
        /// <param name="cacheKey">TBD</param>
        /// <param name="cacheItem">TBD</param>
        public void AddLongTermItemToCache(string cacheKey, object cacheItem)
        {
            AddToCache(cacheKey, cacheItem, LongTermCacheItem);
        }
        #endregion

        #region AddShortTermItemToCache method
        /// <summary>TBD</summary>
        /// <param name="cacheKey">TBD</param>
        /// <param name="cacheItem">TBD</param>
        public void AddShortTermItemToCache(string cacheKey, object cacheItem)
        {
            AddToCache(cacheKey, cacheItem, ShortTermCacheItem);
        }
        #endregion

        #region AddToCache method
        /// <summary>TBD</summary>
        /// <param name="cacheKey">TBD</param>
        /// <param name="cacheItem">TBD</param>
        /// <param name="expiration">TBD</param>
        public void AddToCache(string cacheKey, object cacheItem, ICacheItemExpiration[] expiration)
        {
            if (appCache.Contains(cacheKey))
            {
                appCache.Remove(cacheKey);
            }
            appCache.Add(cacheKey, cacheItem, CacheItemPriority.High, null, expiration);
        }
        #endregion

        #region GetCachedItem method
        /// <summary>TBD</summary>
        /// <param name="cacheKey">TBD</param>
        /// <returns>TBD</returns>
        public object GetCachedItem(string cacheKey)
        {
            if (appCache.Contains(cacheKey))
            {
                return appCache.GetData(cacheKey);
            }
            return null;
        }
        #endregion

        #region Instance property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public static ApolloApp Instance
        {
            get { return instance; }
        }
        #endregion

        public bool IsAdminUser
        {
            get
            {
                if (this.IsSupportUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["adminUserGroup"]);
            }
        }

        public bool IsAgedRevenueFlashUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["agedRevFlashUserGroup"]);
            }
        }

        public bool IsSupportUser
        {
            get
            {
                string[] supportUserIds = ConfigurationManager.AppSettings["devSupportUsers"].Split(';');
                foreach (string userId in supportUserIds)
                {
                    if (String.Compare(userId, Security.GetCurrentUserId, true) == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsCollectionsUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["collectionsUserGroup"]);
            }
        }

        public bool IsCorporateUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["corporateUserGroup"]);
            }
        }

        public bool IsDigitalAvailsAdminUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalAvailsAdminGroup"]);
            }
        }

        public bool IsDigitalAvailsAtlantaUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalAvailsAtlantaGroup"]);
            }
        }
        public bool IsDigitalAvailsChicagoUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalAvailsChicagoGroup"]);
            }
        }

        public bool IsDigitalAvailsNewJerseyUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalAvailsNewJerseyGroup"]);
            }
        }
        public bool IsDigitalAvailsPhiladelphiaUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalAvailsPhiladelphiaGroup"]);
            }
        }
        public bool IsDigitalAvailsNewYorkUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalAvailsNewYorkGroup"]);
            }
        }
        public bool IsDigitalAvailsTorontoUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalAvailsTorontoGroup"]);
            }
        }
        public bool IsDigitalUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["digitalUserGroup"]);
            }
        }
        public bool IsMarketingUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["marketingGroup"]);
            }
        }
        public bool IsSalesCoordinator
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["salesCoordinatorsGroup"]);
            }
        }
        public bool IsContractAdminTeam
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["contractAdminGroup"]);
            }
        }
        public bool IsSalesFlashFullAccessUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["salesFlashFullUserGroup"]);
            }
        }
        public bool IsSalesUser
        {
            get
            {
                if (this.IsSupportUser || this.IsAdminUser) { return true; }
                return IsUserInRole(ConfigurationManager.AppSettings["salesUserGroup"]);
            }
        }

        public bool IsUserInRole(string roleName)
        {
            object isUserInRole = WebCommon.GetSessionValue(string.Format("is{0}User", roleName), null);
            if (isUserInRole == null)
            {
                isUserInRole = TitanADService.CheckRoleForUser(Security.GetCurrentUserId, roleName);
                WebCommon.SetSessionState(string.Format("is{0}User", roleName), isUserInRole);
                return (bool)isUserInRole;
            }
            else
            {
                return (bool)isUserInRole;
            }
        }

        #region ItemExistsInCache method
        /// <summary>TBD</summary>
        /// <param name="cacheKey">TBD</param>
        /// <returns>TBD</returns>
        public bool ItemExistsInCache(string cacheKey)
        {
            if (appCache.Contains(cacheKey) && appCache[cacheKey] != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region LongTermCacheItem property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public ICacheItemExpiration[] LongTermCacheItem
        {
            get
            {
                AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(0, 2, 0, 0));
                return new ICacheItemExpiration[] { expiry };
            }
        }
        #endregion

        #region ShortTermCacheItem property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public ICacheItemExpiration[] ShortTermCacheItem
        {
            get
            {
                AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(0, 0, 15, 0));
                return new ICacheItemExpiration[] { expiry };
            }
        }
        #endregion

    }
}