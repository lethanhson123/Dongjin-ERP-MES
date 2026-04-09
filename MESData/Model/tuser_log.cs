namespace MESData.Model
{
    public partial class tuser_log : BaseModel
    {
        [Key]
        public int? TUSER_IDX { get; set; }
public string? TS_MC_NM { get; set; }
public DateTime? TS_DATE { get; set; }
public DateTime? TS_TIME_ST { get; set; }
public DateTime? TS_TIME_END { get; set; }
public string? TS_USER { get; set; }

        public tuser_log()
        {
        }
    }
}

