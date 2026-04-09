namespace MESData.Model
{
    public partial class tscdgr : BaseModel
    {
        [Key]
        public int? CDGR_IDX { get; set; }
public string? CDGR_SYSNOTE { get; set; }
public string? CDGR_NM_HAN { get; set; }
public string? CDGR_NM_EN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tscdgr()
        {
        }
    }
}

