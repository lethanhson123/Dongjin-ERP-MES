namespace MESData.Model
{
    [Table("tfg_history")]
    public class tfg_history
    {
        [Key]
        public long FG_HIST_IDX { get; set; }
        public string? PACKING_LOT { get; set; }
        public int PART_IDX { get; set; }
        public string? PART_NO { get; set; }
        public string? PART_NM { get; set; }
        public int SNP_QTY { get; set; }
        public int ACTUAL_QTY { get; set; }
        public int REWORK_QTY { get; set; }
        public string? TRANS_TYPE { get; set; }
        public DateTime TRANS_DATE { get; set; }
        public string? TRANS_USER { get; set; }
        public string? SHIPPING_NO { get; set; }
        public string? DESTINATION { get; set; }
        public string? CUSTOMER_CODE { get; set; }
        public string? REMARK { get; set; }
        public DateTime CREATE_DTM { get; set; }
    }
}