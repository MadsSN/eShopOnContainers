namespace Microsoft.eShopOnContainers.Services.Fund.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.eShopOnContainers.Services.Fund.API.IntegrationEvents.Events;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;

    public class OrderStatusChangedToPaidIntegrationEventHandler : 
        IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
    {
        private readonly FundContext _catalogContext;
        private readonly ILogger<OrderStatusChangedToPaidIntegrationEventHandler> _logger;

        public OrderStatusChangedToPaidIntegrationEventHandler(
            FundContext catalogContext,
            ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
        {
            _catalogContext = catalogContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                //we're not blocking stock/inventory
                foreach (var orderStockItem in @event.OrderStockItems)
                {
                    var catalogItem = _catalogContext.FundItems.Find(orderStockItem.ProductId);

                    catalogItem.RemoveStock(orderStockItem.Units);
                }

                await _catalogContext.SaveChangesAsync();

            }
        }
    }
}
