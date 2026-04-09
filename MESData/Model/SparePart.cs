namespace MESData.Model
{
    public class SparePart
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Supplier { get; set; }
        public int? QuantityRequired { get; set; }
        public int? SafetyStock { get; set; }
        public int? Inventory { get; set; }
        public string? FileName { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUserName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUserName { get; set; }
        public virtual ICollection<SparePartScanIn>? ScanInHistory { get; set; }
        public virtual ICollection<SparePartScanOut>? ScanOutHistory { get; set; }
    }
}
