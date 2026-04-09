namespace MESData.Model
{
    public partial class pd_cmpny_part : BaseModel
    {
        [Key]
        public int? PD_CMPNY_PART_IDX { get; set; }
public int? CMPNY_IDX { get; set; }
public int? PDPART_IDX { get; set; }
public string? DSC_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pd_cmpny_part()
        {
        }
    }
}

