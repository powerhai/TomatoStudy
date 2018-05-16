using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Contract;
using Models;

namespace Services
{
    public class WordManager : IWordManager
    {
        private const string WORD_FILE_NAME = "Words.xml";
        public List<UIBook> GetBooks () {
            try {
                var xmlFormat = new XmlSerializer(typeof(List<UIBook>)); 
                var fileStream = new FileStream(WORD_FILE_NAME, FileMode.Open, FileAccess.Read);
                var data = (List<UIBook>)xmlFormat.Deserialize(fileStream);
                return data;
            }
            catch(Exception) {
                return null;
            }
           
        }
        public void SaveBooks (List<UIBook> books) {
             
            var fileStream = File.Open(WORD_FILE_NAME, FileMode.Create );
            using (fileStream) {
                var xmlFormat = new XmlSerializer(typeof(List<UIBook>));
                
                xmlFormat.Serialize(fileStream, books);
            }
        }
    }
}
