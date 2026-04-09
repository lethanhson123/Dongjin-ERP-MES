namespace MESData.Model
{
    public class EmployeeContract
    {
        public long ID { get; set; }
        public long PersonalInfoID { get; set; }
        public string ContractType { get; set; } = null!;
        public DateTime ContractDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ContractFile { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public virtual PersonalInfo? PersonalInfo { get; set; }
    }
}