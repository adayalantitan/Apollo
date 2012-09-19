#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Titan.DataIO;
#endregion

namespace Apollo.MasterData
{

    /// <summary>TBD</summary>
    public class AECommissionAmount
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public AECommissionAmount()
        {
        }
        #endregion

        #region Parameterized constructor (aeCommissionAmountId)
        /// <summary>TBD</summary>
        /// <param name="aeCommissionAmountId">TBD</param>
        public AECommissionAmount(int aeCommissionAmountId)
        {
            this.AECommissionAmountId = aeCommissionAmountId;
            this.LoadAECommissionAmount();
        }
        #endregion

        #region Parameterized constructor (aeCommissionAmountId, aeId, commissionYear, commissionAmount, entryDate, dateLastModified, enteredBy, companyId)
        /// <summary>TBD</summary>
        /// <param name="aeCommissionAmountId">TBD</param>
        /// <param name="aeId">TBD</param>
        /// <param name="commissionYear">TBD</param>
        /// <param name="commissionAmount">TBD</param>
        /// <param name="entryDate">TBD</param>
        /// <param name="dateLastModified">TBD</param>
        /// <param name="enteredBy">TBD</param>
        /// <param name="companyId">TBD</param>
        public AECommissionAmount(int aeCommissionAmountId, string aeId, int commissionYear, double commissionAmount, DateTime entryDate, DateTime dateLastModified, string enteredBy, int companyId)
        {
            this.AECommissionAmountId = aeCommissionAmountId;
            this.AEId = aeId;
            this.CommissionYear = commissionYear;
            this.CommissionAmount = commissionAmount;
            this.EntryDate = entryDate;
            this.DateLastModified = dateLastModified;
            this.EnteredBy = enteredBy;
            this.CompanyId = companyId;
        }
        #endregion

        #region Parameterized constructor (aeId, commissionYear, commissionAmount, companyId)
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="commissionYear">TBD</param>
        /// <param name="commissionAmount">TBD</param>
        /// <param name="companyId">TBD</param>
        public AECommissionAmount(string aeId, int commissionYear, double commissionAmount, int companyId)
        {
            this.AEId = aeId;
            this.CommissionYear = commissionYear;
            this.CommissionAmount = commissionAmount;
            this.CompanyId = companyId;
            this.EntryDate = DateTime.Now;
            this.DateLastModified = DateTime.Now;
            this.EnteredBy = Security.GetCurrentUserId;
        }
        #endregion

        #region AECommissionAmountId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int AECommissionAmountId { get; set; }
        #endregion

        #region AEId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AEId { get; set; }
        #endregion

        #region CommissionAmount property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double CommissionAmount { get; set; }
        #endregion

        #region CommissionAmountDisplay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string CommissionAmountDisplay
        {
            get
            {
                return string.Format("{0:c}", this.CommissionAmount);
            }
        }
        #endregion

        #region CommissionYear property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CommissionYear { get; set; }
        #endregion

        #region CompanyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CompanyId { get; set; }
        #endregion

        #region DateLastModified property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime DateLastModified { get; set; }
        #endregion

        public string DateLastModifiedDisplay
        {
            get
            {
                return string.Format("{0} {1}", this.DateLastModified.ToShortDateString(), this.DateLastModified.ToShortTimeString());
            }
        }

