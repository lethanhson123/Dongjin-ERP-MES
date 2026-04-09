namespace MESData.Model
{
    public partial class torder_barcode_lp : BaseModel
    {
        [Key]
        public int? TORDER_BARCODE_IDX { get; set; }
public string? TORDER_BARCODENM { get; set; }
public int? ORDER_IDX { get; set; }
public int? Barcode_SEQ { get; set; }
public string? TORDER_BC_PRNT { get; set; }
public string? TORDER_BC_WORK { get; set; }
public string? DSCN_YN { get; set; }
public DateTime? WORK_START { get; set; }
public DateTime? WORK_END { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torder_barcode_lp()
        {
        }
    }
}

