using System.Collections.Generic;
using System.Windows.Documents;
using Frontend.Model;

namespace Frontend.ViewModel;

public class TasksViewModel : NotifiableObject
{
    public BackendController controller { get; private set; }
    private BoardModel board;
    private string boardName;
    private List<TaskModel> backLog;

    public List<TaskModel> BackLog
    {
        get => backLog;
    }
    private List<TaskModel> inProgress;

    public List<TaskModel> InProgress
    {
        get => inProgress;
    }
    private List<TaskModel> done;

    public List<TaskModel> Done
    {
        get => done;
    }

    public string BoardName
    {
        get => boardName;
    }

    public TasksViewModel(BoardModel boardModel)
    {
        board = boardModel;
        this.boardName = boardModel.boardName;
        controller = boardModel.Controller;
        backLog = boardModel.BackLog;
        inProgress = boardModel.InProgress;
        done = boardModel.Done;
    }
}