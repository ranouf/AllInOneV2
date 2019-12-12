using System.Threading.Tasks;

namespace AllInOne.Common.Events
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(T args);
    }
}
