using System;
using System.Collections.Generic;
using System.Text.Json;

    namespace IntroSE.Kanban.Backend.ServiceLayer;
    /// <summary>
    /// a class in the service layer that communicates with the front-end through JSON and with the business layer.
    /// for every function there is a JSON serializer for the communication to be sent,
    /// either as an answer or as an error message.
    /// all the functions in this class send the functions to User facade in the business layer.
    /// </summary>
    public class UserService
    {
        private StartSession startSession;
        public UserService(StartSession startSession)
        {
            this.startSession = startSession;
        }
        /// <summary>
        ///  this function sends the data to the the Register function in User facade.
        ///  then it serializes the response into a JSON and sends it back. 
        /// </summary>
        public string Register(string email, string password)
        {
            try
            {
                startSession.boardFacade.Register(email, password);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message);
                return JsonSerializer.Serialize(res);

            }
        
        }
        /// <summary>
        ///  this function sends the data to the the Login function in User facade.
        ///  then it serializes the response into a JSON and sends it back. 
        /// </summary>
        public string Login(string email, string password)
        {
            try
            {
                string eMail = startSession.userFacade.Login(email, password);
                Response r = new Response(eMail, null);
                return JsonSerializer.Serialize(r);
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message);
                return JsonSerializer.Serialize(res);
            }   
        }
        /// <summary>
        ///  this function sends the data to the the Logout function in User facade.
        ///  then it serializes the response into a JSON and sends it back. 
        /// </summary>
        public string Logout(string email)
        {
            try
            {
                startSession.userFacade.Logout(email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message);
                return JsonSerializer.Serialize(res);

            }
        }
        /// <summary>
        ///  this function sends the data to the the GetUserBoards function in user facade.
        ///  then it serializes the response into a JSON and sends it back. 
        /// </summary>
        public string GetUserBoards(string email)
        {
            try
            {
                LinkedList<int> boards = startSession.userFacade.GetUserBoards(email);
                return JsonSerializer.Serialize(new Response(boards,null));
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message);
                return JsonSerializer.Serialize(res);

            }
        }
    }