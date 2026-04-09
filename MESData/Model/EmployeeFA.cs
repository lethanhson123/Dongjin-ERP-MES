namespace MESData.Model
{
    public class EmployeeFA
    {
        public long ID { get; set; }
        public long? DefaultShiftID { get; set; }
        public string? Name { get; set; }
        public string? Dept { get; set; }
        public string? Line { get; set; }
        public string? Process { get; set; }
        public string? EmpCode { get; set; }
        public DateTime? HireDate { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}