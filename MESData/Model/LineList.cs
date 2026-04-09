namespace MESData.Model
{
    public class LineList
    {
        public long ID { get; set; }
        public string? LineGroup { get; set; }
        public string? LineName { get; set; }
        public string? LineType { get; set; }
        public string? Family { get; set; }
        public decimal? LineCapa { get; set; }
        public int? WorkerNumber { get; set; }
        public int? SUB_Worker { get; set; }
        public int? FA_Worker { get; set; }
        public int? RO_Worker { get; set; }
        public int? CLIP_Worker { get; set; }
        public int? Channel { get; set; }
        public int? Vision_LeakTest { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}