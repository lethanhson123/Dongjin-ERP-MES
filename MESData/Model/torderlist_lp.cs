namespace MESData.Model
{
    public partial class torderlist_lp : BaseModel
    {
        [Key]
        public int? ORDER_IDX { get; set; }
public string? MC { get; set; }
public double? TOT_QTY { get; set; }
public double? PERFORMN_L { get; set; }
public double? PERFORMN_R { get; set; }
public string? CONDITION { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }
public string? LEAD_NO { get; set; }
public string? PROJECT { get; set; }
public double? ADJ_AF_QTY { get; set; }
public string? CUR_LEADS { get; set; }
public string? CT_LEADS { get; set; }
public string? CT_LEADS_PR { get; set; }
public string? GRP { get; set; }
public string? PRT { get; set; }
public DateTime? DT { get; set; }
public string? MC2 { get; set; }
public double? BUNDLE_SIZE { get; set; }
public string? HOOK_RACK { get; set; }
public string? WIRE { get; set; }
public string? T1_DIR { get; set; }
public string? TERM1 { get; set; }
public string? STRIP1 { get; set; }
public string? SEAL1 { get; set; }
public string? CCH_W1 { get; set; }
public string? ICH_W1 { get; set; }
public string? T2_DIR { get; set; }
public string? TERM2 { get; set; }
public string? STRIP2 { get; set; }
public string? SEAL2 { get; set; }
public string? CCH_W2 { get; set; }
public string? ICH_W2 { get; set; }
public string? SP_ST { get; set; }
public string? REP { get; set; }
public string? DSCN_YN { get; set; }

        public torderlist_lp()
        {
        }
    }
}

