namespace MESData.Model
{
    public partial class ttoolmaster2 : BaseModel
    {
        [Key]
        public int? TOOLMASTER_IDX { get; set; }
public int? TOOL_IDX { get; set; }
public string? SEQ { get; set; }
public int? TOT_WK_CNT { get; set; }
public int? WK_CNT { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttoolmaster2()
        {
        }
    }
}

