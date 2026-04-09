namespace Data.Model
{
    public partial class WarehouseInput : BaseModel
    {
        public bool? Root { get; set; }
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
        public long? InvoiceInputID { get; set; }
        public string? InvoiceInputName { get; set; }
        public bool? IsSync { get; set; }
        public bool? IsComplete { get; set; }
        public bool? IsStock { get; set; }
        public long? WarehouseOutputID { get; set; }
        public string? WarehouseOutputName { get; set; }
        public WarehouseInput()
        {
            //IsSync = false;
            IsComplete = false;
            Root = false;
            Active = true;
            Date = DateTime.Now;           
        }
    }
}

