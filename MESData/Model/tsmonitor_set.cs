namespace MESData.Model
{
    public partial class tsmonitor_set : BaseModel
    {
        [Key]
        public int? TSMONITOR_SET_IDX { get; set; }
public int? TSMONITOR_SET_NO { get; set; }
public string? TSMONITOR_SET_CODE { get; set; }
public string? VB_NAME { get; set; }
public string? VB_NAME_KR { get; set; }
public string? VB_NAME_ENG { get; set; }
public string? VB_NAME_VN { get; set; }
public string? VB_CHK_YN { get; set; }
public int? VB_CY_COUNT { get; set; }
public int? VB_TIME { get; set; }

        public tsmonitor_set()
        {
        }
    }
}

