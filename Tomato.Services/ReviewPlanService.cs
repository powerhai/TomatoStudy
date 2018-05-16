using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Contract;
using Models;
using WordRemember.Domain;
namespace Tomato.Services
{
    public class ReviewPlanService : IReviewPlanService
    {
        
        private const string ATT_USER = "user";
        private const string ATT_BOOK = "book";
        private const string ATT_START_DATE = "startDate";
        private const string ATT_DAYS = "days"; 
        private const string X_ITEM = "item";
        private const string XML_CONTENT = "<?xml version=\"1.0\"?><root></root>";

        public List<ReviewPlanData> GetReviewPlanDatas(string user)
        {
            var list = new List<ReviewPlanData>();
            if (!File.Exists(SystemFileNames.REVIEW_PLAN_FILE_PATH)) {
                File.AppendAllText(SystemFileNames.REVIEW_PLAN_FILE_PATH, XML_CONTENT);
            }
            var doc = XDocument.Load(SystemFileNames.REVIEW_PLAN_FILE_PATH);
            foreach(var node in doc.Root.Elements().Where(a => a.Attribute(ATT_USER).Value.Equals(user)))
            {
                var d = new ReviewPlanData();
                d.Book = node.Attribute(ATT_BOOK).Value;
                d.User = user;
                d.Days = Convert.ToInt32(node.Attribute(ATT_DAYS).Value);
                if(!string.IsNullOrEmpty(node.Attribute(ATT_START_DATE).Value))
                {
                    d.StartDate = Convert.ToDateTime(node.Attribute(ATT_START_DATE).Value);
                }
                list.Add(d);
            }
            return list;
        }
        public void SaveReviewPlanData(List<ReviewPlanData> data )
        {
            var list = new List<ExreciseData>();
            if (!File.Exists(SystemFileNames.REVIEW_PLAN_FILE_PATH)) {
                File.AppendAllText(SystemFileNames.REVIEW_PLAN_FILE_PATH, XML_CONTENT);
            }
            var doc = XDocument.Load(SystemFileNames.REVIEW_PLAN_FILE_PATH);
            foreach(var d in data)
            {
                var node = doc.Root.Elements().FirstOrDefault(a => a.Attribute(ATT_USER).Value.Equals(d.User) && a.Attribute(ATT_BOOK).Value.Equals(d.Book));
                var startDate = d.StartDate == null ? "" : d.StartDate.Value.ToString();
                if (node == null)
                {
                   
                    node = new XElement(X_ITEM, new XAttribute(ATT_USER, d.User),
                        new XAttribute(ATT_BOOK, d.Book),
                        new XAttribute(ATT_START_DATE, startDate),
                        new XAttribute(ATT_DAYS, d.Days) 
                        );
                    doc.Root.Add(node);
                }
                else
                {
                    node.Attribute(ATT_DAYS).Value = d.Days.ToString();
                    node.Attribute(ATT_START_DATE).Value = startDate;
                }
            }
            doc.Save(SystemFileNames.REVIEW_PLAN_FILE_PATH);
        }
    }
}