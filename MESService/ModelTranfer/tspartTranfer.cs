namespace MESService.Model
{
    public partial class tspartTranfer : tspart
    {
        public bool? CHK { get; set; }
        public int? CODE { get; set; }
        public string? PART_NAME { get; set; }
        public string? BOM_GROUP { get; set; }
        public string? MODEL { get; set; }
        public string? PART_FamilyPC { get; set; }
        public string? Packing_Unit { get; set; }
        public string? Item_TypeK { get; set; }
        public string? Item_TypeE { get; set; }
        public string? Location { get; set; }
        public string? PART_USE { get; set; }
        public string? PART_ENCNO { get; set; }
        public string? LOT_CODE { get; set; }
        public DateTime? PART_ECN_DATE { get; set; }
        public DateTime? Creation_date { get; set; }
        public string? Creation_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public int? CD_IDX { get; set; }
        public int? REWORK_QTY { get; set; }
        public int? IV_IDX { get; set; }
       
        public int? LOC_IDX { get; set; }
        public double? QTY { get; set; }

        public int? MTIN_IDX { get; set; }
        public int? PN_COUNT { get; set; }
        
        public string? UTM { get; set; }
        public string? DESC { get; set; }
        public string? CATALOG { get; set; }
        public string? STAGE { get; set; }
        public decimal? NET_WT { get; set; }
        public decimal? GRS_WT { get; set; }
        public string? PLET_NO { get; set; }
        public string? SHPD_NO { get; set; }
        public DateTime? MTIN_DTM { get; set; }
        public string? BRCD_PRNT { get; set; }
        public string? DSCN_YN { get; set; }
        public string? MTIN_RMK { get; set; }
        public int? SNP_QTY { get; set; }
        public int? SUM_QTY { get; set; }
        public int? SUM_BOX { get; set; }

        public int? BARCD_IDX { get; set; }
        public int? TIME { get; set; }
        public string? BARCD_ID { get; set; }
        public string? PKG_GRP_IDX { get; set; }
        public string? PKG_GRP { get; set; }
        public double? PKG_QTY { get; set; }
        public double? PKG_OUTQTY { get; set; }       
        public DateTime? OUT_DTM { get; set; }        
        public DateTime? DeleteTime { get; set; }        
        public string? BARCD_LOC { get; set; }        
        public string? DeleteBy { get; set; }        
        public string? BBCO { get; set; }
        public string? REMARK { get; set; }
        public string? MIN { get; set; }
        public string? MAX { get; set; }
        public double? Stock { get; set; }
        public string REWORK_YN { get; set; }
        public string? PART_SCN_Name { get; set; }
        public tspartTranfer()
        {
        }
    }
}

