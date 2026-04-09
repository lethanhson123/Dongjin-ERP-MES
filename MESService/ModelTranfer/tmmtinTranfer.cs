namespace MESService.Model
{
	public partial class tmmtinTranfer : tmmtin
    {
        public bool? CHK { get; set; }
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

        
        public string? PART_NAME { get; set; }        
        public string? PKG_QTY { get; set; }
        public string? PKG_OUTQTY { get; set; }
        public string? Pallet_NO { get; set; }
        public string? Shipping_NO { get; set; }
        public string? Receipt_Data { get; set; }
        public string? Receipt_USER { get; set; }
        public string? Release_Date { get; set; }
        public string? Release_USER { get; set; }
        public string? BR_DSCN { get; set; }
        public string? PKG_GRP { get; set; }        
        public string? BARCD_IDX { get; set; }
        public string? BARCD_ID { get; set; }
        public string? LOC { get; set; }
        public string? OR_DT { get; set; }

        public string? NO { get; set; }
        public string? DateRDCE { get; set; }
        public string? TYPE { get; set; }
        public string? SUM_QTY { get; set; }
        public string? STOCK_QTY { get; set; }
        public string? DATE { get; set; }       
        public string? USER { get; set; }

      
        public string? MT_EXCEL { get; set; }
        public string? YEAR { get; set; }
        public string? MONTH { get; set; }
        public string? WEEK { get; set; }
        public string? P_COUNT { get; set; }
        public string? BC_COUNT { get; set; }
        public string? IN_QTY { get; set; }
        public string? OUT_QTY { get; set; }
        public string? PO_PLAN { get; set; }
        public string? STAY_QTY { get; set; }
        public string? STAY_RATIO { get; set; }

        public string? BC_DSCN_YN { get; set; }
        public string? MAX_PKG { get; set; }

        public tmmtinTranfer()
		{
		}
	}
}

