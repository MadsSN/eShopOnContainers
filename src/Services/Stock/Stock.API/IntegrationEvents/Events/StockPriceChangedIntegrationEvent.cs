using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class StockPriceChangedIntegrationEvent : IntegrationEvent
    {
        public int StockId { get; private set; }

        public decimal OldPrice { get; private set; }

        public decimal NewPrice { get; set; }

        public StockPriceChangedIntegrationEvent(int stockId, decimal oldPrice, decimal newPrice)
        {
            StockId = stockId;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }
}
