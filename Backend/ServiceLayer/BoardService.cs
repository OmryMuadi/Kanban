using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using log4net;
using System.Reflection;
using Task = IntroSE.Kanban.Backend.BusinessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        internal BoardFacade bf;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public BoardService(UserService userService) 
        {
            this.bf = new BoardFacade(userService.uf);
        }
        /// <summary>
        /// Creating a board for the given user but without a limit of columns
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public string CreateBoard(string email,string boardName)
        {
            try
            {
                log.Info("Attempting to create board");
                if (bf.CreateBoard(email, boardName))
                {
                    log.Debug("Board created successfully");

                    Response r = new Response(null, null);
                    return JsonSerializer.Serialize(r);
                    
                }
                else
                {
                    log.Error("Board not created due to error: ");

                    Response r = new Response("Board not created due to error: ", null);
                    string json = JsonSerializer.Serialize(r);
                    return json;
                }

            }
            catch (Exception e)
            {
                log.Error("Board not created due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// Creating a board for the given user with a limit of columns
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="backlog"></param>
        /// <param name="inprogress"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        public string CreateBoard(string email, string boardName, int backlog, int inprogress, int done)
        {
            try
            {
                log.Info("Attempting to create board");
                
                if (bf.CreateBoard(email, boardName,backlog,inprogress,done))
                {
                    log.Debug("Board created successfully");

                    Response r = new Response(null, null);
                    return JsonSerializer.Serialize(r);

                }
                else
                {
                    log.Error("Board not created due to error: ");

                    Response r = new Response("Board not created due to error: ", null);
                    string json = JsonSerializer.Serialize(r);
                    return json;
                }

            }
            catch (Exception e)
            {
                log.Error("Board not created due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

        /// <summary>
        /// Deleting a board named boardName for the given user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public string DeleteBoard(string email,string boardName)
        {
            try
            {
                log.Info("Attempting to delete board");

                bf.DeleteBoard(email, boardName);

                log.Debug("Board deleted successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Board not deleted due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        //Here we used
        /// <summary>
        /// Getting number of boards for a user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string boardsNumber(string email)
        {
            try
            {
                log.Info("Getting number of boards");

                int i = bf.boardsNumber(email);

                log.Debug("number of boards got successfully");

                Response r = new Response(null, i);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Board not deleted due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }

        }

        /// <summary>
        /// Adding a task for board named boardName for a given user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="dueDate"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public string AddTask(string email, string boardName, string title, DateTime dueDate, string description)
        {
            try
            {
                log.Info("Adding task to board");

                bf.AddTask(title, dueDate, description, email, boardName);

                log.Debug("task added successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Task not added to board due to error: " + e.Message);

                Response r = new Response(e.Message , null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// Changing the status of a task to another status
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="boardName"></param>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public string MoveStatus(int taskId, string boardName, int status ,string email)
        {
            try
            {
                log.Info("Changing status of a task");

                bf.MoveStatus(taskId, boardName, status,email);

                log.Debug("Task's status changed successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Task's status not changed due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// Getting a column name
        /// </summary>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public string GetColumnName(int status, string email, string boardName)
        {
            try
            {
                log.Info("Getting name of column");

                string s = bf.GetColumnName(status, email, boardName);

                log.Debug("name of column got successfully");

                Response r = new Response(null, s);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Cannot get name of column due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// Getting a column
        /// </summary>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        public string GetColumn(int status, string email, string boardName)
        {
            string toConvert;
            Response res;
            try
            {
                List<Task> result = bf.GetColumn(status, email, boardName);
                List<TaskJson> taskjsons = new List<TaskJson>();
                foreach (Task task in result)
                {
                    TaskJson taskJson = new TaskJson();
                    taskJson.Id = task.Id;
                    taskJson.CreationTime = task.Creation_time;
                    taskJson.Title = task.Title;
                    taskJson.Description = task.Description;
                    taskJson.DueDate = task.Due_time;
                    taskjsons.Add(taskJson);
                }
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                string json = System.Text.Json.JsonSerializer.Serialize(new Response(null, taskjsons), options);
                return json;
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            toConvert = JsonSerializer.Serialize<Response>(res);
            return toConvert;
        }
        /// <summary>
        /// Giving a limit for a columns of a specific board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                log.Info("Limiting of a column");

                bf.LimitColumn(email, boardName, columnOrdinal, limit);

                log.Debug("Column limited successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Column not limited due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// Getting a limit of a column
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                log.Info("Getting limit of a column");

                int i = bf.GetColumnLimit(email, boardName, columnOrdinal);

                log.Debug("Limit of a column got successfully");

                Response r = new Response(null, i);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Cannot get limit of a column due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

        //Here we added Get Boards of given user
        /// <summary>
        /// Getting all boards of a given user, We used this in milestone 1, But we still use it in milestone 2
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetUserBoards(string email)
        {
            try
            {
                log.Info("Getting number of boards");
                List<object> i2 = bf.GetUserBoards(email);
                log.Debug("number of boards got successfully");
                Response r = new Response(null, i2);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Board not deleted due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
            //return JsonSerializer.Serialize(new Response("function not implemented"));
        }

        
        //added methods for milestone 2 
        /// <summary>
        /// Joing the given user to a board with an given id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                log.Info("joining board");

                bf.JoinBoard(email,boardID);

                log.Debug("joining board successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("joining board failed due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

        /// <summary>
        /// Leaving the given user from the board with given id
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                log.Info("leaving board");

                bf.LeaveBoard(email, boardID);

                log.Debug("leaving board successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("leaving board failed due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            try
            {
                log.Info("Getting board name");

                string boardName = bf.GetBoardName(boardId);

                log.Debug("name of the board got successfully");

                Response r = new Response(null, boardName);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("getting board name failed due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                log.Info("Transferring Ownership");

                bf.TransferOwnership(currentOwnerEmail, newOwnerEmail,boardName);

                log.Debug("Transferring Ownership successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Transferring Ownership failed due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method. 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LoadData()
        {
            try
            {
                log.Info("Attempting to load data from database");

                bf.LoadData();

                log.Debug("loading data from database was successful");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);
            }
            catch (Exception e)
            {
                log.Error("Loading data was unsuccessful due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteData()
        {
            try
            {
                log.Info("Attempting to delete data from project variables (on RAM)");

                bf.DeleteData();

                log.Debug("deleting data was successful");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);
            }
            catch (Exception e)
            {
                log.Error("deleting data from project variables was unsuccessful due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// function that returns a json of board's tasks
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTasks(int id)
        {
            try
            {
                log.Info("Getting number of boards");
                List<Column> i2 = bf.GetTasks(id);
                List<TaskColumnJson> taskjsons = new List<TaskColumnJson>();
                foreach (Column c in i2)
                {
                    foreach (Task task in c.GetTasks())
                    {
                        TaskColumnJson taskJson = new TaskColumnJson();
                        taskJson.Id = task.Id;
                        taskJson.CreationTime = task.Creation_time;
                        taskJson.Title = task.Title;
                        taskJson.Description = task.Description;
                        taskJson.DueDate = task.Due_time;
                        taskJson.ColumnId = c.GetId();
                        taskjsons.Add(taskJson);
                    }
                }
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                string json = System.Text.Json.JsonSerializer.Serialize(new Response(null, taskjsons), options);
                return json;

            }
            catch (Exception e)
            {
                log.Error("Board not deleted due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
            //return JsonSerializer.Serialize(new Response("function not implemented"));
        }
        //public class TaskJson
        //{
        //    public int Id { get; set; }
        //    public DateTime CreationTime { get; set; }
        //    public string Title { get; set; }
        //    public string Description { get; set; }
        //    public DateTime DueDate { get; set; }
        //}
    }
}
