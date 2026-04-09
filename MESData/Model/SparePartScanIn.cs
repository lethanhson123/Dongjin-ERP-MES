namespace MESData.Model
{
    public class SparePartScanIn
    {
        public long ID { get; set; }
        public long SparePartID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public int? SafetyQty { get; set; }
        public int? InventoryQty { get; set; }
        public string? Note { get; set; }
        public string? ReasonForUse { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUserName { get; set; }
        public virtual SparePart? SparePart { get; set; }
    }
}
