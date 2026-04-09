namespace MESData.Model
{
    public class Trolley
    {
        public int ID { get; set; }
        public string? TrolleyCode { get; set; }
        public string? Location { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
    }
}

