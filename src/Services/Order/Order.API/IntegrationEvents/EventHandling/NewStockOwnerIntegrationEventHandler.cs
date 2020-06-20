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

    public class NewStockOwnerIntegrationEventHandler : 
        IIntegrationEventHandler<NewStockOwnerIntegrationEvent>
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger<NewStockOwnerIntegrationEventHandler> _logger;

        public NewStockOwnerIntegrationEventHandler(
            OrderContext orderContext,
            ILogger<NewStockOwnerIntegrationEventHandler> logger)
        {
            _orderContext = orderContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(NewStockOwnerIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
                //if (await _orderContext.Stocks.Where(stock => stock.StockId == @event.StockId).CountAsync() == 0)
               // {
                    var newStock = new Stock()
                    {
                        Price = @event.Price,
                        StockId = @event.StockId
                    };
                    await _orderContext.Stocks.AddAsync(newStock);
                    await _orderContext.SaveChangesAsync();
               // }
            }
        }
    }
}
