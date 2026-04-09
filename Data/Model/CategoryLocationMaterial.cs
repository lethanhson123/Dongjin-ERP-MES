namespace Data.Model
{
    public partial class CategoryLocationMaterial : BaseModel
    {
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public int? Count { get; set; }
        public CategoryLocationMaterial()
        {
        }
    }
}

