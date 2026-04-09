namespace Data.Model
{
    public partial class ProductionOrderBOM : BaseModel
    {
        public long? ProductionOrderDetailID { get; set; }
        public long? BOMID { get; set; }
        public string? BOMCode { get; set; }
        public string? BOMName { get; set; }
        public string? BOMVersion { get; set; }
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
        public long? CategoryUnitID { get; set; }
        public string? CategoryUnitName { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public decimal? QuantityPO { get; set; }
        public decimal? QuantityBOM { get; set; }
        public decimal? Quantity { get; set; }

        public ProductionOrderBOM()
        {
        }
    }
}

