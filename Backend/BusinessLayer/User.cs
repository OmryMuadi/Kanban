using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

using log4net;
using log4net.Config;


namespace IntroSE.Kanban.Backend.BusinessLayer
{


    public class User
    {
        /// <summary>
        /// User is an object that can be created through the userManagement, user can login/logout of the system and is
        /// the object representing the client of the project
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="password">the password of the user</param>
        /// <param name="loggedIn">status of the user- logged in or not</param>

        //fields
        private string email;
        private string password;
        private bool login_status;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public User(string email, string password)

        {
            this.password = password;
            this.email = email;
            login_status = true;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("New user is created!");
        }
        /// <summary>
        /// This function changes the current password to the new one
        /// </summary>
        /// <param name="new_password"></param>
        /// <returns></returns>
        public bool ChangePassword(string new_password)
        {
            log.Debug("Successfully called method passwordMatch in User, checking if password matches user's password.");
            if (new_password.Equals(this.password)) return false;
            this.password = new_password;
            return true;
        }
        /// <summary>
        /// This function sets the "log in" state of the user to "not logged in"
        /// </summary>
        public void Logout()
        {
            this.login_status = false;
            log.Debug("Successfully called method logout in User, logging out user.");
        }
        /// <summary>
        /// This function sets the "log in" state of the user to "logged in"
        /// </summary>
        public void Login()
        {
            this.login_status = true;
            log.Debug("Successfully called method login in User, logging in user");
        }
        /// <summary>
        /// getting login status
        /// </summary>
        /// <returns></returns>
        public bool GetLogin_status()
        {
            return login_status;
        }
        /// <summary>
        /// getting password of a user
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return password;
        }
    }
}
