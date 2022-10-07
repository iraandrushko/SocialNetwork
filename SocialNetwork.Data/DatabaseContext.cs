using MongoDB.Driver;
using SocialNetwork.Data.Repositories;

namespace SocialNetwork.Data
{
    public class DatabaseContext
    {
        private readonly MongoClient client;
        private readonly IMongoDatabase database;

        public DatabaseContext(string connectionString)
        {
            client = new MongoClient(connectionString);
            database = client.GetDatabase("socialNetwork");
            
            Users = new UsersRepository(database);
            Posts = new PostsRepository(database);
        }

        public UsersRepository Users { get; }
        public PostsRepository Posts { get; }
    }
}
