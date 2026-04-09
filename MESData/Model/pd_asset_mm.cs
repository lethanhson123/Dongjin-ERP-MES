namespace MESData.Model
{
    public partial class pd_asset_mm : BaseModel
    {
        [Key]
        public int? PD_ASSET_IDX { get; set; }
public string? PDP_ASSET_ID { get; set; }
public DateTime? PDP_ASSET_INDATE { get; set; }
public string? PDP_ASSET_NAME { get; set; }
public string? PDP_ASSET_SPC { get; set; }
public int? PDP_DEPA { get; set; }
public string? PDP_DEPA_USER { get; set; }
public DateTime? PDP_DEPA_USERDT { get; set; }
public string? PDP_DEPA_DYN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pd_asset_mm()
        {
        }
    }
}

