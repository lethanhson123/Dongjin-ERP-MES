namespace MESData.Model
{
    public partial class tsbom_ver02_tmp1 : BaseModel
    {
        [Key]
        public int? BOM_IDX { get; set; }
public string? PAREN_PART_NO { get; set; }
public string? PAREN_EO_NO { get; set; }
public string? PART_NO { get; set; }
public string? BOM_GRUP { get; set; }
public double? BOM_DES { get; set; }
public string? BOM_RMK { get; set; }

        public tsbom_ver02_tmp1()
        {
        }
    }
}

