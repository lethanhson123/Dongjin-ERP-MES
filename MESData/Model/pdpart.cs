namespace MESData.Model
{
    public partial class pdpart : BaseModel
    {
        [Key]
        public int? PDPART_IDX { get; set; }
public string? PN_V { get; set; }
public string? PSPEC_V { get; set; }
public string? PN_K { get; set; }
public string? PSPEC_K { get; set; }
public string? PN_NM { get; set; }
public int? PUNIT_IDX { get; set; }
public double? PQTY { get; set; }
public string? PN_DSCN_YN { get; set; }
public string? PN_GROUP { get; set; }
public string? PN_PHOTO { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pdpart()
        {
        }
    }
}

