namespace Helper.Model
{
	public class Mail
	{
		public string MailFrom { get; set; } = GlobalHelper.InitializationString;
		public string UserName { get; set; } = GlobalHelper.InitializationString;
		public string Password { get; set; } = GlobalHelper.InitializationString;
		public string Display { get; set; } = GlobalHelper.InitializationString;
		public string MailTo { get; set; } = GlobalHelper.InitializationString;
		public string Subject { get; set; } = GlobalHelper.InitializationString;
		public string Content { get; set; } = GlobalHelper.InitializationString;
		public string SMTPServer { get; set; } = GlobalHelper.InitializationString;
		public int SMTPPort { get; set; } = GlobalHelper.InitializationNumber;
		public int IsMailUsingSSL { get; set; } = GlobalHelper.InitializationNumber;
		public int IsMailBodyHtml { get; set; } = GlobalHelper.InitializationNumber;
	}
}
