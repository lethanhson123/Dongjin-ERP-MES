namespace MESData.Model
{
    public partial class torderlist_spst : BaseModel
    {
        [Key]
        public int? ORDER_IDX { get; set; }
public string? OR_NO { get; set; }
public int? WORK_WEEK { get; set; }
public DateTime? PO_DT { get; set; }
public string? LEAD_NO { get; set; }
public double? PO_QTY { get; set; }
public double? SAFTY_QTY { get; set; }
public string? MC { get; set; }
public double? BUNDLE_SIZE { get; set; }
public double? PERFORMN { get; set; }
public string? CONDITION { get; set; }
public int? LEAD_COUNT { get; set; }
public string? PO_YN { get; set; }
public string? DSCN_YN { get; set; }
public string? ERROR_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }
public string? FCTRY_NM { get; set; }
public string? REP { get; set; }
public string? TORDER_FG { get; set; }
public double? TOEXCEL_QTY { get; set; }

        public torderlist_spst()
        {
        }
    }
}

