using AllInOne.Common.Entities;
using AllInOne.Common.Events;
using AllInOne.Common.Extensions;
using AllInOne.Servers.API.Attributes;
using AllInOne.Servers.API.Controllers.Dtos.Entities;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;

namespace AllInOne.Api.SignalR.Hubs
{
    [AuthorizeAdministratorAndManagers]
    public class BaseHubHandler<THub> where THub : Hub
    {
        private readonly IConnectionService _connectionService;
        private readonly IHubContext<THub> _connectionManager;
        private readonly IMapper _mapper;

        public BaseHubHandler(
            IConnectionService connectionService,
            IHubContext<THub> connectionManager,
            IMapper mapper
        )
        {
            _connectionService = connectionService;
            _connectionManager = connectionManager;
            _mapper = mapper;
        }

        public void SendNotification<TPrimaryKeyDto, TSource, TDestination>(
            IEvent @event,
            TSource source,
            string label
        )
            where TSource : IEntity
            where TDestination : IEntityDto<TPrimaryKeyDto>
        {
            var dto = _mapper.Map<TSource, TDestination>(source);

            if (source.IsAssignableToGenericType(typeof(IDeleteAudited<>))
                && source.TryGetPropertyValue<Guid?>("DeletedByUserId", out var deletedByUserId)
                && deletedByUserId.HasValue
            )
            {
                var result = _connectionService.GetAllExcept($"{deletedByUserId.Value}");
                _connectionManager.Clients.Clients(result)
                    .SendAsync(@event.Action, dto)
                    .Wait();
            }
            else if (source.IsAssignableToGenericType(typeof(IUpdateAudited<>))
               && source.TryGetPropertyValue<Guid?>("UpdatedByUserId", out var updatedByUserId)
               && updatedByUserId.HasValue
           )
            {
                var result = _connectionService.GetAllExcept($"{updatedByUserId.Value}");
                _connectionManager.Clients.Clients(result)
                    .SendAsync(@event.Action, dto)
                    .Wait();
            }
            else if (source.IsAssignableToGenericType(typeof(ICreationAudited<>))
                && source.TryGetPropertyValue<Guid?>("CreatedByUserId", out var createdByUserId)
                && createdByUserId.HasValue
            )
            {
                var result = _connectionService.GetAllExcept($"{createdByUserId.Value}");
                _connectionManager.Clients.Clients(result)
                    .SendAsync(@event.Action, dto)
                    .Wait();
            }
            else
            {
                _connectionManager.Clients.All
                    .SendAsync(@event.Action, dto)
                    .Wait();
            }
        }
    }
}
