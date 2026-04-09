namespace MESData.Model
{
    public partial class trackmaster : BaseModel
    {
        [Key]
        public int? RACK_IDX { get; set; }
public string? LEAD_NO { get; set; }
public string? HOOK_RACK { get; set; }
public int? SFTY_STK { get; set; }
public string? REP { get; set; }
public string? CUR_LEADS { get; set; }
public string? SP { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public trackmaster()
        {
        }
    }
}

