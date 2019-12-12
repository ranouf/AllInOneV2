using AllInOne.Common.Paging;
using AllInOne.Servers.API.Controllers.Dtos.Paging;
using AutoMapper;

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
