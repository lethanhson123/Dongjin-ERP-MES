namespace Data.Model
{
    public partial class InvoiceOutput : BaseModel
    {

        public string? PurchaseCode { get; set; }
        public DateTime? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public long? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public long? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public decimal? Total { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalFinal { get; set; }
        public decimal? Coefficient { get; set; }
        public long? WarehouseOutputID { get; set; }
        public string? WarehouseOutputName { get; set; }
        public InvoiceOutput()
        {
            Active = true;
            Date = DateTime.Now;           
        }
    }
}

