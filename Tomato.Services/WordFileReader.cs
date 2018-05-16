using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using Aspose.Words;
using log4net;
using Models;
using NetOffice.WordApi.Enums;
using WordRemember.Domain;
using Document = NetOffice.WordApi.Document;
using Style = NetOffice.WordApi.Style;
namespace Tomato.Services
{
    class WordFileReader
    {
        private readonly string mFilepath;
        private readonly ILog mLog;
        public WordFileReader(string filepath, ILog log)
        {
            mFilepath = filepath;
            mLog = log;
        }

        public string GetBookTitle()
        {  
            try
            {
                var file = new FileInfo($"{SystemFileNames.BOOK_PATH}\\{mFilepath}");
                if (!file.Exists)
                    return "";
                FileStream docStream = new System.IO.FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
                Aspose.Words.Document doc = new Aspose.Words.Document(docStream);
                docStream.Close();
                var node = doc.GetChild(NodeType.Paragraph, 0, true) as Aspose.Words.Paragraph;
                return node?.Range.Text.Trim(); 
            }
            catch(Exception e)
            {
                mLog.Error($"Exception: {nameof(WordFileReader)}.{nameof(GetBookTitle)} - {e}");
            }
            
            return "";
        }
        public List<BookPoint> GetBookPoints()
        { 
            var points = new List<BookPoint>();
            try
            { 
                var file = new FileInfo(mFilepath);
                if(!file.Exists)
                    return points;

                FileStream docStream = new System.IO.FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Aspose.Words.Document doc = new Aspose.Words.Document(docStream);
                docStream.Close();
                var curModule = "";
                var nodes = doc.GetChildNodes(NodeType.Paragraph, true);
                foreach(var p in nodes)
                {
                    var par = p as Aspose.Words.Paragraph;
                    var styleName = par.ParagraphFormat.Style.Name;
                    if (styleName != null && (styleName.Equals(WordStyleNames.STYLE_NAME_MODULE) || styleName.Equals(WordStyleNames.STYLE_NAME_MODULE2)))
                    {
                        curModule = par.Range.Text.Trim();
                    }
                    if(styleName != null && (styleName.Equals(WordStyleNames.STYLE_NAME_UNIT) || styleName.Equals(WordStyleNames.STYLE_NAME_UNIT2)))
                    {
                        var curUnit = par.Range.Text;
                        if(!string.IsNullOrEmpty(curModule))
                        {
                            var point = new BookPoint()
                            {
                                Book = mFilepath?.Trim(),
                                Module = curModule?.Trim(),
                                Unit = curUnit?.Trim()
                            };
                            points.Add(point);
                        }
                    }
                }

            }
            catch(Exception e)
            {
                mLog.Error($"Exception: {nameof(WordFileReader)}.{nameof(GetBookPoints)} - {e}");
            }
 

            return points;
        }
    }
}