using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Runtime.ExceptionServices;

namespace BackendTests
{
    class BoardTests
    {
        private readonly BoardService boardService;
        UserService user;
        public BoardTests(UserService user)
        {
            this.boardService = new BoardService(user);
            this.user = user;

        }
        public void RunTests()
        {
            user.Register("adamrammal4@gmail.com", "Adam2004");
            user.Register("dan@gmail.com", "Dan2008");
            user.Register("tameer@gmail.com", "Tamy1992");

            boardsNumber();
            createBoard();
            deleteBoard();
            AddTask();
            ChangeTaskStatus();
            GetColumnName();
            GetColumn();
            LimitColumn();
            GetColumnLimit();
            GetBoardsOfUser();
            JoinBoard();
            LeaveBoard();
            GetBoardName();
            TransferOwnership();

        }

        public void boardsNumber()
        {
            boardsNumberSuccessed();
        }
        public void boardsNumberSuccessed()
        {
            //test have no board
            string res = boardService.boardsNumber("dan@gmail.com");
            Response res1 = JsonSerializer.Deserialize<Response>(res);
            if (res1.ErrorOccurd || boardService.boardsNumber("dan@gmail.com")!="0")
            {
                Console.WriteLine("create board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("success The user does not has any board");
            }
        }

        public void createBoard()
        {
            Console.WriteLine("creation board success Tests: ");
            createBoardSuccessed();
            Console.WriteLine("creation board failed Tests: ");
            createBoardFailed();
        }
        public void createBoardSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("adamrammal4@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("create board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board successfully");
            }

            //Response res2 = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("dan@gmail.com", "MathBoard", 4, 4, 4));
            //if (res2.ErrorOccurd)
            //{
            //    Console.WriteLine("create board failed: " + res2.ErrorMessage);
            //}
            //else
            //{
            //    Console.WriteLine("Added board successfully");
            //}
            Response res3 = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("adamrammal4@gmail.com", "Board"));
            if (res3.ErrorOccurd)
            {
                Console.WriteLine("create board failed: " + res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board successfully");
            }
        }

