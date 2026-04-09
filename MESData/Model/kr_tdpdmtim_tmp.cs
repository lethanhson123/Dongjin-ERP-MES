namespace MESData.Model
{
    public partial class kr_tdpdmtim_tmp : BaseModel
    {
        [Key]
        public int? KR_TDPDMTIN_IDX { get; set; }
public string? KR_TDPDMTIN_POCODE { get; set; }
public string? KR_TDPDMTIN_PLTNO { get; set; }
public string? KR_TDPDMTIN_PART_NO { get; set; }
public int? KR_TDPDMTIN_SNP { get; set; }
public string? KR_TDPDMTIN_VLID_GRP { get; set; }
public DateTime? KR_TDPDMTIN_DTM { get; set; }
public string? KR_TDPDMTIN_BARCODE { get; set; }
public string? KR_TDPDMTIN_REMARK { get; set; }
public string? KR_TDPDMTIN_DSNY { get; set; }
public string? KR_TDPDMTIN_GRUPCD { get; set; }
public string? KR_TDPDMTIN_INSPNY { get; set; }
public int? KR_TDPDMTIN_MIDX { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public kr_tdpdmtim_tmp()
        {
        }
    }
}

