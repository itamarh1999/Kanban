using System.Windows;
using System.Windows.Controls;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View;

public partial class MyBoardsView : Window
{
    private UserModel u;
    private MyBoardsViewModel viewModel;
    public MyBoardsView(UserModel u)
    {
        InitializeComponent();
        this.u = u;
        viewModel = new MyBoardsViewModel(u);
        DataContext = viewModel;
    }
    
    private void Logout_OnClick(object sender, RoutedEventArgs e)
    {
        viewModel.Logout();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void Enter_OnClick(object sender, RoutedEventArgs e)
    {
        BoardModel b = viewModel.Enter();
        if (b != null)
        {
            TasksView t = new TasksView(b);
            t.Show();
            this.Close();
        }
    }
}