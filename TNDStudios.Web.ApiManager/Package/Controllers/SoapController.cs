using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Web.ApiManager.Controllers
{
    public class SoapManagedController : ManagedController
    {
        public SoapManagedController(ILogger<ManagedController> logger) : base(logger)
        {
        }
    }
}
