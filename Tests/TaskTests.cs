using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{
    class TaskTests
    {
        private BoardService board;
        private TaskService taskService;
        private UserService user;
        public TaskTests(TaskService taskService,UserService user,BoardService boardService)
        {
            this.user = user;
            this.taskService = taskService;
            this.board = boardService;
        }
        public void RunTests()
        {

            user.Register("adamrammal4@gmail.com", "Adam2004");
            user.Register("dan@gmail.com", "Dan2008");
            user.Register("danikesh@gmail.com", "Dan2008");
            board.CreateBoard("danikesh@gmail.com", "mathBoard");
            DateTime date = new DateTime(2023, 5, 20);
            board.AddTask("danikesh@gmail.com", "mathBoard", "math home work", date, "this is for math");

            SetTitle();
            SetDueDate();
            SetDescription();
            InProgressTasks();


        }
        public void SetTitle()
        {
            SetTitleSuccessed();
            SetTitleFialed();
        }
        public void SetTitleSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.SetTitle("danikesh@gmail.com", "mathBoard", 0, 0, "workingBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set title for task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set title successed");
            }
        }
        public void SetTitleFialed()
        {
            Response res2 = JsonSerializer.Deserialize<Response>(taskService.SetTitle("danikesh@gmail.com", "mathBoard", 0, 0, "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("set title for task failed:: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set title successed");
            }
        }


        public void SetDueDate()
        {
            SetDueDateSuccessed();
            SetDueDateFailed();
        }
        public void SetDueDateSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.SetDueDate("danikesh@gmail.com", "mathBoard", 0, 0, new DateTime(2023, 6, 20)));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set due date for task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set due date for task successed");
            }
        }
        public void SetDueDateFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.SetDueDate("danikesh@gmail.com", "mathBoard", 0, 0, new DateTime(1995, 3, 20)));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set due date for task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set due date for task successed");
            }
        }

        public void SetDescription()
        {
            SetDescriptionSuccessed();
            SetDescriptionFailed();
        }
        public void SetDescriptionSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.SetDescription("danikesh@gmail.com", "mathBoard", 0, 0, "this task is about the working stuff"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set description for task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set description for task successed");
            }
        }
        public void SetDescriptionFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.SetDescription("danikesh@gmail.com", "mathBoard", 0, 0, "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzadsds"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set description for task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set description for task successed");
            }
        }

        public void InProgressTasks()
        {
            InProgressTasksSuccessed();
            InProgressTasksFailed();
        }
        public void InProgressTasksSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.InProgressTasks("danikesh@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Get In Progress tasks failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Get In Progress tasks successed");
            }
        }
        public void InProgressTasksFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.InProgressTasks("dan@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Get In Progress tasks failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Get In Progress tasks successed");
            }
        }

        public void AssignTask()
        {
            AssignTaskSeccesed();
            AssignTaskFailed();
        }
        public void AssignTaskSeccesed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.AssignTask("danikesh@gmail.com", "mathBoard", 0, 0, "adamrammal4@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set title for task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set title successed");
            }
        }
        public void AssignTaskFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(taskService.AssignTask("danikesh@gmail.com", "EnglishBoard", 0, 0, "adamrammal4@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set title for task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set title successed");
            }
        }
    }
}