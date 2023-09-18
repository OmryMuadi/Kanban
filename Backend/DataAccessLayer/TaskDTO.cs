using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDTO : DTO
    {
        private int Id;
        private DateTime Creation_time;
        private string Title;
        private string Description;
        private DateTime Due_time;
        private int columnId;
        private int boardId;
        private string assignee;
        public TaskDTO(int id, DateTime creation_time, string title, string description, DateTime due_time, int columnId, int boardId, string assignee):base(new TaskDAO())
        {
            Id = id;
            Creation_time = creation_time;
            Title = title;
            Description = description;
            Due_time = due_time;
            this.columnId = columnId;
            this.boardId = boardId;
            this.assignee = assignee;
        }
        public int GetId() { return Id; }
        public DateTime GetCreation_time() { return Creation_time; }
        public string GetTitle() { return Title; }
        public string GetDescription() { return Description; }
        public DateTime GetDue_time() { return Due_time; }
        public int GetColumnId() { return columnId; }
        public int GetBoardId() { return boardId; }
        public string GetAssignee() { return assignee; }
    }
}
