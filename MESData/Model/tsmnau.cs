namespace MESData.Model
{
    public partial class tsmnau : BaseModel
    {
        [Key]
        public int? MENU_AUTH_IDX { get; set; }
public int? MENU_IDX { get; set; }
public int? AUTH_IDX { get; set; }
public string? MENU_AUTH_YN { get; set; }
public string? IQ_AUTH_YN { get; set; }
public string? RGST_AUTH_YN { get; set; }
public string? MDFY_AUTH_YN { get; set; }
public string? DEL_AUTH_YN { get; set; }
public string? CAN_AUTH_YN { get; set; }
public string? EXCL_AUTH_YN { get; set; }
public string? DNLD_AUTH_YN { get; set; }
public string? PRNT_AUTH_YN { get; set; }
public string? ETC1_AUTH_YN { get; set; }
public string? ETC2_AUTH_YN { get; set; }
public string? ETC3_AUTH_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsmnau()
        {
        }
    }
}

