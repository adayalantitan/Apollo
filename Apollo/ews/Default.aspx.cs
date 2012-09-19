using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Microsoft.Exchange.WebServices.Data;

namespace Apollo
{
    public partial class ews_Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                labelOutput.Text = GetOutput();
            }
        }

        public string GetOutput()
        {
            StringBuilder output = new StringBuilder();
            ExchangeService service = GetService();
            FindItemsResults<Item> contacts = service.FindItems(WellKnownFolderName.Contacts, new ItemView(int.MaxValue));
            if (contacts.TotalCount <= 0)
            {
                return "You do not have any contacts";
            }
            output.Append("<table border=1 cellspacing=1 cellpadding=1><thead><tr><th>Company</th><th>Display Name</th><th>Job Title</th><th>E-Mail</th><th>Business Phone</th><th>Mobile</th><th>Business Fax</th></tbody></tr><tbody>");
            foreach (Item contact in contacts)
            {
                output.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>"
                    //, (((Contact)contact).Companies.Count > 0 ? ((Contact)contact).Companies[0] : "")
                    , ((Contact)contact).CompanyName
                    , ((Contact)contact).DisplayName
                    , ((Contact)contact).JobTitle                    
                    , (((Contact)contact).EmailAddresses.Contains(EmailAddressKey.EmailAddress1) ? ((Contact)contact).EmailAddresses[EmailAddressKey.EmailAddress1].Address : "")                    
                    , (((Contact)contact).PhoneNumbers.Contains(PhoneNumberKey.BusinessPhone) ? ((Contact)contact).PhoneNumbers[PhoneNumberKey.BusinessPhone].ToString() : "")
                    , (((Contact)contact).PhoneNumbers.Contains(PhoneNumberKey.MobilePhone) ? ((Contact)contact).PhoneNumbers[PhoneNumberKey.MobilePhone].ToString() : "")
                    , (((Contact)contact).PhoneNumbers.Contains(PhoneNumberKey.BusinessFax) ? ((Contact)contact).PhoneNumbers[PhoneNumberKey.BusinessFax].ToString() : "")
                );
            }
            output.Append("</tbody></table>");
            return output.ToString();
        }

        #region GetService method
        /// <summary>TBD</summary>
        /// <returns>TBD</returns>
        public static ExchangeService GetService()
        {
            Microsoft.Exchange.WebServices.Data.ExchangeService service = new Microsoft.Exchange.WebServices.Data.ExchangeService();
            service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SID, ((System.Security.Principal.WindowsIdentity)HttpContext.Current.User.Identity).User.Value);
            //service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, "jenna.collins@titan360.com");
            //service.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            service.Credentials = new WebCredentials("crmExchangeMgr", "St3v13rayv", "titanoutdoor");
            service.AutodiscoverUrl(Security.GetUserEmailFromId(Security.GetCurrentUserId));
            return service;
        }
        #endregion
    }
}