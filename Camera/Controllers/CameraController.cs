using System.Security.Claims;
using Camera.BLL.Interfaces;
using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Camera.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CameraController : ControllerBase
{
    private readonly ICameraService _service;
    private readonly IUserService _userService;
    private readonly ICameraNotifiesRepository _cameraNotifiesRepository;
    public CameraController(ICameraService service, IUserService userService, ICameraNotifiesRepository cameraNotifiesRepository)
    {
        this._service = service;
        this._userService = userService;
        _cameraNotifiesRepository = cameraNotifiesRepository;
    }

    [HttpGet("camera")]
    public object GetCamera(int? id = null)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var currentUser = _userService.ReadByEmail(claim.Value);
        
        if (id == null)
            return this._service.Read(i => i.UserId == currentUser.Id);
        else
            return this._service.Read(i => i.Id == id && i.UserId == currentUser.Id);
    }

    [HttpPost("camera")]
    public object SaveCamera([FromBody]Joint.Data.Models.Camera camera)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);
        var currentUser = _userService.ReadByEmail(claim.Value);
        camera.UserId = currentUser.Id;
        this._service.Save(camera);

        return Ok();
    }
    
    [HttpPost("linkNotify")]
    public object LinkNotify(int cameraId, int notificationId)
    {
        _cameraNotifiesRepository.Create(new CameraNotifies
        {
            CameraId = cameraId,
            Camera = null,
            NotificationId = notificationId,
            Notification = null
        });
        _cameraNotifiesRepository.Save();
        return Ok();
    }
    
    [HttpDelete("linkNotify")]
    public object UnlinkNotify(int cameraId, int notificationId)
    {
        var res =_cameraNotifiesRepository.Read(i => i.CameraId == cameraId && i.NotificationId == notificationId);
        _cameraNotifiesRepository.Delete(res.FirstOrDefault());
        _cameraNotifiesRepository.Save();
        return Ok();
    }
    

    [HttpDelete("camera")]
    public object DeleteCamera(int id)
    {
        this._service.Delete(new Joint.Data.Models.Camera() {Id = id});

        return Ok();
    }
    
    
}
