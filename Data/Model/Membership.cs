namespace Data.Model
{
    public partial class Membership : BaseModel
    {

        public int? USER_IDX { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? GUID { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public long? CategoryDepartmentID { get; set; }
        public string? CategoryDepartmentName { get; set; }
        public long? CategoryPositionID { get; set; }
        public string? CategoryPositionName { get; set; }


        public Membership()
        {
        }
    }
}

