using Camera.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Camera.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CameraController : ControllerBase
{
    private readonly ICameraService _service;
    
    public CameraController(ICameraService service)
    {
        this._service = service;
    }

    [HttpGet("camera")]
    public object GetCamera(int? id = null)
    {
        if (id == null)
            return this._service.Read();
        else
            return this._service.Read(id);
    }

    [HttpPost("camera")]
    public object SaveCamera(Joint.Data.Models.Camera camera)
    {
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
