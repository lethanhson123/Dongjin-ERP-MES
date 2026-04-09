namespace MESData.Model
{
    public partial class OQC_NG
    {
        [Key]
        public int ID { get; set; }
        public int Part_IDX { get; set; }
        public string? LotCode { get; set; }
        public long LineListID { get; set; }
        public long NGList_ID { get; set; }
        public string? ECN { get; set; }
        public bool? GP12 { get; set; }
        public string? REMARK { get; set; }
        public string? Picture { get; set; }
        public DateTime? CREATE_DTM { get; set; }
        public string? CREATE_USER { get; set; }
        public DateTime? UPDATE_DTM { get; set; }
        public string? UPDATE_USER { get; set; }
        public OQC_NG()
        {
        }
    }
}
