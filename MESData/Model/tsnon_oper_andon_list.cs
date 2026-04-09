namespace MESData.Model
{
    public partial class tsnon_oper_andon_list : BaseModel
    {
        [Key]
        public int? TSNON_OPER_MITOR_IDX { get; set; }
public string? TSNON_OPER_MITOR_MCNM { get; set; }
public string? TSNON_OPER_MITOR_NOIC { get; set; }
public string? TSNON_OPER_MITOR_RUNYN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public tsnon_oper_andon_list()
        {
        }
    }
}

