using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer;
/// <summary>
/// this class is the controller class for all BoardMember DTO related actions and is used to communicate with the database (DB).
/// every line in the DB represents a single BoardMember.
/// this class should have only 1 entity at any runtime.
/// </summary>
public class BoardMemberController
{
    private const string TableName = "BOARDMEMBER";
    private readonly string connectionString;
    private readonly string tableName;
    /// <summary>
    /// a constructor method for the controller
    /// </summary>
    public BoardMemberController()
    {
        string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
        this.connectionString = $"Data Source={path}; Version=3;";
        this.tableName = TableName;
    }
    /// <summary>
    /// this function inserts a new line to the DB.
    /// </summary>
    /// <param name="boardMember">the boardMember DTO holding the info to upload to the DB</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Insert(BoardMemberDTO boardMember)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText =
                    $"INSERT INTO {TableName} ({BoardMemberDTO.EMAIL} ,{BoardMemberDTO.BOARDID}) " +
                    $"VALUES (@emailVal,@boardidVal);";

                SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", boardMember.Email);
                SQLiteParameter boardIdParam = new SQLiteParameter(@"boardidVal", boardMember.BoardId);
                command.Parameters.Add(emailParam);
                command.Parameters.Add(boardIdParam);
                command.Prepare();
                res = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { 
                throw new Exception("cant insert BoardMember (SQL)");

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
    /// this function deletes a single line from the DB.
    /// </summary>
    /// <param name="email">the email of the user who left the board</param>
    /// <param name="boardID">the board id of the board that was left</param>
    public bool DeleteOnUserLeaving(string email, int boardID)
    {
        int res = -1;

        using (var connection = new SQLiteConnection(connectionString))
        {
            var command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"DELETE FROM {tableName} WHERE {BoardMemberDTO.BOARDID} = @boardID AND {BoardMemberDTO.EMAIL} = @email"
            };
    
            command.Parameters.AddWithValue("@boardID", boardID);
            command.Parameters.AddWithValue("@email", email);
    
            try
            {
                connection.Open();
                res = command.ExecuteNonQuery();
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
    /// this function deletes all the lines that contain the boards id.
    /// that is because the board itself was deleted. 
    /// </summary>
    /// <param name="boardID">the board id of the board that have been deleted</param>
    public bool DeleteBoard(int boardID)
    {
        int res = -1;

        using (var connection = new SQLiteConnection(connectionString))
        {
            var command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"delete from {tableName} where {BoardMemberDTO.BOARDID}={boardID}"
            };
            try
            {
                connection.Open();
                res = command.ExecuteNonQuery();
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
    /// this function pulls all the lines from the Db and returns them as BoardMemberDTOs.
    /// </summary>
    /// <returns>a LinkedList of all the BoardMemberDTOs</returns>
    /// <exception cref="Exception">throws an exception if it couldn't pull from the DB</exception>
    public LinkedList<BoardMemberDTO> GetAllBoardMembers()
    {
        LinkedList<BoardMemberDTO> results = new LinkedList<BoardMemberDTO>();
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
    /// this function translates the line from the DB to a BoardMemberDTO
    /// </summary>
    /// <param name="reader">an entity that represents the line from the DB</param>
    /// <returns>a BoardMemberDTO</returns>
    private BoardMemberDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        return new BoardMemberDTO(reader.GetString(0),reader.GetInt32(1));
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
                throw e;
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