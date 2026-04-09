namespace MESData.Model
{
    public partial class torderinspection : BaseModel
    {
        [Key]
        public int? INSPECTION_IDX { get; set; }
public int? ORDER_IDX { get; set; }
public string? COLSIP { get; set; }
public double? CCH1 { get; set; }
public double? CCW1 { get; set; }
public double? CCH2 { get; set; }
public double? CCW2 { get; set; }
public double? ICH1 { get; set; }
public double? ICW1 { get; set; }
public double? ICH2 { get; set; }
public double? ICW2 { get; set; }
public double? WIRE_FORCE { get; set; }
public int? WIRE_LENGTH { get; set; }
public string? IN_RESILT { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public torderinspection()
        {
        }
    }
}

