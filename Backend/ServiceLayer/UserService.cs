
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        internal UserFacade uf;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserService()
        {
            uf = new UserFacade();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        }
        /// <summary>
        /// This function is for registering, making a new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Register(string email, string password)
        {
            try
            {
                log.Info("Attempting to create a new user");

                uf.Register(email, password);

                log.Debug("Created new user successfully");

                Response r = new Response(null, null);
                string js = JsonSerializer.Serialize(r);
                return js;
                
            }
            catch (Exception e)
            {
                log.Error("User not created due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// This function is for logging in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Login(string email, string password)
        {
            try
            {
                log.Info("Attempting to login");
                uf.Login(email, password);
                log.Debug("user logged in successfully");
                Response r = new Response(null, email);
                string js = JsonSerializer.Serialize(r);

                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("User not logged in due to error: " + e.Message);

                Response r = new Response(e.Message, null);
                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// This function is for logging out
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string Logout(string email)
        {
            try
            {
                log.Info("Attempting to log out");

                uf.Logout(email);

                log.Debug("user logged out successfully");

                Response r = new Response(null, null);

                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("User not logged out due to error: " + e.Message);

                Response r = new Response(e.Message, null);

                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// changing a passowrd for a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newP"></param>
        /// <returns></returns>

        public string ChangePassword(string email, string newP)
        {
            try
            {
                log.Info("Attempting to change the password");

                uf.ChangePassword(email, newP);

                log.Debug("Password changed successfully");


                Response r = new Response(null, null);

                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Password is not changed due to error: " + e.Message);


                Response r = new Response(e.Message, null);

                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
        /// <summary>
        /// checking if the password is valid
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string CheckPassword(string password)
        {
            try
            {
                log.Info("Attempting to create a new user");

                uf.CheckPassword(password);

                log.Debug("Password is valid");


                Response r = new Response(null, null);

                return JsonSerializer.Serialize(r);

            }
            catch (Exception e)
            {
                log.Error("Password is Invalid due to: " + e.Message);


                Response r = new Response(e.Message, null);

                string json = JsonSerializer.Serialize(r);
                return json;
            }
        }
    }
}