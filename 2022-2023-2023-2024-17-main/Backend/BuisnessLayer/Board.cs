using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.buisnessLayer;
/// <summary>
/// this class represents a single entity of a board.
/// a board has the following attributes:
/// string: name - is the title of the board
/// string: owner - the email of the creating user
/// an array of 3 columns (0-backlog, 1-in progress, 2-done)
/// int: taskCounter - is used to distribute uniq taskID's
/// 
/// </summary>
public class Board
{
    private const int deafualtTaskCounter = 0;
    public int boardID;
    public string name;
    public string owner;
    public Column[] columns;
    public int taskCounter;
    public LinkedList<string> members;
    public BoardDTO boardDTO;
    public static readonly ILog log = 
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    

    public string Name
    {
        get => name; 
    }
    public string Owner
    {
        get => owner;
    }
    public Column[] Columns
    {
        get => columns;
    }
    public int TaskCounter
    {
        get => taskCounter;
    }
    public int BoardID
    {
        get => boardID;
    }

    public Board(string name, string owner,int boardID,BoardController boardController,
        ColumnController columnController)
    {
        this.name = name;
        this.owner = owner;
        this.boardID = boardID;
        this.members = new LinkedList<string>();
        members.AddFirst(owner);
        this.columns = new Column[3];
        columns[0] = new Column(this.boardID,0, columnController);
        columns[1] = new Column(this.boardID,1, columnController);
        columns[2] = new Column(this.boardID,2, columnController);
        taskCounter = 0;
        this.boardDTO = new BoardDTO(name,boardID,owner,taskCounter,boardController);
    }
    public Board(BoardDTO boardDto)
    {
        this.name = boardDto.Name;
        this.owner = boardDto.OwnerEmail;
        this.members = new LinkedList<string>();
        this.columns = new Column[3];
        this.boardID = boardDto.Id;
        this.taskCounter = boardDto.TastCounter;
        this.boardDTO = boardDto;
    }
    /// <summary>
    /// a function that checks if the user is registered to the board
    /// </summary>
    /// <param name="email">the email to check</param>
    /// <returns>true or false</returns>
    public bool hasMember(string email)
    {
        return members.Contains(email);
    }
    //-----------methods-----------
    /// <summary>
    /// a getter for the creator's email
    /// </summary>
    /// <returns>the creator's email</returns>
    public string getOwner()
    {
        return owner;
    }
    /// <summary>
    /// sends the data to the backlog column with a uniq ID given by the taskCounter.
    /// </summary>
    /// <param name="title">the title for the new task</param>
    /// <param name="description">the description for the new task</param>
    /// <param name="duedate">the due-date for the new task</param>
    public void AddTask(string owner, string boardName, string title, string description,
        DateTime duedate, TaskController taskController)
    {
        columns[0].AddTask(taskCounter, owner, boardName, title, description, duedate,
            this.boardID,taskController);
        taskCounter++;
        this.boardDTO.UpdateTaskCounter(boardID,taskCounter);
        log.Info($"task {title} has been added successfully added to column 'BackLog'");

    }
    /// <summary>
    /// a function that checks that the parameters for the column ordinal are correct and sends the limit to the right
    /// column for limiting (the column will limit itself using the 'SetLimit' function).
    /// </summary>
    /// <param name="columnOrdinal">an int representing the desired column to limit</param>
    /// <param name="limit">the desired limit for the column</param>
    /// <returns>a true or false value for if the function worked failed</returns>
    /// <exception cref="Exception">throws an exception if the user is not logged in at the time of the command</exception>
    public bool LimitColumn(int columnOrdinal, int limit)
    {
        if (columnOrdinal > 1 | columnOrdinal < 0)
        {
            throw new Exception("not a valid columnordinal");
        }
        if (columnOrdinal >= 0 && columnOrdinal <= 2)
        {
            columns[columnOrdinal].SetLimit(limit, this.boardID, columnOrdinal);
            return true;
        }
        return false;
    }
    /// <summary>
    /// a function that moves a task through the progression columns
    /// either from "backlog" to "in progress" or from "in progress" to "done".
    /// </summary>
    /// <param name="collumnOrdinal">an int representing the desired column to limit</param>
    /// <param name="taskID">an int representing the task using it's task ID</param>
    /// <exception cref="Exception">throws an exception if the task was not found in the given column</exception>
    /// <exception cref="Exception">throws an exception if the user trying to advance the task is not the assignee</exception>
    public void AdvanceTask(string email, int collumnOrdinal,int taskID)
    {
        if (collumnOrdinal > 1 | collumnOrdinal < 0)
        {
            log.Error($"advance task failed because columnOrdinal set to illegal value - {collumnOrdinal}");
            throw new Exception("not column to advance to");
        }
        if (!columns[collumnOrdinal].HasTask(taskID))
        {
            log.Error($"advance task failed because task dont exist");
            throw new Exception($"no such task- task {taskID} ");
        }
        if (email != columns[collumnOrdinal].GetTask(taskID).Assignee)
        {
            log.Error("only the assignee can advance the task");
            throw new Exception("only the assignee can advance the task");
        }
        Task task = columns[collumnOrdinal].RemoveTask(taskID);
        task.AdvanceTask(collumnOrdinal + 1,this.boardID);
        if (collumnOrdinal != 2 )
            columns[collumnOrdinal + 1].AddTask(task);
        log.Info($"task {taskID} has been advanced successfully from column {collumnOrdinal} to {collumnOrdinal+1}");
    }
    /// <summary>
    /// the function receives the desired column ordinal and sends it back.
    /// </summary>
    /// <param name="columnOrdinal">an int representing the desired column to limit</param>
    /// <returns>returns the desired column</returns>
    public LinkedList<Task> GetColumn(int columnOrdinal)
    {
        if (columnOrdinal > 2 | columnOrdinal < 0)
        {
            log.Error($"illegal collumn ordinal - {columnOrdinal}");
            throw new Exception("not a valid columnordinal");
        }
        return columns[columnOrdinal].GetTasks();
    }
    /// <summary>
    /// a function that returns the limit of a desired column.
    /// </summary>
    /// <param name="columnOrdinal">an int representing the desired column to limit</param>
    /// <returns>int: the limit of the chosen column</returns>
    public int GetColumnLimit(int columnOrdinal)
    {
        if (columnOrdinal > 2 | columnOrdinal < 0)
        {
            log.Error($"illegal collumn ordinal - {columnOrdinal}");
            throw new Exception("not a valid columnordinal");
        }
        return columns[columnOrdinal].GetLimit();
    }
    /// <summary>
    /// a function that returns the name of a desired column.
    /// </summary>
    /// <param name="columnOrdinal">an int representing the desired column to limit</param>
    /// <returns>string: the name of the chosen column</returns>
    /// <exception cref="Exception">throws an exception if the column ordinal is not 0,1,2 </exception>
    public string GetColumnName(int columnOrdinal)
    { 
        if (columnOrdinal == 0)
        {
            return "backlog";
        }
        if (columnOrdinal == 1)
        {
            return "in progress";
        }
        if (columnOrdinal == 2)
        {
            return "done";
        }
        log.Error($"illegal collumn ordinal - {columnOrdinal}");
        throw new Exception("not one of the columns");
        
    }
    /// <summary>
    /// currently unused - DO NOT TRY TO RUN.
    /// </summary>
    public Task RemoveTask(int taskID, int columnOrdinal)
    { 
        if (columnOrdinal > 2 | columnOrdinal < 0)
        {
            log.Error($"illegal collumn ordinal - {columnOrdinal}");
            throw new Exception("not a valid columnordinal");
        }
        log.Info($"task {taskID} has been removed successfully from column{columnOrdinal}");
        return (columns[columnOrdinal].RemoveTask(taskID));
    }
    /// <summary>
    /// runs the GetTasks function of the in progress column.
    /// </summary>
    /// <returns>returns a LinkedList of the tasks in the "in progress" column</returns>
    public LinkedList<Task> getInProgressTasks(string email)
    {
        return columns[1].getInProgressTasks(email);
    }
    /// <summary>
    /// the function returns the current board name.
    /// </summary>
    /// <returns>string: containing the boards name</returns>
    public string GetBoardName()
    {
        return this.name;
    }
    /// <summary>
    /// the function runs the UpdateTaskDueDate function of the desired column
    /// (given that the task was found inside the column)
    /// with the taskID and the new dueDate. 
    /// </summary>
    /// <param name="columnOrdinal">an int representing the column in which the task is</param>
    /// <param name="taskID">an int representing the task using it's taskID</param>
    /// <param name="dueDate">the new due-date for the task</param>
    /// <exception cref="Exception">throws an exception if the task was not found in the given column</exception>
    /// <exception cref="Exception">throws an exception if the user trying to update the task is not the assignee</exception>
    public void UpdateTaskDueDate(string email, int columnOrdinal, int taskID, DateTime dueDate)
    {
        if (columnOrdinal > 1 | columnOrdinal < 0)
        {
            log.Error($"illegal collumn ordinal - {columnOrdinal}");
            throw new Exception("not a valid columnordinal");
        }
        if (!columns[columnOrdinal].HasTask(taskID)){
            log.Error($"cant update - no such task - taskID : {taskID}");
            throw new Exception("no such task ");
        }
        columns[columnOrdinal].UpdateTaskDueDate(taskID, dueDate,BoardID,email);
        log.Info($"task {taskID} dueDate's has been updated successfully to {dueDate.ToShortDateString()} ");

    }
    /// <summary>
    /// the function runs the UpdateTaskTitle function of the desired column
    /// (given that the task was found inside the column)
    /// with the taskID and the new title. 
    /// </summary>
    /// <param name="columnOrdinal">an int representing the column in which the task is</param>
    /// <param name="taskID">an int representing the task using it's taskID</param>
    /// <param name="title">the new title for the task</param>
    /// <exception cref="Exception">throws an exception if the task was not found in the given column</exception>
    /// <exception cref="Exception">throws an exception if the user trying to update the task is not the assignee</exception>
    public void UpdateTaskTitle(string email, int columnOrdinal, int taskID, string title)
    {
        if (columnOrdinal > 1 | columnOrdinal < 0)
        {
            log.Error($"illegal collumn ordinal - {columnOrdinal}");
            throw new Exception("not a valid columnordinal");
        }

        if (!columns[columnOrdinal].HasTask(taskID)){
            log.Error($"cant update - no such task - taskID : {taskID}");
            throw new Exception("no such task ");
        }
        if (email != columns[columnOrdinal].GetTask(taskID).Assignee)
        {
            log.Error("only the assignee can update the task");
            throw new Exception("only the assignee can update the task");
        }
        columns[columnOrdinal].UpdateTaskTitle(taskID, title,this.boardID,email);
        log.Info($"task {taskID} title's has been updated successfully to {title} ");
    }
    /// <summary>
    /// the function runs the UpdateTaskDescription function of the desired column
    /// (given that the task was found inside the column)
    /// with the taskID and the new description. 
    /// </summary>
    /// <param name="columnOrdinal">an int representing the column in which the task is</param>
    /// <param name="taskID">an int representing the task using it's taskID</param>
    /// <param name="description">the new description for the task</param>
    /// <exception cref="Exception">throws an exception if the task was not found in the given column</exception>
    /// <exception cref="Exception">throws an exception if the user trying to update the task is not the assignee</exception>
    public void UpdateTaskDescription(string email, int columnOrdinal, int taskID, string description)
    {
        if (columnOrdinal > 1 | columnOrdinal < 0)
        {
            log.Error($"illegal collumn ordinal - {columnOrdinal}");
            throw new Exception("not a valid columnordinal");
        }
        if (!columns[columnOrdinal].HasTask(taskID)){
            log.Error($"cant update - no such task - taskID : {taskID}");
            throw new Exception("no such task ");
        }
        if (description == null)
        {
            log.Error("Description can't be Null");
            throw new Exception("Description can't be Null");
        }
        if (email != columns[columnOrdinal].GetTask(taskID).Assignee)
        {
            log.Error("only the assignee can update the task");
            throw new Exception("only the assignee can update the task");
        }
        columns[columnOrdinal].UpdateTaskDescription(taskID, description, this.boardID);
        log.Info($"task {taskID} description's has been updated successfully to {description} ");
    }
    /// <summary>
    /// a function that assigns a new assignee to a task
    /// </summary>
    /// <param name="columnOrdinal">the column in which the task is</param>
    /// <param name="taskID">the tasks uniques id</param>
    /// <param name="emailAssignee">the email of the assigned user</param>
    /// <exception cref="Exception">throws an exception if the task was not in the column</exception>
    public void AssignTask(int columnOrdinal, int taskID, string emailAssignee, string emailAssigner)
    {
        
        if (!this.hasMember(emailAssignee))//check if the one who assignee is a member
        {
            log.Error("assignee is not registered to the board");
            throw new Exception("assignee is not registered to the board");
        }
        if (!this.hasMember(emailAssigner))//check if the one who assigner is a member
        {
            log.Error("assigner is not registered to the board");
            throw new Exception("assigner is not registered to the board");
        }
        if (columns[columnOrdinal].HasTask(taskID))
        {
            columns[columnOrdinal].AssignTask(taskID, emailAssignee, this.boardID, emailAssigner);
        }
        else
        {
            log.Error("task not in this colimn");
            throw new Exception("task not in this column");
        }
    }
    /// <summary>
    /// a function that registers a user to a board.
    /// </summary>
    /// <param name="email">the  email to be added</param>
    /// <exception cref="Exception">throw an exception if the user is already in the board's list</exception>
    public bool JoinBoard(string email)
    {
        if (members.Contains(email))
        {
            log.Error("user already registered to this board");
            throw new Exception("user already registered to this board");
        }
        members.AddLast(email);
        return true;
    }
    /// <summary>
    /// This method removes a user from the members list of a board.
    /// the function also leaves any tasks that had the user's assigned to them without an assignee.
    /// </summary>
    /// <param name="email">The email of the user</param>
    /// <exception cref="Exception">throws an exception if the user is not registered to this board</exception>
    public bool LeaveBoard(string email)
    {
        if (!members.Contains(email))
        {
            log.Error("user is not registered to this board");
            throw new Exception("user is not registered to this board");
        }
        members.Remove(email);
        foreach (Column column in columns)
        {
            column.RemoveAssignee(email);
        }
        return true;
    }
    /// <summary>
    /// a setter for owner
    /// </summary>
    /// <param name="newOwner">the new owner to set</param>
    public void SetOwner(string newOwner)
    {
        if (members.Contains(newOwner))
        {
            this.owner = newOwner;
            boardDTO.UpdateOwner(this.boardID,newOwner);
        }
        else
        {
            log.Error("cannot give ownership, the new owner isn't a member in this board");
            throw new Exception("cannot give ownership, the new owner isn't a member in this board");
        }
    }
    /// <summary>
    /// this function calls for the delete function in boardDTO in order to delete it
    /// </summary>
    public void DeleteBoard()
    {
        this.boardDTO.Delete();
    }
    /// <summary>
    /// this functoin sets a new column object from its dto
    /// </summary>
    /// <param name="columnDto">the DTO that contains all the relevant information (except columnController) for creating a column entity</param>
    /// <param name="columnController">the controller is also a parameter used in creating a column</param>
    public void setColumnFromDTO(ColumnDTO columnDto)
    {
        this.columns[columnDto.ColumnOrdinal] = 
            new Column(columnDto);
    }
    /// <summary>
    /// this function finds the right column to add the task to and calls
    /// the setTaskFromDTO function inside the specific column
    /// </summary>
    /// <param name="taskDto">the task DTO that holds the relevant information to use when creating a task from a DTO</param>
    /// <param name="taskController">the controller is also a parameter used in creating a task</param>
    public void setTaskFromDTO(TaskDTO taskDto)
    {
        columns[taskDto.ColumnOrdinal].setTaskFromDTO(taskDto);
    }
    
}