namespace MESData.Model
{
    public partial class torderinspection_sw : BaseModel
    {
        [Key]
        public int? INSPECTION_IDX { get; set; }
public int? ORDER_IDX { get; set; }
public string? LOC_LRJ { get; set; }
public string? COLSIP { get; set; }
public double? CCH { get; set; }
public double? CCW { get; set; }
public double? ICH { get; set; }
public double? ICW { get; set; }
public double? WIRE_FORCE { get; set; }
public string? IN_RESILT { get; set; }

        public torderinspection_sw()
        {
        }
    }
}

