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
        public async Task<ActionResult<Model.Stock>> UpdateAccountAsync([FromBody] Model.Stock stock)
        {
            var createStock = new Model.Stock()
            {
                Name = stock.Name,
                Price = stock.Price
            };

            _stockContext.Stocks.Update(createStock);
            var @event = new StockPriceChangedIntegrationEvent(createStock.Id, 0, createStock.Price);

            await _stockIntegrationEventService.SaveEventAndStockContextChangesAsync(@event);
            await _stockIntegrationEventService.PublishThroughEventBusAsync(@event);

            return CreatedAtAction(nameof(AccountByIdAsync), new { id = createStock.Id }, createStock);
        }


        /*
        // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Stock>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Stock>>> ItemsWithNameAsync(string name, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var totalItems = await _stockContext.Stocks
                .Where(c => c.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await _stockContext.Stocks
                .Where(c => c.Name.StartsWith(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<Stock>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/1/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Stock>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Stock>>> ItemsByTypeIdAndBrandIdAsync(int catalogTypeId, int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<Stock>)_stockContext.Stocks;

            root = root.Where(ci => ci.StockTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.StockBrandId == catalogBrandId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<Stock>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/type/all/brand[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/type/all/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Stock>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Stock>>> ItemsByBrandIdAsync(int? catalogBrandId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<Stock>)_stockContext.Stocks;

            if (catalogBrandId.HasValue)
            {
                root = root.Where(ci => ci.StockBrandId == catalogBrandId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<Stock>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/StockTypes
        [HttpGet]
        [Route("catalogtypes")]
        [ProducesResponseType(typeof(List<StockType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<StockType>>> StockTypesAsync()
        {
            return await _stockContext.StockTypes.ToListAsync();
        }

        // GET api/v1/[controller]/StockBrands
        [HttpGet]
        [Route("catalogbrands")]
        [ProducesResponseType(typeof(List<StockBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<StockBrand>>> StockBrandsAsync()
        {
            return await _stockContext.StockBrands.ToListAsync();
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody]Stock productToUpdate)
        {
            var catalogItem = await _stockContext.Stocks.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
            }

            var oldPrice = catalogItem.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            catalogItem = productToUpdate;
            _stockContext.Stocks.Update(catalogItem);

            if (raiseProductPriceChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            {
                //Create Integration Event to be published through the Event Bus
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, oldPrice);

                // Achieving atomicity between original Stock database operation and the IntegrationEventLog thanks to a local transaction
                await _stockIntegrationEventService.SaveEventAndStockContextChangesAsync(priceChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                await _stockIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }
            else // Just save the updated product because the Product's Price hasn't changed.
            {
                await _stockContext.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
        }

        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody]Stock product)
        {
            var item = new Stock
            {
                StockBrandId = product.StockBrandId,
                StockTypeId = product.StockTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };

            _stockContext.Stocks.Add(item);

            await _stockContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var product = _stockContext.Stocks.SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _stockContext.Stocks.Remove(product);

            await _stockContext.SaveChangesAsync();

            return NoContent();
        }

        private List<Stock> ChangeUriPlaceholder(List<Stock> items)
        {
            var baseUri = _settings.PicBaseUrl;
            var azureStorageEnabled = _settings.AzureStorageEnabled;

            foreach (var item in items)
            {
                item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);
            }

            return items;
        }
        */
    }
}
