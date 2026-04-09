namespace Data.Model
{
    public partial class WarehouseInputDetail : BaseModel
    {
        public long? CategoryDepartmentID { get; set; }
        public string? CategoryDepartmentName { get; set; }
        public DateTime? DateScan { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
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
        public decimal? QuantityInvoiceGAP { get; set; }
        public decimal? QuantityInventory { get; set; }
        public decimal? QuantityOutput { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityActual { get; set; }
        public decimal? QuantityGAP { get; set; }
        public decimal? QuantityStock { get; set; }
        public decimal? QuantityStockInput { get; set; }
        public decimal? QuantityStockOuput { get; set; }
        public decimal? QuantityStockGAP { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalFinal { get; set; }
        public decimal? Coefficient { get; set; }
        public bool? IsExport { get; set; }
        public bool? IsStock { get; set; }
        public long? CategoryLocationID { get; set; }
        public string? CategoryLocationName { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public long? CategoryCompanyID { get; set; }
        public string? CategoryCompanyName { get; set; }
        public WarehouseInputDetail()
        {
            Active = false;
            IsExport = false;
            DateBegin = new DateTime(2025, 12, 31);
            DateEnd = new DateTime(2025, 12, 31);
        }
    }
}

