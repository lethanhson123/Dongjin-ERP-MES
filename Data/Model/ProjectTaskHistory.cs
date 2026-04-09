namespace Data.Model
{
    public partial class ProjectTaskHistory : BaseModel
    {
        public long? ProjectTaskID { get; set; }
        public string? ProjectTaskName { get; set; }
        public long? MembershipID { get; set; }
        public string? MembershipCode { get; set; }
        public string? MembershipName { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal? Hour { get; set; }
        public decimal? HourBegin { get; set; }
        public decimal? HourEnd { get; set; }
        public bool? IsComplete { get; set; }

        public ProjectTaskHistory()
        {
            Active = true;
            IsComplete = false;
            DateBegin = DateTime.Now;
            DateEnd = DateBegin;
            HourBegin = 7;
            HourEnd = 17;
        }
    }
}

