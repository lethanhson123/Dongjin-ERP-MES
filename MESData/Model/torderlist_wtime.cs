namespace MESData.Model
{
    public partial class torderlist_wtime : BaseModel
    {
        [Key]
        public int? TOWT_INDX { get; set; }
public string? MENU_TEXT { get; set; }
public string? USER_ID { get; set; }
public string? USER_MC { get; set; }
public int? ORDER_IDX { get; set; }
public DateTime? S_TIME { get; set; }
public DateTime? E_TIME { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public torderlist_wtime()
        {
        }
    }
}

