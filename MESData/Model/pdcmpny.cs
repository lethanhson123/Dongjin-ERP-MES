namespace MESData.Model
{
    public partial class pdcmpny : BaseModel
    {
        [Key]
        public int? CMPNY_IDX { get; set; }
public string? CMPNY_NM { get; set; }
public string? CMPNY_DVS { get; set; }
public string? CMPNY_NO { get; set; }
public string? CMPNY_ADDR { get; set; }
public string? CMPNY_TEL { get; set; }
public string? CMPNY_FAX { get; set; }
public string? CMPNY_MNGR { get; set; }
public string? CMPNY_RMK { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pdcmpny()
        {
        }
    }
}

