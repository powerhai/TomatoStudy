using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
namespace Contract
{
    public interface IAudioService
    {
        Task PlaySound (string word);
    }
    public interface IWordManager
    {
        List<UIBook> GetBooks ();
        void SaveBooks (List<UIBook> books);
    }
}
