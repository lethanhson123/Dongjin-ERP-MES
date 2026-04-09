namespace MESData.Model
{
    public partial class tsbom_po_list : BaseModel
    {
        [Key]
        public int? BOM_PO_LIST_IDX { get; set; }
public string? PART_NO { get; set; }
public int? PO_QTY { get; set; }
public string? DSYN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsbom_po_list()
        {
        }
    }
}

