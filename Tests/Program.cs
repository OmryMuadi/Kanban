using System;
using IntroSE.Kanban.Backend.ServiceLayer;
using BackendTests;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Configuration;
using System.Text.Json;

namespace BackendTests
{
    class Program
    {
        static void Main(string[] args) 
        {
            GradingService g = new GradingService();
            //g.LoadData();
            g.DeleteData();
            //Console.WriteLine("Welcome!");
            //UserService user = new UserService();
            //new UserTests(user).Runtests();
            //BoardService boardService = new BoardService(user);
            //new BoardTests(user).RunTests();
            //TaskService taskService = new TaskService(boardService, user);
            //new TaskTests(taskService, user, boardService).RunTests();
            g.Register("a@gmail.com", "Omr1234567");
            g.CreateBoard("a@gmail.com", "lala");
            g.AddTask("a@gmail.com", "lala", "baba", "babababa", new DateTime(2025, 2, 3));
            Console.WriteLine(g.GetColumn("a@gmail.com", "lala", 0)); 
            //g.AddTask("a@gmail.com", "lala", "aaaa", "qqq", new DateTime(2024, 4, 2));
            //g.AddTask("a@gmail.com", "lala", "aaa", "qq", new DateTime(2024, 4, 2));

        }
    }
}