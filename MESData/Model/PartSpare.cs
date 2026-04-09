namespace MESData.Model
{
    public class PartSpare
    {
        public long ID { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Display { get; set; }
        public string? Description { get; set; }

        public int? QuantityRequired { get; set; }
        public int? SafetyStock { get; set; }
        public int? InventoryWarning { get; set; }
        public int? Inventory { get; set; }

        public decimal? Price { get; set; }
        public decimal? TotalAmount { get; set; }

        public string? FileName { get; set; }

        public bool Active { get; set; } = true;

        public DateTime? CreateDate { get; set; }
        public string? CreateUserName { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string? UpdateUserName { get; set; }
    }
}
