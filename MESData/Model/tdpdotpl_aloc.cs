namespace MESData.Model
{
    public partial class tdpdotpl_aloc : BaseModel
    {
        [Key]
        public int? PDOTPL_IDX { get; set; }
public int? PART_IDX { get; set; }
public string? D_ARRVL { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdpdotpl_aloc()
        {
        }
    }
}

