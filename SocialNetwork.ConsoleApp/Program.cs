using SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new DatabaseContext(new Configuration().GetConnectionString());
            var appContext = new AppContext(dbContext);

            var menu = new Menu(appContext, dbContext);

            menu.ShowMenu();
        }
    }
}
