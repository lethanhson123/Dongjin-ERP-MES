namespace MESData.Model
{
    public partial class ttc_barcode : BaseModel
    {
        [Key]
        public int? TTC_BARCODE_IDX { get; set; }
public string? TTC_BARCODENM { get; set; }
public int? TTC_ORDER_IDX { get; set; }
public int? Barcode_SEQ { get; set; }
public string? TTC_BC_WORK { get; set; }
public string? DSCN_YN { get; set; }
public DateTime? WORK_START { get; set; }
public DateTime? WORK_END { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttc_barcode()
        {
        }
    }
}

