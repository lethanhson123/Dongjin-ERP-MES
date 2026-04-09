namespace MESData.Model
{
    public partial class torder_bom_sw : BaseModel
    {
        [Key]
        public int? TORDER_BOM_IDX { get; set; }
public int? ORDER_IDX { get; set; }
public string? LOC_LRJ { get; set; }
public double? PERFORMN { get; set; }
public string? DSCN_YN { get; set; }
public int? T1_TOOL_IDX { get; set; }
public string? ERROR_CHK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torder_bom_sw()
        {
        }
    }
}

