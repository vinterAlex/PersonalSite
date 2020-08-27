using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PersonalSite.Models
{
    public class Email
    {
		public static string Send(string Subject, string Body, bool IsHtml, string AttachmentPath)
		{
			try
			{
				string GmailAccount = "alex.vinter24@gmail.com"; // must change "your_gmail_account@gmail.com"
				string GmailPassword = "unsoaredecangur"; // must change to Gmail account password
				string ToEmails = "alex.vinter24@gmail.com"; // this is the addresses to send the email to; can be the same Gmail account or another email address; separate multiple emails with commas

				MailMessage mail = new MailMessage(GmailAccount, ToEmails);
				mail.Subject = Subject;
				mail.IsBodyHtml = IsHtml;
				mail.Body = Body;
				if (!string.IsNullOrWhiteSpace(AttachmentPath))
				{
					Attachment attachment = new Attachment(AttachmentPath);
					mail.Attachments.Add(attachment);
					mail.Priority = MailPriority.High; // optional
				}

				SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
				smtp.EnableSsl = true;
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new System.Net.NetworkCredential(GmailAccount, GmailPassword);
				smtp.Send(mail);
				return ""; // return nothing when it is successful
			}
			catch (Exception ex)
			{
				return "An error occured while sending your message. " + ex.Message;
			}
		}
	}
}