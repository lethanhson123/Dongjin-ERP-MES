namespace MESData.Model
{
    public partial class ttensilbndlst : BaseModel
    {
        [Key]
        public int? BNDLST_IDX { get; set; }
public string? BNDLST_NM { get; set; }
public double? BNDLST_MIN { get; set; }
public double? BNDLST_MAX { get; set; }
public double? STRENGTH { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttensilbndlst()
        {
        }
    }
}

