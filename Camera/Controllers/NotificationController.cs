using System.Net;
using System.Security.Claims;
using Camera.BLL.Interfaces;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Camera.Controllers;

[Authorize]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly ICommonNotificationService _commonNotificationService;
    
    public NotificationController(IUserService userService, ICommonNotificationService commonNotificationService)
    {
        this._userService = userService;
        this._commonNotificationService = commonNotificationService;
    }

    [HttpPost("notify")]
    public object SaveNotify(Notification notification)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);

        var currentUser = _userService.ReadByEmail(claim.Value);

        notification.UserId = currentUser.Id;
        notification.User = currentUser;
        
        this._commonNotificationService.Save(notification);

        return Ok();
    }

    [HttpGet("notify")]
    public object GetNotify()
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);

        var currentUser = _userService.ReadByEmail(claim.Value);

        return this._commonNotificationService.Read(i => i.UserId == currentUser.Id);

    }
    
    
    [HttpPost("send")]
    public object SendNotify([FromForm]NotifyToSend notify, IFormFile file)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);

        var currentUser = _userService.ReadByEmail(claim.Value);
        
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        // Получение времени в секундах с начала эпохи Unix
        TimeSpan timeSpan = notify.Date - epoch;
        long unixTime = (long)timeSpan.TotalSeconds;
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), currentUser.Email, notify.CameraId.ToString(), $"{unixTime}.jpg" );
        
        using var stream = System.IO.File.OpenWrite(path);
        file.CopyTo(stream);

        notify.PathToFile = path;
        if (notify.IsNeedToNotify)
        {
            this._commonNotificationService.Notify(notify);
        }
        
        return Ok();
    }
}