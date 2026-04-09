namespace MESData.Model
{
    public partial class tdpdmtim_tmp : BaseModel
    {
        [Key]
        public int? PDTMP_IDX { get; set; }
public string? VLID_GRP { get; set; }
public string? VLID_USE_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public tdpdmtim_tmp()
        {
        }
    }
}

