namespace Data.Model
{
    public partial class CategoryTerm : BaseModel
    {
        public decimal? SQ { get; set; }
        public decimal? Wire { get; set; }
        public decimal? Insulation { get; set; }
        public decimal? CCH1 { get; set; }
        public decimal? CCW1 { get; set; }
        public decimal? ICH1 { get; set; }
        public decimal? ICW1 { get; set; }
        public CategoryTerm()
        {
        }
    }
}

