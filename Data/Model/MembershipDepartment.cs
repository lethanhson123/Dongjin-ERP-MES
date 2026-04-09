namespace Data.Model
{
    public partial class MembershipDepartment : BaseModel
    {
        public long? CategoryDepartmentID { get; set; }
        public string? CategoryDepartmentName { get; set; }
        public MembershipDepartment()
        {
        }
    }
}

