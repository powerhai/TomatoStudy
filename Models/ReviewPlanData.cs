using System;
namespace Models
{
    public class ReviewPlanData
    {
        public string User { get; set; }
        public string Book { get; set; }
        public int Days { get; set; }
        public DateTime? StartDate { get; set; }
    }
}