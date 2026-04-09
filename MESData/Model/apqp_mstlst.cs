namespace MESData.Model
{
    public partial class apqp_mstlst : BaseModel
    {
        [Key]
        public int? APQP_MS_IDX { get; set; }
public int? APQP_CSTM { get; set; }
public int? APQP_MS_RNK { get; set; }
public int? APQP_MS_PRJT { get; set; }
public string? APQP_MS_CODE0 { get; set; }
public int? APQP_MS_CODE1 { get; set; }
public int? APQP_MS_CODE2 { get; set; }
public int? APQP_MS_CODE3 { get; set; }
public string? APQP_MS_NM { get; set; }
public string? APQP_MS_TM { get; set; }
public int? APQP_MS_USER { get; set; }
public DateTime? APQP_MS_SDT { get; set; }
public DateTime? APQP_MS_EDT { get; set; }
public string? APQP_MS_DSYN { get; set; }
public string? APQP_MS_NOTE { get; set; }
public string? APQP_MS_REMARK { get; set; }
public string? APQP_MS_FLDSYN { get; set; }
public string? APQP_MS_FILE { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public apqp_mstlst()
        {
        }
    }
}

