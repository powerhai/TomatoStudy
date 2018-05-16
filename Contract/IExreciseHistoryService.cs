using System.Collections.Generic;
using Models;
namespace Contract
{
    public interface IExreciseHistoryService
    {
        void SaveExrecise(string user, string book,string bookTitle, string module, string unit);
        List<ExreciseData> GetExrecises(string user);
        List<ExreciseData> GetTodayExrecises(string user);
        List<ExreciseData> GetTodayExrecises(string user, string book);
        List<ExreciseData> GetDaysExrecises(string user);
    }
}