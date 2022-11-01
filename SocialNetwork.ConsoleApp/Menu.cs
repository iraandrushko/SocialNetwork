using SocialNetwork.Data;
using SocialNetwork.Data.Models;
using SocialNetwork.Data.Neo4j;
using SocialNetwork.Data.Neo4j.Models;
using System;
using System.Collections.Generic;

namespace SocialNetwork.ConsoleApp
{
    public class Menu
    {
        private readonly AppContext appContext;
        private readonly DatabaseContext dbContext;
        private readonly DatabaseContextNeo4j dbNeo4jContext;

        public Menu(AppContext appContext, DatabaseContext dbContext, DatabaseContextNeo4j dbNeo4jContext)
        {
            this.appContext = appContext;
            this.dbContext = dbContext;
            this.dbNeo4jContext = dbNeo4jContext;

            appContext.SignIn("kmeus4@upenn.edu", "aUTdmmmbH");

        }
        public void ShowMenu() 
        {
            if (appContext.IsAuthenticated())
            {
                ShowAuthentaciatedUserMenu();
            }
            else 
            {
                ShowAnonymousUserMenu();
            }
        }

        private void ShowAuthentaciatedUserMenu() 
        {;
            int input = -1;

            do
            {
                Console.WriteLine("\n1. Show My Posts");
                Console.WriteLine("2. Show Latest Posts");
                Console.WriteLine("3. Create Post");
                Console.WriteLine("4. Create Comment");
                Console.WriteLine("5. Delete Account");
                Console.WriteLine("6. Show Users");
                Console.WriteLine("7. Subscribe to a User");
                Console.WriteLine("8. Show Follows");
                Console.WriteLine("9. Show Stream");
                Console.WriteLine("10. Like/Dislike Post");
                Console.WriteLine("11. Show Friends");
                Console.WriteLine("12. UnSubscribe");
                Console.WriteLine("13. Check If Friend");
                Console.WriteLine("14. Get Shortest Path Length");
                Console.WriteLine("15. Check If Follows");
                Console.WriteLine("0. Sign Out");
                Console.Write(">> ");
                input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        ShowAuthenticatedUserPost();                 
                        break;
                    case 2:
                        ShowLatestPosts();
                        break;
                    case 3:
                        CreatePost();
                        break;
                    case 4:
                        CreateComment();
                        break;
                    case 5:
                        DeleteUser();
                        break;
                    case 6:
                        ShowUsers();
                        break;
                    case 7:
                        SubscribeToAUser();
                        break;
                    case 8:
                        ShowFollows();
                        break;
                    case 9:
                        ShowPosts();
                        break;
                    case 10:
                        LikeOrDislikePost();
                        break;
                    case 11:
                        ShowFriends();
                        break;
                    case 12:
                        UnSubscribe();
                        break;
                    case 13:
                        ShowIfFriends();
                        break;
                    case 14:
                        ShowPathLength();
                        break;
                    case 15:
                        ShowIfFollows();
                        break;
                    case 0:
                        SignOut();
                        break;
                }
            } while (input != 0);
        }

