namespace MESData.Model
{
    public partial class tmbrcd_longterm : BaseModel
    {
        [Key]
        public int? TMBRCD_LONGTERM_IDX { get; set; }
public int? LT_BARCD_IDX { get; set; }
public DateTime? LT_INS_DATE { get; set; }
public string? LT_RUS { get; set; }
public string? LT_STAY { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tmbrcd_longterm()
        {
        }
    }
}

