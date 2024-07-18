using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json;
using IntroSE.Kanban.Backend.buisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer;
/// <summary>
/// a class in the service layer that communicates with the front-end through JSON and with the business layer.
/// for every function there is a JSON serializer for the communication to be sent,
/// either as an answer or as an error message.
/// all the functions in this class send the functions to board facade in the business layer.
/// </summary>
public class BoardService
{
    private StartSession startSession;
    public BoardService(StartSession startSession)
    {
        this.startSession = startSession;
    }
    /// <summary>
    ///  this function sends the data to the the CreateBoard function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string CreateBoard(string email, string boardName)
    {
        try
        {
            startSession.boardFacade.CreateBoard(email, boardName);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the DeleteBoard function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string DeleteBoard(string email, string boardName)
    {
        try
        {
            startSession.boardFacade.DeleteBoard(email, boardName);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the GetBoard function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string GetBoard(string email, string boardName)
    {
        try
        {
            Board board = startSession.boardFacade.GetBoard(email, boardName);
            BoardToSend boardToSend = new BoardToSend(board);
            Response r = new Response(JsonSerializer.Serialize(boardToSend), null);
            return JsonSerializer.Serialize(r);
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);
        }
    }
    /// <summary>
    ///  this function sends the data to the the LimitColumn function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string LimitColumn(string email, string boardName,int columnordinal, int limit)
    {
        try
        {
            startSession.boardFacade.LimitColumn(email, boardName, columnordinal, limit);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the GetColumnLimit function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string GetColumnLimit(string email, string boardName,int columnordinal)
    {
        try
        {
            int limit = startSession.boardFacade.GetColumnLimit(email, boardName,columnordinal);
            Response r = new Response(limit, null);
            return JsonSerializer.Serialize(r);
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the GetColumn function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string GetColumn(string email, string boardName,int columnordinal)
    {
        try
        {
            LinkedList<Task> taskList = startSession.boardFacade.GetColumn(email,boardName,columnordinal);
            Response r = new Response(JsonSerializer.Serialize(taskList), null);
            return JsonSerializer.Serialize(r);
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);
        }
    }
    /// <summary>
    ///  this function sends the data to the the GetColumnName function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string GetColumnName(string email, string boardName,int columnordinal)
    {
        try
        {
            string columnName = startSession.boardFacade.GetColumnName(email, boardName, columnordinal);
            Response r = new Response(columnName, null);
            return JsonSerializer.Serialize(r);
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the GetUserInProgressTask function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string InProgressTasks(string email)
    {
        try
        {
            LinkedList<Task> taskList = startSession.boardFacade.GetUserInProgressTask(email);
            Response r = new Response(taskList, null);
            return JsonSerializer.Serialize(r);
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);
        }
    }
    /// <summary>
    ///  this function sends the data to the the JoinBoard function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string JoinBoard(string email, int boardID)
    {
        try
        {
            startSession.boardFacade.JoinBoard(email, boardID);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the LeaveBoard function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string LeaveBoard(string email, int boardID)
    {		 
        try
        {
            startSession.boardFacade.LeaveBoard(email, boardID);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the GetBoardName function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>    
    public string GetBoardName(int boardID)		 
    {		 
        try
        {
            string name = startSession.boardFacade.GetBoardName(boardID);
            return JsonSerializer.Serialize(new Response(name, null));
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the TransferOwnership function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
    {
        try
        {
            startSession.boardFacade.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the LoadData function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string LoadData()		 
    {		 
        try
        {
            startSession.boardFacade.LoadData();
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the DeleteData function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string DeleteData()		 
    {		 
        try
        {
            startSession.boardFacade.DeleteData();
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the GetBoardID function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string GetBoardID(string email, string boardName)
    {
        try
        {
            int ans = startSession.boardFacade.GetBoardID(email, boardName);
            return JsonSerializer.Serialize(new Response(JsonSerializer.Serialize(ans),null));
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
}