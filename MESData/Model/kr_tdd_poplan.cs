namespace MESData.Model
{
    public partial class kr_tdd_poplan : BaseModel
    {
        [Key]
        public int? TDD_PP_IDX { get; set; }
public string? TDD_PO_TYPE { get; set; }
public string? TDD_POCODE { get; set; }
public int? TDD_POCSTM { get; set; }
public int? TDD_PP_PNIDX { get; set; }
public DateTime? TDD_PP_DT { get; set; }
public int? TDD_PP_QTY { get; set; }
public int? TDD_PP_PACK { get; set; }
public string? TDD_DSCN_YN { get; set; }
public string? TDD_REMARK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public kr_tdd_poplan()
        {
        }
    }
}

