namespace CorporatePortal.Common.Constants;

public static class UserDataApiConstants
{
    public const string BaseUrl = $"{SectionName}:BaseUrl";
    public const string Login = $"{SectionName}:Login";
    public const string Password = $"{SectionName}:Password";
    public const string KeyAccount = $"{SectionName}:KeyAccount";
    public const string Type = $"{SectionName}:Type";
    public const string Sign = $"{SectionName}:Sign";
    public const string Request = $"{SectionName}:Request";

    public const string AuthMethod = "Basic";

    public const string PhotoMethodName = "Dyctionary_UploadPhoto";
    public const string UserDataMethodName = "Dyctionary_UploadEmployee";
    public const string UserDataDismissMethodName = "Dyctionary_EmployeeOFF";
    
    public const string DataRootName = "data";

    private const string SectionName = "UserDataApiIntegrationSettings";
}