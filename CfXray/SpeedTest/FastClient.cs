using CfXray.SpeedTest.Enums;
using CfXray.SpeedTest.Models;

namespace CfXray.SpeedTest;

public static class FastClient
{
    public static FastHttpClient Client { get; set; } = new();

    /// <summary>
    /// Calculates download speed using the provided server
    /// </summary>
    /// <param name="unit">Specifies in which unit download speed should be returned</param>
    /// <returns>An instance of type DownloadSpeed</returns>
    public static async Task<DownloadSpeed> GetDownloadSpeed(SpeedTestUnit unit = SpeedTestUnit.KiloBytesPerSecond, CancellationToken? cts = null) => await Client?.GetDownloadSpeed(unit, cts);
}