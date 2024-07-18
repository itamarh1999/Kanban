using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Documents;

namespace Frontend.Model;

public class BoardModel:NotifiableModelObject
{
    public UserModel user { get; }
    public string boardName;
    public List<TaskModel> BackLog;
    public List<TaskModel> InProgress;
    public List<TaskModel> Done;
    public BoardModel(BackendController controller, UserModel user , string boardname) : base(controller)
    {
        this.user = user;
        this.boardName = boardname;
        BackLog = controller.GetColumn(user.Email, boardname, 0);
        InProgress = controller.GetColumn(user.Email, boardname, 1);
        Done =controller.GetColumn(user.Email, boardname, 2);
    }
}

