namespace Data.Model
{
    public partial class InventoryDetail : BaseModel
    {
        public DateTime? DateScan { get; set; }
        public DateTime? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? Week { get; set; }
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public long? ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Barcode { get; set; }
        public string? ProductionCode { get; set; }
        public long? CategoryUnitID { get; set; }
        public string? CategoryUnitName { get; set; }
        public decimal? TotalInvoice { get; set; }
        public bool? IsSNP { get; set; }
        public decimal? QuantitySNP { get; set; }
        public decimal? QuantityInvoice { get; set; }
        public decimal? QuantityInventory { get; set; }
        public decimal? QuantityOutput { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityActual { get; set; }
        public decimal? QuantityGAP { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalFinal { get; set; }
        public decimal? Coefficient { get; set; }
        public bool? IsExport { get; set; }
        public long? CategoryLocationID { get; set; }
        public string? CategoryLocationName { get; set; }

        public InventoryDetail()
        {
            Active = false;
            IsExport = false;
            UpdateDate = GlobalHelper.InitializationDateTime.AddHours(1);
        }
    }
}

