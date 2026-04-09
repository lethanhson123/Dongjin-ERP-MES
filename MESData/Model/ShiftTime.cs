namespace MESData.Model
{
    public class ShiftTime
    {
        public long ID { get; set; }
        public string? ShiftName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? BreakTime { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }

}

