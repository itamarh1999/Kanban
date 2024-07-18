namespace IntroSE.Kanban.Backend.DataAccessLayer;

public class BoardMemberDTO
{
    public const string EMAIL = "EMAIL";
    public const string BOARDID = "BOARDID";
    private string email;
    private int boardID;
    private BoardMemberController boardMemberController;
    /// <summary>
    /// a constructor method for the BoardMemberDTO.
    /// </summary>
    /// <param name="email">the email of the user that was singed t this board</param>
    /// <param name="boardId">the board id of the board that the user has been assigned to</param>
    /// <param name="boardMemberController">the boardMemberController</param>
    public BoardMemberDTO(string email, int boardId,BoardMemberController boardMemberController)
    {
        this.email = email;
        boardID = boardId;
        this.boardMemberController = boardMemberController;
        Insert();
    }
    /// <summary>
    /// a constructor method for the BoardMemberDTO.
    /// </summary>
    /// <param name="email">the email of the user that was singed t this board</param>
    /// <param name="boardId">the board id of the board that the user has been assigned to</param>
    public BoardMemberDTO(string email, int boardId)
    {
        this.email = email;
        boardID = boardId;
    }
    /// <summary>
    /// this method calls the Insert in boardMemberController
    /// </summary>
    public void setController(BoardMemberController boardMemberController)
    {
        this.boardMemberController = boardMemberController;
    }
    /// <summary>
    /// a setter for the email field
    /// </summary>
    public string Email => email;
    /// <summary>
    /// a setter for the BoardId field
    /// </summary>
    public int BoardId => boardID;
    /// <summary>
    /// this function calls for the Insert function in boardMemberController
    /// </summary>
    public void Insert()
    {
        boardMemberController.Insert(this);
    }
    
}