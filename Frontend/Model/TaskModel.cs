using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private int taskId;
        private DateTime creationTime;
        private string title;
        private string description;
        private DateTime dueDate;
        private int columnId;
        public TaskModel(BackendController controller, int taskId, DateTime creationTime, string title, string description, DateTime dueDate, int columnId) : base(controller)
        {
            this.taskId = taskId;
            this.creationTime = creationTime;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.columnId = columnId;
        }
        //setters and getters
        public int TaskId
        {
            get => taskId;
            set
            {
                taskId = value;
                RaisePropertyChanged("TaskId");
            }
        }
        public DateTime CreationTime
        {
            get => creationTime;
            set
            {
                creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        public DateTime DueTime
        {
            get => dueDate;
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueTime");
            }
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        public int ColumnId
        {
            get => columnId;
            set
            {
                columnId = value;
                RaisePropertyChanged("ColumnId");
            }
        }
    }
}
