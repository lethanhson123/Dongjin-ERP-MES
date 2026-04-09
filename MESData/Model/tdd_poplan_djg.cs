namespace MESData.Model
{
    public partial class tdd_poplan_djg : BaseModel
    {
        [Key]
        public int? TDD_PPD_IDX { get; set; }
public string? TDD_PO_TYPE { get; set; }
public int? TDD_PPD_PNIDX { get; set; }
public DateTime? TDD_PPD_DT { get; set; }
public string? TDD_PPD_DVC { get; set; }
public int? TDD_PPD_QTY { get; set; }
public int? TDD_PPD_BEQTY { get; set; }
public string? TDD_DSCN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdd_poplan_djg()
        {
        }
    }
}

