namespace Data.Model
{
    public partial class Material : BaseModel
    {
        public string? PartNumber { get; set; }
        public bool? IsSNP { get; set; }
        public bool? IsFIFO { get; set; }
        public bool? IsSpecial { get; set; }
        public long? MESID { get; set; }
        public DateTime? MESCreateDate { get; set; }
        public string? MESCreateUserCode { get; set; }
        public string? MESCreateUserName { get; set; }
        public DateTime? MESUpdateDate { get; set; }
        public string? MESUpdateUserCode { get; set; }
        public string? MESUpdateUserName { get; set; }
        public long? FactoryID { get; set; }
        public string? FactoryName { get; set; }
        public bool? IsFactory01 { get; set; }
        public bool? IsFactory02 { get; set; }
        public decimal? QuantityInput { get; set; }
        public decimal? QuantityOutput { get; set; }
        public int? QuantitySNP { get; set; }
        public int? QuantitySNP_TMMTIN_Last { get; set; }
        public decimal? Quantity { get; set; }
        public long? CategoryMaterialID { get; set; }
        public string? CategoryMaterialName { get; set; }
        public long? CategoryLocationID { get; set; }
        public string? CategoryLocationName { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public long? CategoryLineID { get; set; }
        public string? CategoryLineName { get; set; }
        public string? OriginalEquipmentManufacturer { get; set; }
        public string? CarMaker { get; set; }
        public string? CarType { get; set; }
        public string? Item { get; set; }
        public string? DevelopmentStage { get; set; }        
        public long? CategoryLocationID01 { get; set; }
        public string? CategoryLocationName01 { get; set; }
        public Material()
        {
            Active = true;
        }
    }
}

