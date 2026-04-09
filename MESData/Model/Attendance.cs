namespace MESData.Model
{
    public class Attendance
    {
        public int ID { get; set; }
        public string? MachineName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime CheckTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte? CheckType { get; set; }
    }
}