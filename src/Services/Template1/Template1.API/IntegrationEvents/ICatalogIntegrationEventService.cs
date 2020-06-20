using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace Template1.API.IntegrationEvents
{
    public interface ITemplate1IntegrationEventService
    {
        Task SaveEventAndTemplate1ContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
