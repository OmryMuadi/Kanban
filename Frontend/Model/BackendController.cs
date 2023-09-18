using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class BackendController
    {
        internal UserService userService { get; set; }
        internal BoardService boardService { get; set; }

        public BackendController()
        {
            this.userService = new UserService();
            this.boardService = new BoardService(userService);
            boardService.LoadData();
        }
        /// <summary>
        /// login function
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public UserModel Login(string username, string password)
        {
            Response user = JsonSerializer.Deserialize<Response>(userService.Login(username, password));
            if (user.ErrorOccurd)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, username);
        }
        /// <summary>
        /// register function
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public UserModel Register(string username, string password)
        {
            Response user = JsonSerializer.Deserialize<Response>(userService.Register(username, password));
            if (user.ErrorOccurd)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, username);
        }
        /// <summary>
        /// logout function
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string Logout(string username)
        {
            Response user = JsonSerializer.Deserialize<Response>(userService.Logout(username));
            if (user.ErrorOccurd)
            {
                throw new Exception(user.ErrorMessage);
            }
            return user.ErrorMessage;
        }
        /// <summary>
        /// function that returns the list of board ids for given user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<object> GetUserBoards(string username)
        {
            Response baords1 = JsonSerializer.Deserialize<Response>(boardService.GetUserBoards(username));
            if (baords1.ErrorOccurd)
            {
                throw new Exception("Getting list of boards failed");
            }
            object bs = baords1.ReturnValue;
            string jsonString = JsonSerializer.Serialize(bs);
            List<object> boardList = JsonSerializer.Deserialize<List<object>>(jsonString);
            return boardList;
        }
        /// <summary>
        /// function that returns the board's tasks by given id
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<TaskColumnJson> GetTasks(int boardId)
        {
            Response tasks = JsonSerializer.Deserialize<Response>(boardService.GetTasks(boardId));
            if (tasks.ErrorOccurd)
            {
                throw new Exception("Getting list of boards failed");
            }
            object bs = tasks.ReturnValue;
            string jsonString = JsonSerializer.Serialize(bs);
            List<TaskColumnJson> taskList = JsonSerializer.Deserialize<List<TaskColumnJson>>(jsonString);
            return taskList;
        }
    }
}
