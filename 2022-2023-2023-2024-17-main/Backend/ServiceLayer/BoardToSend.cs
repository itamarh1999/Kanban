using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.buisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
/// <summary>
/// this class is strictly for sending a board's info. it does not have a purpose or actions beyond that.
/// </summary>
public class BoardToSend
{
    public int boardID;
    public string name;
    public string owner;
    public int taskCounter;
    public ColumnToSend[] columns;

    public BoardToSend(Board b)
    {
        this.boardID = b.boardID;
        this.name = b.name;
        this.owner = b.owner;
        this.taskCounter = b.taskCounter;
        this.columns = new ColumnToSend[3];
        this.columns[0] = new ColumnToSend(b.columns[0]);
        this.columns[1] = new ColumnToSend(b.columns[1]);
        this.columns[2] = new ColumnToSend(b.columns[2]);
    }
[JsonConstructor]
    public BoardToSend(int boardID, string name, string owner, int taskCounter, ColumnToSend[] columns)
    {
        this.boardID = boardID;
        this.name = name;
        this.owner = owner;
        this.taskCounter = taskCounter;
        this.columns = columns;
    }
    

    public int BoardID => boardID;

    public string Name => name;

    public string Owner => owner;

    public int TaskCounter => taskCounter;

    public ColumnToSend[] Columns => columns;
}