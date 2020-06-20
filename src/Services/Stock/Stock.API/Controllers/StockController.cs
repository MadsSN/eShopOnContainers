using Stock.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Stock.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Stock.API.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.Events;
using Stock.API.Request;

namespace Microsoft.eShopOnContainers.Services.Stock.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockContext _stockContext;
        private readonly StockSettings _settings;
        private readonly IStockIntegrationEventService _stockIntegrationEventService;

        public StockController(StockContext context, IOptionsSnapshot<StockSettings> settings, IStockIntegrationEventService catalogIntegrationEventService)
        {
            _stockContext = context ?? throw new ArgumentNullException(nameof(context));
            _stockIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/accounts
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<Model.Stock>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AccountsAsync()
        {
            return Ok(await _stockContext.Stocks.ToListAsync());
        }


        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Model.Stock), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Model.Stock>> AccountByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _stockContext.Stocks.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Model.Stock), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Model.Stock>> UpdateAccountAsync(int id, [FromBody] Model.Stock stock)
        {
            if (id <= 0)
            {
                return BadRequest();
            }


            var item = await _stockContext.Stocks.SingleOrDefaultAsync(ci => ci.Id == id);


            if (item == null)
            {
                return NotFound();
            }

            var @event = new StockPriceChangedIntegrationEvent(item.Id, item.Price, stock.Price);

            item.Price = stock.Price;
            item.Name = stock.Name;
            _stockContext.Stocks.Update(item);


            await _stockIntegrationEventService.SaveEventAndStockContextChangesAsync(@event);
            await _stockIntegrationEventService.PublishThroughEventBusAsync(@event);

            return Ok(item);
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Model.Stock), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Model.Stock>> UpdateAccountAsync([FromBody] StockCreateRequest request)
        {
            //Maybe in time exchange this to a Grpc request.
            var trader =
                _stockContext.StockTraders.SingleOrDefault(trader => trader.StockTraderId == request.StockTraderId);
            if (trader == null)
            {
                return BadRequest();
            }

            var createStock = new Model.Stock()
            {
                Name = request.Name,
                Price = request.Price,
                TotalShares = request.Shares
            };

            _stockContext.Stocks.Update(createStock);
            var @event = new NewStockOwnerIntegrationEvent(createStock.Id, request.Shares, request.StockTraderId, request.Price);

            await _stockIntegrationEventService.SaveEventAndStockContextChangesAsync(@event);
            await _stockIntegrationEventService.PublishThroughEventBusAsync(@event);

            return CreatedAtAction(nameof(AccountByIdAsync), new { id = createStock.Id }, createStock);
        }
    }
}
