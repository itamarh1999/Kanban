using System;
using System.Windows;
using System.Windows.Controls;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.viewModel = (MainViewModel)DataContext;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                 MyBoardsView boardView = new MyBoardsView(u);
                 boardView.Show();
                 this.Close();
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Register();
            if (u != null)
            {
                MyBoardsView boardView = new MyBoardsView(u);
                boardView.Show();
                this.Close();
            }
        }
    }
}