using System.Collections.Generic;
using Models;
namespace Contract
{
    public interface IReviewPlanService
    {
        List<ReviewPlanData> GetReviewPlanDatas(string user);
        void SaveReviewPlanData(List<ReviewPlanData> data);
    }
}