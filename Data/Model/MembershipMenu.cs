namespace Data.Model
{
    public partial class MembershipMenu : BaseModel
    {
        public long? CategoryMenuID { get; set; }
        public string? CategoryMenuName { get; set; }

        public MembershipMenu()
        {
        }
    }
}

