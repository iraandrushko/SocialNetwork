using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId idmdb { get; set; }

        public string IdStringValue { get { return idmdb.ToString(); } }

        [BsonElement("userId")]
        public int UserId { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("likes")]
        public List<int> Likes { get; set; }

        [BsonElement("comments")]
        public List<Comment> Comments { get; set; }


    }
}
