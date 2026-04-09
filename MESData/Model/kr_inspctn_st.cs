namespace MESData.Model
{
    public partial class kr_inspctn_st : BaseModel
    {
        [Key]
        public int? KR_INSPCTN_IDX { get; set; }
public int? KR_INSPCTN_PARTIDX { get; set; }
public string? KR_INSPCTN_EO { get; set; }
public int? KR_INSPCTN_VER { get; set; }
public DateTime? KR_INSPCTN_DATE { get; set; }
public int? KR_INSPCTN_NO { get; set; }
public string? KR_INSPCTN_TEXT { get; set; }
public double? KR_INSPCTN_ST { get; set; }
public double? KR_INSPCTN_MIN { get; set; }
public double? KR_INSPCTN_MAX { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public kr_inspctn_st()
        {
        }
    }
}

