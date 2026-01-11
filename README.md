# C# Email Sender with MailKit

A production-ready C# console application for sending emails using **MailKit** with **dependency injection**, **configuration management**, and **automatic retry logic**.

## ✨ Features

- 🚀 **Modern .NET 8 Application** - Uses latest C# features and .NET hosting model
- 📧 **SMTP Email Sending** - Powered by MailKit for reliable email delivery
- 🔧 **Dependency Injection** - Follows best practices with Microsoft.Extensions.DependencyInjection
- ⚙️ **Flexible Configuration** - Supports `appsettings.json`, environment variables, and user secrets
- 🔄 **Automatic Retry Logic** - Uses Polly for resilient email sending with exponential backoff
- 🔒 **Secure Credential Management** - User secrets support for development
- 📝 **Clean Architecture** - Separation of concerns with interfaces and implementations

## 📋 Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Installing .NET SDK

**Ubuntu/Debian:**
```bash
sudo apt update
sudo apt install dotnet-sdk-8.0
```

**Verify installation:**
```bash
dotnet --version
```

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/111morris/sending-email-in-c-sharp.git
cd sending-email-in-c-sharp
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure SMTP Settings

You have two options for configuration:

#### Option A: User Secrets (Recommended for Development)

```bash
# Initialize user secrets
dotnet user-secrets init

# Set SMTP configuration
dotnet user-secrets set "SmtpOptions:SmtpServer" "smtp.gmail.com"
dotnet user-secrets set "SmtpOptions:SmtpPort" "587"
dotnet user-secrets set "SmtpOptions:SmtpUsername" "your_email@gmail.com"
dotnet user-secrets set "SmtpOptions:SmtpPassword" "your_app_password"
dotnet user-secrets set "SmtpOptions:FromName" "Your Name"
dotnet user-secrets set "SmtpOptions:FromAddress" "your_email@gmail.com"
```

#### Option B: Edit appsettings.json (Not Recommended for Production)

Edit `appsettings.json`:

```json
{
  "SmtpOptions": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your_email@gmail.com",
    "SmtpPassword": "your_app_password",
    "SmtpEnableSsl": true,
    "FromName": "Your Name",
    "FromAddress": "your_email@gmail.com"
  }
}
```

> **⚠️ Important for Gmail Users:**  
> Gmail requires an **App Password**, not your regular account password.  
> 1. Enable [2-Factor Authentication](https://myaccount.google.com/security)
> 2. Generate an [App Password](https://myaccount.google.com/apppasswords)
> 3. Use the generated 16-character password in your configuration

### 4. Update Recipient Information

Edit `Program.cs` (around line 23-27) to set your recipient:

```csharp
await sender.SendAsync(
    toName   : "Recipient Name",
    toAddress: "recipient@example.com",
    subject  : "Testing Email",
    plainText: "Hello, this is a test email!");
```

### 5. Build and Run

```bash
# Build the project
dotnet build

# Run the application
dotnet run
```

## 📁 Project Structure

```
sending-email-in-c-sharp/
├── Program.cs              # Main entry point with DI setup
├── SmtpEmailSender.cs      # IEmailSender interface & implementation
├── SmtpOptions.cs          # SMTP configuration model
├── SimpleExample.cs        # Basic MailKit usage example
├── appsettings.json        # Configuration file
├── sending-email-in-c#.csproj  # Project file
└── README.md               # This file
```

## 🔧 Configuration Options

| Setting | Description | Example |
|---------|-------------|---------|
| `SmtpServer` | SMTP server hostname | `smtp.gmail.com` |
| `SmtpPort` | SMTP server port | `587` |
| `SmtpUsername` | SMTP authentication username | `your_email@gmail.com` |
| `SmtpPassword` | SMTP authentication password | `your_app_password` |
| `SmtpEnableSsl` | Enable SSL/TLS | `true` |
| `FromName` | Sender's display name | `Your Name` |
| `FromAddress` | Sender's email address | `your_email@example.com` |

## 📧 Supported SMTP Providers

### Gmail
```
Server: smtp.gmail.com
Port: 587
SSL/TLS: Yes (StartTls)
Note: Requires App Password
```

### Outlook/Microsoft 365
```
Server: smtp.office365.com
Port: 587
SSL/TLS: Yes (StartTls)
```

### Yahoo Mail
```
Server: smtp.mail.yahoo.com
Port: 587
SSL/TLS: Yes (StartTls)
```

### Custom SMTP Server
Update the `SmtpServer` and `SmtpPort` settings in your configuration.

## 🔍 How It Works

1. **Configuration Loading**: The application loads configuration from `appsettings.json`, environment variables, and user secrets
2. **Dependency Injection**: Services are registered with the DI container
3. **Email Sending**: The `SmtpEmailSender` uses MailKit to connect to the SMTP server
4. **Retry Logic**: Polly automatically retries failed sends (3 attempts with exponential backoff)
5. **Connection Reuse**: SMTP connection is kept open for potential reuse

## 🛠️ Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| MailKit | 4.3.0 | SMTP email sending |
| Microsoft.Extensions.Hosting | 8.0.0 | Application hosting & DI |
| Microsoft.Extensions.Configuration | 8.0.0 | Configuration management |
| Microsoft.Extensions.Configuration.Json | 8.0.0 | JSON configuration provider |
| Microsoft.Extensions.Configuration.EnvironmentVariables | 8.0.0 | Environment variable support |
| Microsoft.Extensions.Configuration.UserSecrets | 8.0.0 | User secrets support |
| Microsoft.Extensions.Options.ConfigurationExtensions | 8.0.0 | Options pattern |
| Polly | 8.2.1 | Retry logic & resilience |

## ⚠️ Troubleshooting

### "535 Authentication failed"
- **Gmail**: Ensure you're using an App Password, not your regular password
- **All providers**: Verify username and password are correct
- Check if "Less secure app access" is required (for some providers)

### "Connection timeout"
- Verify SMTP server hostname and port
- Check firewall settings
- Ensure your network allows outbound connections on the SMTP port

### "Could not authenticate"
- Confirm credentials are correct
- For Gmail, verify 2FA is enabled and you're using an App Password
- Check if the email provider requires additional security settings

### Configuration not loading
- Ensure `appsettings.json` is copied to the output directory
- Verify the configuration section name is `SmtpOptions` (case-sensitive)
- Check user secrets are initialized: `dotnet user-secrets list`

### Build errors
- Ensure .NET SDK 8.0 is installed: `dotnet --version`
- Run `dotnet restore` to restore NuGet packages
- Clean and rebuild: `dotnet clean && dotnet build`

## 📝 Example Output

When successful, you should see:

```
info: SmtpEmailSender[0]
      Email sent to recipient@example.com
```

## 🔐 Security Best Practices

1. **Never commit credentials** to source control
2. **Use user secrets** for development
3. **Use environment variables** or secure vaults for production
4. **Enable 2FA** on your email account
5. **Use App Passwords** instead of account passwords
6. **Rotate credentials** regularly

## 📚 Additional Resources

- [MailKit Documentation](https://github.com/jstedfast/MailKit)
- [.NET Configuration](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)
- [Polly Documentation](https://github.com/App-vNext/Polly)
- [Safe Storage of App Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)

## 📄 License

This project is open source and available under the MIT License.

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!

## 👤 Author

**Morris** - [GitHub](https://github.com/111morris)

---

⭐ If you found this helpful, please give it a star!
