namespace MESData.Model
{
    public class LineAssignmentDetailDTO
    {
        public long ID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public long LineID { get; set; } 
        public string? LineName { get; set; }
        public string? Position { get; set; }
        public long ShiftID { get; set; }  
        public string? ShiftName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
    }
}