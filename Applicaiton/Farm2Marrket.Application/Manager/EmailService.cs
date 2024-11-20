using Farm2Marrket.Application.DTOs;
using Farm2Marrket.Application.Sevices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Farm2Marrket.Application.Manager
{
	public class EmailService : IEmailService
	{
		private readonly MailSettings _emailSettings;

		public EmailService(IOptions<MailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string body)
		{
			var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
			{
				Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
				EnableSsl = _emailSettings.EnableSsl,
				Port = _emailSettings.Port
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
				Subject = subject,
				Body = body,
				IsBodyHtml = false // Set to true if sending HTML email
			};

			mailMessage.To.Add(toEmail);

			await smtpClient.SendMailAsync(mailMessage);
		}
	}
}
