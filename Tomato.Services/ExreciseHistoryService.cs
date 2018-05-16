using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Contract;
using log4net;
using Models;
using WordRemember.Domain;
namespace Tomato.Services
{
    public class ExreciseHistoryService : IExreciseHistoryService
    {
        private readonly ILog mLog;
        public ExreciseHistoryService(ILog log)
        {
            mLog = log;
            mLog.Info("ExreciseHistoryService Started.");
        }

        private const string ATT_GUID = "guid";
        private const string ATT_USER = "user";
        private const string ATT_BOOK = "book";
        private const string ATT_BOOK_TITLE = "bookTitle";
        private const string ATT_MODULE = "module";
        private const string ATT_UNIT = "unit";
        private const string ATT_REVIEW_DATE = "reviewDate";
        private const string ATT_REVIEW_COUNT = "reviewCount";
        private const string ATT_LEVEL = "level";
        private const string X_ITEM = "item";
        private const string XML_CONTENT = "<?xml version=\"1.0\"?><root></root>";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="book">word 文件名</param>
        /// <param name="module">章</param>
        /// <param name="unit">节</param>
        public void SaveExrecise(string user, string book, string bookTitle, string module, string unit)
        {
            try
            {

                if(!File.Exists(SystemFileNames.EXRECISE_FILE_PATH))
                {
                    File.AppendAllText(SystemFileNames.EXRECISE_FILE_PATH, XML_CONTENT);
                }
                var doc = XDocument.Load(SystemFileNames.EXRECISE_FILE_PATH);
                var node =
                    doc.Root.Elements()
                        .FirstOrDefault(
                            a =>
                                a.Attribute(ATT_BOOK).Value.Equals(book) &&
                                a.Attribute(ATT_MODULE).Value.Equals(module) && a.Attribute(ATT_UNIT).Value.Equals(unit) &&
                                a.Attribute(ATT_USER).Value.Equals(user));
                if(node == null)
                {
                    node = new XElement(X_ITEM, new XAttribute(ATT_USER, user), new XAttribute(ATT_BOOK, book),
                        new XAttribute(ATT_BOOK_TITLE, bookTitle), new XAttribute(ATT_MODULE, module),
                        new XAttribute(ATT_UNIT, unit), new XAttribute(ATT_REVIEW_COUNT, 1),
                        new XAttribute(ATT_REVIEW_DATE, DateTime.Now), new XAttribute(ATT_LEVEL, "1"));
                    doc.Root.Add(node);
                }
                else
                {
                    var reviewCount = Convert.ToInt32(node.Attribute(ATT_REVIEW_COUNT).Value);
                    node.Attribute(ATT_REVIEW_DATE).Value = DateTime.Now.ToString();
                    node.Attribute(ATT_REVIEW_COUNT).Value = (reviewCount + 1).ToString();
                }
                doc.Save(SystemFileNames.EXRECISE_FILE_PATH);
            }
            catch(Exception e)
            {
                mLog.Error($"Exception: {nameof(ExreciseHistoryService)}.{nameof(SaveExrecise)} - {e}");

            }
            SaveExreciseDays(user, book, bookTitle, module, unit);
        }
        public void SaveExreciseDays(string user, string book, string bookTitle, string module, string unit)
        {
            try {
                 
                if (!File.Exists(SystemFileNames.EXRECISE_DAYS_FILE_PATH)) {
                    File.AppendAllText(SystemFileNames.EXRECISE_DAYS_FILE_PATH, XML_CONTENT);
                }
                var doc = XDocument.Load(SystemFileNames.EXRECISE_DAYS_FILE_PATH);
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                var todayNode = doc.Root.Elements().FirstOrDefault(a=>a.Attribute(ATT_REVIEW_DATE ).Value.Equals(date));
                if(todayNode == null)
                {
                    todayNode = new XElement("Date", new XAttribute(ATT_REVIEW_DATE,date ));
                    doc.Root.Add(todayNode);
                }

                var node =
                    todayNode.Elements()
                        .FirstOrDefault(
                            a =>
                                a.Attribute(ATT_BOOK).Value.Equals(book) &&
                                a.Attribute(ATT_MODULE).Value.Equals(module) && a.Attribute(ATT_UNIT).Value.Equals(unit) &&
                                a.Attribute(ATT_USER).Value.Equals(user));
                if (node == null) {
                    node = new XElement(X_ITEM, new XAttribute(ATT_USER, user), new XAttribute(ATT_BOOK, book),
                        new XAttribute(ATT_BOOK_TITLE, bookTitle), new XAttribute(ATT_MODULE, module),
                        new XAttribute(ATT_UNIT, unit), new XAttribute(ATT_REVIEW_COUNT, 1),
                        new XAttribute(ATT_REVIEW_DATE, DateTime.Now), new XAttribute(ATT_LEVEL, "1"),
                        new XAttribute(ATT_GUID, Guid.NewGuid())
                        );
                    todayNode.Add(node);
                }
                else {
                    var reviewCount = Convert.ToInt32(node.Attribute(ATT_REVIEW_COUNT).Value);
                    node.Attribute(ATT_REVIEW_DATE).Value = DateTime.Now.ToString();
                    node.Attribute(ATT_REVIEW_COUNT).Value = (reviewCount + 1).ToString();
                }
                doc.Save(SystemFileNames.EXRECISE_DAYS_FILE_PATH);
            }
            catch (Exception e) {
                mLog.Error($"Exception: {nameof(ExreciseHistoryService)}.{nameof(SaveExrecise)} - {e}");

            }
        }

