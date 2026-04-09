namespace MESData.Model
{
    public class PersonalInfo
    {
        public long ID { get; set; }
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime DOB { get; set; }
        public string? MaritalStatus { get; set; }
        public int? Dependents { get; set; }
        public string CitizenID { get; set; } = null!;
        public DateTime IDIssueDate { get; set; }
        public string IDIssuePlace { get; set; } = null!;
        public string PermAddress { get; set; } = null!;
        public string CurrAddress { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? RegistrationToken { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }
}