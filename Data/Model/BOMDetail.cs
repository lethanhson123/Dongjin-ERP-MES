namespace Data.Model
{
    public partial class BOMDetail : BaseModel
    {
        public string? Version { get; set; }
        public long? MaterialID01 { get; set; }
        public string? MaterialName01 { get; set; }
        public string? MaterialCode01 { get; set; }
        public decimal? Quantity01 { get; set; }
        public long? CategoryUnitID01 { get; set; }
        public string? CategoryUnitName01 { get; set; }
        public long? CategoryMaterialID01 { get; set; }
        public string? CategoryMaterialName01 { get; set; }
        public long? MaterialID02 { get; set; }
        public string? MaterialName02 { get; set; }
        public string? MaterialCode02 { get; set; }
        public decimal? Quantity02 { get; set; }
        public decimal? QuantityActual { get; set; }
        public decimal? QuantityCompare { get; set; }
        public decimal? Percent { get; set; }
        public long? CategoryUnitID02 { get; set; }
        public string? CategoryUnitName02 { get; set; }
        public long? CategoryMaterialID02 { get; set; }
        public string? CategoryMaterialName02 { get; set; }
        public long? BOMRawDetailID { get; set; }
        public long? TechInfoID { get; set; }
        public string? Wire { get; set; }
        public decimal? Diameter { get; set; }
        public string? Color { get; set; }
        public string? WRNo { get; set; }
        public string? Category { get; set; }
        public decimal? QuantitySumActual { get; set; }
        public decimal? QuantitySumCompare { get; set; }
        public decimal? PercentSum { get; set; }
        public BOMDetail()
        {
        }
    }
}

