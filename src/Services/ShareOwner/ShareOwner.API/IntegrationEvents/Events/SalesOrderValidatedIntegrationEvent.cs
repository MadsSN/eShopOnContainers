using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class SalesOrderValidatedIntegrationEvent : IntegrationEvent
    {
        public int SalesOrderId { get; private set; }

        public SalesOrderValidatedIntegrationEvent(int salesOrderId)
        {
            SalesOrderId = salesOrderId;
        }
    }
}