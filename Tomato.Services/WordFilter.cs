using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Xps.Packaging;
using Aspose.Words;
using log4net;
using Models;
using NetOffice.WordApi.Enums;
using WordRemember.Domain;
using Style = NetOffice.WordApi.Style;
namespace Tomato.Services
{
    internal class WordFilter
    {
        #region Fields
        private readonly string mFilepath;
        private readonly ILog mLog;
        #endregion
        #region Constructors
        public WordFilter(string filepath, ILog log)
        {
            mFilepath = filepath;
            mLog = log;
        }
        #endregion
        #region Public Methods
        public XpsDocumentData GetFilterDocument(string module, string unit)
        { 
            try
            { 
                var file = new FileInfo(mFilepath);
                if(!file.Exists)
                    return null;

                FileStream docStream = new System.IO.FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Aspose.Words.Document doc = new Aspose.Words.Document(docStream);
                docStream.Close();
               var curModule = "";
                var curUnit = "";
                var curMusic = "";
                
                 
                var nodes = doc.GetChildNodes(NodeType.Paragraph, true);
                foreach (var p in nodes) {
                    var par = p as Aspose.Words.Paragraph;
                    var styleName = par.ParagraphFormat.Style.Name;
                     Debug.WriteLine( $"[{styleName}] = {par.Range.Text}");
                    if((styleName != null) &&
                       (styleName.Equals(WordStyleNames.STYLE_NAME_MODULE) ||
                        styleName.Equals(WordStyleNames.STYLE_NAME_MODULE2)))
                    {
                        curModule = par.Range.Text.Trim();
                        if(!curModule.Equals(module))
                        {
                            Debug.WriteLine($"Delete --- [{styleName}] = {par.Range.Text}");
                            par.ParentNode.RemoveChild(par); 
                            par.Range.Delete();
                        } 
                    }
                    else if((styleName != null) &&
                            (styleName.Equals(WordStyleNames.STYLE_NAME_UNIT) ||
                             styleName.Equals(WordStyleNames.STYLE_NAME_UNIT2)))
                    {
                        curUnit = par.Range.Text?.Trim();
                        if(!curModule.Equals(module))
                        {
                            Debug.WriteLine($"Delete --- [{styleName}] = {par.Range.Text}");
                             
                            par.Range.Delete();
                        }
                        else
                        {
                            if(!curUnit.Equals(unit))
                            {
                                Debug.WriteLine($"Delete --- [{styleName}] = {par.Range.Text}");
                                 
                                par.Range.Delete();
                            }
                               
                        }
                    }
                    else
                    {
                        if(!curUnit.Equals(unit) || !curModule.Equals(module))
                        { 
                            if (par.ParentNode != null && par.ParentNode is Aspose.Words.Tables.Cell) {
                                var cell = par.ParentNode as Aspose.Words.Tables.Cell;
                                //if (cell.ParentRow.ParentTable.ParentNode != null)
                                //    cell.ParentRow?.ParentTable?.Remove();
                            
                            }
                            Debug.WriteLine($"Delete --- [{styleName}] = {par.Range.Text}");
                            
                            par.Range.Delete(); 
                        }
                        else
                        {
                            var txt = par.Range?.Text?.Trim();
                            if(!string.IsNullOrEmpty(txt))
                            {
                                const string music = "Music=";
                                if( txt.Length> music.Length && txt.Substring(0, music.Length).Equals(music))
                                {
                                    curMusic = txt.Substring(music.Length);
                                }
                            }
                            
                            //builder.InsertNode(par.Clone(false));
                        }
                    }
                }

                var tables = doc.GetChildNodes(NodeType.Table, true);
                foreach(var t in tables)
                {
                    var tb = t as Aspose.Words.Tables.Table;
                    var txt = tb.GetText().Replace('\a', ' ').Trim(); 
                    if(string.IsNullOrEmpty(txt))
                        tb.Remove();
                }
                var xpsFile = Path.GetTempFileName();
                doc.Save(xpsFile, SaveFormat.Xps); 
                var xpsDoc = new XpsDocument(xpsFile, FileAccess.Read);
                return new XpsDocumentData()
                {
                    Document = xpsDoc,
                    MusicFilePath = curMusic
                };
            }
            catch(Exception e)
            {
                mLog.Error($"Exception: {nameof(WordFilter)}.{nameof(GetFilterDocument)} - {e}");
            }
 
            return null;
        }
        #endregion
    }
}