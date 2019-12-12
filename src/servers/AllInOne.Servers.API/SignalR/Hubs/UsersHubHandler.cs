using AllInOne.Common.Events;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Domains.Core.Identity.Events;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace AllInOne.Api.SignalR.Hubs
{
    public class UsersHubHandler :
        BaseHubHandler<BaseHub>,
        IEventHandler<UserRegisteredEvent>,
        IEventHandler<UserCreatedEvent>,
        IEventHandler<UserLockedEvent>,
        IEventHandler<UserUnlockedEvent>,
        IEventHandler<UserUpdatedEvent>,
        IEventHandler<UserDeletedEvent>
    {
        public UsersHubHandler(
            IConnectionService connectionService,
            IHubContext<BaseHub> connectionManager,
            IMapper mapper
        ) : base(connectionService, connectionManager, mapper)
        {
        }

        public async Task HandleAsync(UserRegisteredEvent args)
        {
            await SendNotificationAsync<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public async Task HandleAsync(UserCreatedEvent args)
        {
            await SendNotificationAsync<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public async Task HandleAsync(UserUpdatedEvent args)
        {
            await SendNotificationAsync<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public async Task HandleAsync(UserLockedEvent args)
        {
            await SendNotificationAsync<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public async Task HandleAsync(UserUnlockedEvent args)
        {
            await SendNotificationAsync<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public async Task HandleAsync(UserDeletedEvent args)
        {
            await SendNotificationAsync<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }
    }
}
