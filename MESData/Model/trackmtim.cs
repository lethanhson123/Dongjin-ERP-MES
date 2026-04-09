namespace MESData.Model
{
    public partial class trackmtim : BaseModel
    {
        [Key]
        public int? TRACK_IDX { get; set; }
        public int? RACK_IDX { get; set; }
        public string? RACKCODE { get; set; }
        public int? TABLE_IDX { get; set; }
        public string? TABLE_NM { get; set; }
        public string? LEAD_NM { get; set; }
        public string? BARCODE_NM { get; set; }
        public DateTime? RACKDTM { get; set; }
        public DateTime? RACKOUT_DTM { get; set; }
        public double? QTY { get; set; }
        public string? RACKIN_YN { get; set; }
        public string? RACKOUT_YN { get; set; }
        public DateTime? CREATE_DTM { get; set; }
        public string? CREATE_USER { get; set; }
        public string? FinishGoodsCode { get; set; }
        public string? FinishGoodsList { get; set; }
        public string? POCode { get; set; }
        public string? ECN { get; set; }
        public long? MaterialID { get; set; }
        public long? POID { get; set; }
        public long? BOMID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserID { get; set; }
        public string? UpdateUserCode { get; set; }
        public string? UpdateUserName { get; set; }
        public long? CompanyID { get; set; }
        public long? CategoryDepartmentID { get; set; }
        public string? CategoryDepartmentName { get; set; }
        
        public trackmtim()
        {
        }
    }
}

