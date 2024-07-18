using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer;

public class BoardDTO
{
    public const string BOARDNAME = "NAME";
    public const string BOARDID = "ID";
    public const string OWNEREMAIL = "OWNER";
    public const string TASKCOUNTER = "TASKCOUNTER";
    
    private string name;
    private int id;
    private string ownerEmail;
    private int taskCounter;
    private BoardController boardController;
    /// <summary>
    /// a constructor method for the BoardDTO 
    /// </summary>
    /// <param name="name">the board's name</param>
    /// <param name="id">the board's id</param>
    /// <param name="ownerEmail">the board's owner email</param>
    /// <param name="taskCounter">the board's task counter</param>
    /// <param name="boardController">the boardController</param>
    public BoardDTO(string name, int id, string ownerEmail, int taskCounter,BoardController boardController)
    {
        this.name = name;
        this.id = id;
        this.ownerEmail = ownerEmail;
        this.taskCounter = taskCounter;
        this.boardController = boardController;
        Insert();
    }
    /// <summary>
    /// a constructor method for the BoardDTO 
    /// </summary>
    /// <param name="name">the board's name</param>
    /// <param name="id">the board's id</param>
    /// <param name="ownerEmail">the board's owner email</param>
    /// <param name="taskCounter">the board's task counter</param>
    public BoardDTO(string name, int id, string ownerEmail, int taskCounter)
    {       
        this.name = name;
        this.id = id;
        this.ownerEmail = ownerEmail;
        this.taskCounter = taskCounter;
    }
    /// <summary>
    /// a setter for the boardController
    /// </summary>
    /// <param name="boardController">the boardController</param>
    public void setController(BoardController boardController)
    {
        this.boardController = boardController;
    }

    public string Name { get => name; }
    public int Id { get => id; }
    public string OwnerEmail { get => ownerEmail; }
    public int TastCounter { get => taskCounter; }
    /// <summary>
    /// this method calls the getAllBoard in boardController
    /// </summary>
    /// <returns>returns a linked-list containing all the board DTOs</returns>
    public LinkedList<BoardDTO> getAllBoards()
    {
        return boardController.GetAllBoard();
    }
    /// <summary>
    /// this method calls the Insert in boardController
    /// </summary>
    public void Insert()
    {
        boardController.Insert(this);
    }
    /// <summary>
    /// this method calls the UpdateOwner in boardController
    /// it also updates the ownerEmail field in the DTO.
    /// </summary>
    public void UpdateOwner(int boardId, string newOwner)
    {
        boardController.UpdateOwner(boardId, newOwner);
        this.ownerEmail = newOwner;

    }
    /// <summary>
    /// this method calls the UpdateTaskCounter in boardController
    /// it also updates the taskCounter field in the DTO.
    /// </summary>
    public void UpdateTaskCounter(int boardId, int newTaskCounter)
    {
        boardController.UpdateTaskCounter(boardId, newTaskCounter);
        this.taskCounter = newTaskCounter;

    }
    /// <summary>
    /// this method calls the Delete in boardController
    /// </summary>
    public void Delete()
    {
        boardController.Delete(this);
    }
}