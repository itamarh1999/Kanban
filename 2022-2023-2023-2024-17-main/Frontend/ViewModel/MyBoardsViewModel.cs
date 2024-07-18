using System.Collections.Generic;
using Frontend.Model;
using Frontend.View;
using IntroSE.Kanban.Backend.buisnessLayer;

namespace Frontend.ViewModel;

public class MyBoardsViewModel : NotifiableObject
{
    private BackendController controller;
    private UserModel user;
    private string _title;

    public string title
    {
        get => _title;
    }
    private bool _isEnabaled;
    public bool isEnabaled
    {
        get => _isEnabaled;
        set
        {
            _isEnabaled = value;
            RaisePropertyChanged("isEnabaled");
        }
    }
    private string _selectedBoard;
    public string SelectedBoard
    {
        get => _selectedBoard;
        set
        {
            _selectedBoard = value;
            isEnabaled = value != null;
            RaisePropertyChanged("SelectedBoardModel");
        }
    }
    private List<string> _myBoards;
    public List<string> myBoards
    {
        get => _myBoards;
        set
        {
            _myBoards = value;
            RaisePropertyChanged("myBoards");
        }
    }

    public MyBoardsViewModel(UserModel user)
    {
        this.user = user;
        controller = this.user.Controller;
        _title = user.Email;
        _isEnabaled = false;
        _myBoards = controller.GetAllUserBoards(user.Email);
    }

    public void Logout()
    {
        controller.Logout(user.Email);
    }

    public BoardModel Enter()
    {
        return controller.GetBoard(user, _selectedBoard);
    }
}