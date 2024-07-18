using System;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.buisnessLayer;
/// <summary>
/// this class represents a single entity of a Task.
/// a Task has the following attributes:
/// int: taskID - a unique ID given by the board for each task created on the board.
/// string: title - the title of the task.
/// string: description - the description of the task.
/// DateTime: duedate - the due date for the task.
/// DateTime: the time the task was created.
/// </summary>
public class Task
{
    private int taskID;
    private string title;
    private string description;
    private DateTime dueDate;
    private DateTime creationTime;
    private string assignee;
    private TaskDTO taskDTO;
    /// <summary>
    /// a getter/setter for the task's id.
    /// </summary>
    ///
    public int TaskId
    {
        get => taskID;
        set => taskID = value;
    }
    /// <summary>
    /// a getter/setter for the task's title.
    /// </summary>
    public string Title
    {
        get => title;
        set => title = value;
    }
    /// <summary>
    /// a getter/setter for the task's description.
    /// </summary>
    public string Description
    {
        get => description;
        set => description = value;
    }
    /// <summary>
    /// a getter/setter for the task's duedate.
    /// </summary>
    public DateTime DueDate
    {
        get => dueDate;
        set => dueDate = value;
    }
    /// <summary>
    /// a getter/setter for the task's creation time.
    /// </summary>
    public string Assignee
    {
        get => assignee;
        set => assignee = value;
    }
    /// <summary>
    /// a getter/setter for the task's creation time.
    /// </summary>
    public DateTime CreationTime
    {
        get => creationTime;
        set => creationTime = value;
    }
    public static ILog Log => log;
    private static readonly ILog log = 
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //--------------methods-----------
    /// <summary>
    /// a constructor method for creating a new task.
    /// </summary>
    /// <param name="taskId">the task's unique id as given by the board</param>
    /// <param name="owner">the email of the owner of the board</param>
    /// <param name="boardName">the name of the board</param>
    /// <param name="title">the title of the task</param>
    /// <param name="description">the description of the task, can be blank</param>
    /// <param name="duedate">the due date for the completion of the task</param>
    /// <param name="boardID">the id for the board the task is on</param>
    /// <param name="taskController">a parameter to inject to taskDTO</param>
    public Task(int taskId, string owner, string boardName, string title,
        string description,DateTime duedate, int boardID, TaskController taskController)
    {
        taskID = taskId;
        this.title = title;
        this.description = description;
        this.dueDate = duedate;
        this.creationTime =DateTime.Now;
        this.assignee = "";
        this.taskDTO = new TaskDTO(taskID, boardID, title, description, duedate,DateTime.Now,
            0,"",taskController);
    }

    public Task(TaskDTO taskDto)
    {
        this.taskID = taskDto.TaskId;
        this.assignee = taskDto.Assignee;
        this.title = taskDto.Title;
        this.description = taskDto.Description;
        this.creationTime = taskDto.CreationTime;
        this.dueDate = taskDto.DueDate;
        this.taskDTO = taskDto;
    }

    public Task()
    {
    }

    /// <summary>
    /// a constructor method for creating a task from it's DTO.
    /// </summary>
    /// <param name="taskId">the task's unique id as given by the board</param>
    /// <param name="title">the title of the task</param>
    /// <param name="description">the description of the task, can be blank</param>
    /// <param name="duedate">the due date for the completion of the task</param>
    /// <param name="creationTime">the creation time of the task</param>
    /// <param name="boardID">the id for the board the task is on</param>
    /// <param name="taskController">a parameter to inject to taskDTO</param>
    /// <param name="assignee">the email of the user assigned to the task</param>
    /// <param name="columnOrdinal">the column that holds the task</param>
    public Task(int taskId, string title, string description, DateTime duedate,
        DateTime creationTime, int boardID, TaskController taskController,string assignee,int columnOrdinal)
    {
        taskID = taskId;
        this.title = title;
        this.description = description;
        this.dueDate = duedate;
        this.creationTime =creationTime;
        this.assignee = assignee;
        this.taskDTO = new TaskDTO(taskID, boardID, title, description, duedate,DateTime.Now,
            columnOrdinal,assignee,taskController);
    }
    /// <summary>
    /// a getter for the tasks unique id.
    /// </summary>
    /// <returns>int: the task id</returns>
    public int getTaskID()
    {
        return this.taskID;
    }
    /// <summary>
    /// a function that updates the task's title.
    /// </summary>
    /// <param name="title">string: the new title for the task</param>
    public void updateTaskTitle(string title, int boardID, string email)
    {
        if (email != assignee)
        {
            log.Info(($"{email} isnt the assigner so he cant update the task"));
            throw new Exception($"{email} isnt the assigner so he cant update the task");
        }
        if (title.Length > 50 | title.Length < 1)
        {
            log.Info(($"title to update length was not according to the restriction:{title}"));
            throw new Exception("title must be between 1 to 50 characters");
        }
        this.title = title;
        this.taskDTO.UpdateTitle(this.taskID,boardID,title);
        log.Info($"title has been updated to {title}");
    }
    /// <summary>
    /// a function that updates the task's description.
    /// </summary>
    /// <param name="description">string: the new description for the task</param>
    public void updateTaskDescription(string description, int boardID)
    {
        this.description = description;
        this.taskDTO.UpdateDescription(this.taskID,boardID,description);
        log.Info($"description has been updated to {this.description}");

    }
    /// <summary>
    /// a function that updates the task's duedate.
    /// </summary>
    /// <param name="duedate">string: the new duedate for the task</param>
    public void updateTaskDueDate(DateTime duedate ,int boardID,string email)
    {
        if ( this.assignee != email)
        {
            log.Error("only the assignee can update the task");
            throw new Exception("only the assignee can update the task");
        }
        this.dueDate = duedate;
        this.taskDTO.UpdateDueDate(this.taskID,boardID,duedate);
        log.Info($"duedate has been updated to {duedate.ToShortDateString()}");
    }
    /// <summary>
    /// a setter for the assignee
    /// </summary>
    /// <param name="emailAssignee">the new assigned user's email</param>
    public void AssignTask(string emailAssignee, int boardID,string emailAssigner)
    {
        if (assignee != "" && assignee != emailAssigner)
        {
            log.Error($"{emailAssigner} cant assign this task because he is not the assignee of it ");
            throw new Exception($"{emailAssigner} cant assign this task because he is not the assignee of it ");
        }
        this.assignee = emailAssignee;
        this.taskDTO.UpdateAssignee(this.taskID,boardID,emailAssignee);
    }
    /// <summary>
    /// checks if the assignee is the same as the one provided and if so leaves the field empty.
    /// </summary>
    /// <param name="email">the email to check if assignee</param>
    public void RemoveAssignee(string email)
    {
        if (this.assignee == email)
        {
            this.assignee = "";
        }
    }
    /// <summary>
    /// this function rewrites the column ordinal of the task in its dto
    /// </summary>
    /// <param name="newCollumnOrdinal">the new columnOrdinal</param>
    /// <param name="boardID">the board id of the board that holds the task</param>
    public void AdvanceTask(int newCollumnOrdinal,int boardID)
    {
        this.taskDTO.UpdateColumnOrdinal(this.taskID,boardID,newCollumnOrdinal);
    }
}