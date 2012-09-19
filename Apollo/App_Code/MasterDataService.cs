#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Apollo.MasterData;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for MasterDataService
    /// </summary>
    [WebService(Namespace = "")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class MasterDataService : System.Web.Services.WebService
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public MasterDataService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }
        #endregion

        #region AddAECommissionAmountRecord method
        /// <summary>TBD</summary>
        /// <param name="aeCommissionAmount">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void AddAECommissionAmountRecord(AECommissionAmount aeCommissionAmount)
        {
            try
            {
                aeCommissionAmount.Save();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to add the Commission Amount record.");
            }
        }
        #endregion

        #region AddAEDrawPaymentRecord method
        /// <summary>TBD</summary>
        /// <param name="aeDrawPayment">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void AddAEDrawPaymentRecord(AEDrawPayment aeDrawPayment)
        {
            try
            {
                aeDrawPayment.Save();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Draw/Payment record.");
            }
        }
        #endregion

        #region AddAEFlatRateRecord method
        /// <summary>TBD</summary>
        /// <param name="aeFlatRate">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void AddAEFlatRateRecord(AEFlatRate aeFlatRate)
        {
            try
            {
                aeFlatRate.Save();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Flat Rate record.");
            }
        }
        #endregion

        #region CheckExistingCommissionAmountYear method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="year">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool CheckExistingCommissionAmountYear(string aeId, int companyId, int year)
        {
            try
            {
                return AECommissionAmount.CheckExistingCommissionAmountYear(aeId, companyId, year);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while checking for existing commission amount years.");
            }
        }
        #endregion

        #region CheckExistingFlatRateEffectiveDate method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="flatRateEffectiveDate">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public bool CheckExistingFlatRateEffectiveDate(string aeId, int companyId, DateTime flatRateEffectiveDate)
        {
            try
            {
                return AEFlatRate.CheckExistingFlatRateEffectiveDate(aeId, companyId, flatRateEffectiveDate);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while checking for existing effective dates.");
            }
        }
        #endregion

        #region DeleteAECommissionAmountRecord method
        /// <summary>TBD</summary>
        /// <param name="aeCommissionAmountId">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeleteAECommissionAmountRecord(int aeCommissionAmountId)
        {
            try
            {
                AECommissionAmount.DeleteRecord(aeCommissionAmountId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to delete the Draw/Payment record.");
            }
        }
        #endregion

        #region DeleteAEDrawPaymentRecord method
        /// <summary>TBD</summary>
        /// <param name="aeDrawPaymentId">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeleteAEDrawPaymentRecord(int aeDrawPaymentId)
        {
            try
            {
                AEDrawPayment.DeleteRecord(aeDrawPaymentId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to delete the Draw/Payment record.");
            }
        }
        #endregion

        #region DeleteAEFlatRateRecord method
        /// <summary>TBD</summary>
        /// <param name="aeFlatRateId">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void DeleteAEFlatRateRecord(int aeFlatRateId)
        {
            try
            {
                AEFlatRate.DeleteRecord(aeFlatRateId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to delete the Flat Rate record.");
            }
        }
        #endregion

        #region GetAECommissionAmountRecord method
        /// <summary>TBD</summary>
        /// <param name="aeCommissionAmountId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public AECommissionAmount GetAECommissionAmountRecord(int aeCommissionAmountId)
        {
            try
            {
                return new AECommissionAmount(aeCommissionAmountId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Commission Amount record.");
            }
        }
        #endregion

        #region GetAECommissionAmounts method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<AECommissionAmount> GetAECommissionAmounts(string aeId, int companyId)
        {
            try
            {
                return AECommissionAmount.GetAECommissionAmounts(aeId, companyId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Commission Amount data.");
            }
        }
        #endregion

        #region GetAEDrawPaymentRecord method
        /// <summary>TBD</summary>
        /// <param name="aeDrawPaymentId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public AEDrawPayment GetAEDrawPaymentRecord(int aeDrawPaymentId)
        {
            try
            {
                return new AEDrawPayment(aeDrawPaymentId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Draw/Payment record.");
            }
        }
        #endregion

        #region GetAEDrawPayments method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<AEDrawPayment> GetAEDrawPayments(string aeId, int companyId)
        {
            try
            {
                return AEDrawPayment.GetAEDrawPayments(aeId, companyId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Draw/Payment data.");
            }
        }
        #endregion

        #region GetAEFlatRateRecord method
        /// <summary>TBD</summary>
        /// <param name="aeFlatRateId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public AEFlatRate GetAEFlatRateRecord(int aeFlatRateId)
        {
            try
            {
                return new AEFlatRate(aeFlatRateId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Flat Rate record.");
            }
        }
        #endregion

        #region GetAEFlatRates method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<AEFlatRate> GetAEFlatRates(string aeId, int companyId)
        {
            try
            {
                return AEFlatRate.GetAEFlatRates(aeId, companyId);
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Flat Rate data.");
            }
        }
        #endregion

        #region UpdateAECommissionAmountRecord method
        /// <summary>TBD</summary>
        /// <param name="aeCommissionAmount">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void UpdateAECommissionAmountRecord(AECommissionAmount aeCommissionAmount)
        {
            try
            {
                aeCommissionAmount.Save();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Commission Amount record.");
            }
        }
        #endregion

        #region UpdateAEDrawPaymentRecord method
        /// <summary>TBD</summary>
        /// <param name="aeDrawPayment">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void UpdateAEDrawPaymentRecord(AEDrawPayment aeDrawPayment)
        {
            try
            {
                aeDrawPayment.Save();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Draw/Payment record.");
            }
        }
        #endregion

        #region UpdateAEFlatRateRecord method
        /// <summary>TBD</summary>
        /// <param name="aeFlatRate">TBD</param>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public void UpdateAEFlatRateRecord(AEFlatRate aeFlatRate)
        {
            try
            {
                aeFlatRate.Save();
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                throw new Exception("An error occurred while trying to retrieve the Flat Rate record.");
            }
        }
        #endregion

    }

}
