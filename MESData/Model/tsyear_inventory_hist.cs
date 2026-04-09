namespace MESData.Model
{
    public partial class tsyear_inventory_hist : BaseModel
    {
        [Key]
        public int? TSYEAR_INV_IDX { get; set; }
public string? TSYEAR_INV_YEAR { get; set; }
public int? TSYEAR_INV_SERIALNO { get; set; }
public string? TSYEAR_INV_DEPART { get; set; }
public string? TSYEAR_INV_PKILOC { get; set; }
public string? TSYEAR_INV_PART_TNM { get; set; }
public int? TSYEAR_INV_PART_IDX { get; set; }
public int? TSYEAR_INV_QTY { get; set; }
public string? TSYEAR_INV_DSNY { get; set; }
public string? TSYEAR_INV_MESNO { get; set; }
public string? TSYEAR_INV_ANM { get; set; }
public string? TSYEAR_INV_DJGLOC { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsyear_inventory_hist()
        {
        }
    }
}

