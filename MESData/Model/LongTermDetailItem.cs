using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESData.Model
{
    public class LongTermDetailItem
    {
        public string? LEAD_NO { get; set; }
        public string? TABLE_NM { get; set; }
        public string? LOCATION { get; set; }
        public string? BARCODE { get; set; }
        public decimal? QTY { get; set; }
        public DateTime? LAST_INSPECT { get; set; } 
        public int? YEAR { get; set; } 
        public int? MONTH { get; set; } 
        public int? MONTHS_AGO { get; set; } 
    }
}
