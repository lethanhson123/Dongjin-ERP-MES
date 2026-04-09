namespace Data.Model
{
    public partial class MembershipHistoryURL : BaseModel
    {

        public string? Token { get; set; }
        public string? URL { get; set; }
        public DateTime? Date { get; set; }
        public string? IPAddress { get; set; }
        public string? IPAddressLocal { get; set; }
        public string? Type { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
      
      
        public MembershipHistoryURL()
        {
        }
    }
}

