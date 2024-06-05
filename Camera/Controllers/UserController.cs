using Camera.BLL.Interfaces;
using Camera.BLL.Services;
using Camera.Localize;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Camera.Controllers;

[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    
    public UserController(IUserService service)
    {
        this._service = service;
    }

    [HttpGet("user")]
    public object GetUser(int? id = null)
    {
        if (id == null)
            return this._service.Read();
        else
            return this._service.Read(id);
    }

    [AllowAnonymous]
    [HttpPost("user")]
    public object CreateUser([FromBody] User user)
    {
        this._service.Save(user);

        return Ok(Resources.userCreated);
    }

    [HttpGet("currentuser")]
    public User GetCurrentUser()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        var currentUser = _service.ReadByEmail(claim.Value);

        return currentUser;
    }
}