using System;
namespace Models
{
    public class PracticeData {
        public string User {
            get;
            set;
        }
        public string Book {
            get;
            set;
        }
        public string Modules {
            get;
            set;
        }
        public DateTime DateTime {
            get;
            set;
        }

        public float WordsCountMinute {
            get;
            set;
        }
        public int WordsCount {
            get;
            set;
        }
        public int ErrorWordsCount {
            get;
            set;
        }
        public int Seconds {
            get;
            set;
        }
    }
}