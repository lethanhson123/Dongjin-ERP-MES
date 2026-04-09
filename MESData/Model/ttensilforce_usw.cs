namespace MESData.Model
{
    public partial class ttensilforce_usw : BaseModel
    {
        [Key]
        public int? FORCE_USW_IDX { get; set; }
public string? FORCE_USW_NM { get; set; }
public double? FORCE_USW_MIN_HORI { get; set; }
public double? FORCE_USW_MIN_VERT { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttensilforce_usw()
        {
        }
    }
}

