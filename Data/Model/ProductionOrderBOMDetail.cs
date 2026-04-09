namespace Data.Model
{
    public partial class ProductionOrderBOMDetail : BaseModel
    {
        public long? BOMID { get; set; }
        public string? ECN { get; set; }
        public long? ProductionOrderBOMID { get; set; }
        public long? ProductionOrderDetailID { get; set; }        
        public long? BOMDetailID { get; set; }
        public long? MaterialID { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialPartNumber { get; set; }
        public long? CategoryUnitID { get; set; }
        public string? CategoryUnitName { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public bool? IsSNP { get; set; }
        public int? QuantitySNP { get; set; }
        public int? QuantityBox { get; set; }
        public decimal? QuantityBOM { get; set; }
        public decimal? QuantityPO { get; set; }
        public decimal? Quantity { get; set; }

        public ProductionOrderBOMDetail()
        {
        }
    }
}

