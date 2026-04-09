namespace Data.Model
{
    public partial class InvoiceInput : BaseModel
    {
        public long? ProductionOrderID { get; set; }
        public string? ProductionOrderName { get; set; }
        public string? PurchaseCode { get; set; }
        public DateTime? DateETD { get; set; }
        public DateTime? DateETA { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public long? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public long? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? Total { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalFinal { get; set; }
        public decimal? Coefficient { get; set; }
        public bool? IsFuture { get; set; }
        public bool? IsSync { get; set; }
        public bool? IsComplete { get; set; }
        public InvoiceInput()
        {
            Active = true;
            IsSync = false;
            IsComplete = false;
            DateETD = DateTime.Now;
            DateETA = DateTime.Now;           
        }
    }
}

