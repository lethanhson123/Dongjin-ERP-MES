namespace MESData.Model
{
    public partial class ttc_order : BaseModel
    {
        [Key]
        public int? TTC_PO_INX { get; set; }
public int? TTC_PN_IDX { get; set; }
public DateTime? TTC_PO_DT { get; set; }
public int? TTC_PO { get; set; }
public int? PERFORMN { get; set; }
public string? CONDITION { get; set; }
public string? DSCN_YN { get; set; }
public string? ERROR_YN { get; set; }
public string? TTC_ENG { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public ttc_order()
        {
        }
    }
}

