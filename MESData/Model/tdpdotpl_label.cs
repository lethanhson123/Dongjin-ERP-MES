namespace MESData.Model
{
    public partial class tdpdotpl_label : BaseModel
    {
        [Key]
        public int? PDOTPL_IDX { get; set; }
public DateTime? PDLB_DATE { get; set; }
public int? LABEL_NO { get; set; }
public string? LABEL_TXT { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public tdpdotpl_label()
        {
        }
    }
}

