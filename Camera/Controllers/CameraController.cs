using System.Security.Claims;
using Camera.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Camera.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CameraController : ControllerBase
{
    private readonly ICameraService _service;
    private readonly IUserService _userService;
    public CameraController(ICameraService service, IUserService userService)
    {
        this._service = service;
        this._userService = userService;
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

    [HttpDelete("camera")]
    public object DeleteCamera(int id)
    {
        this._service.Delete(new Joint.Data.Models.Camera() {Id = id});

        return Ok();
    }
    
    
}
