namespace MESData.Model
{
    public partial class tsauth : BaseModel
    {
        [Key]
        public int? AUTH_IDX { get; set; }
public string? AUTH_ID { get; set; }
public string? AUTH_NM { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsauth()
        {
        }
    }
}

