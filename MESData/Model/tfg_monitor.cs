namespace MESData.Model
{
    public partial class tfg_monitor : BaseModel
    {
        [Key]
        public int? TFG_MO_IDX { get; set; }
public DateTime? TFG_DATE { get; set; }
public int? TFG_CD_IDX { get; set; }
public int? TFG_QTY { get; set; }
public int? TFG_QTYBOX { get; set; }
public int? TFG_TOTL_QTY { get; set; }
public int? TFG_TOTL_QTYBOX { get; set; }
public string? TFG_NTC { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tfg_monitor()
        {
        }
    }
}

