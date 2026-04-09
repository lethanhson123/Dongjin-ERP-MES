namespace MESData.Model
{
    public partial class torder_bom_spst2 : BaseModel
    {
        [Key]
        public int? TORDER_BOMSPST2_IDX { get; set; }
public int? TORDER_BOMSPST_IDX { get; set; }
public int? TORDER_SPSTORDER_IDX { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torder_bom_spst2()
        {
        }
    }
}

