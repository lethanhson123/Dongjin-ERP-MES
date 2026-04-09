namespace MESData.Model
{
    public partial class tmmtin : BaseModel
    {
        [Key]
        public int? MTIN_IDX { get; set; }
public int? PART_IDX { get; set; }
public string? UTM { get; set; }
public string? DESC { get; set; }
public double? QTY { get; set; }
public decimal? NET_WT { get; set; }
public decimal? GRS_WT { get; set; }
public string? PLET_NO { get; set; }
public string? SHPD_NO { get; set; }
public DateTime? MTIN_DTM { get; set; }
public string? BRCD_PRNT { get; set; }
public string? DSCN_YN { get; set; }
public string? MTIN_RMK { get; set; }
public int? SNP_QTY { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tmmtin()
        {
        }
    }
}

