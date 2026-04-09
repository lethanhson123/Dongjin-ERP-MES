namespace MESData.Model
{
    public partial class tsurau : BaseModel
    {
        [Key]
        public int? USER_AUTH_IDX { get; set; }
public int? USER_IDX { get; set; }
public int? AUTH_IDX { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsurau()
        {
        }
    }
}

