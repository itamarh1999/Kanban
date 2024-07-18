using System;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer;
/// <summary>
/// a class in the service layer that communicates with the front-end through JSON and with the business layer.
/// for every function there is a JSON serializer for the communication to be sent,
/// either as an answer or as an error message.
/// all the functions in this class send the functions to board facade in the business layer.
/// </summary>
public class TaskService
{
    private StartSession startSession;
    /// <summary>
    ///  this function sends the data to the the GetBoard function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public TaskService(StartSession startSession)
    {
        this.startSession = startSession;
    }
    /// <summary>
    ///  this function sends the data to the the AddTask function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string AddTask(string email, string boardName, string title, string description, DateTime date)
    {
        try
        {
            startSession.boardFacade.AddTask(email, boardName, title, description, date);
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
    public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dateTime)
    {
        try
        {
            startSession.boardFacade.UpdateTaskDueDate(email, boardName,columnOrdinal, taskId, dateTime);
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
    public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
    {
        try
        {
            startSession.boardFacade.UpdateTaskTitle(email, boardName,columnOrdinal, taskId, title);
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
    public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description) 
    {
        try
        {
            startSession.boardFacade.UpdateTaskDescription(email, boardName,columnOrdinal, taskId, description);
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
    public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
    {
        try
        {
            startSession.boardFacade.AdvanceTask(email, boardName,columnOrdinal, taskId);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    /// <summary>
    ///  this function sends the data to the the AssignTask function in board facade.
    ///  then it serializes the response into a JSON and sends it back. 
    /// </summary>
    public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
    {		 
        try
        {
            startSession.boardFacade.AssignTask(email, boardName,columnOrdinal, taskID, emailAssignee);
            return JsonSerializer.Serialize(new Response());
        }
        catch (Exception e)
        {
            Response res = new Response(e.Message);
            return JsonSerializer.Serialize(res);

        }
    }
    
}