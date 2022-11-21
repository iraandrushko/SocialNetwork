using System.Configuration;
using System;

namespace SocialNetwork.Data.Redis
{
    public class Helper
    {
        public static string CnnVal() => ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        public static string CnnValRedis() => "localhost";
    }
}
