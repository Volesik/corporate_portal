using System.Text.Json.Serialization;
using CorporatePortal.Common.Converters;

namespace CorporatePortal.Workers.Models;

public class ImportUserDto
{
    [JsonPropertyName("СотрудникССылка")]
    public required string FullName { get; set; }
    
    [JsonPropertyName("Organization")]
    public string? Organization { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("mphone")]
    public string? MobilePhone { get; set; }
    
    [JsonPropertyName("cphone")]
    public string? WorkPhone { get; set; }
    
    [JsonPropertyName("Департамент")]
    public string? Department { get; set; }
    
    [JsonPropertyName("ОтделДепартамента")]
    public string? SubDepartment { get; set; }
    
    [JsonPropertyName("ДолжностьУПР")]
    public string? Position { get; set; }
    
    [JsonPropertyName("ЛинейныйРуководитель")]
    public string? ChiefFullName { get; set; }
    
    [JsonPropertyName("birthday")]
    [JsonConverter(typeof(CustomDateConverter))]
    public DateTime? Birthday { get; set; }
    
    [JsonPropertyName("City")]
    public string? City { get; set; }
    
    [JsonPropertyName("СотрудникID")]
    public required Guid Guid { get; set; }
    
    [JsonPropertyName("Cabinet")]
    public string? Room { get; set; }
    
    [JsonPropertyName("DateOfEmployment")]
    [JsonConverter(typeof(CustomDateConverter))]
    public DateTime? EmploymentDate { get; set; }
    
    [JsonPropertyName("inphone")]
    public string? InternalPhone { get; set; }
    
    [JsonPropertyName("Сотрудник")]
    public string? AlternativeName { get; set; }
}