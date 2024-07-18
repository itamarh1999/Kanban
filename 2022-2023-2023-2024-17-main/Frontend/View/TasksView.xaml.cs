using System.Windows;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View;

public partial class TasksView : Window
{
    private BoardModel boardModel;
    private TasksViewModel viewModel;
    public TasksView(BoardModel boardModel)
    {
        InitializeComponent();
        this.boardModel = boardModel;
        this.viewModel = new TasksViewModel(boardModel);
        DataContext = viewModel;
    }

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        MyBoardsView b = new MyBoardsView(boardModel.user);
        b.Show();
        this.Close();
    }
}