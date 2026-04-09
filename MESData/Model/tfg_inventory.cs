namespace MESData.Model
{
    [Table("tfg_inventory")]
    public class tfg_inventory
    {
        [Key]
        public long FG_INV_IDX { get; set; }
        public string? PACKING_LOT { get; set; }
        public int PART_IDX { get; set; }
        public string? PART_NO { get; set; }
        public string? PART_NM { get; set; }
        public int SNP_QTY { get; set; }
        public int ACTUAL_QTY { get; set; }
        public int REWORK_QTY { get; set; }
        public DateTime FG_IN_DATE { get; set; }
        public string? FG_IN_USER { get; set; }
        public string? STATUS { get; set; }
        public DateTime? FG_OUT_DATE { get; set; }
        public string? FG_OUT_USER { get; set; }
        public string? SHIPPING_NO { get; set; }
        public DateTime? QC_DATE { get; set; }
        public string? QC_USER { get; set; }
        public string? CUSTOMER_CODE { get; set; }
        public string? REMARK { get; set; }
        public DateTime CREATE_DTM { get; set; }
        public DateTime? UPDATE_DTM { get; set; }
    }
}