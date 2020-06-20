using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class SalesOrderFailedIntegrationEvent : IntegrationEvent
    {
        public int SalesOrderId { get; private set; }

        public SalesOrderFailedIntegrationEvent(int salesOrderId)
        {
            SalesOrderId = salesOrderId;
        }
    }
}