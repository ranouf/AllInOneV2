using AllInOne.Common.Logging;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllInOne.Common.Events
{
    public class DomainEvents : IDomainEvents
    {
        private readonly IComponentContext _container;
        private readonly ILoggerService<DomainEvents> _logger;

        public DomainEvents(IComponentContext container, ILoggerService<DomainEvents> logger)
        {
            _container = container;
            _logger = logger;
        }

        public Task RaiseAsync<T>(T args) where T : IEvent
        {
            var handlers = _container.Resolve<IEnumerable<IEventHandler<T>>>();

            return Task.Factory.StartNew(() => RaiseAction(handlers, args));
        }

        private void RaiseAction<T>(IEnumerable<IEventHandler<T>> handlers, T args) where T : IEvent
        {
            try
            {
                handlers.AsParallel().ForAll(async handler => await handler.HandleAsync(args));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
