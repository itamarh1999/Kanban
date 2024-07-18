using System.Text.Json;
using IntroSE.Kanban.Backend.buisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTest;

public class UserServiceTest
{
    private UserService userService;
    private BoardService boardService;

    public UserServiceTest(UserService userService,BoardService boardService)
    {
        this.userService = userService;
        this.boardService = boardService;
    }
    
    public void RunTests()
    {
        RegisterSunny();
        RegisterRainy();
        LogoutSunny();
        LogoutRainy();
        LoginSunny();
        LoginRainy();
        GetUserBoardsSunny();
        GetUserBoardsRainy();
    }

    private string ErrorCheck(string s)
    {
        Response res = JsonSerializer.Deserialize<Response>(s);
        if (res.ErrorMessage != null)
            return res.ErrorMessage;
        else
        {
            return "";
        }
    }

    public void RegisterSunny()
    {
        string user1 = "David@gmail.com";
        string user2 = "Ahiya@gmail.com";
        string user3 = "mazogi@gmail.com";
        string password = "Aa123456";
        Console.WriteLine("-----------------Register Sunny test: expected success as empty Response");
        string reg1 = userService.Register(user1, password);
        Response response1 = JsonSerializer.Deserialize<Response>(reg1);
        string reg2 = userService.Register(user2, password);
        Response response2 = JsonSerializer.Deserialize<Response>(reg2);
        string reg3 = userService.Register(user3, password);
        Response response3 = JsonSerializer.Deserialize<Response>(reg3);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        if (response2.ErrorMessage != null)
            Console.WriteLine(response2.ErrorMessage);
        if (response3.ErrorMessage != null)
            Console.WriteLine(response3.ErrorMessage);
        Console.ReadLine();
    }

    public void RegisterRainy()
    {
        string user1 = "David@gmail.com";
        string password = "Aa123456";
        string inValidMail = "David@gmail";
        string inValidPassword = "123";
        Console.WriteLine("-----------------Register Rainy test: expected  3 Fails");
        string register1 = userService.Register(user1, password);
        Response response1 = JsonSerializer.Deserialize<Response>(register1);
        string ans1 = response1.ErrorMessage;
        Console.WriteLine(ans1);
        
        string register2 = userService.Register(inValidMail, password);
        Response response2 = JsonSerializer.Deserialize<Response>(register2);
        string ans2 = response2.ErrorMessage;
        Console.WriteLine(ans2);
        
        string register3 = userService.Register(user1, inValidPassword);
        Response response3 = JsonSerializer.Deserialize<Response>(register3);
        string ans3 = response3.ErrorMessage;
        Console.WriteLine(ans3);
        Console.ReadLine();
    }

    public void LogoutSunny()
    {
        string user1 = "David@gmail.com";
        string user2 = "Ahiya@gmail.com";
        string password = "Aa123456";
        Console.WriteLine("-----------------Logout Sunny test: expected success as empty Response");
        string logout1 = userService.Logout(user1);
        Response response1 = JsonSerializer.Deserialize<Response>(logout1);
        string logout2 = userService.Logout(user2);
        Response response2 = JsonSerializer.Deserialize<Response>(logout2);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        if (response2.ErrorMessage != null)
            Console.WriteLine(response2.ErrorMessage);
        Console.ReadLine();
    }
    
    public void LogoutRainy()
    {
        string user1 = "David@gmail.com";
        string userNotExist = "Neomi@gmail.com";
        Console.WriteLine("-----------------Logout Rainy test: expected 1 Fails");
        string register1 = userService.Logout(user1);
        Response response1 = JsonSerializer.Deserialize<Response>(register1);
        string ans1 = response1.ErrorMessage;
        Console.WriteLine(ans1);
        Console.ReadLine();
    }
    
    public void LoginSunny()
    {
        string user1 = "David@gmail.com";
        string password = "Aa123456";
        Console.WriteLine("-----------------Login Sunny test: expected success as empty Response");
        string reg1 = userService.Login(user1, password);
        string reg2 = userService.Login("Ahiya@gmail.com", "Aa123456");
        Response response1 = JsonSerializer.Deserialize<Response>(reg1);
        Response response2 = JsonSerializer.Deserialize<Response>(reg2);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        if (response2.ErrorMessage != null)
            Console.WriteLine(response2.ErrorMessage);
        Console.ReadLine();
    }

