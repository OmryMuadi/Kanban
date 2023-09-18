using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskDAO : DAO
    {
        private const string taskTable = "Task";
        private const string id = "ID";
        private const string creation_time = "CreationTime";
        private const string title = "Title";
        private const string description = "Description";
        private const string due_time = "DueTime";
        private const string columnId = "columnId";
        private const string boardId = "boardId";
        private const string assignee = "assignee";

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TaskDAO() : base(taskTable)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// This method inserts a Task into the task table in the database
        /// </summary>
        /// <param name="task">a task for the database</param>
        /// <returns>returns true if the insertion was successful</returns>
        public bool Insert(TaskDTO task)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;

                SQLiteCommand command = new SQLiteCommand(null, connection);

                try
                {
                    log.Info("Attempting to open connection with database and inserting a new task entry in TaskDalController.");

                    connection.Open();
                    command.CommandText = $"INSERT INTO {taskTable} ({id}, {creation_time},{title},{description},{due_time},{columnId},{boardId},{assignee}) " +
                        $"VALUES (@taskIdVal,@creationTimeVal,@titleVal,@decsVal,@dueDateVal,@columnIndexVal,@boardIdVal,@assigneeVal);";

                    SQLiteParameter taskIdParam = new SQLiteParameter(@"taskIdVal", task.GetId());

                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", task.GetCreation_time());

                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.GetTitle());

                    SQLiteParameter decsParam = new SQLiteParameter(@"decsVal", task.GetDescription());

                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.GetDue_time());

                    SQLiteParameter columnIndexParam = new SQLiteParameter(@"columnIndexVal", task.GetColumnId());

                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", task.GetBoardId());

                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", task.GetAssignee());


                    command.Parameters.Add(taskIdParam);

                    command.Parameters.Add(creationTimeParam);

                    command.Parameters.Add(titleParam);

                    command.Parameters.Add(decsParam);

                    command.Parameters.Add(dueDateParam);

                    command.Parameters.Add(columnIndexParam);

                    command.Parameters.Add(boardIdParam);

                    command.Parameters.Add(assigneeParam);


                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    log.Debug("successfully added new task entry into database.");
                }
                catch
                {
                    //could have to throw error
                    log.Error("Attempting to add new task entry to database was unsuccessful for some reason");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection with database in TaskDAO.");

                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }




        /// <summary>
        /// This method extracts data from the database and turns it into a DTO, (specifically used to load data into project)
        /// </summary>
        /// <param name="reader">the sql reader that translates the database data</param>
        /// <returns>returns a dto</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            log.Info("Converting database entries extracted by reader into a DTO in TaskDalController.");

            string assignee = "";

            if (reader.IsDBNull(7))
            {
                assignee = null;
            }
            else
            {
                assignee = reader.GetString(7);
            }

            TaskDTO taskDTO = new TaskDTO(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5), reader.GetInt32(6), assignee);
            return taskDTO;
        }

        /// <summary>
        /// Deletes table content
        /// </summary>
        /// <returns>returns true if the deletion was successful and false otherwise</returns>
        public bool Delete() // delete table content
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {taskTable} "
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete content from  " + taskTable + " table");

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted " + taskTable + "table");
                }
                catch
                {
                    //check if error
                    log.Error("Deleting table " + taskTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in TaskDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        /// <summary>
        /// This method is responsible for updating a specified database entry
        /// </summary> 
        /// <param name="id">id of entry to update</param>
        /// <param name="attributeName">name of attribute to update</param>
        /// <param name="attributeValue"> new value to update</param>
        /// <param name="boardId">type of attribute</param>
        /// <returns>returns true if it was successful, false otherwise</returns>
        public bool Update(int id, string attributeName, string attributeValue, int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTable} set [{attributeName}]=@{attributeName} where ID = '{id}' and boardId = '{boardId}'"
                };
                try
                {
                    log.Info("Attempting to open connection and update " + attributeName + " in table " + taskTable + " in database");

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully updated entry in table " + taskTable);
                }
                catch
                {
                    //check if error
                    log.Error("Updating entry in table " + taskTable + " was unsuccessful for some reason");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in TaskDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }

        /// <summary>
        /// Selects entries from the database
        /// </summary>
        /// <returns>a list of DTOs</returns>
        public List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {taskTable}";
                SQLiteDataReader dataReader = null;
                try
                {
                    log.Info("Attempting to open connection with database and select data from " + taskTable);

                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }

                    log.Debug("Successfully selected all entries from " + taskTable);
                }
                catch
                {
                    //check if error
                    log.Error("Selecting entries from table " + taskTable + " from the database was unsuccessful for some reason");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        log.Debug("Closing dataReader in DAO");

                        dataReader.Close();
                    }

                    log.Debug("Disposing command and closing connection in TaskDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }
        /// <summary>
        /// deleting a specified entry from database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool Delete(long id, long boardId) 
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {taskTable} where ID='{id}' and boardId = '{boardId}'"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete an entry from " + taskTable);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted entry from table " + taskTable);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting entry in table " + taskTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in TaskDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }
        /// <summary>
        /// deleting a specified entry from database
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool DeleteBoardTasks(long boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {taskTable} where boardId = '{boardId}'"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete an entry from " + taskTable);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted entry from table " + taskTable);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting entry in table " + taskTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in TaskDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        /// <summary>
        /// updating a specified entry in the database
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool Update(string attributeName, string attributeValue, int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTable} set [{attributeName}]=@{attributeName} where boardId = '{boardId}'"
                };
                try
                {
                    log.Info("Attempting to open connection and update " + attributeName + " in table " + taskTable + " in database");

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully updated entry in table " + taskTable);
                }
                catch
                {
                    //check if error
                    log.Error("Updating entry in table " + taskTable + " was unsuccessful for some reason");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in TaskDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
    }
}
