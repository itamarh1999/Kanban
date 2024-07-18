using System;
using System.Collections.Generic;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer;
/// <summary>
/// this class is the controller class for all board DTO related actions and is used to communicate with the database (DB).
/// every line in the DB represents a single board.
/// this class should have only 1 entity at any runtime.
/// </summary>
public class BoardController
{
    private const string TableName = "BOARD";
    private readonly string connectionString;
    private readonly string tableName;
    /// <summary>
    /// a constructor method for the controller
    /// </summary>

    public BoardController() {
        string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
        this.connectionString = $"Data Source={path}; Version=3;";
        this.tableName = TableName;
    }
    /// <summary>
    /// this function inserts a new line to the DB.
    /// </summary>
    /// <param name="boards">the board DTO holding the info to upload to the DB</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Insert(BoardDTO boards)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText =
                    $"INSERT INTO {TableName} ({BoardDTO.BOARDNAME}, {BoardDTO.BOARDID}," +
                                $"{BoardDTO.OWNEREMAIL}, {BoardDTO.TASKCOUNTER}) " +
                            $"VALUES (@nameVal,@idVal,@ownerEmailVal,@taskCounterVal);";

                SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", boards.Name);
                SQLiteParameter idParam = new SQLiteParameter(@"idVal", boards.Id);
                SQLiteParameter ownerEmailParam = new SQLiteParameter(@"ownerEmailVal", boards.OwnerEmail);
                SQLiteParameter taskCounterParam = new SQLiteParameter(@"taskCounterVal", boards.TastCounter);
                command.Parameters.Add(nameParam);
                command.Parameters.Add(idParam);
                command.Parameters.Add(ownerEmailParam);
                command.Parameters.Add(taskCounterParam);
                command.Prepare();
                res = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("cant insert Board (SQL)");
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
    /// this function delete a line from the DB.
    /// </summary>
    /// <param name="boards">the board DTO holding the info to upload to the DB</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Delete(BoardDTO boards)
    {
        int res = -1;

        using (var connection = new SQLiteConnection(connectionString))
        {
            var command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"delete from {tableName} where {BoardDTO.BOARDID}={boards.Id};"
            };
            try
            {
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Delete Board (SQL)");
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
    /// it updates the owner column in the DB.
    /// </summary>
    /// <param name="boardId">the board's id</param>
    /// <param name="newOwner">the new owners email</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateOwner(int boardId, string newOwner)
    {
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"update {tableName} " +
                              $"set [{BoardDTO.OWNEREMAIL}]= @value " +
                              $"where {BoardDTO.BOARDID}={boardId}"
            };
            try
            {

                command.Parameters.Add(new SQLiteParameter(@"value", newOwner));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update Owner Board (SQL)");
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
    /// it updates the TaskCounter column in the DB.
    /// </summary>
    /// <param name="boardId">the board's id</param>
    /// <param name="newTaskCounter">the new task counter</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool UpdateTaskCounter(int boardId, int newTaskCounter)
    {
        int res = -1;
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand
            {
                Connection = connection,

                CommandText = $"UPDATE {tableName} SET [{BoardDTO.TASKCOUNTER}] = @value WHERE {BoardDTO.BOARDID} = {boardId};"
            };
            try
            {

                command.Parameters.Add(new SQLiteParameter(@"value", newTaskCounter));
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Update Task Counter Board (SQL)");
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
    /// this function pulls all the lines from the Db and returns them as BoardDTOs.
    /// </summary>
    /// <returns>a LinkedList of all the boardDTOs</returns>
    /// <exception cref="Exception">throws an exception if it couldn't pull from the DB</exception>
    public LinkedList<BoardDTO> GetAllBoard()
    {
        LinkedList<BoardDTO> results = new LinkedList<BoardDTO>();
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
                throw new Exception("cant Get Board (SQL)");
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
    /// this function translates the line from the DB to a boardDTO
    /// </summary>
    /// <param name="reader">an entity that represents the line from the DB</param>
    /// <returns>a boardDTO</returns>
    private BoardDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        return new BoardDTO(reader.GetString(1),reader.GetInt32(0)
            ,reader.GetString(2),reader.GetInt32(3));
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
                throw new Exception("cant Delete Board (SQL)");
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