namespace MESData.Model
{
    public partial class tdpdotpl_tmp : BaseModel
    {
        [Key]
        public int? PDOTPL_IDX { get; set; }
public string? PO_CODE { get; set; }
public string? PART_NO { get; set; }
public int? PO_QTY { get; set; }
public int? NT_QTY { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public tdpdotpl_tmp()
        {
        }
    }
}

