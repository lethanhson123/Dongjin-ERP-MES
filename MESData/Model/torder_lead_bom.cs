namespace MESData.Model
{
    public partial class torder_lead_bom : BaseModel
    {
        [Key]
        public int? LEAD_INDEX { get; set; }
public string? LEAD_SCN { get; set; }
public string? LEAD_PN { get; set; }
public int? BUNDLE_SIZE { get; set; }
public int? W_PN_IDX { get; set; }
public int? T1_PN_IDX { get; set; }
public int? S1_PN_IDX { get; set; }
public int? T2_PN_IDX { get; set; }
public int? S2_PN_IDX { get; set; }
public string? STRIP1 { get; set; }
public string? STRIP2 { get; set; }
public string? CCH_W1 { get; set; }
public string? ICH_W1 { get; set; }
public string? CCH_W2 { get; set; }
public string? ICH_W2 { get; set; }
public string? T1NO { get; set; }
public string? T2NO { get; set; }
public string? W_LINK { get; set; }
public string? WR_NO { get; set; }
public string? WIRE_NM { get; set; }
public string? W_Diameter { get; set; }
public string? W_Color { get; set; }
public double? W_Length { get; set; }
public string? DSCN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torder_lead_bom()
        {
        }
    }
}

