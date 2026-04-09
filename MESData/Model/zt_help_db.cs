namespace MESData.Model
{
    public partial class zt_help_db : BaseModel
    {
        [Key]
        public int? IDX { get; set; }
public string? MENU_NM { get; set; }
public string? TAB_NAME { get; set; }
public string? REV_NO { get; set; }
public string? FILE_NM { get; set; }
public string? FILE_EX { get; set; }
public string? DN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }

        public zt_help_db()
        {
        }
    }
}

