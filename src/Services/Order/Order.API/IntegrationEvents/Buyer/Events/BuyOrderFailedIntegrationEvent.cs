using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class BuyOrderFailedIntegrationEvent : IntegrationEvent
    {
        public int BuyOrderId { get; private set; }

        public BuyOrderFailedIntegrationEvent(int buyOrderId)
        {
            BuyOrderId = buyOrderId;
        }
    }
}