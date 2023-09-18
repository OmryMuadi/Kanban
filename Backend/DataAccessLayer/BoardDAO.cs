using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardDAO : DAO
    {
        //fields
        private const string BoardTableName = "Board";
        private const string name = "BoardName";
        private const string owner = "User";
        private const string id = "BoardId";
        private const string backlog = "Backlog";
        private const string inprogress = "InProgress";
        private const string done = "Done";

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        //constructor
        public BoardDAO() : base(BoardTableName)
        {

            //logging setup
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }


        //methods


        /// <summary>
        /// This method inserts a board entry into the board table in the database
        /// </summary>
        /// <param name="board">a board DTO for the database</param>
        /// <returns>returns true if the insertion was successful</returns>
        public bool Insert(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Attempting to open connection with database and inserting a new board entry in BoardDalController.");

                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({name}, {owner},{id} , {backlog} , {inprogress} , {done}) " +
                        $"VALUES (@boardNameVal,@ownerVal,@boardIdVal,@backLogCapacity,@inProgressCapacity,@doneCapacity);";

                    SQLiteParameter boardNameParam = new SQLiteParameter(@"boardNameVal", board.GetName());

                    SQLiteParameter ownerParam = new SQLiteParameter(@"ownerVal", board.GetUser());

                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", board.GetID());

                    SQLiteParameter backLogCapacityParam = new SQLiteParameter(@"backLogCapacity", board.Getbacklog());

                    SQLiteParameter inProgressCapacityParam = new SQLiteParameter(@"inProgressCapacity", board.GetInProgress());

                    SQLiteParameter doneCapacityParam = new SQLiteParameter(@"doneCapacity", board.GetDone());


                    command.Parameters.Add(boardNameParam);

                    command.Parameters.Add(ownerParam);

                    command.Parameters.Add(boardIdParam);

                    command.Parameters.Add(backLogCapacityParam);

                    command.Parameters.Add(inProgressCapacityParam);

                    command.Parameters.Add(doneCapacityParam);


                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    log.Debug("successfully added new board entry into database.");
                }
                catch(Exception ex)
                {
                    //could have to throw error
                    log.Error("Attempting to add new board entry to database was unsuccessful for " + ex.Message);
                }
                finally
                {
                    log.Debug("Disposing command and closing connection with database in BoardDAO.");

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
            log.Info("Converting database entries extracted by reader into a DTO in BoardDAO.");

            BoardDTO boardDTO = new(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
            return boardDTO;
        }

        /// <summary>
        /// This method is responsible for updating a specified database entry
        /// </summary> 
        /// <param name="id">id of entry to update</param>
        /// <param name="attributeName">name of attribute to update</param>
        /// <param name="attributeValue"> new value to update</param>
        /// <param name="kind">type of attribute</param>
        /// <returns>returns true if it was successful, false otherwise</returns>
        public bool Update(long id, string attributeName, string attributeValue, string kind)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {BoardTableName} set [{attributeName}]=@{attributeName} where {kind}={id}"
                };
                try
                {
                    log.Info("Attempting to open connection and update " + attributeName + " in table " + BoardTableName + " in database");

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully updated entry in table " + BoardTableName);
                }
                catch
                {
                    //check if error
                    log.Error("Updating entry in table " + BoardTableName + " was unsuccessful for some reason");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in BoardDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        /// <summary>
        /// Selects entries from the database
        /// </summary>
        /// <returns></returns>
        public List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {BoardTableName}";
                SQLiteDataReader dataReader = null;
                try
                {
                    log.Info("Attempting to open connection with database and select data from " + BoardTableName);

                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }

                    log.Debug("Successfully selected all entries from " + BoardTableName);
                }
                catch
                {
                    //check if error
                    log.Error("Selecting entries from table " + BoardTableName + " from the database was unsuccessful for some reason");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        log.Debug("Closing dataReader in DalController");

                        dataReader.Close();
                    }

                    log.Debug("Disposing command and closing connection in BoardDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }


        /// <summary>
        /// This method is responsible for deleting data from the database
        /// </summary>
        /// <param name="boardId">an id of the board we want to delete</param>
        /// <returns>returns true if it was successful</returns>
        public bool Delete(long boardId) // deletion of board
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {BoardTableName} where BoardId='{boardId}'"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete an entry from " + BoardTableName);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted entry from table " + BoardTableName);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting entry in table " + BoardTableName + " was unsuccessful for some reason!");
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
                    CommandText = $"delete from {BoardTableName} "
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete content from  " + BoardTableName + " table");

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted " + BoardTableName + "table");
                }
                catch
                {
                    //check if error
                    log.Error("Deleting table " + BoardTableName + " was unsuccessful for some reason!");
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
        /// Selects entries from the database by given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DTO> Select(string user)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {BoardTableName} where User = '{user}'";
                SQLiteDataReader dataReader = null;
                try
                {
                    log.Info("Attempting to open connection with database and select data from " + BoardTableName);

                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }

                    log.Debug("Successfully selected all entries from " + BoardTableName);
                }
                catch
                {
                    //check if error
                    log.Error("Selecting entries from table " + BoardTableName + " from the database was unsuccessful for some reason");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        log.Debug("Closing dataReader in DalController");

                        dataReader.Close();
                    }

                    log.Debug("Disposing command and closing connection in BoardDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }
        
    }
}
