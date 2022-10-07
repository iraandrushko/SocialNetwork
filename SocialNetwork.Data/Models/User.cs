using System;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Data.Models
{
    
    public class User
    {
        [BsonId]
        public ObjectId idmdb { get; set; }
        
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("interests")]
        public List<string> Interests { get; set; }

        [BsonElement("friends")]
        public List<int> Friends { get; set; }

        [BsonElement("follows")]
        public List<int> Follows { get; set; }


    }
}
