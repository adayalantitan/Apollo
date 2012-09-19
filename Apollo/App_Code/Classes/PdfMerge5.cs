#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
#endregion

namespace Apollo
{

    /// <summary>
    /// Summary description for PDFMerge
    ///     From: http://alex.buayacorp.com/merge-pdf-files-with-itext-and-net.html
    /// </summary>
    public class PdfMerge5
    {

        #region Member variables
        /// <summary>TBD</summary>
        private readonly List<PdfReader> documents;
        /// <summary>TBD</summary>
        private bool enablePagination = false;
        #endregion

        #region Default constructor
        /// <summary>TBD</summary>
        public PdfMerge5()
        {
            documents = new List<PdfReader>();
        }
        #endregion

        #region AddDocument method (pdfContents)
        /// <summary>TBD</summary>
        /// <param name="pdfContents">TBD</param>
        public void AddDocument(byte[] pdfContents)
        {
            documents.Add(new PdfReader(pdfContents));
        }
        #endregion

        #region AddDocument method (pdfDocument)
        /// <summary>TBD</summary>
        /// <param name="pdfDocument">TBD</param>
        public void AddDocument(PdfReader pdfDocument)
        {
            documents.Add(pdfDocument);
        }
        #endregion

        #region AddDocument method (fileName)
        /// <summary>TBD</summary>
        /// <param name="fileName">TBD</param>
        public void AddDocument(string fileName)
        {
            documents.Add(new PdfReader(fileName));
        }
        #endregion

        #region AddDocument method (pdfStream)
        /// <summary>TBD</summary>
        /// <para
        public void AddDocument(Stream pdfStream)
        {
            documents.Add(new PdfReader(pdfStream));
        }
        #endregion

        #region Documents property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public List<PdfReader> Documents
        {
            get { return documents; }
        }
        #endregion

        #region EnablePagination property
        /// <summary>TBD</summary>
        /// <value>TBD</value>
        public bool EnablePagination
        {
            get { return enablePagination; }
            set { enablePagination = value; }
        }
        #endregion

        #region Merge method (outputFileName)
        /// <summary>TBD</summary>
        /// <param name="outputFileName">TBD</param>
        public void Merge(string outputFileName)
        {
            Merge(new FileStream(outputFileName, FileMode.Create));
        }
        #endregion

        #region Merge method (outputStream)
        /// <summary>TBD</summary>
        /// <param name="outputStream">TBD</param>
        public void Merge(Stream outputStream)
        {
            if (outputStream == null || !outputStream.CanWrite)
            {
                throw new Exception("Output stream is null or cannot be written to.");
            }
            Document newDocument = null;
            try
            {
                newDocument = new Document();
                PdfWriter pdfWriter = PdfWriter.GetInstance(newDocument, outputStream);
                newDocument.Open();
                PdfContentByte pdfContentByte = pdfWriter.DirectContent;
                //if (EnablePagination)
                //{
                //    documents.ForEach(delegate(PdfReader doc) { totalPages += doc.NumberOfPages; });
                //}
                //int currentPage = 1;
                foreach (PdfReader pdfReader in documents)
                {
                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        newDocument.NewPage();
                        PdfImportedPage importedPage = pdfWriter.GetImportedPage(pdfReader, page);
                        pdfContentByte.AddTemplate(importedPage, 0, 0);
                    }
                }                
            }
            finally
            {
                outputStream.Flush();
                if (newDocument != null)
                {
                    newDocument.Close();
                }
                outputStream.Close();
            }
        }
        #endregion

    }

}
