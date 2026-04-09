namespace MESService.Models
{
    public class HistoryDataTranfer
    {
        public int ID { get; set; }
        public string SupplierCode { get; set; }
        public string InvoiceName { get; set; }
        public string Barcode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal NGQty { get; set; }
        public string Unit { get; set; }
        public decimal Percentage { get; set; }
        public string InputDate { get; set; }
        public string IssueDate { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public string ErrorInfo { get; set; }
        public string Picture { get; set; }
        public string Status { get; set; }
        public string ImprovementPlans { get; set; }
        public string Note { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
    }
}