using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class SalesOrderCancelledIntegrationEvent : IntegrationEvent
    {
        public SalesOrderCancelledIntegrationEvent(int salesOrderId)
        {
            SalesOrderId = salesOrderId;
        }

        public int SalesOrderId { get; private set; }
    }
}