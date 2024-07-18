using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer;
/// <summary>
/// this class is the controller class for all Column DTO related actions and is used to communicate with the database (DB).
/// every line in the DB represents a single Column.
/// this class should have only 1 entity at any runtime.
/// </summary>
public class ColumnController
{
    private const string TableName = "COLUMN";
    private readonly string connectionString;
    private readonly string tableName;
    /// <summary>
    /// a constructor method for the controller
    /// </summary>
    public ColumnController() {

        string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
        this.connectionString = $"Data Source={path}; Version=3;";
        this.tableName = TableName;

    }
    /// <summary>
    /// this function inserts a new line to the DB.
    /// </summary>
    /// <param name="column">the column DTO holding the info to upload to the DB</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Insert(ColumnDTO column)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText =
                    $"INSERT INTO {TableName} ({ColumnDTO.BOARDID}, {ColumnDTO.COLUMNORDINAL}," +
                    $"{ColumnDTO.MAXTASKS}) " +
                    $"VALUES (@boardIdVal,@columnOrdinalVal,@maxTasksVal);";

                SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", column.BoardId);
                SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", column.ColumnOrdinal);
                SQLiteParameter maxTasksParam = new SQLiteParameter(@"maxTasksVal", column.MaxTasks);
                command.Parameters.Add(boardIdParam);
                command.Parameters.Add(columnOrdinalParam);
                command.Parameters.Add(maxTasksParam);
                command.Prepare();
                res = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("cant insert Column (SQL)");
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
    /// <param name="boardId">the board's id</param>
    /// <param name="columnOrdinal">the columnOrdinal to update the max-tasks in</param>
    /// <param name="newMaxTasks">the value to update to</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateMaxTasks(int boardId, int columnOrdinal, int newMaxTasks)
    {
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"UPDATE {tableName} SET [{ColumnDTO.MAXTASKS}] = @value " +
                              $"WHERE {ColumnDTO.BOARDID} = {boardId} AND {ColumnDTO.COLUMNORDINAL} = {columnOrdinal};"

            };
            try
            {

                command.Parameters.Add(new SQLiteParameter(@"value", newMaxTasks));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update max tasks Column (SQL)");
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
    /// <param name="column">the column DTO</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Delete(ColumnDTO column)
    {
        int res = -1;

        using (var connection = new SQLiteConnection(connectionString))
        {
            var command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"delete from {tableName} " +
                              $"where {ColumnDTO.BOARDID}={column.BoardId} " +
                              $"and {ColumnDTO.COLUMNORDINAL}={column.ColumnOrdinal}"
            };
            try
            {
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Delete Column (SQL)");
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
    /// this function pulls all the lines from the Db and returns them as ColumnDTOs.
    /// </summary>
    /// <returns>a LinkedList of all the ColumnDTOs</returns>
    /// <exception cref="Exception">throws an exception if it couldn't pull from the DB</exception>
    public LinkedList<ColumnDTO> GetAllColumns()
    {
        LinkedList<ColumnDTO> results = new LinkedList<ColumnDTO>();
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
            catch(Exception e)
            {
                throw new Exception("cant Get All Column (SQL)");
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
    /// <summary>
    /// this function translates the line from the DB to a ColumnDTO
    /// </summary>
    /// <param name="reader">an entity that represents the line from the DB</param>
    /// <returns>a ColumnDTO</returns>
    private ColumnDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        return new ColumnDTO(reader.GetInt32(0),reader.GetInt32(1),
            reader.GetInt32(2));
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
            catch(Exception e)
            {
                throw new Exception("cant Delete All Column (SQL)");
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