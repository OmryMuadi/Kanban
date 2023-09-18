using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Board
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string name;
        internal List<Column> columns;
        private int taskIdCounter;
        private int id;//added
        private string owner;//added
        internal List<string> emails;//added

        /// <summary>
        /// Constructing a board with limit of column
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="backlog"></param>
        /// <param name="progress"></param>
        /// <param name="done"></param>
        /// 
        public Board(string name, string email, int id, int backlog, int progress, int done)
        {
            this.name = name;
            columns = new List<Column>();
            columns.Add(new Column(0, backlog));
            columns.Add(new Column(1, progress));
            columns.Add(new Column(2, done));
            this.id = id;
            taskIdCounter = 0;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            this.owner = email;
            emails = new List<string>();
        }

        /// <summary>
        /// Constructing a board without a limit of column
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        public Board(string name, string email, int id) 
        {
            this.name = name;
            columns = new List<Column>();
            columns.Add(new Column(0));
            columns.Add(new Column(1));
            columns.Add(new Column(2));
            this.id = id;
            taskIdCounter = 0;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            this.owner = email;
            emails = new List<string>();
        }
        
        /// <summary>
        /// Increasing task id by one
        /// </summary>

        public void AddOne() {
            taskIdCounter++;
        }
        /// <summary>
        /// getting name of a board
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return name; 
        }
        /// <summary>
        /// getting a column with a given status of the board
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Column getColumn(int status) 
        {
            if (!(status == 0 || status == 1 || status == 2))
            {
                throw new ArgumentException("Invalid status");
            }
            return columns[status]; 
        }
        /// <summary>
        /// getting task id
        /// </summary>
        /// <returns></returns>
        public int getTaskIdCounter()
        {
            return taskIdCounter;
        }
        /// <summary>
        /// Changing the status of a task to another status
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Board MoveStatus(int taskId, int status,string email)
        {
            Task task = getTask(taskId);
            if (task.assignee != email)
            {
                throw new Exception("This user is not the assigne of this task");
            }
            if (status != 0 && status != 1)
                throw new ArgumentException("Invalid Status");
            if (columns[status].GetTasks().Contains(task))
            {
                columns[status].DeleteTask(task);
                columns[status + 1].addTask(task);
                return this;
            }
            throw new ArgumentException("Move status didn't work");
        }
        /// <summary>
        /// getting the task object that has a given id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task getTask(int taskId)
        {
            foreach (Task task in columns[0].GetTasks())
            {
                if (task.Id == taskId)
                {
                    return task;
                }
            }

            foreach (Task task in columns[1].GetTasks())
            {
                if (task.Id == taskId)
                {
                    return task;
                }
            }

            foreach (Task task in columns[2].GetTasks())
            {
                if (task.Id == taskId)
                {
                    return task;
                }
            }

            throw new Exception("Task doesnt exist.");
        }
        public void DeleteTasks()
        {
            foreach (Task task in columns[0].GetTasks())
            {
                columns[0].DeleteTask(task);
            }
            foreach (Task task in columns[1].GetTasks())
            {
                columns[1].DeleteTask(task);
            }
            foreach (Task task in columns[2].GetTasks())
            {
                columns[2].DeleteTask(task);
            }
        }
        public string GetOwner()
        {
            return owner;
        }
        public int GetId()
        {
            return id;
        }
        public void SetOwner(string owner)
        {
            this.owner = owner;
        }
        /// <summary>
        /// Giving a limit for a columns of a board
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Board LimitColumn(int columnOrdinal, int limit)
        {
            if (columns[columnOrdinal].GetTasks().Count > limit)
            {
                throw new Exception("Invalid limit");
            }
            else
            {
                columns[columnOrdinal].SetMaxTask(limit);
            }
            return this;
        }
        /// <summary>
        /// Getting a limit of a column
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetColumnLimit(int columnOrdinal)
        {
            if(columnOrdinal == 0 || columnOrdinal == 1 || columnOrdinal == 2)
            {
                return columns[columnOrdinal].GetMaxTask();
            }
            throw new Exception("Invalid column") ;
        }

    }
}
