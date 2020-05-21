using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;

namespace Microsoft.eShopOnContainers.Services.Fund.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;

    public class StockTraderCreatedIntegrationEventHandler : 
        IIntegrationEventHandler<StockTraderCreatedIntegrationEvent>
    {
        private readonly FundContext _fundContext;
        private readonly ILogger<StockTraderCreatedIntegrationEventHandler> _logger;

        public StockTraderCreatedIntegrationEventHandler(
            FundContext fundContext,
            ILogger<StockTraderCreatedIntegrationEventHandler> logger)
        {
            _fundContext = fundContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(StockTraderCreatedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                await _fundContext.Accounts.AddAsync(new Account()
                {
                    Credit = 0,
                    Id = @event.StockTraderId
                });

                 await _fundContext.SaveChangesAsync();

            }
        }
    }
}
