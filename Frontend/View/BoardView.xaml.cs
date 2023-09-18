using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>

    
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;
        private UserModel user;
        public BoardView(UserModel user, BoardModel u)
        {
            InitializeComponent();
            viewModel = new BoardViewModel(u);
            this.DataContext = viewModel;
            this.user = user;
        }
        /// <summary>
        /// button for returning back to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_button(object sender, RoutedEventArgs e)
        {
            BoardListView userBoardView = new BoardListView(user);
            userBoardView.Show();
            this.Close();
        }
    }
}
