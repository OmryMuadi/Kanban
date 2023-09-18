using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskColumnJson
    {
        [JsonInclude]
        public int Id { get; set; }
        [JsonInclude]
        public DateTime CreationTime { get; set; }
        [JsonInclude]
        public string Title { get; set; }
        [JsonInclude]
        public string Description { get; set; }
        [JsonInclude]
        public DateTime DueDate { get; set; }
        [JsonInclude]
        public int ColumnId { get; set; }
    }
}
