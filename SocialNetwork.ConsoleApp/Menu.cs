using Microsoft.Extensions.Configuration;
using SocialNetwork.Data.Models;
using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.ConsoleApp
{
    public class Menu
    {
        private readonly AppContext appContext;
        private readonly DatabaseContext dbContext;

        public Menu(AppContext appContext, DatabaseContext dbContext)
        {
            this.appContext = appContext;
            this.dbContext = dbContext;

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
                Console.WriteLine("5. Show Users");
                Console.WriteLine("6. Subscribe to a User");
                Console.WriteLine("7. Show Follows");
                Console.WriteLine("8. Show Stream");
                Console.WriteLine("9. Like/Dislike Post");
                Console.WriteLine("10. Show Friends");
                Console.WriteLine("11. UnSubscribe");
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
                        ShowUsers();
                        break;
                    case 6:
                        SubscribeToAUser();
                        break;
                    case 7:
                        ShowFollowers();
                        break;
                    case 8:
                        ShowPosts();
                        break;
                    case 9:
                        LikeOrDislikePost();
                        break;
                    case 10:
                        ShowFriends();
                        break;
                    case 11:
                        UnSubscribe();
                        break;
                    case 0:
                        SignOut();
                        break;
                }
            } while (input != 0);
        }
        private void SubscribeToAUser()
        {
            Console.WriteLine("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            dbContext.Users.Subscribe(appContext.CurrentUser.Id, userId);
        }

        private void UnSubscribe()
        {
            Console.WriteLine("User Id: ");
            var userId = int.Parse(Console.ReadLine());
            dbContext.Users.UnSubscribe(appContext.CurrentUser.Id, userId);
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
        private void ShowFollowers()
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
                Console.WriteLine("0. Exit");

                Console.Write(">> ");
                input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        SignIn();
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
