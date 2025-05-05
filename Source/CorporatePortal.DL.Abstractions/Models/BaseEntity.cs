using CorporatePortal.DL.Abstractions.Interfaces;

namespace CorporatePortal.DL.Abstractions.Models;

public class BaseEntity : IBaseEntity
{
    public long Id { get; set; }
    
    public DateTimeOffset CreatedWhen { get; set; }
    
    public DateTimeOffset UpdatedWhen { get; set; }
}