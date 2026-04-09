namespace MESData.Model
{
    public partial class ttensilforce : BaseModel
    {
        [Key]
        public int? FORCE_IDX { get; set; }
public string? FORCE_NM { get; set; }
public double? FORCE_MIN { get; set; }
public double? FORCE_MAX { get; set; }
public double? STRENGTH { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttensilforce()
        {
        }
    }
}

