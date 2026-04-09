namespace Data.Model
{
    public partial class Project : BaseModel
    {
        public bool? IsComplete { get; set; }
        public long? CategoryDepartmentID { get; set; }
        public string? CategoryDepartmentName { get; set; }        
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal? Hour { get; set; }
        public DateTime? DateBeginActual { get; set; }
        public DateTime? DateEndActual { get; set; }
        public decimal? HourActual { get; set; }       

        public Project()
        {
            Active = true;
            IsComplete = false;            
            DateBegin = DateTime.Now;
            DateEnd = DateBegin;
        }
    }
}

