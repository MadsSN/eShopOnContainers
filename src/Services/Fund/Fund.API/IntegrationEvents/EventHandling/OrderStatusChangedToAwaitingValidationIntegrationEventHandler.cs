namespace Microsoft.eShopOnContainers.Services.Fund.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using BuildingBlocks.EventBus.Events;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using global::Fund.API.IntegrationEvents;
    using IntegrationEvents.Events;
    using Serilog.Context;
    using Microsoft.Extensions.Logging;

    public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler : 
        IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly FundContext _catalogContext;
        private readonly IFundIntegrationEventService _catalogIntegrationEventService;
        private readonly ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

        public OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
            FundContext catalogContext,
            IFundIntegrationEventService catalogIntegrationEventService,
            ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        {
            _catalogContext = catalogContext;
            _catalogIntegrationEventService = catalogIntegrationEventService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

                foreach (var orderStockItem in @event.OrderStockItems)
                {
                    var catalogItem = _catalogContext.FundItems.Find(orderStockItem.ProductId);
                    var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
                    var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);

                    confirmedOrderStockItems.Add(confirmedOrderStockItem);
                }

                var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
                    ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(@event.OrderId, confirmedOrderStockItems)
                    : new OrderStockConfirmedIntegrationEvent(@event.OrderId);

                await _catalogIntegrationEventService.SaveEventAndFundContextChangesAsync(confirmedIntegrationEvent);
                await _catalogIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);

            }
        }
    }
}
