namespace MESService.Model
{
	public partial class tsuserTranfer : tsuser
    {
		public int? AUTH_IDX { get; set; }
		public string? AUTH_NM { get; set; }
		public string? AUTH_ID { get; set; }
        public tsuserTranfer()
		{
		}
	}
}

