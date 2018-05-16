using System;
namespace Models
{
    public class BookPoint
    {
        public string Book { get; set; } 
        public string Module { get; set; }
        public string Unit { get; set; }
        public string BookTitle { get; set; } 
        public double MinuteForTimesup { get; set; }
    }
}