        public List<ExreciseData> GetExrecises(string user)
        {
            try
            {
                var list = new List<ExreciseData>();
                if (!File.Exists(SystemFileNames.EXRECISE_FILE_PATH)) {
                    File.AppendAllText(SystemFileNames.EXRECISE_FILE_PATH, XML_CONTENT);
                }
                var doc = XDocument.Load(SystemFileNames.EXRECISE_FILE_PATH);
                foreach (var item in doc.Root.Elements()) {
                    var xuser = item.Attribute(ATT_USER).Value;
                    if (!user.Equals(xuser))
                        continue;
                    var data = new ExreciseData() {
                        Book = item.Attribute(ATT_BOOK).Value,
                        Module = item.Attribute(ATT_MODULE).Value,
                        Unit = item.Attribute(ATT_UNIT).Value,
                        ReviewCount = Convert.ToInt32(item.Attribute(ATT_REVIEW_COUNT).Value),
                        ReviewDate = Convert.ToDateTime(item.Attribute(ATT_REVIEW_DATE).Value),
                        Level = Convert.ToInt32(item.Attribute(ATT_LEVEL).Value),
                        BookTitle = item.Attribute(ATT_BOOK_TITLE).Value,
                        User = item.Attribute(ATT_USER).Value
                    };
                    list.Add(data);
                }
                return list;
            }
            catch(Exception e)
            {
                mLog.Error($"Exception: {nameof(ExreciseHistoryService)}.{nameof(GetExrecises)} - {e}");
            }
            return new List<ExreciseData>();
        }

        public List<ExreciseData> GetDaysExrecises(string user)
        {
            var list = new List<ExreciseData>();
            if (!File.Exists(SystemFileNames.EXRECISE_DAYS_FILE_PATH)) {
                File.AppendAllText(SystemFileNames.EXRECISE_DAYS_FILE_PATH, XML_CONTENT);
            }
            var doc = XDocument.Load(SystemFileNames.EXRECISE_DAYS_FILE_PATH);
            foreach(var dateNode in doc.Root.Elements())
            { 

                foreach(var item in dateNode.Elements())
                {
                    var userName = item.Attribute(ATT_USER).Value;
                    if(user.Equals(userName))
                    {
                        var data = new ExreciseData() {
                            Book = item.Attribute(ATT_BOOK).Value,
                            Module = item.Attribute(ATT_MODULE).Value,
                            Unit = item.Attribute(ATT_UNIT).Value,
                            ReviewCount = Convert.ToInt32(item.Attribute(ATT_REVIEW_COUNT).Value),
                            ReviewDate = Convert.ToDateTime(item.Attribute(ATT_REVIEW_DATE).Value),
                            Level = Convert.ToInt32(item.Attribute(ATT_LEVEL).Value),
                            BookTitle = item.Attribute(ATT_BOOK_TITLE).Value,
                            User = item.Attribute(ATT_USER).Value,
                            Guid = item.Attribute(ATT_GUID).Value
                        };
                   
                        list.Add(data);                        
                    }

                }
            }
            return list;
        }
        public List<ExreciseData> GetTodayExrecises(string user)
        {
            try {
                var list = new List<ExreciseData>();
                if (!File.Exists(SystemFileNames.EXRECISE_DAYS_FILE_PATH)) {
                    File.AppendAllText(SystemFileNames.EXRECISE_DAYS_FILE_PATH, XML_CONTENT);
                }
                var doc = XDocument.Load(SystemFileNames.EXRECISE_DAYS_FILE_PATH);

                var date = DateTime.Now.ToString("yyyy-MM-dd");
                var todayNode = doc.Root.Elements().FirstOrDefault(a => a.Attribute(ATT_REVIEW_DATE).Value.Equals(date));
                if (todayNode == null) {
                    todayNode = new XElement("Date", new XAttribute(ATT_REVIEW_DATE, date));
                    doc.Root.Add(todayNode);
                }

                foreach (var item in todayNode.Elements()) {
                    var xuser = item.Attribute(ATT_USER).Value;
                    if (!user.Equals(xuser))
                        continue;
                    var data = new ExreciseData() {
                        Book = item.Attribute(ATT_BOOK).Value,
                        Module = item.Attribute(ATT_MODULE).Value,
                        Unit = item.Attribute(ATT_UNIT).Value,
                        ReviewCount = Convert.ToInt32(item.Attribute(ATT_REVIEW_COUNT).Value),
                        ReviewDate = Convert.ToDateTime(item.Attribute(ATT_REVIEW_DATE).Value),
                        Level = Convert.ToInt32(item.Attribute(ATT_LEVEL).Value),
                        BookTitle = item.Attribute(ATT_BOOK_TITLE).Value,
                        User = item.Attribute(ATT_USER).Value,
                        Guid = item.Attribute(ATT_GUID).Value
                    };
                    list.Add(data);
                }
                return list;
            }
            catch (Exception e) {
                mLog.Error($"Exception: {nameof(ExreciseHistoryService)}.{nameof(GetExrecises)} - {e}");
            }
            return new List<ExreciseData>();
        }
        public List<ExreciseData> GetTodayExrecises(string user, string book)
        {
            var historyItems = GetTodayExrecises(user);
            var list = new List<ExreciseData>();
            foreach(var  point in  historyItems)
            {
                if(point.Book.Equals(book))
                    list.Add(point);
            }
            return list;
        }
    }
}
