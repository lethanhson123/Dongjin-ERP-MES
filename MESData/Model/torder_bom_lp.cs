namespace MESData.Model
{
    public partial class torder_bom_lp : BaseModel
    {
        [Key]
        public int? TORDER_BOM_IDX { get; set; }
public int? ORDER_IDX { get; set; }
public int? T1_TOOL_IDX { get; set; }
public int? T2_TOOL_IDX { get; set; }
public string? ERROR_CHK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torder_bom_lp()
        {
        }
    }
}

