using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;

namespace IntroSE.Kanban.Backend.buisnessLayer;
/// <summary>
/// a controller class for users.
/// the class has only one field:
/// a LinkedList of all the User entities registered to the system. 
/// </summary>

public class UserFacade
{
    private LinkedList<User> users;
    private UserController userController;
    private static readonly ILog log = 
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public UserFacade()
    {
        users = new LinkedList<User>();
        userController = new UserController();
    }
    /// <summary>
    /// a function that registers a new user using his email and a password of his choice.
    /// </summary>
    /// <param name="email">the user email - will be used as a key for the user</param>
    /// <param name="password"> the user's password of choice</param>
    /// <returns>returns the new user entity created</returns>
    /// <exception cref="Exception">throws an exception if the user didnt enter a proper email address</exception>
    /// <exception cref="Exception">throws an exception if the user entered an email address which already exists in the system</exception>
    public void Register(string email, string password)
    {
        string emailRegex = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        if (!Regex.IsMatch(email,emailRegex))
        {
            log.Error("illegal email");
            throw new Exception("illegal email");
        }
        if ( password.Length < 6 || password.Length > 20 || password.Contains(" "))
        {
            log.Error("illegal password");
            throw new Exception("illegal password");
        }
        User userToAdd = GetUser(email);
        if (userToAdd != null)
        {
            log.Error("tried to register but user allready exist");
            throw new Exception("user with this email allready exists");
        }
        userToAdd = new User(password, email, userController);
        users.AddLast(userToAdd);
        log.Info($"successfully registered {email} ");
    }
    /// <summary>
    /// a function that verifies that the user's email exist in the system and logs the user in using
    /// the function 'Login' in the class 'User'.
    /// </summary>
    /// <param name="email">the user's email with which he used to register into the system</param>
    /// <param name="password">the user password with which he used to register into the system</param>
    /// <returns>returns the user's entity</returns>
    /// <exception cref="Exception">if the email is not registered to the system an exception will be thrown</exception>
    public string Login(string email, string password)
    {
        User user = GetUser(email);
        if (user == null)
        {
            log.Error("user doesnt exist cant login");
            throw new Exception("user doesn't exist");
        }
        return user.login(email,password);
    }
    /// <summary>
    /// a function that checks if the user is currently logged in using the method 'isLoggedIn' in the class 'User'.
    /// </summary>
    /// <param name="email">the user's email with which he used to register into the system</param>
    /// <returns>a true or false value</returns>
    /// <exception cref="Exception">if the email is not registered to the system an exception will be thrown</exception>
    public bool IsLogedIn(string email)
    {
        User user = GetUser(email);
        if (user == null){
            log.Error("user doesnt exist");
            throw new Exception("user does not exist");
        }
    return user.isLoggedIn();
    }
    /// <summary>
    /// a function that logs a user out using the method 'Logout' in the class 'User'.
    /// </summary>
    /// <param name="email">the user's email with which he used to register into the system</param>
    /// <exception cref="Exception">if the email is not registered to the system an exception will be thrown</exception>
    public void Logout(string email)
    {
        User user = GetUser(email);
        if (user == null)
            throw new Exception("user does not exist");
        user.logout();
    }
    /// <summary>
    /// a getter for a specific user from UserFacade's list using an email address as a key.
    /// </summary>
    /// <param name="email">the user's email with which he used to register into the system</param>
    /// <returns>the User entity that was found (if exists), else will return null</returns>
    public User GetUser(string email)
    {
        foreach (User user in users)
            if (user.getEmail().Equals(email))
                return user;
        return null;   
        
    }
    /// <summary>
    /// this function calls the corresponding function in User in order to add a new board to his 'boards' list. 
    /// </summary>
    /// <param name="email">the user's email</param>
    /// <param name="boardID">the board ID</param>
    public void addBoardToUser(string email, int boardID)
    {
        User u = GetUser(email);
        u.addBoardToUser(boardID);
    }
    /// <summary>
    /// this function calls the corresponding function in User in order to remove a board from his 'boards' list. 
    /// </summary>
    /// <param name="email">the user's email</param>
    /// <param name="boardID">the board ID</param>
    public void removeBoardFromUser(string email, int boardID)
    {
        User u = GetUser(email);
        u.removeBoardFromUser(boardID);
    }
    /// <summary>
    /// this function gets all of the board IDs that the user is registered to.
    /// </summary>
    /// <param name="email">the user's email</param>
    /// <returns>a linked list of ints representing the boards IDs</returns>
    public LinkedList<int> GetUserBoards(string email)
    {
        User user = GetUser(email); // check if user is connected as well
        if (user == null)
        {
            throw new Exception($"{email} - does not exist");
        }
        return user.getUserBoards();
    }
    /// <summary>
    /// this function uses a function from boardController that returns a list of all the board DTOs that it holds.
    /// it then creates a new user entity for each DTO and adds iot to the list of users. 
    /// </summary>
    public void LoadUsers()
    {
        LinkedList<UserDTO> userDtos = userController.GetAllUsers();
        foreach (UserDTO userDto in userDtos)
        {
            userDto.setController(userController);
            users.AddLast(new User(userDto));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void DeleteData()
    {
        userController.DeleteAll();
    }

    public LinkedList<User> GetUsers()
    {
        return users;
    }
}
