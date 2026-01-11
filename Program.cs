using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration

       .AddJsonFile("appsettings.json", optional: false)
       .AddEnvironmentVariables()
       .AddUserSecrets<Program>();   // dotnet user-secrets init

builder.Services.Configure<SmtpOptions>(
        builder.Configuration.GetSection(SmtpOptions.Section));
builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
builder.Services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Information));

using var host = builder.Build();

var sender = host.Services.GetRequiredService<IEmailSender>();

await sender.SendAsync(
        toName   : "Recipient Name",
        toAddress: "recipient@example.com",
        subject  : "Testing Email",
        plainText: "Hello, this is a test email!");