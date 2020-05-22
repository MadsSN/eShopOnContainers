using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class StockTraderCreatedIntegrationEvent : IntegrationEvent
    {
        public int StockTraderId { get; private set; }

        //In principal this should only include the desired/needed 
        public string Name { get; private set; }


        public StockTraderCreatedIntegrationEvent(int stockTraderId, string name)
        {
            StockTraderId = stockTraderId;
            Name = name;
        }
    }
}
