using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using IntroSE.Kanban.Backend.buisnessLayer;
using Task = IntroSE.Kanban.Backend.buisnessLayer.Task;

namespace BackendTest;

public class BoardServiceTest
{
    private BoardService boardService;
    private UserService userService;
    private TaskService taskService;
    public BoardServiceTest(BoardService boardService,UserService userService, TaskService taskService)
        {
            this.boardService = boardService;
            this.userService = userService;
            this.taskService = taskService;
        }
    
    public void RunTest()
    {
        CreateBoardSuccess();
        CreateBoardFail();
        GetColumnNameTestSunny();
        GetColumnNameTestRainy();
        LimitColumnTestSuccess();
        LimitColumnTestFail();
        GetColumnLimitSunny();
        GetColumnLimitRainy();
        GetColumnSunnyTest();
        GetColumnRainyTest();
        InProgressTasksTestSunny();
        InProgressTasksTestRainy();
        DeleteBoardSuccess();
        DeleteBoardFail();
        JoinBoardSunny();
        JoinBoardRainy();
        TransferOwnershipSunny();
        TransferOwnershipRainy();
        LeaveBoardSunny();
        LeaveBoardRainy();
        GetBoardNameSunny();
        GetBoardNameRainy();
    }

    private string user = "SE2023@gmail.com";
    private string boardName = "Test Board";
    public string ErrorCheck(string s)
    {
        Response res = JsonSerializer.Deserialize<Response>(s);
        if (res.ErrorMessage != null)
            return res.ErrorMessage;
        else
        {
            return "";
        }
    }

    public void printErrorsForSunny(string s)
    {
        Response res = JsonSerializer.Deserialize<Response>(s);
        if (res.ErrorMessage != null)
            Console.WriteLine(res.ErrorMessage);
    }

