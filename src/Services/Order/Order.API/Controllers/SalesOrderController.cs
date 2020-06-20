using Order.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Order.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Order.API.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;

namespace Microsoft.eShopOnContainers.Services.Order.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly OrderContext _fundContext;
        private readonly OrderSettings _settings;
        private readonly IOrderIntegrationEventService _fundIntegrationEventService;

        public SalesOrderController(OrderContext context, IOptionsSnapshot<OrderSettings> settings, IOrderIntegrationEventService catalogIntegrationEventService)
        {
            _fundContext = context ?? throw new ArgumentNullException(nameof(context));
            _fundIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/SalesOrders
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<SalesOrder>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SalesOrdersAsync()
        {
            return Ok(await _fundContext.SalesOrders.ToListAsync());
        }


        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SalesOrder), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SalesOrder>> SalesOrderByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _fundContext.SalesOrders.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        [HttpPut]
        [Route("{id:int}/cancel")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SalesOrder), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SalesOrder>> UpdateSalesOrderAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _fundContext.SalesOrders.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item == null)
            {
                return NotFound();
            }

            if (item.Status == OrderStatus.Cancelled || item.Status == OrderStatus.Matched || item.Status == OrderStatus.Failed)
            {
                return BadRequest("Order is already in it's final state: " + item.Status);
            }

            item.Status = OrderStatus.Cancelled;
            var @event = new SalesOrderCancelledIntegrationEvent(item.Id);
            await _fundIntegrationEventService.SaveEventAndOrderContextChangesAsync(@event);
            await _fundIntegrationEventService.PublishThroughEventBusAsync(@event);

            return Ok(item);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(SalesOrder), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SalesOrder>> CreateSalesOrderAsync(SalesOrder salesOrder)
        {
            if (salesOrder.SharesCount < 0)
            {
                return BadRequest("Shares must be positive");
            }

            var newSalesOrder = new SalesOrder()
            {
                Status = OrderStatus.PendingValidation,
                SharesCount = salesOrder.SharesCount,
                StockId = salesOrder.StockId,
                StockTraderId = salesOrder.StockTraderId
            };

            _fundContext.SalesOrders.Add(newSalesOrder);
            var @event = new SalesOrderPendingValidationIntegrationEvent(newSalesOrder.Id, newSalesOrder.StockTraderId, newSalesOrder.StockId, newSalesOrder.SharesCount);
            await _fundIntegrationEventService.SaveEventAndOrderContextChangesAsync(@event);
            await _fundIntegrationEventService.PublishThroughEventBusAsync(@event);
            return Ok(newSalesOrder);
        }
    }
}
