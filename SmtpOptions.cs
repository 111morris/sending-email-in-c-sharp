public sealed class SmtpOptions
{
 public const string Section = "SmtpOptions";

 public string SmtpServer { get; set; } = string.Empty;
 public int SmtpPort { get; set; }
 public string SmtpUsername { get; set; } = string.Empty;
 public string SmtpPassword { get; set; } = string.Empty;
 public bool SmtpEnableSsl { get; set; }
 public string FromName { get; set; } = string.Empty;
 public string FromAddress { get; set; } = string.Empty;
}