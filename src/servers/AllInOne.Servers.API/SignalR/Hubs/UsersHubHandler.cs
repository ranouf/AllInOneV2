using AllInOne.Common.Events;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Domains.Core.Identity.Events;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;

namespace AllInOne.Api.SignalR.Hubs
{
    public class UsersHubHandler :
        BaseHubHandler<BaseHub>,
        IEventHandler<UserCreatedEvent>,
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

        public void Handle(UserCreatedEvent args)
        {
            SendNotification<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public void Handle(UserUpdatedEvent args)
        {
            SendNotification<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public void Handle(UserLockedEvent args)
        {
            SendNotification<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public void Handle(UserUnlockedEvent args)
        {
            SendNotification<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }

        public void Handle(UserDeletedEvent args)
        {
            SendNotification<Guid?, User, UserDto>(args, args.User, args.User.FullName);
        }
    }
}
