namespace MESData.Model
{
    public partial class torder_bom_spst1 : BaseModel
    {
        [Key]
        public int? TORDER_BOMSPST_IDX { get; set; }
public string? TORDER_BARCODENM { get; set; }
public double? BARCODE_QTY { get; set; }
public double? USE_QTY { get; set; }
public string? DSCN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torder_bom_spst1()
        {
        }
    }
}

