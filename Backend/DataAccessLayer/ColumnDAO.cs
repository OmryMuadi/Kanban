using log4net;
using log4net.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnDAO : DAO
    {
        private const string ColumnTable = "Column";
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string id = "ID";
        private readonly string boardId = "BoardId";
        private readonly string maxTask = "maxTask";
        private readonly string name = "Name";
        public ColumnDAO() : base(ColumnTable)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// This method extracts data from the database and turns it into a DTO, (specifically used to load data into project)
        /// </summary>
        /// <param name="reader">the sql reader that translates the database data</param>
        /// <returns>returns a dto</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            log.Info("Converting database entries extracted by reader into a DTO in ColumnDAO.");

           ColumnDTO columnDto = new(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3));
            return columnDto;
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
                    CommandText = $"update {ColumnTable} set [{attributeName}]=@{attributeName} where ID = '{id}' and BoardId = '{boardId}'"
                };
                try
                {
                    log.Info("Attempting to open connection and update " + attributeName + " in table " + ColumnTable + " in database");

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully updated entry in table " + ColumnTable);
                }
                catch
                {
                    //check if error
                    log.Error("Updating entry in table " + ColumnTable + " was unsuccessful for some reason");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in columnDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }

        /// <summary>
        /// Inserting a specified entry to the database
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool Insert(ColumnDTO column)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Attempting to open connection with database and inserting a new User entry in UserDAO.");

                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTable} ({id}, {boardId}, {maxTask}, {name}) " +
                        $"VALUES (@columnIdval,@boardIdval,@maxTaskval,@nameval);";

                    SQLiteParameter columnId = new SQLiteParameter(@"columnIdval", column.GetId());

                    SQLiteParameter boardIdv = new SQLiteParameter(@"boardIdval", column.GetBoardId());

                    SQLiteParameter maxTaskv = new SQLiteParameter(@"maxTaskval", column.GetMaxTask());

                    SQLiteParameter namev = new SQLiteParameter(@"nameval", column.GetName());


                    command.Parameters.Add(columnId);

                    command.Parameters.Add(boardIdv);

                    command.Parameters.Add(maxTaskv);

                    command.Parameters.Add(namev);


                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    log.Debug("successfully added new Column entry into database.");
                }
                catch
                {
                    //could have to throw error
                    log.Error("Attempting to add new User entry to database was unsuccessful for some reason");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection with database in ColumnDAO.");

                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
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
                command.CommandText = $"select * from {ColumnTable}";
                SQLiteDataReader dataReader = null;
                try
                {
                    log.Info("Attempting to open connection with database and select data from " + ColumnTable);

                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }

                    log.Debug("Successfully selected all entries from " + ColumnTable);
                }
                catch
                {
                    //check if error
                    log.Error("Selecting entries from table " + ColumnTable + " from the database was unsuccessful for some reason");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        log.Debug("Closing dataReader in DAO");

                        dataReader.Close();
                    }

                    log.Debug("Disposing command and closing connection in ColumnDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }
        /// <summary>
        /// deleting a specified entry from database
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool DeleteColumns(long boardId) // deletion of all columns
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {ColumnTable} where BoardId='{boardId}'"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete an entry from " + ColumnTable);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted entry from table " + ColumnTable);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting entry in table " + ColumnTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in DalController");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
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
                    CommandText = $"delete from {ColumnTable} "
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete content from  " + ColumnTable + " table");

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted " + ColumnTable + "table");
                }
                catch
                {
                    //check if error
                    log.Error("Deleting table " + ColumnTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in ColumnDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        }
}
