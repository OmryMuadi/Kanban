using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        //fields, setters and getters
        private List<TaskModel> backlogTasks;
        private List<TaskModel> inProgressTasks;
        private List<TaskModel> doneTasks;
        private string Title;
        private Model.BackendController controller;
        public List<TaskModel> BacklogList
        {
            get => backlogTasks;
            set
            {
                backlogTasks = value;
                RaisePropertyChanged("BacklogList");
            }
        }
        public List<TaskModel> InProgressList
        {
            get => inProgressTasks;
            set
            {
                inProgressTasks = value;
                RaisePropertyChanged("InProgressList");
            }
        }
        public List<TaskModel> DoneList
        {
            get => doneTasks;
            set
            {
                doneTasks = value;
                RaisePropertyChanged("DoneList");
            }
        }
        /// <summary>
        /// this is responsible for showing datas on the window
        /// </summary>
        /// <param name="boardModel"></param>
        public BoardViewModel(BoardModel boardModel)
        {
            if (boardModel != null)
            {
                Title = "columns for: " + boardModel.Name;
                controller = boardModel.Controller;
                List<TaskColumnJson> taskj = boardModel.Controller.GetTasks(boardModel.ID);
                backlogTasks = new List<TaskModel>();
                inProgressTasks = new List<TaskModel>();
                doneTasks = new List<TaskModel>();
                foreach (TaskColumnJson t in taskj)
                {
                    if (t.ColumnId == 0)
                    {
                        backlogTasks.Add(new TaskModel(controller, t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.ColumnId));
                    }
                    if (t.ColumnId == 1)
                    {
                        inProgressTasks.Add(new TaskModel(controller, t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.ColumnId));
                    }
                    if (t.ColumnId == 2)
                    {
                        doneTasks.Add(new TaskModel(controller, t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.ColumnId));
                    }
                }
            }
            else
            {
                Title = "There are no boards for this user yet!";
            }
        }

    }
}
