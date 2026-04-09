namespace MESData.Model
{
    public class EmployeeJob
    {
        public long ID { get; set; }
        public long PersonalInfoID { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeType { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? Line { get; set; }
        public string? Education { get; set; }
        public string? Specialization { get; set; }
        public string? CompanyEmail { get; set; }
        public DateTime? InterviewDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string? TimeUnit { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public virtual PersonalInfo? PersonalInfo { get; set; }
    }
}