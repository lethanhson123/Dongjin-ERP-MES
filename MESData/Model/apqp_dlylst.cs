namespace MESData.Model
{
    public partial class apqp_dlylst : BaseModel
    {
        [Key]
        public int? APQP_DLYLST_IDX { get; set; }
public int? APQP_MSTLST_IDX { get; set; }
public int? APQP_DLYLST_RNK { get; set; }
public DateTime? APQP_DLYLST_NXTDT { get; set; }
public string? APQP_DLYLST_NM { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public apqp_dlylst()
        {
        }
    }
}

