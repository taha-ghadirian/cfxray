//

using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using CfXray.SpeedTest;
using CfXray.SpeedTest.Enums;

{
    Console.WriteLine("REAL IP...");
    HttpClient httpClient = new();
    var x = await httpClient.GetFromJsonAsync<GetResult>("http://httpbin.org/get");
    Console.WriteLine(x.origin);
}

var sr = new StreamReader("ips.txt");

var baseconfig = await File.ReadAllTextAsync("XrayFiles/Base.json");
string line;
while ((line = await sr.ReadLineAsync()) != null)
{
    await File.WriteAllTextAsync("XrayFiles/config.json",
        baseconfig.Replace("{{{IP}}}", $"{line}"));

    var xrayStartInfo = new ProcessStartInfo(Path.Combine("XrayFiles", "xray-linux-amd64"));
    xrayStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    xrayStartInfo.RedirectStandardOutput = false;
    var xrayProcess = Process.Start(xrayStartInfo);
    xrayProcess.OutputDataReceived += (sender, eventArgs) =>
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.WriteLine(eventArgs.Data);
        Console.BackgroundColor = ConsoleColor.White;
    };
    await Task.Delay(3000);
    Console.WriteLine($"Started \t {line}");

    #region Get ip

    {
        try
        {
            CancellationTokenSource ct = new CancellationTokenSource(8000);
            Console.WriteLine("Testing...");
            HttpClient httpClient = new HttpClient(new HttpClientHandler()
            {
                Proxy = new WebProxy("socks5://127.0.0.1:1080")
            });
            var x = await httpClient.GetFromJsonAsync<GetResult>("https://httpbin.org/get", ct.Token);
            Console.WriteLine(x.origin.ToString());
        }
        catch
        {
            Console.WriteLine("Ip get kir shod!");
            xrayProcess.Close();
            await Task.Delay(200);
            xrayProcess.Dispose();

            Console.WriteLine($"khelas shod! {line}");
            await Task.Delay(5000);
            Console.Clear();
            continue;
        }
    }

    #endregion


    #region Speed test

    CancellationTokenSource cts = new CancellationTokenSource(15000);
    cts.CancelAfter(15000);
    try
    {
        FastClient.Client = new FastHttpClient(new HttpClientHandler()
        {
            Proxy = new WebProxy("socks5://127.0.0.1:1080")
        });
        var test2 = await FastClient.GetDownloadSpeed(SpeedTestUnit.KiloBitsPerSecond, cts.Token);

        Console.WriteLine(Math.Round(test2.Speed) + "\tkb");
    }
    catch
    {
        Console.WriteLine("Speed test kir shod!");
    }

    xrayProcess.Close();
    await Task.Delay(200);
    xrayProcess.Dispose();

    Console.WriteLine($"khelas shod! {line}");
    await Task.Delay(5000);
    Console.Clear();
}

#endregion