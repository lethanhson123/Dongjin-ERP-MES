namespace Data.Model
{
    public partial class WarehouseStockDetail : BaseModel
    {
        public long? BOMID { get; set; }
        public string? BOMCode { get; set; }
        public decimal? BOMQuantity { get; set; }
        public decimal? QuantityStock { get; set; }
        public decimal? Quantity { get; set; }
        public WarehouseStockDetail()
        {
        }
    }
}

