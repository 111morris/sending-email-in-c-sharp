using MailKit.Net.Smtp;
using MimeKit;

var message = new MimeMessage();
message.From.Add(new MailboxAddress("[Your Name]", "[youraddress@gmail.com]"));
message.To.Add(new MailboxAddress("[Recipient Name]", "[recipient@example.com]"));
message.Subject = "[Testing Email]";
message.Body = new TextPart("plain") { Text = "[Hello, this is a test email!]" };

using var client = new SmtpClient();
await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
await client.AuthenticateAsync("[youraddress@gmail.com]", "[yourpassword]");
await client.SendAsync(message);
await client.DisconnectAsync(true);
