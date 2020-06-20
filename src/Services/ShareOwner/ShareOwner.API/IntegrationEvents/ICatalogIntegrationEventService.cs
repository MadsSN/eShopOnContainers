using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace ShareOwner.API.IntegrationEvents
{
    public interface IShareOwnerIntegrationEventService
    {
        Task SaveEventAndShareOwnerContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
        Task SaveEventAsync(IntegrationEvent @event);
    }
}
