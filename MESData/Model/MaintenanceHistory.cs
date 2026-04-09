namespace MESData.Model
{
    [Table("MaintenanceHistory")]
    public class MaintenanceHistory
    {
        public long ID { get; set; }
        public long ToolShopID { get; set; }
        public string? ToolShopSubCode { get; set; }
        public string? MaintenanceType { get; set; }
        public string? CurrentStatus { get; set; }
        public string? Reason { get; set; }
        public string? Solution { get; set; }
        public string? SparePartsUsed { get; set; }
        public string? MaintenedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DurationMinutes { get; set; }
        public decimal? Cost { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? CreateUser { get; set; }
        public string? UpdateUser { get; set; }
        public bool IsActive { get; set; } = true;
    }
}