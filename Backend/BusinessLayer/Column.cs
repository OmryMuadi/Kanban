using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Column
    {
        private int maxTask;
        private List<Task> tasks;
        private string name;
        private int id;
        public Column(int id) 
        {
            
            tasks = new List<Task>();
            this.id = id;
            maxTask = -1;
            if (id == 0)
            {
                name = "backlog";
            }
            else
            {
                if (id == 1)
                {
                    name = "in progress";
                }
                else
                {
                    if (id == 2)
                    {
                        name = "done";
                    }
                }
            }
        }
        public Column(int id, int limit) 
        {
            tasks = new List<Task>();
            this.id = id;
            if (id == 0)
            {
                name = "backlog";
            }
            else
            {
                if (id == 1)
                {
                    name = "in progress";
                }
                else
                {
                    if (id == 2)
                    {
                        name = "done";
                    }
                }
            }
            maxTask = limit;
        }
        /// <summary>
        ///  add a task to a column
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool addTask(Task task) 
        {
            if (tasks.Count == maxTask) throw new ArgumentException("The size exceeds the limit");
            if(tasks.Contains(task))
            {
                return false;
            }
            tasks.Add(task);
            return true;
        }
        /// <summary>
        /// delete a task from a column
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool DeleteTask(Task task)
        {
            if(tasks.Contains(task))
            {
                tasks.Remove(task);
                
                return true;
            }
            return false;
        }
        public void DeleteTasks()
        {
            if(tasks.Count > 0 )
            {
                tasks.Clear(); 
            }
            else
            {
                throw new Exception("There's no tasks already");
            }
        }
        public string GetName()
        {
            if (name == null)
                throw new Exception("name is null");
            return name;
        }
        public int GetMaxTask()
        {
            return maxTask;
        }
        /// <summary>
        /// Set the Max num of Tasks in column
        /// </summary>
        /// <param name="limit"></param>
        public void SetMaxTask(int limit)
        {
            maxTask = limit;
        }
        public List<Task> GetTasks()
        {
            if (tasks == null)
                throw new ArgumentException("tasks are null");
            return tasks;
        }
        public Task GetTask(int id)
        {
            return tasks[id];
        }
        public int GetId()
        {
            return id;
        }
    }
}
