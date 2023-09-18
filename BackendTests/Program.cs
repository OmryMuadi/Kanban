using IntroSE.Kanban.Backend.ServiceLayer;
using System;

namespace BackendTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome!");
            UserService user = new UserService();
            new UserTests(user).Runtests();
        }
    }
}
