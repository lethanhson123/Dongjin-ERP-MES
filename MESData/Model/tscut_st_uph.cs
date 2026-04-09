namespace MESData.Model
{
    public partial class tscut_st_uph : BaseModel
    {
        [Key]
        public int? TSCUT_ST_UPH_IDX { get; set; }
public string? TSCUT_ST_UPH_RUT { get; set; }
public string? TSCUT_ST_UPH_NAME { get; set; }
public double? TSCUT_ST_UPH_MIN { get; set; }
public double? TSCUT_ST_UPH_MAX { get; set; }
public double? TSCUT_ST_UPH_TCNT { get; set; }
public double? TSCUT_ST_UPH_SCNT { get; set; }
public double? TSCUT_ST_UPH_RUS { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tscut_st_uph()
        {
        }
    }
}

