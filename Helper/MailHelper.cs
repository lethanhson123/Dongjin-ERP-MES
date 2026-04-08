namespace Helper
{
	public class MailHelper
	{
		public static void SendMail(Mail mail)
		{
			System.Net.Mail.MailMessage smail = new System.Net.Mail.MailMessage();

			// specifying if our email must be sent in html or text format
			if (mail.IsMailBodyHtml == 1)
			{
				smail.IsBodyHtml = true;
			}
			else
			{
				smail.IsBodyHtml = false;
			}
			// email Body Encoding
			smail.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
			// Now specify your email adress and your/company name
			// you can even set a erroneous adress if you want to send spams for example.
			smail.From = new System.Net.Mail.MailAddress(mail.MailFrom, mail.Display);
			foreach (string toMailAddress in mail.MailTo.Split(','))
			{
				if (!string.IsNullOrEmpty(toMailAddress))
				{
					smail.To.Add(new System.Net.Mail.MailAddress(toMailAddress));
				}
			}
			// mail title/subject            
			smail.Subject = mail.Subject;
			smail.Body = mail.Content;
			smail.Priority = MailPriority.High;
			// MailMessage instance to a specified SMTP server
			System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
			// Setting up the Smtp server
			if (mail.IsMailUsingSSL == 1)
			{
				client.EnableSsl = true;
			}
			else
			{
				client.EnableSsl = false;
			}
            
            client.Host = mail.SMTPServer;
			client.Port = mail.SMTPPort;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			//credential
			//client.UseDefaultCredentials = false;
			System.Net.NetworkCredential theCredential = new System.Net.NetworkCredential(mail.UserName, mail.Password);
			client.Credentials = theCredential;
			// Sending the mail
			try
			{
				client.Send(smail);
			}
			catch (Exception ex)
			{
				string mes = ex.Message;
			}
		}
	}
}
