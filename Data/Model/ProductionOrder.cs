namespace Data.Model
{
    public partial class ProductionOrder : BaseModel
    {
        public DateTime? Date { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? DaySpan { get; set; }
        public long? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public long? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public bool? IsComplete { get; set; }
        public bool? IsSync { get; set; }

        public ProductionOrder()
        {
            Active = true;
            IsComplete = false;
            IsSync = false;
            Date = DateTime.Now;
            DateEnd = Date;
            SupplierID = GlobalHelper.CompanyID;
            CustomerID = GlobalHelper.CompanyID;
            DaySpan = GlobalHelper.DaySpan;
        }
    }
}

