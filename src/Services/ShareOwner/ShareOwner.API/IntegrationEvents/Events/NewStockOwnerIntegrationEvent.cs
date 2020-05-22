using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class NewStockOwnerIntegrationEvent : IntegrationEvent
    {
        public int StockId { get; private set; }

        public decimal Shares { get; private set; }

        public int StockTraderId { get; set; }

        public NewStockOwnerIntegrationEvent(int stockId, decimal shares, int stockTraderId)
        {
            StockId = stockId;
            Shares = shares;
            StockTraderId = stockTraderId;
        }
    }
}
