namespace MESData.Model
{
    public partial class apqp_code : BaseModel
    {
        [Key]
        public int? CD_IDX { get; set; }
public int? CDGR_IDX { get; set; }
public string? CD_SYS_NOTE { get; set; }
public string? CD_NM_GRP { get; set; }
public string? CD_NM { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public apqp_code()
        {
        }
    }
}

