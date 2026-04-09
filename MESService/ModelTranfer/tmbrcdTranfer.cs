namespace MESService.Model
{
    public partial class tmbrcdTranfer : tmbrcd
    {
        public bool? CHK { get; set; }
        public string? Result { get; set; }
        public int? PART_IDX { get; set; }
        public string? UTM { get; set; }
        public string? DESC { get; set; }
        public double? QTY { get; set; }
        public decimal? NET_WT { get; set; }
        public decimal? GRS_WT { get; set; }
        public string? PLET_NO { get; set; }
        public string? SHPD_NO { get; set; }
        public DateTime? MTIN_DTM { get; set; }
        public string? BRCD_PRNT { get; set; }
        public string? MTIN_RMK { get; set; }
        public int? SNP_QTY { get; set; }


        public string? PART_NO { get; set; }
        public string? PART_NM { get; set; }
        public string? BOM_GRP { get; set; }
        public string? PART_CAR { get; set; }
        public string? PART_FML { get; set; }
        public int? PART_SNP { get; set; }
        public int? PART_SCN { get; set; }
        public string? PART_LOC { get; set; }
        public string? PART_USENY { get; set; }
        public string? PART_RMK { get; set; }
        public string? PART_SUPL { get; set; }


        public string? NAME { get; set; }
        public string? LOC { get; set; }
        public string? DATE { get; set; }
        public int? STOCK { get; set; }
        public tmbrcdTranfer()
        {
            Result = GlobalHelper.InitializationString;
        }
    }
}

