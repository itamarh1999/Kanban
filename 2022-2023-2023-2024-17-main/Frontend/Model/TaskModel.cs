using System;
using IntroSE.Kanban.Backend.buisnessLayer;

namespace Frontend.Model;

public class TaskModel : NotifiableModelObject
{
    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            this._title = value;
            RaisePropertyChanged("Title");
        }
    }
    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            this._description = value;
            RaisePropertyChanged("Description");
        }
    }
    private string _assignee;
    public string Assignee
    {
        get => _assignee;
        set
        {
            this._assignee = value;
            RaisePropertyChanged("Body");
        }
    }

    private string _dueDate;

    public string DueDate
    {
        get => _dueDate;
        set
        {
            this._dueDate = value;
            RaisePropertyChanged("DueDate");
        }
    }

    public TaskModel(BackendController controller, string title, string
        desc, string assignee,DateTime dueDate) : base(controller)
    {
        Title ="Title: " + title;
        Description = "Description: " + desc;
        if (!assignee.Equals(""))
            Assignee = "Assignee: " + assignee;
        else
            Assignee = "Assignee: " + "NONE";
        DueDate = "DueDate: " + dueDate.ToShortDateString();
    }
}
