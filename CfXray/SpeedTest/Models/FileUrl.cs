using Newtonsoft.Json;

namespace CfXray.SpeedTest.Models;

public partial class FileUrl
{
    [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
    public Uri Url { get; set; }
}