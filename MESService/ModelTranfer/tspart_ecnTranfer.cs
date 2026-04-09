namespace MESService.Model
{
	public partial class tspart_ecnTranfer : tspart_ecn
    {
        public string? PART_NO { get; set; }
        public string? PART_NAME { get; set; }
        public string? FVWR { get; set; }
        public string? ADDFILE { get; set; }        
        public tspart_ecnTranfer()
		{
		}
	}
}

