namespace MESData.Model
{
    public partial class twtool : BaseModel
    {
        [Key]
        public int? TOOLWORK_IDX { get; set; }
public int? TOOL_IDX { get; set; }
public string? TOOL_WORK { get; set; }
public int? WK_QTY { get; set; }
public int? TOT_WK_QTY { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public twtool()
        {
        }
    }
}

