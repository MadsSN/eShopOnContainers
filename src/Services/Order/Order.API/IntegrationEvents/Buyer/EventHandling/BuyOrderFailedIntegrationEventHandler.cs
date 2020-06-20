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
    public class BuyOrderFailedIntegrationEventHandler :
        IIntegrationEventHandler<BuyOrderFailedIntegrationEvent>
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger<BuyOrderFailedIntegrationEventHandler> _logger;

        public BuyOrderFailedIntegrationEventHandler(
            OrderContext orderContext,
            ILogger<BuyOrderFailedIntegrationEventHandler> logger)
        {
            _orderContext = orderContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(BuyOrderFailedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                BuyOrder order = await _orderContext.BuyOrders.SingleOrDefaultAsync(order => order.Id == @event.BuyOrderId);

                if (order.Status == OrderStatus.PendingValidation)
                {
                    order.Status = OrderStatus.Failed;
                    _orderContext.BuyOrders.Update(order);
                    await _orderContext.SaveChangesAsync();
                }
            }
        }
    }
}