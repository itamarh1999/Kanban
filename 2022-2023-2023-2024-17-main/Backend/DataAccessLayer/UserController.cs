
using System;
using System.Collections;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer;
public class UserController
{
    private const string TableName = "USER";
    private readonly string connectionString;
    private readonly string tableName;
    /// <summary>
    /// a constructor method for the controller
    /// </summary>
    public UserController() {

        string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
        this.connectionString = $"Data Source={path}; Version=3;";
        this.tableName = TableName;

    }
    /// <summary>
    /// this function pulls all the lines from the Db and returns them as userDTOs.
    /// </summary>
    /// <returns>a LinkedList of all the userDTOs</returns>
    /// <exception cref="Exception">throws an exception if it couldn't pull from the DB</exception>
    public LinkedList<UserDTO> GetAllUsers()
    {
        LinkedList<UserDTO> results = new LinkedList<UserDTO>();
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
            catch
            {
                throw new Exception("cant read USERS from sql");
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
    private UserDTO ConvertReaderToObject(SQLiteDataReader reader)
    {
        return new UserDTO(reader.GetString(0), reader.GetString(1));
    }
    /// <summary>
    /// this function inserts a new line to the DB.
    /// </summary>
    /// <param name="users">the user DTO holding the info to upload to the DB</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Insert(UserDTO users)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            SQLiteCommand command = new SQLiteCommand(null, connection);
            int res = -1;
            try
            {
                connection.Open();
                command.CommandText =
                    $"INSERT INTO {TableName} ({UserDTO.USEREMAIL} ,{UserDTO.USERPASSWORD}) " +
                    $"VALUES (@emailVal,@passwordVal);";

                SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", users.Email);
                SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", users.Password);
                command.Parameters.Add(emailParam);
                command.Parameters.Add(passwordParam);
                command.Prepare();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("Insert User (SQL)");
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
    /// this function deletes a row from the DB
    /// </summary>
    /// <param name="users">the user DTO</param>
    /// <exception cref="Exception">throws an exception if it couldn't update the DB</exception>
    public bool Delete(UserDTO users)
    {
        int res = -1;

        using (var connection = new SQLiteConnection(connectionString))
        {
            var command = new SQLiteCommand
            {
                Connection = connection,
                CommandText = $"delete from {tableName} where {UserDTO.USEREMAIL}={users.Email}"
            };
            try
            {
                connection.Open();
                res = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("cant Delete User (SQL)");
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
            catch(Exception e)
            {
                throw new Exception("cant Delete All User (SQL)");
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