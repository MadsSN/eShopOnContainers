using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class StockTraderCreatedIntegrationEvent : IntegrationEvent
    {
        public int StockTraderId { get; private set; }
        
        public StockTraderCreatedIntegrationEvent(int stockTraderId)
        {
            StockTraderId = stockTraderId;
        }
    }
}
