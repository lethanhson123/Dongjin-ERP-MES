namespace Data.Model
{
    public partial class InvoiceInputHistory : BaseModel
    {
        public long? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public DateTime? DateETD { get; set; }
        public DateTime? DateETA { get; set; }
        public DateTime? DateATA { get; set; }
        public DateTime? DateFT { get; set; }
        public DateTime? DateReal { get; set; }
        public InvoiceInputHistory()
        {           
        }
    }
}

