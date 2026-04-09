namespace Data.Model
{
    public partial class WarehouseOutputMaterial : BaseModel
    {

        public long? WarehouseOutputDetailBarcodeID { get; set; }
        public long? CategoryMaterialID { get; set; }
        public string? CategoryMaterialName { get; set; }       
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public long? CategoryUnitID { get; set; }
        public string? CategoryUnitName { get; set; }
        public decimal? Quantity { get; set; }
        public long? BOMID { get; set; }
        public string? ECN { get; set; }
        public DateTime? BOMDate { get; set; }
        public WarehouseOutputMaterial()
        {
        }
    }
}

