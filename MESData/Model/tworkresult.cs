namespace MESData.Model
{
    public partial class tworkresult : BaseModel
    {
        [Key]
        public int? WORK_IDX { get; set; }
public string? GROUP_CODE { get; set; }
public DateTime? WORK_DTM { get; set; }
public int? USER_IDX { get; set; }
public string? W_GRC { get; set; }
public int? W_PEP { get; set; }
public string? LEAD_NM { get; set; }
public int? PART_IDX { get; set; }
public int? GOOD_QTY { get; set; }
public int? BAD_QTY { get; set; }
public int? ETC_QTY { get; set; }
public int? TOT_QTY { get; set; }
public int? ORDER_IDX { get; set; }
public string? DSCN_YN { get; set; }
public DateTime? WK_ST { get; set; }
public DateTime? WK_ET { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }
public string? PART_NM { get; set; }
public string? ROUTER { get; set; }
public string? WORK_RMK { get; set; }
public string? MC_NM { get; set; }
public string? Q_CON { get; set; }

        public tworkresult()
        {
        }
    }
}

