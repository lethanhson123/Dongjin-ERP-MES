namespace MESData.Model
{
    public partial class tdpdmtin_serial : BaseModel
    {
        [Key]
        public int? TDPDMTIN_SERIAL_IDX { get; set; }
public int? PART_IDX { get; set; }
public DateTime? TDPD_DATE { get; set; }
public int? SEQ_NO { get; set; }
public string? SERIAL_NO { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public string? CustomerCode { get; set; }

        public tdpdmtin_serial()
        {
        }
    }
}

