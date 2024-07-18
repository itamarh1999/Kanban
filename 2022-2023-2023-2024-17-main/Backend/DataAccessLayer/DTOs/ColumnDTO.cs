using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer;

public class ColumnDTO
{
    public const string BOARDID = "BOARDID";
    public const string COLUMNORDINAL = "COLUMNORDINAL";
    public const string MAXTASKS = "MAXTASKS";
    
    private int boardID;
    private int columnOrdinal;
    private int maxTasks;
    private ColumnController columnController;
    /// <summary>
    /// a constructor method for the BoardDTO
    /// </summary>
    /// <param name="boardId">the board's id</param>
    /// <param name="columnOrdinal">the column's ordinal (0-backlog,1-inProgress,2-done)</param>
    /// <param name="maxTasks">the limit of tasks for this column (-1 i default)</param>
    /// <param name="columnController">the columnController</param>
    public ColumnDTO(int boardId, int columnOrdinal, int maxTasks,ColumnController columnController)
    {
        this.boardID = boardId;
        this.columnOrdinal = columnOrdinal;
        this.maxTasks = maxTasks;
        this.columnController = columnController;
        Insert();
    }
    /// <summary>
    /// a constructor method for the BoardDTO
    /// </summary>
    /// <param name="boardId">the board's id</param>
    /// <param name="columnOrdinal">he column's ordinal (0-backlog,1-inProgress,2-done)</param>
    /// <param name="maxTasks">the limit of tasks for this column (-1 i default)</param>
    public ColumnDTO(int boardId, int columnOrdinal, int maxTasks)
    {
        this.boardID = boardId;
        this.columnOrdinal = columnOrdinal;
        this.maxTasks = maxTasks;
    }
    /// <summary>
    /// a setter for columnController
    /// </summary>
    /// <param name="columnController">the columnController</param>
    public void setController(ColumnController columnController)
    {
        this.columnController = columnController;
    }
    /// <summary>
    ///  a getter for boardID
    /// </summary>
    public int BoardId {get=> boardID;}
    /// <summary>
    ///  a getter for columnOrdinal
    /// </summary>
    public int ColumnOrdinal {get=> columnOrdinal;}
    /// <summary>
    ///  a getter for maxTasks
    /// </summary>
    public int MaxTasks {get=> maxTasks;}
    /// <summary>
    /// this function gets all the column DTOs 
    /// </summary>
    /// <returns> returns a list of all the column DTOs</returns>
    public LinkedList<ColumnDTO> getAllColumns()
    {
        return columnController.GetAllColumns();
    }
    /// <summary>
    /// this function calls for the Insert function in columnController
    /// </summary>
    public void Insert()
    {
        columnController.Insert(this);
    }
    /// <summary>
    /// this function updates the max tasks of a column.
    /// </summary>
    /// <param name="boardId">the board's id</param>
    /// <param name="columnOrdinal">the column's ordinal (0-backlog,1-inProgress,2-done)</param>
    /// <param name="newMaxTasks">the new limit</param>
    public void UpdateMaxTasks(int boardId, int columnOrdinal, int newMaxTasks)
    {
        columnController.UpdateMaxTasks(boardId, columnOrdinal, newMaxTasks);
        this.maxTasks = newMaxTasks;

    }
    /// <summary>
    /// this function call the Delete function in columnController
    /// </summary>
    public void Delete()
    {
        columnController.Delete(this);
    }
    
}