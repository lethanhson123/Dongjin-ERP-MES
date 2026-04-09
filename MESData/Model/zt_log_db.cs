namespace MESData.Model
{
    public partial class zt_log_db : BaseModel
    {
        [Key]
        public int? IDX { get; set; }
public string? MENU_NM { get; set; }
public string? TAB_NAME { get; set; }
public string? BUTTON_NM { get; set; }
public string? USER_ID { get; set; }
public string? PRG_CODE { get; set; }
public string? DN_YN { get; set; }
public string? DATA_TEXT { get; set; }
public string? SQL_NM { get; set; }
public DateTime? CREATE_DTM { get; set; }

        public zt_log_db()
        {
        }
    }
}

