using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TNDStudios.Web.ApiManager.Controllers
{
    /// <summary>
    /// Managed controller to handle passing context of the user etc. to the 
    /// controllers that inherit it 
    /// </summary>
    public class ManagedController : ControllerBase
    {
        public ILogger Logger { get; set; }

        public ManagedController(ILogger logger)
            => Logger = logger;

    }
}
