using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESData.Model
{
    public class LongTermSummaryItem
    {
        public string? YEAR { get; set; } 
        public string? MONTH { get; set; } 
        public long? LEAD_COUNT { get; set; } 
        public decimal? ADD_1 { get; set; } 
        public decimal? ADD_2 { get; set; }
        public decimal? ADD_3 { get; set; }
        public decimal? ADD_4 { get; set; }
        public decimal? ADD_5 { get; set; }
        public decimal? ADD_6 { get; set; }
        public decimal? OVER_9 { get; set; } 
        public decimal? OVER_10 { get; set; } 
        public decimal? SUM_QTY { get; set; }  
    }
}
