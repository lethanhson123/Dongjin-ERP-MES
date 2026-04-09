using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESData.Model
{
    public class InStockItem
    {
        public string? TABLE_NM { get; set; }
        public string? HOOK_RACK_LOC { get; set; }
        public string? LEAD_NO { get; set; }
        public int? Stock_QTY { get; set; }
        public string? BARCODE { get; set; }
        public DateTime? IN_DATE { get; set; }
        public string? QTY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string? CREATE_USER { get; set; }
        public int? TRACK_IDX { get; set; }
        public int? TABLE_IDX { get; set; }
    }
}
