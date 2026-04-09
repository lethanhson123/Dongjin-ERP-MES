namespace Data.Model
{
    public partial class ZaloToken : BaseModel
    {
        public DateTime? Date { get; set; }
        public string? OAAccessToken { get; set; }
        public string? OARefreshToken { get; set; }
        public string? AppID { get; set; }
        public string? SecretKey { get; set; }
        public string? URL { get; set; }
        public ZaloToken()
        {
            Date = GlobalHelper.InitializationDateTime;
        }
    }
}

