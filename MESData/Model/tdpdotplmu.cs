namespace MESData.Model
{
    public partial class tdpdotplmu : BaseModel
    {
        [Key]
        public int? TDPDOTPLMU_IDX { get; set; }
public int? PDOTPL_IDX { get; set; }
public string? PLET_NO { get; set; }
public string? PLET_COMS { get; set; }
public DateTime? PDOTPL_DTM { get; set; }
public int? QTY { get; set; }
public int? SE_QTY { get; set; }
public string? DSCN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdpdotplmu()
        {
        }
    }
}

