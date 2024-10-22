using CorporatePortal.DL.Abstractions.Models;

namespace CorporatePortal.DL.EntityFramework.Models;

public class UserInfo : BaseEntity
{
    public required string FullName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Organizations { get; set; }
    
    public string? PersonalMobilePhone { get; set; }
    
    public string? MobilePhone { get; set; }
    
    public string? WorkPhone { get; set; }
    
    public string? Department { get; set; }
    
    public string? SubDepartment { get; set; }
    
    public string? ChiefFullName { get; set; }
    
    public DateTimeOffset? Birthday { get; set; }
    
    public string? City { get; set; }
    
    public required Guid UniqueId { get; set; }
    
    public string? Position { get; set; }
    
    public string? Room { get; set; }
    
    public DateTimeOffset? EmploymentDate { get; set; }
    
    public string? AlternativeName { get; set; }
    
    public string? InternalPhone { get; set; }
}