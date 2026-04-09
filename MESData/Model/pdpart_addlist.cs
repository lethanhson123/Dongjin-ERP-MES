namespace MESData.Model
{
    public partial class pdpart_addlist : BaseModel
    {
        [Key]
        public int? PDPART_AL_IDX { get; set; }
public string? PN_NM_AL { get; set; }
public string? PN_V_AL { get; set; }
public string? PSPEC_V_AL { get; set; }
public string? PN_K_AL { get; set; }
public string? PSPEC_K_AL { get; set; }
public double? PQTY_AL { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public pdpart_addlist()
        {
        }
    }
}

