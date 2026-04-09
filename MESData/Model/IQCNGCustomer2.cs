namespace MESData.Model
{
    public class IQCNGCustomer2
    {
        public long ID { get; set; }
        public string? SupplierCode { get; set; }
        public string? Barcode { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public double? Quantity { get; set; }
        public double? TotalInvoice { get; set; }
        public double? NGQty { get; set; }
        public string? InvoiceName { get; set; }
        public string? Unit { get; set; }
        public DateTime? InputDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public int? WarehouseInputDetailBarcodeID { get; set; }
        public int? Week { get; set; }
        public int? Month { get; set; }
        public string? ErrorInfo { get; set; }
        public string? Picture { get; set; }
        public string? ImprovementPlans { get; set; }
        public string? Note { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}