using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace Order.API.IntegrationEvents
{
    public interface IOrderIntegrationEventService
    {
        Task SaveEventAndOrderContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
