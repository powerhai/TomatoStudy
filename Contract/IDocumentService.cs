using System.Collections.Generic;
using System.Windows.Xps.Packaging;
using Models;
namespace Contract
{
    public interface IDocumentService
    {
        List<BookPoint> GetBookPoints(string bookName);
        XpsDocumentData GetDocument(string book, string module, string unit);
    }
}