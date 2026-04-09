namespace Data.Model
{
    public partial class MembershipToken : BaseModel
    {
        public string? Token { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }

        public MembershipToken()
        {
        }
    }
}

