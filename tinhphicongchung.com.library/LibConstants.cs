using System.Configuration;

namespace tinhphicongchung.com.library
{
    public class LibConstants
    {
        public static string CommonConstr = ConfigurationManager.AppSettings["CommonConstr"] ?? string.Empty;
    }
}
