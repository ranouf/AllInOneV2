using AllInOne.Common.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AllInOne.Servers.API.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        public IMapper Mapper { get; }
        public ILoggerService Logger { get; }

        public BaseController(IMapper mapper, ILoggerService logger)
        {
            Mapper = mapper;
            Logger = logger;
        }
    }
}
