using MongoDB.Driver;
using SocialNetwork.Data.Models;
using SocialNetwork.Data.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Repositories
{
    public class UsersRepository 
    {

        private readonly IMongoCollection<User> users;

        public UsersRepository(IMongoDatabase db)
        {
            users = db.GetCollection<User>("users");
        }

        public void Add(User newUser)
        {
            users.InsertOne(newUser);
        }

        public List<User> GetAll()
        {
            return users.Find(Builders<User>.Filter.Empty).ToList();
        }

        public IEnumerable<User> Get(UserSearchModel search)
        {
            var filter = Builders<User>.Filter.Empty;

            if (!string.IsNullOrEmpty(search.FirstName))
            {
                filter &= Builders<User>.Filter.Eq(u => u.FirstName, search.FirstName);
            }

            if (!string.IsNullOrEmpty(search.LastName))
            {
                filter &= Builders<User>.Filter.Eq(u => u.LastName, search.LastName);
            }

            return users.Find(filter).ToList();
        }

        public User GetByCredentials(string email, string password) 
        {
            return users.Find(u => u.Email == email && u.Password == password).FirstOrDefault();
        }

        public IEnumerable<User> GetUserFollows(int userId)
        {
            var user = users.Find(Builders<User>.Filter.Eq(u => u.Id, userId)).FirstOrDefault();
            var follows = users.Find(Builders<User>.Filter.In(u => u.Id, user.Follows));
            
            return follows.ToList();
        }

        public void Subscribe(int userId, int potentialFriendId)
        {
            var user = users.Find(Builders<User>.Filter.Eq(u => u.Id, userId)).FirstOrDefault();
            var potentialFriend = users.Find(Builders<User>.Filter.Eq(u => u.Id, potentialFriendId)).FirstOrDefault();
            if (!user.Follows.Contains(potentialFriendId))
            {
                user.Follows.Add(potentialFriendId);
                users.UpdateOne(Builders<User>.Filter.Eq(u => u.Id, userId), Builders<User>.Update.Push(u => u.Follows, potentialFriendId));
            }

            if(potentialFriend.Follows.Contains(userId))
            {
                potentialFriend.Friends.Add(potentialFriendId);
                users.UpdateOne(Builders<User>.Filter.Eq(u => u.Id, potentialFriendId), Builders<User>.Update.Push(u => u.Friends, userId));
                user.Friends.Add(userId);
                users.UpdateOne(Builders<User>.Filter.Eq(u => u.Id, userId), Builders<User>.Update.Push(u => u.Friends, potentialFriendId));
            }
        }

        public void UnSubscribe(int userId, int anotherUId)
        {
            var user = users.Find(Builders<User>.Filter.Eq(u => u.Id, userId)).FirstOrDefault();
            var anotherUser = users.Find(Builders<User>.Filter.Eq(u => u.Id, anotherUId)).FirstOrDefault();
            if (user.Follows.Contains(anotherUId))
            {
                user.Follows.Remove(anotherUId);
                users.UpdateOne(Builders<User>.Filter.Eq(u => u.Id, userId), Builders<User>.Update.Pull(u => u.Follows, anotherUId));
            }

            if (user.Friends.Contains(anotherUId))
            {
                user.Friends.Remove(anotherUId);
                users.UpdateOne(Builders<User>.Filter.Eq(u => u.Id, userId), Builders<User>.Update.Pull(u => u.Friends, anotherUId));
                anotherUser.Friends.Remove(userId);
                users.UpdateOne(Builders<User>.Filter.Eq(u => u.Id, anotherUId), Builders<User>.Update.Pull(u => u.Friends, userId));
            }
        }
        public IEnumerable<User> GetUserFriends(int userId)
        {
            var user = users.Find(Builders<User>.Filter.Eq(u => u.Id, userId)).FirstOrDefault();
            var follows = users.Find(Builders<User>.Filter.In(u => u.Id, user.Friends));

            return follows.ToList();
        }

    }
}
