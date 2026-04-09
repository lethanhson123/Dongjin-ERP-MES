namespace MESData.Model
{
    public partial class tsnon_oper : BaseModel
    {
        [Key]
        public int? TSNON_OPER_IDX { get; set; }
public string? TSNON_OPER_CODE { get; set; }
public string? TSNON_OPER_MCNM { get; set; }
public string? TSNON_OPER_USERNM { get; set; }
public DateTime? TSNON_OPER_DATE { get; set; }
public DateTime? TSNON_OPER_STIME { get; set; }
public DateTime? TSNON_OPER_ETIME { get; set; }
public int? TSNON_OPER_TIME { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsnon_oper()
        {
        }
    }
}

