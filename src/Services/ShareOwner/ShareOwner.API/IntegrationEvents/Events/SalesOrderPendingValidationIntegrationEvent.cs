using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class SalesOrderPendingValidationIntegrationEvent : IntegrationEvent
    {
        public int SalesOrderId { get; private set; }

        public int StockId { get; private set; }

        public int StockTraderId { get; set; }

        public decimal SharesCount { get; private set; }

        public SalesOrderPendingValidationIntegrationEvent(int salesOrderId, int stockId, int stockTraderId, decimal sharesCount)
        {
            SalesOrderId = salesOrderId;
            StockId = stockId;
            StockTraderId = stockTraderId;
            SharesCount = sharesCount;
        }
    }

    public class SalesOrderFailedValidationIntegrationEvent : IntegrationEvent
    {
        public SalesOrderPendingValidationIntegrationEvent Evt { get; private set; }

        public string Message { get; private set; }

        public SalesOrderFailedValidationIntegrationEvent(SalesOrderPendingValidationIntegrationEvent evt, string message)
        {
            Evt = evt;
            Message = message;
        }
    }
}