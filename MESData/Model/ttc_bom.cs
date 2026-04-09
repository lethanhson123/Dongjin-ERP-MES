namespace MESData.Model
{
    public partial class ttc_bom : BaseModel
    {
        [Key]
        public int? TTC_BOM_IDX { get; set; }
public int? PART_IDX { get; set; }
public int? TTC_PART_IDX { get; set; }
public double? TTC_BOMSNP { get; set; }
public string? TCC_DSVYN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttc_bom()
        {
        }
    }
}

