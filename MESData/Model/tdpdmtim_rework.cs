namespace MESData.Model
{
    public partial class tdpdmtim_rework : BaseModel
    {
        [Key]
        public int? PDMTIN_IDX { get; set; }
public int? VLID_PART_IDX { get; set; }
public int? VLID_PART_SNP { get; set; }
public string? VLID_GRP { get; set; }
public DateTime? VLID_DTM { get; set; }
public string? VLID_BARCODE { get; set; }
public string? VLID_REMARK { get; set; }
public int? TDPDOTPLMU_IDX { get; set; }
public int? PDOTPL_IDX { get; set; }
public int? TSCOST_IDX { get; set; }
public string? BARCD_LOC { get; set; }
public string? VLID_DSCN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdpdmtim_rework()
        {
        }
    }
}

