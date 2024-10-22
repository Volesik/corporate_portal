using System.Text.Json;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.Workers.Interfaces;
using CorporatePortal.Workers.Models;
using Hangfire;

namespace CorporatePortal.Workers.Workers;

public class UserInfoReadWorker : IWorker
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IUserInfoService _userInfoService;

    public UserInfoReadWorker(
        IBackgroundJobClient backgroundJobClient,
        IUserInfoService userInfoService)
    {
        _backgroundJobClient = backgroundJobClient;
        _userInfoService = userInfoService;
    }
    
    public async Task ExecuteAsync(int bulkSize)
    {
        try
        {
            var currentDirectory = AppContext.BaseDirectory;
            var userInfosJsonFile = Path.Combine(currentDirectory, "test.json");
            var jsonString = await File.ReadAllTextAsync(userInfosJsonFile);
            var importUsers = JsonSerializer.Deserialize<ImportUserDto[]>(jsonString);

            foreach (var importUser in importUsers)
            {
                var userInfo = Map(importUser);
                _backgroundJobClient.Enqueue(() => ProcessAsync(userInfo));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }

        UserInfo Map(ImportUserDto importUser)
        {
            return new UserInfo
            {
                FullName = importUser.FullName,
                UniqueId = importUser.Guid,
                City = importUser.City,
                Department = importUser.Department,
                Email = importUser.Email,
                MobilePhone = importUser.MobilePhone,
                Organizations = importUser.Organization,
                SubDepartment = importUser.SubDepartment,
                ChiefFullName = importUser.ChiefFullName,
                WorkPhone = importUser.WorkPhone,
                Position = importUser.Position,
                Birthday = importUser.Birthday.HasValue
                    ? new DateTimeOffset(importUser.Birthday.Value.Date, TimeSpan.Zero)
                    : null,
                Room = importUser.Room,
                EmploymentDate = importUser.EmploymentDate.HasValue
                    ? new DateTimeOffset(importUser.EmploymentDate.Value.Date, TimeSpan.Zero)
                    : null,
                InternalPhone = importUser.InternalPhone,
                AlternativeName = importUser.AlternativeName
            };
        }
    }

    public async Task ProcessAsync(UserInfo user)
    {
        await _userInfoService.UpsertAsync(user, CancellationToken.None);
    }
}