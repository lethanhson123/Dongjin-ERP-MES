namespace Data.Model
{
    public partial class BOMTerm : BaseModel
    {
       
        public decimal? CCH1 { get; set; }
        public decimal? CCW1 { get; set; }
        public decimal? ICH1 { get; set; }
        public decimal? ICW1 { get; set; }
        public decimal? CCH2 { get; set; }
        public decimal? CCW2 { get; set; }
        public decimal? ICH2 { get; set; }
        public decimal? ICW2 { get; set; }
        public BOMTerm()
        {
        }
    }
}

