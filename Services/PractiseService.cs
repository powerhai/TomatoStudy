using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Contract;
using Models;
namespace Services
{
    public class PractiseService : IPractiseService
    {
        private const string XML_FILE_PATH = "Practises.xml";
        public void SavePractise (string user, string book, string[] modules, List<WordPractise> practisess, int seconds , float wordsCountMinute) {
            try {
                if(!File.Exists(XML_FILE_PATH)) {
                    File.AppendAllText(XML_FILE_PATH, "<?xml version=\"1.0\"?><root></root>");
                }
                var doc = XDocument.Load(XML_FILE_PATH);
                var userNode = doc.Root.Elements().FirstOrDefault(a => a.Attribute("name").Value == user);
                if(userNode == null) {
                    userNode = new XElement("user", new XAttribute("name", user));
                    doc.Root.Add(userNode);
                }
                var wordsNode = userNode.Element("words");
                if(wordsNode == null) {
                    wordsNode = new XElement("words");
                    userNode.Add(wordsNode);
                }
                var practisesNode = userNode.Element("practises");
                if(practisesNode == null) {
                    practisesNode = new XElement("practises");
                    userNode.Add(practisesNode);
                }
                //write down practises
                var practiseNode = new XElement("practise", new XAttribute("date", DateTime.Now),
                    new XAttribute("book", book), new XAttribute("modules", string.Join(",", modules)),
                    new XAttribute("count", practisess.Count),
                    new XAttribute("error", practisess.Count(wp => wp.IsSucceed == false)),
                    new XAttribute("seconds", seconds),
                    new XAttribute("wordsCountMinute", wordsCountMinute)
                    );
                practisesNode.Add(practiseNode);
                //write down words
                foreach(var word in practisess) {
                    var wordNode = wordsNode.Elements().FirstOrDefault(a => a.Attribute("name").Value == word.Word);
                    if(wordNode == null) {
                        wordNode = new XElement("word", new XAttribute("name", word.Word),
                            new XAttribute("corrected", word.IsSucceed == true ? 1 : 0),
                            new XAttribute("failed", word.IsSucceed == false ? 1 : 0));
                        wordsNode.Add(wordNode);
                    } else {
                        var correctedNode = wordNode.Attribute("corrected");
                        var failedNode = wordNode.Attribute("failed");
                        if(word.IsSucceed) {
                            correctedNode.Value = (1 + Convert.ToInt32(correctedNode.Value)).ToString();
                        } else {
                            failedNode.Value = (1 + Convert.ToInt32(failedNode.Value)).ToString();
                        }
                    }
                }
                var file = File.OpenWrite(XML_FILE_PATH);
                doc.Save(file);
                file.Close();
            }
            catch(Exception e) {
                // ignored
            }
        }
        public List<WordPractiseData> GetWordPractiseDatas (string userName) {
            var list = new List<WordPractiseData>();
            if (!File.Exists(XML_FILE_PATH)) {
                return list;
            }
            try {
                var doc = XDocument.Load(XML_FILE_PATH);
                var userNode = doc.Root.Elements().FirstOrDefault(a => a.Attribute("name").Value == userName);
                var wordsNode = userNode.Element("words");
                foreach (var word in wordsNode.Elements()) {
                    var pr = new WordPractiseData() { 
                        Word =  word.Attribute("name").Value,
                        Corrected  = Convert.ToInt32(word.Attribute("corrected").Value),
                        Failed = Convert.ToInt32(word.Attribute("failed").Value),
                   };
                    if(pr.Failed>0)
                        list.Add(pr);
                }
            }
            catch (Exception e) {


            }

            return list;
        }
        public List<PracticeData> GetPractices (string userName) {
            var list = new List<PracticeData>();
            if(!File.Exists(XML_FILE_PATH)) {
                return list;
            }
            try {
                var doc = XDocument.Load(XML_FILE_PATH);
                var userNode = doc.Root.Elements().FirstOrDefault(a => a.Attribute("name").Value == userName);
                var practisesNode = userNode.Element("practises");
                foreach(var pra in practisesNode.Elements()) {
                    var pr = new PracticeData() {
                        Book = pra.Attribute("book").Value,
                        Modules = pra.Attribute("modules").Value,
                        DateTime = Convert.ToDateTime(pra.Attribute("date").Value),
                        WordsCount = Convert.ToInt32(pra.Attribute("count").Value),
                        ErrorWordsCount = Convert.ToInt32(pra.Attribute("error").Value),
                        Seconds = Convert.ToInt32(pra.Attribute("seconds").Value ),
                        User = userName,
                        WordsCountMinute = Convert.ToSingle(pra.Attribute("wordsCountMinute").Value )
                    };
                    list.Add(pr);
                }
        }
            catch(Exception e) {


            }

            return list;
        }
    }
}