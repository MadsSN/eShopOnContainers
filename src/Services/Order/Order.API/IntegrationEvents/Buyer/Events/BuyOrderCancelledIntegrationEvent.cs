using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class BuyOrderCancelledIntegrationEvent : IntegrationEvent
    {
        public BuyOrderCancelledIntegrationEvent(int buyOrderId)
        {
            BuyOrderId = buyOrderId;
        }

        public int BuyOrderId { get; private set; }
    }
}