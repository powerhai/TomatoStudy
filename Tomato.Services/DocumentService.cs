using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Xps.Packaging;
using Contract;
using log4net;
using Models;
using NetOffice.WordApi.Enums;
using WordRemember.Domain;
namespace Tomato.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILog mLog; 
        public DocumentService(ILog log)
        {
            mLog = log;
            mLog.Info("DocumentService has been Started.");

        }
        public List<BookPoint> GetBookPoints(string bookName)
        {
            try
            {
                var reader = new WordFileReader($"{SystemFileNames.BOOK_PATH}\\{bookName}", mLog);
                return reader.GetBookPoints();
            }
            catch(Exception e)
            {
                mLog.Error($"Exception: {nameof(DocumentService)}.{nameof(GetBookPoints)} - {e}");
            }
            return new List<BookPoint>();
        }
        public XpsDocumentData GetDocument(string book, string module, string unit)
        {
            try
            {
                var filter = new WordFilter(book, mLog);
                return filter.GetFilterDocument(module, unit);
            }
            catch (Exception e) {
                mLog.Error($"Exception: {nameof(DocumentService)}.{nameof(GetDocument)} - {e}");
            }
            return null;
        }
    }
 
}