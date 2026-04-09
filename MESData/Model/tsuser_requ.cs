namespace MESData.Model
{
    public partial class tsuser_requ : BaseModel
    {
        [Key]
        public int? REQU_IDX { get; set; }
public string? REQU_ID { get; set; }
public DateTime? REQU_DATE { get; set; }
public string? REQU_NOTE { get; set; }
public string? REQU_NID { get; set; }
public string? REQU_NAME { get; set; }
public string? REQU_DEP { get; set; }
public string? REQU_TSAUTH { get; set; }
public string? REQU_DES { get; set; }
public string? REQU_APP { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsuser_requ()
        {
        }
    }
}

