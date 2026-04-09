namespace MESService.Model
{
    public partial class tsauthTranfer : tsauth
    {
        public int? USER_IDX { get; set; }
        public int? MENU_AUTH_IDX { get; set; }
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
        public int? MENU_IDX { get; set; }
        public int? MENU_CD { get; set; }
        public int? MENU_LVL { get; set; }      
        public string? SCRN_PATH { get; set; }
        public string? DECYN { get; set; }        
        public tsauthTranfer()
        {
        }
    }
}

