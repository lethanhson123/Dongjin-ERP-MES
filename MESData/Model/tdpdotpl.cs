namespace MESData.Model
{
    public partial class tdpdotpl : BaseModel
    {
        [Key]
        public int? PDOTPL_IDX { get; set; }
public string? PO_CODE { get; set; }
public int? PART_IDX { get; set; }
public int? PART_IDX_SNP { get; set; }
public int? PO_QTY { get; set; }
public int? NT_QTY { get; set; }
public int? PACK_QTY { get; set; }
public string? DONE_YN { get; set; }
public int? SORTNO { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdpdotpl()
        {
        }
    }
}

