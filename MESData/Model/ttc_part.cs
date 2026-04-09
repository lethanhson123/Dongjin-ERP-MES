namespace MESData.Model
{
    public partial class ttc_part : BaseModel
    {
        [Key]
        public int? TTC_PART_IDX { get; set; }
public string? TC_PART_NM { get; set; }
public string? TC_DESC { get; set; }
public int? RAW_PART_IDX { get; set; }
public int? TC_SIZE { get; set; }
public string? TC_MC { get; set; }
public int? TC_PACKUNIT { get; set; }
public string? TC_LOC { get; set; }
public double? TC_W_S { get; set; }
public double? TC_W_MS { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttc_part()
        {
        }
    }
}

