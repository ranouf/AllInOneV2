using AllInOne.Common.Paging;
using AllInOne.Servers.API.Controllers.Dtos.Paging;
using AutoMapper;

namespace AllInOne.Servers.API.Controllers.Dtos
{
    public class DtosProfile : Profile
    {
        public DtosProfile()
        {
            CreateMap(typeof(PagedResult<>), typeof(PagedResultDto<>));
            CreateMap(typeof(PagedResult<,>), typeof(PagedResultDto<,>));
            CreateMap(typeof(PagedResult<>), typeof(PagedResultDto<,>));
        }
    }
}
