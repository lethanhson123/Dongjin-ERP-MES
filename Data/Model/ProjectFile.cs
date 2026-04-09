namespace Data.Model
{
    public partial class ProjectFile : BaseModel
    {
        public long? ProjectTaskID { get; set; }
        public string? ProjectTaskName { get; set; }
        public ProjectFile()
        {
        }
    }
}

