
﻿using log4net.Config;
using log4net;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class TaskFacade
    {
        private BoardFacade bf;
        private UserFacade users;
        private TaskDAO taskDao;

        public TaskFacade(BoardFacade bf,UserFacade uf)
        {
            taskDao = bf.taskDao;
            this.users = uf;
            this.bf = bf;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// Setting a title for a task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <exception cref="Exception"></exception>
        public void SetTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            if(!users.UserExists(email))
            {
                throw new Exception("user" + email + " is not exist");
            }
            if (!users.IsLoggedIn(email))
            {
                throw new Exception("user" + email + " is not loggedIn");
            }
            if (bf.GetBoard(email,boardName) == null)
            {
                throw new ArgumentException("board doesn't exist");
            }
            if (title.Length == 0) throw new Exception("length invalid");
            if (title.Length > 50) throw new Exception("length invalid");
            Board b = null;
            List<Board> boards = bf.GetBoardsOfUser(email);
            foreach (Board board in boards)
            {
                if (boardName.Equals(board.getName()))
                {
                    b = board;
                }
            }
            if (b != null)
            {

                Column column = b.getColumn(columnOrdinal);
                if (column.GetTask(taskId).assignee != email && column.GetName() != "done") 
                    throw new Exception("only assignee can modify undone task");
                column.GetTask(taskId).Title = title;
                if(!taskDao.Update(taskId, "Title", title, b.GetId()))
                {
                    throw new Exception("Title didn't update");
                }
            }
            else
            {
                throw new Exception("Column doesn't exist");
            }
        }
        /// <summary>
        /// assign task to a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="assignee"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void assign(string email, string boardName, int columnOrdinal, int taskId, string assignee)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user" + email + " is not exist");
            }
            if (!users.IsLoggedIn(email))//change to "isBoardMember"
            {
                throw new Exception("user" + email + " is not loggedIn");
            }
            if (bf.GetBoard(email, boardName) == null)
            {
                throw new ArgumentException("board doesn't exist");
            }
            
            Board b = null;
            List<Board> boards = bf.GetBoardsOfUser(email);
            foreach (Board board in boards)
            {
                if (boardName.Equals(board.getName()))
                {
                    b = board;
                }
            }
            if (b != null)
            {
                Column column = b.getColumn(columnOrdinal);
                if (column.GetTask(taskId).assignee != null && column.GetTask(taskId).assignee != email)
                    throw new Exception("only assignee can modify task assignment");
                if (!users.UserExists(assignee))//&& assignee isnt boardmember
                {
                    throw new Exception("user" + assignee + " is not exist");
                }
                column.GetTask(taskId).assignee = assignee;
                if (!taskDao.Update(taskId, "assignee", assignee, b.GetId()))
                {
                    throw new Exception("Assignee didn't update");
                }

            }
            else
            {
                throw new Exception("Column doesn't exist");
            }
        }

        /// <summary>
        /// Setting a description for a task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public void SetDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user" + email + " is not exist");
            }
            if (!users.IsLoggedIn(email))//change to "isBoardMember"
            {
                throw new Exception("user" + email + " is not loggedIn");
            }
            if (bf.GetBoard(email, boardName) == null)
            {
                throw new ArgumentException("board doesn't exist");
            }
            if (description.Length == 0 || (description.Length > 200)) 
                throw new ArgumentException("invalid description");
            Board b = null;
            List<Board> boards = bf.GetBoardsOfUser(email);
            foreach (Board board in boards)
            {
                if (boardName.Equals(board.getName()))
                {
                    b = board;
                }
            }
            if (b != null)
            {
                Column column = b.getColumn(columnOrdinal);
                if (column.GetTask(taskId).assignee != email && column.GetName() != "done") 
                    throw new Exception("only assignee can modify undone task");
                column.GetTask(taskId).Description = description;
                if (!taskDao.Update(taskId, "Description", description, b.GetId()))
                {
                    throw new Exception("Description didn't update");
                }
            }
            else
            {
                throw new Exception("Column doesn't exist");
            }
        }


        /// <summary>
        /// Setting a due date for a task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <param name="due_date"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public string SetDueDate(string email,string boardName,int status, int id, DateTime due_date)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user" + email + " is not exist");
            }
            if (!users.IsLoggedIn(email))//change to "isBoardMember"
            {
                throw new Exception("user" + email + " is not loggedIn");
            }
            if (bf.GetBoard(email, boardName) == null)
            {
                throw new ArgumentException("board doesn't exist");
            }
            Board b = null;
            List<Board> boards = bf.GetBoardsOfUser(email);
            foreach (Board board in boards)
            {
                if (boardName.Equals(board.getName()))
                {
                    b = board;
                }
            }
            if (b != null) 
            {
                Column column = b.getColumn(status);
                if (column.GetTask(id).Creation_time.CompareTo(due_date) > 0) 
                    return "due_date cannot be earlier than Creation_time!";
                if (column.GetTask(id).assignee != email && column.GetName() != "done")
                    throw new Exception("only assignee can modify undone task");
                column.GetTask(id).Due_time = due_date;
                if (!taskDao.Update(id, "DueTime", due_date+"", b.GetId()))
                {
                    throw new Exception("Due Time didn't update");
                }
                return "due_date succsessfully set!";
            }
            throw new Exception("Column doesn't exist");
        }
        /// <summary>
        /// Getting a list of tasks that are in "in progress" column
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Task> InProgressTasks(string email)
        {

            if (!users.UserExists(email))
            {
                throw new Exception("user" + email + " is not exist");
            }
            if (!users.IsLoggedIn(email))//change to "isBoardMember"
            {
                throw new Exception("user" + email + " is not loggedIn");
            }
            List<Task> tasks = new List<Task>();
            List<Board> boards = bf.GetBoardsOfUser(email);
            foreach (Board board in boards)
            {
                foreach (Task task in board.getColumn(1).GetTasks())
                {
                    if (task.assignee == email)
                    {
                        tasks.Add(task);
                    }
                }
            }
            return tasks;
        }

    }
}
