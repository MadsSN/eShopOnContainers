using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class StockTraderCreatedIntegrationEvent : IntegrationEvent
    {
        public int StockTraderId { get; private set; }

        public string Name { get; private set; }


        public StockTraderCreatedIntegrationEvent(int stockTraderId, string name)
        {
            StockTraderId = stockTraderId;
            Name = name;
        }
    }
}