        public void createBoardFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("adamrammal4@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("create board failed:" + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board successfully");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("dan@gmail.com", ""));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("create board failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Added board successfully");
            }
        }

        public void deleteBoard()
        {
            deleteBoardSuccesfully();
            deleteBoardFailed();
        }
        public void deleteBoardSuccesfully()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("adamrammal4@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("delete board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("the board deleted successfully");
            }
        }
        public void deleteBoardFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("adamrammal4@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("delete board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("the board deleted successfully");
            }
        }


        public void AddTask()
        {
            addTaskSuccessfully();
            addTaskFailed();
        }
        public void addTaskSuccessfully()
        {
            DateTime date = new DateTime(2023, 5, 20);
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AddTask("dan@gmail.com", "MathBoard", "math home work", date, ""));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("add task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.AddTask("dan@gmail.com", "MathBoard", "english home work", date, ""));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("add task failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }
            Response res3 = JsonSerializer.Deserialize<Response>(boardService.AddTask("dan@gmail.com", "MathBoard", "hebrew home work", date, ""));
            if (res3.ErrorOccurd)
            {
                Console.WriteLine("add task failed: " + res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }
        }
        public void addTaskFailed()
        {
            DateTime date = new DateTime(2023, 5, 20);
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.AddTask("dan@gmail.com", "MathBoard", "", date, "this homework in math about tringlrs and pitagoras "));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("add task failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }
        }

        public void ChangeTaskStatus()
        {
            ChangeTaskStatusSuccess();
            ChangeTaskStatusFailed();
        }
        public void ChangeTaskStatusSuccess()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.MoveStatus(0, "MathBoard", 0, "dan@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("change task status failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task status has been changed successfully");
            }

            Response res2 = JsonSerializer.Deserialize<Response>(boardService.MoveStatus(0, "MathBoard", 1, "dan@gmail.com"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("change task status failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task status has been changed successfully");
            }



        }
        public void ChangeTaskStatusFailed()
        {
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.MoveStatus(0, "MathBoard", 2, "dan@gmail.com"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("change task status failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task status has been changed successfully");
            }
        }


        public void GetColumnName()
        {
            GetColumnNameSuccessed();
            GetColumnNameFailed();
        }

        public void GetColumnNameSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName(2, "dan@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting column name failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column name got successfully");
            }
        }

        public void GetColumnNameFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnName(5, "dan@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting column name failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column name got successfully");
            }
        }

        public void GetColumnLimit()
        {
            GetColumnLimitSuccessed();
            GetColumnLimitFailed();
        }
        public void GetColumnLimitSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("dan@gmail.com", "MathBoard", 1));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting column limit failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column limit got successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("adamrammal4@gmail.com", "Board", 1));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("getting column limit failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column limit got successfully");
            }
        }
        public void GetColumnLimitFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("dan@gmail.com", "MathBoard", 4));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting column limit failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column limit got successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("dan@gmail.com", "ss", 0));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("getting column limit failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column limit got successfully");
            }
        }
        public void GetColumn()
        {
            GetColumnSuccessed();
            GetColumnFailed();
        }
        public void GetColumnSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumn(2, "dan@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting column failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column got successfully");
            }
        }
        public void GetColumnFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetColumn(5, "dan@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting column failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column got successfully");
            }
        }

        public void LimitColumn()
        {
            LimitColumnSuccessed();
            LimitColumnFaild();
        }
        public void LimitColumnSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LimitColumn("dan@gmail.com", "MathBoard", 1, 10));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set limit for column failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set limit for column successfully");
            }
        }
        public void LimitColumnFaild()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LimitColumn("dan@gmail.com", "MathBoard", 0, 1));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("set limit for column failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("set limit for column successfully");
            }
        }

        public void GetBoardsOfUser()
        {
            GetBoardsOfUserSuccessed();
            GetBoardsOfUserFailed();
        }
        public void GetBoardsOfUserSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetUserBoards("dan@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting the boards of the user failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting the boards of the user successfully");
            }
        }
        public void GetBoardsOfUserFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetUserBoards("tameer@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("getting the boards of the user failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("getting the boards of the user successfully");
            }
        }

        public void JoinBoard()
        {
            JoinBoardSuccessed();
            JoinBoardFailed();
        }
        public void JoinBoardSuccessed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("dan@gmail.com",2));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("join user to the board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("join user to the board successfully");
            }
        }
        public void JoinBoardFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("dan@gmail.com", 1));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("join user to the board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("join user to the board successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("dan@gmail.com", 99));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("join user to the board failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("join user to the board successfully");
            }
        }

        public void LeaveBoard()
        {
            LeaveBoardSuccessful();
            LeaveBoardFailed();
        }
        public void LeaveBoardSuccessful()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("dan@gmail.com", 2));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Leaving user from the board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Leaving user from the the board successfully");
            }
        }
        public void LeaveBoardFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("dan@gmail.com", 2));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Leaving user from the board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Leaving user from the the board successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("dan@gmail.com", 99));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("Leaving user from the board failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Leaving user from the the board successfully");
            }
        }


        public void GetBoardName()
        {
            GetBoardNameSuccessful();
            GetBoardNameFailed();
        }
        public void GetBoardNameSuccessful()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(1));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Getting board name failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Getting board name successfully");
            }
        }
        public void GetBoardNameFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(99));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Getting board name failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Getting board name successfully");
            }
        }

        public void TransferOwnership()
        {
            TransferOwnershipSuccessful();
            TransferOwnershipFailed();
        }
        public void TransferOwnershipSuccessful()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("adamrammal4@gmail.com", "dan@gmail.com", "Board"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("TransferOwnership of the board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("TransferOwnership of the board successfully");
            }
        }
        public void TransferOwnershipFailed()
        {
            Response res1 = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("tameer@gmail.com", "adamrammal4@gmail.com", "MathBoard"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("TransferOwnership of the board failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("TransferOwnership of the board successfully");
            }
        }


    }
}