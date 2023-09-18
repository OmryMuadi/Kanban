using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private int id;
        private string name;
        public BoardModel(BackendController controller, int id, string name) : base(controller)
        {
            this.id = id;
            this.name = name;
        }
        //setters and getters
        public int ID
        {
            get => id;
            set
            {
                id = value;
                RaisePropertyChanged("ID");
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

    }
}
