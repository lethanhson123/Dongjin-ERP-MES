namespace Helper.Model
{
	public struct ZaloZNSDataRequest
    {
        public string? phone { get; set; }
        public string? template_id { get; set; }
        public string? tracking_id { get; set; }
        public template_data? template_data { get; set; }
    }
}
