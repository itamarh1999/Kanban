using System.Collections.Generic;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.buisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer;
/// <summary>
/// this class is strictly for sending a column's info. it does not have a purpose or actions beyond that.
/// </summary>
public class ColumnToSend
{
    public int maxTasks;
    public LinkedList<Task> tasks;

    public ColumnToSend(Column c)
    {
        this.maxTasks = c.MaxTasks;
        this.tasks = c.Tasks;
    }
    
    [JsonConstructor]
    public ColumnToSend(int maxTasks, LinkedList<Task> tasks)
    {
        this.maxTasks = maxTasks;
        this.tasks = tasks;
    }

    public int MaxTasks => maxTasks;

    public LinkedList<Task> Tasks => tasks;
}