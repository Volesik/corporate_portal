namespace CorporatePortal.Common.Models;

public class SearchParameter
{
    public string? SearchTerm { get; set; }
    
    public string? Filter { get; set; }

    public int Take { get; set; } = 20;

    public int Skip { get; set; } = 0;
}