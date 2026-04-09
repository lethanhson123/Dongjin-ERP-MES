using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESService.Model
{
    public class ScrapListTranfer
    {
        public int? Order_IDX { get; set; }
        public string? PROJECT { get; set; }
        public string? LEAD_NO { get; set; }
        public string? MC { get; set; }
        public string? MaterialCode { get; set; }
        public DateTime? Plan_Date { get; set; }
        public double? Percentage { get; set; }
        public double? Total_Plan { get; set; }
        public double? Total_Actual { get; set; }
        public double? Total_Using { get; set; }
        public double? Total_Scrap { get; set; }
        public string? Unit { get; set; }
        public double? Total_BOM { get; set; }
        public double? TotalQty { get; set; }
        public string? MType { get; set; }
        public string? CONDITION { get; set; }
        public string? CREATE_USER { get; set; }
        public string? USER_NM { get; set; }
    }
}
