namespace MESData.Model
{
    public partial class tuser_log_chk : BaseModel
    {
        [Key]
        public int? TUSER_IDX { get; set; }
public string? TS_USERID { get; set; }
public string? TS_USER_IP { get; set; }
public string? TS_USER_CNN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public tuser_log_chk()
        {
        }
    }
}

