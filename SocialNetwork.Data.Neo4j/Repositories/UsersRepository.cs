using Neo4jClient;
using SocialNetwork.Data.Neo4j.Models;
//using SocialNetworkNeo4J.EntitiesNeo4J;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SocialNetwork.Data.Neo4j.Repositories
{
    public class UsersRepository
    {
        private readonly BoltGraphClient client;

        public UsersRepository(BoltGraphClient client)
        {
            this.client = client;
        }

        public User GetByCredentials(string email, string password)
        {
            var user = client.Cypher
                .Match("(u:User {email: $ue})")
                .WithParam("ue", email)
                .Where("u.password= $password")
                .WithParam("password", password)
                .Return(u => u.As<User>())
                .ResultsAsync.Result;

            return user.ElementAt(0);
        }

        public void GetUsers()
        {
            var users = client.Cypher
                .Match("(u:User)")
                .Return(u => u.As<User>())
                .OrderBy("u.lastName ASC")
                .ResultsAsync.Result;

        }

        public void GetUsersWithFollowers()
        {
            var usersWithFollowers = client.Cypher
                .Match("(u:User)")
                .OptionalMatch("(u)-[r:Follows]->(f)")
                .Return((u, f) => new
                {
                    User = u.As<User>(),
                    Followers = f.CollectAs<User>()
                })
                .OrderBy("u.lastName ASC")
                .ResultsAsync.Result;
        }

        public void CreateUser(User newUser)
        {
            client.Cypher
                .Create("(u:User $newUser)")
                .WithParam("newUser", newUser)
                .ExecuteWithoutResultsAsync().Wait();

        }
        public void DeleteUser(int userId)
        {
            client.Cypher
                .Match("(u:User {id: $deleteUser})")
                .WithParam("deleteUser", userId)
                .DetachDelete("u")
                .ExecuteWithoutResultsAsync().Wait();

        }
        public void CreateRelationshipUserFollows(int userId, int followsId)
        {
            client.Cypher
                .Match("(u:User{id:$uid})", "(f:User{id: $fid})")
                .WithParam("uid", userId)
                .WithParam("fid", followsId)
                .Create("(u)-[:Follows]->(f)")
                .ExecuteWithoutResultsAsync().Wait();
        }


        public void DeleteRelationshipUserFollower(int userId, int followsId)
        {
            client.Cypher
                .Match("(u:User{id: $uid})-[r:Follows]-(f:User{id: $fid})")
                .WithParam("uid", userId)
                .WithParam("fid", followsId)
                .Delete("r")
                .ExecuteWithoutResultsAsync().Wait();
        }

        ////match(p: User { username: 'user1'})-[r]-> (f: User { username:'user5'})return r
        public bool AreFriends(int userId, int searchedUserId)
        {
            var areFriends = client.Cypher
                .Match("(u:User {id: $uid})-[r:Follows]-(s: User {id: $sid})")
                .WithParam("uid", userId)
                .WithParam("sid", searchedUserId)
                .Return((u, s) => new
                {
                    Follow = s.As<User>()
                })
                .ResultsAsync.Result
                .Count() == 2;

            return areFriends;
        }

        public long? ShortestPathToSearthedUser(int userId, int searchedUserId)
        {
            var shortestPath = client.Cypher
                .Match("sp = shortestPath((:User {id: $uid})-[*]-(:User {id: $sid}))")
                .WithParam("uid", userId)
                .WithParam("sid", searchedUserId)
                .Return(sp => sp.Length())
                .ResultsAsync.Result;
            return shortestPath.FirstOrDefault();
        }

    }
}