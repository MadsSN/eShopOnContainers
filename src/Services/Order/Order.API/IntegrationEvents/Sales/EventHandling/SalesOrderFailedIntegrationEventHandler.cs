using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.Order.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Order.API.Model;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Microsoft.eShopOnContainers.Services.Order.API.IntegrationEvents.EventHandling
{
    public class SalesOrderFailedIntegrationEventHandler :
        IIntegrationEventHandler<SalesOrderFailedIntegrationEvent>
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger<SalesOrderFailedIntegrationEventHandler> _logger;

        public SalesOrderFailedIntegrationEventHandler(
            OrderContext orderContext,
            ILogger<SalesOrderFailedIntegrationEventHandler> logger)
        {
            _orderContext = orderContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(SalesOrderFailedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                Model.SalesOrder order = await _orderContext.SalesOrders.SingleOrDefaultAsync(order => order.Id == @event.SalesOrderId);

                if (order.Status == OrderStatus.PendingValidation)
                {
                    order.Status = OrderStatus.Failed;
                    _orderContext.SalesOrders.Update(order);
                    await _orderContext.SaveChangesAsync();
                }
            }
        }
    }
}