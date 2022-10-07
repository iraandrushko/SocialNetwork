using SocialNetwork.Data;
using SocialNetwork.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.ConsoleApp
{
    public class AppContext
    {
        private readonly DatabaseContext dbContext;

        public AppContext(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
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

            var user = dbContext.Users.GetByCredentials(email, password);

            if (user != null) 
            {
                CurrentUser = user;

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
