namespace MESData.Model
{
    public partial class ttc_rackmtin : BaseModel
    {
        [Key]
        public int? TRACK_IDX { get; set; }
public string? RACKCODE { get; set; }
public int? TTC_PART_IDX { get; set; }
public string? BARCODE_NM { get; set; }
public DateTime? RACKDTM { get; set; }
public string? IN_USER { get; set; }
public DateTime? RACKOUT_DTM { get; set; }
public string? OUT_USER { get; set; }
public int? QTY { get; set; }
public string? RACKIN_YN { get; set; }
public string? RACKOUT_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttc_rackmtin()
        {
        }
    }
}

