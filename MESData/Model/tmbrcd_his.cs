namespace MESData.Model
{
    public partial class tmbrcd_his : BaseModel
    {
        [Key]
        public int? BARCD_IDX { get; set; }
        public string? BARCD_ID { get; set; }
        public string? PKG_GRP_IDX { get; set; }
        public string? PKG_GRP { get; set; }
        public int? PKG_QTY { get; set; }
        public int? PKG_OUTQTY { get; set; }
        public string? DSCN_YN { get; set; }
        public DateTime? OUT_DTM { get; set; }
        public int? MTIN_IDX { get; set; }
        public string? BARCD_LOC { get; set; }
        public DateTime? CREATE_DTM { get; set; }
        public string? CREATE_USER { get; set; }
        public DateTime? UPDATE_DTM { get; set; }
        public string? UPDATE_USER { get; set; }
        public string? BBCO { get; set; }
        public int? TMMTIN_DMM_IDX { get; set; }
        public string? Name { get; set; }
        public int? ReasonID { get; set; }
        public string? Reason { get; set; }
        public tmbrcd_his()
        {
        }
    }
}

