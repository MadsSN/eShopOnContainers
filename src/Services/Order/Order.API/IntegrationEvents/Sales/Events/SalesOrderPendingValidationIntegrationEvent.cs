using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class SalesOrderPendingValidationIntegrationEvent : IntegrationEvent
    {
        public int SalesOrderId { get; private set; }
        public int StockTraderId { get; private set; }

        public int StockId { get; private set; }

        public decimal SharesCount { get; private set; }

        public SalesOrderPendingValidationIntegrationEvent(int salesOrderId, int stockTraderId, int stockId, decimal sharesCount)
        {
            SalesOrderId = salesOrderId;
            StockTraderId = stockTraderId;
            StockId = stockId;
            SharesCount = sharesCount;
        }
    }
}