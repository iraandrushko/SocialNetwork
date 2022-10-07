using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.ConsoleApp
{
    public class Configuration
    {
        private readonly IConfiguration config;

        public Configuration()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile(GetPath("DataConnectionString", "connection-string.json"))
                .Build();
        }

        public string GetConnectionString()
        {
            return config.GetConnectionString("socialNetwork");
        }

        private string GetPath(string prefix, string fileName)
        {
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, prefix, fileName);
        }
    }
}
