using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Model;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ShareOwner.API.IntegrationEvents;

namespace Microsoft.eShopOnContainers.Services.ShareOwner.API.IntegrationEvents.EventHandling
{
    public class SalesOrderPendingValidationIntegrationEventHandler :
        IIntegrationEventHandler<SalesOrderPendingValidationIntegrationEvent>
    {
        private readonly ShareOwnerContext _shareownerContext;
        private readonly ILogger<SalesOrderPendingValidationIntegrationEventHandler> _logger;
        private readonly IShareOwnerIntegrationEventService _shareownerIntegrationEventService;

        public SalesOrderPendingValidationIntegrationEventHandler(ShareOwnerContext shareownerContext, ILogger<SalesOrderPendingValidationIntegrationEventHandler> logger, IShareOwnerIntegrationEventService shareownerIntegrationEventService)
        {
            _shareownerContext = shareownerContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _shareownerIntegrationEventService = shareownerIntegrationEventService;
        }

        public async Task Handle(SalesOrderPendingValidationIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var shareOwner = await _shareownerContext.ShareOwners
                    .Include(owner => owner.Reservations)
                    .SingleOrDefaultAsync(ci => ci.StockTraderId == @event.StockTraderId && ci.StockId == @event.StockId);

                if (shareOwner == null)
                {
                    SalesOrderFailedValidationIntegrationEvent failedEvent = new SalesOrderFailedValidationIntegrationEvent(@event, "No shareowner found matching stockId and stockTraderid. You cannot sell what you do not own.");
                    await _shareownerIntegrationEventService.SaveEventAsync(failedEvent);
                    await _shareownerIntegrationEventService.PublishThroughEventBusAsync(failedEvent);
                    return;
                }

                var reserve = new Reservation()
                {
                    Reserved = @event.SharesCount,
                    ShareOwnerId = shareOwner.Id,
                    SalesOrderId = @event.SalesOrderId,
                    Status = ReservationStatus.Reserved
                };

                shareOwner.Reservations.Add(reserve);
                _shareownerContext.ShareOwners.Update(shareOwner);
                try
                {
                    SalesOrderValidatedIntegrationEvent evt = new SalesOrderValidatedIntegrationEvent(@event.SalesOrderId);
                    await _shareownerIntegrationEventService.SaveEventAndShareOwnerContextChangesAsync(evt);
                    await _shareownerIntegrationEventService.PublishThroughEventBusAsync(evt);
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Failed {message}", e.Message);

                    SalesOrderFailedValidationIntegrationEvent failedEvent = new SalesOrderFailedValidationIntegrationEvent(@event, "You cannot reserve more shares than you own.");
                    await _shareownerIntegrationEventService.SaveEventAsync(failedEvent);
                    await _shareownerIntegrationEventService.PublishThroughEventBusAsync(failedEvent);
                }
            }
        }
    }
}