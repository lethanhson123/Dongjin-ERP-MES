namespace MESData.Model
{
    public partial class torderlist_work : BaseModel
    {
        [Key]
        public int? TORDERLIST_WORK_IDX { get; set; }
public string? TORDERLIST_WORK_MC { get; set; }
public int? TORDERLIST_WORK_ORDERIDX { get; set; }
public string? TORDERLIST_WORK_USER { get; set; }

        public torderlist_work()
        {
        }
    }
}

