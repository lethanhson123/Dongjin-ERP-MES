namespace Data.Model
{
    public partial class ProjectTask : BaseModel
    {
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal? Hour { get; set; }
        public DateTime? DateBeginActual { get; set; }
        public DateTime? DateEndActual { get; set; }
        public decimal? HourActual { get; set; }
        public long? ModuleID { get; set; }
        public string? ModuleName { get; set; }
        public long? CategoryDepartmentID { get; set; }
        public string? CategoryDepartmentName { get; set; }
        public long? CategorySystemID { get; set; }
        public string? CategorySystemName { get; set; }
        public long? CategoryTypeID { get; set; }
        public string? CategoryTypeName { get; set; }
        public long? CategoryStatusID { get; set; }
        public string? CategoryStatusName { get; set; }
        public long? CategoryLevelID { get; set; }
        public string? CategoryLevelName { get; set; }
        public long? RequirementID { get; set; }
        public string? Requirement { get; set; }
        public long? DevelopmentID { get; set; }
        public string? Development { get; set; }
        public long? TestingID { get; set; }
        public string? Testing { get; set; }
        public long? TrainingID { get; set; }
        public string? Training { get; set; }
        public long? HandoverID { get; set; }
        public string? Handover { get; set; }
        public bool? IsComplete { get; set; }

        public ProjectTask()
        {
            Active = true;
            IsComplete = false;
            DateBegin = DateTime.Now;
            DateEnd = DateBegin;
        }
    }
}

