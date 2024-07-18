using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

public class UserDTO
{
    public const string USEREMAIL = "EMAIL";
    public const string USERPASSWORD = "PASSWORD";

    private string email ;
    private string password;
    private UserController userController;
    /// <summary>
    /// this is a constructor method for a userDTO
    /// </summary>
    /// <param name="email">the user's email</param>
    /// <param name="password">the user's password</param>
    /// <param name="userController">the userController</param>
    public UserDTO(string email, string password, UserController userController)
    {
        this.email = email;
        this.password = password;
        this.userController = userController;
        Insert();
    }
    /// <summary>
    /// this is a constructor method for a userDTO
    /// </summary>
    /// <param name="email">the user's email</param>
    /// <param name="password">the user's password</param>
    public UserDTO(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
    /// <summary>
    /// a setter for the userController
    /// </summary>
    /// <param name="userController">the userController</param>
    public void setController(UserController userController)
    {
        this.userController = userController;
    }
    public string Email {get => email;}
    public string Password { get => password; }
    /// <summary>
    /// this method calls the getAllUsers in userController
    /// </summary>
    /// <returns>returns a linked-list containing all the user DTOs</returns>
    public LinkedList<UserDTO> getAllUsers()
    {
       return userController.GetAllUsers();
    }
    /// <summary>
    /// this method calls the Insert in userController
    /// </summary>
    public void Insert()
    {
        userController.Insert(this);
    }
    /// <summary>
    /// this method calls the Delete in userController
    /// </summary>
    public void Delete()
    {
        userController.Delete(this);
    }
    /// <summary>
    /// this method calls the DeleteAll in userController
    /// </summary>
    public void DeleteAll()
    {
        userController.DeleteAll();
    }
}