using CorporatePortal.BL.Interfaces;
using CorporatePortal.DL.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace CorporatePortal.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly ILogger<UserInfoController> _logger;
    private readonly IUserInfoService _userInfoService;

    public UserInfoController(
        ILogger<UserInfoController> logger,
        IUserInfoService userInfoService)
    {
        _logger = logger;
        _userInfoService = userInfoService;
    }
    
    [HttpGet(Name = "GetUsers")]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfosAsync()
    {
        var users = await _userInfoService.GetAllAsync();
        return Ok(users);
    }

}