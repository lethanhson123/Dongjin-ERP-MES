namespace MESData.Model
{
    public partial class pd_inout_part : BaseModel
    {
        [Key]
        public int? PD_INPUT_PART_IDX { get; set; }
public int? ORDER_IDX { get; set; }
public int? PDPART_IDX { get; set; }
public string? DSC_INOUT { get; set; }
public int? PUR_QTY { get; set; }
public DateTime? PUR_TIME { get; set; }
public string? OUT_USER { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pd_inout_part()
        {
        }
    }
}

