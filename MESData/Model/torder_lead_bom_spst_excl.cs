namespace MESData.Model
{
    public partial class torder_lead_bom_spst_excl : BaseModel
    {
        [Key]
        public int? SPST_IDX { get; set; }
public string? M_PART_IDX { get; set; }
public string? S_PART_IDX { get; set; }
public int? RQR_MENT { get; set; }
public string? S_LR { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torder_lead_bom_spst_excl()
        {
        }
    }
}

