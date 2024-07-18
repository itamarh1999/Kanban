using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Documents;
using Frontend.Model;
using IntroSE.Kanban.Backend.buisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend;

public class BackendController
{
    private StartSession startSession { get; set; }
    private UserService userService { get; set; }
    private BoardService boardService { get; set; }
    private TaskService taskService { get; set; }
    public BackendController()
    {
        this.startSession = new StartSession();
        this.userService = new UserService(startSession);
        this.boardService = new BoardService(startSession);
        this.taskService = new TaskService(startSession);
        boardService.LoadData();
    }
    internal void Register(string username, string password)
    {
        Response res = JsonSerializer.Deserialize<Response>(userService.Register(username, password));
        if (res.ErrorOccured())
        { 
            throw new Exception(res.ErrorMessage);
        }
    }
    internal void Logout(string username)
    {
        Response res = JsonSerializer.Deserialize<Response>(userService.Logout(username));
        if (res.ErrorOccured())
        { 
            throw new Exception(res.ErrorMessage);
        }
    }
    
    public UserModel Login(string username, string password)
    {
        Response user = JsonSerializer.Deserialize<Response>(userService.Login(username, password));
        if (user.ErrorOccured())
        {
            throw new Exception(user.ErrorMessage);
        }
        return new UserModel(this, username);
    }
    internal BoardModel GetBoard(UserModel userModel,string boardName)
    {
        Response response = JsonSerializer.Deserialize<Response>(boardService.GetBoard(userModel.Email, boardName));
        if (response.ErrorOccured())
        {
            throw new Exception(response.ErrorMessage);
        }
        BoardToSend board = JsonSerializer.Deserialize<BoardToSend>(response.ReturnValue.ToString());
        return new BoardModel(this, userModel, boardName);
    }

    internal List<string> GetAllUserBoards(string email)
    {
        Response boards = JsonSerializer.Deserialize<Response>(userService.GetUserBoards(email));
        if (boards.ErrorOccured())
        {
            throw new Exception(boards.ErrorMessage);
        }
        LinkedList<int> boardIds = JsonSerializer.Deserialize<LinkedList<int>>(boards.ReturnValue.ToString());
        LinkedList<string> ans = new LinkedList<string>();
        foreach (int boardId in boardIds)
        {
            Response boardnameResponse = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(boardId));
            string boardname = boardnameResponse.ReturnValue.ToString();
            ans.AddLast(boardname);
        }
        return ans.ToList();
    }

    internal List<TaskModel> GetColumn(string email, string boardName, int collumnOrdinal)
    {
        Response response = 
            JsonSerializer.Deserialize<Response>(boardService.GetColumn(email, boardName, collumnOrdinal));
        if (response.ErrorOccured())
        {
            throw new Exception(response.ErrorMessage);
        }
        List<Task> list = JsonSerializer.Deserialize<List<Task>>(response.ReturnValue.ToString());
        LinkedList<TaskModel> ans = new LinkedList<TaskModel>();
        foreach (Task task in list)
        {
            ans.AddLast(new TaskModel(this, task.Title, task.Description, task.Assignee, task.DueDate));
        }
        return ans.ToList();
    }
}