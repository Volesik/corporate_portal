using CorporatePortal.DL.EntityFramework.Models;
using CorporatePortal.Workers.Models;

namespace CorporatePortal.Workers.Mappers;

public class UserInfoMapper
{
    public UserInfo ToUserInfo(ImportUserDto importUserDto)
    {
        return new UserInfo
        {
            FullName = importUserDto.FullName,
            UniqueId = importUserDto.Guid,
            City = importUserDto.City,
            Department = importUserDto.Department,
            Email = importUserDto.Email,
            MobilePhone = importUserDto.MobilePhone,
            Organizations = importUserDto.Organization,
            SubDepartment = importUserDto.SubDepartment,
            ChiefFullName = importUserDto.ChiefFullName,
            WorkPhone = importUserDto.WorkPhone,
            Position = importUserDto.Position,
            Birthday = importUserDto.Birthday.HasValue
                ? new DateTimeOffset(importUserDto.Birthday.Value.Date, TimeSpan.Zero)
                : null,
            Room = importUserDto.Room,
            EmploymentDate = importUserDto.EmploymentDate.HasValue
                ? new DateTimeOffset(importUserDto.EmploymentDate.Value.Date, TimeSpan.Zero)
                : null,
            InternalPhone = importUserDto.InternalPhone,
            AlternativeName = importUserDto.AlternativeName,
            IsActive = true
        };
    }
}