    public void LoginRainy()
    {
        string user1 = "David@gmail.com";
        string user2 = "Ahiya@gmail.com";
        string password = "Aa123456";
        string wrongPassword = "Aa12345";
        string userNotExist = "Neomi@gmail.com";
        Console.WriteLine("-----------------Login Rainy test: expected 3 Fails");
        string login1 = userService.Login(user1, password);
        Response response1 = JsonSerializer.Deserialize<Response>(login1);
        string ans1 = response1.ErrorMessage;
        Console.WriteLine(ans1);//user already login
        
        string login2 = userService.Login(userNotExist,password);
        Response response2 = JsonSerializer.Deserialize<Response>(login2);
        string ans2 = response2.ErrorMessage;
        Console.WriteLine(ans2);//user not exist
        
        string login3 = userService.Login(user2,wrongPassword);
        Response response3 = JsonSerializer.Deserialize<Response>(login3);
        string ans3 = response3.ErrorMessage;
        Console.WriteLine(ans3);//wrong password
        Console.ReadLine();
    }

    public void GetUserBoardsSunny()
    {
        string test;
        Console.WriteLine("-----------------Get User Boards Sunny----------------");
        //creating boards with ahiya
        test = boardService.CreateBoard("Ahiya@gmail.com", "1_board_ahiya");
        Response response1 = JsonSerializer.Deserialize<Response>(test);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        test = boardService.CreateBoard("Ahiya@gmail.com", "2_board_ahiya");
        response1 = JsonSerializer.Deserialize<Response>(test);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        test = boardService.CreateBoard("Ahiya@gmail.com", "3_board_ahiya");
        response1 = JsonSerializer.Deserialize<Response>(test);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        //joining david to ahiya boards 1 and 2
        test = boardService.JoinBoard("David@gmail.com", 0);
        response1 = JsonSerializer.Deserialize<Response>(test);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        test = boardService.JoinBoard("David@gmail.com", 1);
        response1 = JsonSerializer.Deserialize<Response>(test);
        if (response1.ErrorMessage != null)
            Console.WriteLine(response1.ErrorMessage);
        //getting david's boards
        test = userService.GetUserBoards("David@gmail.com");
        response1 = JsonSerializer.Deserialize<Response>(test);
        LinkedList<int> ans = JsonSerializer.Deserialize<LinkedList<int>>(response1.ReturnValue.ToString());
        Console.WriteLine("David@gmail.com boardsId");
        Console.Write("Id's: ");
        foreach (int boardId in ans)
        {
            Console.Write(boardId + "   ");
        }
        Console.WriteLine();
        //getting ahiya's boards
        test = userService.GetUserBoards("Ahiya@gmail.com");
        response1 = JsonSerializer.Deserialize<Response>(test);
        ans = JsonSerializer.Deserialize<LinkedList<int>>(response1.ReturnValue.ToString());
        Console.WriteLine("Ahiya@gmail.com boardsId");
        Console.Write("Id's: ");
        foreach (int boardId in ans)
        {
            Console.Write(boardId + "   ");
        }
        Console.WriteLine();

        //getting david's boards after adding the 3 board
        boardService.JoinBoard("David@gmail.com", 2);
        test = userService.GetUserBoards("David@gmail.com");
        response1 = JsonSerializer.Deserialize<Response>(test);
        ans = JsonSerializer.Deserialize<LinkedList<int>>(response1.ReturnValue.ToString());
        Console.WriteLine("David@gmail.com");
        foreach (int boardId in ans)
        {
            Console.Write(boardId + "   ");
        }
        Console.ReadLine();
    }

    public void GetUserBoardsRainy()
    {
        string ans;
        string test;
        Console.WriteLine("-----------------Get User Boards Rainy----------------");
        //email doesnt exist in the system
        test = userService.GetUserBoards("Duvid@gmail.com");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected Failure - " + ans);
        //user doesnt have any boards
        test = userService.GetUserBoards("mazogi@gmail.com");
        ans = ErrorCheck(test);
        Console.WriteLine("Expected nothing because he has 0 boards - " + ans);

        Console.ReadLine();
    }

}