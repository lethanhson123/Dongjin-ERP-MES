namespace Data.Model
{
    public partial class CategoryLocation : BaseModel
    {
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }
        public long? CategoryLayerID { get; set; }
        public string? CategoryLayerName { get; set; }
        public bool? IsTemporary { get; set; }
        public long? CategoryDepartmentID { get; set; }
        public string? CategoryDepartmentName { get; set; }
        public CategoryLocation()
        {
            Active = true;
        }
    }
}

