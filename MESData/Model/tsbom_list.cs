namespace MESData.Model
{
    public partial class tsbom_list : BaseModel
    {
        [Key]
        public int? BOM_LIST_IDX { get; set; }
public string? PAREN_PART_NO { get; set; }
public string? PART_NO { get; set; }
public string? PART_NONM { get; set; }
public string? BOM_DES { get; set; }
public string? BOM_UNIT { get; set; }
public string? BOM_GRUP { get; set; }
public string? BOM_RMK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsbom_list()
        {
        }
    }
}

