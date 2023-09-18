using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserBoardDTO : DTO
    {
        public string email { get; set; }
        public int boardid { get; set; }
        public string isOwner { get; set; }
        public UserBoardDTO(string email, int boardid, string isowner) : base(new UserBoardDAO())
        { 
            this.email= email;
            this.boardid= boardid;
            this.isOwner= isowner;
        }
        
    }
}
