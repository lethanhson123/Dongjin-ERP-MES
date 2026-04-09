namespace MESData.Model
{
    public partial class kr_tdpdotpl_inpo : BaseModel
    {
        [Key]
        public int? PDOTPL_INPO_IDX { get; set; }
public string? PO_CODE { get; set; }
public string? PART_NO { get; set; }
public int? PART_SNP { get; set; }
public int? PO_QTY { get; set; }
public int? IN_QTY { get; set; }
public string? DONE_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public kr_tdpdotpl_inpo()
        {
        }
    }
}

