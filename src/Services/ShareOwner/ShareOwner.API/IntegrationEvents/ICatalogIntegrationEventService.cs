using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace ShareOwner.API.IntegrationEvents
{
    public interface IShareOwnerIntegrationEventService
    {
        Task SaveEventAndShareOwnerContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
