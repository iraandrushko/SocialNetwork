using SocialNetwork.ConsoleApp.Config;
using SocialNetwork.Data;
using SocialNetwork.Data.Neo4j;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Configuration();
            var dbContext = new DatabaseContext(config.GetMongoConnectionString());
            var neo4jConfig = config.GetNeo4JCredentials();
            var neo4jContext = new DatabaseContextNeo4j(neo4jConfig.Uri, neo4jConfig.UserName, neo4jConfig.Password);
            var appContext = new AppContext(dbContext, neo4jContext);

            var menu = new Menu(appContext, dbContext, neo4jContext);

            menu.ShowMenu();
        }
    }
}
