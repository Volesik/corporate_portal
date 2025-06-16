using CorporatePortal.Common.Models;

namespace CorporatePortal.Common.Dtos;

public class SearchParameterDto
{
    public string? SearchTerm { get; set; }
    
    public UserInfoFilter? Filter { get; set; }

    public int Take { get; set; } = 0;

    public int Skip { get; set; } = 0;
}