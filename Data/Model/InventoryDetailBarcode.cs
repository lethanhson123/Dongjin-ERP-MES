namespace Data.Model
{
    public partial class InventoryDetailBarcode : BaseModel
    {
        public long? WarehouseInputDetailID { get; set; }
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
        public bool? IsSNP { get; set; }
        public decimal? QuantitySNP { get; set; }
        public decimal? QuantityInvoice { get; set; }
        public decimal? QuantityInventory { get; set; }
        public decimal? QuantityOutput { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityMES { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalInvoice { get; set; }
        public decimal? Total { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalFinal { get; set; }
        public decimal? Coefficient { get; set; }
        public decimal? QuantityOutputMES { get; set; }
        public decimal? QuantityInventoryMES { get; set; }
        public bool? IsExport { get; set; }
        public long? CategoryLocationID { get; set; }
        public string? CategoryLocationName { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public bool? IsScan { get; set; }
        public long? MESID { get; set; }
        public string? GroupCode { get; set; }
        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public bool? IsSync { get; set; }
        public double? PKG_QTY { get; set; }
        public double? PKG_OUTQTY { get; set; }
        public double? PKG_QTYActual { get; set; }
        public string? BARCD_LOC { get; set; }
        public string? PKG_GRP { get; set; }
        public DateTime? OUT_DTM { get; set; }
        public DateTime? CREATE_DTM { get; set; }
        public DateTime? Date01 { get; set; }
        public DateTime? Date02 { get; set; }
        public DateTime? Date03 { get; set; }
        public long? UpdateUserID01 { get; set; }
        public long? UpdateUserID02 { get; set; }
        public long? UpdateUserID03 { get; set; }
        public string? UpdateUserCode01 { get; set; }
        public string? UpdateUserCode02 { get; set; }
        public string? UpdateUserCode03 { get; set; }
        public string? UpdateUserName01 { get; set; }
        public string? UpdateUserName02 { get; set; }
        public string? UpdateUserName03 { get; set; }
        public decimal? Quantity01 { get; set; }
        public decimal? Quantity02 { get; set; }
        public decimal? Quantity03 { get; set; }
        public decimal? QuantityGAP01 { get; set; }
        public decimal? QuantityGAP02 { get; set; }
        public decimal? QuantityGAP03 { get; set; }
        public long? BOMID { get; set; }
        public string? ECN { get; set; }
        public string? BOMECNVersion { get; set; }
        public DateTime? BOMDate { get; set; }
        public string? InvoiceInputName { get; set; }
        public InventoryDetailBarcode()
        {
            Active = false;
            IsExport = false;
            DateScan = GlobalHelper.InitializationDateTime;
        }
    }
}

