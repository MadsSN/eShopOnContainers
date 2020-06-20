using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class BuyOrderPendingValidationIntegrationEvent : IntegrationEvent
    {
        public int BuyOrderId { get; private set; }
        public int StockTraderId { get; private set; }

        public int StockId { get; private set; }

        public decimal SharesCount { get; private set; }

        public decimal PricePerShare { get; private set; }
        public BuyOrderPendingValidationIntegrationEvent(int buyOrderId, int stockTraderId, int stockId, decimal sharesCount, decimal pricePerShare)
        {
            BuyOrderId = buyOrderId;
            StockTraderId = stockTraderId;
            StockId = stockId;
            SharesCount = sharesCount;
            PricePerShare = pricePerShare;
        }
    }
}