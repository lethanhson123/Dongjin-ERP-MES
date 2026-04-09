namespace MESData.Model
{
    public partial class tscmpny : BaseModel
    {
        [Key]
        public int? CMPNY_IDX { get; set; }
public string? CMPNY_NM { get; set; }
public string? CMPNY_DVS { get; set; }
public string? CMPNY_ADDR { get; set; }
public string? CMPNY_TEL { get; set; }
public string? CMPNY_FAX { get; set; }
public string? CMPNY_mngr { get; set; }
public string? CMPNY_rmk { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tscmpny()
        {
        }
    }
}

