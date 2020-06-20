using System.Collections.Generic;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Model;

namespace Microsoft.eShopOnContainers.Services.ShareOwner.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;

    public class NewStockOwnerIntegrationEventHandler : 
        IIntegrationEventHandler<NewStockOwnerIntegrationEvent>
    {
        private readonly ShareOwnerContext _shareownerContext;
        private readonly ILogger<NewStockOwnerIntegrationEventHandler> _logger;

        public NewStockOwnerIntegrationEventHandler(
            ShareOwnerContext shareownerContext,
            ILogger<NewStockOwnerIntegrationEventHandler> logger)
        {
            _shareownerContext = shareownerContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(NewStockOwnerIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                await _shareownerContext.ShareOwners.AddAsync(new Model.ShareOwner()
                {
                    StockId = @event.StockId,
                    StockTraderId = @event.StockTraderId,
                    Shares = @event.Shares,
                    Reservations = new List<Reservation>()
                });

                 await _shareownerContext.SaveChangesAsync();

            }
        }
    }
}
