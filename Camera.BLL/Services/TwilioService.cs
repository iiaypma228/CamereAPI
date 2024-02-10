using Camera.BLL.Interfaces;
using Joint.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Camera.BLL.Services;

public class TwilioService : ITwilioService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    
    public TwilioService(IConfiguration configuration, ILogger<TwilioService> logger)
    {
        this._configuration = configuration;
        this._logger = logger;

        string accountSid = _configuration["Twilio:AccountSid"];
        string authToken = _configuration["Twilio:AuthToken"];
        
        _logger.LogInformation($"Start Init TWILIO => AccountSid:{accountSid}; AuthToken:{authToken}");
        
        TwilioClient.Init(accountSid, authToken);        
    }
    
    
    public Task SendMessage(string body, string to)
    {
        var message = MessageResource.Create(
            body: body,
            from: new Twilio.Types.PhoneNumber(_configuration["Twilio:TwilioPhoneNumber"]),
            to: new Twilio.Types.PhoneNumber(to)
        );
        
        return Task.CompletedTask;
    }

    public void Notify(NotifyToSend notifyToSend)
    {
        this.SendMessage(notifyToSend.Message, notifyToSend.SendAddress);
    }
}