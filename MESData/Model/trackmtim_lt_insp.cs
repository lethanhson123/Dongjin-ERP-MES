namespace MESData.Model
{
    public partial class trackmtim_lt_insp : BaseModel
    {
        [Key]
        public int? LT_INSP_IDX { get; set; }
public int? TRACKMTIN_IDX { get; set; }
public DateTime? INSP_DATE { get; set; }
public int? INSP_QTY { get; set; }
public int? OK_QTY { get; set; }
public int? NG_QTY { get; set; }
public string? INSP_RESULT { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public trackmtim_lt_insp()
        {
        }
    }
}

