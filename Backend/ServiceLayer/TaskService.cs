
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using static IntroSE.Kanban.Backend.ServiceLayer.BoardService;
using Task = IntroSE.Kanban.Backend.BusinessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private TaskFacade tf;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TaskService(BoardService boardService,UserService userService) 
        {
            this.tf = new TaskFacade(boardService.bf,userService.uf);
        }
        /// <summary>
        /// Setting a title for a task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public string SetTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                log.Info("Attempting to create board");

                tf.SetTitle( email,  boardName,  columnOrdinal,  taskId,  title);

                log.Debug("Board created successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

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
        /// Setting a due date for a task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public string SetDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                log.Info("Attempting to create board");

                tf.SetDueDate( email,  boardName,  columnOrdinal,  taskId,  dueDate);

                log.Debug("Board created successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

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
        /// Setting a description for a task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public string SetDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                log.Info("Attempting to Set Description");

                tf.SetDescription( email,  boardName,  columnOrdinal,  taskId,  description);

                log.Debug("Description Set successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Description not Set due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// Getting a list of tasks that are in "in progress" column
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string InProgressTasks(string email)
        {
            string toConvert;
            Response res;
            try
            {
                List<Task> result = tf.InProgressTasks(email);
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
        //added methods for milestone 2
        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                log.Info("Attempting to Set Assignee");

                tf.assign(email, boardName, columnOrdinal, taskID, emailAssignee);

                log.Debug("Assignee Set successfully");

                Response r = new Response(null, null);
                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Assignee not Set due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }

    }
   
}

