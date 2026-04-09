namespace MESService.Model
{
    public class TaskTimeStandard
    {
        public int ID { get; set; }
        public string? ECNNo { get; set; }
        public string? Vehicle { get; set; }
        public string? Family { get; set; }
        public string? PartNo { get; set; }
        public int? Lead { get; set; }
        public decimal? CT { get; set; }
        public decimal? LP { get; set; }
        public decimal? FA { get; set; }
        public decimal? Total { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUserName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUserName { get; set; }
    }

}

