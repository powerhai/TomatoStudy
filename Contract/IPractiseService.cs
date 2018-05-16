using System.Collections.Generic;
using Models;
namespace Contract
{
    public interface IPractiseService
    {
        List<WordPractiseData> GetWordPractiseDatas (string userName);
        List<PracticeData> GetPractices (string userName);
        void SavePractise (string user,string book, string[] modules, List<WordPractise>  practises, int seconds, float wordsCountMinute);
    }
}