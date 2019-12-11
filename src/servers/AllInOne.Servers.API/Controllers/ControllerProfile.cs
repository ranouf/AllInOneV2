using AllInOne.Common.Paging;
using AllInOne.Servers.API.Controllers.Dtos.Paging;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllInOne.Servers.API.Controllers
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            CreateMap(typeof(PagedResult<>), typeof(PagedResultDto<>));
        }
    }
}
