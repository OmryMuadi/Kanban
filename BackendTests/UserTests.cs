using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{
    public class UserTests
    {
        private readonly UserService user;
        public UserTests(UserService user)
        {
            this.user = user;
        }
        public void Runtests()
        {
            Console.WriteLine("Registration Test:");
            RegisterTest();
            Console.WriteLine("Logout Test:");
            LogoutTest();
            Console.WriteLine("Login Test:");
            LoginTest();
            Console.WriteLine("Change Password Test:");
            ChangePasswordTest();
            Console.WriteLine("Check Password Test:");
            CheckPasswordTest();
        }
        public void RegisterTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(user.Register("omrymuadi2@gmail.com", null));
            if (res0.ErrorOccurd)
            {
                Console.WriteLine("Register failed: " + res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }
            Response res1 = JsonSerializer.Deserialize<Response>(user.Register("omrymuadi2@gmail.com", "123"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Register failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(user.Register("omrymuadi2@gmail.com", "Im123"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("Register failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }
            Response res3 = JsonSerializer.Deserialize<Response>(user.Register("omrymuadi2@gmail.com", "Om1029"));
            if (res3.ErrorOccurd)
            {
                Console.WriteLine("Register failed: " + res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }
        }
        public void LogoutTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(user.Logout("omry@gmail.com"));
            if (res0.ErrorOccurd)
            {
                Console.WriteLine("Logout failed: " + res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("User logged out successfully");
            }
            Response res1 = JsonSerializer.Deserialize<Response>(user.Logout("omrymuadi2@gmail.com"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Logout failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("User logged out successfully");
            }
        }
        public void LoginTest()
        {
            Response res4 = JsonSerializer.Deserialize<Response>(user.Login("omry@gmail.com", "ahdgs"));
            if (res4.ErrorOccurd)
            {
                Console.WriteLine("Login failed: " + res4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("User logged in successfully");
            }
            Response res0 = JsonSerializer.Deserialize<Response>(user.Login("omrymuadi2@gmail.com", null));
            if (res0.ErrorOccurd)
            {
                Console.WriteLine("Login failed: " + res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("User logged in successfully");
            }
            Response res1 = JsonSerializer.Deserialize<Response>(user.Login("omrymuadi2@gmail.com", "123"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("Login failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("User logged in successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(user.Login("omrymuadi2@gmail.com", "Im123"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("Login failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("User logged in successfully");
            }
        }
        public void ChangePasswordTest()
        {
            Response res0 = JsonSerializer.Deserialize<Response>(user.ChangePassword("omrymuadi2@gmail.com", "eeee"));
            if (res0.ErrorOccurd)
            {
                Console.WriteLine("ChangePassword failed: " + res0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Password changed successfully");
            }
            Response res1 = JsonSerializer.Deserialize<Response>(user.ChangePassword("omrymuadi2@gmail.com", "Im123"));
            if (res1.ErrorOccurd)
            {
                Console.WriteLine("ChangePassword failed: " + res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Password changed successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(user.ChangePassword("omrymuadi2@gmail.com", "abcd"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("ChangePassword failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Password changed successfully");
            }
            Response res3 = JsonSerializer.Deserialize<Response>(user.ChangePassword("omrymuadi2@gmail.com", "Ab3333"));
            if (res3.ErrorOccurd)
            {
                Console.WriteLine("ChangePassword failed: " + res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Password changed successfully");
            }
        }
        public void CheckPasswordTest()
        {
            Response res3 = JsonSerializer.Deserialize<Response>(user.CheckPassword("9b3333"));
            if (res3.ErrorOccurd)
            {
                Console.WriteLine("CheckPassword failed: " + res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Password is valid");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(user.CheckPassword("Im123"));
            if (res2.ErrorOccurd)
            {
                Console.WriteLine("CheckPassword failed: " + res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Password is valid");
            }
        }
    }
}
