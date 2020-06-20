using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Microsoft.eShopOnContainers.Services.Template1.API;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Template1.API.Infrastructure;

namespace Template1.API.IntegrationEvents
{
    public class Template1IntegrationEventService : ITemplate1IntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly Template1Context _catalogContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<Template1IntegrationEventService> _logger;

        public Template1IntegrationEventService(
            ILogger<Template1IntegrationEventService> logger,
            IEventBus eventBus,
            Template1Context catalogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_catalogContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndTemplate1ContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Template1IntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_catalogContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                await _catalogContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _catalogContext.Database.CurrentTransaction);
            });
        }
    }
}
