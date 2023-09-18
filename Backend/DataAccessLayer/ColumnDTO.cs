using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnDTO : DTO
    {
        private int id;
        private int boardId;
        private int maxTask;
        private string name;
        public ColumnDTO(int id, int boardId, int maxTask, string name): base(new ColumnDAO())
        {
            this.id = id;
            this.boardId = boardId;
            this.maxTask = maxTask;
            this.name = name;
        }
        public int GetId() { return id; }
        public int GetBoardId() { return boardId; }
        public int GetMaxTask() { return maxTask; }
        public string GetName() { return name; }
    }
}
