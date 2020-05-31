using System.Threading.Tasks;

namespace AspNetCore.EventBus
{
    public interface IDynamicEventHandler
    {
        Task Handle(dynamic eventData);
    }
}