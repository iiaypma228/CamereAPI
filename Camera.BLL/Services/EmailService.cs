using System.Net;
using System.Net.Mail;
using Camera.BLL.Interfaces;
using Joint.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Camera.BLL.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    private readonly SmtpClient _smtpClient;
    
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var smtpDomain = configuration["Email:SmtpDomain"];
        var username = configuration["Email:Username"];
        var password = configuration["Email:Password"];
        
        _logger.LogInformation($"Start INIT SMTP CLIENT AS {smtpDomain}, with NetworkCredential {username} {password}");

        _smtpClient = new SmtpClient(smtpDomain)
        {
            Port = Convert.ToInt32(configuration["Email:Port"]),
            EnableSsl = true,
            UseDefaultCredentials = false
        };
        _smtpClient.Credentials = new NetworkCredential(username, password);
    }
    
    
    public void Notify(NotifyToSend notifyToSend)
    {
        _logger.LogInformation($"SEND MESSAGE TO EMAIL:{notifyToSend.SendAddress}\nMESSAGE:{notifyToSend.Message}");

        var message = new MailMessage(_configuration["Email:EmailAddress"], notifyToSend.SendAddress,
            "OLEG CAMERA SERVICE!", notifyToSend.Message);
        if (!string.IsNullOrEmpty(notifyToSend.PathToFile))
        {
            message.Attachments.Add(new Attachment(notifyToSend.PathToFile));
        }
        
        _smtpClient.Send(message);
    }
}