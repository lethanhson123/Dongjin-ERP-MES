namespace MESService.Model
{
    public class P04
    {
        public long? ID { get; set; }
        public string? ECNNo { get; set; }
        public string? ProductionLineName { get; set; }
        public string? ProcessName { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Family { get; set; }
        public int? CircuitNumber { get; set; }
        public decimal? TaskTime { get; set; }
        public decimal? CustomerTaskTime { get; set; }
        public decimal? Gap { get; set; }
        public string? Note { get; set; }
        public int? Active { get; set; }          
        public DateTime? CreateDate { get; set; }
        public string? CreateUserName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUserName { get; set; }
    }

}

