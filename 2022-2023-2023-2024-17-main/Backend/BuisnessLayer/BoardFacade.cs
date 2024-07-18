using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;

namespace IntroSE.Kanban.Backend.buisnessLayer;
/// <summary>
/// controlling class for boards.
/// /// the class has only one field:
/// a LinkedList of all the board entities created in the system. 
/// </summary>
public class BoardFacade
{
    private Dictionary<int,Board> boardsByID;
    private Dictionary<string, Dictionary<string, Board>> boardsByEmail;
    private UserFacade UF;
    private int boardCounter = 0;
    private BoardController boardController;
    private BoardMemberController boardMemberController;
    private ColumnController columnController;
    private TaskController taskController;
    private static readonly ILog log = 
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //-------------------methodes-----------------------//
    public Board GetBoardByID(int boardID)
    {
        if (boardsByID.ContainsKey(boardID))
        {
            return boardsByID[boardID];
        }
        return null;
    }
    public BoardFacade(UserFacade uf)
    {
        this.UF = uf;
        this.boardsByID = new Dictionary<int, Board>();
        this.boardsByEmail = new Dictionary<string, Dictionary<string, Board>>();
        this.boardController = new BoardController();
        this.boardMemberController = new BoardMemberController();
        this.columnController = new ColumnController();
        this.taskController = new TaskController();
    }
    /// <summary>
    /// the func makes sure that we can create a new board without
    /// breaking any restrictions and then creates a new board.
    /// </summary>
    /// <param name="email">the email of the user who initiated the action</param>
    /// <param name="boardName">the required board name for the new board</param>
    /// <exception cref="Exception">an exception if the board name is already in use for the specified user</exception>
    public void CreateBoard(string email, string boardName)
    {
        User u = UF.GetUser(email);
        if (u == null)
        {
            log.Info($"user {email} doesnt exist");
            throw new Exception($"user {email} doesnt exist");
        }
        if (!u.isLoggedIn())
        {
            log.Info($"user {email} isnt logged in");
            throw new Exception($"user {email} isnt logged in");
        }
        if (boardName== null || boardName.Length < 1)
        {
            log.Info($"denied illegal board name");
            throw new Exception("illegal boardName");
        }
        if (boardsByEmail[email].ContainsKey(boardName)) //GetBoard also check if user is logged in
        {
            log.Error("BoardNameTaken");
            throw new Exception("Board name taken");
        }
        Board boardtoAdd = new Board(boardName, email,this.boardCounter,boardController,
            columnController);
        boardsByEmail[email].Add(boardName,boardtoAdd);
        boardsByID.Add(boardCounter,boardtoAdd);
        u.addBoardToUser(boardCounter);
        new BoardMemberDTO(email, boardCounter, boardMemberController);
        this.boardCounter++;
        log.Info($"successfully created {boardName}");
    }
    /// <summary>
    /// the func makes sure that the user has a board with the specified name and then deletes it.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the required board name for the board to be deleted</param>
    /// <exception cref="Exception">an exception if there is no board with the specified name for the user</exception>
    public void DeleteBoard(string email, string boardName)
    {
        Board boardToDelete = GetBoard(email, boardName);   //GetBoard also check if user is logged in and board exist
        int boardid = boardToDelete.boardID;
        boardMemberController.DeleteBoard(boardid);
        boardToDelete.DeleteBoard();
        boardsByID.Remove(boardid);
        LinkedList<string> members = boardToDelete.members;
        foreach (string member in members)
        {
            UF.removeBoardFromUser(member,boardid);
            boardsByEmail[member].Remove(boardName);
        }
        log.Info($"successfully removed {boardName}");
        
    }
    /// <summary>
    /// a func that searches for a certain board of a certain user in all of the boards.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the required board name</param>
    /// <returns>returns the found board (if exists)</returns>
    /// <exception cref="Exception">throws an exception if the board does not exist</exception>
    public Board GetBoard(string email, string boardName) //returns board if exist and check if owner is logged in
    {
        User user = UF.GetUser(email);
        if (user == null)
        {
            log.Error("user not found");
            throw new Exception("user not found");
        }

        if (!user.isLoggedIn())
        {
            log.Error("user is not logged in");
            throw new Exception("user is not logged in");
        }
        Board board = BoardExistByUser(boardName, email);
        if (board == null)
        {
            log.Error("board does not exist");
            throw new Exception("board does not exist");
        }
        return board;
    }
    /// <summary>
    /// a func that adds a new task to a specific board.
    /// </summary>
    /// <param name="owner">the email of the user</param>
    /// <param name="boardName">the required board name to add the task to</param>
    /// <param name="title">the title of the new task</param>
    /// <param name="description">the description of the new task</param>
    /// <param name="duedate">the duedate for the new task</param>
    /// <exception cref="Exception">throw an exception if the board is not found</exception>
    public void AddTask(string owner, string boardName, string title, string description, DateTime duedate)
    {
        if (title.Length > 50 || title.Length < 1 )
        {
            log.Error("failed to add task, title length illegal");
            throw new Exception("title length must be between 1 to 50 characters");
        }

        if (description.Length > 300)
        {
            log.Error("failed to add task, description length illegal");
            throw new Exception("description length must be between under 300 characters");

        }
        Board board = GetBoard(owner, boardName);       //GetBoard check if user is logged in and board exist
        board.AddTask(owner, boardName, title, description, duedate,taskController);
    }
    /// <summary>
    /// advances a task from the backlog to in progress or from in progress to done.
    /// run the 'AdvanceTask' method in the right board.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the required board name to advance the task in</param>
    /// <param name="collumnOrdinal">the column in which the task is currently in</param>
    /// <param name="taskID">the id of the task to advance</param>
    public void AdvanceTask(string email,string boardName,int collumnOrdinal,int taskID)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        board.AdvanceTask(email,collumnOrdinal,taskID);
    }
    /// <summary>
    /// a func that updates a certain tasks due date.
    /// run the 'UpdateTaskDueDate' method in the right board.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the required board name to update the task in</param>
    /// <param name="columnOrdinal"></param>
    /// <param name="taskID">the column in which the task is currently in</param>
    /// <param name="dueDate">the new due date for the task</param>
    public void UpdateTaskDueDate(string email,string boardName,int columnOrdinal,int taskID,DateTime dueDate)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        board.UpdateTaskDueDate(email,columnOrdinal, taskID, dueDate);
    }
    /// <summary>
    /// a func that updates a certain tasks title.
    /// run the 'UpdateTaskTitle' method in the right board.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the required board name to update the task in</param>
    /// <param name="columnOrdinal"></param>
    /// <param name="taskID">the column in which the task is currently in</param>
    /// <param name="title">the new title for the task</param>
    public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskID, string title)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        board.UpdateTaskTitle(email,columnOrdinal, taskID, title);
    }
    /// <summary>
    /// a func that updates a certain tasks description.
    /// run the 'UpdateTaskDescription' method in the right board.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the required board name to update the task in</param>
    /// <param name="columnOrdinal"></param>
    /// <param name="taskID">the column in which the task is currently in</param>
    /// <param name="description">the new description for the task</param>
    public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskID, string description)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        board.UpdateTaskDescription(email,columnOrdinal, taskID, description);
    }
    /// <summary>
    /// a function that makes sure that a usr has a board with the specified name and returns it.
    /// </summary>
    /// <param name="boardName">the required board name to be checked</param>
    /// <param name="email">the email of the user who will be checked</param>
    /// <returns>returns the board (if found)</returns>
    private Board BoardExistByUser(string boardName, string email)
    {
        if (!boardsByEmail[email].ContainsKey(boardName))
        {
            return null;
        }
        return boardsByEmail[email][boardName];
    }
    /// <summary>
    /// a function that finds a user entity using his email address.
    /// </summary>
    /// <param name="userName">the email of the user to be found</param>
    /// <returns>returns the user entity that corresponds to the email </returns>
    public bool UserExist(string userName) { return UF.GetUser(userName) != null; }
    /// <summary>
    /// a func that limits the number of tasks in a certain column.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the board name of the board in which the column is to be limited </param>
    /// <param name="columnOrdinal">the column to be limited (out of the 3)</param>
    /// <param name="Limit">the limit to be set on the colmun</param>
    public void LimitColumn(string email, string boardName, int columnOrdinal,int Limit)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        board.LimitColumn(columnOrdinal, Limit);
    }
    /// <summary>
    /// a getter for a column limit.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the board on which the action had been initiated </param>
    /// <param name="columnOrdinal">the column to get the limit from</param>
    /// <returns>returns the limit in int form</returns>
    public int GetColumnLimit(string email, string boardName, int columnOrdinal)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        return board.GetColumnLimit(columnOrdinal);
    }
    /// <summary>
    /// a func that gets a whole column.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the board on which the action was initiated</param>
    /// <param name="columnOrdinal">the number of the column to be returned</param>
    /// <returns>returns the column</returns>
    public LinkedList<Task> GetColumn(string email, string boardName, int columnOrdinal)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        return (board.GetColumn(columnOrdinal));
    }
    /// <summary>
    /// a func that returns the column name (the options are:
    /// 1. backlog
    /// 2. in progress
    /// 3. done
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <param name="boardName">the board on which the action was initiated</param>
    /// <param name="columnOrdinal">the required columns name</param>
    /// <returns> a string with the columns name</returns>
    public string GetColumnName(string email, string boardName, int columnOrdinal)
    {
        Board board = GetBoard(email, boardName);//GetBoard check if user is logged in and board exist
        return board.GetColumnName(columnOrdinal);
    }
    /// <summary>
    /// a function that gets the board's ID number 
    /// </summary>
    /// <param name="email">the email of the owner</param>
    /// <param name="boardName">the board's name</param>
    /// <returns>int: the board's ID</returns>
    /// <exception cref="Exception">if the board was not found an exception will be thrown</exception>
    public int GetBoardID(string email, string boardName)
    {
        Board b = GetBoard(email, boardName); // check if user is logged in and board exist
        int ans = b.BoardID;
        return ans;

    }
    /// <summary>		  
    /// This method adds a user to the members list of a board.		 
    /// </summary>		 
    /// <param name="email">The email of the user. Must be logged in</param>		 
    /// <param name="boardID">The board's ID</param>
    /// <exception cref="Exception">throws an exception if the user is not logged in</exception>
    public void JoinBoard(string email, int boardID ,Board b )
    {
        if (b ==null)
        {
            log.Error("board doesnt exist");
            throw new Exception("board doesnt exist");
        }
        User u = UF.GetUser(email);
        if (u == null)
        {
            log.Error($"{email} isn't registered");
            throw new Exception($"{email} isn't registered");
        }
        if (!u.isLoggedIn())
        {
            log.Error("user is not logged in");
            throw new Exception("user is not logged in");
        }
        if (boardsByEmail[email].ContainsKey(b.Name))
        {
            log.Error($" {u.getEmail()} - already has a board with that name");
            throw new Exception($" {u.getEmail()} - already has a board with that name");
        }
        if (b.JoinBoard(email))
        {
            UF.addBoardToUser(email,boardID);
        }
        boardsByEmail[email].Add(b.Name,b);
        BoardMemberDTO add = new BoardMemberDTO(email, boardID,boardMemberController);
    }
    /// <summary>
    /// this function is a gateway function for the first joinBoard function which holds the logic
    /// </summary>
    /// <param name="email">the email of the user trying to join the board</param>
    /// <param name="boardID">the boardID of the board that the user is trying to join</param>
    public void JoinBoard(string email, int boardID)
    {
        this.JoinBoard(email,boardID,GetBoardByID(boardID));
    }
    /// <summary>		  
    /// This method removes a user from the members list of a board.		 
    /// </summary>		 
    /// <param name="email">The email of the user. Must be logged in</param>		 
    /// <param name="boardID">The board's ID</param>
    /// <exception cref="Exception">throws an exception if the user is not logged in</exception>
    /// <exception cref="Exception">throws an exception if the user is the owner</exception>
    public void LeaveBoard(string email, int boardID)
    {
        Board b = GetBoardByID(boardID);
        User u = UF.GetUser(email);
        if (u == null)
        {
            log.Error($" {email} - does not exist");
            throw new Exception($" {email} - does not exist");
        }
        if (b == null)
        {
            log.Error($"{boardID} - a board with this ID does not exist");
            throw new Exception($"{boardID} - a board with this ID does not exist");
        }
        if (!u.isLoggedIn())
        {
            log.Error("user is not logged in");
            throw new Exception("user is not logged in");
        }
        if (b.Owner==email)
        {
            log.Error("the owner cant leave the board");
            throw new Exception("the owner cant leave the board");
        }
        if (b.LeaveBoard(email))
        {
            UF.removeBoardFromUser(email, boardID);
        }
        boardsByEmail[email].Remove(b.Name);
        boardMemberController.DeleteOnUserLeaving(email, boardID);
    }
    /// <summary>
    /// this function returns the board's name.
    /// </summary>
    /// <param name="boardId">the boards unique id</param>
    /// <returns> a string with the boards name</returns>
    /// <exception cref="Exception">throws an exception if the board is not found using his ID</exception>
    public string GetBoardName(int boardId)
    {
        Board b = GetBoardByID(boardId);
        if (b==null)
        {
            log.Error("boardID does not exist");
            throw new Exception("boardID does not exist");
        }
        return b.GetBoardName();
    }
    /// <summary>
    /// this function transfers the ownership of the board to a different member of the board. 
    /// </summary>
    /// <param name="currentOwnerEmail">the email of the current owner</param>
    /// <param name="newOwnerEmail">the email of the owner to be</param>
    /// <param name="boardName">the name of the board.</param>
    /// <exception cref="Exception">throws an exception if the one trying to give ownership is not the owner</exception>
    public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
    {
        if (!boardsByEmail.ContainsKey(newOwnerEmail))
        {
            log.Error($"{newOwnerEmail} isnt register to the system");
            throw new Exception($"{newOwnerEmail} isnt register to the system");
        }
        if (!boardsByEmail[newOwnerEmail].ContainsKey(boardName))
        {
            log.Error("cannot give ownership, the new owner isn't a member in this board");
            throw new Exception("cannot give ownership, the new owner isn't a member in this board");
        }

        Board b = GetBoard(currentOwnerEmail, boardName);
        
        if (b.Owner==currentOwnerEmail)
        {
            b.SetOwner(newOwnerEmail);
        }
        else
        {
            log.Error("cannot give ownership if your not the owner");
            throw new Exception("cannot give ownership if your not the owner");
        }
    }
    /// <summary>
    /// a function to assign a new user to a task
    /// </summary>
    /// <param name="email">the email of the user assigning</param>
    /// <param name="boardName">the board's name</param>
    /// <param name="columnOrdinal">the column in which the task is</param>
    /// <param name="taskID">the task's unique ID number</param>
    /// <param name="emailAssignee">the email of the new assignee</param>
    public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
    {
        Board b = GetBoard(email, boardName); // check if user is logged in and board exist
        b.AssignTask(columnOrdinal, taskID, emailAssignee, email);
    }
    /// <summary>
    /// a func that returns ALL of the users 'in progress tasks from across all of his boards.
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <returns> returns a LinkedList containing all of the users 'in progress' tasks</returns>
    public LinkedList<Task> GetUserInProgressTask(string email)
    {
        LinkedList<Task> inProgressTasks = new LinkedList<Task>();
        LinkedList<int> userBoards = UF.GetUserBoards(email);
        foreach (int board in userBoards)
        {
            LinkedList<Task> toAdd = GetBoardByID(board).getInProgressTasks(email);
            foreach (Task task in toAdd)
            {
                inProgressTasks.AddLast(task);
            }
        }
        return inProgressTasks;
    }
    /// <summary>
    /// this function calls for the corresponding load function for each entity.
    /// the important thing to notice is the order in which the functions are called
    /// LoadUsers is first.
    /// LoadBoards is second so that the board would be able to hold the columns when the next function is called.
    /// LoadColumns is second so that the columns would be able to hold the tasks when the next function is called.
    /// </summary>
    public void LoadData()
    {
        UF.LoadUsers();
        LinkedList<User> users = UF.GetUsers();
        foreach (User u in users)
        {
            boardsByEmail.Add(u.getEmail(),new Dictionary<string, Board>());
        }
        LoadBoards();
        LoadColumns();
        LoadTasks();
        loadBoardMembers();
        this.boardCounter++;
    }
    /// <summary>
    /// this function uses a function from taskController that returns a list of all the task DTOs that it holds.
    /// it then calls the addTaskFromDTO function for each of the DTOs. 
    /// </summary>
    private void LoadTasks()
    {
        LinkedList<TaskDTO> taskDtos = taskController.getAllTasks();
        foreach (TaskDTO taskDTO in taskDtos)
        {
            taskDTO.setControler(taskController);
            addTaskFromDTO(taskDTO);
        }
    }
    /// <summary>
    /// this function finds the original board of the task and then calls the setTaskFromDTO method in that board.
    /// </summary>
    /// <param name="taskDto">the taskDto that hold the information for creating a task from its DTO</param>
    private void addTaskFromDTO(TaskDTO taskDto)
    {
        Board board = GetBoardByID(taskDto.BoardId);
        board.setTaskFromDTO(taskDto);
    }
    /// <summary>
    /// this function uses a function from columnController that returns a list of all the column DTOs that it holds.
    /// it then calls the addColumnFromDTO function for each of the DTOs. 
    /// </summary>
    private void LoadColumns()
    {
        LinkedList<ColumnDTO> columnDtos = columnController.GetAllColumns();
        foreach (ColumnDTO columnDTO in columnDtos)
        {
            columnDTO.setController(columnController);
            addColumnFromDTO(columnDTO);
        }
    }
    /// <summary>
    /// this function finds the original board of the column and then calls the setColumnFromDTO method in that board.
    /// </summary>
    /// <param name="columnDto">the columnDto that hold the information for creating a column from its DTO</param>
    private void addColumnFromDTO(ColumnDTO columnDto)
    {
        Board board = GetBoardByID(columnDto.BoardId);
        board.setColumnFromDTO(columnDto);
    }
    /// <summary>
    /// this function uses a function from boardController that returns a list of all the board DTOs that it holds.
    /// it then calls the addBoardFromDTO function for each of the DTOs. 
    /// </summary>
    private void LoadBoards()
    {
        LinkedList<BoardDTO> boardDtos = boardController.GetAllBoard();
        foreach (BoardDTO boardDto in boardDtos)
        {
            boardDto.setController(boardController);
            addBoardFromDTO(boardDto);
        } 
    }
    /// <summary>
    /// this function creates a new board entity from the DTO and adds it to the dictionaries holding the boards.
    /// it then counts the boards. 
    /// </summary>
    /// <param name="boardDto">the boardDto that hold the information for creating a board from its DTO</param>
    private void addBoardFromDTO(BoardDTO boardDto)
    {
        Board boardtoAdd = new Board(boardDto);
        boardCounter = Math.Max(boardCounter, boardtoAdd.boardID);
        boardsByID[boardDto.Id] = boardtoAdd;
        boardsByEmail[boardDto.OwnerEmail][boardDto.Name] = boardtoAdd;
    }
    /// <summary>
    /// 
    /// </summary>
    public void DeleteData()
    {		 
        UF.DeleteData();
        boardController.DeleteAll();
        boardMemberController.DeleteAll();
        columnController.DeleteAll();
        taskController.DeleteAll();
    }
    /// <summary>
    /// this function calls the register function in user facade and adds the email to the boardsByEmail dictionary.
    /// </summary>
    /// <param name="email">the email for the registration</param>
    /// <param name="password">the password that the users sets for he account </param>
    public void Register(string email, string password)
    {
        UF.Register(email,password);
        boardsByEmail.Add(email,new Dictionary<string, Board>());
    }
    /// <summary>
    /// this function updates the board members for each board and boards for each user after load data. 
    /// </summary>
    private void loadBoardMembers()
    {
        LinkedList<BoardMemberDTO> boardMemberDtos = boardMemberController.GetAllBoardMembers();
        foreach (BoardMemberDTO boardMemberDto in boardMemberDtos)
        {
            Board b = GetBoardByID(boardMemberDto.BoardId);
            b.JoinBoard(boardMemberDto.Email);
            UF.addBoardToUser(boardMemberDto.Email, boardMemberDto.BoardId);
        }
    }
}