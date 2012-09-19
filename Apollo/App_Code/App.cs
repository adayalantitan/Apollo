#region Using Statements
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Titan.DataIO;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for App
    /// </summary>
    public abstract class App
    {

        #region DataSetType enumeration
        public enum DataSetType
        {
            AdvertiserDataSetType,
            ADOffices,
            ADDepartments,
            AEDataSetType,
            AgencyDataSetType,
            AppDefaultDataSetType,
            CompanyDataSetType,
            ConsolidatedAdvertiserType,
            ContractLineYearsDataSetType,
            MarketDataSetType,
            MediaTypeDataSetType,
            MediaFormDataSetType,
            NewMediaTypeDataSetType,
            NewMediaFormDataSetType,
            ParentProductClassDataSetType,
            ProductClassDataSetType,
            ProductClassDLDataSetType,
            ProfitCenterDataSetType,
            StationDataSetType,
            UploadedByDataSetType,
        }
        #endregion

        #region PopulateApplicationCacheDelegate delegate
        /// <summary>TBD</summary>
        private delegate void PopulateApplicationCacheDelegate();
        #endregion

        #region AppEnd method
        /// <summary>
        ///     This method is called in the Application_End event
        ///     in Global.asax and flushes the cached data.
        /// </summary>
        public static void AppEnd()
        {
            //ICacheManager appCache = CacheFactory.GetCacheManager();
            //appCache.Flush();
        }
        #endregion

        #region AppStart method
        /// <summary>
        ///     This method is called in the Application_Start event
        ///     in Global.asax and caches frequently used, mostly-static DataSets
        /// </summary>
        public static void AppStart()
        {
            //
            //BeginBackgroundCaching();
        }
        #endregion

        #region BeginBackgroundCaching method
        /// <summary>TBD</summary>
        private static void BeginBackgroundCaching()
        {
            PopulateApplicationCacheDelegate appCaching = new PopulateApplicationCacheDelegate(PopulateApplicationCache);
            appCaching.BeginInvoke(null, null);
        }
        #endregion

        #region GetADDepartments method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetADDepartments()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromSql("SELECT * FROM v_AD_DEPARTMENTS ORDER BY 1"));
            }
        }
        #endregion

        #region GetADOffices method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetADOffices()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromSql("SELECT * FROM v_AD_OFFICES ORDER BY 1"));
            }
        }
        #endregion

        #region GetAdvertisers method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetAdvertisers()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetAdvertisers"));
            }
        }
        #endregion

        #region GetAEs method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetAEs()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetAEs"));
            }
        }
        #endregion

        #region GetAgencies method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetAgencies()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetAgencies"));
            }
        }
        #endregion

        #region GetAppDefaults method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetAppDefaults()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetAppDefaults"));
            }
        }
        #endregion

        #region GetCachedDataSet method
        /// <summary>TBD</summary>
        /// <param name="type">TBD</param>
        /// <returns>TBD</returns>
        public static DataSet GetCachedDataSet(DataSetType type)
        {
            //return GetCachedDataSet(Enum.GetName(typeof(DataSetType), type));
            /*
            ICacheManager appCache = CacheFactory.GetCacheManager();
            if (appCache.Contains(Enum.GetName(typeof(DataSetType), type)))
            {
                if ((DataSet)appCache.GetData(Enum.GetName(typeof(DataSetType), type)) == null)
                {
                    appCache.Remove(Enum.GetName(typeof(DataSetType), type));
                    AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(0, 3, 30, 0));
                    appCache.Add(Enum.GetName(typeof(DataSetType), type), GetDataSetFromKey(type), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
                    return GetDataSetFromKey(type);
                }
                return (DataSet)appCache.GetData(Enum.GetName(typeof(DataSetType), type));
                //return appCache[Enum.GetName(typeof(DataSetType), type)] as DataSet;
            }
            else
            {
             */
                return GetDataSetFromKey(type);
            //}
        }
        #endregion

        #region GetConsolidatedAdvertisers method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetConsolidatedAdvertisers()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetConsolidatedAdvertisers"));
            }
        }
        #endregion

        #region GetContractLineItemYears method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetContractLineItemYears()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetContractLineItemYears"));
            }
        }
        #endregion

        #region GetDataSetFromKey method
        /// <summary>TBD</summary>
        /// <param name="type">TBD</param>
        /// <returns>TBD</returns>
        public static DataSet GetDataSetFromKey(DataSetType type)
        {
            switch (type)
            {
                case DataSetType.AdvertiserDataSetType:
                    return GetAdvertisers();
                case DataSetType.ADDepartments:
                    return GetADDepartments();
                case DataSetType.ADOffices:
                    return GetADOffices();
                case DataSetType.AEDataSetType:
                    return GetAEs();
                case DataSetType.AgencyDataSetType:
                    return GetAgencies();
                case DataSetType.AppDefaultDataSetType:
                    return GetAppDefaults();
                case DataSetType.ConsolidatedAdvertiserType:
                    return GetConsolidatedAdvertisers();
                case DataSetType.ContractLineYearsDataSetType:
                    return GetContractLineItemYears();
                case DataSetType.MarketDataSetType:
                    return GetMarkets();
                case DataSetType.MediaFormDataSetType:
                    return GetMediaForms();
                case DataSetType.MediaTypeDataSetType:
                    return GetMediaTypes();
                case DataSetType.NewMediaFormDataSetType:
                    return GetNewMediaForms();
                case DataSetType.NewMediaTypeDataSetType:
                    return GetNewMediaTypes();
                case DataSetType.ParentProductClassDataSetType:
                    return GetParentProductClasses();
                case DataSetType.ProductClassDataSetType:
                    return GetProductClasses();
                case DataSetType.ProductClassDLDataSetType:
                    return GetProductClassesDL();
                case DataSetType.ProfitCenterDataSetType:
                    return GetProfitCenters();
                case DataSetType.StationDataSetType:
                    return GetStations();
                case DataSetType.UploadedByDataSetType:
                    return GetUploadedBy();
                default:
                    return null;
            }
        }
        #endregion

        #region GetMarkets method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetMarkets()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetMarkets"));
            }
        }
        #endregion

        #region GetMediaForms method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetMediaForms()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetMediaForms"));
            }
        }
        #endregion

        #region GetMediaTypes method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetMediaTypes()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetMediaTypes"));
            }
        }
        #endregion

        #region GetNewMediaForms method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetNewMediaForms()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetNewMediaForms"));
            }
        }
        #endregion

        #region GetNewMediaTypes method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetNewMediaTypes()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetNewMediaTypes"));
            }
        }
        #endregion

        #region GetParentProductClasses method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetParentProductClasses()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetParentProductClasses"));
            }
        }
        #endregion

        #region GetProductClasses method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetProductClasses()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetProductClasses"));
            }
        }
        #endregion

        #region GetProductClassesDL method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetProductClassesDL()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetDLProductClasses"));
            }
        }
        #endregion

        #region GetProfitCenters method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetProfitCenters()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetProfitCenters"));
            }
        }
        #endregion

        #region GetStations method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetStations()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("AppCache_GetStations"));
            }
        }
        #endregion

        #region GetUploadedBy method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        private static DataSet GetUploadedBy()
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                return io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("GetUniqueUploadedBy"));
            }
        }
        #endregion

        #region PopulateApplicationCache method
        /// <summary>TBD</summary>
        private static void PopulateApplicationCache()
        {
            ICacheManager appCache = CacheFactory.GetCacheManager();
            //Common Expiration time, used by cached data which is built from tables that refresh as part of the SQL Server scheduled job
            AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(0, 3, 30, 0));
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ADDepartments)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ADDepartments), GetADDepartments(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ADOffices)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ADOffices), GetADOffices(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.MarketDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.MarketDataSetType), GetMarkets(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.MediaFormDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.MediaFormDataSetType), GetMediaForms(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.NewMediaFormDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.NewMediaFormDataSetType), GetNewMediaForms(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.NewMediaTypeDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.NewMediaTypeDataSetType), GetNewMediaTypes(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.MediaTypeDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.MediaTypeDataSetType), GetMediaTypes(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.AEDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.AEDataSetType), GetAEs(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ConsolidatedAdvertiserType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ConsolidatedAdvertiserType), GetConsolidatedAdvertisers(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.AgencyDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.AgencyDataSetType), GetAgencies(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.AdvertiserDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.AdvertiserDataSetType), GetAdvertisers(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.AppDefaultDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.AppDefaultDataSetType), GetAppDefaults(), CacheItemPriority.High, null, new ICacheItemExpiration[] { new AbsoluteTime(new TimeSpan(0, 12, 0, 0)) });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ContractLineYearsDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ContractLineYearsDataSetType), GetContractLineItemYears(), CacheItemPriority.High, null, new ICacheItemExpiration[] { new AbsoluteTime(new TimeSpan(0, 12, 0, 0)) });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ProfitCenterDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ProfitCenterDataSetType), GetProfitCenters(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ParentProductClassDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ParentProductClassDataSetType), GetParentProductClasses(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ProductClassDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ProductClassDataSetType), GetProductClasses(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.ProductClassDLDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.ProductClassDLDataSetType), GetProductClassesDL(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.StationDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.StationDataSetType), GetStations(), CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
            }
            if (!appCache.Contains(Enum.GetName(typeof(DataSetType), DataSetType.UploadedByDataSetType)))
            {
                appCache.Add(Enum.GetName(typeof(DataSetType), DataSetType.UploadedByDataSetType), GetUploadedBy(), CacheItemPriority.High, null, new ICacheItemExpiration[] { new AbsoluteTime(new TimeSpan(0, 0, 30, 0)) });
            }
        }
        #endregion

    }

}
