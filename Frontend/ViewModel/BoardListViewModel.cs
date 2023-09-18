using Frontend.Model;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel
{
    public class BoardListViewModel : NotifiableObject
    {
        private List<BoardModel> boards;
        //setter and getter
        public List<BoardModel> Boards
        {
            get => boards;
            set
            {
                boards = value;
                RaisePropertyChanged("Boards");
            }
        }
        //field, setter and getter for selected board(after clicking the select board button)
        private BoardModel selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return selectedBoard;
            }
            set
            {
                selectedBoard = value;
                RaisePropertyChanged("SelectedBoard");
            }
        }
        private Model.BackendController controller;
        private UserModel user;
        public string Title { get; private set; }
        /// <summary>
        /// logout function
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            try
            {
                controller.Logout(user.Email);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// this is the responsible for showing datas on the window
        /// </summary>
        /// <param name="user"></param>
        public BoardListViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Title = "Boards for " + user.Email;
            boards = new List<BoardModel>();
            List<object> bs = controller.GetUserBoards(user.Email);
            foreach(JsonElement b in bs)
            {
                if (b.ValueKind == JsonValueKind.Number && b.TryGetInt32(out int intValue))
                {
                    int id = b.GetInt32();
                    object nameValue = JsonSerializer.Deserialize<Response>(controller.boardService.GetBoardName(id)).ReturnValue;
                    string name = JsonSerializer.Deserialize<string>((JsonElement)nameValue);
                    boards.Add(new BoardModel(controller, id,name));
                }
            }
        }

    }
}
