using System.Threading.Tasks;

namespace AspNetCore.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : Event
    {
        Task Handle(TEvent @event);
    }
}