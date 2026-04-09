namespace MESData.Model
{
    public partial class tdpdotpl_etc : BaseModel
    {
        [Key]
        public int? TDPDOTPL_ETC_IDX { get; set; }
public string? ETC_PO_CODE { get; set; }
public DateTime? DATE { get; set; }
public int? PART_IDX { get; set; }
public int? QTY { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tdpdotpl_etc()
        {
        }
    }
}

