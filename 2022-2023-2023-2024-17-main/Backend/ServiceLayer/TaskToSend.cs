using System;
using IntroSE.Kanban.Backend.buisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer;
/// <summary>
/// this class is strictly for sending a task's info. it does not have a purpose or actions beyond that.
/// </summary>
public class TaskToSend
{
    public int Id{ get; set; }
    public DateTime CreationTime{ get; set; }
    public string Title{ get; set; }
    public string Description{ get; set; }
    public DateTime DueDate{ get; set; }


    public TaskToSend()
    {
    }

    public TaskToSend(Task task)
    {
        this.Id = task.TaskId;
        this.CreationTime = task.CreationTime;
        this.Title = task.Title;
        this.Description = task.Description;
        this.DueDate = task.DueDate;
    }
}