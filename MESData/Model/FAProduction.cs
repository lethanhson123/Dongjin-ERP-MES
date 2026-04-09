namespace MESData.Model
{
    public class FAProduction
    {
        public long ID { get; set; }
        public string? Line { get; set; }
        public string? Barcode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }

    }
}

