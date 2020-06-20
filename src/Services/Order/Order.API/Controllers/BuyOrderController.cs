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
    public class BuyOrderController : ControllerBase
    {
        private readonly OrderContext _fundContext;
        private readonly OrderSettings _settings;
        private readonly IOrderIntegrationEventService _fundIntegrationEventService;

        public BuyOrderController(OrderContext context, IOptionsSnapshot<OrderSettings> settings, IOrderIntegrationEventService catalogIntegrationEventService)
        {
            _fundContext = context ?? throw new ArgumentNullException(nameof(context));
            _fundIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/BuyOrders
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<BuyOrder>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BuyOrdersAsync()
        {
            return Ok(await _fundContext.BuyOrders.ToListAsync());
        }


        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BuyOrder), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BuyOrder>> BuyOrderByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _fundContext.BuyOrders.SingleOrDefaultAsync(ci => ci.Id == id);


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
        [ProducesResponseType(typeof(BuyOrder), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BuyOrder>> UpdateBuyOrderAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _fundContext.BuyOrders.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item == null)
            {
                return NotFound();
            }

            if (item.Status == OrderStatus.Cancelled || item.Status == OrderStatus.Matched || item.Status == OrderStatus.Failed)
            {
                return BadRequest("Order is already in it's final state: " + item.Status);
            }

            item.Status = OrderStatus.Cancelled;
            var @event = new BuyOrderCancelledIntegrationEvent(item.Id);
            await _fundIntegrationEventService.SaveEventAndOrderContextChangesAsync(@event);
            await _fundIntegrationEventService.PublishThroughEventBusAsync(@event);

            return Ok(item);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BuyOrder), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BuyOrder>> CreateBuyOrderAsync(BuyOrder BuyOrder)
        {
            if (BuyOrder.SharesCount < 0)
            {
                return BadRequest("Shares must be positive");
            }

            //Find price - Static for now..
            decimal price = 10;

            var newBuyOrder = new BuyOrder()
            {
                Status = OrderStatus.PendingValidation,
                SharesCount = BuyOrder.SharesCount,
                StockId = BuyOrder.StockId,
                StockTraderId = BuyOrder.StockTraderId,
                PricePerShare = price
            };

            _fundContext.BuyOrders.Add(newBuyOrder);
            var @event = new BuyOrderPendingValidationIntegrationEvent(newBuyOrder.Id, newBuyOrder.StockTraderId, newBuyOrder.StockId, newBuyOrder.SharesCount, newBuyOrder.PricePerShare);
            await _fundIntegrationEventService.SaveEventAndOrderContextChangesAsync(@event);
            await _fundIntegrationEventService.PublishThroughEventBusAsync(@event);
            return Ok(newBuyOrder);
        }
    }
}
