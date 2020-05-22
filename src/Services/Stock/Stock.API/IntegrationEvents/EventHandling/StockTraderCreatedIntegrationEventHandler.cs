using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.Stock.API.Model;

namespace Microsoft.eShopOnContainers.Services.Stock.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;

    public class StockTraderCreatedIntegrationEventHandler : 
        IIntegrationEventHandler<StockTraderCreatedIntegrationEvent>
    {
        private readonly StockContext _stockContext;
        private readonly ILogger<StockTraderCreatedIntegrationEventHandler> _logger;

        public StockTraderCreatedIntegrationEventHandler(
            StockContext stockContext,
            ILogger<StockTraderCreatedIntegrationEventHandler> logger)
        {
            _stockContext = stockContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(StockTraderCreatedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

             //   await _stockContext.Stocks.AddAsync(new Model.Stock()
             //   {
              //      Credit = 0,
              //      StockTraderId = @event.StockTraderId
               // });

               //  await _stockContext.SaveChangesAsync();

            }
        }
    }
}
