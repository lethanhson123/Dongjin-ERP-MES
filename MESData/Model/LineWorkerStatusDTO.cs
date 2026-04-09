namespace MESData.Model
{
    public class LineWorkerStatusDTO
    {
        public string? LineCode { get; set; }
        public string? LineName { get; set; }
        public int RequiredFAWorkers { get; set; }
        public int AssignedFAWorkers { get; set; }
        public bool IsFullyAssigned => AssignedFAWorkers >= RequiredFAWorkers;
    }
}

