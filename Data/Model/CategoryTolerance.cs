namespace Data.Model
{
    public partial class CategoryTolerance : BaseModel
    {
        public decimal? Begin { get; set; }
        public decimal? End { get; set; }
        public decimal? CCH1 { get; set; }
        public decimal? CCW1 { get; set; }
        public decimal? ICH1 { get; set; }
        public decimal? ICW1 { get; set; }
        public decimal? CCH2 { get; set; }
        public decimal? CCW2 { get; set; }
        public decimal? ICH2 { get; set; }
        public decimal? ICW2 { get; set; }
        public CategoryTolerance()
        {
        }
    }
}

