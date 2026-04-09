namespace MESData.Model
{
    public partial class tsbom_ver02 : BaseModel
    {
        [Key]
        public int? BOM_IDX { get; set; }
public int? PAREN_PART_IDX { get; set; }
public int? PAREN_EO_IDX { get; set; }
public int? PART_IDX { get; set; }
public int? LV_NO { get; set; }
public string? BOM_GRUP { get; set; }
public double? BOM_DES { get; set; }
public string? BOM_RMK { get; set; }
public string? DSYN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsbom_ver02()
        {
        }
    }
}

