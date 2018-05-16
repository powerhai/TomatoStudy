using System;
using System.Collections.Generic;
using System.IO;
using Contract;
using log4net;
using Models;
using WordRemember.Domain;
namespace Tomato.Services
{
    public class BookService : IBookService
    {
        private readonly ILog mLog; 
        public BookService(ILog log)
        {
            mLog = log;
            mLog.Info("BookService has been Started.");
        }
        public List<Book> GetBooks()
        {
            var books = new List<Book>();
            try
            { 
                var dir = new System.IO.DirectoryInfo(SystemFileNames.BOOK_PATH);
                if(!dir.Exists)
                    dir.Create(); 
                var files = dir.GetFiles(  "*.docx" );
                foreach(var item in files)
                {

                    if((item.Attributes & FileAttributes.Hidden) != 0)
                        continue; 
                    var reader = new WordFileReader(item.Name, mLog); 
                    var book = new Book()
                    {
                        Name = item.Name,
                        Title = reader.GetBookTitle() 
                    };
                    books.Add(book); 
                } 
            }
            catch (Exception e) {
                mLog.Error($"Exception: {nameof(BookService)}.{nameof(GetBooks)} - {e}");
            }
            return books;
        }
    }
}