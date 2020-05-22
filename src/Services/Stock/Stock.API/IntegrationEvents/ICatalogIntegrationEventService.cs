using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace Stock.API.IntegrationEvents
{
    public interface IStockIntegrationEventService
    {
        Task SaveEventAndStockContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
