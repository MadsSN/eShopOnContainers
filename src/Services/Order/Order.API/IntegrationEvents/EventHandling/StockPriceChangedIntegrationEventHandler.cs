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

    public class StockPriceChangedIntegrationEventHandler : 
        IIntegrationEventHandler<StockPriceChangedIntegrationEvent>
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger<StockPriceChangedIntegrationEventHandler> _logger;

        public StockPriceChangedIntegrationEventHandler(
            OrderContext orderContext,
            ILogger<StockPriceChangedIntegrationEventHandler> logger)
        {
            _orderContext = orderContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(StockPriceChangedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var stock = await _orderContext.Stocks.SingleOrDefaultAsync(stock => stock.StockId == @event.StockId);
                stock.Price = @event.NewPrice;
                _orderContext.Stocks.Update(stock);
                await _orderContext.SaveChangesAsync();
            }
        }
    }
}
