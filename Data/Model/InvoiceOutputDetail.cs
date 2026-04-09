namespace Data.Model
{
    public partial class InvoiceOutputDetail : BaseModel
    {

        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public long? ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Barcode { get; set; }
        public string? ProductionCode { get; set; }
        public long? CategoryUnitID { get; set; }
        public string? CategoryUnitName { get; set; }
        public decimal? QuantityInvoice { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalFinal { get; set; }
        public decimal? Coefficient { get; set; }
        public string? PalletNo { get; set; }
        public string? ShippedNo { get; set; }

        public InvoiceOutputDetail()
        {
            Active = true;
        }
    }
}

