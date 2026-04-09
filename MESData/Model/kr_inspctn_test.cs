namespace MESData.Model
{
    public partial class kr_inspctn_test : BaseModel
    {
        [Key]
        public int? KR_INSPCTN_TEST_IDX { get; set; }
public string? KR_INSPCTN_CODE { get; set; }
public int? KR_INSPCTN_PARTIDX { get; set; }
public int? KR_INSPCTN_IDX { get; set; }
public DateTime? KR_INSPCTN_DATE { get; set; }
public string? KR_INSPCTN_TEXT { get; set; }
public string? KR_INSPCTN_N { get; set; }
public double? KR_INSPCTN_X1 { get; set; }
public double? KR_INSPCTN_X2 { get; set; }
public double? KR_INSPCTN_X3 { get; set; }
public double? KR_INSPCTN_X4 { get; set; }
public double? KR_INSPCTN_X5 { get; set; }
public string? KR_INSPCTN_RUS { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public kr_inspctn_test()
        {
        }
    }
}

