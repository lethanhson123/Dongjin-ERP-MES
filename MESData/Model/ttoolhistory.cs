namespace MESData.Model
{
    public partial class ttoolhistory : BaseModel
    {
        [Key]
        public int? TOOL_HIS_IDX { get; set; }
public DateTime? WORK_DTM { get; set; }
public int? TOOL_IDX { get; set; }
public int? WK_QTY { get; set; }
public int? TOT_QTY { get; set; }
public string? CONTENT { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttoolhistory()
        {
        }
    }
}

