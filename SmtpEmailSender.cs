using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using System.Net.Sockets;
using System.IO;

public interface IEmailSender
{
 Task SendAsync(string toName, string toAddress,
 string subject, string plainText,
 CancellationToken ct = default);
}

public sealed class SmtpEmailSender : IEmailSender, IAsyncDisposable
{
 private readonly SmtpOptions _opts;
 private readonly ILogger<SmtpEmailSender> _log;
 private SmtpClient? _client;   // kept open for reuse if you want

 public SmtpEmailSender(IOptions<SmtpOptions> opts,
 ILogger<SmtpEmailSender> log)
 {
  _opts = opts.Value;
  _log  = log;
 }

 public async Task SendAsync(string toName, string toAddress,
 string subject, string plainText,
 CancellationToken ct = default)
 {
  var msg = new MimeMessage();
  msg.From.Add(new MailboxAddress(_opts.FromName, _opts.FromAddress));
  msg.To  .Add(new MailboxAddress(toName, toAddress));
  msg.Subject = subject;
  msg.Body = new TextPart("plain") { Text = plainText };

  var policy = Policy
   .Handle<SmtpCommandException>()
   .Or<SocketException>()
   .Or<IOException>()
   .WaitAndRetryAsync(
3,
retry => TimeSpan.FromSeconds(Math.Pow(2, retry)),
(ex, ts, _) => _log.LogWarning(ex, "SMTP retry after {Delay}", ts));

 await policy.ExecuteAsync(async token =>
 {
  var client = await GetOrConnectAsync(token);
  await client.SendAsync(msg, token);
  _log.LogInformation("Email sent to {Email}", toAddress);
 }, ct);
}

 private async Task<SmtpClient> GetOrConnectAsync(CancellationToken ct)
 {
  if (_client is { IsConnected: true, IsAuthenticated: true })
   return _client;

  _client = new SmtpClient();
  await _client.ConnectAsync(_opts.SmtpServer, _opts.SmtpPort,
   SecureSocketOptions.StartTls, ct);
  await _client.AuthenticateAsync(_opts.SmtpUsername, _opts.SmtpPassword, ct);
  return _client;
 }

 public async ValueTask DisposeAsync()
 {
  if (_client is { IsConnected: true })
   await _client.DisconnectAsync(true);
  _client?.Dispose();
 }
}