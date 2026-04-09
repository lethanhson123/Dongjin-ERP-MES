namespace MESData.Model
{
    public class AttendanceRecords
    {
        public long? ID { get; set; }
        public long? EmployeeFAID { get; set; }
        public long? ShiftID { get; set; }
        public string? Line { get; set; }
        public DateTime? ScanIn { get; set; }
        public DateTime? ScanOut { get; set; }
        public decimal? WorkingTime { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalDownTime { get; set; }
        public bool? Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}