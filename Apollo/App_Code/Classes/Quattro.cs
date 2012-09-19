#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Apollo.Quattro
{

    /// <summary>TBD</summary>
    public class Attachment
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public Attachment()
        {
        }
        #endregion

        #region Parameterized constructor (objectId, extension, name, title, description, type, typeId, attachmentDate, contractNumber, companyId)
        /// <summary>TBD</summary>
        /// <param name="objectId">TBD</param>
        /// <param name="extension">TBD</param>
        /// <param name="name">TBD</param>
        /// <param name="title">TBD</param>
        /// <param name="description">TBD</param>
        /// <param name="type">TBD</param>
        /// <param name="typeId">TBD</param>
        /// <param name="attachmentDate">TBD</param>
        /// <param name="contractNumber">TBD</param>
        /// <param name="companyId">TBD</param>
        public Attachment(string objectId, string extension, string name, string title, string description, string type, int typeId, DateTime attachmentDate, string contractNumber, int companyId)
        {
            this.objectId = objectId;
            this.extension = extension;
            this.name = name;
            this.title = title;
            this.description = description;
            this.type = type;
            this.typeId = typeId;
            this.attachmentDate = attachmentDate;
            this.contractNumber = contractNumber;
            this.companyId = companyId;
        }
        #endregion

        #region attachmentDate property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public DateTime attachmentDate { get; set; }
        #endregion

        #region companyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int companyId { get; set; }
        #endregion

        #region contractNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string contractNumber { get; set; }
        #endregion

        #region description property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string description { get; set; }
        #endregion

        #region extension property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string extension { get; set; }
        #endregion

        #region name property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string name { get; set; }
        #endregion

        #region objectId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string objectId { get; set; }
        #endregion

        #region title property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string title { get; set; }
        #endregion

        #region type property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string type { get; set; }
        #endregion

        #region typeId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int typeId { get; set; }
        #endregion

    }

    /// <summary>TBD</summary>
    public class AttachmentSearchParams
    {

        #region Default constructor
        /// <summary>TBD</summary>
        public AttachmentSearchParams()
        {
        }
        #endregion

        #region Parameterized constructor (contractNumber, invoiceNumber, advertiser, uidObjectType, companyId)
        /// <summary>TBD</summary>
        /// <param name="contractNumber">TBD</param>
        /// <param name="invoiceNumber">TBD</param>
        /// <param name="advertiser">TBD</param>
        /// <param name="uidObjectType">TBD</param>
        /// <param name="companyId">TBD</param>
        public AttachmentSearchParams(List<String> contractNumber, List<String> invoiceNumber, string advertiser, int uidObjectType, int companyId)
        {
            this.contractNumber = contractNumber;
            this.invoiceNumber = invoiceNumber;
            this.advertiser = advertiser;
            this.uidObjectType = uidObjectType;
            this.companyId = companyId;
        }
        #endregion

        #region advertiser property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public string advertiser { get; set; }
        #endregion

        #region companyId property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int companyId { get; set; }
        #endregion

        #region contractNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<String> contractNumber { get; set; }
        #endregion

        #region invoiceNumber property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<String> invoiceNumber { get; set; }
        #endregion

        #region uidObjectType property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public int uidObjectType { get; set; }
        #endregion

    }

}
