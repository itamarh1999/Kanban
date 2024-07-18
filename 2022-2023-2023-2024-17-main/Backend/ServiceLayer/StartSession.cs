using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.buisnessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer;
/// <summary>
/// this class is used for starting the runtime of the system.
/// </summary>
public class StartSession
{
    public UserFacade userFacade;
    public BoardFacade boardFacade;
    public StartSession()
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        this.userFacade = new UserFacade();
        this.boardFacade = new BoardFacade(userFacade);
    }

    public void LoadData()
    {
        boardFacade.LoadData();
    }
    
}