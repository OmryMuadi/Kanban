using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserFacade
    {
        /// <summary>
        /// Is an object representing the control and management of the users of the project.
        /// </summary>
        /// <param name="_users">a list of users</param>
        ///<param name="emailCheck">an attribute to check an email</param>
        private Dictionary<string, User> _users;
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //magic numbers
        private readonly int MAX_PASSWORD_LENGTH = 20;
        private readonly int MIN_PASSWORD_LENGTH = 6;

        private EmailAddressAttribute emailCheck;
        private UserDAO userd;

        public bool IsLoggedIn(string email)
        {
            if (!_users.ContainsKey(email)) return false;
            return _users[email].GetLogin_status();
        }
        public UserFacade()
        {
            userd = new UserDAO();
            _users = new Dictionary<string, User>();
            emailCheck = new EmailAddressAttribute();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUser(string email)
        {
            return _users[email];
        }

        /// <summary>
        /// This function is for registering, making a new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Register(string email, string password)
        {
            log.Debug("successfully called method Register in UserFacade, A new user is registering named: " + email);
            if (_users.ContainsKey(email) || !CheckPassword(password) || password.Length == 0)
                throw new ArgumentException("error in register");
            User user = new User(email, password);
            _users[email] = user;
            if (!userd.Insert(new UserDTO(email, password)))
            {
                throw new Exception("User is not added to database");
            }
            return true;
        }

        /// <summary>
        /// This function is for logging in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public void Login(string email, string password)
        {
            log.Debug("successfully called method Login in UserFacade, user named " + email + "Is logging in");
            if (_users.ContainsKey(email))
            {
                if (!_users[email].GetLogin_status())
                {
                    if (password == null || password.Length == 0)
                        throw new ArgumentException("password is null");
                   if (password.Equals(_users[email].GetPassword()))
                    {
                        _users[email].Login();
                    }
                        
                    else
                        throw new ArgumentException("Password is incorrect");
                }
                else
                    throw new ArgumentException("The email is already logged in");
            }
            else
            {
                throw new ArgumentException("The email is not registered");
            }
            
        }

        /// <summary>
        /// This function is for logging out
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public void Logout(string email)
        {
            log.Debug("successfully called method Logout in UserFacade, user named " + email + "Is logging out");
            if (_users.ContainsKey(email))
            {
                if (_users[email].GetLogin_status())
                {
                    _users[email].Logout();
                }
                else
                    throw new ArgumentException("The email is already logged out");
            }
            else throw new ArgumentException("The email is not registered");
        }

        /// <summary>
        /// This function is for changing password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newP"></param>
        public void ChangePassword(string email, string newP)
        {
            log.Debug("successfully called method ChangePassword in UserFacade, changing password for the user named: " + email);
            if (_users.ContainsKey(email))
            {
                _users[email].ChangePassword(newP);
            }
        }

        /// <summary>
        ///This function checks the validity of the given password in order to allow a succesfull registration. 
        /// </summary>
        /// <param name="password">the password of the user</param>
        /// <returns>true if the password is valid</returns>
        /// <exception cref="Exception">password isnt valid</exception>
        public bool CheckPassword(string password)
        {
            log.Debug("successfully called method passwordCheck in UserManagement, that checks if a password is valid");

            if (password == null)
                throw new ArgumentNullException("password is null");

            if (password.Length < MIN_PASSWORD_LENGTH | password.Length > MAX_PASSWORD_LENGTH)
                throw new ArgumentException("Password illegal");

            int upper = 0;
            int lower = 0;
            int num = 0;

            for (int i = 0; i < password.Length; i++)
            {

                char ch = password[i];
                if (char.IsUpper(ch))
                    upper = upper + 1;
                if (char.IsLower(ch))
                    lower = lower + 1;
                if (char.IsNumber(ch))
                    num = num + 1;

            }

            if (num > 0 & upper > 0 & lower > 0)
            {
                return true;

            }
            throw new ArgumentException("Password illegal");
        }

        /// <summary>
        ///This function checks that the email is valid, by using regex.
        ///</summary>
        /// <param name="email">the email of the user</param>
        /// <returns>true if the email is valid</returns>
        public bool isValidEmail(string email)
        {
            log.Debug("Successfully called method isValidEmail in UserManagement, validating user email.");

            if (email == null)
            {
                throw new Exception("Email is null");
            }

            email = email.ToLower();
            Regex validateEmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex validateEmailRegex2 = new Regex(@"^\w+([.-]?\w+)@\w+([.-]?\w+)(.\w{2,3})+$");
            return validateEmailRegex.IsMatch(email) & validateEmailRegex2.IsMatch(email) & emailCheck.IsValid(email);
        }
        /// <summary>
        /// This functions checks if the user is existing
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool UserExists(string email)
        {
            return _users.ContainsKey(email);
        }
        public string LoadData()
        {
            List<DTO> userDTOs = userd.Select();
            foreach(DTO dto in userDTOs)
            {
                UserDTO u = (UserDTO)dto;
                if (!UserExists(u.GetEmail()))
                {
                    User u1 = new User(u.GetEmail(), u.GetPassword());
                    u1.Logout();
                    _users.Add(u.GetEmail(),u1);
                }
            }
            return "";
        }
        public string DeleteData()
        {
            log.Debug("Successfully called method DeleteData in UserFacade, clearing all data in UserFacade");

            if (userd.Delete())
            {
                _users.Clear();
                return "";
            }
            throw new Exception("Database faild to remove User table");
        }
    }
}
