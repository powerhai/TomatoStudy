using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ExreciseData
    {
        public string Book { get; set; }
        public string BookTitle { get; set; }
        public string Module { get; set; }
        public string Unit { get; set; }
        public int ReviewCount { get; set; }
        public int Level { get; set; }
        public DateTime ReviewDate { get; set; }
        public string User { get; set; }
        public string Guid { get; set; }
    }
}
