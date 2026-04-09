namespace MESService.Model
{
	public partial class tuser_logTranfer : tuser_log
    {      
        public DateTime? MIN { get; set; }
        public DateTime? MAX { get; set; }      
        public tuser_logTranfer()
		{
		}
	}
}
