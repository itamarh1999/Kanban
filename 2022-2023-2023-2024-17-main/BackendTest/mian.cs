using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTest;

public class mian
{
    public static void Main(string[] args)
    {
        StartSession startSession = new StartSession();
        UserService userService = new UserService(startSession);
        BoardService boardService = new BoardService(startSession);
        TaskService taskService = new TaskService(startSession);
        TaskServiceTest t = new TaskServiceTest(taskService,userService,boardService);
        boardService.DeleteData();
    }

    public static void RunUserServiceTest()
    {
        StartSession startSession = new StartSession();
        UserService userService = new UserService(startSession);
        BoardService boardService = new BoardService(startSession);
        UserServiceTest u = new UserServiceTest(userService,boardService);
        Console.WriteLine("User Tests");
        Console.ReadLine();
        boardService.DeleteData();
        u.RunTests();
    }

    public static void RunBoardServiceTest()
    {
        StartSession startSession = new StartSession();
        UserService userService = new UserService(startSession);
        BoardService boardService = new BoardService(startSession);
        TaskService taskService = new TaskService(startSession);
        BoardServiceTest b = new BoardServiceTest(boardService,userService,taskService);
        Console.WriteLine("Board Tests");
        Console.ReadLine();
        boardService.DeleteData();
        b.RunTest();
    }
    public static void RunTaskServiceTest()
    {
        StartSession startSession = new StartSession();
        UserService userService = new UserService(startSession);
        BoardService boardService = new BoardService(startSession);
        TaskService taskService = new TaskService(startSession);
        TaskServiceTest t = new TaskServiceTest(taskService,userService,boardService);
        Console.WriteLine("Task Tests");
        Console.ReadLine();
        boardService.DeleteData();
        t.RunTests();
    }
}