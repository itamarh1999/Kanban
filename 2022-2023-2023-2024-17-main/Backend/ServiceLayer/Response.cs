namespace IntroSE.Kanban.Backend.ServiceLayer;
/// <summary>
/// this class is the response format that is in used for communication.
/// it is the format that will be serialized into a JSON and deserialized from JSON.
/// it will hold the data to be transferred to the front-end and back-end. 
/// </summary>
public class Response
{
    /// <summary>
    /// the data will be in the return value.
    /// </summary>
    public object ReturnValue { get; set; }
    /// <summary>
    /// if an error message occurs it will be in here.
    /// </summary>
    public string ErrorMessage { get; set; }
    
    
    public Response(){}

    public Response(object res)
    {
        ReturnValue = res;
    }
    
    public Response(string msg)
    {
        ErrorMessage = msg;
    }
    
    public Response(object res, string msg)
    {
        ReturnValue = res;
        ErrorMessage = msg;
    }

    public bool ErrorOccured()
    {
        return ErrorMessage != null;
    }


}