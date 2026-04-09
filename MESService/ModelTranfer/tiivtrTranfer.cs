namespace MESService.Model
{
	public partial class tiivtrTranfer : tiivtr
    {
        public string? PART_NO { get; set; }
        public string? PART_NM { get; set; }
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
        public tiivtrTranfer()
		{
		}
	}
}

