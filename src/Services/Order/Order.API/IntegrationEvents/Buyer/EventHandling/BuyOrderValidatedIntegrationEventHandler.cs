using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.Order.API.Model;

namespace Microsoft.eShopOnContainers.Services.Order.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;

    public class BuyOrderValidatedIntegrationEventHandler : 
        IIntegrationEventHandler<BuyOrderValidatedIntegrationEvent>
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger<BuyOrderValidatedIntegrationEventHandler> _logger;

        public BuyOrderValidatedIntegrationEventHandler(
            OrderContext orderContext,
            ILogger<BuyOrderValidatedIntegrationEventHandler> logger)
        {
            _orderContext = orderContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(BuyOrderValidatedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                BuyOrder order = await _orderContext.BuyOrders.SingleOrDefaultAsync(order => order.Id == @event.BuyOrderId);

                if (order.Status == OrderStatus.PendingValidation)
                {
                    order.Status = OrderStatus.PendingMatch;
                    _orderContext.BuyOrders.Update(order);
                    await _orderContext.SaveChangesAsync();
                }
            }
        }
    }
}
