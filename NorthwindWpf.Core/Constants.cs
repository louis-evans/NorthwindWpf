using System.Configuration;

namespace NorthwindWpf.Core
{
    public static class Constants
    {
        public static string ShortDateFormat => ConfigurationManager.AppSettings["ShortDateFormat"];
    }
}
