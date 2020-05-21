using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace Fund.API.IntegrationEvents
{
    public interface IFundIntegrationEventService
    {
        Task SaveEventAndFundContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
