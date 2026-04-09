namespace MESData.Model
{
    public partial class track_bc_tmp : BaseModel
    {
        [Key]
        public int? TRACK_BC_IDX { get; set; }
public string? TRACK_BC_LEAD { get; set; }
public string? TRACK_BC_NAME { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public track_bc_tmp()
        {
        }
    }
}

