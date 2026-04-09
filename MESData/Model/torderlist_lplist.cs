namespace MESData.Model
{
    public partial class torderlist_lplist : BaseModel
    {
        [Key]
        public int? TORDERLIST_LPLIST_IDX { get; set; }
public string? LPLIST_LEADNO { get; set; }
public string? LPLIST_DYN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torderlist_lplist()
        {
        }
    }
}

