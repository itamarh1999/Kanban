using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.buisnessLayer;
/// <summary>
/// this class represents a single entity of a column.
/// a column has the following attributes:
/// int: maxTasks - represents the max limit for task that can be in the column.
/// LinkedList of Tasks: tasks - holds the Task entities assigned to the column.
/// 
/// </summary>
public class Column
{
    private int maxTasks;
    private LinkedList<Task> tasks;
    private const int defaultLimit = -1;
    private ColumnDTO columnDTO;
    private static readonly ILog log = 
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    /// <summary>
    /// a getter/setter for the max limit of tasks that can be assigned to the column. 
    /// </summary>
    public int MaxTasks
    {
        get => maxTasks;
        set => maxTasks = value;
    }
    /// <summary>
    /// a getter/setter for the tasks assigned to the column.
    /// </summary>
    public LinkedList<Task> Tasks
    {
        get => tasks;
        set => tasks = value;
    }
    /// <summary>
    /// a constructor for a new column 
    /// </summary>
    /// <param name="boardID">the id of board that holds the column</param>
    /// <param name="columnOrdinal">the number that represents the column (0-backLog,1-inProgress,2-done)</param>
    /// <param name="columnController">the columnController which is given to the columnDTO constructor</param>
    public Column(int boardID, int columnOrdinal,ColumnController columnController)
    {
        this.maxTasks = defaultLimit;
        this.tasks = new LinkedList<Task>();
        this.columnDTO = new ColumnDTO(boardID, columnOrdinal, -1,columnController);
    }
    public Column()
    {
    }
    public Column(ColumnDTO columnDto)
    {
        this.maxTasks = columnDto.MaxTasks;
        this.tasks = new LinkedList<Task>();
        this.columnDTO = columnDto;
    }

