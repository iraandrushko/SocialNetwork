using SocialNetwork.Data.Models;
using SocialNetwork.Data.Repositories;
using SocialNetwork.Data.Neo4j.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Redis
{
    public class Hashing
    {
        public static void AddComment(Comment comment)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            db.InsertRecord("comments", comment);
        }

        public static void AddFriend(User user, User friend)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            Neo4j.Repositories.UsersRepository ndb = new UsersRepository();
            if (db.GetAllRecords<User>("users")
                    .FirstOrDefault(u => u.id == user.id && u.friends_id.All(el => el != friend.id)) is not null)
            {
                db.CreateFriend(user.id, friend.id);
                ndb.CreateFriend(user.id, friend.id);
            }
        }

        public static void AddLike(Post post, User user)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            if (db.GetAllRecords<Post>("posts")
                    .FirstOrDefault(p => p.id == post.id && p.liked_by.All(u_id => u_id != user.id)) is not null)
                db.AddLikeToPost(post.id, user.id);
        }

        public static void AddLike(Comment comment, User user)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            if (db.GetAllRecords<Comment>("comments")
                    .FirstOrDefault(c => c.id == comment.id && c.liked_by.All(u_id => u_id != user.id)) is not null)
                db.AddLikeToComment(comment.id, user.id);
        }

        public static void AddPost(Post post)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            db.InsertRecord("posts", post);
        }

        public static bool CheckPassword(string login, string password)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            var res = db.GetAllRecords<UserModel>("users").Where(u => u.login == login && u.password == password)
                .FirstOrDefault();
            return res != null;
        }

        public static List<CommentModel> GetAllComments()
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            var res = db.GetAllRecords<CommentModel>("comments");
            return res;
        }

        public static List<PostModel> GetAllPosts()
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            var res = db.GetAllRecords<PostModel>("posts");
            return res;
        }

        public static List<UserModel> GetAllUsers()
        {
            string recordKey = $"Users_" + DateTime.Now.ToString("yyyyMMdd_hh");
            var res = RecordRepository.GetRecord<List<User>>(recordKey);
            if (res == default(List<User>))
            {
                Repositories.UsersRepository db = new(Helper.CnnVal());
                var mongoData = db.GetAllRecords<User>("users");
                RecordRepository.SetRecord(recordKey, mongoData);
                return mongoData;
            }
            return res;

          
        }

        public static User GetUserByID(Guid id)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            var res = db.GetAllRecords<User>("users").Where(u => u.id == id).FirstOrDefault();
            return res;
        }

        public static User GetUserByLogin(string login)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            var res = db.GetAllRecords<User>("users").Where(u => u.login == login).FirstOrDefault();
            return res;
        }

        public static void RemoveFriend(User user, User friend)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            Neo4j.Repositories.UsersRepository ndb = new UsersRepository();
            if (db.GetAllRecords<User>("users")
                    .FirstOrDefault(u => u.id == user.id && u.friends_id.Any(el => el == friend.id)) is not null)
            {
                db.RemoveFriend(user.id, friend.id);
                ndb.RemoveFriend(user.id, friend.id);
            }

        }

        public static void RemoveLike(PostModel post, UserModel user)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            if (db.GetAllRecords<Post>("posts")
                    .FirstOrDefault(p => p.id == post.id && p.liked_by.Any(u_id => u_id == user.id)) is not null)
                db.RemoveLikeFromPost(post.id, user.id);
        }

        public static void RemoveLike(Comment comment, User user)
        {
            Repositories.UsersRepository db = new(Helper.CnnVal());
            if (db.GetAllRecords<Comment>("comments")
                    .FirstOrDefault(c => c.id == comment.id && c.liked_by.Any(u_id => u_id == user.id)) is not null)
                db.RemoveLikeFromComment(comment.id, user.id);
        }

        public static int GetDistanceToUser(Guid userFromId, Guid userToId)
        {
            Neo4j.Repositories.UsersRepository ndb = new UsersRepository();
            return ndb.GetDistance(userFromId, userToId);
        }
    }
}
