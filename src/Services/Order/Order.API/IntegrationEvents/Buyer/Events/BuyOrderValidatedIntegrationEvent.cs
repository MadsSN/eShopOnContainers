using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class BuyOrderValidatedIntegrationEvent : IntegrationEvent
    {
        public int BuyOrderId { get; private set; }

        public BuyOrderValidatedIntegrationEvent(int buyOrderId)
        {
            BuyOrderId = buyOrderId;
        }
    }
}