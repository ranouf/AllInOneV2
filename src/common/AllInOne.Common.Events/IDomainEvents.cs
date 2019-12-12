using System.Threading.Tasks;

namespace AllInOne.Common.Events
{
    public interface IDomainEvents
    {
        Task RaiseAsync<T>(T args) where T : IEvent;
    }
}
