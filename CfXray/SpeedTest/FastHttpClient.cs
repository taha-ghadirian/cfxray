using System.Net;
using CfXray.SpeedTest.Enums;
using CfXray.SpeedTest.Helpers;
using CfXray.SpeedTest.Models;
using Newtonsoft.Json;

namespace CfXray.SpeedTest;

public class FastHttpClient : BaseHttpClient
{
    private readonly string Api = "https://api.fast.com/netflix/speedtest/";
    private readonly string Website = "https://fast.com/";
    private readonly string TokenIdentifier = "token:";

    private readonly string KeyHttps = "https";
    private readonly string KeyUrlCount = "urlCount";   
    private readonly string KeyToken = "token";

    private readonly bool ValueHttps = true;
    private readonly int ValueUrlCount = 5;
    
    private static string Token { get; set; }

    public FastHttpClient()
    {
        
    }

    public FastHttpClient(HttpClientHandler handler) : base(handler)
    {
        
    }
    
    internal async Task<DownloadSpeed> GetDownloadSpeed(SpeedTestUnit unit = SpeedTestUnit.KiloBytesPerSecond, CancellationToken? cts = null)
    {
        if (string.IsNullOrEmpty(Token))
        {
            var jsonFilePath = await GetJsonFilePath();
            Token = await GetToken(jsonFilePath);
        }

        var urls = await GetUrls(Token);

        var speed = await GetDownloadSpeed(urls?.Select(x => x.Url.AbsoluteUri), unit, cts);

        return new DownloadSpeed
        {
            Speed = speed.Speed,
            Unit = speed.Unit,
            Source = SpeedTestSource.Fast.ToSourceString()
        };
    }

    private async Task<List<FileUrl>> GetUrls(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var urls = await GetStringAsync($"{Api}?{KeyHttps}={ValueHttps}&{KeyUrlCount}={ValueUrlCount}&{KeyToken}={token}");

            return JsonConvert.DeserializeObject<List<FileUrl>>(urls);
        }
        catch
        {
            return null;
        }
    }

    private async Task<string> GetToken(string jsFilePath)
    {
        try
        {
            if (string.IsNullOrEmpty(jsFilePath))
                return "";

            var javascript = await GetStringAsync(jsFilePath);

            int? index = javascript?.IndexOf(TokenIdentifier);
            if (index == null || index == -1)
                return "";

            javascript = javascript.Substring(index ?? 0);

            index = javascript?.IndexOf(",");
            if (index == null || index == -1)
                return "";

            javascript = javascript.Substring(0, index ?? 0);

            return javascript.Replace("\"", "").Replace(TokenIdentifier, "");
        }
        catch
        {
            return null;
        }
    }

    private async Task<string> GetJsonFilePath()
    {
        try
        {
            var html = await GetStringAsync(Website);

            int? index = html.IndexOf("<script src=");
            if (index == null || index == -1)
                return null;

            html = html.Substring((index ?? 0) + 13);
            var jsFileName = html.Substring(0, html.IndexOf("\""));
            return Website + jsFileName;
        }
        catch
        {
            return null;
        }
    }
}