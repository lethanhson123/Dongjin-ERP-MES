namespace MESData.Model
{
    public partial class tdd_ct_st : BaseModel
    {
        [Key]
        public int? TDD_CT_IDX { get; set; }
public int? TDD_CT_PNIDX { get; set; }
public DateTime? TDD_CT_DT { get; set; }
public double? TDD_CT_QTY { get; set; }
public double? TDD_CT_SUB { get; set; }
public double? TDD_CT_FA { get; set; }
public double? TDD_CT_RO { get; set; }
public double? TDD_MPP { get; set; }
public DateTime? TDD_CT_BEDT { get; set; }
public double? TDD_CT_BEQTY { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdd_ct_st()
        {
        }
    }
}