    /// <summary>
    /// a constructor for a column from a DTO 
    /// </summary>
    /// <param name="boardID">the id of board that holds the column</param>
    /// <param name="columnOrdinal">the number that represents the column (0-backLog,1-inProgress,2-done)</param>
    /// <param name="limit">the existing limit for the column (-1 is the default)</param>
    /// <param name="columnController">the columnController which is given to the columnDTO constructor</param>
    public Column(int boardID, int columnOrdinal, int limit, ColumnController columnController)
    {
        this.maxTasks = limit;
        this.tasks = new LinkedList<Task>();
        this.columnDTO = new ColumnDTO(boardID, columnOrdinal, limit,columnController);
    }
    /// <summary>
    /// a constructor with a task limit.
    /// </summary>
    /// <param name="maxTasks">the task limit</param>
    public Column(int maxTasks)
    {
        this.maxTasks = maxTasks;
        this.tasks = new  LinkedList<Task>();
    }
    /// <summary>
    /// a setter for the max number of tasks that can be hold in this column.
    /// </summary>
    /// <param name="limit">int: the limit</param>
    /// <exception cref="Exception">throws an exception if the new limit to set is lower than the current number
    /// of tasks assigned to the column</exception>
    public void SetLimit(int limit, int boardID,int columnOrdinal)
    {
        if (tasks.Count > limit)
        {
            log.Error("cant limit, has too many tasks ");
            throw new Exception("limit is too low theres already too many tasks");
        }

        if (limit<0)
        {
            log.Error($"setLimit failed cant limit to {limit}");
            throw new Exception($"setLimit failed cant limit to {limit}");
        }

        if (tasks.Count >limit)
        {
            log.Error($"setLimit failed cant limit to {limit} there are too many tasks");
            throw new Exception($"setLimit failed cant limit to {limit} there are too many tasks");
        }
        log.Info($"successfully limit column to {limit}");
        this.maxTasks = limit;
        this.columnDTO.UpdateMaxTasks(boardID,columnOrdinal,limit);
    }
    /// <summary>
    /// a getter for the column's limit
    /// </summary>
    /// <returns>int: the current limit</returns>
    public int GetLimit()
    {
        return this.maxTasks;
    }
    /// <summary>
    /// a function for removing a task from the column.
    /// </summary>
    /// <param name="taskID">the ID of the wanted task </param>
    /// <returns>returns the removed task. if the task was not found in the column will return null</returns>
    public Task RemoveTask(int taskID)
    {
        foreach (Task task in tasks)
        {
            if (taskID == task.getTaskID())
            {
                tasks.Remove(task);
                log.Info($"task {taskID} has been removed successfully from column");
                return task;
            }
        }
        return null;
    }
    /// <summary>
    /// a function that checks if the task is in the column using the tasks uniq ID.
    /// </summary>
    /// <param name="taskID">int: the uniq task ID</param>
    /// <returns> a boolean value. true if the task is in the column, false otherwise.</returns>
    public bool HasTask(int taskID)
    {
        foreach (Task task in tasks)
        {
            if (taskID == task.getTaskID())
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// a getter for all the tasks in the column.
    /// </summary>
    /// <returns> returns a linked list of all the tasks assigned to the column</returns>
    public LinkedList<Task> GetTasks()
    {
        return tasks;
    }
    /// <summary>
    /// this function traverses the second colums (in progress) and checks the assignee for each task.
    /// if the assignee is the user who initiated the action that the task will be returned in the list. 
    /// </summary>
    /// <param name="email">the email of the user who asked for the tasks</param>
    /// <returns>a list containing all of the tasks the user sis assigned to</returns>
    public LinkedList<Task> getInProgressTasks(string email)
    {
        LinkedList<Task> ans = new LinkedList<Task>();
        foreach (Task task in tasks)
        {
            if (task.Assignee == email)
            {
                ans.AddLast(task);
            }
        }
        return ans;
    }
    public List<Task> GetTasksList()
    {
        List<Task> ans = new List<Task>();
        foreach (Task task in tasks)
        {
            ans.Add(task);
        }
        return ans;
    }
    //------------methods-----------
    /// <summary>
    /// a function that creates a new task and adds the task to the column
    /// (will exclusively be used on the backlog column). 
    /// </summary>
    /// <param name="taskID">int: the unique ID given by the board task counter </param>
    /// <param name="title">string: the title for the task</param>
    /// <param name="description">string: the description for the new task</param>
    /// <param name="duedate">DateTime: the due date for the new task</param>
    public void AddTask(int taskID, string owner, string boardName, string title,
        string description,DateTime duedate,int boardID,TaskController taskController)
    {
        if (maxTasks != defaultLimit && tasks.Count >= maxTasks)
        {
            log.Error("addTask failed too many tasks");
            throw new Exception("to much tasks in the column");
        }
        Task taskToAdd = new Task(taskID,owner,boardName,title,description,duedate,boardID,taskController);
        tasks.AddLast(taskToAdd); 
    }
    /// <summary>
    /// a function that adds an existing task to the column.
    /// mainly used to transfer tasks between columns.
    /// </summary>
    /// <param name="task">the Task entity to be added to the column</param>
    /// <exception cref="Exception">throws an exception if the new task to be added exceeds the
    /// max number of task for the column.</exception>
    public void AddTask(Task task)
    {
        if (maxTasks != defaultLimit && tasks.Count >= maxTasks)
        {
            log.Error("addTask failed too many tasks");
            throw new Exception("to much tasks in the column");
            
        }
        tasks.AddLast(task);
    }
    /// <summary>
    /// a getter for a specific task in the column.
    /// </summary>
    /// <param name="taskID">int: the task's unique ID</param>
    /// <returns>returns the task (if found)</returns>
    /// <exception cref="Exception">throw an exception if the task is not in the column</exception>
    public Task GetTask(int taskID)
    {
        foreach (Task task in tasks)
        {
            if (task.getTaskID() == taskID) 
                return task;
        }
        log.Error($"getTask failed task {taskID} is not in this column");
        throw new Exception("task is not in this column");
    }
    /// <summary>
    /// finds the tasks to be updated and runs the method 'updateTaskDueDate' of the class Task.
    /// </summary>
    /// <param name="taskID">int: the tasks unique ID</param>
    /// <param name="dueDate">the new due-date to update</param>
    public void UpdateTaskDueDate(int taskID, DateTime dueDate, int boardID , string email)
    {
        Task task = GetTask(taskID);
        task.updateTaskDueDate(dueDate,boardID,email);
    }
    /// <summary>
    /// finds the tasks to be updated and runs the method 'updateTaskTitle' of the class Task.
    /// </summary>
    /// <param name="taskID">int: the tasks unique ID</param>
    /// <param name="title">the new title to update</param>
    public void UpdateTaskTitle(int taskID, string title,int boardID,string email)
    {
        Task task = GetTask(taskID);
        task.updateTaskTitle(title,boardID,email);
    }
    /// <summary>
    /// finds the tasks to be updated and runs the method 'updateTaskDescription' of the class Task.
    /// </summary>
    /// <param name="taskID">int: the tasks unique ID</param>
    /// <param name="Description">the new Description to update</param>
    public void UpdateTaskDescription(int taskID, string Description, int boardID)
    {
        Task task = GetTask(taskID);
        task.updateTaskDescription(Description, boardID);
    }
    /// <summary>
    /// this function finds the right task in the column and runs the assignTask function in the specific task. 
    /// </summary>
    /// <param name="taskID">the unique task ID</param>
    /// <param name="emailAssignee">the email of the assignee</param>
    public void AssignTask(int taskID, string emailAssignee, int boardID, string emailAssigner)
    {
        GetTask(taskID).AssignTask(emailAssignee,boardID,emailAssigner);
    }
    /// <summary>
    /// this function traverses through all of the tasks in the column and removes the assignee for each task
    /// that has the user assigned to it.
    /// </summary>
    /// <param name="email">the user's email to remove</param>
    public void RemoveAssignee(string email)
    {
        foreach (Task task in Tasks)
        {
            task.RemoveAssignee(email);
        }
    }
    /// <summary>
    /// this function creates a new task entity and adds it to the column using the information from the DTO
    /// </summary>
    /// <param name="taskDto">the task DTO that contains the information required for creating a task</param>
    /// <param name="taskController">the taskController which is given to the new taskDto</param>

    public void setTaskFromDTO(TaskDTO taskDto)
    {
        this.tasks.AddLast(new Task(taskDto));
    }
}