namespace MESData.Model
{
    public partial class tsbom_part_inf : BaseModel
    {
        [Key]
        public int? INF_BOM_IDX { get; set; }
public int? INF_PART_IDX { get; set; }
public string? INF_PART_NONM { get; set; }
public string? INF_BOM_UNIT { get; set; }
public string? INF_BOM_MAKER { get; set; }
public double? INF_BOM_WEIGHT { get; set; }
public string? INF_BOM_COLOR { get; set; }
public string? INF_BOM_MOQ { get; set; }
public string? INF_BOM_RMK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsbom_part_inf()
        {
        }
    }
}

