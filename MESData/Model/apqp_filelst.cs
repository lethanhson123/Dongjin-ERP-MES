namespace MESData.Model
{
    public partial class apqp_filelst : BaseModel
    {
        [Key]
        public int? APQP_FLLST_IDX { get; set; }
public int? APQP_MSTLST_IDX { get; set; }
public int? APQP_FLLST_RNK { get; set; }
public DateTime? APQP_FLLST_DATE { get; set; }
public string? APQP_FLLST_RVW { get; set; }
public string? APQP_FLLST_NM { get; set; }
public string? APQP_FLLST_ETN { get; set; }
public string? APQP_FLLST_SIZE { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public apqp_filelst()
        {
        }
    }
}