        private void ShowIfFollows()
        {
            Console.WriteLine("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            var ifFollows = dbNeo4jContext.Users.IfFollows(appContext.CurrentUser.Id, userId);
            Console.WriteLine("Follows: {0}", ifFollows);
        }

        private void ShowIfFriends()
        {
            Console.WriteLine("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            var areFriends = dbNeo4jContext.Users.AreFriends(appContext.CurrentUser.Id, userId);
            Console.WriteLine("Are Friends: {0}", areFriends);
        }

        private void ShowPathLength()
        {
            Console.WriteLine("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            var sp = dbNeo4jContext.Users.ShortestPathToSearthedUser(appContext.CurrentUser.Id, userId);
            if(sp == 0)
            {
                Console.WriteLine("There Is No Path To This User");
            }
            else
            {
                Console.WriteLine("Shortest Path: {0}", sp);
            }
            
        }

        private void DeleteUser()
        {
            //Console.Write("Enter userId: ");
            //int userId = int.Parse(Console.ReadLine());

            dbContext.Users.Delete(appContext.CurrentUser.Id);
            dbNeo4jContext.Users.DeleteUser(appContext.CurrentUser.Id);
            Console.WriteLine($"User {appContext.CurrentUser.Email} Deleted");
            SignOut();
        }
        private void CreateUser()
        {
            var userMongo = new Data.Models.User();
            var userNeo4j = new Data.Neo4j.Models.User();
            int input = -1;
            Console.Write("FirstName: ");
            var firstName = Console.ReadLine();
            Console.Write("LastName: ");
            var lastName = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Gender: ");
            var gender = Console.ReadLine();
            Console.Write("Age: ");
            var age = int.Parse(Console.ReadLine());
            Console.Write("Password: ");
            var password = Console.ReadLine();
            Console.WriteLine("Interests: ");
            List<string> interests = new List<string>();
            do
            {
                Console.WriteLine("1.Add interest");
                Console.WriteLine("0.Exit");
                Console.Write(">> ");
                input = int.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        Console.Write("Interest: ");
                        var interest = Console.ReadLine();
                        interests.Add(interest);
                        break;
                    case 0:
                        break;
                }
            } while (input != 0);
            List<int> follows = new List<int>();
            List<int> friends = new List<int>();
            userMongo = new Data.Models.User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Gender = gender,
                Age = age,
                Password = password,
                Interests = interests,
                Follows = follows,
                Friends = friends
            };
            dbContext.Users.Add(userMongo);

            userNeo4j = new Data.Neo4j.Models.User
            {
                Id = userMongo.Id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Gender = gender,
                Age = age,
                Password = password
            };
            dbNeo4jContext.Users.CreateUser(userNeo4j);
            Console.WriteLine($"User {userMongo.Email} Created");
        }
        private void SubscribeToAUser()
        {
            Console.WriteLine("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            dbContext.Users.Subscribe(appContext.CurrentUser.Id, userId);
            dbNeo4jContext.Users.CreateRelationshipUserFollows(appContext.CurrentUser.Id, userId);
            Console.WriteLine("Cogratulation!! Valid Subscription");

        }
        private void UnSubscribe()
        {
            Console.WriteLine("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            dbContext.Users.UnSubscribe(appContext.CurrentUser.Id, userId);
            dbNeo4jContext.Users.DeleteRelationshipUserFollower(appContext.CurrentUser.Id, userId);
            Console.WriteLine("Cogratulation!! Valid UnSubscription");
        }
        private void LikeOrDislikePost()
        {
            Console.WriteLine("Post Id: ");
            var postId = Console.ReadLine();
            dbContext.Posts.React(appContext.CurrentUser.Id, postId);
        }
        private void ShowPosts()
        {
            var posts = dbContext.Posts.GetAllUserPosts();
            foreach (var post in posts)
            {
                Console.WriteLine("\n" + post.IdStringValue + "\n" + post.Date.ToShortDateString() + "\n" + post.Body);
                Console.WriteLine("\n" + "Comments:");
                if (post.Comments != null)
                    foreach (var comment in post.Comments)
                    {
                        Console.WriteLine("\t" + comment.UserId + ": " + comment.Body);
                    }
            }
        }
        private void ShowLatestPosts()
        {
            var latestPosts = dbContext.Posts.GetLatest();
            foreach (var post in latestPosts)
            {
                Console.WriteLine("\n" + post.IdStringValue + "\n" + post.Date.ToShortDateString() + "\n" + post.Body);
                Console.WriteLine("\n" + "Comments:");
                if (post.Comments != null)
                    foreach (var comment in post.Comments)
                    {
                        Console.WriteLine("\t" + comment.UserId + ": " + comment.Body);
                    }
            }
        }
        private void ShowAuthenticatedUserPost()
        {
            var authenticatedUserPots = dbContext.Posts.GetUserPosts(appContext.CurrentUser.Id);
            foreach (var post in authenticatedUserPots)
            {
                Console.WriteLine("\n" + post.Date.ToShortDateString() + "\n" + post.Body);
            }
        }
        private void CreateComment()
        {
            Console.WriteLine("Post Id: ");
            var postId = Console.ReadLine();

            Console.WriteLine("Body of the comment: ");
            var body = Console.ReadLine();
            dbContext.Posts.CreateComment(appContext.CurrentUser.Id, postId, body);
            Console.WriteLine("Post created.");

        }
        private void CreatePost()
        {
            Console.WriteLine("Body of the post: ");
            var body = Console.ReadLine();
            dbContext.Posts.Create(appContext.CurrentUser.Id, body);
            Console.WriteLine("Post created.");
        }
        private void ShowUsers()
        {
            var users = dbContext.Users.GetAll();
            foreach (var user in users)
            {
                Console.WriteLine(user.FirstName + " " + user.LastName);
            }
        }
        private void ShowFollows()
        {
            var authenticatedUserFollowers = dbContext.Users.GetUserFollows(appContext.CurrentUser.Id);
            foreach (var follower in authenticatedUserFollowers)
            {
                Console.WriteLine(follower.FirstName + " " + follower.LastName);
            }
        }
        private void ShowFriends()
        {
            var authenticatedUserFollowers = dbContext.Users.GetUserFriends(appContext.CurrentUser.Id);
            foreach (var follower in authenticatedUserFollowers)
            {
                Console.WriteLine(follower.FirstName + " " + follower.LastName);
            }
        }
        private void ShowAnonymousUserMenu()
        {            
            int input = -1;

            do
            {
                Console.WriteLine("1. Sign In");
                Console.WriteLine("2. Create Account");
                Console.WriteLine("0. Exit");

                Console.Write(">> ");
                input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        SignIn();
                        break;
                    case 2:
                        CreateUser();
                        break;
                }

            } while (input != 0);
        }
        private void SignIn() 
        {
            Console.WriteLine("Email: ");
            var email = Console.ReadLine();

            Console.WriteLine("Password: ");
            var password = Console.ReadLine();
            try
            {
                appContext.SignIn(email, password);
            }
            catch (Exception)
            {
                Console.WriteLine("There was an error during authentication, check your credentials and try again");
                ShowAnonymousUserMenu();
            }

            if (appContext.IsAuthenticated())
            {
                Console.WriteLine("Sign in successful");
                Console.WriteLine("Hello, " + appContext.CurrentUser.FirstName);
                ShowAuthentaciatedUserMenu();
            }
            else 
            {
                Console.WriteLine("There was an error during authentication, check your credentials and try again");
                ShowAnonymousUserMenu();
            }
        }
        private void SignOut() 
        {
            appContext.SignOut();
            ShowAnonymousUserMenu();
        }
    }
}
