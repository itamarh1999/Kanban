using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer;
/// <summary>
/// this class is the controller class for all Task DTO related actions and is used to communicate with the database (DB).
/// every line in the DB represents a single Task.
/// this class should have only 1 entity at any runtime.
/// </summary>
public class TaskController
{
    private const string TaskTableName = "TASK";
    private readonly string connectionString;
    private readonly string tableName;
    
    public TaskController() {

        string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
        this.connectionString = $"Data Source={path}; Version=3;";
        this.tableName = TaskTableName;

    }
    public LinkedList<TaskDTO> getAllTasks()
    {
        LinkedList<TaskDTO> results = new LinkedList<TaskDTO>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            command.CommandText = $"select * from {tableName};";
            SQLiteDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    results.AddFirst(ConvertReaderToObject(dataReader));
                }
            }
            catch (Exception e)
            {
                throw new Exception("cant load tasks sql error");
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }

                command.Dispose();
                connection.Close();
            }
        }
        return results;
    }
    public TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        return new TaskDTO(reader.GetInt32(0),reader.GetInt32(1),reader.GetString(2),
            reader.GetString(3),DateTime.Parse(reader.GetString(4)),
            DateTime.Parse(reader.GetString(5)),reader.GetInt32(6),reader.GetString(7));
    }
    /// <summary>
    /// this function inserts a new line to the DB.
    /// </summary>
    /// <param name="task">the taskDTO to add</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Insert(TaskDTO task)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            string CreationTime = $"{task.CreationTime.Day}/{task.CreationTime.Month}/{task.CreationTime.Year}";
            string DueDate = $"{task.DueDate.Day}/{task.DueDate.Month}/{task.DueDate.Year}";
            try
            {
                connection.Open();
                command.CommandText =
                    $"INSERT INTO {TaskTableName} ({TaskDTO.TASKID}, {TaskDTO.BOARDID}, {TaskDTO.TITLE}, {TaskDTO.DESCRIPTION}, " +
                    $"{TaskDTO.DUEDATE}, {TaskDTO.CREATIONDATE}, {TaskDTO.COLUMNORDINAL}, {TaskDTO.ASSIGNEE}) " +
                    $"VALUES (@taskIDVal, @boardId, @titleVal, @descriptionVal, @dueDateVal, @creationDateVal, @columnOrdinalVal, @assigneeVal);";
                
                SQLiteParameter taskIdParam = new SQLiteParameter(@"taskIDVal", task.TaskId);
                SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", task.BoardId);
                SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate.ToString());
                SQLiteParameter creationDateParam = new SQLiteParameter(@"creationDateVal", task.CreationTime.ToString());
                SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", task.ColumnOrdinal);
                SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", task.Assignee);
                command.Parameters.Add(taskIdParam);
                command.Parameters.Add(boardIdParam);
                command.Parameters.Add(titleParam);
                command.Parameters.Add(descriptionParam);
                command.Parameters.Add(dueDateParam);
                command.Parameters.Add(creationDateParam);
                command.Parameters.Add(columnOrdinalParam);
                command.Parameters.Add(assigneeParam);
                command.Prepare();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Insert Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();

            }
            return res > 0;
        }
    }
    /// <summary>
    /// this function updates a line in the DB.
    /// </summary>
    /// <param name="taskID">the task's id</param>
    /// <param name="boardId">the board's id</param>
    /// <param name="newTitle">the new title for the task</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateTitle(int taskID,int boardId, string newTitle)
    {
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"update {tableName} set [{TaskDTO.TITLE}]=@value " +
                              $"where {TaskDTO.TASKID}={taskID} " +
                              $"and {TaskDTO.BOARDID}={boardId} "
            };
            try
            {
                command.Parameters.Add(new SQLiteParameter(@"value", newTitle));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update Title for Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

        }
        return res > 0;
    }
    /// <summary>
    /// this function updates a line in the DB.
    /// </summary>
    /// <param name="taskID">the task's id</param>
    /// <param name="boardId">the board's id</param>
    /// <param name="newDescription">the new Description for the task</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateDescription(int taskID,int boardId, string newDescription)
    {
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"update {tableName} set [{TaskDTO.DESCRIPTION}]=@value " +
                              $"where {TaskDTO.TASKID}={taskID} " +
                              $"and {TaskDTO.BOARDID}={boardId} "
            };
            try
            {

                command.Parameters.Add(new SQLiteParameter(@"value", newDescription));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update Description for Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

        }
        return res > 0;
    }
    /// <summary>
    /// this function updates a line in the DB.
    /// </summary>
    /// <param name="taskID">the task's id</param>
    /// <param name="boardId">the board's id</param>
    /// <param name="newDueDate">the new DueDate for the task</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateDueDate(int taskID,int boardId, DateTime newDueDate)
    {
        string DueDate = $"{newDueDate.Day}/{newDueDate.Month}/{newDueDate.Year}";
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"update {tableName} set [{TaskDTO.DUEDATE}]=@value " +
                              $"where {TaskDTO.TASKID}={taskID} " +
                              $"and {TaskDTO.BOARDID}={boardId} " 
            };
            try
            {
                
                command.Parameters.Add(new SQLiteParameter(@"value", newDueDate));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update Due Date for Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

        }
        return res > 0;
    }
    /// <summary>
    /// this function updates a line in the DB.
    /// </summary>
    /// <param name="taskID">the task's id</param>
    /// <param name="boardId">the board's id</param>
    /// <param name="newAssignee">the new Assignee for the task</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateAssignee(int taskID,int boardId, string newAssignee)
    {
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"update {tableName} set [{TaskDTO.ASSIGNEE}]=@value " +
                              $"where {TaskDTO.TASKID}={taskID} " +
                              $"and {TaskDTO.BOARDID}={boardId} " 
            };
            try
            {

                command.Parameters.Add(new SQLiteParameter(@"value", newAssignee));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update Assignee for Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

        }
        return res > 0;
    }
    /// <summary>
    /// this function updates a line in the DB.
    /// </summary>
    /// <param name="taskID">the task's id</param>
    /// <param name="boardId">the board's id</param>
    /// <param name="newColumnOrdinal">the new ColumnOrdinal for the task</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateColumnOrdinal(int taskID,int boardId, int newColumnOrdinal)
    {
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"update {tableName} set [{TaskDTO.COLUMNORDINAL}]=@value " +
                              $"where {TaskDTO.TASKID}={taskID} " +
                              $"and {TaskDTO.BOARDID}={boardId} " 
            };
            try
            {

                command.Parameters.Add(new SQLiteParameter(@"value", newColumnOrdinal));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update Column Ordinal Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

        }
        return res > 0;
    }
    /// <summary>
    /// this function deletes a row from the DB
    /// </summary>
    /// <param name="task">the task DTO</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Delete(TaskDTO task)
    {
        int res = -1;

        using (var connection = new SQLiteConnection(connectionString))
        {
            var command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"delete from {tableName} where {TaskDTO.TASKID}={task.TaskId}"
            };
            try
            {
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Delete Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

        }
        return res > 0;
    }
    /// <summary>
    /// this function clears the DB from all the information on it.
    /// </summary>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool DeleteAll()
    {
        int res = -1;

        using (var connection = new SQLiteConnection(connectionString))
        {
            var command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"delete from {tableName};"
            };
            try
            {
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("cant Delete All Task (SQL)");
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

        }
        return res > 0;
    }

}