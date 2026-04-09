namespace MESData.Model
{
    public class FAWorkOrder
    {
        public int ID { get; set; }
        public long? LineID { get; set; }
        public string PartNumber { get; set; }
        public string? ECN { get; set; }
        public string? New_ECN { get; set; }
        public int? SNP { get; set; }
        public string? Remark { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public virtual LineList? Line { get; set; }
    }
}