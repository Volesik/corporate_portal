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
    private readonly IUserPhotoService _userPhotoService;

    public UserInfoController(
        ILogger<UserInfoController> logger,
        IUserInfoService userInfoService,
        IUserPhotoService userPhotoService)
    {
        _logger = logger;
        _userInfoService = userInfoService;
        _userPhotoService = userPhotoService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfosAsync(string? searchTerm, int skip)
    {
        var users = await _userInfoService.GetAllAsync(searchTerm, skip, CancellationToken.None);
        return Ok(users);
    }
    
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetUserInfosCountAsync(string? searchTerm)
    {
        var users = await _userInfoService.CountAsync(searchTerm, CancellationToken.None);
        return Ok(users);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserInfo>> GetUserInfosAsync(long id)
    {
        var user = await _userInfoService.GetByIdAsync(id, CancellationToken.None);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    
    [HttpGet("getTodayBirthdayUsers")]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetTodayBirthdayUsersAsync()
    {
        var users = await _userInfoService.GetTodayBirthdayUsersAsync(CancellationToken.None);
        return Ok(users);
    }
    
    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<UserInfo>>> SearchUserInfo(string? searchTerm)
    {
        var users = await _userInfoService.SearchAsync(searchTerm, CancellationToken.None);
        return Ok(users);
    }
    
    [HttpGet("photo/{guid}")]
    public async Task<ActionResult<string>> GetPhotoAsync(string guid)
    {
        var result = await _userPhotoService.SendAsync(guid);
        await _userPhotoService.SavePhotoAsync(result, guid);
        return Ok();
    }
}