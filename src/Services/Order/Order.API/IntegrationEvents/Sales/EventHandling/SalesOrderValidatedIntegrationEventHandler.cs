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

    public class SalesOrderValidatedIntegrationEventHandler : 
        IIntegrationEventHandler<SalesOrderValidatedIntegrationEvent>
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger<SalesOrderValidatedIntegrationEventHandler> _logger;

        public SalesOrderValidatedIntegrationEventHandler(
            OrderContext orderContext,
            ILogger<SalesOrderValidatedIntegrationEventHandler> logger)
        {
            _orderContext = orderContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(SalesOrderValidatedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                SalesOrder order = await _orderContext.SalesOrders.SingleOrDefaultAsync(order => order.Id == @event.SalesOrderId);

                if (order.Status == OrderStatus.PendingValidation)
                {
                    order.Status = OrderStatus.PendingMatch;
                    _orderContext.SalesOrders.Update(order);
                    await _orderContext.SaveChangesAsync();
                }
            }
        }
    }
}
