using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.Services.Order.API.Model;

namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events
{
    public class NewStockOwnerIntegrationEvent : IntegrationEvent
    {
        public int StockId { get; private set; }

        //In principal this should only include the desired/needed 
        public decimal Price { get; private set; }

        public NewStockOwnerIntegrationEvent(int stockId, decimal price)
        {
            StockId = stockId;
            Price = price;
        }
    }

}
