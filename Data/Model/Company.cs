namespace Data.Model
{
    public partial class Company : BaseModel
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Person { get; set; }
        public Company()
        {
        }
    }
}

