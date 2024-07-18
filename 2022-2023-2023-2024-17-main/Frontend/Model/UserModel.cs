using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;

namespace Frontend.Model;

public class UserModel:NotifiableModelObject
{
    private string _email;
    private BackendController controller;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            RaisePropertyChanged("Email");
        }
    }


    public BoardModel GetBoard(string boardname)
    {
        return new BoardModel(Controller, this , boardname);
    }
    public UserModel(BackendController controller, string email) :
        base(controller)
    {
        this.Email = email;
    }
}