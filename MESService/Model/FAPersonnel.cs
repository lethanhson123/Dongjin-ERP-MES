namespace MESService.Model
{
    public class FAPersonnel
    {
        public long ID { get; set; }
        public string? Line { get; set; }
        public string? Family { get; set; }
        public string? PlanDay { get; set; }
        public string? TimeWork { get; set; }
        public string? Productivity { get; set; }
        public int? WorkersNeed { get; set; }
        public int? WorkersActual { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string? CreateUserName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUserName { get; set; }
    }

}

