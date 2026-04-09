namespace Data.Model
{
    public partial class WarehouseRequestDetail : BaseModel
    {
        public DateTime? Date { get; set; }
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public long? ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Barcode { get; set; }
        public string? ProductionCode { get; set; }
        public long? CategoryUnitID { get; set; }        
        public string? CategoryUnitName { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public int? QuantityCheck { get; set; }
        public decimal? QuantityWIP { get; set; }
        public decimal? QuantityStock { get; set; }
        public decimal? QuantityInvoice00 { get; set; }
        public decimal? QuantityInventory { get; set; }
        public decimal? QuantityInventory00 { get; set; }
        public decimal? QuantityInvoice { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityBegin { get; set; }
        public decimal? QuantityEnd { get; set; }
        public decimal? QuantityGAP { get; set; }
        public bool? IsSNP { get; set; }
        public decimal? QuantitySNP { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalInvoice { get; set; }
        public decimal? Total { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalFinal { get; set; }
        public decimal? Coefficient { get; set; }

        public WarehouseRequestDetail()
        {
            Active = true;
        }
    }
}

