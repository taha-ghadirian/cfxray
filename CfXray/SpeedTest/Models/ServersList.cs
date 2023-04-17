namespace CfXray.SpeedTest.Models;

internal class ServersList
{
    internal List<Server> Servers { get; set; }

    internal ServersList(List<Server> servers)
    {
        Servers = servers;
    }

    internal void CalculateDistances(Coordinate clientCoordinate)
    {
        Servers.ForEach(server => server.Distance = clientCoordinate.GetDistanceTo(server.GeoCoordinate));
    }

    internal readonly int[] IgnoreIds = { 1525, 1716, 1758, 1762, 1816, 1834, 1839, 1840, 1850, 1854, 1859,
        1860, 1861, 1871, 1873, 1875, 1880, 1902, 1913, 3280, 3448, 3695, 3696, 3697, 3698, 3699,
        3725, 3726, 3727, 3728, 3729, 3730, 3731, 3733, 3788, 4140, 4533, 5085, 5086, 5087, 5894,
        6130, 6285, 6397, 6398, 6412, 7326, 7334, 7529, 8591, 9123, 9466, 9816, 10221, 10226, 10556,
        10557, 10558, 10561, 10562, 10563, 10564, 10565, 10566, 10567, 10901, 10923, 11201, 11736, 11737,
        11792, 12688, 12689, 12861, 12862, 12863, 13362, 14209, 14445, 14446, 14448, 14804, 14805, 14806,
        14807, 14808, 14809, 14810, 14811, 14812, 14813, 14814, 14880, 14881, 14882, 14883, 14884, 14908,
        14909, 14910, 14911, 14946, 14981, 14982, 14983, 14984, 14985, 15012, 15030, 15034, 15035, 15036,
        15037, 15079, 15080, 15081, 15115, 15181, 15182, 15262, 15316, 15359, 15668, 15845, 15949, 15950,
        15951, 15952, 15953, 15954, 15955, 15956, 15957, 16030, 16136, 16275, 16340, 949, 5249 };
}