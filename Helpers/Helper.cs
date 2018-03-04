using System.Net;

namespace Core.Helpers
{
    public static class Extensions
    {
        public static string ToIpv4(this string ip) => ip == "::1" ? "127.0.0.1" : ip;
        public static string ToIpv4(this IPAddress ip) => ToIpv4(ip.ToString());
    }
}