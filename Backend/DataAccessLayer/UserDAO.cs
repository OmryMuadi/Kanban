﻿using log4net;
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
    internal class UserDAO : DAO
    {
        private const string UserName = "User";
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string email = "Email";
        private readonly string password = "Password";

        public UserDAO() : base(UserName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        }
        /// <summary>
        /// inserting a new user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Insert(UserDTO user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Attempting to open connection with database and inserting a new User entry in UserDAO.");

                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserName} ({email}, {password}) " +
                        $"VALUES (@emailVal,@passwordVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.GetEmail());

                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.GetPassword());


                    command.Parameters.Add(emailParam);

                    command.Parameters.Add(passwordParam);


                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    log.Debug("successfully added new User entry into database.");
                }
                catch(Exception ex)
                {
                    //could have to throw error
                    log.Error("Attempting to add new User entry to database was unsuccessful for some reason" + ex.Message);
                }
                finally
                {
                    log.Debug("Disposing command and closing connection with database in UserDAO.");

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
            log.Info("Converting database entries extracted by reader into a DTO in UserDAO.");

            UserDTO userDTO = new(reader.GetString(0), reader.GetString(1));
            return userDTO;
        }
        /// <summary>
        /// This method is responsible for updating a specified database entry
        /// </summary> 
        /// <param name="id">id of entry to update</param>
        /// <param name="attributeName">name of attribute to update</param>
        /// <param name="attributeValue"> new value to update</param>
        /// <param name="kind">type of attribute</param>
        /// <returns>returns true if it was successful, false otherwise</returns>
        public bool Update(string id, string attributeName, string attributeValue, string kind)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {UserName} set [{attributeName}]=@{attributeName} where {kind}='{id}'"
                };
                try
                {
                    log.Info("Attempting to open connection and update " + attributeName + " in table " + UserName + " in database");

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully updated entry in table " + UserName);
                }
                catch
                {
                    //check if error
                    log.Error("Updating entry in table " + UserName + " was unsuccessful for some reason");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in UserDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        /// <summary>
        /// This method is responsible for selecting the data from a database table
        /// </summary>
        /// <returns>returns a list of dto's</returns>
        public List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {UserName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    log.Info("Attempting to open connection with database and select data from " + UserName);

                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }

                    log.Debug("Successfully selected all entries from " + UserName);
                }
                catch
                {
                    //check if error
                    log.Error("Selecting entries from table " + UserName + " from the database was unsuccessful for some reason");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        log.Debug("Closing dataReader in DalController");

                        dataReader.Close();
                    }

                    log.Debug("Disposing command and closing connection in UserDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }
        /// <summary>
        /// This method is responsible for deleting data from the database
        /// </summary>
        /// <param name="email">an email of the user we want to delete</param>
        /// <returns>returns true if it was successful</returns>
        public bool Delete(string email) // deletion of emailId
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {UserName} where email={email}"
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete an entry from " + UserName);

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted entry from table " + UserName);
                }
                catch
                {
                    //check if error
                    log.Error("Deleting entry in table " + UserName + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in UserDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
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
                    CommandText = $"delete from {UserName} "
                };
                try
                {
                    log.Info("Attempting to open connecting with database and delete content from  " + UserName + " table");

                    connection.Open();
                    res = command.ExecuteNonQuery();

                    log.Debug("Successfully deleted " + UserName + "table");
                }
                catch
                {
                    //check if error
                    log.Error("Deleting table " + UserName + " was unsuccessful for some reason!");
                }
                finally
                {
                    log.Debug("Disposing command and closing connection in UserDAO");

                    command.Dispose();
                    connection.Close();
                }
            }
            return res >= 0;
        }
        
    }
}
