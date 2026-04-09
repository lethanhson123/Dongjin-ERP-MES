namespace MESData.Model
{
    public partial class ttoolmaster : BaseModel
    {
        [Key]
        public int? TOOL_IDX { get; set; }
public string? APPLICATOR { get; set; }
public string? SEQ_TOT { get; set; }
public int? TOO_SUPPLY { get; set; }
public int? MAX_CNT { get; set; }
public int? TOT_WK_CNT { get; set; }
public int? WK_CNT { get; set; }
public string? SPP_NO { get; set; }
public string? TYPE { get; set; }
public string? GAUGE { get; set; }
public string? COPL_NOR { get; set; }
public string? COPL_SPE { get; set; }
public string? INSPL_SEALTYPE { get; set; }
public string? INSPL_NONSEAL { get; set; }
public string? INSPL_XTYPE { get; set; }
public string? INSPL_KTYPE { get; set; }
public string? INSPL_SPE { get; set; }
public string? ANVIL_NOR { get; set; }
public string? ANVIL_SPE { get; set; }
public string? CMU_NOR { get; set; }
public string? CMU_SPE { get; set; }
public string? IMU_NOR { get; set; }
public string? IMU_NONSEAL { get; set; }
public string? IMU_SPE { get; set; }
public string? CUTPL_ONE { get; set; }
public string? CUTPL_DET { get; set; }
public string? CUTAN_ONE { get; set; }
public string? CUTAN_DET { get; set; }
public string? CUTHO_ONE { get; set; }
public string? CUTHO_DET { get; set; }
public string? RRBLK_ONE { get; set; }
public string? RRBLK_DET { get; set; }
public string? RRCUTHO_ONE { get; set; }
public string? RRCUTHO_DET { get; set; }
public string? FRCUTHO_ONE { get; set; }
public string? FRCUTHO_DET { get; set; }
public string? RRCUTAN_ONE { get; set; }
public string? RRCUTAN_DET { get; set; }
public string? WRDN_ONE { get; set; }
public string? WRDN_DET { get; set; }
public string? DESC { get; set; }
public string? COMB_CODE { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttoolmaster()
        {
        }
    }
}

