namespace Data.Model
{
    public partial class WarehouseOutput : BaseModel
    {
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
        public long? WarehouseRequestID { get; set; }
        public string? WarehouseRequestName { get; set; }
        public bool? IsSync { get; set; }
        public bool? IsComplete { get; set; }
        public long? MembershipID { get; set; }
        public string? MembershipCode { get; set; }
        public string? MembershipName { get; set; }
        public WarehouseOutput()
        {
            IsSync = false;
            IsComplete = false;
            Active = true;
            Date = DateTime.Now;          
        }
    }
}

