
﻿using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

 namespace IntroSE.Kanban.Backend.BusinessLayer
 {

     //delete
     public class Task
     {

         //fields
         public int Id { get; set; }
         public DateTime Creation_time { get; set; }
         public string Title { get; set; }
         public string Description { get; set; }
         public DateTime Due_time { get; set; }
         public string assignee { get; set; } //added

         private int boardId; //added
         //check this tomorrow with tom



         private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
         //methods:

         //constructor
         public Task(int id, DateTime due_time, string title = "", string description = "")
         {

             //logging setup
             var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
             XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
             log.Info("A task " + title + " was created");

             Id = id;
             Title = title;
             Description = description;

             Creation_time = DateTime.Now;
             Due_time = due_time;
             assignee = null;
         }

     }
 }

