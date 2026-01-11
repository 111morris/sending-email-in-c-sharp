---
description: Build and run the C# email sender project
---

# Run Email Project Workflow

## Prerequisites
1. .NET SDK 8.0 must be installed
2. SMTP credentials must be configured (either in appsettings.json or user-secrets)

## Steps

// turbo-all
1. Navigate to project directory
```bash
cd /home/mulandi/Documents/programming_files/projects/c-sharp/sending-email-in-c-sharp
```

2. Restore NuGet packages
```bash
dotnet restore
```

3. Build the project
```bash
dotnet build
```

4. Run the project
```bash
dotnet run
```

## Configuration

### Using User Secrets (Recommended)
```bash
dotnet user-secrets init
dotnet user-secrets set "SmtpOptions:SmtpServer" "smtp.gmail.com"
dotnet user-secrets set "SmtpOptions:SmtpPort" "587"
dotnet user-secrets set "SmtpOptions:SmtpUsername" "your_email@gmail.com"
dotnet user-secrets set "SmtpOptions:SmtpPassword" "your_app_password"
dotnet user-secrets set "SmtpOptions:FromName" "Your Name"
dotnet user-secrets set "SmtpOptions:FromAddress" "your_email@gmail.com"
```

### Expected Success Output
```
info: SmtpEmailSender[0]
      Email sent to recipient@example.com
```
