

/*
namespace Microsoft.eShopOnContainers.Services.Template1.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;

    public class StockTraderCreatedIntegrationEventHandler : 
        IIntegrationEventHandler<StockTraderCreatedIntegrationEvent>
    {
        private readonly Template1Context _fundContext;
        private readonly ILogger<StockTraderCreatedIntegrationEventHandler> _logger;

        public StockTraderCreatedIntegrationEventHandler(
            Template1Context fundContext,
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

                await _fundContext.Template1s.AddAsync(new global::Template1.API.Model.Template1()
                {
                    Credit = 0,
                    StockTraderId = @event.StockTraderId
                });

                 await _fundContext.SaveChangesAsync();

            }
        }
    }
}
*/
