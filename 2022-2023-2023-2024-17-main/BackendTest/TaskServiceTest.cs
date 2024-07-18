using System.Runtime.Intrinsics.X86;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using log4net.Filter;

namespace BackendTest;

public class TaskServiceTest
{
    private TaskService t;
    private UserService u;
    private BoardService b;
    public TaskServiceTest(TaskService t,UserService u,BoardService b)
    {
        this.t = t;
        this.u = u;
        this.b = b;
    }

    private string error_check(string s)
    {
        Response response = JsonSerializer.Deserialize<Response>(s);
        if (response.ErrorMessage != null)
        {
            return response.ErrorMessage;
        }
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
    public void starter()
    {
        u.Register("mazogi@mazogi.com", "mazo0giZ");
        u.Register("itamar@hadida.HaEfes", "crossFit");
        b.CreateBoard("mazogi@mazogi.com", "mor");
        b.CreateBoard("itamar@hadida.HaEfes", "itamar");
    }
    public void AddTaskTestSunny()
    {
        Console.WriteLine("-----------------Add Task Tests Sunny---------------");
        string task1 = t.AddTask("mazogi@mazogi.com", "mor", "title", "description", DateTime.Now);
        printErrorsForSunny(task1);
        string task2 = t.AddTask("mazogi@mazogi.com", "mor", "title", "", DateTime.Now);
        printErrorsForSunny(task2);
        Console.ReadLine();
    }
    public void AddTaskTestRainy()
    {
        Console.WriteLine("-----------------Add Task Tests Rainy---------------");
        //title is empty
        string task1 = t.AddTask("mazogi@mazogi.com", "mor", "", "an empty title", DateTime.Now);
        string ans = error_check(task1);
        Console.WriteLine("expected Failure - " + ans);
        //title is to long
        string task2 = t.AddTask("mazogi@mazogi.com", "mor",
            "a reeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeealy long title",
            "description", DateTime.Now);
        ans = error_check(task2);
        Console.WriteLine("Expected Failure - " + ans);
        //board does not exist
        string task3 = t.AddTask("mazogi@mazogi.com", "1", "title", "description", DateTime.Now);
        ans = error_check(task3);
        Console.WriteLine("Expected Failure - " + ans);
        //email is not in the system
        string task4 = t.AddTask("mazogi@bazogi.com", "mor", "title", "description", DateTime.Now);
        ans = error_check(task4);
        Console.WriteLine("Expected Failure - " + ans);
        //tried to access an existing board under the wrong user email
        string task5 = t.AddTask("itamar@hadida.HaEfes", "mor", "title", "description", DateTime.Now);
        ans = error_check(task5);
        Console.WriteLine("Expected Failure - " + ans);
        Console.ReadLine();
    }
    public void UpdateTaskDueDateTestSunny()
    {
        Console.WriteLine("--------------Update Task Due Date Tests Sunny------------");
        string task = t.AssignTask("mazogi@mazogi.com", "mor", 0, 0, "mazogi@mazogi.com");
        printErrorsForSunny(task);
        task = t.AssignTask("mazogi@mazogi.com", "mor", 0, 1, "mazogi@mazogi.com");
        printErrorsForSunny(task);
        string task1 = t.UpdateTaskDueDate("mazogi@mazogi.com", "mor", 0, 0, DateTime.Today);
        printErrorsForSunny(task1);
        string task2 = t.UpdateTaskDueDate("mazogi@mazogi.com", "mor", 0, 1, DateTime.Today);
        printErrorsForSunny(task2);
        Console.ReadLine();
    }
    public void UpdateTaskDueDateTestRainy()
    {
        string ans;
        Console.WriteLine("--------------Update Task Due Date Tests Rainy------------");
        // user does not exist
        string task = t.UpdateTaskDueDate("david@Gmail.com", "mor", 0, 0, DateTime.Today);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        // board does not exist
        task = t.UpdateTaskDueDate("mazogi@mazogi.com", "10", 0, 1, DateTime.Now);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //task id does not exist
        task = t.UpdateTaskDueDate("mazogi@mazogi.com", "mor", 0, 8754, DateTime.Now);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //column does not exist
        task = t.UpdateTaskDueDate("mazogi@mazogi.com", "mor", 5, 0, DateTime.Now);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        
        Console.ReadLine();
    }
    public void UpdateTaskTitleTestSunny()
    {
        Console.WriteLine("--------------Update Task Title Tests Sunny------------");
        
        string task1 = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 0, 0, "titletitle");
        printErrorsForSunny(task1);
        string task2 = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 0, 1, "tities");//taskId not exist
        printErrorsForSunny(task2);
       Console.ReadLine();
    }
    public void UpdateTaskTitleTestRainy()
    {
        string ans;
        Console.WriteLine("--------------Update Task Title Tests Rainy------------");
        // user does not exist
        string task = t.UpdateTaskTitle("david@Gmail.com", "mor", 0, 0, "title");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        // board does not exist
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "10", 0, 1, "title");//taskId not exist
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //task id does not exist
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 0, 8754, "title");//taskId not exist
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //column does not exist
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 5, 0, "title");//taskId not exist
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //title is to short
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 0, 0, "");//taskId not exist
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //title is to long
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 0, 0,
            "everything is awesome! everything is cool when you're part of a team!");//taskId not exist
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        
        Console.ReadLine();
    }
    public void UpdateTaskDescriptionTestSunny()
    {
        Console.WriteLine("--------------Update Task Description Tests Sunny------------");
        string task1 = t.UpdateTaskDescription("mazogi@mazogi.com", "mor", 0, 0, "titletitle");
        printErrorsForSunny(task1);
        string task2 = t.UpdateTaskDescription("mazogi@mazogi.com", "mor", 0, 1, "tities");//taskId not exist
        printErrorsForSunny(task1);
        Console.ReadLine();
    }
    public void UpdateTaskDescriptionTestRainy()
    {
        string ans;
        Console.WriteLine("--------------Update Task Title Tests Rainy------------");
        // user does not exist
        string task = t.UpdateTaskTitle("david@Gmail.com", "mor", 0, 0, "title");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        // board does not exist
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "10", 0, 1, "title");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //task id does not exist
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 0, 8754, "title");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //column does not exist
        task = t.UpdateTaskTitle("mazogi@mazogi.com", "mor", 5, 0, "title");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);

        Console.ReadLine();
    }
    public void AdvanceTaskTestSunny()
    {
        Console.WriteLine("---------------Advance Task Tests Sunny--------------");
        string task1 = t.AdvanceTask("mazogi@mazogi.com", "mor", 0, 0);
        printErrorsForSunny(task1);
        string task2 = t.AdvanceTask("mazogi@mazogi.com", "mor", 1, 0);
        printErrorsForSunny(task2);
        string task3 = t.AdvanceTask("mazogi@mazogi.com", "mor", 0, 1);
        printErrorsForSunny(task3);
        Console.ReadLine();

    }
    public void AdvanceTaskTestRainy()
    {
        string ans;
        Console.WriteLine("---------------Advance Task Tests Rainy--------------");
        // user does not exist
        string task = t.AdvanceTask("david@Gmail.com", "mor", 0, 0);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        // board does not exist
        task = t.AdvanceTask("mazogi@mazogi.com", "10", 0, 1);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //task id does not exist
        task = t.AdvanceTask("mazogi@mazogi.com", "mor", 0, 8754);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //column does not exist
        task = t.AdvanceTask("mazogi@mazogi.com", "mor", 5, 0);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //task exists in different column
        task = t.AdvanceTask("mazogi@mazogi.com", "mor", 0, 0);
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);

        Console.ReadLine();
    }
    public void AssignTaskSunny()
    {
        Console.WriteLine("---------------Assign Task Tests Sunny--------------");
        string task = b.JoinBoard("itamar@hadida.HaEfes", 0);
        printErrorsForSunny(task);
        task = t.AssignTask("mazogi@mazogi.com", "mor", 1, 1, "itamar@hadida.HaEfes");
        printErrorsForSunny(task);
        task = t.AssignTask("itamar@hadida.HaEfes", "mor", 1, 1, "mazogi@mazogi.com");
        Console.WriteLine(error_check(task));
        Console.ReadLine();
    }
    public void AssignTaskRainy()
    {
        string ans;
        Console.WriteLine("---------------Assign Task Tests Rainy--------------");
        //task not in this column
        string task = t.AssignTask("mazogi@mazogi.com", "mor", 0, 1, "itamar@hadida.HaEfes");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //task ID does not exist
        task = t.AssignTask("mazogi@mazogi.com", "mor", 1, 11, "itamar@hadida.HaEfes");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //assignee email is not a member in this board
        task = t.AssignTask("mazogi@mazogi.com", "mor", 1, 1, "tamar@hadida.HaEfes");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //board was not found
        task = t.AssignTask("mazogi@mazogi.com", "morrr", 1, 1, "itamar@hadida.HaEfes");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        //user dont exist
        task = t.AssignTask("azogi@gmail.com", "mor", 1, 1, "itamar@hadida.HaEfes");
        ans = error_check(task);
        Console.WriteLine("Expected Failure - " + ans);
        Console.ReadLine();
    }
    public void RunTests()
    {
        starter();
        AddTaskTestSunny();
        AddTaskTestRainy();
        UpdateTaskDueDateTestSunny();
        UpdateTaskDueDateTestRainy();
        UpdateTaskTitleTestSunny();
        UpdateTaskTitleTestRainy();
        UpdateTaskDescriptionTestSunny();
        UpdateTaskDescriptionTestRainy();
        AdvanceTaskTestSunny();
        AdvanceTaskTestRainy();
        AssignTaskSunny();
        AssignTaskRainy();
    }
}