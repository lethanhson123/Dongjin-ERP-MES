namespace MESData.Model
{
    public class EmployeeFinance
    {
        public long ID { get; set; }
        public long PersonalInfoID { get; set; }
        public string? InsuranceCode { get; set; }
        public string? TaxCode { get; set; }
        public string? BankName { get; set; }
        public string? BankAccount { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public virtual PersonalInfo? PersonalInfo { get; set; }
    }
}