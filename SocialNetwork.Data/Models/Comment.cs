using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Models
{
    public class Comment 
    {
        [BsonElement("userId")]
        public int UserId { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }
    }
}
