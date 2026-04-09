namespace MESService.Model
{
	public partial class tsmenuTranfer : tsmenu
    {
        public int? ParentID { get; set; }
        public int? GroupCode { get; set; }
        public bool? Visible { get; set; }
        public string? Code { get; set; }
        public tsmenuTranfer()
		{
		}
	}
}

