namespace MESData.Model
{
    public class DowntimeRecords
    {
        public long? ID { get; set; }
        public string? Line { get; set; } 
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? Duration { get; set; }
    }
}