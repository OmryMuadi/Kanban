using Frontend.Model;
using Frontend.ViewModel;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Windows;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardListView : Window
    {
        private BoardListViewModel viewModel;
        private UserModel user;
        public BoardListView(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardListViewModel(u);
            this.DataContext = viewModel;
            this.user = u;
        }

        /// <summary>
        /// logout button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_button(object sender, RoutedEventArgs e)
        {
            this.viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        /// <summary>
        /// button for selecting a board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_Select(object sender, RoutedEventArgs e)
        {
            BoardView boardView = new BoardView(user, viewModel.SelectedBoard);
            if (viewModel.SelectedBoard != null)
            {
                boardView.Show();
                this.Close();
            }
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