    public void CreateBoardSuccess()
    {
        string password = "Aa123456";
        Console.WriteLine("--------------Create Board Sunny test: expected success as empty Response");
        string reg = userService.Register(user, password);
        printErrorsForSunny(reg);
        string board = boardService.CreateBoard(user, boardName);
        printErrorsForSunny(board);
        string task = taskService.AddTask(user, boardName, "First Task", "work please", DateTime.Now);
        printErrorsForSunny(task);
        task = taskService.AddTask(user, boardName, "The main Task", "work please", DateTime.Now);
        printErrorsForSunny(task);
        task = taskService.AddTask(user, boardName, "Test For the Task", "work please", DateTime.Now);
        printErrorsForSunny(task);
        task = taskService.AddTask(user, boardName, "I am Student", "work please", DateTime.Now);
        printErrorsForSunny(task);
        Console.ReadLine();

    }
    public void CreateBoardFail()
    {                                   //assume we allready created "Test Board"
        string password = "Aa123456";
        Console.WriteLine("--------------Create Board Rainy test : Expected 2 Fails"); 
        string board1= boardService.CreateBoard(user, boardName);
        string ans = ErrorCheck(board1);
        Console.WriteLine(ans);
        string user2 = "malihi@gmail.com";//user dont exist
        string board2= boardService.CreateBoard(user2, boardName);
        string ans2 = ErrorCheck(board2);
        Console.WriteLine(ans2);
        Console.ReadLine();

    }
    public void DeleteBoardSuccess()
    {
        string password = "Aa123456";
        Console.WriteLine("--------------Delete Board test success: expected empty Response"); 
        string delete = boardService.DeleteBoard(user, boardName);
        printErrorsForSunny(delete);
        Console.ReadLine();

    }
    public void DeleteBoardFail()
    {
        string wrongBoardName = "just a board";
        Console.WriteLine("--------------Delete Board test: expected 1 Fail"); 
        string delete = boardService.DeleteBoard(user, wrongBoardName);
        string ans = ErrorCheck(delete);
        Console.WriteLine(ans);
        Console.ReadLine();

    }
    public void LimitColumnTestSuccess()
    {
        Console.WriteLine("--------------LimitColumnTest Sunny test: expected Success as empty Response"); 
        string limit1 = boardService.LimitColumn(user,boardName,0,10);//success
        printErrorsForSunny(limit1);
        Console.ReadLine();
    }
    public void LimitColumnTestFail()
    {
        Console.WriteLine("--------------Limit Column Rainy test: expected 3 Fails");
        taskService.AddTask(user, "1", "add1", "", DateTime.Now);
        taskService.AddTask(user, "1", "add2", "", DateTime.Now);
        string limit2 = boardService.LimitColumn("David@gmail.com",boardName,0,1);
        Response response2 = JsonSerializer.Deserialize<Response>(limit2);//too many tasks already in column
        string ans = ErrorCheck(limit2);
        Console.WriteLine("Expected Failure:" + ans); 
        string limit3 = boardService.LimitColumn(user,"not exist",0,1);
        Response response3 = JsonSerializer.Deserialize<Response>(limit2);//board dont exist
        ans= ErrorCheck(limit3);
        Console.WriteLine(ans);
        
        limit3 = boardService.LimitColumn(user,boardName,7,1);
        ans = ErrorCheck(limit3);//ColumnOrdinal out of bound
        Console.WriteLine(ans);
        Console.ReadLine();
    }
    public void GetColumnLimitSunny()
    {
        Console.WriteLine("--------------Get Limit Column Sunny test: expectedc success as limit");

        string limit = boardService.GetColumnLimit(user, boardName, 0);
        Response response = JsonSerializer.Deserialize<Response>(limit);
        int ans = JsonSerializer.Deserialize<int>((JsonElement)response.ReturnValue);
        Console.WriteLine("Limit- " + ans);
        Console.ReadLine();
    }
    public void GetColumnLimitRainy()
    {
        Console.WriteLine("--------------Get Limit Column Rainy test: expected 2 Fail");
        string limit = boardService.GetColumnLimit(user, "10", 0);
        string ans = ErrorCheck(limit);//board not exist
        Console.WriteLine(ans);
        
        limit = boardService.GetColumnLimit(user, boardName, 7);
        ans = ErrorCheck(limit);//columnordinal out of bound
        Console.WriteLine(ans);
        Console.ReadLine();
    }
    public void GetColumnNameTestSunny()
    {
        Console.WriteLine("--------------Get Column Name Sunny test: expected - 'backlog , in progress , done'");
        string limit1 = boardService.GetColumnName(user, boardName, 2);//success
        Response response1 = JsonSerializer.Deserialize<Response>(limit1);
        string ans3 = JsonSerializer.Deserialize<string>((JsonElement)response1.ReturnValue);
        string limit2 = boardService.GetColumnName(user, boardName, 1);
        Response response2 = JsonSerializer.Deserialize<Response>(limit2);
        string ans2 = JsonSerializer.Deserialize<string>((JsonElement)response2.ReturnValue);
        string limit3 = boardService.GetColumnName(user, boardName, 0);
        Response response3 = JsonSerializer.Deserialize<Response>(limit3);
        string ans1 = JsonSerializer.Deserialize<string>((JsonElement)response3.ReturnValue);
        Console.WriteLine(ans1 + " , " + ans2 + " , " + ans3);
        Console.ReadLine();
    }
    public void GetColumnNameTestRainy()
    {
        Console.WriteLine("--------------Get Column Name Rainy test: expected 2Fail");
        string limit = boardService.GetColumnName(user, "10", 1);
        string ans = ErrorCheck(limit);//board not exist
        Console.WriteLine(ans);
        
        limit = boardService.GetColumnName(user, boardName, 7);
        ans = ErrorCheck(limit);//ColumnOrdinal out of bound
        Console.WriteLine(ans);
        Console.ReadLine();
    }
    public void GetColumnSunnyTest()
    {
        Console.WriteLine("--------------Get Column Sunny Test: expected success as List of all column's tasks");
        string columns1 = boardService.GetColumn(user, boardName, 0);
        Response response1 = JsonSerializer.Deserialize<Response>(columns1);
        List<Task> ans1 = JsonSerializer.Deserialize<List<Task>>(response1.ReturnValue.ToString());
        Console.Write("column task Id's:  ");
        foreach (Task task in ans1)
        {
            Console.Write(task.getTaskID() + "   ");
        }
        Console.ReadLine();
    }
    public void GetColumnRainyTest()
    {
        Console.WriteLine("--------------Get Column Rainy Test: expected 2 Fails");

        string columns = boardService.GetColumn(user, "10", 0);
        string ans = ErrorCheck(columns);//Board not exist
        Console.WriteLine(ans);
        
        columns = boardService.GetColumn(user, boardName, 10);
        ans = ErrorCheck(columns);//ColumnOrdinal out of bound
        Console.WriteLine(ans);
        Console.ReadLine();
    }
    public void InProgressTasksTestSunny()
    {
        Console.WriteLine("--------------In Progress Tasks Sunny Test: expected success as list of all tasks in inProgress");
        string assignee = taskService.AssignTask(user, boardName, 0, 0, user);
        printErrorsForSunny(assignee);
        string advanceTask = taskService.AdvanceTask(user, boardName, 0, 0);
        printErrorsForSunny(advanceTask);
        string tasks1 = boardService.InProgressTasks(user);
        printErrorsForSunny(tasks1);
        Response response1 = JsonSerializer.Deserialize<Response>(tasks1);
        LinkedList<Task> ans1 = JsonSerializer.Deserialize<LinkedList<Task>>(response1.ReturnValue.ToString());
        foreach (Task task in ans1)
        {
            Console.WriteLine("TaskId : " + task.getTaskID());
        }
        Console.ReadLine();
    }
    public void InProgressTasksTestRainy()
    {
        Console.WriteLine("--------------In Progress Tasks Rainy Test: expected 1 Fail");
        string tasks = boardService.InProgressTasks("david@Gmail.com");
        string ans = ErrorCheck(tasks);//user not exsist
        Console.WriteLine(ans);
        Console.ReadLine();
    }
    public void JoinBoardSunny()
    {
        userService.Register("mazogi@gmail.com", "Aa123456");
        boardService.CreateBoard("mazogi@gmail.com", "mor1");
        boardService.CreateBoard("mazogi@gmail.com", "mor2");
        boardService.CreateBoard(user, "user1");
        boardService.CreateBoard(user, "user2");
        Response res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor1"));
        int id1 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor2"));
        int id2 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user1"));
        int id3 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user2"));
        int id4 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        string test;
        Console.WriteLine("--------------Join Board Sunny Tests---------------");
        //join the test user to the newly created boards
        test = boardService.JoinBoard(user, id1);
        printErrorsForSunny(test);
        test = boardService.JoinBoard(user, id2);
        printErrorsForSunny(test);
        //joining the newly created user to the original user's boards
        test = boardService.JoinBoard("mazogi@gmail.com", id3);
        printErrorsForSunny(test);
        test = boardService.JoinBoard("mazogi@gmail.com", id4);
        printErrorsForSunny(test);
        Console.ReadLine();
    }
    public void JoinBoardRainy()
    {
        string ans;
        Response res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor1"));
        int id1 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor2"));
        int id3 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        string test;
        Console.WriteLine("--------------Join Board Rainy---------------");
        //board ID does not exist
        test = boardService.JoinBoard(user, -5);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //user is already in the board
        test = boardService.JoinBoard(user, id1);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //user does not exist
        test = boardService.JoinBoard("azogi@gmail.com", id1);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        Console.ReadLine();
    }
    public void TransferOwnershipSunny()
    {
        string test;
        Console.WriteLine("--------------Transfer Ownership Sunny Tests---------------");
        test = boardService.TransferOwnership("mazogi@gmail.com", user, "mor1");
        Console.WriteLine(ErrorCheck(test));
        test = boardService.TransferOwnership( user,"mazogi@gmail.com", "user1");
        Console.WriteLine(ErrorCheck(test));
        Console.ReadLine();
    }
    public void TransferOwnershipRainy()
    {
        userService.Register("idanazama@jamal.com","Aa123456");
        string ans;
        string test;
        Console.WriteLine("--------------Transfer Ownership Rainy Tests---------------");
        //the user giving ownership is not the owner
        test = boardService.TransferOwnership("mazogi@gmail.com", user, "mor1");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the user giving ownership isn't a member in this board (and does not exist)
        test = boardService.TransferOwnership( "azogi@gmail.com","mazogi@gmail.com", "user1");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the user receiving ownership isn't a member in this board (and does not exist)
        test = boardService.TransferOwnership( "mazogi@gmail.com","azogi@gmail.com", "user1");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the user giving ownership isn't a member in this board (and exists)
        test = boardService.TransferOwnership( "idanazama@jamal.com","mazogi@gmail.com", "user1");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the user receiving ownership isn't a member in this board (and exists)
        test = boardService.TransferOwnership( "mazogi@gmail.com","idanazama@jamal.com", "user1");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the board is not owned by the giving user
        test = boardService.TransferOwnership( "mazogi@gmail.com",user, "godDamn that's a lot of work for nothing");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        Console.ReadLine();
    }
    public void LeaveBoardSunny()
    {
        Response res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user1"));
        int id1 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor2"));
        int id2 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor1"));
        int id3 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user2"));
        int id4 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        string test;
        Console.WriteLine("--------------Leave Board Sunny Tests---------------");
        test = boardService.LeaveBoard(user, id2);
        Console.WriteLine(ErrorCheck(test));
        test = boardService.LeaveBoard("mazogi@gmail.com", id4);
        Console.WriteLine(ErrorCheck(test));
        Console.ReadLine();

    }
    public void LeaveBoardRainy()
    {
        string ans;
        Response res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "user1"));
        int id1 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor2"));
        int id2 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "mor1"));
        int id3 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user2"));
        int id4 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        string test;
        Console.WriteLine("--------------Leave Board Rainy Tests---------------");
        //the user is the owner of the board
        test = boardService.LeaveBoard("mazogi@gmail.com", id1);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the user is not in the board
        test = boardService.LeaveBoard(user, id2);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the user does not exist
        test = boardService.LeaveBoard("azogi@gmail.com", id3);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //the board does not exist
        test = boardService.LeaveBoard("mazogi@gmail.com", -5);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);

        Console.ReadLine();
    }
    public void GetBoardNameSunny()
    {
        string ans = boardService.GetBoardID(user, "mor1");
        Response res = JsonSerializer.Deserialize<Response>(ans);
        int id1 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor2"));
        int id2 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "user1"));
        int id3 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user2"));
        int id4 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        string test;
        Console.WriteLine("--------------Get Board Name Sunny Tests---------------");
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(id1));
        string ans1 = JsonSerializer.Deserialize<string>((JsonElement)res.ReturnValue);
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(id2));
        string ans2 = JsonSerializer.Deserialize<string>((JsonElement)res.ReturnValue);
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(id3));
        string ans3 = JsonSerializer.Deserialize<string>((JsonElement)res.ReturnValue);
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(id4));
        string ans4 = JsonSerializer.Deserialize<string>((JsonElement)res.ReturnValue);
        Console.WriteLine($@"Board Names: {ans1}, {ans2}, {ans3}, {ans4}");
        Console.ReadLine();
    }
    public void GetBoardNameRainy()
    {
        string ans;
        Response res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user1"));
        int id1 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor2"));
        int id2 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID("mazogi@gmail.com", "mor1"));
        int id3 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        res = JsonSerializer.Deserialize<Response>(boardService.GetBoardID(user, "user2"));
        int id4 = JsonSerializer.Deserialize<int>(res.ReturnValue.ToString());
        string test;
        Console.WriteLine("--------------Get Board Name Rainy Tests---------------");
        //board does not exist
        test = boardService.GetBoardName(-5);
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);


        Console.ReadLine();
    }
    
}