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

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserBoardDAO : DAO
    {
        private const string UserBoardTable = "UserBoard";
        private const string name = "Email";
        private const string owner = "IsOwner";
        private const string id = "BoardId";

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserBoardDAO() : base(UserBoardTable)
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
            log.Info("Converting database entries extracted by reader into a DTO in BoardDAO.");

            UserBoardDTO userboardDTO = new(reader.GetString(0), reader.GetInt32(1), reader.GetString(2));
            return userboardDTO;
        }
        /// <summary>
        /// insert function that supports join boards
        /// </summary>
        /// <returns></returns>
        public bool Insert(UserBoardDTO ubd)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Attempting to open connection with database and inserting a new board entry in BoardDalController.");

                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserBoardTable} ({name}, {id}, {owner}) " +
                        $"VALUES (@emailVal,@boardIdVal,@ownerVal);";

                    SQLiteParameter emailv = new SQLiteParameter(@"emailVal", ubd.email);

                    SQLiteParameter boardIdv = new SQLiteParameter(@"boardIdVal", ubd.boardid);

                    SQLiteParameter ownerv = new SQLiteParameter(@"ownerVal", ubd.isOwner);


                    command.Parameters.Add(emailv);

                    command.Parameters.Add(boardIdv);

                    command.Parameters.Add(ownerv);


                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    log.Debug("successfully added new board entry into database.");
                }
                catch
                {
                    //could have to throw error
                    log.Error("Attempting to add new board entry to database was unsuccessful for some reason");
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
       /// deleting userboard table from database
       /// </summary>
       /// <returns></returns>
        public bool Delete()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {UserBoardTable}"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete " + UserBoardTable);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted table " + UserBoardTable);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting table " + UserBoardTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in UserBoardDAO");

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
                command.CommandText = $"select * from {UserBoardTable}";
                SQLiteDataReader dataReader = null;
                try
                {
                    log.Info("Attempting to open connection with database and select data from " + UserBoardTable);

                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }

                    log.Debug("Successfully selected all entries from " + UserBoardTable);
                }
                catch
                {
                    //check if error
                    log.Error("Selecting entries from table " + UserBoardTable + " from the database was unsuccessful for some reason");
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
        /// updating a specified entry from database
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool Update(string attributeName, string attributeValue, string email, int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {UserBoardTable} set [{attributeName}]={attributeValue} where Email='{email}' and BoardId={boardId}"
                };
                try
                {
                    log.Info("Attempting to open connection and update " + attributeName + " in table " + UserBoardTable + " in database");

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully updated entry in table " + UserBoardTable);
                }
                catch(Exception ex)
                {
                    //check if error
                    log.Error("Updating entry in table " + UserBoardTable + " was unsuccessful for " + ex.Message);
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
        /// deleting a specified entry from database
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool Delete(string email, int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {UserBoardTable} where Email='{email}' and BoardId = {boardId}"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete " + UserBoardTable);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted table " + UserBoardTable);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting table " + UserBoardTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in UserBoardDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        /// <summary>
        /// deleting a specified entry from database
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool Delete(int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {UserBoardTable} where BoardId = {boardId}"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete " + UserBoardTable);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted table " + UserBoardTable);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting table " + UserBoardTable + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in UserBoardDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        /// <summary>
        /// selecting a specified entry from database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<DTO> Select(string email)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {UserBoardTable} where Email = '{email}'";
                SQLiteDataReader dataReader = null;
                try
                {
                    log.Info("Attempting to open connection with database and select data from " + UserBoardTable);

                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }

                    log.Debug("Successfully selected all entries from " + UserBoardTable);
                }
                catch
                {
                    //check if error
                    log.Error("Selecting entries from table " + UserBoardTable + " from the database was unsuccessful for some reason");
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
