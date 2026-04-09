namespace MESData.Model
{
    public class PartSpareScanOut
    {
        public long ID { get; set; }
        public long? ParentID { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }

        public int? Quantity { get; set; }
        public int? SafetyQty { get; set; }
        public int? InventoryQty { get; set; }

        public string? Note { get; set; }
        public string? Description { get; set; }

        public bool Active { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? CreateUserName { get; set; }
    }
}
