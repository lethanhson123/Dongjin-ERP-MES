namespace MESData.Model
{
    public partial class tsnon_worktime : BaseModel
    {
        [Key]
        public int? TSNON_WT_IDX { get; set; }
public int? TSNON_MCIDX { get; set; }
public int? TSNON_SHIF { get; set; }
public DateTime? TSNON_DATE { get; set; }
public DateTime? TSNON_ST { get; set; }
public DateTime? TSNON_ET { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsnon_worktime()
        {
        }
    }
}

