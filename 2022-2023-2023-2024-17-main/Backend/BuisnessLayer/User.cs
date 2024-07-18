using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;

namespace IntroSE.Kanban.Backend.buisnessLayer;
/// <summary>
/// a class that defines the User entity.
/// the entity has the following attributes:
/// 1. string: an email address - used as the key value.
/// 2. string: a password - used to identify the user upon login.
/// 3. bool: a boolean value used to distinguish between when a user is logged in or logged out. 
/// </summary>
public class User
{
    private string password;
    private string email;
    private bool loggedIn;
    private UserDTO UserDTO;
    private LinkedList<int> boards;
    private static readonly ILog log = 
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public LinkedList<int> Boards
    {
        get => boards;
    }
    //--------------------methodes------------------
    public User(string password, string email, UserController userController)
    {
        this.password = password;
        this.email = email;
        this.loggedIn = true;
        this.UserDTO = new UserDTO(email,password,userController);
        this.boards = new LinkedList<int>();
    }
    
    public User(string password, string email, UserController userController, bool logged)
    {
        this.password = password;
        this.email = email;
        this.loggedIn = logged;
        this.UserDTO = new UserDTO(email,password,userController);
    }

    public User(UserDTO userDto)
    {
        this.password = userDto.Password;
        this.email = userDto.Email;
        this.loggedIn = false;
        this.UserDTO = userDto;
        boards = new LinkedList<int>();
    }

    /// <summary>
    /// a function that compares the entity 'email' and 'password' fields to the 'email' and 'password' string given
    /// to it, in order to verify the user. once the verification is complete the entity's boolean field 'loggedIn' will
    /// turn to true. 
    /// </summary>
    /// <param name="email">the email address given to the function for verification</param>
    /// <param name="password">the password given to the function for verification</param>
    /// <returns>returns the email address given to it</returns>
    /// <exception cref="Exception">if the verification failed for any reason (wrong email or wrong password)
    /// an exception will be thrown</exception>
    public string login(string email, string password)
    {
        if (loggedIn)
        {
            log.Error("user allready logged in");
            throw new Exception("allready logged in");
        }
        if (this.email.Equals(email) && this.password.Equals(password))
            loggedIn = true;
        else
        {
            log.Error("failed to login wrong email or password");
            throw new Exception("wrong email or password");
        }
        log.Info($"user {email} logged in successfully ");
        return email;


    }
    /// <summary>
    /// a getter for the value of 'loggedIn'.
    /// </summary>
    /// <returns>a boolean for if the user is currently logged in</returns>
    public bool isLoggedIn()
    {
        return this.loggedIn;
    }
    /// <summary>
    /// a function that logs out the user, no need for verification.
    /// </summary>
    /// <returns>returns true</returns>
    /// <exception cref="Exception">an exception will be thrown if a user is already logged out</exception>
    public void logout()
    {
        if (!loggedIn) throw new Exception("allready logged out");
        this.loggedIn = false;
        log.Info($"user {email} logged out");
    }
    /// <summary>
    /// a getter for the entity's email field.
    /// </summary>
    /// <returns>string: the email used as a key (the one used to register to the system</returns>
    public string getEmail()
    {
        return this.email;
    }
    /// <summary>
    /// this function checks that the board is not already in the users list and adds him
    /// </summary>
    /// <param name="boardID">the boards ID</param>
    /// <exception cref="Exception">throws an exception if the board is already registered in the user's list</exception>
    public void addBoardToUser(int boardID)
    {
        if (boards.Contains(boardID))
        {
            log.Error("board is already registered to this user");
            throw new Exception("board is already registered to this user");
        }
        else
        {
            boards.AddFirst(boardID);
        }
    }
    /// <summary>
    /// this function checks that the board is in the users list and removes him
    /// </summary>
    /// <param name="boardID">the boards ID</param>
    /// <exception cref="Exception">throws an exception if the board is not registered in the user's list</exception>
    public void removeBoardFromUser(int boardID)
    {
        if (boards.Contains(boardID))
        {
            boards.Remove(boardID);
        }
        else
        {
            log.Error("board is not registered to the user");
            throw new Exception("board is not registered to the user");
        }
    }
    /// <summary>
    /// this function gets all the board ids from the boards the user is registered for.
    /// </summary>
    /// <returns>a LinkedList containing all the ids</returns>
    public LinkedList<int> getUserBoards()
    {
        return boards;
    }
}