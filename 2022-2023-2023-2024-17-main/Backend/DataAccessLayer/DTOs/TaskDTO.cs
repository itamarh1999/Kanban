using System;
using Microsoft.VisualBasic;

namespace IntroSE.Kanban.Backend.DataAccessLayer;

public class TaskDTO
{
    public const string TASKID = "TASKID";
    public const string BOARDID = "BOARDID";
    public const string TITLE = "TITLE";
    public const string DESCRIPTION = "DESCRIPTION";
    public const string DUEDATE = "DUEDATE";
    public const string CREATIONDATE = "CREATIONDATE";
    public const string COLUMNORDINAL = "COLUMNORDINAL";
    public const string ASSIGNEE = "ASSIGNEE";

    private int taskID;
    private int boardID;
    private string title;
    private string description;
    private DateTime dueDate;
    private DateTime creationTime;
    private int columnOrdinal;
    private string assignee;
    private TaskController taskController;

    public int TaskId { get => taskID;}

    public int BoardId { get => boardID; }

    public string Title { get => title; }
    public string Description { get => description; }

    public DateTime DueDate {get=> dueDate; }

    public DateTime CreationTime {get=> creationTime;}

    public int ColumnOrdinal {get=> columnOrdinal;}

    public string Assignee {get => assignee;}
    /// <summary>
    /// a setter for the taskController
    /// </summary>
    /// <param name="taskController">the taskController</param>
    public void setControler(TaskController taskController)
    {
        this.taskController = taskController;
    }
    /// <summary>
    /// a constructor method for a taskDTO
    /// </summary>
    /// <param name="taskId">the task unique id</param>
    /// <param name="boardId">the board's id</param>
    /// <param name="title">the task's title</param>
    /// <param name="description">the task's description (default is empty)</param>
    /// <param name="dueDate">the task's due date</param>
    /// <param name="creationTime">the task's creation time</param>
    /// <param name="columnOrdinal">a number that represents the column the task is in</param>
    /// <param name="assignee">the email of the assignee</param>
    /// <param name="taskController">the taskController</param>
    public TaskDTO(int taskId, int boardId, string title, string description, DateTime dueDate, DateTime creationTime,
        int columnOrdinal, string assignee,TaskController taskController)
    {
        taskID = taskId;
        boardID = boardId;
        this.title = title;
        this.description = description;
        this.dueDate = dueDate;
        this.creationTime = creationTime;
        this.columnOrdinal = columnOrdinal;
        this.assignee = assignee;
        this.taskController = taskController;
        Insert();
    }
    /// <summary>
    /// a constructor method for a taskDTO
    /// </summary>
    /// <param name="taskId">the task unique id</param>
    /// <param name="boardId">the board's id</param>
    /// <param name="title">the task's title</param>
    /// <param name="description">the task's description (default is empty)</param>
    /// <param name="dueDate">the task's due date</param>
    /// <param name="creationTime">the task's creation time</param>
    /// <param name="columnOrdinal">a number that represents the column the task is in</param>
    /// <param name="assignee">the email of the assignee</param>
    public TaskDTO(int taskId, int boardId, string title, string description, DateTime dueDate, DateTime creationTime,
        int columnOrdinal, string assignee)
    {
        taskID = taskId;
        boardID = boardId;
        this.title = title;
        this.description = description;
        this.dueDate = dueDate;
        this.creationTime = creationTime;
        this.columnOrdinal = columnOrdinal;
        this.assignee = assignee;
    }
    /// <summary>
    /// this method calls the Insert in taskController
    /// </summary>
    public void Insert()
    {
        taskController.Insert(this);
    }
    /// <summary>
    /// this method calls the UpdateTitle in taskController
    /// it also updates the TITLE field in the DTO.
    /// </summary>
    public void UpdateTitle(int taskId,int boardId, string newTitle)
    {
        taskController.UpdateTitle(taskId, boardId, newTitle);
        title = newTitle;

    }
    /// <summary>
    /// this method calls the UpdateDescription in taskController
    /// it also updates the description field in the DTO.
    /// </summary>
    public void UpdateDescription(int taskId,int boardId, string newDescription)
    {
        taskController.UpdateDescription(taskId, boardId, newDescription);
        description = newDescription;
    }
    /// <summary>
    /// this method calls the UpdateDueDate in taskController
    /// it also updates the dueDate field in the DTO.
    /// </summary>
    public void UpdateDueDate(int taskId,int boardId, DateTime newDueDate)
    {
        
        taskController.UpdateDueDate(taskId, boardId, newDueDate);
        dueDate = newDueDate;
    }
    /// <summary>
    /// this method calls the UpdateColumnOrdinal in taskController
    /// it also updates the columnOrdinal field in the DTO.
    /// </summary>
    public void UpdateColumnOrdinal(int taskId,int boardId, int newColumnOrdinal)
    {
        taskController.UpdateColumnOrdinal(taskId, boardId, newColumnOrdinal);
        this.columnOrdinal = newColumnOrdinal;
    }
    /// <summary>
    /// this method calls the UpdateAssignee in taskController
    /// it also updates the assignee field in the DTO.
    /// </summary>
    public void UpdateAssignee(int taskId,int boardId, string newAssignee)
    {
        taskController.UpdateAssignee(taskId, boardId, newAssignee);
        this.assignee = newAssignee;
    }
    /// <summary>
    /// this method calls the Delete in taskController
    /// </summary>
    public void Delete()
    {
        taskController.Delete(this);
    }
    /// <summary>
    /// this method calls the DeleteAll in taskController
    /// </summary>
    public void DeleteAll()
    {
        taskController.DeleteAll();
    }
}