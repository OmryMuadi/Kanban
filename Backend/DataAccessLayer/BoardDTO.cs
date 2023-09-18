using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDTO : DTO
    {
        private string name;
        private string user;
        private int ID;
        private int backlog;
        private int in_progress;
        private int done;
        public BoardDTO(string name, string user, int iD, int backlog, int in_progress, int done): base(new BoardDAO())
        {
            this.name = name;
            this.user = user;
            ID = iD;
            this.backlog = backlog;
            this.in_progress = in_progress;
            this.done = done;
        }
        public string GetName() { return name; }
        public string GetUser() { return user; }
        public int GetID() { return ID; }
        public int Getbacklog() { return backlog; }
        public int GetInProgress() { return in_progress; }
        public int GetDone() { return done; }
    }
}
