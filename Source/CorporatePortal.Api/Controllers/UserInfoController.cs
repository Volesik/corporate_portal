using CorporatePortal.BL.Interfaces;
using CorporatePortal.DL.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace CorporatePortal.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserInfoController(
    IUserInfoService userInfoService,
    IExternalUserDataService externalUserDataService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfosAsync(string? searchTerm, int skip)
    {
        var users = await userInfoService.GetAsync(searchTerm, CancellationToken.None, skip, 20);
        return Ok(users);
    }
    
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetUserInfosCountAsync(string? searchTerm)
    {
        var users = await userInfoService.CountAsync(searchTerm, CancellationToken.None);
        return Ok(users);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserInfo>> GetUserInfosAsync(long id)
    {
        var user = await userInfoService.GetAsync(id, CancellationToken.None);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    
    [HttpGet("getTodayBirthdayUsers")]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetTodayBirthdayUsersAsync()
    {
        var today = DateTime.Today;
        var users = await userInfoService.GetUpcomingBirthdayUsersAsync(CancellationToken.None);
        return Ok(users);
    }
    
    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<UserInfo>>> SearchUserInfo(string? searchTerm)
    {
        var users = await userInfoService.SearchAsync(searchTerm, CancellationToken.None);
        return Ok(users);
    }
    
    [HttpGet("photo/{guid}")]
    public async Task<ActionResult<string>> GetPhotoAsync(string guid)
    {
        var result = await externalUserDataService.SendPhotoRequestAsync(guid);
        await externalUserDataService.SavePhotoAsync(result, guid);
        return Ok();
    }
}