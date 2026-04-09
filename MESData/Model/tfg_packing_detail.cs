namespace MESData.Model
{
    [Table("tfg_packing_detail")]
    public class tfg_packing_detail
    {
        [Key]
        public long FG_DETAIL_IDX { get; set; }
        public string? PACKING_LOT { get; set; }
        public string? LOT_CODE { get; set; }
        public int PART_IDX { get; set; }
        public string? PART_NO { get; set; }
        public string? ECN_NO { get; set; }
        public string? REWORK_YN { get; set; }
        public DateTime? QC_SCAN_DATE { get; set; }
        public string? QC_USER { get; set; }
        public DateTime FG_IN_DATE { get; set; }
        public string? FG_IN_USER { get; set; }
        public DateTime CREATE_DTM { get; set; }
    }
}