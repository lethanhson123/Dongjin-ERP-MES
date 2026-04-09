namespace MESData.Model
{
    public class NGList
    {
        public long ID { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorType { get; set; }
        public string? ErrorDescription { get; set; }
        public string? KoreanDescription { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}