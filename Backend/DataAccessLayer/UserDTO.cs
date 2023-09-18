using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDTO : DTO
    {
        private string email;
        private string password;
        public UserDTO(string email, string password) :base(new UserDAO())
        {
            this.email = email;
            this.password = password;
        }
        public string GetEmail() { return email; }
        public string GetPassword() { return password; }
    }
}
