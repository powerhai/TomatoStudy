using System.Collections.Generic;
using Models;
namespace Contract
{
    public interface IBookService
    {
        List<Book> GetBooks();
    }
}