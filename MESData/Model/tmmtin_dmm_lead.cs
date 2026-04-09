namespace MESData.Model
{
    public partial class tmmtin_dmm_lead : BaseModel
    {
        [Key]
        public int? TMMTIN_DMM_IDX { get; set; }
public int? TMMTIN_DMM_STGC { get; set; }
public DateTime? TMMTIN_DATE { get; set; }
public int? TMMTIN_PART { get; set; }
public int? TMMTIN_PART_SNP { get; set; }
public string? TMMTIN_QTY { get; set; }
public string? TMMTIN_REC_YN { get; set; }
public string? TMMTIN_CNF_YN { get; set; }
public string? TMMTIN_DSCN_YN { get; set; }
public int? TMMTIN_SHEETNO { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tmmtin_dmm_lead()
        {
        }
    }
}

