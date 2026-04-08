namespace Helper.Model
{
	public struct ZaloRefreshTokenDataRespond
    {
        public string? access_token { get; set; }
        public string? refresh_token { get; set; }
        public string? expires_in { get; set; }
    }
}
