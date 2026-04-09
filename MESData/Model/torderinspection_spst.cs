namespace MESData.Model
{
    public partial class torderinspection_spst : BaseModel
    {
        [Key]
        public int? INSPECTION_IDX { get; set; }
public int? ORDER_IDX { get; set; }
public string? COLSIP { get; set; }
public double? RES_H { get; set; }
public double? RES_V { get; set; }
public string? IN_RESILT { get; set; }

        public torderinspection_spst()
        {
        }
    }
}

