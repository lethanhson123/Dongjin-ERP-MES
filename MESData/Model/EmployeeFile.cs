namespace MESData.Model
{
    public class EmployeeFile
    {
        public long ID { get; set; }
        public long PersonalInfoID { get; set; }
        public string FileName { get; set; } = null!;
        public string OriginalFileName { get; set; } = null!;
        public string? FileType { get; set; }
        public string FilePath { get; set; } = null!;
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public virtual PersonalInfo? PersonalInfo { get; set; }
    }
}