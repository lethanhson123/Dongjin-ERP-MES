namespace MESData.Model
{
    public partial class tdpdmtim_loc : BaseModel
    {
        [Key]
        public int? TDLOC_IDX { get; set; }
public int? TDLOC_PART_IDX { get; set; }
public string? TDLOC_PARTNO { get; set; }
public string? TDLOC_COSM { get; set; }
public string? TDLOC_MOQ { get; set; }
public string? TDLOC_PARTNM { get; set; }
public string? TDLOC_LOC { get; set; }
public string? TDLOC_REMARK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdpdmtim_loc()
        {
        }
    }
}

