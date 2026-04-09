namespace Data.Model
{
    public partial class ProjectTaskMembership : BaseModel
    {
        public long? ProjectTaskID { get; set; }
        public string? ProjectTaskName { get; set; }
        public long? MembershipID { get; set; }
        public string? MembershipCode { get; set; }
        public string? MembershipName { get; set; }
        public long? CategoryStatusID { get; set; }
        public string? CategoryStatusName { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal? Hour { get; set; }
        public long? CategoryLevelID { get; set; }
        public string? CategoryLevelName { get; set; }

        public ProjectTaskMembership()
        {           
        }
    }
}

