namespace MESData.Model
{
    public class AttendanceSession
    {
        public long ID { get; set; }
        public long EmployeeID { get; set; }
        public long ShiftID { get; set; }
        public DateTime WorkDate { get; set; }
        public string? Line { get; set; }
        public int? CheckInID { get; set; }
        public DateTime? CheckInTime { get; set; }
        public int? CheckOutID { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int? WorkingMinutes { get; set; }
        public int LateMinutes { get; set; }
        public int OvertimeMinutes { get; set; }
        public string? Status { get; set; }
        public bool IsLate { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}