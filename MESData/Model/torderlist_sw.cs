namespace MESData.Model
{
    public partial class torderlist_sw : BaseModel
    {
        [Key]
        public int? ORDER_IDX { get; set; }
public string? MC { get; set; }
public double? TOT_QTY { get; set; }
public double? PERFORMN { get; set; }
public string? CONDITION { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torderlist_sw()
        {
        }
    }
}

