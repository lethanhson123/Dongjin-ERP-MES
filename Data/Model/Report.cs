namespace Data.Model
{
    public partial class Report : BaseModel
    {
        public DateTime? Date { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }

        public Report()
        {
        }
    }
}

