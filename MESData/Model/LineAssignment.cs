namespace MESData.Model
{
    public class LineAssignment
    {
        public long ID { get; set; }
        public long EmployeeID { get; set; }
        public long LineID { get; set; }  
        public string? Position { get; set; }
        public long ShiftID { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}