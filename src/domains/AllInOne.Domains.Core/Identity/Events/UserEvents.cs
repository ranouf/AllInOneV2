using AllInOne.Common.Events;
using AllInOne.Domains.Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllInOne.Domains.Core.Identity.Events
{
    public class UserEvent : Event
    {
        public User User { get; set; }
    }

    public class UserCreatedEvent : UserEvent
    {
    }

    public class UserRegisteredEvent : UserEvent
    {
    }

    public class UserUpdatedEvent : UserEvent
    {
    }

    public class UserLockedEvent : UserEvent
    {
    }

    public class UserUnlockedEvent : UserEvent
    {
    }

    public class UserDeletedEvent : UserEvent
    {
    }
}
