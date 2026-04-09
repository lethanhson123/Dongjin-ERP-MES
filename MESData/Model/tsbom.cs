namespace MESData.Model
{
    public partial class tsbom : BaseModel
    {
        [Key]
        public int? BOM_IDX { get; set; }
public int? PAREN_PART_IDX { get; set; }
public int? PART_IDX { get; set; }
public double? BOM_US { get; set; }
public string? BOM_DSYN { get; set; }
public string? BOM_REMARK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsbom()
        {
        }
    }
}

