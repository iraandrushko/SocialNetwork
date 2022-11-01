using SocialNetwork.Data;
using SocialNetwork.Data.Models;
using System;
using SocialNetwork.Data.Neo4j;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.ConsoleApp
{
    public class AppContext
    {
        private readonly DatabaseContext dbContext;
        private readonly DatabaseContextNeo4j dbNeo4jContext;

        public AppContext(DatabaseContext dbContext, DatabaseContextNeo4j dbNeo4jContext)
        {
            this.dbContext = dbContext;
            this.dbNeo4jContext = dbNeo4jContext;
        }

        public User CurrentUser { get; private set; }

        public bool IsAuthenticated() 
        {
            return CurrentUser != null;
        }

        public bool SignIn(string email, string password) 
        {
            if (CurrentUser != null) 
            {
                throw new Exception("User already signed in, sign out first!");
            }

            var userMongo = dbContext.Users.GetByCredentials(email, password);
            var userNeo4j = dbNeo4jContext.Users.GetByCredentials(email, password);
            if (userMongo != null && userNeo4j != null) 
            {
                CurrentUser = userMongo;

                return true;
            }

            return false;
        }

        public void SignOut() 
        {
            CurrentUser = null;
        }
    }
}
