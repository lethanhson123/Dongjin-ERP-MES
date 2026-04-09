using MESData.Model;

namespace MESData.Model
{
    [Table("ToolShop")]
    public class ToolShop
    {
        public long ID { get; set; }
        public string? Code { get; set; }
        public string? Sub_Code { get; set; }
        public string? NameVN { get; set; }
        public string? NameEN { get; set; }
        public string? NameKO { get; set; }
        public string? Serial { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public DateTime? UsingDate { get; set; }
        public string? InvNumber { get; set; }
        public string? CustomDeclaration { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Currency { get; set; }
        public string? Type { get; set; }
        public string? Owner { get; set; }
        public string? Supplier { get; set; }
        public string? Dept { get; set; }
        public string? ProductionLine { get; set; }
        public string? Note { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }

        // Audit fields
        public bool Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}