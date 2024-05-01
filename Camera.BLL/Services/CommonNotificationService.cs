using System.Linq.Expressions;
using Camera.BLL.Interfaces;
using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Constants;
using Joint.Data.Models;
using Microsoft.Extensions.Logging;

namespace Camera.BLL.Services;

public class CommonNotificationService : ICommonNotificationService
{
    private readonly ICameraService _cameraService;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationToSendRepository _notificationToSendRepository;

    private readonly ILogger _logger;
    
    //NOTIFICATION IMPLEMETATION SERVICE
    private readonly IEmailService _emailService; // email
    private readonly ITwilioService _twilioService; //SMS
    
    
    public CommonNotificationService(ICameraService cameraService, 
        INotificationRepository notificationRepository,
        IEmailService emailService,
        ITwilioService twilioService,
        INotificationToSendRepository notificationToSendRepository,
        ILogger<CommonNotificationService> logger)
    {
        this._cameraService = cameraService;
        this._notificationRepository = notificationRepository;
        this._emailService = emailService;
        this._twilioService = twilioService;
        this._notificationToSendRepository = notificationToSendRepository;
        this._logger = logger;
    }
    
    public void Notify(NotifyToSend notifyToSend)
    {
        var notifyes = this._notificationRepository.Read(i => notifyToSend.CameraId == i.CameraId).ToList();
        this.SendNotify(notifyes, notifyToSend);
    }


    public void Dispose()
    {
        _cameraService.Dispose();
        _notificationRepository.Dispose();
    }

    public void Save(Notification item)
    {
        this.Save(new List<Notification>(){item});
    }

    public void Save(IList<Notification> items)
    {
        foreach (var item in items)
        {
            var old = this._notificationRepository.Read(i => i.Id == item.Id).FirstOrDefault();

            if (old == null)
            {
                this._notificationRepository.Create(item);
            }
            else
            {
                this._notificationRepository.Update(item);
            }
        }
        
        this._notificationRepository.Save();
    }

    public IList<Notification> Read()
    {
        return this._notificationRepository.Read(i => true).ToList();
    }

    public IList<Notification> Read(Expression<Func<Notification, bool>> where)
    {
        return this._notificationRepository.Read(where).ToList();
    }

    public Notification Read(object id)
    {
        return this._notificationRepository.Read(i => i.Id == (int)id).FirstOrDefault();
    }

    public void Delete(Notification item)
    {
        this.Delete(new List<Notification>() {item});
    }

    public void Delete(IList<Notification> items)
    {
        foreach (var item in items)
        {
            var old = this._notificationRepository.Read(i => i.Id == item.Id).FirstOrDefault();

            if (old != null)
            {
                this._notificationRepository.Delete(old);
            }
        }
        
        this._notificationRepository.Save();
    }

    private void SendNotify(IList<Notification> notifications, NotifyToSend notifyToSend)
    {
        INotificationService _notificationService;

        foreach (var notify in notifications)
        {
            switch (notify.NotificationType)
            {
                case NotificationType.SMS:
                    _notificationService = _twilioService;
                    break;
                case NotificationType.Email:
                    _notificationService = _emailService;
                    break;
                case NotificationType.Telegram:
                    throw new Exception("TELEGRAM NOTIFY IS DEVELOPED");
                    break;
                default:
                    throw new Exception("Not allowed notification type!");
            }

            string oldSendAddress = notifyToSend.SendAddress;
            
            if (string.IsNullOrEmpty(notifyToSend.SendAddress))
            {
                notifyToSend.SendAddress = notify.Data;
            }

            try
            {
                _notificationService.Notify(notifyToSend);
                notifyToSend.IsSent = true;
            }
            catch (Exception e)
            {
                notifyToSend.IsSent = false;
                _logger.LogError($"ERORR WHEN SEND NOTIFY : {e}");
            }

            notifyToSend.Id = 0;
            this._notificationToSendRepository.Create(notifyToSend);
            this._notificationToSendRepository.Save();
            notifyToSend.SendAddress = oldSendAddress;

        }
    }
    
}