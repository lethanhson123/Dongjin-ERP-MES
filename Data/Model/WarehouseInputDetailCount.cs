namespace Data.Model
{
    public partial class WarehouseInputDetailCount : BaseModel
    {

        public DateTime? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? Week { get; set; }
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public int? Count { get; set; }     
        public long? BOMID { get; set; }
        public string? ECN { get; set; }
        public DateTime? BOMDate { get; set; }
        public WarehouseInputDetailCount()
        {
        }
    }
}

