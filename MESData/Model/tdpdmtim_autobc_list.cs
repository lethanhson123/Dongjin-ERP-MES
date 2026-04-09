namespace MESData.Model
{
    public partial class tdpdmtim_autobc_list : BaseModel
    {
        [Key]
        public int? PDMTINABC_IDX { get; set; }
public int? PDMTINABC_PRT_IDX { get; set; }
public string? PDMTN_BARCODE { get; set; }
public string? PDMTINABC_DSCN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdpdmtim_autobc_list()
        {
        }
    }
}

