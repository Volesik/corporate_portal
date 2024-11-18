using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml;
using CorporatePortal.BL.Interfaces;

namespace CorporatePortal.BL.Services;

public class Downloader : IDownloader
{
    public async Task DownloadUserInfoDataAsync()
    {
        var soapRequest = @"
            <x:Envelope xmlns:x='http://schemas.xmlsoap.org/soap/envelope/' xmlns:bit='http://bayadera.ua/bitrix24'>
                <x:Header/>
                <x:Body>
                    <bit:Upload>
                        <bit:Type>JSON</bit:Type>
                    </bit:Upload>
                </x:Body>
            </x:Envelope>";
        
        var url = "https://1c.corp.bayadera.ua/mis/ws/bitrix24.1cws";
        
        var jsonResponse = await SendSoapRequestAsync(url, soapRequest);
        var jsonData = ExtractJsonFromSoapResponse(jsonResponse);

        var currentDirectory = AppContext.BaseDirectory;
        var userInfosJsonFile = Path.Combine(currentDirectory, "test.json");
        SaveJsonToFile(jsonData, userInfosJsonFile);
    }
    
    private async Task<string> SendSoapRequestAsync(string url, string request)
    {
        var username = "Bitrix24";
        var password = "QwertY2016";
        var base64Credentials = System.Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
        
        var content = new StringContent(request, Encoding.UTF8, "text/xml");
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }
    
    private static string ExtractJsonFromSoapResponse(string soapResponse)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(soapResponse);
    
        var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
        nsmgr.AddNamespace("m", "http://bayadera.ua/bitrix24");
    
        var jsonNode = xmlDoc.SelectSingleNode("//m:return", nsmgr);
        if (jsonNode != null)
        {
            return jsonNode.InnerText.Trim();
        }
    
        throw new Exception("JSON data not found in the SOAP response.");
    }
    
    private static void SaveJsonToFile(string jsonData, string filePath)
    {
        using var doc = JsonDocument.Parse(jsonData);
        
        // Save to file
        File.WriteAllText(filePath, jsonData);
        Console.WriteLine($"JSON data saved to {filePath}");
    }
}