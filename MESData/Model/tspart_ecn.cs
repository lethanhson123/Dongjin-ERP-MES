namespace MESData.Model
{
    public partial class tspart_ecn : BaseModel
    {
        [Key]
        public int? PARTECN_IDX { get; set; }
public int? PART_IDX { get; set; }
public string? PART_ENCNO { get; set; }
public DateTime? PART_ECN_DATE { get; set; }
public string? DWG_NO { get; set; }
public string? DWG_FILE_GRP { get; set; }
public string? DWG_FILE_EXPOR { get; set; }
public string? PART_ECN_USENY { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tspart_ecn()
        {
        }
    }
}

