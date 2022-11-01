using Neo4jClient;
using SocialNetwork.Data.Neo4j.Repositories;

namespace SocialNetwork.Data.Neo4j
{
    public class DatabaseContextNeo4j
    {
        private readonly BoltGraphClient client;

        public DatabaseContextNeo4j(string uri, string userName, string password)
        {
            client = new BoltGraphClient(uri, userName, password);
            client.ConnectAsync().Wait();

            Users = new UsersRepository(client);
        }

        public UsersRepository Users { get; }
    }
}
