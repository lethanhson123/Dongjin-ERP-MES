namespace MESService.Model
{
    public class NGDataTranfer
    {
        public int ID {  get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? SupplierName { get; set; }
        public string? Barcode { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public string? InvoiceName { get; set; }
        public double? Quantity { get; set; }
        public double? TotalInvoice { get; set; }
        public DateTime? InputDate { get; set; }
        public decimal QuantityInvoice { get; set; }
        public string? CategoryUnitName { get; set; }
    }
}
