namespace MESData.Model
{
    public partial class tsbom_ver02_po : BaseModel
    {
        [Key]
        public int? BOM_IDX { get; set; }
public DateTime? PO_DATE { get; set; }
public int? PAREN_PART_IDX { get; set; }
public int? PAREN_EO_IDX { get; set; }
public double? PO_QTY { get; set; }
public string? DSYN { get; set; }
public string? PO_GRUP { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsbom_ver02_po()
        {
        }
    }
}