        public static bool CheckExistingCommissionAmountYear(string aeId, int companyId, int year)
        {
            int count;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                count = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("MASTERDATA_CHECKEXISTINGCOMMISSIONAMOUNTYEAR",
                    Param.CreateParam("AEID", SqlDbType.VarChar, aeId),
                    Param.CreateParam("COMPANYID", SqlDbType.Int, companyId),
                    Param.CreateParam("YEAR", SqlDbType.Int, year))));
            }
            return (count > 0);
        }

        #region DeleteRecord method
        /// <summary>TBD</summary>
        /// <param name="aeCommissionAmountId">TBD</param>
        public static void DeleteRecord(int aeCommissionAmountId)
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_DELETEAECOMMISSIONAMOUNTRECORD",
                    Param.CreateParam("AECOMMISSIONAMOUNTID", SqlDbType.Int, aeCommissionAmountId),
                    Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId)));
            }
        }
        #endregion

        #region EnteredBy property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string EnteredBy { get; set; }
        #endregion

        #region EntryDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime EntryDate { get; set; }
        #endregion

        #region GetAECommissionAmounts method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public static List<AECommissionAmount> GetAECommissionAmounts(string aeId, int companyId)
        {
            List<AECommissionAmount> aeCommissionAmounts = new List<AECommissionAmount>();
            DataSet historyData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                historyData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GETCOMMISSIONAMOUNTHISTORY",
                    Param.CreateParam("AEID", SqlDbType.VarChar, aeId),
                    Param.CreateParam("COMPANYID", SqlDbType.Int, companyId)));
            }
            if (historyData.Tables[0].Rows.Count == 0)
            {
                return aeCommissionAmounts;
            }
            AECommissionAmount commissionAmount;
            foreach (DataRow commissionAmountRecord in historyData.Tables[0].Rows)
            {
                commissionAmount = new AECommissionAmount();
                commissionAmount.AECommissionAmountId = (int)commissionAmountRecord["AE_COMMISSION_AMOUNT_ID"];
                commissionAmount.AEId = Convert.ToString(IO.GetDataRowValue(commissionAmountRecord, "ACCOUNT_EXECUTIVE_ID", ""));
                commissionAmount.CommissionYear = (int)commissionAmountRecord["COMMISSION_YEAR"];
                commissionAmount.CommissionAmount = Convert.ToDouble(commissionAmountRecord["COMMISSION_AMOUNT"]);
                commissionAmount.EntryDate = Convert.ToDateTime(IO.GetDataRowValue(commissionAmountRecord, "ENTRY_DATE", DateTime.MinValue));
                commissionAmount.DateLastModified = Convert.ToDateTime(IO.GetDataRowValue(commissionAmountRecord, "DATE_LAST_MODIFIED", DateTime.MinValue));
                commissionAmount.EnteredBy = Convert.ToString(IO.GetDataRowValue(commissionAmountRecord, "ENTERED_BY", ""));
                commissionAmount.CompanyId = (int)commissionAmountRecord["COMPANY_ID"];
                aeCommissionAmounts.Add(commissionAmount);
            }
            return aeCommissionAmounts;
        }
        #endregion

        #region LoadAECommissionAmount method
        /// <summary>TBD</summary>
        private void LoadAECommissionAmount()
        {
            DataSet commissionAmountData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                commissionAmountData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GETCOMMISSIONSAMOUNTRECORD",
                    Param.CreateParam("AECOMMISSIONAMOUNTID", SqlDbType.Int, this.AECommissionAmountId)));
            }
            if (commissionAmountData.Tables.Count == 0)
            {
                return;
            }
            if (commissionAmountData.Tables[0].Rows.Count == 0)
            {
                return;
            }
            DataRow commissionAmountRecord = commissionAmountData.Tables[0].Rows[0];
            this.AEId = Convert.ToString(IO.GetDataRowValue(commissionAmountRecord, "ACCOUNT_EXECUTIVE_ID", ""));
            this.CommissionYear = (int)commissionAmountRecord["COMMISSION_YEAR"];
            this.CommissionAmount = Convert.ToDouble(commissionAmountRecord["COMMISSION_AMOUNT"]);
            this.EntryDate = Convert.ToDateTime(IO.GetDataRowValue(commissionAmountRecord, "ENTRY_DATE", DateTime.MinValue));
            this.DateLastModified = Convert.ToDateTime(IO.GetDataRowValue(commissionAmountRecord, "DATE_LAST_MODIFIED", DateTime.MinValue));
            this.EnteredBy = Convert.ToString(IO.GetDataRowValue(commissionAmountRecord, "ENTERED_BY", ""));
            this.CompanyId = (int)commissionAmountRecord["COMPANY_ID"];
        }
        #endregion

        #region Save method
        /// <summary>TBD</summary>
        public void Save()
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("COMMISSIONYEAR", SqlDbType.Int, this.CommissionYear));
            spParams.Add(Param.CreateParam("COMMISSIONAMOUNT", SqlDbType.Decimal, this.CommissionAmount));
            spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, (this.EnteredBy == null ? Security.GetCurrentUserId : this.EnteredBy)));
            string storedProcName = "";
            if (this.AECommissionAmountId == -1)
            {
                spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, this.AEId));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, this.CompanyId));
                storedProcName = "MASTERDATA_ADDAECOMMISSIONAMOUNTRECORD";
            }
            else
            {
                spParams.Add(Param.CreateParam("AECOMMISSIONAMOUNTID", SqlDbType.Int, this.AECommissionAmountId));
                storedProcName = "MASTERDATA_UPDATECOMMISSIONAMOUNTRECORD";
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc(storedProcName, spParams));
            }
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class AEDrawPayment
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public AEDrawPayment()
        {
        }
        #endregion

        #region Parameterized constructor (aeDrawPaymentId)
        /// <summary>TBD</summary>
        /// <param name="aeDrawPaymentId">TBD</param>
        public AEDrawPayment(int aeDrawPaymentId)
        {
            this.AEDrawPaymentId = aeDrawPaymentId;
            LoadAEDrawPayment();
        }
        #endregion

        #region Parameterized constructor (aeDrawPaymentId, aeId, drawPaymentType, drawPaymentAmount, drawPaymentYear, paymentMonth, paymentDate, paymentStatus, companyId)
        /// <summary>TBD</summary>
        /// <param name="aeDrawPaymentId">TBD</param>
        /// <param name="aeId">TBD</param>
        /// <param name="drawPaymentType">TBD</param>
        /// <param name="drawPaymentAmount">TBD</param>
        /// <param name="drawPaymentYear">TBD</param>
        /// <param name="paymentMonth">TBD</param>
        /// <param name="paymentDate">TBD</param>
        /// <param name="paymentStatus">TBD</param>
        /// <param name="companyId">TBD</param>
        public AEDrawPayment(int aeDrawPaymentId, string aeId, string drawPaymentType, double drawPaymentAmount, int drawPaymentYear, int paymentMonth, DateTime paymentDate, string paymentStatus, int companyId)
        {
            this.AEDrawPaymentId = aeDrawPaymentId;
            this.AEId = aeId;
            this.DrawPaymentType = drawPaymentType;
            this.DrawPaymentAmount = drawPaymentAmount;
            this.DrawPaymentYear = drawPaymentYear;
            this.PaymentMonth = paymentMonth;
            this.PaymentDate = paymentDate;
            this.PaymentStatus = paymentStatus;
            this.CompanyId = companyId;
            this.EnteredBy = Security.GetCurrentUserId;
        }
        #endregion

        #region Parameterized constructor (aeId, drawPaymentType, drawPaymentAmount, drawPaymentYear, paymentMonth, paymentDate, paymentStatus, companyId)
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="drawPaymentType">TBD</param>
        /// <param name="drawPaymentAmount">TBD</param>
        /// <param name="drawPaymentYear">TBD</param>
        /// <param name="paymentMonth">TBD</param>
        /// <param name="paymentDate">TBD</param>
        /// <param name="paymentStatus">TBD</param>
        /// <param name="companyId">TBD</param>
        public AEDrawPayment(string aeId, string drawPaymentType, double drawPaymentAmount, int drawPaymentYear, int paymentMonth, DateTime paymentDate, string paymentStatus, int companyId)
        {
            this.AEDrawPaymentId = -1;
            this.AEId = aeId;
            this.DrawPaymentType = drawPaymentType;
            this.DrawPaymentAmount = drawPaymentAmount;
            this.DrawPaymentYear = drawPaymentYear;
            this.PaymentMonth = paymentMonth;
            this.PaymentDate = paymentDate;
            this.PaymentStatus = paymentStatus;
            this.CompanyId = companyId;
            this.EnteredBy = Security.GetCurrentUserId;
        }
        #endregion

        #region AEDrawPaymentId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int AEDrawPaymentId { get; set; }
        #endregion

        #region AEId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AEId { get; set; }
        #endregion

        #region CompanyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CompanyId { get; set; }
        #endregion

        #region DateLastModified property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime DateLastModified { get; set; }
        #endregion

        #region DeleteRecord method
        /// <summary>TBD</summary>
        /// <param name="aeDrawPaymentId">TBD</param>
        public static void DeleteRecord(int aeDrawPaymentId)
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_DELETEDRAWPAYMENTRECORD",
                    Param.CreateParam("DRAWPAYMENTID", SqlDbType.Int, aeDrawPaymentId),
                    Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId)));
            }
        }
        #endregion

        #region DrawPaymentAmount property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double DrawPaymentAmount { get; set; }
        #endregion

        #region DrawPaymentAmountDisplay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string DrawPaymentAmountDisplay
        {
            get
            {
                return string.Format("{0:C}", this.DrawPaymentAmount);
            }
        }
        #endregion

        #region DrawPaymentType property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string DrawPaymentType { get; set; }
        #endregion

        #region DrawPaymentTypeDisplay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string DrawPaymentTypeDisplay
        {
            get
            {
                if (String.Compare(this.DrawPaymentType, "D", true) == 0)
                {
                    return "Draw";
                }
                return "Payment";
            }
        }
        #endregion

        #region DrawPaymentYear property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int DrawPaymentYear { get; set; }
        #endregion

        #region EnteredBy property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string EnteredBy { get; set; }
        #endregion

        #region GetAEDrawPayments method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public static List<AEDrawPayment> GetAEDrawPayments(string aeId, int companyId)
        {
            List<AEDrawPayment> aeDrawPayments = new List<AEDrawPayment>();
            DataSet historyData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                historyData = io.ExecuteDataSetQuery(IO.CreateCommandFromSql("SELECT * FROM AE_DRAW_PAYMENT WHERE LTRIM(RTRIM(ACCOUNT_EXECUTIVE_ID)) = LTRIM(RTRIM('{0}')) AND COMPANY_ID={1} ORDER BY DRAW_PAYMENT_YEAR DESC, DRAW_PAYMENT_TYPE, PAYMENT_MONTH", aeId, companyId));
            }
            if (historyData.Tables[0].Rows.Count == 0)
            {
                return aeDrawPayments;
            }
            AEDrawPayment drawPayment;
            foreach (DataRow drawPaymentRecord in historyData.Tables[0].Rows)
            {
                drawPayment = new AEDrawPayment();
                drawPayment.AEDrawPaymentId = (int)drawPaymentRecord["AE_DRAW_PAYMENT_ID"];
                drawPayment.AEId = Convert.ToString(IO.GetDataRowValue(drawPaymentRecord, "ACCOUNT_EXECUTIVE_ID", ""));
                drawPayment.DrawPaymentType = (string)drawPaymentRecord["DRAW_PAYMENT_TYPE"];
                drawPayment.DrawPaymentAmount = Convert.ToDouble(drawPaymentRecord["DRAW_PAYMENT_AMOUNT"]);
                drawPayment.DrawPaymentYear = (int)drawPaymentRecord["DRAW_PAYMENT_YEAR"];
                drawPayment.PaymentMonth = Convert.ToInt32(IO.GetDataRowValue(drawPaymentRecord, "PAYMENT_MONTH", -1));
                drawPayment.PaymentDate = Convert.ToDateTime(IO.GetDataRowValue(drawPaymentRecord, "PAYMENT_DATE", DateTime.MinValue));
                drawPayment.PaymentMonth = Convert.ToInt32(IO.GetDataRowValue(drawPaymentRecord, "PAYMENT_MONTH", -1));
                drawPayment.EnteredBy = Convert.ToString(IO.GetDataRowValue(drawPaymentRecord, "ENTERED_BY", ""));
                drawPayment.CompanyId = (int)drawPaymentRecord["COMPANY_ID"];
                drawPayment.PaymentDate = Convert.ToDateTime(IO.GetDataRowValue(drawPaymentRecord, "DATE_LAST_MODIFIED", DateTime.MinValue));
                aeDrawPayments.Add(drawPayment);
            }
            return aeDrawPayments;
        }
        #endregion

        #region LoadAEDrawPayment method
        /// <summary>TBD</summary>
        private void LoadAEDrawPayment()
        {
            DataSet drawPaymentData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                drawPaymentData = io.ExecuteDataSetQuery(IO.CreateCommandFromStoredProc("MASTERDATA_GETDRAWPAYMENTRECORD", Param.CreateParam("DRAWPAYMENTID", SqlDbType.Int, this.AEDrawPaymentId)));
            }
            if (drawPaymentData.Tables.Count == 0)
            {
                return;
            }
            if (drawPaymentData.Tables[0].Rows.Count == 0)
            {
                return;
            }
            DataRow drawPaymentRecord = drawPaymentData.Tables[0].Rows[0];
            this.AEId = Convert.ToString(IO.GetDataRowValue(drawPaymentRecord, "ACCOUNT_EXECUTIVE_ID", ""));
            this.DrawPaymentType = (string)drawPaymentRecord["DRAW_PAYMENT_TYPE"];
            this.DrawPaymentAmount = Convert.ToDouble(drawPaymentRecord["DRAW_PAYMENT_AMOUNT"]);
            this.DrawPaymentYear = (int)drawPaymentRecord["DRAW_PAYMENT_YEAR"];
            this.PaymentMonth = Convert.ToInt32(IO.GetDataRowValue(drawPaymentRecord, "PAYMENT_MONTH", -1));
            this.PaymentDate = Convert.ToDateTime(IO.GetDataRowValue(drawPaymentRecord, "PAYMENT_DATE", DateTime.MinValue));
            this.PaymentMonth = Convert.ToInt32(IO.GetDataRowValue(drawPaymentRecord, "PAYMENT_MONTH", -1));
            this.EnteredBy = Convert.ToString(IO.GetDataRowValue(drawPaymentRecord, "ENTERED_BY", ""));
            this.CompanyId = (int)drawPaymentRecord["COMPANY_ID"];
            this.PaymentDate = Convert.ToDateTime(IO.GetDataRowValue(drawPaymentRecord, "DATE_LAST_MODIFIED", DateTime.MinValue));
        }
        #endregion

        #region PaymentDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime PaymentDate { get; set; }
        #endregion

        #region PaymentMonth property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int PaymentMonth { get; set; }
        #endregion

        #region PaymentMonthDisplay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string PaymentMonthDisplay
        {
            get
            {
                if (String.Compare(this.DrawPaymentType, "D", true) == 0)
                {
                    return "";
                }
                DateTime month = new DateTime(DateTime.Now.Year, this.PaymentMonth, 1);
                return string.Format("{0:MMMM}", month);
            }
        }
        #endregion

        #region PaymentStatus property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string PaymentStatus { get; set; }
        #endregion

        #region PaymentStatusDisplay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string PaymentStatusDisplay
        {
            get
            {
                if (String.Compare(this.PaymentStatus, "P", true) == 0)
                {
                    return "Paid";
                }
                if (String.Compare(this.PaymentStatus, "N", true) == 0)
                {
                    return "Not Paid";
                }
                return "";
            }
        }
        #endregion

        #region Save method
        /// <summary>TBD</summary>
        public void Save()
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("DRAWPAYMENTTYPE", SqlDbType.Char, this.DrawPaymentType));
            spParams.Add(Param.CreateParam("DRAWPAYMENTAMOUNT", SqlDbType.Decimal, this.DrawPaymentAmount));
            spParams.Add(Param.CreateParam("DRAWPAYMENTYEAR", SqlDbType.Int, this.DrawPaymentYear));
            if (String.Compare(this.DrawPaymentType, "P", true) == 0)
            {
                spParams.Add(Param.CreateParam("PAYMENTMONTH", SqlDbType.Int, this.PaymentMonth));
                spParams.Add(Param.CreateParam("PAYMENTDATE", SqlDbType.Date, this.PaymentDate));
                spParams.Add(Param.CreateParam("PAYMENTSTATUS", SqlDbType.Char, this.PaymentStatus));
            }
            else
            {
                spParams.Add(Param.CreateParam("PAYMENTMONTH", SqlDbType.Int, DBNull.Value));
                spParams.Add(Param.CreateParam("PAYMENTDATE", SqlDbType.Date, DBNull.Value));
                spParams.Add(Param.CreateParam("PAYMENTSTATUS", SqlDbType.Char, DBNull.Value));
            }
            spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, (this.EnteredBy == null ? Security.GetCurrentUserId : this.EnteredBy)));
            string storedProcName = "";
            if (this.AEDrawPaymentId == -1)
            {
                spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, this.AEId));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, this.CompanyId));
                storedProcName = "MASTERDATA_ADDDRAWPAYMENTRECORD";
            }
            else
            {
                spParams.Add(Param.CreateParam("DRAWPAYMENTID", SqlDbType.Int, this.AEDrawPaymentId));
                storedProcName="MASTERDATA_UPDATEDRAWPAYMENTRECORD";
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc(storedProcName, spParams));
            }
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class AEFlatRate
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public AEFlatRate()
        {
        }
        #endregion

        #region Parameterized constructor (aeFlatRateId)
        /// <summary>TBD</summary>
        /// <param name="aeFlatRateId">TBD</param>
        public AEFlatRate(int aeFlatRateId)
        {
            this.FlatRateId = aeFlatRateId;
            LoadAEFlatRate();
        }
        #endregion

        #region Parameterized constructor (flatRateId, aeId, flatRateNew, flatRateRenew, flatRateEffectiveDate, companyId)
        /// <summary>TBD</summary>
        /// <param name="flatRateId">TBD</param>
        /// <param name="aeId">TBD</param>
        /// <param name="flatRateNew">TBD</param>
        /// <param name="flatRateRenew">TBD</param>
        /// <param name="flatRateEffectiveDate">TBD</param>
        /// <param name="companyId">TBD</param>
        public AEFlatRate(int flatRateId, string aeId, double flatRateNew, double flatRateRenew, DateTime flatRateEffectiveDate, int companyId)
        {
            this.FlatRateId = flatRateId;
            this.AEId = aeId;
            this.FlatRateNew = flatRateNew;
            this.FlatRateRenew = flatRateRenew;
            this.FlatRateEffectiveDate = flatRateEffectiveDate;
            this.CompanyId = companyId;
            this.DateLastModified = DateTime.Now;
        }
        #endregion

        #region Parameterized constructor (aeId, flatRateNew, flatRateRenew, flatRateEffectiveDate, companyId)
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="flatRateNew">TBD</param>
        /// <param name="flatRateRenew">TBD</param>
        /// <param name="flatRateEffectiveDate">TBD</param>
        /// <param name="companyId">TBD</param>
        public AEFlatRate(string aeId, double flatRateNew, double flatRateRenew, DateTime flatRateEffectiveDate, int companyId)
        {
            this.FlatRateId = -1;
            this.AEId = aeId;
            this.FlatRateNew = flatRateNew;
            this.FlatRateRenew = flatRateRenew;
            this.FlatRateEffectiveDate = flatRateEffectiveDate;
            this.CompanyId = companyId;
            this.DateLastModified = DateTime.Now;
        }
        #endregion

        #region AEFlatRateAmount property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AEFlatRateAmount { get; set; }
        #endregion

        #region AEId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AEId { get; set; }
        #endregion

        #region CheckExistingFlatRateEffectiveDate method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="flatRateEffectiveDate">TBD</param>
        /// <returns>TBD</returns>
        public static bool CheckExistingFlatRateEffectiveDate(string aeId, int companyId, DateTime flatRateEffectiveDate)
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, aeId));
            spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, companyId));
            spParams.Add(Param.CreateParam("FLATRATEEFFECTIVEDATE", SqlDbType.Date, flatRateEffectiveDate));
            int count = 0;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                count = Convert.ToInt32(io.ExecuteScalarQuery(IO.CreateCommandFromStoredProc("MASTERDATA_CHECKEXISTINGFLATRATEEFFECTIVEDATE", spParams)));
            }
            return (count > 0);
        }
        #endregion

        #region CompanyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CompanyId { get; set; }
        #endregion

        #region DateLastModified property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime DateLastModified { get; set; }
        #endregion

        #region DeleteRecord method
        /// <summary>TBD</summary>
        /// <param name="aeFlatRateId">TBD</param>
        public static void DeleteRecord(int aeFlatRateId)
        {
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc("MASTERDATA_DELETEFLATRATERECORD",
                    Param.CreateParam("AEFLATRATEID", SqlDbType.Int, aeFlatRateId),
                    Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId)));
            }
        }
        #endregion

        #region FlatRateEffectiveDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime FlatRateEffectiveDate { get; set; }
        #endregion

        #region FlatRateEffectiveDateDisplay property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string FlatRateEffectiveDateDisplay
        {
            get
            {
                return this.FlatRateEffectiveDate.ToShortDateString();
            }
        }
        #endregion

        #region FlatRateId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int FlatRateId { get; set; }
        #endregion

        #region FlatRateNew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double FlatRateNew { get; set; }
        #endregion

        #region FlatRateRenew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double FlatRateRenew { get; set; }
        #endregion

        #region GetAEFlatRates method
        /// <summary>TBD</summary>
        /// <param name="aeId">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public static List<AEFlatRate> GetAEFlatRates(string aeId, int companyId)
        {
            try
            {
                List<AEFlatRate> aeFlatRates = new List<AEFlatRate>();
                DataSet historyData;
                using (IO io = new IO(WebCommon.ConnectionString))
                {
                    historyData = io.ExecuteDataSetQuery(IO.CreateCommandFromSql("SELECT * FROM ACCOUNT_EXECUTIVE_FLAT_RATE WHERE LTRIM(RTRIM(ACCOUNT_EXECUTIVE_ID)) = LTRIM(RTRIM('{0}')) AND COMPANY_ID={1} ORDER BY FLAT_RATE_EFFECTIVE_DATE DESC", aeId, companyId));
                }
                if (historyData.Tables[0].Rows.Count == 0)
                {
                    return aeFlatRates;
                }
                AEFlatRate aeFlatRate;
                foreach (DataRow row in historyData.Tables[0].Rows)
                {
                    aeFlatRate = new AEFlatRate();
                    aeFlatRate.FlatRateId = (int)row["FLAT_RATE_ID"];
                    aeFlatRate.AEId = Convert.ToString(IO.GetDataRowValue(row, "ACCOUNT_EXECUTIVE_ID", ""));
                    aeFlatRate.AEFlatRateAmount = Convert.ToDouble(IO.GetDataRowValue(row, "ACCOUNT_EXECUTIVE_FLAT_RATE", 0.00));
                    aeFlatRate.FlatRateEffectiveDate = Convert.ToDateTime(IO.GetDataRowValue(row, "FLAT_RATE_EFFECTIVE_DATE", DateTime.MinValue));
                    aeFlatRate.DateLastModified = Convert.ToDateTime(IO.GetDataRowValue(row, "DATE_LAST_MODIFIED", DateTime.MinValue));
                    aeFlatRate.FlatRateNew = Convert.ToDouble(IO.GetDataRowValue(row, "FLAT_RATE_NEW", 0.00));
                    aeFlatRate.FlatRateRenew = Convert.ToDouble(IO.GetDataRowValue(row, "FLAT_RATE_RENEW", 0.00));
                    aeFlatRate.CompanyId = (int)row["COMPANY_ID"];
                    aeFlatRates.Add(aeFlatRate);
                }
                return aeFlatRates;
            }
            catch (Exception ex)
            {
                WebCommon.LogExceptionInfo(ex);
                return new List<AEFlatRate>();
            }
        }
        #endregion

        #region LoadAEFlatRate method
        /// <summary>TBD</summary>
        public void LoadAEFlatRate()
        {
            DataSet flatRateData;
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                flatRateData = io.ExecuteDataSetQuery(IO.CreateCommandFromSql("SELECT * FROM ACCOUNT_EXECUTIVE_FLAT_RATE WHERE FLAT_RATE_ID={0}", this.FlatRateId));
            }
            if (flatRateData.Tables.Count == 0)
            {
                return;
            }
            if (flatRateData.Tables[0].Rows.Count == 0)
            {
                return;
            }
            DataRow flatRateRecord = flatRateData.Tables[0].Rows[0];
            this.AEId = Convert.ToString(IO.GetDataRowValue(flatRateRecord, "ACCOUNT_EXECUTIVE_ID", ""));
            this.AEFlatRateAmount = Convert.ToDouble(IO.GetDataRowValue(flatRateRecord, "ACCOUNT_EXECUTIVE_FLAT_RATE", 0.00));
            this.FlatRateEffectiveDate = Convert.ToDateTime(IO.GetDataRowValue(flatRateRecord, "FLAT_RATE_EFFECTIVE_DATE", DateTime.MinValue));
            this.DateLastModified = Convert.ToDateTime(IO.GetDataRowValue(flatRateRecord, "DATE_LAST_MODIFIED", DateTime.MinValue));
            this.FlatRateNew = Convert.ToDouble(IO.GetDataRowValue(flatRateRecord, "FLAT_RATE_NEW", 0.00));
            this.FlatRateRenew = Convert.ToDouble(IO.GetDataRowValue(flatRateRecord, "FLAT_RATE_RENEW", 0.00));
            this.CompanyId = (int)flatRateRecord["COMPANY_ID"];
        }
        #endregion

        #region Save method
        /// <summary>TBD</summary>
        public void Save()
        {
            List<System.Data.SqlClient.SqlParameter> spParams = new List<System.Data.SqlClient.SqlParameter>();
            spParams.Add(Param.CreateParam("FLATRATENEW", SqlDbType.Decimal, this.FlatRateNew));
            spParams.Add(Param.CreateParam("FLATRATERENEW", SqlDbType.Decimal, this.FlatRateRenew));
            spParams.Add(Param.CreateParam("FLATRATEEFFECTIVEDATE", SqlDbType.Date, this.FlatRateEffectiveDate));
            spParams.Add(Param.CreateParam("ENTEREDBY", SqlDbType.VarChar, Security.GetCurrentUserId));
            string storedProcName = "";
            if (this.FlatRateId == -1)
            {
                spParams.Add(Param.CreateParam("AEID", SqlDbType.VarChar, this.AEId));
                spParams.Add(Param.CreateParam("COMPANYID", SqlDbType.Int, this.CompanyId));
                storedProcName = "MASTERDATA_ADDAEFLATRATE";
            }
            else
            {
                spParams.Add(Param.CreateParam("FLATRATEID", SqlDbType.Int, this.FlatRateId));
                storedProcName = "MASTERDATA_UPDATEAEFLATRATE";
            }
            using (IO io = new IO(WebCommon.ConnectionString))
            {
                io.ExecuteActionQuery(IO.CreateCommandFromStoredProc(storedProcName, spParams));
            }
        }
        #endregion

    }

    /// <summary>TBD</summary>
    public class ContractCommissionSplit
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public ContractCommissionSplit()
        {
        }
        #endregion

        #region Parameterized constructor (commissionId, contractNumber, effectiveDate, packageId, ae1Id, ae1RevenuePercentage, ae1CommissionPercentage, ae1PointsNew, ae1PointsRenew, ae2Id, ae2RevenuePercentage, ae2CommissionPercentage, ae2PointsNew, ae2PointsRenew, ae3Id, ae3RevenuePercentage, ae3CommissionPercentage, ae3PointsNew, ae3PointsRenew, ae4Id, ae4RevenuePercentage, ae4CommissionPercentage, ae4PointsNew, ae4PointsRenew, usePoints, approvedById, approvedByDate, entryDate, enteredBy, companyId)
        /// <summary>TBD</summary>
        /// <param name="commissionId">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="effectiveDate">TBD</param>
        /// <param name="packageId">TBD</param>
        /// <param name="ae1Id">TBD</param>
        /// <param name="ae1RevenuePercentage">TBD</param>
        /// <param name="ae1CommissionPercentage">TBD</param>
        /// <param name="ae1PointsNew">TBD</param>
        /// <param name="ae1PointsRenew">TBD</param>
        /// <param name="ae2Id">TBD</param>
        /// <param name="ae2RevenuePercentage">TBD</param>
        /// <param name="ae2CommissionPercentage">TBD</param>
        /// <param name="ae2PointsNew">TBD</param>
        /// <param name="ae2PointsRenew">TBD</param>
        /// <param name="ae3Id">TBD</param>
        /// <param name="ae3RevenuePercentage">TBD</param>
        /// <param name="ae3CommissionPercentage">TBD</param>
        /// <param name="ae3PointsNew">TBD</param>
        /// <param name="ae3PointsRenew">TBD</param>
        /// <param name="ae4Id">TBD</param>
        /// <param name="ae4RevenuePercentage">TBD</param>
        /// <param name="ae4CommissionPercentage">TBD</param>
        /// <param name="ae4PointsNew">TBD</param>
        /// <param name="ae4PointsRenew">TBD</param>
        /// <param name="usePoints">TBD</param>
        /// <param name="approvedById">TBD</param>
        /// <param name="approvedByDate">TBD</param>
        /// <param name="entryDate">TBD</param>
        /// <param name="enteredBy">TBD</param>
        /// <param name="companyId">TBD</param>
        public ContractCommissionSplit(int commissionId, string contractNumber, DateTime effectiveDate, int packageId,
        string ae1Id, double ae1RevenuePercentage, double ae1CommissionPercentage, double ae1PointsNew, double ae1PointsRenew,
        string ae2Id, double ae2RevenuePercentage, double ae2CommissionPercentage, double ae2PointsNew, double ae2PointsRenew,
        string ae3Id, double ae3RevenuePercentage, double ae3CommissionPercentage, double ae3PointsNew, double ae3PointsRenew,
        string ae4Id, double ae4RevenuePercentage, double ae4CommissionPercentage, double ae4PointsNew, double ae4PointsRenew,
        bool usePoints, string approvedById, DateTime approvedByDate, DateTime entryDate, string enteredBy, int companyId)
        {
            this.CommissionId = commissionId;
            this.ContractNumber = contractNumber;
            this.EffectiveDate = effectiveDate;
            this.PackageId = packageId;
            this.AE1Id = ae1Id;
            this.AE1RevenuePercentage = ae1RevenuePercentage;
            this.AE1CommissionPercentage = ae1CommissionPercentage;
            this.AE1PointsNew = ae1PointsNew;
            this.AE1PointsRenew = ae1PointsRenew;
            this.AE2Id = ae2Id;
            this.AE2RevenuePercentage = ae2RevenuePercentage;
            this.AE2CommissionPercentage = ae2CommissionPercentage;
            this.AE2PointsNew = ae2PointsNew;
            this.AE2PointsRenew = ae2PointsRenew;
            this.AE3Id = ae3Id;
            this.AE3RevenuePercentage = ae3RevenuePercentage;
            this.AE3CommissionPercentage = ae3CommissionPercentage;
            this.AE3PointsNew = ae3PointsNew;
            this.AE3PointsRenew = ae3PointsRenew;
            this.AE4Id = ae4Id;
            this.AE4RevenuePercentage = ae4RevenuePercentage;
            this.AE4CommissionPercentage = ae4CommissionPercentage;
            this.AE4PointsNew = ae4PointsNew;
            this.AE4PointsRenew = ae4PointsRenew;
            this.UsePoints = usePoints;
            this.ApprovedById = approvedById;
            this.ApprovedByDate = approvedByDate;
            this.EntryDate = entryDate;
            this.EnteredBy = enteredBy;
            this.CompanyId = companyId;
        }
        #endregion

        #region AE1CommissionPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE1CommissionPercentage { get; set; }
        #endregion

        #region AE1Id property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AE1Id { get; set; }
        #endregion

        #region AE1PointsNew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE1PointsNew { get; set; }
        #endregion

        #region AE1PointsRenew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE1PointsRenew { get; set; }
        #endregion

        #region AE1RevenuePercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE1RevenuePercentage { get; set; }
        #endregion

        #region AE2CommissionPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE2CommissionPercentage { get; set; }
        #endregion

        #region AE2Id property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AE2Id { get; set; }
        #endregion

        #region AE2PointsNew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE2PointsNew { get; set; }
        #endregion

        #region AE2PointsRenew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE2PointsRenew { get; set; }
        #endregion

        #region AE2RevenuePercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE2RevenuePercentage { get; set; }
        #endregion

        #region AE3CommissionPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE3CommissionPercentage { get; set; }
        #endregion

        #region AE3Id property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AE3Id { get; set; }
        #endregion

        #region AE3PointsNew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE3PointsNew { get; set; }
        #endregion

        #region AE3PointsRenew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE3PointsRenew { get; set; }
        #endregion

        #region AE3RevenuePercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE3RevenuePercentage { get; set; }
        #endregion

        #region AE4CommissionPercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE4CommissionPercentage { get; set; }
        #endregion

        #region AE4Id property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string AE4Id { get; set; }
        #endregion

        #region AE4PointsNew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE4PointsNew { get; set; }
        #endregion

        #region AE4PointsRenew property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE4PointsRenew { get; set; }
        #endregion

        #region AE4RevenuePercentage property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public double AE4RevenuePercentage { get; set; }
        #endregion

        #region ApprovedByDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime ApprovedByDate { get; set; }
        #endregion

        #region ApprovedById property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ApprovedById { get; set; }
        #endregion

        #region CommissionId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CommissionId { get; set; }
        #endregion

        #region CompanyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CompanyId { get; set; }
        #endregion

        #region ContractNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ContractNumber { get; set; }
        #endregion

        #region Delete method
        /// <summary>TBD</summary>
        public void Delete()
        {
        }
        #endregion

        #region EffectiveDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime EffectiveDate { get; set; }
        #endregion

        #region EnteredBy property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string EnteredBy { get; set; }
        #endregion

        #region EntryDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime EntryDate { get; set; }
        #endregion

        #region GetSplitHistory method
        /// <summary>TBD</summary>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <returns>TBD</returns>
        public static List<ContractCommissionSplit> GetSplitHistory(string contractNumber, int companyId)
        {
            List<ContractCommissionSplit> commissionSplitHistory = new List<ContractCommissionSplit>();
            return commissionSplitHistory;
        }
        #endregion

        #region PackageId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int PackageId { get; set; }
        #endregion

        #region Save method
        /// <summary>TBD</summary>
        public void Save()
        {
        }
        #endregion

        #region Update method
        /// <summary>TBD</summary>
        public void Update()
        {
        }
        #endregion

        #region UsePoints property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool UsePoints { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class ContractNote
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public ContractNote()
        {
        }
        #endregion

        #region Parameterized constructor (noteId, screenName, contractNumber, noteText, noteDate, companyId, userId)
        /// <summary>TBD</summary>
        /// <param name="noteId">TBD</param>
        /// <param name="screenName">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="noteText">TBD</param>
        /// <param name="noteDate">TBD</param>
        /// <param name="companyId">TBD</param>
        /// <param name="userId">TBD</param>
        public ContractNote(int noteId, string screenName, int contractNumber, string noteText, DateTime noteDate, int companyId, string userId)
        {
            this.NoteId = noteId;
            this.ScreenName = screenName;
            this.ContractNumber = contractNumber;
            this.NoteText = noteText;
            this.NoteDate = noteDate;
            this.CompanyId = companyId;
            this.UserId = userId;
        }
        #endregion

        #region CompanyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int CompanyId { get; set; }
        #endregion

        #region ContractNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int ContractNumber { get; set; }
        #endregion

        #region NoteDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime NoteDate { get; set; }
        #endregion

        #region NoteId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int NoteId { get; set; }
        #endregion

        #region NoteText property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string NoteText { get; set; }
        #endregion

        #region ScreenName property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string ScreenName { get; set; }
        #endregion

        #region UserId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string UserId { get; set; }
        #endregion

    }

}
